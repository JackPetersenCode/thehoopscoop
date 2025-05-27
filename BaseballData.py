import requests
import time
import pandas as pd
import csv

BASE_URL = "https://statsapi.mlb.com/api/v1"

def get_game_ids_for_season(season):
    """Fetches all game IDs for the given season"""
    url = f"{BASE_URL}/schedule"
    params = {
        "sportId": 1,  # MLB
        "season": season,
        "gameType": "R",  # Regular season
        "fields": "dates.games.gamePk"
    }
    
    response = requests.get(url, params=params)
    if response.status_code != 200:
        print(f"Error fetching schedule: {response.text}")
        return []

    data = response.json()
    #game_ids = [game["gamePk"] for date in data["dates"] for game in date["games"]]
    print(data)
    #return "returned!"    
    #game_ids = data
    #return game_ids

def get_box_score(game_id):
    """Fetches the box score for a given game ID"""
    url = f"{BASE_URL}/game/{game_id}/boxscore"
    response = requests.get(url)
    if response.status_code != 200:
        print(f"Error fetching box score for game {game_id}: {response.text}")
        return None

    return response.json()

def fetch_box_scores_for_season(season):
    """Fetches box scores for every game in the given season"""
    game_ids = get_game_ids_for_season(season)
    box_scores = {}

    for game_id in game_ids[:10]:  # Limits to the first 10 games
        print(f"Fetching box score for game {game_id}...")
        box_scores[game_id] = get_box_score(game_id)
        time.sleep(0.5)  # Prevent hitting rate limits

    return box_scores

# Example: Fetch box scores for the 2023 season
#season_box_scores = fetch_box_scores_for_season(2024)

# Print example box score
#if season_box_scores:
#    example_game_id = next(iter(season_box_scores))
#    print(f"\nExample Box Score for Game {example_game_id}:")
#    print(season_box_scores[example_game_id])

BASE_URL = "https://statsapi.mlb.com/api/v1/schedule"

def get_mlb_schedule(season):
    """Fetches the MLB schedule for a given season"""
    params = {
        "sportId": 1,  # MLB Sport ID
        "season": season,
        "gameType": "R",  # Regular season
        #"fields": "dates.games.gamePk,dates.games.teams.away.team.name,dates.games.teams.home.team.name,dates.date"
    }

    response = requests.get(BASE_URL, params=params)
    
    if response.status_code != 200:
        print(f"Error fetching schedule: {response.text}")
        return None

    return response.json()

def save_games_to_csv(games_data, season):
    """Stores MLB games data into a CSV file"""
    game_list = []

    for game in games_data.get("dates", []):
        for g in game.get("games", []):
            print(g)
            game_list.append({
                "game_pk": g["gamePk"],
                "game_guid": g["gameGuid"],
                "game_type": g["gameType"],
                "season": g["season"],
                "game_date": g["gameDate"],
                "official_date": g["officialDate"],
                "abstract_game_state": g["status"]["abstractGameState"],
                "coded_game_state": g["status"]["codedGameState"],
                "detailed_state": g["status"]["detailedState"],
                "status_code": g["status"]["statusCode"],
                "start_time_tbd": g["status"]["startTimeTBD"],
                "away_team_id": g["teams"]["away"]["team"]["id"],
                "away_team_name": g["teams"]["away"]["team"]["name"],
                "away_score": g["teams"]["away"].get("score", None),  # Handle missing score
                "away_wins": g["teams"]["away"]["leagueRecord"]["wins"],
                "away_losses": g["teams"]["away"]["leagueRecord"]["losses"],
                "away_win_pct": g["teams"]["away"]["leagueRecord"]["pct"],
                "away_is_winner": g["teams"]["away"].get("isWinner", None),
                "home_team_id": g["teams"]["home"]["team"]["id"],
                "home_team_name": g["teams"]["home"]["team"]["name"],
                "home_score": g["teams"]["home"].get("score", None),  # Handle missing score,
                "home_wins": g["teams"]["home"]["leagueRecord"]["wins"],
                "home_losses": g["teams"]["home"]["leagueRecord"]["losses"],
                "home_win_pct": g["teams"]["home"]["leagueRecord"]["pct"],
                "home_is_winner": g["teams"]["home"].get("isWinner", None),
                "venue_id": g["venue"]["id"],
                "venue_name": g["venue"]["name"],
                "is_tie": g.get("isTie", None),  # Handle missing is_tie
                "game_number": g["gameNumber"],
                "double_header": g["doubleHeader"],
                "day_night": g["dayNight"],
                "description": g.get("description", ""),
                "scheduled_innings": g["scheduledInnings"],
                "games_in_series": g["gamesInSeries"],
                "series_game_number": g["seriesGameNumber"],
                "series_description": g["seriesDescription"],
                "if_necessary": g["ifNecessary"],
                "if_necessary_desc": g["ifNecessaryDescription"]
            })

    df = pd.DataFrame(game_list)
    csv_filename = f"./mlb_stats/mlb_games_{season}.csv"
    df.to_csv(csv_filename, index=False)
    
    print(df)
    return df, csv_filename


def get_schedule_save_mlb_games(season):
    schedule_data = get_mlb_schedule(season)
    print(schedule_data)
    #Print sample data
    if schedule_data:
        # Save to CSV and display
        df_games, filename = save_games_to_csv(schedule_data, season)
        print(df_games)


def read_csv_file(file_path):
    """
    Reads a CSV file and prints its contents.

    :param file_path: Path to the CSV file
    :return: List of dictionaries where each row is represented as a dictionary
    """
    data = []
    
    try:
        with open(file_path, mode='r', encoding='utf-8') as file:
            csv_reader = csv.DictReader(file)  # Reads CSV as a list of dictionaries
            
            for row in csv_reader:
                data.append(row)  # Store each row in the list
            
    except FileNotFoundError:
        print(f"Error: The file '{file_path}' was not found.")
    except Exception as e:
        print(f"An error occurred: {e}")
    
    return data  # Return the list of dictionaries

# Example Usage
#csv_data = read_csv_file("./mlb_stats/mlb_games_2023.csv")
#print(csv_data)  # Print the CSV data as a list of dictionaries

def get_mlb_play_by_play_from_game_pk(gamePk, season):
    BASE_URL = f"http://statsapi.mlb.com/api/v1/game/{gamePk}/playByPlay"
    print(gamePk)
    response = requests.get(BASE_URL)
    if response.status_code == 200:
        return response.json()  # Return the parsed JSON response
    else:
        print("error: " + {gamePk})
        print(f"Error: Unable to fetch data for gamePk {gamePk} (Status Code: {response.status_code})")
        return None  # Return None if request fails


