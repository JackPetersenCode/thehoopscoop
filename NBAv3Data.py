import asyncio
import json
import csv

import aiofiles
import aiohttp

from nba_api.stats.endpoints import boxscoretraditionalv3
from nba_api.stats.library.parameters import EndPeriod, EndRange, RangeType, StartPeriod, StartRange




boxScoreArrayTraditional = []
async def readLeagueGamesTraditional():
    """Fetches table length and reads game data asynchronously."""
    URL = 'http://localhost:5190/api/tablelength/box/box_score_traditional_2024_25'
    count = 0
    async with aiohttp.ClientSession() as session:
        try:
            async with session.get(URL) as response:
                if response.status == 200:
                    data = await response.json()
                    count = data['count']
                else:
                    print(f"❌ Failed to fetch data. Status: {response.status}")
                    return

            # Read the JSON file asynchronously
            file_path = './juicystats/league_games_2024_25.json'
            async with aiofiles.open(file_path, mode='r', encoding='utf-8') as f:
                games_content = await f.read()
                games = json.loads(games_content)


        except Exception as e:
            print(f"❌ Error in readLeagueGames(): {e}")
    idList = []
    end = len(games["resultSets"][0]["rowSet"])
    start = int(count) * 2
    print(start)
    print(end)
    for i in range (367, end):
        print(i)
        if games["resultSets"][0]["rowSet"][i][4] in idList or games["resultSets"][0]["rowSet"][i][4] is None:
            continue
        idList.append(games["resultSets"][0]["rowSet"][i][4])
        await boxScoreTraditional(games["resultSets"][0]["rowSet"][i][4])
        #box = await boxScoreTraditional(games["resultSets"][0]["rowSet"][i][4])
        #game_ids = [
        #    "0022401181",
        #    "0022401169",
        #    "0022401178",
        #    "0022401171",
        #    "0022401188",
        #    "0022401174",
        #    "0022401186",
        #    "0022401192",
        #    "0022401183",
        #    "0022401199",
        #    "0022401173",
        #    "0022401177",
        #    "0022401187",
        #    "0022401185",
        #    "0022401198",
        #    "0022401175",
        #    "0022401176",
        #    "0022401184",
        #    "0022401195",
        #    "0022401189",
        #    "0022401172",
        #    "0022401194",
        #    "0022401196",
        #    "0022401180",
        #    "0022401167",
        #    "0022401193",
        #    "0022401191",
        #    "0022401179",
        #    "0022401190",
        #    "0022401200",
        #    "0022401197",
        #    "0022401161",
        #    "0022401170",
        #    "0022401182"
        #]
        #boxScoreArrayTraditional.append(box)
    # Closing file
    f.close()

