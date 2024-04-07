using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReactApp4.Server.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NpgsqlTypes;
using static System.Net.WebRequestMethods;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Numerics;
using System.Reflection.Emit;
using ReactApp4.Server.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;

namespace ReactApp4.Server.Services
{
    public class PlayerResultsDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<IActionResult> GetPlayerResults(string selectedSeason, string selectedOpponent, string player_id, string propBetStats)
        {
            //percentage of games over/under the line
            var decodedPropBetStatsJson = System.Net.WebUtility.UrlDecode(propBetStats);

            // Deserialize the JSON strings into lists of objects
            var propBetStatsList = JsonConvert.DeserializeObject<List<PropBetStats>>(decodedPropBetStatsJson);

            var decodedSelectedOpponentJson = System.Net.WebUtility.UrlDecode(selectedOpponent);

            // Deserialize the JSON strings into lists of objects
            var selectedOpponentObject = JsonConvert.DeserializeObject<NBATeam>(decodedSelectedOpponentJson);

            // Now you can work with 'rosterList' and 'propBetStatsList' as lists of objects
            Console.WriteLine(selectedSeason);
            Console.WriteLine("selected season");
            if (selectedSeason == "1")
            {
                Console.WriteLine("season is 1");
                try
                {
                    var query = $@"
                            (
                                SELECT box_score_traditional_2015_16.*, league_games_2015_16.game_date, league_games_2015_16.matchup
                                FROM box_score_traditional_2015_16
                                JOIN league_games_2015_16
                                ON box_score_traditional_2015_16.game_id = league_games_2015_16.game_id
                                AND box_score_traditional_2015_16.team_id = league_games_2015_16.team_id
                                WHERE player_id = @player_id AND box_score_traditional_2015_16.min > 0 ";
                    if (selectedOpponentObject != null)
                    {
                        query += $@"AND league_games_2015_16.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%' ";
                    }

                    query += $@"ORDER BY box_score_traditional_2015_16.id
                            )
                                UNION ALL
                            (
                                SELECT box_score_traditional_2016_17.*, league_games_2016_17.game_date, league_games_2016_17.matchup
                                FROM box_score_traditional_2016_17
                                JOIN league_games_2016_17
                                ON box_score_traditional_2016_17.game_id = league_games_2016_17.game_id
                                AND box_score_traditional_2016_17.team_id = league_games_2016_17.team_id
                                WHERE player_id = @player_id AND box_score_traditional_2016_17.min > 0 ";
                    if (selectedOpponentObject != null)
                    {
                        query += $@"AND league_games_2016_17.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%' ";
                    }

                    query += $@"ORDER BY box_score_traditional_2016_17.id
                            )
                                UNION ALL
                            (    
                                SELECT box_score_traditional_2017_18.*, league_games_2017_18.game_date, league_games_2017_18.matchup
                                FROM box_score_traditional_2017_18
                                JOIN league_games_2017_18
                                ON box_score_traditional_2017_18.game_id = league_games_2017_18.game_id
                                AND box_score_traditional_2017_18.team_id = league_games_2017_18.team_id
                                WHERE player_id = @player_id AND box_score_traditional_2017_18.min > 0 ";
                    if (selectedOpponentObject != null)
                    {
                        query += $@"AND league_games_2017_18.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%' ";
                    }

                    query += $@"ORDER BY box_score_traditional_2017_18.id
                            )
                                UNION ALL
                            (    
                                SELECT box_score_traditional_2018_19.*, league_games_2018_19.game_date, league_games_2018_19.matchup
                                FROM box_score_traditional_2018_19
                                JOIN league_games_2018_19
                                ON box_score_traditional_2018_19.game_id = league_games_2018_19.game_id
                                AND box_score_traditional_2018_19.team_id = league_games_2018_19.team_id
                                WHERE player_id = @player_id AND box_score_traditional_2018_19.min > 0 ";
                    if (selectedOpponentObject != null)
                    {
                        query += $@"AND league_games_2018_19.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%' ";
                    }

                    query += $@"ORDER BY box_score_traditional_2018_19.id
                            )
                                UNION ALL
                            (
                                SELECT box_score_traditional_2019_20.*, league_games_2019_20.game_date, league_games_2019_20.matchup
                                FROM box_score_traditional_2019_20
                                JOIN league_games_2019_20
                                ON box_score_traditional_2019_20.game_id = league_games_2019_20.game_id
                                AND box_score_traditional_2019_20.team_id = league_games_2019_20.team_id
                                WHERE player_id = @player_id AND box_score_traditional_2019_20.min > 0 ";
                    if (selectedOpponentObject != null)
                    {
                        query += $@"AND league_games_2019_20.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%' ";
                    }

                    query += $@"ORDER BY box_score_traditional_2019_20.id
                            )
                                UNION ALL
                            (    
                                SELECT box_score_traditional_2020_21.*, league_games_2020_21.game_date, league_games_2020_21.matchup
                                FROM box_score_traditional_2020_21
                                JOIN league_games_2020_21
                                ON box_score_traditional_2020_21.game_id = league_games_2020_21.game_id
                                AND box_score_traditional_2020_21.team_id = league_games_2020_21.team_id
                                WHERE player_id = @player_id AND box_score_traditional_2020_21.min > 0 ";
                    if (selectedOpponentObject != null)
                    {
                        query += $@"AND league_games_2020_21.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%' ";
                    }

                    query += $@"ORDER BY box_score_traditional_2020_21.id
                            )
                                UNION ALL
                            (
                                SELECT box_score_traditional_2021_22.*, league_games_2021_22.game_date, league_games_2021_22.matchup
                                FROM box_score_traditional_2021_22
                                JOIN league_games_2021_22
                                ON box_score_traditional_2021_22.game_id = league_games_2021_22.game_id
                                AND box_score_traditional_2021_22.team_id = league_games_2021_22.team_id
                                WHERE player_id = @player_id AND box_score_traditional_2021_22.min > 0 ";
                    if (selectedOpponentObject != null)
                    {
                        query += $@"AND league_games_2021_22.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%' ";
                    }

                    query += $@"ORDER BY box_score_traditional_2021_22.id
                            )
                                UNION ALL
                            (
                                SELECT box_score_traditional_2022_23.*, league_games_2022_23.game_date, league_games_2022_23.matchup
                                FROM box_score_traditional_2022_23
                                JOIN league_games_2022_23
                                ON box_score_traditional_2022_23.game_id = league_games_2022_23.game_id
                                AND box_score_traditional_2022_23.team_id = league_games_2022_23.team_id
                                WHERE player_id = @player_id AND box_score_traditional_2022_23.min > 0 ";
                    if (selectedOpponentObject != null)
                    {
                        query += $@"AND league_games_2022_23.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%' ";
                    }

                    query += $@"ORDER BY box_score_traditional_2022_23.id
                            )
                                UNION ALL
                            (    
                                SELECT box_score_traditional_2023_24.*, league_games_2023_24.game_date, league_games_2023_24.matchup
                                FROM box_score_traditional_2023_24
                                JOIN league_games_2023_24
                                ON box_score_traditional_2023_24.game_id = league_games_2023_24.game_id
                                AND box_score_traditional_2023_24.team_id = league_games_2023_24.team_id
                                WHERE player_id = @player_id AND box_score_traditional_2023_24.min > 0 ";

                    if (selectedOpponentObject != null)
                    {
                        query += $@"AND league_games_2023_24.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%' ";
                    }

                    query += $@"ORDER BY box_score_traditional_2023_24.id
                            )";



                    //if (selectedOpponentObject != null)
                    //{
                    //    query += $@" WHERE {leagueGamesTable}.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%'";
                    //}
                    //query += $@"ORDER BY Games_Played.id DESC";

                    Console.WriteLine(query);

                    var boxScores = await _context.BoxScoreWithGameDates.FromSqlRaw(query,
                            new NpgsqlParameter("@player_id", player_id))
                        .ToListAsync();

                    Console.WriteLine(boxScores.ToString());
                    Console.WriteLine(boxScores);
                    Console.Write(boxScores);
                    return Ok(boxScores);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return StatusCode(500, $"Internal Server Error: {ex}");
                }

            }

