import time
import csv
import aiofiles
import schedule
import time
import os
from email import header
import json
import asyncio
from dotenv import load_dotenv
from types import SimpleNamespace
from unittest import result
from urllib import response
from xml.etree.ElementTree import tostring
from nba_api.stats.endpoints import boxscoremiscv2, boxscorescoringv2, boxscorescoringv3, boxscoresummaryv2, boxscoretraditionalv3, defensehub, playercareerstats, leaguedashplayerstats, boxscoretraditionalv2, leaguedashplayershotlocations, leaguedashplayerptshot, leaguedashplayerclutch, assistleaders, assisttracker, leaguegamelog, leaguehustlestatsplayer, leaguedashlineups, leaguedashoppptshot, shotchartdetail, alltimeleadersgrids, boxscoreadvancedv2, playergamelog
from nba_api.stats.library.parameters import LeagueID, PerModeSimple, PlayerOrTeam, Season, SeasonType
from nba_api.stats.library.parameters import ConferenceNullable, DivisionSimpleNullable, PlayerScope, GameScopeDetailed, GameScopeSimpleNullable, LastNGamesNullable, LeagueIDNullable, LocationNullable, MonthNullable, OutcomeNullable, PerModeSimpleNullable, PlayerExperienceNullable, PlayerPositionAbbreviationNullable, SeasonNullable, SeasonSegmentNullable, SeasonTypeAllStarNullable, StarterBenchNullable, DivisionNullable
from nba_api.stats.library.parameters import EndPeriod, EndRange, RangeType, StartPeriod, StartRange
from nba_api.stats.library.parameters import Direction, LeagueID, PlayerOrTeamAbbreviation, Season, SeasonTypeAllStar, Sorter
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import PerModeTime, Season, SeasonTypeAllStar, ConferenceNullable, DivisionSimpleNullable, LeagueIDNullable, LocationNullable, MonthNullable, OutcomeNullable, PlayerExperienceNullable, PlayerPositionNullable, SeasonSegmentNullable, DivisionNullable
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import PerModeTime, Season, SeasonTypeAllStar, ConferenceNullable, DivisionSimpleNullable, LeagueIDNullable, LocationNullable, MonthNullable, OutcomeNullable, PlayerExperienceNullable, PlayerPositionNullable, SeasonSegmentNullable, DivisionNullable
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import GroupQuantity, LastNGames, MeasureTypeDetailedDefense, Month, PaceAdjust, PerModeDetailed, Period, PlusMinus, Rank, Season, SeasonTypeAllStar, ConferenceNullable, DivisionSimpleNullable, GameSegmentNullable, LeagueIDNullable, LocationNullable, OutcomeNullable, SeasonSegmentNullable, ShotClockRangeNullable, DivisionNullable
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import LeagueID, PerModeSimple, Season, SeasonTypeAllStar, ConferenceNullable, DivisionNullable, GameSegmentNullable, LastNGamesNullable, LocationNullable, MonthNullable, OutcomeNullable, PeriodNullable, SeasonSegmentNullable, ShotClockRangeNullable
from nba_api.stats.library.parameters import SeasonAll
import json
from nba_api.stats.static import players
#import psycopg2
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import AheadBehind, ClutchTime, LastNGames, MeasureTypeDetailedDefense, Month, PaceAdjust, PerModeDetailed, Period, PlusMinus, PointDiff, Rank, Season, SeasonTypeAllStar, ConferenceNullable, DivisionSimpleNullable, GameScopeSimpleNullable, GameSegmentNullable, LeagueIDNullable, LocationNullable, OutcomeNullable, PlayerExperienceNullable, PlayerPositionAbbreviationNullable, SeasonSegmentNullable, ShotClockRangeNullable, StarterBenchNullable, DivisionNullable
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import LeagueID, PerModeSimple, Season, SeasonTypeAllStar, ConferenceNullable, DivisionNullable, GameSegmentNullable, LastNGamesNullable, LocationNullable, MonthNullable, OutcomeNullable, PeriodNullable, PlayerExperienceNullable, PlayerPositionNullable, SeasonSegmentNullable, ShotClockRangeNullable, StarterBenchNullable
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import DistanceRange, LastNGames, MeasureTypeSimple, Month, PaceAdjust, PerModeDetailed, Period, PlusMinus, Rank, Season, SeasonTypeAllStar, ConferenceNullable, DivisionSimpleNullable, GameScopeSimpleNullable, GameSegmentNullable, LeagueIDNullable, LocationNullable, OutcomeNullable, PlayerExperienceNullable, PlayerPositionAbbreviationNullable, SeasonSegmentNullable, ShotClockRangeNullable, StarterBenchNullable, DivisionNullable
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import DistanceRange, ContextMeasureSimple, LastNGames, MeasureTypeSimple, Month, PaceAdjust, PerModeDetailed, Period, PlusMinus, Rank, Season, SeasonTypeAllStar, ConferenceNullable, DivisionSimpleNullable, GameScopeSimpleNullable, GameSegmentNullable, LeagueIDNullable, LocationNullable, OutcomeNullable, PlayerExperienceNullable, PlayerPositionAbbreviationNullable, SeasonSegmentNullable, ShotClockRangeNullable, StarterBenchNullable, DivisionNullable
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import EndPeriod, EndRange, RangeType, StartPeriod, StartRange
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import LastNGames, MeasureTypeDetailedDefense, Month, PaceAdjust, PerModeDetailed, Period, PlusMinus, Rank, Season, SeasonTypeAllStar, ConferenceNullable, DivisionSimpleNullable, GameScopeSimpleNullable, GameSegmentNullable, LeagueIDNullable, LocationNullable, OutcomeNullable, PlayerExperienceNullable, PlayerPositionAbbreviationNullable, SeasonSegmentNullable, ShotClockRangeNullable, StarterBenchNullable, DivisionNullable
from nba_api.stats.endpoints._base import Endpoint
from nba_api.stats.library.http import NBAStatsHTTP
from nba_api.stats.library.parameters import PerMode36, LeagueIDNullable
import requests
import aiocron
from datetime import datetime, timedelta
import aiohttp

