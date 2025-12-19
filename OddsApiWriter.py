import csv
from pathlib import Path
import requests
# Replace this with your full API response object

from BaseballData import get_existing_game_pks
import csv
from typing import List, Dict

def write_h2h_odds_to_csv(sport, season):

    sport_var = "baseball_mlb"
    output_file = f"./mlb_stats/mlb_h2h_odds_{season}.csv"

    if (sport == "NBA"):
        sport_var = "basketball_nba"
        output_file = f"./juicystats/nba_h2h_odds_{season}.csv"

    response = requests.get(f"https://api.the-odds-api.com/v4/sports/{sport_var}/odds?regions=us,us2&apiKey=f8d80068fa8107d46ae62e3a3f15092f")
    api_response = response.json()
    
    fieldnames = [
        'game_id',
        'sport_key',
        'sport_title',
        'commence_time',
        'home_team',
        'away_team',
        'bookmaker_key',
        'bookmaker_title',
        'bookmaker_last_update',
        'market_key',
        'market_last_update',
        'outcome_name',
        'outcome_price'
    ]

    with open(output_file, mode='w', newline='', encoding='utf-8') as csvfile:
        writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
        writer.writeheader()

        for game in api_response:
            game_id = game.get('id')
            sport_key = game.get('sport_key')
            sport_title = game.get('sport_title')
            commence_time = game.get('commence_time')
            home_team = game.get('home_team')
            away_team = game.get('away_team')

            for bookmaker in game.get('bookmakers', []):
                bookmaker_key = bookmaker.get('key')
                bookmaker_title = bookmaker.get('title')
                bookmaker_last_update = bookmaker.get('last_update')

                for market in bookmaker.get('markets', []):
                    market_key = market.get('key')
                    market_last_update = market.get('last_update')

                    for outcome in market.get('outcomes', []):
                        writer.writerow({
                            'game_id': game_id,
                            'sport_key': sport_key,
                            'sport_title': sport_title,
                            'commence_time': commence_time,
                            'home_team': home_team,
                            'away_team': away_team,
                            'bookmaker_key': bookmaker_key,
                            'bookmaker_title': bookmaker_title,
                            'bookmaker_last_update': bookmaker_last_update,
                            'market_key': market_key,
                            'market_last_update': market_last_update,
                            'outcome_name': outcome.get('name'),
                            'outcome_price': outcome.get('price')
                        })

    print(f"✅ Odds data written to: {output_file}")


def write_player_props(eventId, player_prop, season):
    response = requests.get(f"https://api.the-odds-api.com/v4/sports/baseball_mlb/events/{eventId}/odds/?regions=us,us2&markets={player_prop}&apiKey=f8d80068fa8107d46ae62e3a3f15092f")

    if response.status_code == 404:
        print("Error 404: Event not found.")
        return
    elif response.status_code == 200:
        api_response = response.json()
        print("Success:", api_response)
    else:
        print(f"Unexpected status code: {response.status_code}")
        return
    # Flatten the JSON data
    rows = []
    for bookmaker in api_response["bookmakers"]:
        book_title = bookmaker["title"]
        for market in bookmaker["markets"]:
            market_key = market["key"]
            for outcome in market["outcomes"]:
                rows.append({
                    "game_id": api_response["id"],
                    "commence_time": api_response["commence_time"],
                    "home_team": api_response["home_team"],
                    "away_team": api_response["away_team"],
                    "bookmaker": book_title,
                    "market": market_key,
                    "player": outcome["description"],
                    "line": outcome["point"],
                    "bet_type": outcome["name"],
                    "decimal_odds": outcome["price"]
                })

    # Write the flattened data to a CSV file
    if len(rows) == 0:
        return
    csv_path = Path(f"mlb_player_props_{season}.csv")
    with csv_path.open("w", newline="", encoding="utf-8") as csvfile:
        writer = csv.DictWriter(csvfile, fieldnames=rows[0].keys())
        writer.writeheader()
        writer.writerows(rows)

    print(f"CSV file written to {csv_path.resolve()}")

def call_write_player_props(season, table, prop):
    new_event_ids = get_existing_game_pks(table)
    for event_id in new_event_ids:
        print(event_id["game_id"])
        write_player_props(event_id["game_id"], prop, season)
        


write_h2h_odds_to_csv("NBA", "2025_26")
#call_write_player_props("2025", "odds_api_h2h_2025", "batter_hits")