#!/usr/bin/env python3
"""
NBA Player Points Prop Model — Training & Scoring
-------------------------------------------------
Reads Postgres table `model.points_training_set`, trains a regression model to predict
player points, estimates per-player residual variance with shrinkage, and outputs
Over/Under probabilities for provided prop lines.

Usage examples:
  # Train & evaluate (prints metrics)
  python nba_points_props_model.py --pg-url postgresql+psycopg2://user:pass@host:5432/db

  # Train, then score a CSV of props and write results
  python nba_points_props_model.py \
      --pg-url postgresql+psycopg2://user:pass@host:5432/db \
      --props-csv props_to_score.csv \
      --out-csv scored_props.csv \
      --line-col line  --player-col player_id --game-col game_id

Props CSV expected columns (customizable via args):
  - player_id
  - game_id (optional; used for grouping/reporting; model does not require for prediction)
  - line (points line to score)
"""

import os
import argparse
from datetime import datetime
import numpy as np
import pandas as pd

from sqlalchemy import create_engine

from sklearn.ensemble import GradientBoostingRegressor
from sklearn.metrics import mean_absolute_error, r2_score
from scipy.stats import norm

#############################
# 1) Arguments & DB engine  #
#############################

def parse_args():
    ap = argparse.ArgumentParser(description="Train & score NBA player points prop model.")
    ap.add_argument("--pg-url", default=os.getenv("PG_URL", "postgresql+psycopg2://postgres:redsox45@localhost:5432/hoop_scoop"),
                    help="SQLAlchemy Postgres URL (or set PG_URL env var)")
    ap.add_argument("--valid-quantile", type=float, default=0.85,
                    help="Fractional date quantile used as validation split (default 0.85 == last 15%% as valid)")
    ap.add_argument("--estimators", type=int, default=600, help="GBR n_estimators")
    ap.add_argument("--lr", type=float, default=0.03, help="GBR learning_rate")
    ap.add_argument("--max-depth", type=int, default=3, help="GBR max_depth")
    ap.add_argument("--subsample", type=float, default=0.9, help="GBR subsample fraction")

    # Scoring inputs/outputs
    ap.add_argument("--props-csv", default=None, help="CSV with props to score (columns: player_id, [game_id], line)")
    ap.add_argument("--out-csv", default="scored_props.csv", help="Where to write scored props CSV")

    # Custom column names for the props CSV
    ap.add_argument("--player-col", default="player_id", help="Column name for player id in props CSV")
    ap.add_argument("--game-col", default="game_id", help="Column name for game id in props CSV (optional)")
    ap.add_argument("--line-col", default="line", help="Column name for prop line in props CSV")

    return ap.parse_args()


def make_engine(pg_url: str):
    return create_engine(pg_url)

from sqlalchemy import text

def run_sql_file(engine, path: str):
    """
    Execute a .sql file (DDL pipeline) before training.
    Assumes the file contains regular SQL (no psql backslash commands).
    """
    with open(path, "r", encoding="utf-8") as f:
        sql = f.read()

    # Use a transaction; exec_driver_sql lets us run multiple statements.
    with engine.begin() as conn:
        conn.exec_driver_sql(sql)
#############################################
# 2) Load training data from materialized MV#
#############################################

TRAIN_SQL = """
SELECT
  game_id, player_id, game_date, pts,
  is_home::int AS is_home,
  pace_model,
  p_pts_l5, p_pts_l10, p_pts_sd_l10,
  p_min_l5, p_fga_l5, p_fta_l5, p_3pa_l5
FROM model.points_training_set
WHERE pts IS NOT NULL
ORDER BY game_date, game_id, player_id;
"""

FEATURES = [
    "is_home", "pace_model",
    "p_pts_l5","p_pts_l10","p_pts_sd_l10",
    "p_min_l5","p_fga_l5","p_fta_l5","p_3pa_l5"
]