#from postFunctions import post_league_games_by_season
# Load environment variables from .env file
load_dotenv()


# Ensure 'juicystats' folder exists
JUICYSTATS_PATH = os.path.join(os.getcwd(), "juicystats")  # Gets absolute path

if not os.path.exists(JUICYSTATS_PATH):
    os.makedirs(JUICYSTATS_PATH)  # Create directory if it doesn't exist

player_dict = players.get_players()

def allassists():
	response = assistleaders.AssistLeaders(
		league_id=LeagueID.default,
        per_mode_simple=PerModeSimple.default,
        player_or_team=PlayerOrTeam.default,
        season=Season.default,
        season_type_playoffs=SeasonType.default,
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
	)

	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	with open("./juicystats/alltimeassists.json", "w") as outfile:
	    outfile.write(jsonContent)

def assiststracker():
	response = assisttracker.AssistTracker(
        college_nullable='',
        conference_nullable=ConferenceNullable.default,
        country_nullable='',
        date_from_nullable='',
        date_to_nullable='',
        division_simple_nullable=DivisionSimpleNullable.default,
        draft_pick_nullable='',
        draft_year_nullable='',
        game_scope_simple_nullable=GameScopeSimpleNullable.default,
        height_nullable='',
        last_n_games_nullable=LastNGamesNullable.default,
        league_id_nullable=LeagueIDNullable.default,
        location_nullable=LocationNullable.default,
        month_nullable=MonthNullable.default,
        opponent_team_id_nullable='',
        outcome_nullable=OutcomeNullable.default,
        po_round_nullable='',
        per_mode_simple_nullable=PerModeSimpleNullable.default,
        player_experience_nullable=PlayerExperienceNullable.default,
        player_position_abbreviation_nullable=PlayerPositionAbbreviationNullable.default,
        season_nullable=SeasonNullable.default,
        season_segment_nullable=SeasonSegmentNullable.default,
        season_type_all_star_nullable=SeasonTypeAllStarNullable.default,
        starter_bench_nullable=StarterBenchNullable.default,
        team_id_nullable='',
        vs_conference_nullable=ConferenceNullable.default,
        vs_division_nullable=DivisionNullable.default,
        weight_nullable='',
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
	)
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	with open("./juicystats/assiststacker.json", "w") as outfile:
	    outfile.write(jsonContent)

boxScoreArray = []
async def readLeagueGames():
    #URL = 'http://localhost:5190/api/tablelength/box/box_score_advanced_2024_25'
    #response = requests.get(url = URL)
    #data = response.json()
    #count = data['count']
    #f = open('./juicystats/league_games_2024_25.json')
	## returns JSON object as 
	## a dictionary
    #games = json.load(f)
    """Fetches table length and reads game data asynchronously."""
    URL = 'http://localhost:5190/api/tablelength/box/box_score_advanced_2024_25'
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
	# Iterating through the json
	# list
    idList = []
    end = len(games["resultSets"][0]["rowSet"])
    start = int(count) * 2
    print(start)
    print(end)
    for i in range (start - 1, end):
        ##print(len(games["resultSets"][0]["rowSet"]))
        ##print(games["resultSets"][0]["rowSet"][i])
        print(i)
        if games["resultSets"][0]["rowSet"][i][4] in idList:
            continue
        idList.append(games["resultSets"][0]["rowSet"][i][4])
        box = await boxscoreadvanced(games["resultSets"][0]["rowSet"][i][4])
        boxScoreArray.append(box)
    # Closing file
    f.close()   

async def boxscoreadvanced(gameId):
	response = boxscoreadvancedv2.BoxScoreAdvancedV2(
		game_id=gameId,
		end_period=EndPeriod.default,
		end_range=EndRange.default,
		range_type=RangeType.default,
		start_period=StartPeriod.default,
		start_range=StartRange.default,
		proxy=None,
		headers=None,
		timeout=30,
		get_request=True
	)
	
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	boxData = json.loads(jsonContent, object_hook=lambda d: SimpleNamespace(**d))
	header = boxData.resultSets[0].headers
	try:
		with open('./juicystats/box_score_advanced_2024_25.csv', 'a', encoding='UTF8', newline='') as f:
			writer = csv.writer(f)
			writer.writerow(header)
			writer.writerows(boxData.resultSets[0].rowSet)
			f.close()
	except ValueError:
		print("VALUE ERROR?!?!?!!?!!??!?!??!??!?!!?")

#import csv
#import aiofiles
#
#async def boxscoreadvanced(gameId):
#    """Fetches and processes advanced box score data asynchronously."""
#    try:
#        response = boxscoreadvancedv2.BoxScoreAdvancedV2(
#		    game_id=gameId,
#		    end_period=EndPeriod.default,
#		    end_range=EndRange.default,
#		    range_type=RangeType.default,
#		    start_period=StartPeriod.default,
#		    start_range=StartRange.default,
#		    proxy=None,
#		    headers=None,
#		    timeout=30,
#		    get_request=True
#	    )
#        content = json.loads(response.get_json())
#        boxData = json.loads(json.dumps(content), object_hook=lambda d: SimpleNamespace(**d))
#
#        file_path = './juicystats/box_score_advanced_2024_25.csv'
#
#        async with aiofiles.open(file_path, 'a', encoding='UTF8', newline='') as f:
#            writer = csv.writer(await f)
#            await writer.writerow(boxData.resultSets[0].headers)
#            await writer.writerows(boxData.resultSets[0].rowSet)
#
#        print(f"✅ Box score advanced data written for game {gameId}")
#
#    except Exception as e:
#        print(f"❌ Error in boxscoreadvanced(): {e}")

