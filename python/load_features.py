# pip install sqlalchemy psycopg2-binary pandas scikit-learn scipy
import os
import pandas as pd
from sqlalchemy import create_engine, text

import numpy as np
from sklearn.ensemble import GradientBoostingRegressor
from sklearn.model_selection import train_test_split
from sklearn.isotonic import IsotonicRegression
from scipy.stats import norm


# 1) Keep secrets in env vars
PG_USER = os.getenv("PGUSER", "postgres")
PG_PASS = os.getenv("PGPASSWORD", "secret")
PG_HOST = os.getenv("PGHOST", "localhost")
PG_PORT = os.getenv("PGPORT", "5432")
PG_DB   = os.getenv("PGDATABASE", "sports")

engine = create_engine(f"postgresql+psycopg2://{PG_USER}:{PG_PASS}@{PG_HOST}:{PG_PORT}/{PG_DB}",
                       pool_pre_ping=True, pool_size=5, max_overflow=5)

# 2) (Optional) trigger refreshes from Python
with engine.begin() as conn:
    conn.execute(text("REFRESH MATERIALIZED VIEW CONCURRENTLY nba_points_target;"))
    conn.execute(text("REFRESH MATERIALIZED VIEW CONCURRENTLY feat_player_roll;"))
    conn.execute(text("REFRESH MATERIALIZED VIEW CONCURRENTLY feat_game_context;"))
    conn.execute(text("REFRESH MATERIALIZED VIEW CONCURRENTLY nba_points_features;"))

# 3) Load features to pandas
df = pd.read_sql("SELECT * FROM nba_points_features;", engine)

############################################################################################
# train

# ----- prepare data -----
# Suppose you also have a table with historical prop lines (for validation):
# odds_props(game_id, player_id, market='PLAYER_POINTS', line, over_odds, under_odds)

props = pd.read_sql("""
SELECT p.game_id, p.player_id, p.line, p.over_odds, p.under_odds
FROM odds_props p
WHERE p.market = 'PLAYER_POINTS';
""", engine)

data = (df
        .merge(props, on=["game_id","player_id"], how="left")
        .dropna(subset=["pts"]))  # ensure targets present

feature_cols = [
    "p_pts_l5","p_pts_l10","p_pts_sd_l10",
    "p_min_l5","p_fga_l5","p_fta_l5","p_3pa_l5",
    "pace_est","spread","total"
]
# Basic NA handling
data[feature_cols] = data[feature_cols].fillna(0.0)

# Time-based split (example: use 'season' to split)
train = data[data["season"] <= 2022]
valid = data[data["season"] == 2023]

X_tr, y_tr = train[feature_cols], train["pts"].values
X_va, y_va = valid[feature_cols], valid["pts"].values
line_va    = valid["line"].values  # for validation only

def make_q(alpha):
    return GradientBoostingRegressor(loss="quantile", alpha=alpha,
                                     n_estimators=800, learning_rate=0.03,
                                     max_depth=3, subsample=0.9, random_state=42)

m_q10 = make_q(0.10).fit(X_tr, y_tr)
m_q50 = make_q(0.50).fit(X_tr, y_tr)
m_q90 = make_q(0.90).fit(X_tr, y_tr)

q10 = m_q10.predict(X_va)
q50 = m_q50.predict(X_va)
q90 = m_q90.predict(X_va)

def prob_over_from_quants(q10, q50, q90, line):
    sigma = np.maximum(1e-6, (q90 - q10) / (2*1.2816))
    mu = q50
    z = (line - mu)/sigma
    return 1 - norm.cdf(z)

p_over_raw = prob_over_from_quants(q10, q50, q90, line_va)

# Probability calibration: map p_over_raw to reality
y_over_bin = (y_va > line_va).astype(int)
iso = IsotonicRegression(out_of_bounds="clip")
p_over_cal = iso.fit(p_over_raw, y_over_bin).predict(p_over_raw)

#####################################################################################
# compare and make picks

def american_to_prob(odds):
    return (100/(odds+100)) if odds>0 else ((-odds)/(100-odds))

def no_vig_two_way(p_over_imp, p_under_imp):
    s = p_over_imp + p_under_imp
    return p_over_imp/s, p_under_imp/s

valid["p_over_raw"] = p_over_raw
valid["p_over_cal"] = p_over_cal

# Compute fair (no vig) prob from book odds
imp_over  = valid["over_odds"].map(american_to_prob)
imp_under = valid["under_odds"].map(american_to_prob)
fair_over, fair_under = no_vig_two_way(imp_over.values, imp_under.values)

valid["fair_over"] = fair_over
valid["edge_over"] = valid["p_over_cal"] - valid["fair_over"]

# Keep only positive edges above a threshold
picks = valid.sort_values("edge_over", ascending=False)
picks = picks[picks["edge_over"] > 0.02]  # 2% edge threshold
picks[["game_id","player_id","line","p_over_cal","fair_over","edge_over"]].head(20)

#########################################################################################
# game day scoring pipe line

slate = pd.read_sql("""
SELECT f.*, p.line, p.over_odds, p.under_odds
FROM nba_points_features f
JOIN odds_props p
  ON p.game_id = f.game_id
 AND p.player_id = f.player_id
WHERE p.market = 'PLAYER_POINTS'
  AND f.game_date = CURRENT_DATE;  -- adjust for timezone
""", engine)

X = slate[feature_cols].fillna(0.0)
q10 = m_q10.predict(X)
q50 = m_q50.predict(X)
q90 = m_q90.predict(X)

p_over = prob_over_from_quants(q10, q50, q90, slate["line"].values)
p_over = iso.predict(p_over)  # calibrated

imp_over  = slate["over_odds"].map(american_to_prob)
imp_under = slate["under_odds"].map(american_to_prob)
fair_over, _ = no_vig_two_way(imp_over.values, imp_under.values)
edge = p_over - fair_over

slate["p_over"] = p_over
slate["fair_over"] = fair_over
slate["edge_over"] = edge

# Persist or export
slate.to_csv("nba_points_slate.csv", index=False)
# or write back:
# slate[["game_id","player_id","line","p_over","fair_over","edge_over"]].to_sql("model_points_signals",
#     engine, if_exists="append", index=False)