def load_training(engine):
    df = pd.read_sql(TRAIN_SQL, engine)
    # Ensure proper dtypes
    df["game_date"] = pd.to_datetime(df["game_date"])
    # Handle early-season NaNs: forward fill by player, then zeros as last resort
    df.sort_values(["player_id","game_date","game_id"], inplace=True)
    for col in FEATURES:
        #fills with last value from player
        df[col] = df.groupby("player_id")[col].ffill()
    #fills with 0 for first game. should change to fill with average from previous season
    df[FEATURES] = df[FEATURES].fillna(0.0)
    return df


##########################################
# 3) Train / evaluate baseline regressor #
##########################################

def split_time(df, valid_quantile=0.85):
    cutoff = df["game_date"].quantile(valid_quantile)
    train = df[df["game_date"] < cutoff].copy()
    valid = df[df["game_date"] >= cutoff].copy()
    return train, valid, cutoff

def train_model(train_df, args):
    X_tr = train_df[FEATURES].values
    y_tr = train_df["pts"].values
    mdl = GradientBoostingRegressor(
        n_estimators=args.estimators,
        learning_rate=args.lr,
        max_depth=args.max_depth,
        subsample=args.subsample,
        random_state=42
    )
    mdl.fit(X_tr, y_tr)
    return mdl

def evaluate_model(mdl, train_df, valid_df):
    X_tr, y_tr = train_df[FEATURES].values, train_df["pts"].values
    X_va, y_va = valid_df[FEATURES].values, valid_df["pts"].values
    pred_tr = mdl.predict(X_tr)
    pred_va = mdl.predict(X_va)
    metrics = {
        "train_mae": float(mean_absolute_error(y_tr, pred_tr)),
        "train_r2": float(r2_score(y_tr, pred_tr)),
        "valid_mae": float(mean_absolute_error(y_va, pred_va)),
        "valid_r2": float(r2_score(y_va, pred_va)),
    }
    return pred_tr, pred_va, metrics


####################################################
# 4) Estimate residual sigma per player (shrinkage)#
####################################################

def estimate_player_sigma(valid_df, pred_va, shrink_k=20):
    res = valid_df.copy()
    res["pred"] = pred_va
    res["err"] = res["pts"] - res["pred"]

    global_sigma = float(res["err"].std(ddof=1)) if len(res) > 1 else 7.5  # safe default

    per_player = res.groupby("player_id")["err"].std(ddof=1).to_frame("sigma_player")
    per_player["sigma_player"] = per_player["sigma_player"].fillna(global_sigma)

    counts = res.groupby("player_id").size().to_frame("n")
    merged = per_player.join(counts, how="left").fillna({"n":0})
    # Shrink toward global by appearances (n): sigma_hat = sqrt((n*s_p^2 + k*g^2)/(n+k))
    merged["sigma_hat"] = np.sqrt((merged["n"] * (merged["sigma_player"]**2) + shrink_k * (global_sigma**2)) / (merged["n"] + shrink_k))
    player_sigma = merged["sigma_hat"].to_dict()
    return player_sigma, global_sigma


########################################
# 5) Scoring helpers / prop probability#
########################################

def normal_over_under_probs(mean, sigma, line):
    if sigma <= 1e-6:
        p_over = float(mean > line)
    else:
        p_over = 1.0 - norm.cdf((line - mean) / sigma)
    return p_over, 1.0 - p_over

def build_mean_predictions(mdl, df):
    preds = mdl.predict(df[FEATURES].values)
    out = df[["game_id","player_id","game_date"]].copy()
    out["pred_mean"] = preds
    return out

def attach_sigmas(pred_frame, player_sigma, global_sigma):
    pred_frame["sigma"] = pred_frame["player_id"].map(player_sigma).fillna(global_sigma)
    return pred_frame


########################################
# 6) Optional: score a props CSV        #
########################################