boxScoreScoringArray = []
async def readLeagueGamesScoring():
    URL = 'http://localhost:5190/api/tablelength/box_score_scoring_2024_25'
    response = requests.get(url = URL)
    data = response.json()
    count = data['count']
    
    f = open('./juicystats/league_games_2024_25.json')
	# returns JSON object as 
	# a dictionary
    games = json.load(f)
	# Iterating through the json
	# list
    idList = []
    end = len(games["resultSets"][0]["rowSet"])
    start = int(count) * 2
    print(start)
    print(end)
    print(len(games["resultSets"][0]["rowSet"]))
    for i in range (0, end):
        ##print(len(games["resultSets"][0]["rowSet"]))
        ##print(games["resultSets"][0]["rowSet"][i])
        print(i)
        if games["resultSets"][0]["rowSet"][i][4] in idList:
            continue
        idList.append(games["resultSets"][0]["rowSet"][i][4])
        box = boxScoreScoring(games["resultSets"][0]["rowSet"][i][4])
        boxScoreScoringArray.append(box)
    # Closing file
    f.close()   

def boxScoreScoring(gameId):

	response = boxscorescoringv2.BoxScoreScoringV2(
		game_id=gameId,
		end_period=EndPeriod.default,
		end_range=EndRange.default,
		range_type=RangeType.default,
		start_period=StartPeriod.default,
		start_range=StartRange.default,
		proxy=None,
		headers=None,
		timeout=30,
		get_request=True
	)
	
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	boxData = json.loads(jsonContent, object_hook=lambda d: SimpleNamespace(**d))
	header = boxData.resultSets[0].headers
	try:
		with open('./juicystats/box_score_scoring_2024_25.csv', 'a', encoding='UTF8', newline='') as f:
			writer = csv.writer(f)
			writer.writerow(header)
			writer.writerows(boxData.resultSets[0].rowSet)
			f.close()
	except ValueError:
		print("VALUE ERROR?!?!?!!?!!??!?!??!??!?!!?")

def alltimers():
	response = alltimeleadersgrids.AllTimeLeadersGrids(
        league_id=LeagueID.default,
        per_mode_simple=PerModeSimple.default,
        season_type=SeasonType.default,
        topx=20,
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
	)

	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	with open("./juicystats/alltimeleadersgrids2.json", "w") as outfile:
	    outfile.write(jsonContent)

async def shotchartdetailfunction():
	response = shotchartdetail.ShotChartDetail(
		team_id=0,
		player_id=0,
		context_measure_simple='FGA',
		season_nullable='2024-25',
		season_type_all_star='Regular Season',
	)
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	with open("./juicystats/shots_2024_25.json", "w") as outfile:
	    outfile.write(jsonContent)

def playergamelogfunction(playerId, season):
	print('helllllooooo??????????')
	response = playergamelog.PlayerGameLog(player_id=playerId, season=season)
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	with open("./juicystats/games.json", "w") as outfile:
		outfile.write(jsonContent)

async def writeNBAplayers():
	seasons = ['2015', '2016', '2017', '2018', '2019', '2020', '2021', '2022', '2023', '2024', '2025']
	list = []
	for player in player_dict:
		print(player)
		jsonContent = json.dumps(player)
		list.append(player)
		jsonList = json.dumps(list)
	with open("./juicystats/playersNBA2025.json", "w") as outfile:
		outfile.write(jsonList)

async def leaguegames():
	response = leaguegamelog.LeagueGameLog(
		counter=0,
        direction=Direction.default,
        league_id=LeagueID.default,
        player_or_team_abbreviation=PlayerOrTeamAbbreviation.default,
        season="2024-25",
        season_type_all_star=SeasonTypeAllStar.default,
        sorter=Sorter.default,
        date_from_nullable='',
        date_to_nullable='',
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
	)

	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	with open("./juicystats/league_games_2024_25.json", "w") as outfile:
	    outfile.write(jsonContent)

def leaguehustlestats():
	response = leaguehustlestatsplayer.LeagueHustleStatsPlayer(
		per_mode_time=PerModeTime.default,
        season="2024-25",
        season_type_all_star=SeasonTypeAllStar.default,
        college_nullable='',
        conference_nullable=ConferenceNullable.default,
        country_nullable='',
        date_from_nullable='',
        date_to_nullable='',
        division_simple_nullable=DivisionSimpleNullable.default,
        draft_pick_nullable='',
        draft_year_nullable='',
        height_nullable='',
        league_id_nullable=LeagueIDNullable.default,
        location_nullable=LocationNullable.default,
        month_nullable=MonthNullable.default,
        opponent_team_id_nullable='',
        outcome_nullable=OutcomeNullable.default,
        po_round_nullable='',
        player_experience_nullable=PlayerExperienceNullable.default,
        player_position_nullable=PlayerPositionNullable.default,
        season_segment_nullable=SeasonSegmentNullable.default,
        team_id_nullable='',
        vs_conference_nullable=ConferenceNullable.default,
        vs_division_nullable=DivisionNullable.default,
        weight_nullable='',
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
	)
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)

	with open("./juicystats/leaguehustlestatsplayer2024-2025.json", "w") as outfile:
	    outfile.write(jsonContent)