async def boxScoreTraditional(id):

    response = boxscoretraditionalv3.BoxScoreTraditionalV3(
        game_id = id,
        end_period=EndPeriod.default,
        end_range=EndRange.default,
        range_type=RangeType.default,
        start_period=StartPeriod.default,
        start_range=StartRange.default,
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True,
    )
    
    content = json.loads(response.get_json())

    jsonContent = json.dumps(content)
    newContent = json.loads(jsonContent)  # Convert JSON string to dict
    # Extract game ID
    game_id = newContent['boxScoreTraditional']['gameId']
    # Output file names
    all_games_file = 'box_score_traditional_2024_25'
    home_file = 'home_box_score_traditional_2024_25.csv'
    away_file = 'away_box_score_traditional_2024_25.csv'
    
    # Define column headers
    headers = [
        'gameId', 'teamType', 'teamId', 'teamCity', 'teamName',
        'personId', 'firstName', 'familyName', 'nameI', 'playerSlug',
        'position', 'comment', 'jerseyNum', 'minutes', 'fieldGoalsMade',
        'fieldGoalsAttempted', 'fieldGoalsPercentage', 'threePointersMade',
        'threePointersAttempted', 'threePointersPercentage', 'freeThrowsMade',
        'freeThrowsAttempted', 'freeThrowsPercentage', 'reboundsOffensive',
        'reboundsDefensive', 'reboundsTotal', 'assists', 'steals',
        'blocks', 'turnovers', 'foulsPersonal', 'points', 'plusMinusPoints'
    ]

    def flatten_player(player, team, teamType):
        stats = player['statistics']

        return {
            'gameId': game_id,
            'teamType': teamType,
            'teamId': team['teamId'],
            'teamCity': team['teamCity'],
            'teamName': team['teamName'],
            'personId': player['personId'],
            'firstName': player['firstName'],
            'familyName': player['familyName'],
            'nameI': player['nameI'],
            'playerSlug': player['playerSlug'],
            'position': player['position'],
            'comment': player['comment'],
            'jerseyNum': player['jerseyNum'],
            'minutes': stats.get('minutes', ''),
            'fieldGoalsMade': stats.get('fieldGoalsMade', 0),
            'fieldGoalsAttempted': stats.get('fieldGoalsAttempted', 0),
            'fieldGoalsPercentage': stats.get('fieldGoalsPercentage', 0),
            'threePointersMade': stats.get('threePointersMade', 0),
            'threePointersAttempted': stats.get('threePointersAttempted', 0),
            'threePointersPercentage': stats.get('threePointersPercentage', 0),
            'freeThrowsMade': stats.get('freeThrowsMade', 0),
            'freeThrowsAttempted': stats.get('freeThrowsAttempted', 0),
            'freeThrowsPercentage': stats.get('freeThrowsPercentage', 0),
            'reboundsOffensive': stats.get('reboundsOffensive', 0),
            'reboundsDefensive': stats.get('reboundsDefensive', 0),
            'reboundsTotal': stats.get('reboundsTotal', 0),
            'assists': stats.get('assists', 0),
            'steals': stats.get('steals', 0),
            'blocks': stats.get('blocks', 0),
            'turnovers': stats.get('turnovers', 0),
            'foulsPersonal': stats.get('foulsPersonal', 0),
            'points': stats.get('points', 0),
            'plusMinusPoints': stats.get('plusMinusPoints', 0)
        }

    try:
        with open('./juicystats/box_score_traditional_2024_25_whole_season.csv', 'a', encoding='UTF8', newline='') as f:
            writer = csv.writer(f)
            writer.writerow(headers)
            home_team_data = newContent['boxScoreTraditional']['homeTeam']
            away_team_data = newContent['boxScoreTraditional']['awayTeam']
            for player in home_team_data['players']:
                row = flatten_player(player, home_team_data, 'homeTeam')
                #print(row)
                finalRow = [row[col] for col in headers]

                writer.writerow(finalRow)  
            for player in away_team_data['players']:
                row = flatten_player(player, away_team_data, 'awayTeam')
                #print(row)
                finalRow = [row[col] for col in headers]

                writer.writerow(finalRow) 
            f.close()
    except ValueError:
        print("VALUE ERROR?!?!?!!?!!??!?!??!??!?!!?")
    
    
# Number of retries for each API call
MAX_RETRIES = 3
RETRY_DELAY = 5  # Seconds to wait before retrying on failure

async def safe_run(func, *args):
    """Runs an async function with retries and error handling, ensuring sequential execution."""
    for attempt in range(3):  # Retry up to 3 times
        try:
            await func(*args)  # Ensure the function fully completes before moving on
            print(f"✅ {func.__name__} executed successfully.")
            return  # Exit the function after success
        except Exception as e:
            print(f"❌ Error in {func.__name__}: {e}")
            if attempt < 2:
                print(f"Retrying {func.__name__} in 5 seconds...")
                await asyncio.sleep(5)
            else:
                print(f"{func.__name__} failed after 3 retries.")


    
async def run_tasks():
    """Executes NBA API tasks sequentially with error handling and rate limiting."""
    
    # API calls executed one after the other
    #await safe_run(leaguegames)
    #await safe_run(shotchartdetailfunction)
    #await safe_run(allassists)
    #await safe_run(assiststracker)
    #await safe_run(playergamelogfunction, '153', '0021700807')  
    #await safe_run(readLeagueGames)
    #await safe_run(leaguehustlestats)
#
    #numPlayers = ['2','3','4','5']
    #boxTypes = ['Base', 'Advanced', 'Four Factors', 'Misc', 'Scoring', 'Opponent']
    #for num in numPlayers:
    #    for boxType in boxTypes:
    #        await safe_run(leaguedashlineupsfunction, num, boxType, '2024_25')
#
    #await safe_run(leaguedashoppptshotfunction)
    #await safe_run(leaguedashplayerclutchfunction)
    #await safe_run(leaguedashplayerptshotfunction)
    #await safe_run(leaguedashplayershotlocationsfunction)
    await readLeagueGamesTraditional()
    #await safe_run(leagueDashPlayerStatsFunction)
    #await safe_run(getPlayerIds)  # Runs before player stats function
    #await safe_run(readBoxScoreSummary)
    #await safe_run(readLeagueGamesScoring)
    #await safe_run(writeNBAplayers)
    #await safe_run(getOdds)
    #await safe_run(readLeagueMisc)
    #await safe_run(defenseHub)

    print("✅ All tasks completed successfully!")

# Run the async event loop
if __name__ == "__main__":
    asyncio.run(run_tasks())