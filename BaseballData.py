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
    schedule_data = get_mlb_schedule(2023)
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



def get_box_score_from_game_pk(gamePk, season):
    # Define the base URL dynamically using gamePk
    BASE_URL = f"http://statsapi.mlb.com/api/v1/game/{gamePk}/boxscore"
    
    # Make the API request
    response = requests.get(BASE_URL)
    
    # Check if the request was successful
    if response.status_code == 200:
        return response.json()  # Return the parsed JSON response
    else:
        print(f"Error: Unable to fetch data for gamePk {gamePk} (Status Code: {response.status_code})")
        return None  # Return None if request fails


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
    player_game_stats_rows = []

    for game in csv_data:
        game_pk = game["game_pk"]
        box_score_response = get_box_score_from_game_pk(game_pk, season)

        for team_side in ["away", "home"]:
            team = box_score_response["teams"][team_side]["team"]
            team_name = team["teamName"]
            record = team.get("record", {})
            league_record = record.get("leagueRecord", {})

            # ==== TEAM INFO ====
            team_rows.append({
                "gamePk": game_pk,
                "teamSide": team_side,
                "teamId": team["id"],
                "teamName": team_name,
                "season": team["season"],
                "spring_league_id": team["springLeague"]["id"],
                "spring_league_name": team["springLeague"]["name"],
                "spring_league_abbreviation": team["springLeague"]["abbreviation"],
                "all_star_status": team["allStarStatus"],
                "venue_id": team["venue"]["id"],
                "venue_name": team["venue"]["name"],
                "team_code": team["teamCode"],
                "file_code": team["fileCode"],
                "abbreviation": team["abbreviation"],
                "location_name": team["locationName"],
                "first_year_of_play": team["firstYearOfPlay"],
                "league_id": team["league"]["id"],
                "league_name": team["league"]["name"],
                "sport_id": team["sport"]["id"],
                "sport_name": team["sport"]["name"],
                "short_name": team["shortName"],
                "record_games_played": record.get("gamesPlayed"),
                "record_wild_card_games_back": record.get("wildCardGamesBack"),
                "record_league_games_back": record.get("leagueGamesBack"),
                "record_spring_league_games_back": record.get("springLeagueGamesBack"),
                "record_sport_games_back": record.get("sportGamesBack"),
                "record_division_games_back": record.get("divisionGamesBack"),
                "record_conference_games_back": record.get("conferenceGamesBack"),
                "record_league_record_wins": league_record.get("wins"),
                "record_league_record_losses": league_record.get("losses"),
                "record_league_record_ties": league_record.get("ties"),
                "record_league_record_pct": league_record.get("pct"),
                "record_division_leader": record.get("divisionLeader"),
                "record_wins": record.get("wins"),
                "record_losses": record.get("losses"),
                "record_winning_percentage": record.get("winningPercentage"),
                "franchise_name": team["franchiseName"],
                "club_name": team["clubName"],
                "active": team["active"]
            })

            # ==== TEAM STATS ====
            stats_group = {
                "batting": team_stats_batting_rows,
                "pitching": team_stats_pitching_rows,
                "fielding": team_stats_fielding_rows
            }

            for stat_type, target_list in stats_group.items():
                stats = box_score_response["teams"][team_side]["teamStats"].get(stat_type, {})
                row = {
                    "gamePk": game_pk,
                    "teamSide": team_side,
                    "teamName": team_name
                }
                row.update(stats)
                target_list.append(row)

            # ==== PLAYER DATA ====
            players = box_score_response["teams"][team_side]["players"]

            for player_id, player in players.items():
                person = player.get("person", {})
                position = player.get("position", {})
                status = player.get("status", {})
                game_status = player.get("gameStatus", {})

                player_stats_rows.append({
                    "gamePk": game_pk,
                    "teamSide": team_side,
                    "teamName": team_name,
                    "playerId": player_id,
                    "personId": person.get("id"),
                    "fullName": person.get("fullName"),
                    "boxscoreName": person.get("boxscoreName"),
                    "jerseyNumber": player.get("jerseyNumber", ""),
                    "position": position.get("name", ""),
                    "position_abbr": position.get("abbreviation", ""),
                    "status_code": status.get("code", ""),
                    "status_description": status.get("description", ""),
                    "isCurrentBatter": game_status.get("isCurrentBatter", False),
                    "isCurrentPitcher": game_status.get("isCurrentPitcher", False),
                    "isOnBench": game_status.get("isOnBench", False),
                    "isSubstitute": game_status.get("isSubstitute", False)
                })

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
                game_stats = player.get("stats", {})
                for category, stat_dict in game_stats.items():
                    row = {
                        "gamePk": game_pk,
                        "teamSide": team_side,
                        "teamName": team_name,
                        "playerId": player_id,
                        "personId": person.get("id"),
                        "category": category
                    }
                    row.update(stat_dict)
                    player_game_stats_rows.append(row)


    # ==== WRITE TO CSV FILES ====
    def write_csv(filename, rows):
        df = pd.DataFrame(rows)
        df.to_csv(f"./mlb_stats/{filename}_{season}.csv", index=False)
        print(f"Saved {filename}_{season}.csv with {len(df)} rows")

    write_csv("team", team_rows)
    write_csv("team_stats_batting", team_stats_batting_rows)
    write_csv("team_stats_pitching", team_stats_pitching_rows)
    write_csv("team_stats_fielding", team_stats_fielding_rows)
    write_csv("player_stats", player_stats_rows)
    write_csv("player_season_stats", player_season_stats_rows)


get_schedule_save_box_scores("2023")

# Mock data for demonstration (you should replace this with actual API response)
mock_schedule_data = {
    "dates": [
        {
            "games": [
                {
                    "gamePk": 718780,
                    "gameGuid": "28889d0d-b745-4054-b0d3-5fc4cfa06df9",
                    "gameType": "R",
                    "season": "2023",
                    "gameDate": "2023-03-30T17:05:00Z",
                    "officialDate": "2023-03-30",
                    "status": {
                        "abstractGameState": "Final",
                        "codedGameState": "F",
                        "detailedState": "Final",
                        "statusCode": "F",
                        "startTimeTBD": False
                    },
                    "teams": {
                        "away": {
                            "leagueRecord": {"wins": 1, "losses": 0, "pct": "1.000"},
                            "score": 7,
                            "team": {"id": 144, "name": "Atlanta Braves"},
                            "isWinner": True
                        },
                        "home": {
                            "leagueRecord": {"wins": 0, "losses": 1, "pct": ".000"},
                            "score": 2,
                            "team": {"id": 120, "name": "Washington Nationals"},
                            "isWinner": False
                        }
                    },
                    "venue": {"id": 3309, "name": "Nationals Park"},
                    "isTie": False,
                    "gameNumber": 1,
                    "doubleHeader": "N",
                    "dayNight": "day",
                    "description": "Nationals home opener",
                    "scheduledInnings": 9,
                    "gamesInSeries": 3,
                    "seriesGameNumber": 1,
                    "seriesDescription": "Regular Season",
                    "ifNecessary": "N",
                    "ifNecessaryDescription": "Normal Game"
                }
            ]
        }
    ]
}