def get_box_score_from_game_pk(gamePk, season):
    # Define the base URL dynamically using gamePk
    BASE_URL = f"http://statsapi.mlb.com/api/v1/game/{gamePk}/boxscore"
    print(gamePk)
    # Make the API request
    response = requests.get(BASE_URL)
    # Check if the request was successful
    if response.status_code == 200:
        return response.json()  # Return the parsed JSON response
    else:
        print("error: " + {gamePk})
        print(f"Error: Unable to fetch data for gamePk {gamePk} (Status Code: {response.status_code})")
        return None  # Return None if request fails


import requests
import pandas as pd
import json
from collections.abc import MutableMapping


#csv_path

# Helper function to flatten nested JSON
def flatten_play(play, parent_key='', sep='_'):
    items = []
    for k, v in play.items():
        new_key = f"{parent_key}{sep}{k}" if parent_key else k
        if isinstance(v, dict):
            items.extend(flatten_play(v, new_key, sep=sep).items())
        elif isinstance(v, list):
            # Handle lists by converting them to a string or processing as needed
            items.append((new_key, str(v)))
        else:
            items.append((new_key, v))
    return dict(items)


def get_schedule_save_play_by_play(season):
    response = requests.get(f"http://localhost:5190/api/MLBGame/read/{season}")
    #csv_data = read_csv_file(f"./mlb_stats/mlb_games_{season}.csv")
    # Check if the request was successful
    if response.status_code == 200:
        csv_data = response.json()  # Return the parsed JSON response
    else:
        print(f"Error: (Status Code: {response.status_code})")
        return None  # Return None if request fails
    
    play_rows = []
    runners_rows = []
    credit_rows = []
    play_event_rows = []
    print("games length: " + str(len(csv_data)))
    for game in csv_data:
        game_pk = game["game_pk"]
        play_by_play_response = get_mlb_play_by_play_from_game_pk(game_pk, season)
        # Sample list of gamePks for demonstration
        # Collect all flattened play data
        #data = play_by_play_response.json()
        all_plays = play_by_play_response.get("allPlays", [])
        for play in all_plays:
            result = play.get("result", {})
            about = play.get("about", {})
            count = play.get("count", {})
            matchup = play.get("matchup", {})
            matchup_batter = matchup.get("batter", {})
            matchup_bat_side = matchup.get("batSide", {})
            matchup_pitcher = matchup.get("pitcher", {})
            matchup_pitch_hand = matchup.get("pitchHand", {})
            matchup_post_on_first = matchup.get("postOnFirst", {})
            matchup_splits = matchup.get("splits", {})
            pitchIndex = play.get("pitchIndex", [])
            actionIndex = play.get("actionIndex", [])
            runnerIndex = play.get("runnerIndex", [])
            runners = play.get("runners", [])
            play_end_time = play.get("playEndTime")
            play_at_bat_index = play.get("atBatIndex")
            play_events = play.get("playEvents", [])

            for runner in runners:
                movement = runner.get("movement", {})
                details = runner.get("details", {})
                credits = runner.get("credits", [])
                details_runner = details.get("runner", {})

                runners_rows.append({
                    "game_pk": game_pk,
                    "at_bat_index": about.get("atBatIndex"),
                    "runners_movement_origin_base": movement.get("originBase"),
                    "runners_movement_start": movement.get("start"),
                    "runners_movement_end": movement.get("end"),
                    "runners_movement_out_base": movement.get("outBase"),
                    "runners_movement_is_out": movement.get("isOut"),
                    "runners_movement_out_number": movement.get("outNumber"),

                    "runners_details_event": details.get("event"),
                    "runners_details_event_type": details.get("eventType"),
                    "runners_details_movement_reason": details.get("movementReason"),
                    "runners_details_player_id": details_runner.get("id"),
                    "runners_details_player_full_name": details_runner.get("fullName"),
                    "runners_details_responsible_pitcher_id": details.get("responsiblePitcher", {}).get("id") if details.get("responsiblePitcher") else None,
                    "runners_details_is_scoring_event": details.get("isScoringEvent"),
                    "runners_details_rbi": details.get("rbi"),
                    "runners_details_earned": details.get("earned"),
                    "runners_details_team_unearned": details.get("teamUnearned"),
                    "runners_details_play_index": details.get("playIndex"),
                    "runners_credits": json.dumps(credits)
                })

                for credit in credits:
                    credit_player = credit.get("player", {})
                    credit_position = credit.get("position", {})
                    credit_rows.append({
                        "game_pk": game_pk,
                        "at_bat_index": about.get("atBatIndex"),
                        "runners_details_play_index": details.get("playIndex"),
                        "runners_details_player_id": details_runner.get("id"),
                        "runners_credits_player_id": credit_player.get("id"),
                        "runners_credits_position_code": credit_position.get("code"),
                        "runners_credits_position_name": credit_position.get("name"),
                        "runners_credits_position_type": credit_position.get("type"),
                        "runners_credits_position_abbreviation": credit_position.get("abbreviation"),
                        "runners_credits_credit": credit.get("credit")
                    })

            for play_event in play_events:
                play_event_details = play_event.get("details", {})
                play_event_details_call = play_event_details.get("call", {})
                play_event_details_type = play_event_details.get("type", {})
                play_event_count = play_event.get("count", {})
                play_event_pitch_data = play_event.get("pitchData", {})
                play_event_pitch_data_coordinates = play_event_pitch_data.get("coordinates", {})
                play_event_pitch_data_breaks = play_event_pitch_data.get("breaks", {})
                play_event_hit_data = play_event.get("hitData", {})
                play_event_hit_data_coordinates = play_event_hit_data.get("coordinates", {})

                play_event_rows.append({
                    "game_pk": game_pk,
                    "about_at_bat_index": about.get("atBatIndex"),
                    "play_events_details_call_code": play_event_details_call.get("code"),
                    "play_events_details_call_description": play_event_details_call.get("description"),
                    "play_events_details_code": play_event_details.get("code"),
                    "play_events_details_description": play_event_details.get("description"),
                    "play_events_details_ball_color": play_event_details.get("ballColor"),
                    "play_events_details_trail_color": play_event_details.get("trailColor"),
                    "play_events_details_is_in_play": play_event_details.get("isInPlay"),
                    "play_events_details_is_strike": play_event_details.get("isStrike"),
                    "play_events_details_is_ball": play_event_details.get("isBall"),
                    "play_events_details_type_code": play_event_details_type.get("code"),
                    "play_events_details_type_description": play_event_details_type.get("description"),
                    "play_events_details_is_out": play_event_details.get("isOut"),
                    "play_events_details_has_review": play_event_details.get("hasReview"),
                    
                    "play_events_count_balls": play_event_count.get("balls"),
                    "play_events_count_strikes": play_event_count.get("strikes"),
                    "play_events_count_outs": play_event_count.get("outs"),
                    
                    "play_events_pitch_data_start_speed": play_event_pitch_data.get("startSpeed"),
                    "play_events_pitch_data_end_speed": play_event_pitch_data.get("endSpeed"),
                    "play_events_pitch_data_strike_zone_top": play_event_pitch_data.get("strikeZoneTop"),
                    "play_events_pitch_data_strike_zone_bottom": play_event_pitch_data.get("strikeZoneBottom"),
                    "play_events_pitch_data_coordinates_aY": play_event_pitch_data_coordinates.get("aY"),
                    "play_events_pitch_data_coordinates_aZ": play_event_pitch_data_coordinates.get("aZ"),
                    "play_events_pitch_data_coordinates_pfxX": play_event_pitch_data_coordinates.get("pfxX"),
                    "play_events_pitch_data_coordinates_pfxZ": play_event_pitch_data_coordinates.get("pfxZ"),
                    "play_events_pitch_data_coordinates_pX": play_event_pitch_data_coordinates.get("pX"),
                    "play_events_pitch_data_coordinates_pZ": play_event_pitch_data_coordinates.get("pZ"),
                    "play_events_pitch_data_coordinates_vX0": play_event_pitch_data_coordinates.get("vX0"),
                    "play_events_pitch_data_coordinates_vY0": play_event_pitch_data_coordinates.get("vY0"),
                    "play_events_pitch_data_coordinates_vZ0": play_event_pitch_data_coordinates.get("vZ0"),
                    "play_events_pitch_data_coordinates_x": play_event_pitch_data_coordinates.get("x"),
                    "play_events_pitch_data_coordinates_y": play_event_pitch_data_coordinates.get("y"),
                    "play_events_pitch_data_coordinates_x0": play_event_pitch_data_coordinates.get("x0"),
                    "play_events_pitch_data_coordinates_y0": play_event_pitch_data_coordinates.get("y0"),
                    "play_events_pitch_data_coordinates_z0": play_event_pitch_data_coordinates.get("z0"),
                    "play_events_pitch_data_coordinates_aX": play_event_pitch_data_coordinates.get("aX"),
                    "play_events_pitch_data_breaks_break_angle": play_event_pitch_data_breaks.get("breakAngle"),
                    "play_events_pitch_data_breaks_break_length": play_event_pitch_data_breaks.get("breakLength"),
                    "play_events_pitch_data_breaks_break_y": play_event_pitch_data_breaks.get("breakY"),
                    "play_events_pitch_data_breaks_break_vertical": play_event_pitch_data_breaks.get("breakVertical"),
                    "play_events_pitch_data_breaks_break_vertical_induced": play_event_pitch_data_breaks.get("breakVerticalInduced"),
                    "play_events_pitch_data_breaks_break_horizontal": play_event_pitch_data_breaks.get("breakHorizontal"),
                    "play_events_pitch_data_breaks_spin_rate": play_event_pitch_data_breaks.get("spinRate"),
                    "play_events_pitch_data_breaks_spin_direction": play_event_pitch_data_breaks.get("spinDirection"),
                    "play_events_pitch_data_zone": play_event_pitch_data.get("zone"),
                    "play_events_pitch_data_type_confidence": play_event_pitch_data.get("typeConfidence"),
                    "play_events_pitch_data_plate_time": play_event_pitch_data.get("plateTime"),
                    "play_events_pitch_data_extension": play_event_pitch_data.get("extension"),

                    "play_events_hit_data_launch_speed": play_event_hit_data.get("launchSpeed"),
                    "play_events_hit_data_launch_angle": play_event_hit_data.get("launchAngle"),
                    "play_events_hit_data_total_distance": play_event_hit_data.get("totalDistance"),
                    "play_events_hit_data_trajectory": play_event_hit_data.get("trajectory"),
                    "play_events_hit_data_hardness": play_event_hit_data.get("hardness"),
                    "play_events_hit_data_location": play_event_hit_data.get("location"),
                    "play_events_hit_data_coordinates_coordX": play_event_hit_data_coordinates.get("coordX"),
                    "play_events_hit_data_coordinates_coordY": play_event_hit_data_coordinates.get("coordY"),

                    "play_events_index": play_event.get("index"),
                    "play_events_play_id": play_event.get("playId"),
                    "play_events_pitch_number": play_event.get("pitchNumber"),
                    "play_events_start_time": play_event.get("startTime"),
                    "play_events_end_time": play_event.get("endTime"),
                    "play_events_is_pitch": play_event.get("isPitch"),
                    "play_events_type": play_event.get("type")
                })
            

            play_rows.append({
                "game_pk": game_pk,
                "result_type": result.get("type"),
                "result_event": result.get("event"),
                "result_event_type": result.get("eventType"),
                "result_description": result.get("description"),
                "result_rbi": result.get("rbi"),
                "result_away_score": result.get("awayScore"),
                "result_home_score": result.get("homeScore"),
                "result_is_out": result.get("isOut"),
                
                "about_at_bat_index": about.get("atBatIndex"),
                "about_half_inning": about.get("halfInning"),
                "about_is_top_inning": about.get("isTopInning"),
                "about_inning": about.get("inning"),
                "about_start_time": about.get("startTime"),
                "about_end_time": about.get("endTime"),
                "about_is_complete": about.get("isComplete"),
                "about_is_scoring_play": about.get("isScoringPlay"),
                "about_has_review": about.get("hasReview"),
                "about_has_out": about.get("hasOut"),
                "about_captivating_index": about.get("captivatingIndex"),
                
                "count_balls": count.get("balls"),
                "count_strikes": count.get("strikes"),
                "count_outs": count.get("outs"),

                "matchup_batter_id": matchup_batter.get("id"),
                "matchup_batter_full_name": matchup_batter.get("fullName"),
                "matchup_bat_side_code": matchup_bat_side.get("code"),
                "matchup_bat_side_description": matchup_bat_side.get("description"),
                "matchup_pitcher_id": matchup_pitcher.get("id"),
                "matchup_pitcher_full_name": matchup_pitcher.get("fullName"),
                "matchup_pitch_hand_code": matchup_pitch_hand.get("code"),
                "matchup_pitch_hand_description": matchup_pitch_hand.get("description"),
                "matchup_splits_batter": matchup_splits.get("batter"),
                "matchup_splits_pitcher": matchup_splits.get("pitcher"),
                "matchup_splits_men_on_base": matchup_splits.get("menOnBase"),
                "matchup_post_on_first_id": matchup_post_on_first.get("id"),
                "matchup_post_on_first_full_name": matchup_post_on_first.get("fullName"),
                "matchup_batter_hot_cold_zones": json.dumps(matchup.get("batterHotColdZones", [])),
                "matchup_pitcher_hot_cold_zones": json.dumps(matchup.get("pitcherHotColdZones", [])),

                "pitch_index": json.dumps(pitchIndex),
                "action_index": json.dumps(actionIndex),
                "runner_index": json.dumps(runnerIndex),
                #runners array
                #playEvents array
                "play_end_time": play_end_time,
                "play_at_bat_index": play_at_bat_index
            })
        #    flat = flatten_play(play)  # Your flattening function
        #    flattened_plays.append(flat)
        #    all_keys.update(flat.keys())  # Collect all possible column headers
    # ==== WRITE TO CSV FILES ====
    def write_csv(filename, rows):
        df = pd.DataFrame(rows)
        df.to_csv(f"./mlb_stats/{filename}_{season}.csv", index=False)
        print(f"Saved {filename}_{season}.csv with {len(df)} rows")

    #write_csv("plays", play_rows)
    #write_csv("runners", runners_rows)
    #write_csv("credits", credit_rows)
    #write_csv("play_events", play_event_rows)
    # Convert to sorted list for consistent column order
    #headers = sorted(all_keys)

    # Write the CSV safely
    #with open("./mlb_stats/mlb_play_by_play.csv", "w", newline='', encoding='utf-8') as f:
    #    writer = csv.DictWriter(f, fieldnames=headers)
    #    writer.writeheader()