##def leaguehustlestatsleaders():
##	response = leaguehustlestatsplayerleaders.LeagueHustleStatsPlayerLeaders(
## 		per_mode_time=PerModeTime.default,
##        season='2022-23',
##        season_type_all_star=SeasonTypeAllStar.default,
##        college_nullable='',
##        conference_nullable=ConferenceNullable.default,
##        country_nullable='',
##        date_from_nullable='',
##        date_to_nullable='',
##        division_simple_nullable=DivisionSimpleNullable.default,
##        draft_pick_nullable='',
##        draft_year_nullable='',
##        height_nullable='',
##        league_id_nullable=LeagueIDNullable.default,
##        location_nullable=LocationNullable.default,
##        month_nullable=MonthNullable.default,
##        opponent_team_id_nullable='',
##        outcome_nullable=OutcomeNullable.default,
##        po_round_nullable='',
##        player_experience_nullable=PlayerExperienceNullable.default,
##        player_position_nullable=PlayerPositionNullable.default,
##        season_segment_nullable=SeasonSegmentNullable.default,
##        team_id_nullable='',
##        vs_conference_nullable=ConferenceNullable.default,
##        vs_division_nullable=DivisionNullable.default,
##        weight_nullable='',
##        proxy=None,
##        headers=None,
##        timeout=30,
##        get_request=True
##	)
##	content = json.loads(response.get_json())
##	jsonContent = json.dumps(content)
##	print(response)
##	print(Season.default)
##	with open("leaguehustlestatsplayerleaders2022-2023.json", "w") as outfile:
##	    outfile.write(jsonContent)
##
def leaguedashlineupsfunction(groupQuantity, measureType, season):
	response = leaguedashlineups.LeagueDashLineups(
		group_quantity=groupQuantity,
        last_n_games=LastNGames.default,
        measure_type_detailed_defense=measureType,
        month=Month.default,
        opponent_team_id=0,
        pace_adjust=PaceAdjust.default,
        per_mode_detailed=PerModeDetailed.default,
        period=Period.default,
        plus_minus=PlusMinus.default,
        rank=Rank.default,
        season=season,
        season_type_all_star=SeasonTypeAllStar.default,
        conference_nullable=ConferenceNullable.default,
        date_from_nullable='',
        date_to_nullable='',
        division_simple_nullable=DivisionSimpleNullable.default,
        game_segment_nullable=GameSegmentNullable.default,
        league_id_nullable=LeagueIDNullable.default,
        location_nullable=LocationNullable.default,
        outcome_nullable=OutcomeNullable.default,
        po_round_nullable='',
        season_segment_nullable=SeasonSegmentNullable.default,
        shot_clock_range_nullable=ShotClockRangeNullable.default,
        team_id_nullable='',
        vs_conference_nullable=ConferenceNullable.default,
        vs_division_nullable=DivisionNullable.default,
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
	)
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content) 
	with open(f"./juicystats/league_dash_lineups_{measureType}_{groupQuantity}man_{season}.json", "w") as outfile:
	    outfile.write(jsonContent)

def leaguedashoppptshotfunction():
	response = leaguedashoppptshot.LeagueDashOppPtShot(
        league_id=LeagueID.default,
        per_mode_simple=PerModeSimple.default,
        season='2022-23',
        season_type_all_star=SeasonTypeAllStar.default,
        close_def_dist_range_nullable='',
        conference_nullable=ConferenceNullable.default,
        date_from_nullable='',
        date_to_nullable='',
        division_nullable=DivisionNullable.default,
        dribble_range_nullable='',
        game_segment_nullable=GameSegmentNullable.default,
        general_range_nullable='',
        last_n_games_nullable=LastNGamesNullable.default,
        location_nullable=LocationNullable.default,
        month_nullable=MonthNullable.default,
        opponent_team_id_nullable='',
        outcome_nullable=OutcomeNullable.default,
        po_round_nullable='',
        period_nullable=PeriodNullable.default,
        season_segment_nullable=SeasonSegmentNullable.default,
        shot_clock_range_nullable=ShotClockRangeNullable.default,
        shot_dist_range_nullable='',
        team_id_nullable='',
        touch_time_range_nullable='',
        vs_conference_nullable=ConferenceNullable.default,
        vs_division_nullable=DivisionNullable.default,
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
    )
   
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	print(response)
	print(Season.default)
	with open("./juicystats/leaguedashoppptshot2022-2023.json", "w") as outfile:
	    outfile.write(jsonContent)

