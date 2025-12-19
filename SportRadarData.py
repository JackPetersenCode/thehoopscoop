import json
import pandas as pd
import requests


def get_play_by_play(season):
    url = "https://api.sportradar.com/mlb/trial/v8/en/games/002a3ffc-80a4-4148-b6ab-85111e42338b/pbp.json"
    headers = {
        "accept": "application/json",
        "x-api-key": "KlWwaPeATnCTPeXsw8YByZI0mJxb4akox07vLInv",
    }

    resp = requests.get(url, headers=headers, timeout=20)
    resp.raise_for_status()  # raises if non-2xx
    data = resp.json()
    game = data["game"]

    ##play by play
    # --- Game Info ---
    venue = game.get("venue", {})
    venue_loc = venue.get("location", {})

    game_info = {
        "game_id": game.get("id"),
        "status": game.get("status"),
        "coverage": game.get("coverage"),
        "game_number": game.get("game_number"),
        "day_night": game.get("day_night"),
        "scheduled": game.get("scheduled"),
        "home_team": game.get("home_team"),
        "away_team": game.get("away_team"),
        "attendance": game.get("attendance"),
        "duration": game.get("duration"),
        "season_id": game.get("season_id"),
        "season_type": game.get("season_type"),
        "season_year": game.get("season_year"),
        "double_header": game.get("double_header"),
        "entry_mode": game.get("entry_mode"),
        "reference": game.get("reference"),
        "venue_name": venue.get("name"),
        "venue_market": venue.get("market"),
        "venue_capacity": venue.get("capacity"),
        "venue_surface": venue.get("surface"),
        "venue_address": venue.get("address"),
        "venue_city": venue.get("city"),
        "venue_state": venue.get("state"),
        "venue_zip": venue.get("zip"),
        "venue_country": venue.get("country"),
        "venue_id": venue.get("id"),
        "venue_field_orientation": venue.get("field_orientation"),
        "venue_stadium_type": venue.get("stadium_type"),
        "venue_time_zone": venue.get("time_zone"),
        "venue_latitude": venue_loc.get("lat"),
        "venue_longitude": venue_loc.get("lng"),
    }

    pd.DataFrame([game_info]).to_csv(f"mlb_stats/sportradar_pbp_game_info.csv", index=False)

    # --- Broadcasts ---
    broadcasts = pd.DataFrame(game.get("broadcasts", []))
    broadcasts.insert(0, "game_id", game.get("id"))  # add as first column
    broadcasts.to_csv(f"mlb_stats/sportradar_pbp_broadcasts.csv", index=False)

    # --- Scoring ---
    scoring = []
    for side in ["home", "away"]:
        row = {"game_id": game.get("id"), "side": side}
        row.update((game.get("scoring") or {}).get(side, {}))
        scoring.append(row)
    pd.DataFrame(scoring).to_csv(f"mlb_stats/sportradar_pbp_scoring.csv", index=False)


    # --- Innings ---
    innings = []
    halfs = []
    lineups = []
    at_bats = []
    pitches = []
    fielders_rows = []
    runners_rows = []
    runner_fielders_rows = []  # fielders credited on runner outs (nested under runners)

    for inning in game.get("innings", []):
        innings.append({"number": inning.get("number"), 
                        "sequence": inning.get("sequence"),
                        "game_id": game.get("id")})

        for half in inning.get("halfs", []):
            halfs.append({"inning": inning.get("number"), 
                          "half": half.get("half"),
                          "game_id": game.get("id")})

            # Lineups
            for ev in half.get("events", []):
                if "lineup" in ev:
                    lineup = ev["lineup"].copy()
                    lineup["game_id"] = game.get("id")
                    lineup["inning"] = inning.get("number")
                    lineup["half"] = half.get("half")
                    lineups.append(lineup)

                if "at_bat" in ev:
                    at_bat = {}
                    ab = ev.get("at_bat").copy()
                    at_bat["game_id"] = game.get("id")
                    ab_id = ab.get("id")
                    at_bat["inning"] = inning.get("number")
                    at_bat["half"] = half.get("half")
                    at_bat["hitter_id"] = ab.get("hitter_id")
                    at_bat["at_bat_id"] = ab.get("id")
                    at_bat["hitter_hand"] = ab.get("hitter_hand")
                    at_bat["pitcher_id"] = ab.get("pitcher_id")
                    at_bat["pitcher_hand"] = ab.get("pitcher_hand")
                    at_bat["sequence_number"] = ab.get("sequence_number")
                    at_bat["description"] = ab.get("description")
                    at_bat["hitter_preferred_name"] = ab.get("hitter").get("preferred_name")
                    at_bat["hitter_first_name"] = ab.get("hitter").get("first_name")
                    at_bat["hitter_last_name"] = ab.get("hitter").get("last_name")
                    at_bat["hitter_jersey_number"] = ab.get("hitter").get("jersey_number")
                    at_bat["hitter_id"] = ab.get("hitter").get("id")
                    at_bat["hitter_full_name"] = ab.get("hitter").get("full_name")
                    at_bat["pitcher_preferred_name"] = ab.get("pitcher").get("preferred_name")
                    at_bat["pitcher_first_name"] = ab.get("pitcher").get("first_name")
                    at_bat["pitcher_last_name"] = ab.get("pitcher").get("last_name")
                    at_bat["pitcher_jersey_number"] = ab.get("pitcher").get("jersey_number")
                    at_bat["pitcher_id"] = ab.get("pitcher").get("id")
                    at_bat["pitcher_full_name"] = ab.get("pitcher").get("full_name")
                    at_bat["home_team_runs"] = ab.get("score").get("home_team_runs")
                    at_bat["away_team_runs"] = ab.get("score").get("away_team_runs")

                    at_bats.append(at_bat)

                    # pitches inside each at_bat
                    for pitch in ab.get("events", []):
                        event = {}
                        pitch_copy = pitch.copy()
                        flags = pitch_copy.get("flags", {})
                        count = pitch_copy.get("count", {})
                        pitcher = pitch_copy.get("pitcher", {})
                        hitter = pitch_copy.get("hitter", {})
                        score = pitch_copy.get("score", {})
                        mlb_pitch_data = pitch_copy.get("mlb_pitch_data", {})
                        mlb_hit_data = pitch_copy.get("mlb_hit_data", {})
                        event["game_id"] = game.get("id")
                        event["at_bat_id"] = ab_id
                        event["inning"] = inning.get("number")
                        event["half"] = half.get("half")
                        event["hit_location"] = pitch_copy.get("hit_location")
                        event["hit_type"] = pitch_copy.get("hit_type")
                        event["status"] = pitch_copy.get("status")
                        event["event_id"] = pitch_copy.get("id")
                        event["outcome_id"] = pitch_copy.get("outcome_id")
                        event["created_at"] = pitch_copy.get("created_at")
                        event["updated_at"] = pitch_copy.get("updated_at")
                        event["sequence_number"] = pitch_copy.get("sequence_number")
                        event["official"] = pitch_copy.get("official")
                        event["type"] = pitch_copy.get("type")
                        event["wall_clock_start_time"] = pitch.get("wall_clock", {}).get("start_time")
                        event["wall_clock_end_time"] = pitch.get("wall_clock", {}).get("end_time")
                        event["is_ab_over"] = flags.get("is_ab_over")
                        event["is_bunt"] = flags.get("is_bunt")
                        event["is_hit"] = flags.get("is_hit")
                        event["is_wild_pitch"] = flags.get("is_wild_pitch")
                        event["is_passed_ball"] = flags.get("is_passed_ball")
                        event["is_double_play"] = flags.get("is_double_play")
                        event["is_triple_play"] = flags.get("is_triple_play")
                        event["balls"] = count.get("balls")
                        event["strikes"] = count.get("strikes")
                        event["outs"] = count.get("outs")
                        event["pitch_count"] = count.get("pitch_count")
                        event["pitch_type"] = pitcher.get("pitch_type")
                        event["pitch_speed"] = pitcher.get("pitch_speed")
                        event["pitch_zone"] = pitcher.get("pitch_zone")
                        event["pitcher_hand"] = pitcher.get("pitcher_hand")
                        event["hitter_hand"] = pitcher.get("hitter_hand")
                        event["pitch_count"] = pitcher.get("pitch_count")
                        event["pitcher_id"] = pitcher.get("id")
                        event["pitch_x"] = pitcher.get("pitch_x")
                        event["pitch_y"] = pitcher.get("pitch_y")
                        event["pitcher_preferred_name"] = pitcher.get("preferred_name")
                        event["pitcher_first_name"] = pitcher.get("first_name")
                        event["pitcher_last_name"] = pitcher.get("last_name")
                        event["pitcher_jersey_number"] = pitcher.get("jersey_number")
                        event["pitcher_full_name"] = pitcher.get("full_name")
                        event["hitter_preferred_name"] = hitter.get("preferred_name")
                        event["hitter_first_name"] = hitter.get("first_name")
                        event["hitter_last_name"] = hitter.get("last_name")
                        event["hitter_jersey_number"] = hitter.get("jersey_number")
                        event["hitter_full_name"] = hitter.get("full_name")
                        event["hitter_id"] = hitter.get("id")
                        event["home_team_runs"] = score.get("home_team_runs")
                        event["away_team_runs"] = score.get("away_team_runs")
                        event["mlb_pitch_speed"] = mlb_pitch_data.get("speed")
                        event["mlb_strike_zone_top"] = mlb_pitch_data.get("strike_zone_top")
                        event["mlb_strike_zone_bottom"] = mlb_pitch_data.get("strike_zone_bottom")
                        event["mlb_pitch_zone"] = mlb_pitch_data.get("zone")
                        event["mlb_pitch_code"] = mlb_pitch_data.get("code")
                        event["mlb_pitch_description"] = mlb_pitch_data.get("description")
                        event["mlb_pitch_x"] = mlb_pitch_data.get("coordinates", {}).get("x")
                        event["mlb_pitch_y"] = mlb_pitch_data.get("coordinates", {}).get("y")
                        event["mlb_hit_trajectory"] = mlb_hit_data.get("trajectory")
                        event["mlb_hit_hardness"] = mlb_hit_data.get("hardness")
                        event["mlb_hit_x"] = mlb_hit_data.get("coordinates", {}).get("coord_x")
                        event["mlb_hit_y"] = mlb_hit_data.get("coordinates", {}).get("coord_y")

                        pitches.append(event)

                        # --- Fielders on the play (top-level under the pitch/event) ---
                        for f in (pitch_copy.get("fielders") or []):
                            fielders_rows.append({
                                "game_id": game.get("id"),
                                "inning": inning.get("number"),
                                "half": half.get("half"),
                                "at_bat_id": ab_id,
                                "event_id": pitch_copy.get("id"),
                                "sequence": f.get("sequence"),
                                "type": f.get("type"),  # e.g., putout/assist
                                "fielder_id": f.get("id"),
                                "preferred_name": f.get("preferred_name"),
                                "first_name": f.get("first_name"),
                                "last_name": f.get("last_name"),
                                "jersey_number": f.get("jersey_number"),
                                "full_name": f.get("full_name"),
                            })
                            # --- Runners involved in the play ---
                            for r in (pitch_copy.get("runners") or []):
                                runners_rows.append({
                                    "game_id": game.get("id"),
                                    "inning": inning.get("number"),
                                    "half": half.get("half"),
                                    "at_bat_id": ab_id,
                                    "event_id": pitch_copy.get("id"),
                                    "starting_base": r.get("starting_base"),
                                    "ending_base": r.get("ending_base"),
                                    "out": r.get("out"),
                                    "outcome_id": r.get("outcome_id"),
                                    "description": r.get("description"),
                                    "runner_id": r.get("id"),
                                    "preferred_name": r.get("preferred_name"),
                                    "first_name": r.get("first_name"),
                                    "last_name": r.get("last_name"),
                                    "suffix": r.get("suffix"),
                                    "jersey_number": r.get("jersey_number"),
                                    "full_name": r.get("full_name"),
                                })

                                # Fielders credited specifically on this runner (nested under the runner)
                                for rf in (r.get("fielders") or []):
                                    runner_fielders_rows.append({
                                        "game_id": game.get("id"),
                                        "inning": inning.get("number"),
                                        "half": half.get("half"),
                                        "at_bat_id": ab_id,
                                        "event_id": pitch_copy.get("id"),
                                        "runner_id": r.get("id"),
                                        "sequence": rf.get("sequence"),
                                        "type": rf.get("type"),
                                        "fielder_id": rf.get("id"),
                                        "preferred_name": rf.get("preferred_name"),
                                        "first_name": rf.get("first_name"),
                                        "last_name": rf.get("last_name"),
                                        "jersey_number": rf.get("jersey_number"),
                                        "full_name": rf.get("full_name"),
                                    })


    pd.DataFrame(innings).to_csv(f"mlb_stats/sportradar_pbp_innings_{season}.csv", index=False)
    pd.DataFrame(halfs).to_csv(f"mlb_stats/sportradar_pbp_halfs_{season}.csv", index=False)
    pd.DataFrame(lineups).to_csv(f"mlb_stats/sportradar_pbp_lineups_{season}.csv", index=False)
    pd.DataFrame(at_bats).to_csv(f"mlb_stats/sportradar_pbp_at_bats_{season}.csv", index=False)
    pd.DataFrame(pitches).to_csv(f"mlb_stats/sportradar_pbp_pitches_{season}.csv", index=False)
    pd.DataFrame(fielders_rows).to_csv(f"mlb_stats/sportradar_pbp_fielders_{season}.csv", index=False)
    pd.DataFrame(runners_rows).to_csv(f"mlb_stats/sportradar_pbp_runners_{season}.csv", index=False)
    pd.DataFrame(runner_fielders_rows).to_csv(f"mlb_stats/sportradar_pbp_runner_fielders_{season}.csv", index=False)
    print("CSV files created successfully!")