#
    #    for row in flattened_plays:
    #        writer.writerow(row)



def get_schedule_save_box_scores(season):

    response = requests.get(f"http://localhost:5190/api/MLBGame/read/{season}")
    #csv_data = read_csv_file(f"./mlb_stats/mlb_games_{season}.csv")
    # Check if the request was successful
    if response.status_code == 200:
        csv_data = response.json()  # Return the parsed JSON response
    else:
        print(f"Error: (Status Code: {response.status_code})")
        return None  # Return None if request fails
    #games_data = get_mlb_schedule(season)
    #print(games_data)
    #print(csv_data)
#
    ## Initialize CSV row containers
    #team_rows = []
    #team_stats_batting_rows = []
    #team_stats_pitching_rows = []
    #team_stats_fielding_rows = []
    #player_stats_rows = []
    #player_season_stats_rows = []
#
    #for game in csv_data:
    #    game_pk = game["game_pk"]
    #    box_score_response = get_box_score_from_game_pk(game_pk, season)
#
    #    # === TEAM DATA (AWAY & HOME) ===
    #    for team_side in ["away", "home"]:
    #        team = box_score_response["teams"][team_side]["team"]
    #        record = team.get("record", {})
    #        league_record = record.get("leagueRecord", {})
#
    #        team_rows.append({
    #            "gamePk": game_pk,
    #            "teamSide": team_side,
    #            "spring_league_id": team["springLeague"]["id"],
    #            "spring_league_name": team["springLeague"]["name"],
    #            "spring_league_abbreviation": team["springLeague"]["abbreviation"],
    #            "all_star_status": team["allStarStatus"],
    #            "season": team["season"],
    #            "venue_id": team["venue"]["id"],
    #            "venue_name": team["venue"]["name"],
    #            "team_code": team["teamCode"],
    #            "file_code": team["fileCode"],
    #            "abbreviation": team["abbreviation"],
    #            "team_name": team["teamName"],
    #            "location_name": team["locationName"],
    #            "first_year_of_play": team["firstYearOfPlay"],
    #            "league_id": team["league"]["id"],
    #            "league_name": team["league"]["name"],
    #            "sport_id": team["sport"]["id"],
    #            "sport_name": team["sport"]["name"],
    #            "short_name": team["shortName"],
    #            "record_games_played": record.get("gamesPlayed"),
    #            "record_wild_card_games_back": record.get("wildCardGamesBack"),
    #            "record_league_games_back": record.get("leagueGamesBack"),
    #            "record_spring_league_games_back": record.get("springLeagueGamesBack"),
    #            "record_sport_games_back": record.get("sportGamesBack"),
    #            "record_division_games_back": record.get("divisionGamesBack"),
    #            "record_conference_games_back": record.get("conferenceGamesBack"),
    #            "record_league_record_wins": league_record.get("wins"),
    #            "record_league_record_losses": league_record.get("losses"),
    #            "record_league_record_ties": league_record.get("ties"),
    #            "record_league_record_pct": league_record.get("pct"),
    #            "record_division_leader": record.get("divisionLeader"),
    #            "record_wins": record.get("wins"),
    #            "record_losses": record.get("losses"),
    #            "record_winning_percentage": record.get("winningPercentage"),
    #            "franchise_name": team["franchiseName"],
    #            "club_name": team["clubName"],
    #            "active": team["active"]
    #        })