#def write():    
#    #establishing the connection
#    conn = psycopg2.connect(
#       database="NBAstatistics", user='petejackerson', password='redsox45', host='localhost', port= '5432'
#    )
#
#    #Setting auto commit false
#    conn.autocommit = True
#
#    #Creating a cursor object using the cursor() method
#    cursor = conn.cursor()
#
#    #Retrieving data
#    cursor.execute('''SELECT * from "hustleFactor"''')
#
#    response = cursor.fetchall()
#   
#    content = json.dumps(response)
#    jsonContent = json.loads(content)
#    jsonResult = json.dumps(jsonContent)
#
#    with open("./juicystats/hustleFactor.json", "w") as outfile:
#        outfile.write(jsonResult)
#    conn.close()
#
def leaguedashplayerclutchfunction():
	response = leaguedashplayerclutch.LeagueDashPlayerClutch(
        ahead_behind=AheadBehind.default,
        clutch_time=ClutchTime.default,
        last_n_games=LastNGames.default,
        measure_type_detailed_defense=MeasureTypeDetailedDefense.default,
        month=Month.default,
        opponent_team_id=0,
        pace_adjust=PaceAdjust.default,
        per_mode_detailed=PerModeDetailed.default,
        period=Period.default,
        plus_minus=PlusMinus.default,
        point_diff=PointDiff.default,
        rank=Rank.default,
        season="2022-23",
        season_type_all_star=SeasonTypeAllStar.default,
        college_nullable='',
        conference_nullable=ConferenceNullable.default,
        country_nullable='',
        date_from_nullable='',
        date_to_nullable='',
        division_simple_nullable=DivisionSimpleNullable.default,
        draft_pick_nullable='',
        draft_year_nullable='',
        game_scope_simple_nullable=GameScopeSimpleNullable.default,
        game_segment_nullable=GameSegmentNullable.default,
        height_nullable='',
        league_id_nullable=LeagueIDNullable.default,
        location_nullable=LocationNullable.default,
        outcome_nullable=OutcomeNullable.default,
        po_round_nullable='',
        player_experience_nullable=PlayerExperienceNullable.default,
        player_position_abbreviation_nullable=PlayerPositionAbbreviationNullable.default,
        season_segment_nullable=SeasonSegmentNullable.default,
        shot_clock_range_nullable=ShotClockRangeNullable.default,
        starter_bench_nullable=StarterBenchNullable.default,
        team_id_nullable='',
        vs_conference_nullable=ConferenceNullable.default,
        vs_division_nullable=DivisionNullable.default,
        weight_nullable='',
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True,
    )
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	with open("./juicystats/leaguedashplayerclutch2022-2023.json", "w") as outfile:
	    outfile.write(jsonContent)

def leaguedashplayerptshotfunction():
	response = leaguedashplayerptshot.LeagueDashPlayerPtShot(
        league_id=LeagueID.default,
        per_mode_simple=PerModeSimple.default,
        season="2022-23",
        season_type_all_star=SeasonTypeAllStar.default,
        close_def_dist_range_nullable='',
        college_nullable='',
        conference_nullable=ConferenceNullable.default,
        country_nullable='',
        date_from_nullable='',
        date_to_nullable='',
        division_nullable=DivisionNullable.default,
        draft_pick_nullable='',
        draft_year_nullable='',
        dribble_range_nullable='',
        game_segment_nullable=GameSegmentNullable.default,
        general_range_nullable='',
        height_nullable='',
        last_n_games_nullable=LastNGamesNullable.default,
        location_nullable=LocationNullable.default,
        month_nullable=MonthNullable.default,
        opponent_team_id_nullable='',
        outcome_nullable=OutcomeNullable.default,
        po_round_nullable='',
        period_nullable=PeriodNullable.default,
        player_experience_nullable=PlayerExperienceNullable.default,
        player_position_nullable=PlayerPositionNullable.default,
        season_segment_nullable=SeasonSegmentNullable.default,
        shot_clock_range_nullable=ShotClockRangeNullable.default,
        shot_dist_range_nullable='',
        starter_bench_nullable=StarterBenchNullable.default,
        team_id_nullable='',
        touch_time_range_nullable='',
        vs_conference_nullable=ConferenceNullable.default,
        vs_division_nullable=DivisionNullable.default,
        weight_nullable='',
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True,
    )
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	with open("./juicystats/leaguedashplayerptshot2022-2023.json", "w") as outfile:
	    outfile.write(jsonContent)


def leaguedashplayershotlocationsfunction():
	response = leaguedashplayershotlocations.LeagueDashPlayerShotLocations(
        distance_range=DistanceRange.default,
        last_n_games=LastNGames.default,
        measure_type_simple=MeasureTypeSimple.default,
        month=Month.default,
        opponent_team_id=0,
        pace_adjust=PaceAdjust.default,
        per_mode_detailed=PerModeDetailed.default,
        period=Period.default,
        plus_minus=PlusMinus.default,
        rank=Rank.default,
        season="2022-23",
        season_type_all_star=SeasonTypeAllStar.default,
        college_nullable='',
        conference_nullable=ConferenceNullable.default,
        country_nullable='',
        date_from_nullable='',
        date_to_nullable='',
        division_simple_nullable=DivisionSimpleNullable.default,
        draft_pick_nullable='',
        draft_year_nullable='',
        game_scope_simple_nullable=GameScopeSimpleNullable.default,
        game_segment_nullable=GameSegmentNullable.default,
        height_nullable='',
        league_id_nullable=LeagueIDNullable.default,
        location_nullable=LocationNullable.default,
        outcome_nullable=OutcomeNullable.default,
        po_round_nullable='',
        player_experience_nullable=PlayerExperienceNullable.default,
        player_position_abbreviation_nullable=PlayerPositionAbbreviationNullable.default,
        season_segment_nullable=SeasonSegmentNullable.default,
        shot_clock_range_nullable=ShotClockRangeNullable.default,
        starter_bench_nullable=StarterBenchNullable.default,
        team_id_nullable='',
        vs_conference_nullable=ConferenceNullable.default,
        vs_division_nullable=DivisionNullable.default,
        weight_nullable='',
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True,
    )
	content = json.loads(response.get_json())
	jsonContent = json.dumps(content)
	with open("./juicystats/leaguedashplayershotlocations2022-2023.json", "w") as outfile:
	    outfile.write(jsonContent)


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
    for i in range (0, end):
        print(i)
        if games["resultSets"][0]["rowSet"][i][4] in idList or games["resultSets"][0]["rowSet"][i][4] is None:
            continue
        idList.append(games["resultSets"][0]["rowSet"][i][4])
        box = await boxScoreTraditional(games["resultSets"][0]["rowSet"][i][4])
        boxScoreArrayTraditional.append(box)
    # Closing file
    print(boxScoreArrayTraditional)
    print(len(boxScoreArrayTraditional))
    f.close()