def score_props_csv(mdl, full_df, player_sigma, global_sigma, csv_path, out_csv, player_col, game_col, line_col):
    props = pd.read_csv(csv_path, dtype={player_col: str, game_col: str})
    # Normalize expected column names
    if player_col not in props.columns:
        raise ValueError(f"Props CSV missing column '{player_col}'")
    if line_col not in props.columns:
        raise ValueError(f"Props CSV missing column '{line_col}'")

    print(props)
    # 🔹 Coerce keys to a common type (string) to avoid int64 vs object merge issues
    props[player_col] = props[player_col].astype(str)
    pred_base = full_df.copy()
    #print(pred_base)
    pred_base["player_id"] = pred_base["player_id"].astype(str)

    if game_col in props.columns:
        print("INSIDE GAME_COL IF STATEMENT$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$")
        props[game_col] = props[game_col].astype(str)
        pred_base["game_id"] = pred_base["game_id"].astype(str)
    # For each prop row, we need the latest row for that player (by date) to get features.
    # If game_id is provided, we can pick that exact game row.
    # pred_base = full_df.copy()

    if game_col in props.columns:
        # Merge on (player_id, game_id)
        merged = props.merge(
            pred_base,
            left_on=[player_col, game_col],
            right_on=["player_id","game_id"],
            how="left",
            suffixes=("","")
        )
    else:
        # Take the most recent row per player
        pred_base["rnk"] = pred_base.groupby("player_id")["game_date"].rank(method="first", ascending=False)
        latest = pred_base[pred_base["rnk"] == 1].drop(columns=["rnk"])
        merged = props.merge(
            latest,
            left_on=player_col,
            right_on="player_id",
            how="left"
        )

    # If some rows don't have features (e.g., never-seen player), drop them
    merged = merged.dropna(subset=FEATURES)

    # Predict mean
    merged["pred_mean"] = mdl.predict(merged[FEATURES].values)

    # Sigma per player
    merged["sigma"] = merged["player_id"].map(player_sigma).fillna(global_sigma)

    # Probabilities
    p_over, p_under = [], []
    for m, s, line in zip(merged["pred_mean"], merged[line_col], merged[line_col]):
        po, pu = normal_over_under_probs(m, s, line)
        p_over.append(po); p_under.append(pu)
    merged["p_over"] = p_over
    merged["p_under"] = p_under
    merged["edge_over"] = merged["p_over"] - 0.5
    merged["edge_under"] = merged["p_under"] - 0.5

    # Select friendly output columns
    keep_cols = []
    if game_col in props.columns:
        keep_cols = [game_col]
    keep_cols += [player_col, line_col, "pred_mean", "sigma", "p_over", "p_under", "edge_over", "edge_under"]
    out = merged[keep_cols].copy()
    out.sort_values(["p_over"], ascending=False, inplace=True)
    out.to_csv(out_csv, index=False)
    return out


########################
# 7) Main entry point  #
########################

def main():
    args = parse_args()
    engine = make_engine(args.pg_url)

    # 1) Rebuild / refresh the SQL feature pipeline
    #    This will recreate model.points_training_set based on the current *_all views.
    print("Rebuilding SQL pipeline (materialized views)...")
    run_sql_file(engine, r"C:\Users\jackp\Desktop\ReactApp4\SQL\nba_pts_model.sql")

    # Load & split
    df = load_training(engine)
    train_df, valid_df, cutoff = split_time(df, args.valid_quantile)

    # Train & evaluate
    mdl = train_model(train_df, args)
    pred_tr, pred_va, metrics = evaluate_model(mdl, train_df, valid_df)

    print("=== Time split cutoff:", cutoff.date(), "===")
    print("TRAIN  MAE={:.3f}  R2={:.3f}".format(metrics["train_mae"], metrics["train_r2"]))
    print("VALID  MAE={:.3f}  R2={:.3f}".format(metrics["valid_mae"], metrics["valid_r2"]))

    # Build residual sigma model on validation set
    player_sigma, global_sigma = estimate_player_sigma(valid_df, pred_va, shrink_k=20)
    print("Global residual sigma (valid): {:.3f}".format(global_sigma))
    print("Players with sigma estimates:", len(player_sigma))

    # Optionally score props CSV
    if args.props_csv:
        scored = score_props_csv(
            mdl, df, player_sigma, global_sigma,
            csv_path=args.props_csv,
            out_csv=args.out_csv,
            player_col=args.player_col,
            game_col=args.game_col,
            line_col=args.line_col
        )
        print(f"Wrote scored props to: {args.out_csv}")
        # Show a small preview
        print(scored.head(12).to_string(index=False))

if __name__ == "__main__":
    main()