#
    #        for stat_type in ["batting", "pitching", "fielding"]:
    #            stats = box_score_response["teams"][team_side]["teamStats"].get(stat_type, {})
#
    #            # Start each row with game/team info
    #            stat_row = {
    #                "gamePk": game_pk,
    #                "teamSide": team_side,
    #                "teamName": team["teamName"]
    #            }
#
    #            # Add each stat as a separate column
    #            for stat_key, stat_value in stats.items():
    #                stat_row[stat_key] = stat_value
#
    #            if stat_type == "batting":
    #                team_stats_batting_rows.append(stat_row)
    #            elif stat_type == "pitching":
    #                team_stats_pitching_rows.append(stat_row)
    #            else:
    #                team_stats_fielding_rows.append(stat_row)

    #for game in csv_data:
#
    #    box_score_response = get_box_score_from_game_pk(game["game_pk"], season)
    #    box_score = {
    #        "gamePk": game["game_pk"],
    #        "away_spring_leauge_id": box_score_response["teams"]["away"]["team"]["springLeague"]["id"],
    #        "away_spring_league_name": box_score_response["teams"]["away"]["team"]["springLeague"]["name"],
    #        "away_spring_league_link": box_score_response["teams"]["away"]["team"]["springLeague"]["link"],
    #        "away_spring_league_abbreviation": box_score_response["teams"]["away"]["team"]["springLeague"]["abbreviation"],
    #        "away_all_star_status": box_score_response["teams"]["away"]["team"]["allStarStatus"],
    #        "away_season": box_score_response["teams"]["away"]["team"]["season"],
    #        "away_venue_id": box_score_response["teams"]["away"]["team"]["venue"]["id"],
    #        "away_venue_name": box_score_response["teams"]["away"]["team"]["venue"]["name"],
    #        "away_venue_link":  box_score_response["teams"]["away"]["team"]["venue"]["link"],
    #        "away_team_code": box_score_response["teams"]["away"]["team"]["teamCode"],
    #        "away_file_code": box_score_response["teams"]["away"]["team"]["fileCode"],
    #        "away_abbreviation": box_score_response["teams"]["away"]["team"]["abbreviation"],
    #        "away_team_name": box_score_response["teams"]["away"]["team"]["teamName"],
    #        "away_location_name": box_score_response["teams"]["away"]["team"]["locationName"],
    #        "away_first_year_of_play": box_score_response["teams"]["away"]["team"]["firstYearOfPlay"],
    #        "away_league_id": box_score_response["teams"]["away"]["team"]["league"]["id"],
    #        "away_league_name": box_score_response["teams"]["away"]["team"]["league"]["name"],
    #        "away_league_link": box_score_response["teams"]["away"]["team"]["league"]["link"],
    #        "away_sport_id": box_score_response["teams"]["away"]["team"]["sport"]["id"],
    #        "away_sport_name": box_score_response["teams"]["away"]["team"]["sport"]["name"],
    #        "away_sport_link": box_score_response["teams"]["away"]["team"]["sport"]["link"],
    #        "away_short_name": box_score_response["teams"]["away"]["team"]["shortName"],
    #        "away_record_games_played": box_score_response["teams"]["away"]["team"]["record"]["gamesPlayed"],
    #        "away_record_wild_card_games_back": box_score_response["teams"]["away"]["team"]["record"]["wildCardGamesBack"],
    #        "away_record_league_games_back": box_score_response["teams"]["away"]["team"]["record"]["leagueGamesBack"],
    #        "away_record_spring_league_games_back": box_score_response["teams"]["away"]["team"]["record"]["springLeagueGamesBack"],
    #        "away_record_sport_games_back": box_score_response["teams"]["away"]["team"]["record"]["sportGamesBack"],
    #        "away_record_division_games_back": box_score_response["teams"]["away"]["team"]["record"]["divisionGamesBack"],
    #        "away_record_conference_games_back": box_score_response["teams"]["away"]["team"]["record"]["conferenceGamesBack"],
    #        "away_record_league_record_wins": box_score_response["teams"]["away"]["team"]["record"]["leagueRecord"]["wins"],
    #        "away_record_league_record_losses": box_score_response["teams"]["away"]["team"]["record"]["leagueRecord"]["losses"],
    #        "away_record_league_record_ties": box_score_response["teams"]["away"]["team"]["record"]["leagueRecord"]["ties"],
    #        "away_record_league_record_pct": box_score_response["teams"]["away"]["team"]["record"]["leagueRecord"]["pct"],
    #        "away_record_records": box_score_response["teams"]["away"]["team"]["record"]["records"],
    #        "away_record_division_leader": box_score_response["teams"]["away"]["team"]["record"]["divisionLeader"],
    #        "away_record_wins": box_score_response["teams"]["away"]["team"]["record"]["wins"],
    #        "away_record_losses": box_score_response["teams"]["away"]["team"]["record"]["losses"],
    ##        "away_record_winning_percentage": box_score_response["teams"]["away"]["team"]["record"]["winningPercentage"],
    ##        "away_franchise_name": box_score_response["teams"]["away"]["team"]["franchiseName"],
    ##        "away_club_name": box_score_response["teams"]["away"]["team"]["clubName"],
    ##        "away_active": box_score_response["teams"]["away"]["team"]["active"],