def flatten_dict(d, parent_key="", sep="_"):
    d = d or {}
    out = {}
    for k, v in d.items():
        nk = f"{parent_key}{sep}{k}" if parent_key else str(k)
        if isinstance(v, dict):
            out.update(flatten_dict(v, nk, sep=sep))
        else:
            out[nk] = v
    return out


def get_game_extended_summary(season):

    game_info_rows = []
    broadcast_rows = []
    weather_rows = []
    team_totals_rows = []
    scoring_rows = []
    lineup_rows = []
    roster_rows = []
    probable_pitcher_rows = []
    starting_pitcher_rows = []
    team_hitting_rows = []
    team_pitching_rows = []
    team_pitching_starters_rows = []
    team_pitching_bullpen_rows = []
    team_fielding_rows = []
    new_games = ["booger"]
    for new_game in new_games:
        url = "https://api.sportradar.com/mlb/trial/v8/en/games/002a3ffc-80a4-4148-b6ab-85111e42338b/extended_summary.json"
        headers = {
            "accept": "application/json",
            "x-api-key": "KlWwaPeATnCTPeXsw8YByZI0mJxb4akox07vLInv",
        }

        resp = requests.get(url, headers=headers, timeout=20)
        resp.raise_for_status()
        data = resp.json()

        game = data.get("game", {}) or {}

        # --- Game info (unchanged from your version, using .get) ---
        game_info = {
            "game_id": game.get("id"),
            "status": game.get("status"),
            "coverage": game.get("coverage"),
            "game_number": game.get("game_number"),
            "day_night": game.get("day_night"),
            "scheduled": game.get("scheduled"),
            "home_team_id": game.get("home_team"),
            "away_team_id": game.get("away_team"),
            "attendance": game.get("attendance"),
            "duration": game.get("duration"),
            "season_id": game.get("season_id"),
            "season_type": game.get("season_type"),
            "season_year": game.get("season_year"),
            "double_header": game.get("double_header"),
            "entry_mode": game.get("entry_mode"),
            "reference": game.get("reference"),
            "time_zones_venue": (game.get("time_zones") or {}).get("venue"),
            "time_zones_home": (game.get("time_zones") or {}).get("home"),
            "time_zones_away": (game.get("time_zones") or {}).get("away"),
            "venue_name": (game.get("venue") or {}).get("name"),
            "venue_market": (game.get("venue") or {}).get("market"),
            "venue_capacity": (game.get("venue") or {}).get("capacity"),
            "venue_surface": (game.get("venue") or {}).get("surface"),
            "venue_address": (game.get("venue") or {}).get("address"),
            "venue_city": (game.get("venue") or {}).get("city"),
            "venue_state": (game.get("venue") or {}).get("state"),
            "venue_zip": (game.get("venue") or {}).get("zip"),
            "venue_country": (game.get("venue") or {}).get("country"),
            "venue_id": (game.get("venue") or {}).get("id"),
            "venue_field_orientation": (game.get("venue") or {}).get("field_orientation"),
            "venue_stadium_type": (game.get("venue") or {}).get("stadium_type"),
            "venue_time_zone": (game.get("venue") or {}).get("time_zone"),
            "venue_location_lat": ((game.get("venue") or {}).get("location") or {}).get("lat"),
            "venue_location_lng": ((game.get("venue") or {}).get("location") or {}).get("lng"),
        }

        game_info_rows.append(game_info)

        # --- Broadcasts ---
        for b in game.get("broadcasts", []) or []:
            broadcast_rows.append({
                "game_id": game.get("id"),
                "network": b.get("network"),
                "type": b.get("type"),
                "locale": b.get("locale"),
                "channel": b.get("channel"),
            })

        # --- Weather ---
        w = (game.get("weather") or {})
        for kind in ("forecast", "current_conditions"):
            block = (w.get(kind) or {})
            if not block:
                continue
            wind = (block.get("wind") or {})
            weather_rows.append({
                "game_id": game.get("id"),
                "kind": kind,  # "forecast" or "current_conditions"
                "temp_f": block.get("temp_f"),
                "condition": block.get("condition"),
                "humidity": block.get("humidity"),
                "dew_point_f": block.get("dew_point_f"),
                "cloud_cover": block.get("cloud_cover"),
                "obs_time": block.get("obs_time"),
                "wind_speed_mph": wind.get("speed_mph"),
                "wind_direction": wind.get("direction"),
            })
        player_hitting_season_to_date_rows = []
        player_hitting_overall_rows = []
        player_fielding_season_to_date_rows = []
        player_fielding_overall_rows = []
        player_pitching_season_to_date_rows = []
        player_pitching_overall_rows = []
        player_pitching_bullpen_rows = []
        player_pitching_starters_rows = []
        for side in ["home", "away"]:
            team = (game.get(side) or {})
            game_id = game.get("id")
            team_id = team.get("id")
            team_abbr = team.get("abbr")

            # Team summary
            team_totals_rows.append({
                "game_id": game_id, "side": side,
                "team_id": team_id, "team_abbr": team_abbr,
                "team_market": team.get("market"),
                "team_name": team.get("name"),
                "runs": team.get("runs"),
                "hits": team.get("hits"),
                "errors": team.get("errors"),
                "win": team.get("win"),
                "loss": team.get("loss"),
            })

            # Probable pitcher
            prob = (team.get("probable_pitcher") or {})
            probable_pitcher_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                "player_id": prob.get("id"),
                "preferred_name": prob.get("preferred_name"),
                "first_name": prob.get("first_name"),
                "last_name": prob.get("last_name"),
                "full_name": prob.get("full_name"),
                "jersey_number": prob.get("jersey_number"),
                "win": prob.get("win"),
                "loss": prob.get("loss"),
                "era": prob.get("era"),
            })

            # Starting pitcher
            sp = (team.get("starting_pitcher") or {})
            starting_pitcher_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                "player_id": sp.get("id"),
                "preferred_name": sp.get("preferred_name"),
                "first_name": sp.get("first_name"),
                "last_name": sp.get("last_name"),
                "full_name": sp.get("full_name"),
                "jersey_number": sp.get("jersey_number"),
                "win": sp.get("win"),
                "loss": sp.get("loss"),
                "era": sp.get("era"),
            })

            # Roster
            for p in (team.get("roster") or []):
                roster_rows.append({
                    "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "player_id": p.get("id"),
                    "preferred_name": p.get("preferred_name"),
                    "first_name": p.get("first_name"),
                    "last_name": p.get("last_name"),
                    "full_name": p.get("full_name"),
                    "jersey_number": p.get("jersey_number"),
                    "status": p.get("status"),
                    "position": p.get("position"),
                    "primary_position": p.get("primary_position"),
                    "suffix": p.get("suffix"),
                })

            # Lineup
            for l in (team.get("lineup") or []):
                lineup_rows.append({
                    "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "player_id": l.get("id"),
                    "inning": l.get("inning"),
                    "order": l.get("order"),
                    "position": l.get("position"),
                    "sequence": l.get("sequence"),
                    "inning_half": l.get("inning_half"),
                })

            # Scoring
            for s in (team.get("scoring") or []):
                scoring_rows.append({
                    "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "number": s.get("number"),
                    "sequence": s.get("sequence"),
                    "runs": s.get("runs"),
                    "hits": s.get("hits"),
                    "errors": s.get("errors"),
                    "type": s.get("type"),
                })

            # Stats (flattened)
            stats = (team.get("statistics") or {})

            hit_overall = flatten_dict((stats.get("hitting") or {}).get("overall"))
            team_hitting_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                **hit_overall
            })

            pit_overall = flatten_dict((stats.get("pitching") or {}).get("overall"))
            team_pitching_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                **pit_overall
            })

            pit_starters = flatten_dict((stats.get("pitching") or {}).get("starters"))
            team_pitching_starters_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                **pit_starters
            })

            pit_bullpen = flatten_dict((stats.get("pitching") or {}).get("bullpen"))
            team_pitching_bullpen_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                **pit_bullpen
            })

            fld_overall = flatten_dict((stats.get("fielding") or {}).get("overall"))
            team_fielding_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                **fld_overall
            })

            players = (team.get("players") or {})
            for player in players:
                hitting_td = player.get("statistics", {}).get("hitting", {}).get("season_td", {})
                hitting_td_onbase = hitting_td.get("onbase", {})
                hitting_td_outcome = hitting_td.get("outcome", {})
                hitting_td_outs = hitting_td.get("outs", {})
                hitting_td_steal = hitting_td.get("steal", {})
                hitting_td_pitches = hitting_td.get("pitches", {})
                hitting_td_games = hitting_td.get("games", {})
                player_hitting_season_to_date_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "ab": hitting_td.get("ab"),
                    "lob": hitting_td.get("lob"),
                    "rbi": hitting_td.get("rbi"),
                    "abhr": hitting_td.get("abhr"),
                    "abk": hitting_td.get("abk"),
                    "bip": hitting_td.get("bip"),
                    "babip": hitting_td.get("babip"),
                    "bbk": hitting_td.get("bbk"),
                    "bbpa": hitting_td.get("bbpa"),
                    "iso": hitting_td.get("iso"),
                    "obp": hitting_td.get("obp"),
                    "ops": hitting_td.get("ops"),
                    "seca": hitting_td.get("seca"),
                    "slg": hitting_td.get("slg"),
                    "xbh": hitting_td.get("xbh"),
                    "pitch_count": hitting_td.get("pitch_count"),
                    "lob_risp_2out": hitting_td.get("lob_risp_2out"),
                    "team_lob": hitting_td.get("team_lob"),
                    "ab_risp": hitting_td.get("ab_risp"),
                    "hit_risp": hitting_td.get("hit_risp"),
                    "rbi_2out": hitting_td.get("rbi_2out"),
                    "linedrive": hitting_td.get("linedrive"),
                    "groundball": hitting_td.get("groundball"),
                    "popup": hitting_td.get("popup"),
                    "flyball": hitting_td.get("flyball"),
                    "ap": hitting_td.get("ap"),
                    "avg": hitting_td.get("avg"),
                    "gofo": hitting_td.get("gofo"),
                    "s": hitting_td_onbase.get("s"),
                    "d": hitting_td_onbase.get("d"),
                    "t": hitting_td_onbase.get("t"),
                    "hr": hitting_td_onbase.get("hr"),
                    "tb": hitting_td_onbase.get("tb"),
                    "bb": hitting_td_onbase.get("bb"),
                    "ibb": hitting_td_onbase.get("ibb"),
                    "hbp": hitting_td_onbase.get("hbp"),
                    "fc": hitting_td_onbase.get("fc"),
                    "roe": hitting_td_onbase.get("roe"),
                    "h": hitting_td_onbase.get("h"),
                    "ci": hitting_td_onbase.get("ci"),
                    "rov": hitting_td_onbase.get("rov"),
                    "cycle": hitting_td_onbase.get("cycle"),
                    "runs_total": hitting_td.get("runs", {}).get("total"),
                    "outcome_klook": hitting_td_outcome.get("klook"),
                    "outcome_kswing": hitting_td_outcome.get("kswing"),
                    "outcome_ktotal": hitting_td_outcome.get("ktotal"),
                    "outcome_ball": hitting_td_outcome.get("ball"),
                    "outcome_iball": hitting_td_outcome.get("iball"),
                    "outcome_dirtball": hitting_td_outcome.get("dirtball"),
                    "outcome_foul": hitting_td_outcome.get("foul"),
                    "outs_po": hitting_td_outs.get("po"),
                    "outs_fo": hitting_td_outs.get("fo"),
                    "outs_fidp": hitting_td_outs.get("fidp"),
                    "outs_lo": hitting_td_outs.get("lo"),
                    "outs_lidp": hitting_td_outs.get("lidp"),
                    "outs_go": hitting_td_outs.get("go"),
                    "outs_gidp": hitting_td_outs.get("gidp"),
                    "outs_klook": hitting_td_outs.get("klook"),
                    "outs_kswing": hitting_td_outs.get("kswing"),
                    "outs_ktotal": hitting_td_outs.get("ktotal"),
                    "outs_sacfly": hitting_td_outs.get("sacfly"),
                    "outs_sachit": hitting_td_outs.get("sachit"),
                    "caught": hitting_td_steal.get("caught"),
                    "stolen": hitting_td_steal.get("stolen"),
                    "pct": hitting_td_steal.get("pct"),
                    "pickoff": hitting_td_steal.get("pickoff"),
                    "pitches_count": hitting_td_pitches.get("count"),
                    "pitches_btotal": hitting_td_pitches.get("btotal"),
                    "pitches_ktotal": hitting_td_pitches.get("ktotal"),
                    "games_start": hitting_td_games.get("start"),
                    "games_play": hitting_td_games.get("play"),
                    "games_finish": hitting_td_games.get("finish"),
                    "games_complete": hitting_td_games.get("complete")
                })

                hitting_overall = player.get("statistics", {}).get("hitting", {}).get("overall", {})
                hitting_overall_onbase = hitting_overall.get("onbase", {})
                hitting_overall_outcome = hitting_overall.get("outcome", {})
                hitting_overall_outs = hitting_overall.get("outs", {})
                hitting_overall_steal = hitting_overall.get("steal", {})
                hitting_overall_pitches = hitting_overall.get("pitches", {})
                hitting_overall_games = hitting_overall.get("games", {})
                player_hitting_overall_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "ab": hitting_overall.get("ab"),
                    "lob": hitting_overall.get("lob"),
                    "rbi": hitting_overall.get("rbi"),
                    "abhr": hitting_overall.get("abhr"),
                    "abk": hitting_overall.get("abk"),
                    "bip": hitting_overall.get("bip"),
                    "babip": hitting_overall.get("babip"),
                    "bbk": hitting_overall.get("bbk"),
                    "bbpa": hitting_overall.get("bbpa"),
                    "iso": hitting_overall.get("iso"),
                    "obp": hitting_overall.get("obp"),
                    "ops": hitting_overall.get("ops"),
                    "seca": hitting_overall.get("seca"),
                    "slg": hitting_overall.get("slg"),
                    "xbh": hitting_overall.get("xbh"),
                    "pitch_count": hitting_overall.get("pitch_count"),
                    "lob_risp_2out": hitting_overall.get("lob_risp_2out"),
                    "team_lob": hitting_overall.get("team_lob"),
                    "ab_risp": hitting_overall.get("ab_risp"),
                    "hit_risp": hitting_overall.get("hit_risp"),
                    "rbi_2out": hitting_overall.get("rbi_2out"),
                    "linedrive": hitting_overall.get("linedrive"),
                    "groundball": hitting_overall.get("groundball"),
                    "popup": hitting_overall.get("popup"),
                    "flyball": hitting_overall.get("flyball"),
                    "ap": hitting_overall.get("ap"),
                    "avg": hitting_overall.get("avg"),
                    "gofo": hitting_overall.get("gofo"),
                    "s": hitting_overall_onbase.get("s"),
                    "d": hitting_overall_onbase.get("d"),
                    "t": hitting_overall_onbase.get("t"),
                    "hr": hitting_overall_onbase.get("hr"),
                    "tb": hitting_overall_onbase.get("tb"),
                    "bb": hitting_overall_onbase.get("bb"),
                    "ibb": hitting_overall_onbase.get("ibb"),
                    "hbp": hitting_overall_onbase.get("hbp"),
                    "fc": hitting_overall_onbase.get("fc"),
                    "roe": hitting_overall_onbase.get("roe"),
                    "h": hitting_overall_onbase.get("h"),
                    "ci": hitting_overall_onbase.get("ci"),
                    "rov": hitting_overall_onbase.get("rov"),
                    "cycle": hitting_overall_onbase.get("cycle"),
                    "total_runs": hitting_overall.get("runs", {}).get("total"),
                    "outcome_klook": hitting_overall_outcome.get("klook"),
                    "outcome_kswing": hitting_overall_outcome.get("kswing"),
                    "outcome_ktotal": hitting_overall_outcome.get("ktotal"),
                    "outcome_ball": hitting_overall_outcome.get("ball"),
                    "outcome_iball": hitting_overall_outcome.get("iball"),
                    "outcome_dirtball": hitting_overall_outcome.get("dirtball"),
                    "outcome_foul": hitting_overall_outcome.get("foul"),
                    "outs_po": hitting_overall_outs.get("po"),
                    "outs_fo": hitting_overall_outs.get("fo"),
                    "outs_fidp": hitting_overall_outs.get("fidp"),
                    "outs_lo": hitting_overall_outs.get("lo"),
                    "outs_lidp": hitting_overall_outs.get("lidp"),
                    "outs_go": hitting_overall_outs.get("go"),
                    "outs_gidp": hitting_overall_outs.get("gidp"),
                    "outs_klook": hitting_overall_outs.get("klook"),
                    "outs_kswing": hitting_overall_outs.get("kswing"),
                    "outs_ktotal": hitting_overall_outs.get("ktotal"),
                    "outs_sacfly": hitting_overall_outs.get("sacfly"),
                    "outs_sachit": hitting_overall_outs.get("sachit"),
                    "caught": hitting_overall_steal.get("caught"),
                    "stolen": hitting_overall_steal.get("stolen"),
                    "pct": hitting_overall_steal.get("pct"),
                    "pickoff": hitting_overall_steal.get("pickoff"),
                    "pitches_count": hitting_overall_pitches.get("count"),
                    "pitches_btotal": hitting_overall_pitches.get("btotal"),
                    "pitches_ktotal": hitting_overall_pitches.get("ktotal"),
                    "games_start": hitting_overall_games.get("start"),
                    "games_play": hitting_overall_games.get("play"),
                    "games_finish": hitting_overall_games.get("finish"),
                    "games_complete": hitting_overall_games.get("complete")
                })

                fielding_td = player.get("statistics", {}).get("fielding", {}).get("season_td", {})
                fielding_td_steal = fielding_td.get("steal", {})
                fielding_td_errors = fielding_td.get("errors", {})
                fielding_td_assists = fielding_td.get("assists", {})
                fielding_td_games = fielding_td.get("games", {})
                player_fielding_season_to_date_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "po": fielding_td.get("po"),
                    "a": fielding_td.get("a"),
                    "dp": fielding_td.get("dp"),
                    "tp": fielding_td.get("tp"),
                    "error": fielding_td.get("error"),
                    "tc": fielding_td.get("tc"),
                    "fpct": fielding_td.get("fpct"),
                    "c_wp": fielding_td.get("c_wp"),
                    "pb": fielding_td.get("pb"),
                    "rf": fielding_td.get("rf"),
                    "inn_1": fielding_td.get("inn_1"),
                    "inn_2": fielding_td.get("inn_2"),
                    "caught": fielding_td_steal.get("caught"),
                    "stolen": fielding_td_steal.get("stolen"),
                    "pickoff": fielding_td_steal.get("pickoff"),
                    "pct": fielding_td_steal.get("pct"),
                    "errors_throwing": fielding_td_errors.get("throwing"),
                    "errors_fielding": fielding_td_errors.get("fielding"),
                    "errors_interference": fielding_td_errors.get("interference"),
                    "errors_total": fielding_td_errors.get("total"),
                    "assists_outfield": fielding_td_assists.get("outfield"),
                    "assists_total": fielding_td_assists.get("total"),
                    "games_start": fielding_td_games.get("start"),
                    "games_play": fielding_td_games.get("play"),
                    "games_finish": fielding_td_games.get("finish"),
                    "games_complete": fielding_td_games.get("complete")
                })
                fielding_overall = player.get("statistics", {}).get("fielding", {}).get("overall", {})
                fielding_overall_steal = fielding_overall.get("steal", {})
                fielding_overall_errors = fielding_overall.get("errors", {})
                fielding_overall_assists = fielding_overall.get("assists", {})
                fielding_overall_games = fielding_overall.get("games", {})
                player_fielding_overall_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "po": fielding_overall.get("po"),
                    "a": fielding_overall.get("a"),
                    "dp": fielding_overall.get("dp"),
                    "tp": fielding_overall.get("tp"),
                    "error": fielding_overall.get("error"),
                    "tc": fielding_overall.get("tc"),
                    "fpct": fielding_overall.get("fpct"),
                    "c_wp": fielding_overall.get("c_wp"),
                    "pb": fielding_overall.get("pb"),
                    "rf": fielding_overall.get("rf"),
                    "inn_1": fielding_overall.get("inn_1"),
                    "inn_2": fielding_overall.get("inn_2"),
                    "caught": fielding_overall_steal.get("caught"),
                    "stolen": fielding_overall_steal.get("stolen"),
                    "pickoff": fielding_overall_steal.get("pickoff"),
                    "pct": fielding_overall_steal.get("pct"),
                    "errors_throwing": fielding_overall_errors.get("throwing"),
                    "errors_fielding": fielding_overall_errors.get("fielding"),
                    "errors_interference": fielding_overall_errors.get("interference"),
                    "errors_total": fielding_overall_errors.get("total"),
                    "assists_outfield": fielding_overall_assists.get("outfield"),
                    "assists_total": fielding_overall_assists.get("total"),
                    "games_start": fielding_overall_games.get("start"),
                    "games_play": fielding_overall_games.get("play"),
                    "games_finish": fielding_overall_games.get("finish"),
                    "games_complete": fielding_overall_games.get("complete")
                })

                fielding_positions = player.get("statistics", {}).get("fielding", {}).get("positions", [])
                player_fielding_positions_rows = []
                for position in fielding_positions:
                    position_steal = position.get("steal")
                    position_errors = position.get("errors")
                    position_assists = position.get("assists")
                    position_games = position.get("games")
                    player_fielding_positions_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                        "po": position.get("po"),
                        "a": position.get("a"),
                        "dp": position.get("dp"),
                        "tp": position.get("tp"),
                        "error": position.get("error"),
                        "tc": position.get("tc"),
                        "fpct": position.get("fpct"),
                        "c_wp": position.get("c_wp"),
                        "pb": position.get("pb"),
                        "rf": position.get("rf"),
                        "inn_1": position.get("inn_1"),
                        "inn_2": position.get("inn_2"),
                        "caught": position_steal.get("caught"),
                        "stolen": position_steal.get("stolen"),
                        "pickoff": position_steal.get("pickoff"),
                        "pct": position_steal.get("pct"),
                        "errors_throwing": position_errors.get("throwing"),
                        "errors_fielding": position_errors.get("fielding"),
                        "errors_interference": position_errors.get("interference"),
                        "errors_total": position_errors.get("total"),
                        "assists_outfield": position_assists.get("outfield"),
                        "assists_total": position_assists.get("total"),
                        "games_start": position_games.get("start"),
                        "games_play": position_games.get("play"),
                        "games_finish": position_games.get("finish"),
                        "games_complete": position_games.get("complete")
                    })
                pitching_td = player.get("statistics", {}).get("pitching", {}).get("seasontd", {})
                pitching_td_onbase = pitching_td.get("onbase", {})
                pitching_td_runs = pitching_td.get("runs", {})
                pitching_td_outcome = pitching_td.get("outcome", {})
                pitching_td_outs = pitching_td.get("outs", {})
                pitching_td_steal = pitching_td.get("steal", {})
                pitching_td_pitches = pitching_td.get("pitches", {})
                pitching_td_in_play = pitching_td.get("in_play", {})
                pitching_td_games = pitching_td.get("games", {})
                player_pitching_season_to_date_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "oba": pitching_td.get("oba"),
                    "lob": pitching_td.get("lob"),
                    "era": pitching_td.get("era"),
                    "k9": pitching_td.get("k9"),
                    "whip": pitching_td.get("whip"),
                    "kbb": pitching_td.get("kbb"),
                    "pitch_count": pitching_td.get("pitch_count"),
                    "wp": pitching_td.get("wp"),
                    "bk": pitching_td.get("bk"),
                    "ip_1": pitching_td.get("ip_1"),
                    "ip_2": pitching_td.get("ip_2"),
                    "bf": pitching_td.get("bf"),
                    "gofo": pitching_td.get("gofo"),
                    "babip": pitching_td.get("babip"),
                    "bf_ip": pitching_td.get("bf_ip"),
                    "gbfb": pitching_td.get("gbfb"),
                    "oab": pitching_td.get("oab"),
                    "slg": pitching_td.get("slg"),
                    "obp": pitching_td.get("obp"),
                    "onbase_s": pitching_td_onbase.get("s"),
                    "onbase_d": pitching_td_onbase.get("d"),
                    "onbase_t": pitching_td_onbase.get("t"),
                    "onbase_hr": pitching_td_onbase.get("hr"),
                    "onbase_tb": pitching_td_onbase.get("tb"),
                    "onbase_bb": pitching_td_onbase.get("bb"),
                    "onbase_ibb": pitching_td_onbase.get("ibb"),
                    "onbase_hbp": pitching_td_onbase.get("hbp"),
                    "onbase_fc": pitching_td_onbase.get("fc"),
                    "onbase_roe": pitching_td_onbase.get("roe"),
                    "onbase_h": pitching_td_onbase.get("h"),
                    "onbase_ci": pitching_td_onbase.get("ci"),
                    "onbase_rov": pitching_td_onbase.get("rov"),
                    "onbase_h9": pitching_td_onbase.get("h9"),
                    "onbase_hr9": pitching_td_onbase.get("hr9"),
                    "runs_total": pitching_td_runs.get("total"),
                    "runs_unearned": pitching_td_runs.get("unearned"),
                    "runs_earned": pitching_td_runs.get("earned"),
                    "runs_ir": pitching_td_runs.get("ir"),
                    "runs_ira": pitching_td_runs.get("ira"),
                    "runs_bqr": pitching_td_runs.get("bqr"),
                    "runs_bqra": pitching_td_runs.get("bqra"),
                    # outcome
                    "outcome_klook": pitching_td_outcome.get("klook"),
                    "outcome_kswing": pitching_td_outcome.get("kswing"),
                    "outcome_ktotal": pitching_td_outcome.get("ktotal"),
                    "outcome_ball": pitching_td_outcome.get("ball"),
                    "outcome_iball": pitching_td_outcome.get("iball"),
                    "outcome_dirtball": pitching_td_outcome.get("dirtball"),
                    "outcome_foul":     pitching_td_outcome.get("foul"),

                    # outs
                    "outs_po": pitching_td_outs.get("po"),
                    "outs_fo": pitching_td_outs.get("fo"),
                    "outs_fidp": pitching_td_outs.get("fidp"),
                    "outs_lo": pitching_td_outs.get("lo"),
                    "outs_lidp": pitching_td_outs.get("lidp"),
                    "outs_go": pitching_td_outs.get("go"),
                    "outs_gidp": pitching_td_outs.get("gidp"),
                    "outs_klook": pitching_td_outs.get("klook"),
                    "outs_kswing": pitching_td_outs.get("kswing"),
                    "outs_ktotal": pitching_td_outs.get("ktotal"),
                    "outs_sacfly": pitching_td_outs.get("sacfly"),
                    "outs_sachit": pitching_td_outs.get("sachit"),

                    # steal
                    "steal_caught": pitching_td_steal.get("caught"),
                    "steal_stolen": pitching_td_steal.get("stolen"),
                    "steal_pickoff": pitching_td_steal.get("pickoff"),

                    # pitches
                    "pitches_count": pitching_td_pitches.get("count"),
                    "pitches_btotal": pitching_td_pitches.get("btotal"),
                    "pitches_ktotal": pitching_td_pitches.get("ktotal"),
                    "pitches_per_ip": pitching_td_pitches.get("per_ip"),
                    "pitches_per_bf": pitching_td_pitches.get("per_bf"),

                    # in_play
                    "in_play_linedrive": pitching_td_in_play.get("linedrive"),
                    "in_play_groundball": pitching_td_in_play.get("groundball"),
                    "in_play_popup": pitching_td_in_play.get("popup"),
                    "in_play_flyball": pitching_td_in_play.get("flyball"),

                    # games
                    "games_start": pitching_td_games.get("start"),
                    "games_play": pitching_td_games.get("play"),
                    "games_finish": pitching_td_games.get("finish"),
                    "games_svo": pitching_td_games.get("svo"),
                    "games_qstart": pitching_td_games.get("qstart"),
                    "games_shutout": pitching_td_games.get("shutout"),
                    "games_complete": pitching_td_games.get("complete"),
                    "games_win": pitching_td_games.get("win"),
                    "games_loss": pitching_td_games.get("loss"),
                    "games_save": pitching_td_games.get("save"),
                    "games_hold": pitching_td_games.get("hold"),
                    "games_blown_save": pitching_td_games.get("blown_save"),
                    "games_team_win": pitching_td_games.get("team_win"),
                    "games_team_loss": pitching_td_games.get("team_loss"),
                })

                pitching_overall = player.get("statistics", {}).get("pitching", {}).get("overall", {})
                pitching_overall_onbase = pitching_overall.get("onbase", {})
                pitching_overall_runs = pitching_overall.get("runs", {})
                pitching_overall_outcome = pitching_overall.get("outcome", {})
                pitching_overall_outs = pitching_overall.get("outs", {})
                pitching_overall_steal = pitching_overall.get("steal", {})
                pitching_overall_pitches = pitching_overall.get("pitches", {})
                pitching_overall_in_play = pitching_overall.get("in_play", {})
                pitching_overall_games = pitching_overall.get("games", {})
                player_pitching_overall_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "oba": pitching_overall.get("oba"),
                    "lob": pitching_overall.get("lob"),
                    "era": pitching_overall.get("era"),
                    "k9": pitching_overall.get("k9"),
                    "whip": pitching_overall.get("whip"),
                    "kbb": pitching_overall.get("kbb"),
                    "pitch_count": pitching_overall.get("pitch_count"),
                    "wp": pitching_overall.get("wp"),
                    "bk": pitching_overall.get("bk"),
                    "ip_1": pitching_overall.get("ip_1"),
                    "ip_2": pitching_overall.get("ip_2"),
                    "bf": pitching_overall.get("bf"),
                    "gofo": pitching_overall.get("gofo"),
                    "babip": pitching_overall.get("babip"),
                    "bf_ip": pitching_overall.get("bf_ip"),
                    "gbfb": pitching_overall.get("gbfb"),
                    "oab": pitching_overall.get("oab"),
                    "slg": pitching_overall.get("slg"),
                    "obp": pitching_overall.get("obp"),
                    "times_through_order": pitching_overall.get("times_through_order"),
                    "onbase_s": pitching_overall_onbase.get("s"),
                    "onbase_d": pitching_overall_onbase.get("d"),
                    "onbase_t": pitching_overall_onbase.get("t"),
                    "onbase_hr": pitching_overall_onbase.get("hr"),
                    "onbase_tb": pitching_overall_onbase.get("tb"),
                    "onbase_bb": pitching_overall_onbase.get("bb"),
                    "onbase_ibb": pitching_overall_onbase.get("ibb"),
                    "onbase_hbp": pitching_overall_onbase.get("hbp"),
                    "onbase_fc": pitching_overall_onbase.get("fc"),
                    "onbase_roe": pitching_overall_onbase.get("roe"),
                    "onbase_h": pitching_overall_onbase.get("h"),
                    "onbase_ci": pitching_overall_onbase.get("ci"),
                    "onbase_rov": pitching_overall_onbase.get("rov"),
                    "onbase_h9": pitching_overall_onbase.get("h9"),
                    "onbase_hr9": pitching_overall_onbase.get("hr9"),

                    "runs_total": pitching_overall_runs.get("total"),
                    "runs_unearned": pitching_overall_runs.get("unearned"),
                    "runs_earned": pitching_overall_runs.get("earned"),
                    "runs_ir": pitching_overall_runs.get("ir"),
                    "runs_ira": pitching_overall_runs.get("ira"),
                    "runs_bqr": pitching_overall_runs.get("bqr"),
                    "runs_bqra": pitching_overall_runs.get("bqra"),

                    "outcome_klook": pitching_overall_outcome.get("klook"),
                    "outcome_kswing": pitching_overall_outcome.get("kswing"),
                    "outcome_ktotal": pitching_overall_outcome.get("ktotal"),
                    "outcome_ball": pitching_overall_outcome.get("ball"),
                    "outcome_iball": pitching_overall_outcome.get("iball"),
                    "outcome_dirtball": pitching_overall_outcome.get("dirtball"),
                    "outcome_foul": pitching_overall_outcome.get("foul"),

                    "outs_po": pitching_overall_outs.get("po"),
                    "outs_fo": pitching_overall_outs.get("fo"),
                    "outs_fidp": pitching_overall_outs.get("fidp"),
                    "outs_lo": pitching_overall_outs.get("lo"),
                    "outs_lidp": pitching_overall_outs.get("lidp"),
                    "outs_go": pitching_overall_outs.get("go"),
                    "outs_gidp": pitching_overall_outs.get("gidp"),
                    "outs_klook": pitching_overall_outs.get("klook"),
                    "outs_kswing": pitching_overall_outs.get("kswing"),
                    "outs_ktotal": pitching_overall_outs.get("ktotal"),
                    "outs_sacfly": pitching_overall_outs.get("sacfly"),
                    "outs_sachit": pitching_overall_outs.get("sachit"),

                    "steal_caught": pitching_overall_steal.get("caught"),
                    "steal_stolen": pitching_overall_steal.get("stolen"),
                    "steal_pickoff": pitching_overall_steal.get("pickoff"),

                    "pitches_count": pitching_overall_pitches.get("count"),
                    "pitches_btotal": pitching_overall_pitches.get("btotal"),
                    "pitches_ktotal": pitching_overall_pitches.get("ktotal"),
                    "pitches_per_ip": pitching_overall_pitches.get("per_ip"),
                    "pitches_per_bf": pitching_overall_pitches.get("per_bf"),

                    "in_play_linedrive": pitching_overall_in_play.get("linedrive"),
                    "in_play_groundball": pitching_overall_in_play.get("groundball"),
                    "in_play_popup": pitching_overall_in_play.get("popup"),
                    "in_play_flyball": pitching_overall_in_play.get("flyball"),

                    "games_start": pitching_overall_games.get("start"),
                    "games_play": pitching_overall_games.get("play"),
                    "games_finish": pitching_overall_games.get("finish"),
                    "games_svo": pitching_overall_games.get("svo"),
                    "games_qstart": pitching_overall_games.get("qstart"),
                    "games_shutout": pitching_overall_games.get("shutout"),
                    "games_complete": pitching_overall_games.get("complete"),
                    "games_win": pitching_overall_games.get("win"),
                    "games_loss": pitching_overall_games.get("loss"),
                    "games_save": pitching_overall_games.get("save"),
                    "games_hold": pitching_overall_games.get("hold"),
                    "games_blown_save": pitching_overall_games.get("blown_save"),
                    "games_team_win": pitching_overall_games.get("team_win"),
                    "games_team_loss": pitching_overall_games.get("team_loss"),

                })
                pitching_bullpen = player.get("statistics", {}).get("pitching", {}).get("bullpen", {})
                pitching_bullpen_onbase = pitching_bullpen.get("onbase", {})
                pitching_bullpen_runs = pitching_bullpen.get("runs", {})
                pitching_bullpen_outcome = pitching_bullpen.get("outcome", {})
                pitching_bullpen_outs = pitching_bullpen.get("outs", {})
                pitching_bullpen_steal = pitching_bullpen.get("steal", {})
                pitching_bullpen_pitches = pitching_bullpen.get("pitches", {})
                pitching_bullpen_in_play = pitching_bullpen.get("in_play", {})
                pitching_bullpen_games = pitching_bullpen.get("games", {})
                player_pitching_bullpen_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "oba": pitching_bullpen.get("oba"),
                    "lob": pitching_bullpen.get("lob"),
                    "era": pitching_bullpen.get("era"),
                    "k9": pitching_bullpen.get("k9"),
                    "whip": pitching_bullpen.get("whip"),
                    "kbb": pitching_bullpen.get("kbb"),
                    "pitch_count": pitching_bullpen.get("pitch_count"),
                    "wp": pitching_bullpen.get("wp"),
                    "bk": pitching_bullpen.get("bk"),
                    "ip_1": pitching_bullpen.get("ip_1"),
                    "ip_2": pitching_bullpen.get("ip_2"),
                    "bf": pitching_bullpen.get("bf"),
                    "gofo": pitching_bullpen.get("gofo"),
                    "babip": pitching_bullpen.get("babip"),
                    "bf_ip": pitching_bullpen.get("bf_ip"),
                    "gbfb": pitching_bullpen.get("gbfb"),
                    "oab": pitching_bullpen.get("oab"),
                    "slg": pitching_bullpen.get("slg"),
                    "obp": pitching_bullpen.get("obp"),
                    "times_through_order": pitching_bullpen.get("times_through_order"),

                    "onbase_s": pitching_bullpen_onbase.get("s"),
                    "onbase_d": pitching_bullpen_onbase.get("d"),
                    "onbase_t": pitching_bullpen_onbase.get("t"),
                    "onbase_hr": pitching_bullpen_onbase.get("hr"),
                    "onbase_tb": pitching_bullpen_onbase.get("tb"),
                    "onbase_bb": pitching_bullpen_onbase.get("bb"),
                    "onbase_ibb": pitching_bullpen_onbase.get("ibb"),
                    "onbase_hbp": pitching_bullpen_onbase.get("hbp"),
                    "onbase_fc": pitching_bullpen_onbase.get("fc"),
                    "onbase_roe": pitching_bullpen_onbase.get("roe"),
                    "onbase_h": pitching_bullpen_onbase.get("h"),
                    "onbase_ci": pitching_bullpen_onbase.get("ci"),
                    "onbase_rov": pitching_bullpen_onbase.get("rov"),
                    "onbase_h9": pitching_bullpen_onbase.get("h9"),
                    "onbase_hr9": pitching_bullpen_onbase.get("hr9"),

                    "runs_total": pitching_bullpen_runs.get("total"),
                    "runs_unearned": pitching_bullpen_runs.get("unearned"),
                    "runs_earned": pitching_bullpen_runs.get("earned"),
                    "runs_ir": pitching_bullpen_runs.get("ir"),
                    "runs_ira": pitching_bullpen_runs.get("ira"),
                    "runs_bqr": pitching_bullpen_runs.get("bqr"),
                    "runs_bqra": pitching_bullpen_runs.get("bqra"),

                    "outcome_klook": pitching_bullpen_outcome.get("klook"),
                    "outcome_kswing": pitching_bullpen_outcome.get("kswing"),
                    "outcome_ktotal": pitching_bullpen_outcome.get("ktotal"),
                    "outcome_ball": pitching_bullpen_outcome.get("ball"),
                    "outcome_iball": pitching_bullpen_outcome.get("iball"),
                    "outcome_dirtball": pitching_bullpen_outcome.get("dirtball"),
                    "outcome_foul": pitching_bullpen_outcome.get("foul"),

                    "outs_po": pitching_bullpen_outs.get("po"),
                    "outs_fo": pitching_bullpen_outs.get("fo"),
                    "outs_fidp": pitching_bullpen_outs.get("fidp"),
                    "outs_lo": pitching_bullpen_outs.get("lo"),
                    "outs_lidp": pitching_bullpen_outs.get("lidp"),
                    "outs_go": pitching_bullpen_outs.get("go"),
                    "outs_gidp": pitching_bullpen_outs.get("gidp"),
                    "outs_klook": pitching_bullpen_outs.get("klook"),
                    "outs_kswing": pitching_bullpen_outs.get("kswing"),
                    "outs_ktotal": pitching_bullpen_outs.get("ktotal"),
                    "outs_sacfly": pitching_bullpen_outs.get("sacfly"),
                    "outs_sachit": pitching_bullpen_outs.get("sachit"),

                    "steal_caught": pitching_bullpen_steal.get("caught"),
                    "steal_stolen": pitching_bullpen_steal.get("stolen"),
                    "steal_pickoff": pitching_bullpen_steal.get("pickoff"),

                    "pitches_count": pitching_bullpen_pitches.get("count"),
                    "pitches_btotal": pitching_bullpen_pitches.get("btotal"),
                    "pitches_ktotal": pitching_bullpen_pitches.get("ktotal"),
                    "pitches_per_ip": pitching_bullpen_pitches.get("per_ip"),
                    "pitches_per_bf": pitching_bullpen_pitches.get("per_bf"),

                    "in_play_linedrive": pitching_bullpen_in_play.get("linedrive"),
                    "in_play_groundball": pitching_bullpen_in_play.get("groundball"),
                    "in_play_popup": pitching_bullpen_in_play.get("popup"),
                    "in_play_flyball": pitching_bullpen_in_play.get("flyball"),

                    "games_start": pitching_bullpen_games.get("start"),
                    "games_play": pitching_bullpen_games.get("play"),
                    "games_finish": pitching_bullpen_games.get("finish"),
                    "games_svo": pitching_bullpen_games.get("svo"),
                    "games_qstart": pitching_bullpen_games.get("qstart"),
                    "games_shutout": pitching_bullpen_games.get("shutout"),
                    "games_complete": pitching_bullpen_games.get("complete"),
                    "games_win": pitching_bullpen_games.get("win"),
                    "games_loss": pitching_bullpen_games.get("loss"),
                    "games_save": pitching_bullpen_games.get("save"),
                    "games_hold": pitching_bullpen_games.get("hold"),
                    "games_blown_save": pitching_bullpen_games.get("blown_save"),
                    "games_team_win": pitching_bullpen_games.get("team_win"),
                    "games_team_loss": pitching_bullpen_games.get("team_loss"),

                })
                pitching_starters = player.get("statistics", {}).get("pitching", {}).get("starters", {})
                pitching_starters_onbase = pitching_bullpen.get("onbase", {})
                pitching_starters_runs = pitching_bullpen.get("runs", {})
                pitching_starters_outcome = pitching_bullpen.get("outcome", {})
                pitching_starters_outs = pitching_bullpen.get("outs", {})
                pitching_starters_steal = pitching_bullpen.get("steal", {})
                pitching_starters_pitches = pitching_bullpen.get("pitches", {})
                pitching_starters_in_play = pitching_bullpen.get("in_play", {})
                pitching_starters_games = pitching_bullpen.get("games", {})
                player_pitching_starters_rows.append({
                "game_id": game_id, "side": side, "team_id": team_id, "team_abbr": team_abbr,
                    "oba": pitching_starters.get("oba"),
                    "lob": pitching_starters.get("lob"),
                    "era": pitching_starters.get("era"),
                    "k9": pitching_starters.get("k9"),
                    "whip": pitching_starters.get("whip"),
                    "kbb": pitching_starters.get("kbb"),
                    "pitch_count": pitching_starters.get("pitch_count"),
                    "wp": pitching_starters.get("wp"),
                    "bk": pitching_starters.get("bk"),
                    "ip_1": pitching_starters.get("ip_1"),
                    "ip_2": pitching_starters.get("ip_2"),
                    "bf": pitching_starters.get("bf"),
                    "gofo": pitching_starters.get("gofo"),
                    "babip": pitching_starters.get("babip"),
                    "bf_ip": pitching_starters.get("bf_ip"),
                    "bf_start": pitching_starters.get("bf_start"),
                    "gbfb": pitching_starters.get("gbfb"),
                    "oab": pitching_starters.get("oab"),
                    "slg": pitching_starters.get("slg"),
                    "obp": pitching_starters.get("obp"),
                    "times_through_order": pitching_starters.get("times_through_order"),

                    "onbase_s": pitching_starters_onbase.get("s"),
                    "onbase_d": pitching_starters_onbase.get("d"),
                    "onbase_t": pitching_starters_onbase.get("t"),
                    "onbase_hr": pitching_starters_onbase.get("hr"),
                    "onbase_tb": pitching_starters_onbase.get("tb"),
                    "onbase_bb": pitching_starters_onbase.get("bb"),
                    "onbase_ibb": pitching_starters_onbase.get("ibb"),
                    "onbase_hbp": pitching_starters_onbase.get("hbp"),
                    "onbase_fc": pitching_starters_onbase.get("fc"),
                    "onbase_roe": pitching_starters_onbase.get("roe"),
                    "onbase_h": pitching_starters_onbase.get("h"),
                    "onbase_ci": pitching_starters_onbase.get("ci"),
                    "onbase_rov": pitching_starters_onbase.get("rov"),
                    "onbase_h9": pitching_starters_onbase.get("h9"),
                    "onbase_hr9": pitching_starters_onbase.get("hr9"),

                    "runs_total": pitching_starters_runs.get("total"),
                    "runs_unearned": pitching_starters_runs.get("unearned"),
                    "runs_earned": pitching_starters_runs.get("earned"),
                    "runs_ir": pitching_starters_runs.get("ir"),
                    "runs_ira": pitching_starters_runs.get("ira"),
                    "runs_bqr": pitching_starters_runs.get("bqr"),
                    "runs_bqra": pitching_starters_runs.get("bqra"),

                    "outcome_klook": pitching_starters_outcome.get("klook"),
                    "outcome_kswing": pitching_starters_outcome.get("kswing"),
                    "outcome_ktotal": pitching_starters_outcome.get("ktotal"),
                    "outcome_ball": pitching_starters_outcome.get("ball"),
                    "outcome_iball": pitching_starters_outcome.get("iball"),
                    "outcome_dirtball": pitching_starters_outcome.get("dirtball"),
                    "outcome_foul": pitching_starters_outcome.get("foul"),

                    "outs_po": pitching_starters_outs.get("po"),
                    "outs_fo": pitching_starters_outs.get("fo"),
                    "outs_fidp": pitching_starters_outs.get("fidp"),
                    "outs_lo": pitching_starters_outs.get("lo"),
                    "outs_lidp": pitching_starters_outs.get("lidp"),
                    "outs_go": pitching_starters_outs.get("go"),
                    "outs_gidp": pitching_starters_outs.get("gidp"),
                    "outs_klook": pitching_starters_outs.get("klook"),
                    "outs_kswing": pitching_starters_outs.get("kswing"),
                    "outs_ktotal": pitching_starters_outs.get("ktotal"),
                    "outs_sacfly": pitching_starters_outs.get("sacfly"),
                    "outs_sachit": pitching_starters_outs.get("sachit"),

                    "steal_caught": pitching_starters_steal.get("caught"),
                    "steal_stolen": pitching_starters_steal.get("stolen"),
                    "steal_pickoff": pitching_starters_steal.get("pickoff"),

                    "pitches_count": pitching_starters_pitches.get("count"),
                    "pitches_btotal": pitching_starters_pitches.get("btotal"),
                    "pitches_ktotal": pitching_starters_pitches.get("ktotal"),
                    "pitches_per_ip": pitching_starters_pitches.get("per_ip"),
                    "pitches_per_bf": pitching_starters_pitches.get("per_bf"),
                    "pitches_per_start": pitching_starters_pitches.get("per_start"),

                    "in_play_linedrive": pitching_starters_in_play.get("linedrive"),
                    "in_play_groundball": pitching_starters_in_play.get("groundball"),
                    "in_play_popup": pitching_starters_in_play.get("popup"),
                    "in_play_flyball": pitching_starters_in_play.get("flyball"),

                    "games_start": pitching_starters_games.get("start"),
                    "games_play": pitching_starters_games.get("play"),
                    "games_finish": pitching_starters_games.get("finish"),
                    "games_svo": pitching_starters_games.get("svo"),
                    "games_qstart": pitching_starters_games.get("qstart"),
                    "games_shutout": pitching_starters_games.get("shutout"),
                    "games_complete": pitching_starters_games.get("complete"),
                    "games_win": pitching_starters_games.get("win"),
                    "games_loss": pitching_starters_games.get("loss"),
                    "games_save": pitching_starters_games.get("save"),
                    "games_hold": pitching_starters_games.get("hold"),
                    "games_blown_save": pitching_starters_games.get("blown_save"),
                    "games_team_win": pitching_starters_games.get("team_win"),
                    "games_team_loss": pitching_starters_games.get("team_loss"),

                })
    # ==== WRITE TO CSV FILES ====
    def write_csv(filename, rows):
        df = pd.DataFrame(rows)
        df.to_csv(f"./mlb_stats/sportradar/{filename}_{season}.csv", index=False)
        print(f"Saved {filename}_{season}.csv with {len(df)} rows")

    write_csv("game_info", game_info_rows)
    write_csv("broadcast", broadcast_rows)
    write_csv("weather", weather_rows)
    write_csv("team_totals", team_totals_rows)
    write_csv("probable_starters", probable_pitcher_rows)
    write_csv("starting_pitchers", starting_pitcher_rows)
    write_csv("rosters", roster_rows)
    write_csv("lineups", lineup_rows)
    write_csv("scoring", scoring_rows)
    write_csv("team_hitting", team_hitting_rows)
    write_csv("team_pitching", team_pitching_rows)
    write_csv("team_pitching_starters", team_pitching_starters_rows)
    write_csv("team_pitching_bullpen", team_pitching_bullpen_rows)
    write_csv("team_fielding", team_fielding_rows)
    write_csv("player_hitting_season_to_date", player_hitting_season_to_date_rows)
    write_csv("player_hitting_overall", player_hitting_overall_rows)
    write_csv("player_fielding_season_to_date", player_fielding_season_to_date_rows)
    write_csv("player_fielding_overall", player_fielding_overall_rows)
    write_csv("player_pitching_season_to_date", player_pitching_season_to_date_rows)
    write_csv("player_pitching_overall", player_pitching_overall_rows)
    write_csv("player_pitching_bullpen", player_pitching_bullpen_rows)
    write_csv("player_pitching_starters", player_pitching_starters_rows)