async def boxScoreTraditional(gameId):
    response = boxscoretraditionalv3.BoxScoreTraditionalV3(
        game_id = gameId,
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
    boxData = json.loads(jsonContent, object_hook=lambda d: SimpleNamespace(**d))
    header = boxData.resultSets[0].headers
    try:
        with open('./juicystats/box_score_traditional_2024_25_v3.csv', 'a', encoding='UTF8', newline='') as f:
            writer = csv.writer(f)
            writer.writerow(header)
            writer.writerows(boxData.resultSets[0].rowSet)
            f.close()
    except ValueError:
        print("VALUE ERROR?!?!?!!?!!??!?!??!??!?!!?")

def leagueDashPlayerStatsFunction():
    response = leaguedashplayerstats.LeagueDashPlayerStats(
        last_n_games=LastNGames.default,
        measure_type_detailed_defense=MeasureTypeDetailedDefense.default,
        month=Month.default,
        opponent_team_id=0,
        pace_adjust=PaceAdjust.default,
        per_mode_detailed=PerModeDetailed.default,
        period=Period.default,
        plus_minus=PlusMinus.default,
        rank=Rank.default,
        season='2022-23',
        season_type_all_star=SeasonTypeAllStar.default,
        college_nullable='',
        conference_nullable=ConferenceNullable.default,
        country_nullable='',
        date_from_nullable='',
        date_to_nullable='',
        division_simple_nullable=DivisionSimpleNullable.default,
        draft_pick_nullable='',
        draft_year_nullable='',
        game_scope_simple_nullable=GameScopeSimpleNullable.default,
        game_segment_nullable=GameSegmentNullable.default,
        height_nullable='',
        league_id_nullable=LeagueIDNullable.default,
        location_nullable=LocationNullable.default,
        outcome_nullable=OutcomeNullable.default,
        po_round_nullable='',
        player_experience_nullable=PlayerExperienceNullable.default,
        player_position_abbreviation_nullable=PlayerPositionAbbreviationNullable.default,
        season_segment_nullable=SeasonSegmentNullable.default,
        shot_clock_range_nullable=ShotClockRangeNullable.default,
        starter_bench_nullable=StarterBenchNullable.default,
        team_id_nullable='',
        two_way_nullable='',
        vs_conference_nullable=ConferenceNullable.default,
        vs_division_nullable=DivisionNullable.default,
        weight_nullable='',
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
    )
    print(response)
    
    content = json.loads(response.get_json())
    jsonContent = json.dumps(content)
    print(jsonContent)
    with open("./juicystats/leaguedashplayerstats2022-2023.json", "w") as outfile:
	    outfile.write(jsonContent)

def getPlayerIds():
    f = open('./juicystats/playersNBA2024.json')
    players = json.load(f)
    for i in range(0, len(players)):
        playerid = str(players[i]['id'])
        playerCareerStatsFunction(playerid)

    f.close()

def playerCareerStatsFunction(playerid):
    print(playerid)
    response = playercareerstats.PlayerCareerStats(
        player_id = playerid,
        per_mode36='PerGame',
        league_id_nullable=LeagueIDNullable.default,
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
    )
    content = json.loads(response.get_json())
    jsonContent = json.dumps(content)
    careerData = json.loads(jsonContent, object_hook=lambda d: SimpleNamespace(**d))

    header = careerData.resultSets[0].headers
    try:
        with open('./juicystats/seasonregularplayerstats2024.csv', 'a', encoding='UTF8', newline='') as f:
            writer = csv.writer(f)
            writer.writerow(header)
            writer.writerows(careerData.resultSets[0].rowSet)
            f.close()
    except ValueError:
        print("VALUE ERROR?!?!?!!?!!??!?!??!??!?!!?")

boxScoreSummaryArray = []
async def readBoxScoreSummary():
    """Fetches table length and reads game data asynchronously."""
    URL = 'http://localhost:5190/api/tablelength/box_score_summary_2024_25'
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
            print(f"❌ Error in readboxscoresummary(): {e}")
    idList = []
    end = len(games["resultSets"][0]["rowSet"])
    start = int(count) * 2
    print('start')
    print(start)
    print('end')
    print(end)
    
    for i in range (start - 1, end):
        print(i)
        if games["resultSets"][0]["rowSet"][i][4] in idList or games["resultSets"][0]["rowSet"][i][4] is None:
            print(games["resultSets"][0]["rowSet"][i][4])
            continue
        idList.append(games["resultSets"][0]["rowSet"][i][4])
        print(games["resultSets"][0]["rowSet"][i][4])
        box = await boxScoreSummaryFunction(games["resultSets"][0]["rowSet"][i][4])
        boxScoreSummaryArray.append(box)
    # Closing file
    f.close()

async def boxScoreSummaryFunction(gameid):
    response = boxscoresummaryv2.BoxScoreSummaryV2 (    
        game_id=gameid,
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
    )
    content = json.loads(response.get_json())
    jsonContent = json.dumps(content)
    careerData = json.loads(jsonContent, object_hook=lambda d: SimpleNamespace(**d))

    header = careerData.resultSets[0].headers
    try:
        with open('./juicystats/box_score_summary_2024_25.csv', 'a', encoding='UTF8', newline='') as f:
            writer = csv.writer(f)
            writer.writerow(header)
            writer.writerows(careerData.resultSets[0].rowSet)
            f.close()
    except ValueError:
        print("VALUE ERROR?!?!?!!?!!??!?!??!??!?!!?")

async def getOdds():
    URL = 'https://api.the-odds-api.com/v4/sports/basketball_nba/odds/?apiKey=f8d80068fa8107d46ae62e3a3f15092f&regions=us&markets=h2h&oddsFormat=american&bookmakers=draftkings'
    response = requests.get(url = URL)
    data = response.json()
    rows = []

    def convert_zulu_to_cst(zulu_time):
    # Parse the Zulu time string into a datetime object
        zulu_datetime = datetime.strptime(zulu_time, '%Y-%m-%dT%H:%M:%SZ')

        # Set the time zone offset for Central Standard Time (CST)
        cst_offset = timedelta(hours=-6)

        # Apply the offset to get CST time
        cst_datetime = zulu_datetime + cst_offset

        # Format the CST datetime as a string
        cst_time_string = cst_datetime.strftime('%Y-%m-%d %H:%M:%S')
        
        return cst_time_string
    
    for i in range (0, len(data)):

        if len(data[i]['bookmakers']) > 0:
            print(i)
            rowSet = [
                'upcoming',
                convert_zulu_to_cst(data[i]['commence_time'])[0:10],
                data[i]['home_team'],
                data[i]['away_team'],
                str(data[i]['bookmakers'][0]['markets'][0]['outcomes'][0]['price']),
                str(data[i]['bookmakers'][0]['markets'][0]['outcomes'][1]['price'])
            ]
            rows.append(rowSet)
        else:
            rowSet = [
                'upcoming',
                convert_zulu_to_cst(data[i]['commence_time'])[0:10],
                data[i]['home_team'],
                data[i]['away_team'],
                'no odds available',
                'no odds available'
            ]
            rows.append(rowSet)
    header = ['game_id', 'commence_time', 'home_team', 'away_team', 'home_odds', 'away_odds']
    try:
        with open('./juicystats/new_odds_2024_25.csv', 'a', encoding='UTF8', newline='') as f:
            writer = csv.writer(f)
            writer.writerow(header)
            writer.writerows(rows)
            f.close()
    except ValueError:
        print("VALUE ERROR?!?!?!!?!!??!?!??!??!?!!?")
	
boxScoreArrayMisc = []
async def readLeagueMisc():
    """Fetches table length and reads game data asynchronously."""
    URL = 'http://localhost:5190/api/tablelength/box/box_score_misc_2024_25'
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
            print(f"❌ Error in readboxscoremisc(): {e}")
    #URL = 'http://localhost:5190/api/tablelength/box/box_score_misc_2024_25'
    #response = requests.get(url = URL)
    #data = response.json()
    #print(data)
    #count = data['count']
    #f = open('./juicystats/league_games_2024_25.json')
    #games = json.load(f)

    idList = []
    end = len(games["resultSets"][0]["rowSet"])
    start = int(count) * 2
    print(start)
    print(end)
    for i in range (start - 1, end):
        print(i)
        if games["resultSets"][0]["rowSet"][i][4] in idList or games["resultSets"][0]["rowSet"][i][4] is None:
            print(games["resultSets"][0]["rowSet"][i][4])
            continue
        idList.append(games["resultSets"][0]["rowSet"][i][4])
        box = await boxScoreMiscFunction(games["resultSets"][0]["rowSet"][i][4])
        boxScoreArrayMisc.append(box)
    # Closing file
    f.close()

async def boxScoreMiscFunction(gameid):
    response = boxscoremiscv2.BoxScoreMiscV2(
        game_id=gameid,
        end_period=EndPeriod.default,
        end_range=EndRange.default,
        range_type=RangeType.default,
        start_period=StartPeriod.default,
        start_range=StartRange.default,
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
    )
    content = json.loads(response.get_json())
    jsonContent = json.dumps(content)
    boxScoreMiscData = json.loads(jsonContent, object_hook=lambda d: SimpleNamespace(**d))
    header = boxScoreMiscData.resultSets[0].headers
    try:
        with open('./juicystats/box_score_misc_2024_25.csv', 'a', encoding='UTF8', newline='') as f:
            writer = csv.writer(f)
            writer.writerow(header)
            writer.writerows(boxScoreMiscData.resultSets[0].rowSet)
            f.close()
    except ValueError:
        print("VALUE ERROR?!?!?!!?!!??!?!??!??!?!!?")

def defenseHub():
    response = defensehub.DefenseHub(
        game_scope_detailed=GameScopeDetailed.default,
        league_id=LeagueID.default,
        player_or_team=PlayerOrTeam.default,
        player_scope=PlayerScope.default,
        season='2023-24',
        season_type_playoffs=SeasonType.default,
        proxy=None,
        headers=None,
        timeout=30,
        get_request=True
    )
    
    content = json.loads(response.get_json())
    print(content)
    jsonContent = json.dumps(content)
    with open("./juicystats/defensehub.json", "w") as outfile:	    
        outfile.write(jsonContent)
        
#
#
## Global configuration
API_RETRY_DELAY = 3  # Seconds to wait before retrying on failure

async def get_json_response_startup(url):
    """Fetch JSON response with retry logic and rate limiting."""
    async with aiohttp.ClientSession() as session:
        for attempt in range(3):  # Retry up to 3 times
            try:
                async with session.get(url, timeout=10) as response:
                    if response.status == 200:
                        return await response.json()
                    elif response.status == 429:  # Rate limit response
                        print(f"Rate limit reached. Retrying in {API_RETRY_DELAY} seconds...")
                        await asyncio.sleep(API_RETRY_DELAY)
                    else:
                        print(f"Request failed with status {response.status}")
                        return None
            except aiohttp.ClientError as e:
                print(f"Network error: {e}. Retrying in {API_RETRY_DELAY} seconds...")
                await asyncio.sleep(API_RETRY_DELAY)
        return None  # Return None if all retries fail

async def post_league_games_by_season(obj, season, base_url):
    """Post game data with error handling and retry logic."""
    url = base_url + f"api/leagueGames/{season}"

    async with aiohttp.ClientSession() as session:
        for attempt in range(3):
            try:
                async with session.post(url, json=obj, timeout=10) as response:
                    if response.status == 200:
                        json_response = await response.json()
                        print(json_response)
                        return json_response
                    elif response.status == 429:
                        print(f"Rate limit on POST request. Retrying in {API_RETRY_DELAY} seconds...")
                        await asyncio.sleep(API_RETRY_DELAY)
                    else:
                        print(f"POST request failed with status {response.status}")
                        return None
            except aiohttp.ClientError as e:
                print(f"Network error during POST: {e}. Retrying in {API_RETRY_DELAY} seconds...")
                await asyncio.sleep(API_RETRY_DELAY)
        return None  # If all retries fail

async def load_up_league_games_by_season(base_url):
    """Fetch and store NBA games with rate limit handling."""
    years = ['2022-2023']
    for year in years:
        TABLE_LENGTH_URL = base_url + f"api/tablelength/leagueGames{year}"
        table_length_response = await get_json_response_startup(TABLE_LENGTH_URL)

        if table_length_response:
            table_length = table_length_response[0].get('count', 0)
            print(f"Table Length: {table_length}")

            LEAGUE_GAMES_URL = base_url + f'api/leagueGames/{year}'
            games_array = await get_json_response_startup(LEAGUE_GAMES_URL)

            if games_array and 'resultSets' in games_array:
                for result_set in games_array['resultSets']:
                    for row in result_set['rowSet']:
                        print(row)
                        await post_league_games_by_season(row, year, base_url)
                        await asyncio.sleep(0.5)  # Small delay to prevent rate limits

    print('FINISHED!!!!!!!!!!!!!!!!!!!!!!')

async def login_and_fetch_data(base_url, login_env):
    """Handles user login and fetches data after authentication."""
    if login_env:
        try:
            my_json_obj = json.loads(login_env)
            post_data = json.dumps(my_json_obj)
            LOGIN_URL = base_url + 'api/users/login'
            print(f"Logging in: {LOGIN_URL}")

            async with aiohttp.ClientSession() as session:
                for attempt in range(3):
                    try:
                        async with session.post(LOGIN_URL, data=post_data, headers={"Content-Type": "application/json"}, timeout=10) as login_response:
                            print(f"Login response: {login_response.status}")
                            login_text = await login_response.text()
                            print(login_text)

                            if login_response.status == 200:
                                await load_up_league_games_by_season(base_url)
                                return  # Stop retrying if login succeeds
                            elif login_response.status == 429:
                                print(f"Rate limit on login. Retrying in {API_RETRY_DELAY} seconds...")
                                await asyncio.sleep(API_RETRY_DELAY)
                            else:
                                print("Login failed!")
                                return
                    except aiohttp.ClientError as e:
                        print(f"Network error during login: {e}. Retrying in {API_RETRY_DELAY} seconds...")
                        await asyncio.sleep(API_RETRY_DELAY)
        except json.JSONDecodeError:
            print("Error decoding JSON")

async def my_function():
    """Main function to manage the data fetch process."""
    node_env = os.environ.get("NODE_ENV")
    print(f"NODE_ENV: {node_env}")

    base_url = "/" if node_env == "production" else "http://localhost:3001/"
    print(f"Base URL: {base_url}")

    login_env = os.environ.get("LOGIN")
    print(f"LOGIN: {login_env}")

    await login_and_fetch_data(base_url, login_env)

    await asyncio.sleep(1)
    return 'end of function'

# Run the function asynchronously
# asyncio.run(my_function())

    #URL = base_url + 'api/leagueGames/2022-2023'
    #response = requests.get(url = URL)
    #data = response.json()
    #print(data)

    ##URL2 = base_url + 'api/leagueGames/2022-2023'
    ##response2 = requests.post(url = URL2, data = "hooligans")

#my_function()
#aiocron.crontab("45 17 * * *", func=my_function)  # Run at 13:31 every day

# Run the event loop
#asyncio.get_event_loop().run_forever()

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
    #await safe_run(readLeagueGamesTraditional)
    #await safe_run(leagueDashPlayerStatsFunction)
    #await safe_run(getPlayerIds)  # Runs before player stats function
    #await safe_run(readBoxScoreSummary)
    await safe_run(readLeagueGamesScoring)
    #await safe_run(writeNBAplayers)
    #await safe_run(getOdds)
    #await safe_run(readLeagueMisc)
    #await safe_run(defenseHub)

    print("✅ All tasks completed successfully!")

# Run the async event loop
if __name__ == "__main__":
    asyncio.run(run_tasks())
## HERE ARE THE FUNCTIONS THAT I WANT TO RUN ONE BY ONE
#leaguegames()
#shotchartdetailfunction()
#allassists()
#assiststracker()
#playergamelogfunction('153', '0021700807')
#readLeagueGames()
#leaguehustlestats()
###leaguehustlestatsleaders()
#numPlayers = ['2','3','4','5']
#boxTypes = ['Base', 'Advanced', 'Four Factors', 'Misc', 'Scoring', 'Opponent']
#for num in numPlayers:
#    for boxType in boxTypes:
#        print(num)
#        print(boxType)
#        leaguedashlineupsfunction(num, boxType, '2024_25')
#leaguedashoppptshotfunction()
###write()
#leaguedashplayerclutchfunction()
#leaguedashplayerptshotfunction()
#leaguedashplayershotlocationsfunction()
#readLeagueGamesTraditional()
#leagueDashPlayerStatsFunction()
#playerCareerStatsFunction()
#getPlayerIds()
#readBoxScoreSummary()
#readLeagueGamesScoring()
#writeNBAplayers()
#getOdds()
#readLeagueMisc()
#defenseHub()