#
#
#
    #        "away_team_stats_batting_flyouts": box_score_response["teams"]["away"]["teamStats"]["batting"]["flyouts"],
    #        "away_team_stats_batting_ground_outs": box_score_response["teams"]["away"]["teamStats"]["batting"]["groundOuts"],
    #        "away_team_stats_batting_air_outs": box_score_response["teams"]["away"]["teamStats"]["batting"]["airOuts"],
    #        "away_team_stats_batting_runs": box_score_response["teams"]["away"]["teamStats"]["batting"]["runs"],
    #        "away_team_stats_batting_doubles": box_score_response["teams"]["away"]["teamStats"]["batting"]["doubles"],
    #        "away_team_stats_batting_triples": box_score_response["teams"]["away"]["teamStats"]["batting"]["triples"],
    #        "away_team_stats_batting_home_runs": box_score_response["teams"]["away"]["teamStats"]["batting"]["homeRuns"],
    #        "away_team_stats_batting_strike_outs": box_score_response["teams"]["away"]["teamStats"]["batting"]["strikeOuts"],
    #        "away_team_stats_batting_base_on_balls": box_score_response["teams"]["away"]["teamStats"]["batting"]["baseOnBalls"],
    #        "away_team_stats_batting_intentional_walks": box_score_response["teams"]["away"]["teamStats"]["batting"]["intentionalWalks"],
    #        "away_team_stats_batting_hits": box_score_response["teams"]["away"]["teamStats"]["batting"]["hits"],
    #        "away_team_stats_batting_hit_by_pitch": box_score_response["teams"]["away"]["teamStats"]["batting"]["hitByPitch"],
    #        "away_team_stats_batting_avg": box_score_response["teams"]["away"]["teamStats"]["batting"]["avg"],
    #        "away_team_stats_batting_atBats": box_score_response["teams"]["away"]["teamStats"]["batting"]["atBats"],
    #        "away_team_stats_batting_obp": box_score_response["teams"]["away"]["teamStats"]["batting"]["obp"],
    #        "away_team_stats_batting_slg": box_score_response["teams"]["away"]["teamStats"]["batting"]["slg"],
    #        "away_team_stats_batting_ops": box_score_response["teams"]["away"]["teamStats"]["batting"]["ops"],
    #        "away_team_stats_batting_caught_stealing": box_score_response["teams"]["away"]["teamStats"]["batting"]["caughtStealing"],
    #        "away_team_stats_batting_stolen_bases": box_score_response["teams"]["away"]["teamStats"]["batting"]["stolenBases"],
    #        "away_team_stats_batting_stolen_base_percentage": box_score_response["teams"]["away"]["teamStats"]["batting"]["stolenBasePercentage"],
    #        "away_team_stats_batting_ground_into_double_play": box_score_response["teams"]["away"]["teamStats"]["batting"]["groundIntoDoublePlay"],
    #        "away_team_stats_batting_ground_into_triple_play": box_score_response["teams"]["away"]["teamStats"]["batting"]["groundIntoTriplePlay"],
    #        "away_team_stats_batting_plate_appearances": box_score_response["teams"]["away"]["teamStats"]["batting"]["plateAppearances"],
    #        "away_team_stats_batting_total_bases": box_score_response["teams"]["away"]["teamStats"]["batting"]["totalBases"],
    #        "away_team_stats_batting_rbi": box_score_response["teams"]["away"]["teamStats"]["batting"]["rbi"],
    #        "away_team_stats_batting_left_on_base": box_score_response["teams"]["away"]["teamStats"]["batting"]["leftOnBase"],
    #        "away_team_stats_batting_sac_bunts": box_score_response["teams"]["away"]["teamStats"]["batting"]["sacBunts"],
    #        "away_team_stats_batting_sac_flies": box_score_response["teams"]["away"]["teamStats"]["batting"]["sacFlies"],
    #        "away_team_stats_batting_catchers_interferance": box_score_response["teams"]["away"]["teamStats"]["batting"]["catchersInterferance"],
    #        "away_team_stats_batting_pickoffs": box_score_response["teams"]["away"]["teamStats"]["batting"]["pickoffs"],
    #        "away_team_stats_batting_at_bats_per_home_run": box_score_response["teams"]["away"]["teamStats"]["batting"]["atBatsPerHomeRun"],
    #        "away_team_stats_batting_pop_outs": box_score_response["teams"]["away"]["teamStats"]["batting"]["popOuts"],
    #        "away_team_stats_batting_line_outs": box_score_response["teams"]["away"]["teamStats"]["batting"]["lineOuts"],
    #        "away_team_stats_pitching_fly_outs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["flyOuts"],
    #        "away_team_stats_pitching_ground_outs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["groundOuts"],
    #        "away_team_stats_pitching_air_outs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["airOuts"],
    #        "away_team_stats_pitching_runs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["runs"],
    #        "away_team_stats_pitching_doubles": box_score_response["teams"]["away"]["teamStats"]["pitching"]["doubles"],
    #        "away_team_stats_pitching_triples": box_score_response["teams"]["away"]["teamStats"]["pitching"]["triples"],
    #        "away_team_stats_pitching_home_runs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["homeRuns"],
    #        "away_team_stats_pitching_strike_outs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["strikeOuts"],
    #        "away_team_stats_pitching_base_on_balls": box_score_response["teams"]["away"]["teamStats"]["pitching"]["baseOnBalls"],
    #        "away_team_stats_pitching_intentional_walks": box_score_response["teams"]["away"]["teamStats"]["pitching"]["intentionalWalks"],
    #        "away_team_stats_pitching_hits": box_score_response["teams"]["away"]["teamStats"]["pitching"]["hits"],
    #        "away_team_stats_pitching_hit_by_pitch": box_score_response["teams"]["away"]["teamStats"]["pitching"]["hitByPitch"],
    #        "away_team_stats_pitching_at_bats": box_score_response["teams"]["away"]["teamStats"]["pitching"]["atBats"],
    #        "away_team_stats_pitching_obp": box_score_response["teams"]["away"]["teamStats"]["pitching"]["obp"],
    #        "away_team_stats_pitching_caught_stealing": box_score_response["teams"]["away"]["teamStats"]["pitching"]["caughtStealing"],
    #        "away_team_stats_pitching_stolen_bases": box_score_response["teams"]["away"]["teamStats"]["pitching"]["stolenBases"],
    #        "away_team_stats_pitching_stolen_base_percentage": box_score_response["teams"]["away"]["teamStats"]["pitching"]["stolenBasePercentage"],
    #        "away_team_stats_pitching_number_of_pitches": box_score_response["teams"]["away"]["teamStats"]["pitching"]["numberOfPitches"],
    #        "away_team_stats_pitching_era": box_score_response["teams"]["away"]["teamStats"]["pitching"]["era"],
    #        "away_team_stats_pitching_innings_pitched": box_score_response["teams"]["away"]["teamStats"]["pitching"]["inningsPitched"],
    #        "away_team_stats_pitching_save_opportunities": box_score_response["teams"]["away"]["teamStats"]["pitching"]["saveOpportunities"],
    #        "away_team_stats_pitching_earned_runs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["earnedRuns"],
    #        "away_team_stats_pitching_whip": box_score_response["teams"]["away"]["teamStats"]["pitching"]["whip"],
    #        "away_team_stats_pitching_batters_faced": box_score_response["teams"]["away"]["teamStats"]["pitching"]["battersFaced"],
    #        "away_team_stats_pitching_outs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["outs"],
    #        "away_team_stats_pitching_complete_games": box_score_response["teams"]["away"]["teamStats"]["pitching"]["completeGames"],
    #        "away_team_stats_pitching_shutouts": box_score_response["teams"]["away"]["teamStats"]["pitching"]["shutouts"],
    #        "away_team_stats_pitching_balls": box_score_response["teams"]["away"]["teamStats"]["pitching"]["balls"],
    #        "away_team_stats_pitching_strikes": box_score_response["teams"]["away"]["teamStats"]["pitching"]["strikes"],
    #        "away_team_stats_pitching_strikePercentage": box_score_response["teams"]["away"]["teamStats"]["pitching"]["strikePercentage"],
    #        "away_team_stats_pitching_hit_batsmen": box_score_response["teams"]["away"]["teamStats"]["pitching"]["hitBatsman"],
    #        "away_team_stats_pitching_balks": box_score_response["teams"]["away"]["teamStats"]["pitching"]["balks"],
    #        "away_team_stats_pitching_wild_pitches": box_score_response["teams"]["away"]["teamStats"]["pitching"]["wildPitches"],
    #        "away_team_stats_pitching_pickoffs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["pickoffs"],
    #        "away_team_stats_pitching_ground_outs_to_airouts": box_score_response["teams"]["away"]["teamStats"]["pitching"]["groundOutsToAirouts"],
    #        "away_team_stats_pitching_rbi": box_score_response["teams"]["away"]["teamStats"]["pitching"]["rbi"],
    #        "away_team_stats_pitching_pitches_per_inning": box_score_response["teams"]["away"]["teamStats"]["pitching"]["pitchesPerInning"],
    #        "away_team_stats_pitching_runs_scored_per_9": box_score_response["teams"]["away"]["teamStats"]["pitching"]["runsScoredPer9"],
    #        "away_team_stats_pitching_home_runs_per_9": box_score_response["teams"]["away"]["teamStats"]["pitching"]["homeRunsPer9"],
    #        "away_team_stats_pitching_inherited_runners": box_score_response["teams"]["away"]["teamStats"]["pitching"]["inheritedRunners"],
    #        "away_team_stats_pitching_inherited_runners_scored": box_score_response["teams"]["away"]["teamStats"]["pitching"]["inheritedRunnersScored"],
    #        "away_team_stats_pitching_catchers_interference": box_score_response["teams"]["away"]["teamStats"]["pitching"]["catchersInterference"],
    #        "away_team_stats_pitching_sac_bunts": box_score_response["teams"]["away"]["teamStats"]["pitching"]["sacBunts"],
    #        "away_team_stats_pitching_sac_flies": box_score_response["teams"]["away"]["teamStats"]["pitching"]["sacFlies"],
    #        "away_team_stats_pitching_passed_ball": box_score_response["teams"]["away"]["teamStats"]["pitching"]["passedBall"],
    #        "away_team_stats_pitching_pop_outs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["popOuts"],
    #        "away_team_stats_pitching_line_outs": box_score_response["teams"]["away"]["teamStats"]["pitching"]["lineOuts"],
    #        "away_team_stats_fielding_caught_stealing": box_score_response["teams"]["away"]["teamStats"]["fielding"]["caughtStealing"],
    #        "away_team_stats_fielding_stolen_bases": box_score_response["teams"]["away"]["teamStats"]["fielding"]["stolenBases"],
    #        "away_team_stats_fielding_stolen_base_percentage": box_score_response["teams"]["away"]["teamStats"]["fielding"]["stolenBasePercentage"],
    #        "away_team_stats_fielding_assists": box_score_response["teams"]["away"]["teamStats"]["fielding"]["assists"],
    #        "away_team_stats_fielding_put_outs": box_score_response["teams"]["away"]["teamStats"]["fielding"]["putOuts"],
    #        "away_team_stats_fielding_errors": box_score_response["teams"]["away"]["teamStats"]["fielding"]["errors"],
    #        "away_team_stats_fielding_chances": box_score_response["teams"]["away"]["teamStats"]["fielding"]["chances"],
    #        "away_team_stats_fielding_passed_ball": box_score_response["teams"]["away"]["teamStats"]["fielding"]["passedBall"],
    #        "away_team_stats_fielding_pickoffs": box_score_response["teams"]["away"]["teamStats"]["fielding"]["pickoffs"],
    #        "away_team_stats_fielding_caught_stealing": box_score_response["teams"]["away"]["players"],