def get_league_schedule(season):
    url = "https://api.sportradar.com/mlb/trial/v8/en/games/2025/REG/schedule.json"
    headers = {
        "accept": "application/json",
        "x-api-key": "KlWwaPeATnCTPeXsw8YByZI0mJxb4akox07vLInv",
    }
    resp = requests.get(url, headers=headers, timeout=20)
    resp.raise_for_status()
    data = resp.json()
    schedule_league = data.get("league") or {}
    schedule_season = data.get("season") or {}
    schedule_games  = data.get("games")  or []

    rows: list[dict] = []
    # ------------ Helpers ------------
    def flatten_dict(src: dict, prefix: str = "", out: dict | None = None) -> dict:
        """Recursively flattens a dict into a single level, prefixing keys."""
        if out is None:
            out = {}
        if not isinstance(src, dict):
            return out
        for k, v in src.items():
            key = f"{prefix}{k}" if not prefix else f"{prefix}{k}"
            if isinstance(v, dict):
                flatten_dict(v, f"{key}_", out)
            else:
                out[key] = v
        return out

    # ------------ Build flat rows ------------
    for g in schedule_games:
        row: dict = {}

        # League & season on every row
        flatten_dict(schedule_league, "league_", row)
        flatten_dict(schedule_season, "season_", row)

        # Game top-level (exclude nested blocks we flatten separately)
        for k, v in (g or {}).items():
            if k in ("venue", "home", "away", "broadcasts"):
                continue
            row[f"game_{k}"] = v

        # Venue / Home / Away (fully flattened, including nested location)
        flatten_dict(g.get("venue") or {}, "venue_", row)
        flatten_dict(g.get("home")  or {}, "home_",  row)
        flatten_dict(g.get("away")  or {}, "away_",  row)

        # Broadcasts (create indexed columns so nothing is lost)
        for i, b in enumerate(g.get("broadcasts") or [], start=1):
            b = b or {}
            for bk, bv in b.items():
                row[f"broadcast_{i}_{bk}"] = bv

        rows.append(row)
    # ==== WRITE TO CSV FILES ====
    df = pd.DataFrame(rows)
    df.to_csv(f"./mlb_stats/sportradar/league_schedule_{season}.csv", index=False)
    print(f"Saved league_schedule_{season}.csv with {len(df)} rows")

#get_play_by_play("2025")
#get_game_extended_summary("2025")
get_league_schedule("2025")