            else
            {
                try
                {

                    var tableName = "box_score_traditional_" + selectedSeason;
                    var leagueGamesTable = "league_games_" + selectedSeason;

                    var query = $@"
                            WITH Box_Scores AS (
                                SELECT *
                                FROM {tableName}
                                WHERE player_id = @player_id
                            ),
                            Games_Played AS (
                                SELECT *
                                FROM {tableName}
                                WHERE player_id = @player_id
                                AND min > 0
                            )
                            SELECT Games_Played.*, {leagueGamesTable}.game_date, {leagueGamesTable}.matchup
                            FROM Games_Played 
                            JOIN {leagueGamesTable}
                            ON Games_Played.game_id = {leagueGamesTable}.game_id
                            AND Games_Played.team_id = {leagueGamesTable}.team_id";
                    if (selectedOpponentObject != null)
                    {
                        query += $@" WHERE {leagueGamesTable}.matchup LIKE '%{selectedOpponentObject.Team_abbreviation}%'";
                    }
                    query += $@" ORDER BY Games_Played.id DESC";

                    Console.WriteLine(query);

                    var boxScores = await _context.BoxScoreWithGameDates.FromSqlRaw(query,
                            new NpgsqlParameter("@player_id", player_id))
                        .ToListAsync();

                    Console.WriteLine(boxScores.ToString());
                    Console.WriteLine(boxScores);
                    Console.Write(boxScores);
                    return Ok(boxScores);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return StatusCode(500, $"Internal Server Error: {ex}");
                }
            }
        }
    }
}