#
#
#
    #        
#
#
    #    }
    #    print(box_score)

    # Initialize row collectors
    team_rows = []
    team_stats_batting_rows = []
    team_stats_pitching_rows = []
    team_stats_fielding_rows = []
    player_stats_rows = []
    player_season_stats_rows = []
    player_game_stats_batting = []
    player_game_stats_pitching = []
    player_game_stats_fielding = []

    for game in csv_data:
        game_pk = game["game_pk"]
        box_score_response = get_box_score_from_game_pk(game_pk, season)

        for team_side in ["away", "home"]:
            team = box_score_response["teams"][team_side]["team"]
            team_name = team["teamName"]
            team_id = team["id"]
            record = team.get("record", {})
            league_record = record.get("leagueRecord", {})

            # ==== TEAM INFO ====
            #team_rows.append({
            #    "gamePk": game_pk,
            #    "teamSide": team_side,
            #    "teamId": team["id"],
            #    "teamName": team_name,
            #    "season": team["season"],
            #    "spring_league_id": team["springLeague"]["id"],
            #    "spring_league_name": team["springLeague"]["name"],
            #    "spring_league_abbreviation": team["springLeague"]["abbreviation"],
            #    "all_star_status": team["allStarStatus"],
            #    "venue_id": team["venue"]["id"],
            #    "venue_name": team["venue"]["name"],
            #    "team_code": team["teamCode"],
            #    "file_code": team["fileCode"],
            #    "abbreviation": team["abbreviation"],
            #    "location_name": team["locationName"],
            #    "first_year_of_play": team["firstYearOfPlay"],
            #    "league_id": team["league"]["id"],
            #    "league_name": team["league"]["name"],
            #    "sport_id": team["sport"]["id"],
            #    "sport_name": team["sport"]["name"],
            #    "short_name": team["shortName"],
            #    "record_games_played": record.get("gamesPlayed"),
            #    "record_wild_card_games_back": record.get("wildCardGamesBack"),
            #    "record_league_games_back": record.get("leagueGamesBack"),
            #    "record_spring_league_games_back": record.get("springLeagueGamesBack"),
            #    "record_sport_games_back": record.get("sportGamesBack"),
            #    "record_division_games_back": record.get("divisionGamesBack"),
            #    "record_conference_games_back": record.get("conferenceGamesBack"),
            #    "record_league_record_wins": league_record.get("wins"),
            #    "record_league_record_losses": league_record.get("losses"),
            #    "record_league_record_ties": league_record.get("ties"),
            #    "record_league_record_pct": league_record.get("pct"),
            #    "record_division_leader": record.get("divisionLeader"),
            #    "record_wins": record.get("wins"),
            #    "record_losses": record.get("losses"),
            #    "record_winning_percentage": record.get("winningPercentage"),
            #    "franchise_name": team["franchiseName"],
            #    "club_name": team["clubName"],
            #    "active": team["active"]
            #})

            # ==== TEAM STATS ====
            #stats_group = {
            #    "batting": team_stats_batting_rows,
            #    "pitching": team_stats_pitching_rows,
            #    "fielding": team_stats_fielding_rows
            #}
#
            #for stat_type, target_list in stats_group.items():
            #    stats = box_score_response["teams"][team_side]["teamStats"].get(stat_type, {})
            #    row = {
            #        "gamePk": game_pk,
            #        "teamSide": team_side,
            #        "teamName": team_name
            #    }
            #    row.update(stats)
            #    target_list.append(row)

            # ==== PLAYER DATA ====
            players = box_score_response["teams"][team_side]["players"]

            for player_id, player in players.items():
                person = player.get("person", {})
                position = player.get("position", {})
                status = player.get("status", {})
                game_status = player.get("gameStatus", {})

                #player_stats_rows.append({
                #    "gamePk": game_pk,
                #    "teamSide": team_side,
                #    "teamName": team_name,
                #    "playerId": player_id,
                #    "personId": person.get("id"),
                #    "fullName": person.get("fullName"),
                #    "boxscoreName": person.get("boxscoreName"),
                #    "jerseyNumber": player.get("jerseyNumber", ""),
                #    "position": position.get("name", ""),
                #    "position_abbr": position.get("abbreviation", ""),
                #    "status_code": status.get("code", ""),
                #    "status_description": status.get("description", ""),
                #    "isCurrentBatter": game_status.get("isCurrentBatter", False),
                #    "isCurrentPitcher": game_status.get("isCurrentPitcher", False),
                #    "isOnBench": game_status.get("isOnBench", False),
                #    "isSubstitute": game_status.get("isSubstitute", False)
                #})

                # ==== PLAYER SEASON STATS ====
                #season_stats = player.get("seasonStats", {})
                #for category, stat_dict in season_stats.items():
                #    row = {
                #        "gamePk": game_pk,
                #        "teamSide": team_side,
                #        "teamName": team_name,
                #        "playerId": player_id,
                #        "personId": person.get("id"),
                #        "category": category
                #    }
                #    row.update(stat_dict)
                #    player_season_stats_rows.append(row)
                #
                            # ==== PLAYER GAME STATS (BOX SCORE) ====
                # ==== PLAYER GAME STATS (BOX SCORE) ====
                game_stats = player.get("stats", {})
                for category, stat_dict in game_stats.items():
                    row = {
                        "gamePk": game_pk,
                        "teamSide": team_side,
                        "teamName": team_name,
                        "teamId": team_id,
                        "playerId": player_id,
                        "personId": person.get("id"),
                    }
                    row.update(stat_dict)

                    if category == "batting":
                        player_game_stats_batting.append(row)
                    elif category == "pitching":
                        player_game_stats_pitching.append(row)
                    elif category == "fielding":
                        player_game_stats_fielding.append(row)



    # ==== WRITE TO CSV FILES ====
    def write_csv(filename, rows):
        df = pd.DataFrame(rows)
        df.to_csv(f"./mlb_stats/{filename}_{season}.csv", index=False)
        print(f"Saved {filename}_{season}.csv with {len(df)} rows")

    #write_csv("team", team_rows)
    #write_csv("team_stats_batting", team_stats_batting_rows)
    #write_csv("team_stats_pitching", team_stats_pitching_rows)
    #write_csv("team_stats_fielding", team_stats_fielding_rows)
    #write_csv("player_stats", player_stats_rows)
    #write_csv("player_season_stats", player_season_stats_rows)
    write_csv("player_game_stats_batting", player_game_stats_batting)
    write_csv("player_game_stats_pitching", player_game_stats_pitching)
    write_csv("player_game_stats_fielding", player_game_stats_fielding)


def get_active_mlb_players_for_season(season: str, output_path: str = None):
    url = "https://statsapi.mlb.com/api/v1/sports/1/players"
    params = {
        "season": season,
        "hydrate": "team,position"
    }

    response = requests.get(url, params=params)
    response.raise_for_status()

    data = response.json()
    players = data.get("people", [])

    result = []
    for player in players:
        result.append({
            "id": player.get("id"),
            "fullName": player.get("fullName"),
            "firstName": player.get("firstName"),
            "lastName": player.get("lastName"),
            "primaryNumber": player.get("primaryNumber"),
            "birthDate": player.get("birthDate"),
            "currentAge": player.get("currentAge"),
            "birthCity": player.get("birthCity"),
            "birthStateProvince": player.get("birthStateProvince"),
            "birthCountry": player.get("birthCountry"),
            "height": player.get("height"),
            "weight": player.get("weight"),
            "active": player.get("active"),
            "mlbDebutDate": player.get("mlbDebutDate"),
            "draftYear": player.get("draftYear"),
            "teamId": player.get("currentTeam", {}).get("id"),
            "teamName": player.get("currentTeam", {}).get("name"),
            "teamLink": player.get("currentTeam", {}).get("link"),
            "primaryPositionCode": player.get("primaryPosition", {}).get("code"),
            "primaryPositionName": player.get("primaryPosition", {}).get("name"),
            "positionType": player.get("primaryPosition", {}).get("type"),
            "batSideCode": player.get("batSide", {}).get("code"),
            "batSideDescription": player.get("batSide", {}).get("description"),
            "pitchHandCode": player.get("pitchHand", {}).get("code"),
            "pitchHandDescription": player.get("pitchHand", {}).get("description"),
            "boxscoreName": player.get("boxscoreName"),
            "nickName": player.get("nickName"),
            "strikeZoneTop": player.get("strikeZoneTop"),
            "strikeZoneBottom": player.get("strikeZoneBottom"),
            "nameSlug": player.get("nameSlug")
        })

    # Save to CSV if output_path is given
    if not output_path:
        output_path = f"./mlb_stats/active_mlb_players_{season}.csv"

    with open(output_path, mode="w", newline="", encoding="utf-8") as f:
        writer = csv.DictWriter(f, fieldnames=result[0].keys())
        writer.writeheader()
        writer.writerows(result)

    print(f"✅ Saved {len(result)} players to {output_path}")
    return result


# Example call (uncomment to run)
#players_2023 = get_active_mlb_players_for_season("2023")
#print(players_2023[:2])  # Print first 2 players as a preview
get_schedule_save_box_scores("2023")
#get_schedule_save_play_by_play("2023")
#get_schedule_save_mlb_games("2023")