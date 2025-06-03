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
    public class MLBPlayerResultsDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<IActionResult> MLBGetPlayerResults(string hittingPitching, string selectedSeason, string selectedOpponent, int player_id, string propBetStats)
        {
            //percentage of games over/under the line
            var decodedPropBetStatsJson = System.Net.WebUtility.UrlDecode(propBetStats);

            // Deserialize the JSON strings into lists of objects
            var propBetStatsList = JsonConvert.DeserializeObject<List<PropBetStats>>(decodedPropBetStatsJson);

            var decodedSelectedOpponentJson = System.Net.WebUtility.UrlDecode(selectedOpponent);

            // Deserialize the JSON strings into lists of objects
            var selectedOpponentObject = JsonConvert.DeserializeObject<MLBTeam>(decodedSelectedOpponentJson);

            // Now you can work with 'rosterList' and 'propBetStatsList' as lists of objects
            Console.WriteLine(selectedSeason);
            Console.WriteLine("selected season");
            var query = $@"";
            try
            {
                //var tableName = "player_game_stats_batting_" + selectedSeason;
                var tableName = hittingPitching == "hitting" ? "player_game_stats_batting_2023" : "player_game_stats_pitching_2023";

                //var mlbGamesTable = "mlb_games_" + selectedSeason;
                var mlbGamesTable = "mlb_games_2023";

                var opponentCTE = $@"";
                List<object> boxScores;

                if (hittingPitching == "hitting")
                {
                    if (selectedOpponentObject != null && selectedOpponentObject.Team_id != "1")
                    {
                        opponentCTE += $@" with_opponent AS (
                            SELECT 
                                pgsb.*,
                                CASE
                                    WHEN at_bats = 0
                                    THEN 0
                                    ELSE CAST(hits AS FLOAT) / at_bats
                                END AS average,
                                hits - (doubles + triples + home_runs) AS singles,
                                mg.game_date,
                                mg.away_team_id,
                                mg.home_team_id,
                                CASE 
                                    WHEN pgsb.team_side = 'home' THEN mg.away_team_id
                                    WHEN pgsb.team_side = 'away' THEN mg.home_team_id
                                END AS opponent_team_id
                                FROM {tableName} pgsb
                                JOIN {mlbGamesTable} mg
                                    ON pgsb.game_pk = CAST(mg.game_pk AS INT)
                                WHERE person_id = @person_id
                                AND pgsb.games_played > 0
                        )";
                        query += $@"WITH ";
                        query += opponentCTE;
                        //query += $@",";
                        query += $@"
                            SELECT *
                            FROM with_opponent
                            WHERE CAST(opponent_team_id AS TEXT) = @selectedOpponent
                            ORDER BY with_opponent.id DESC";
                    }
                    else
                    {
                        query += $@"WITH ";
                        query += $@"
                            Games_Played AS (
                                SELECT *,
                                hits - (doubles + triples + home_runs) AS singles
                                FROM {tableName}
                                WHERE person_id = @person_id
                                AND {tableName}.games_played > 0
                            )
                            SELECT Games_Played.*, {mlbGamesTable}.game_date, {mlbGamesTable}.home_team_id, {mlbGamesTable}.away_team_id
                            FROM Games_Played 
                            JOIN {mlbGamesTable}
                            ON Games_Played.game_pk = CAST({mlbGamesTable}.game_pk AS INT)
                            ORDER BY Games_Played.id DESC";
                    }
                    boxScores = await _context.MLBBattingBoxScoreWithGameDates
                        .FromSqlRaw(query,
                            new NpgsqlParameter("@person_id", player_id),
                            new NpgsqlParameter("@selectedOpponent", selectedOpponentObject?.Team_id)
                        )
                        .ToListAsync<object>();
                }

                else
                {
                    if (selectedOpponentObject != null && selectedOpponentObject.Team_id != "1")
                    {
                        opponentCTE += $@" with_opponent AS (
                            SELECT 
                                pgsb.*,
                                CASE 
                                  WHEN outs = 0 THEN NULL
                                  ELSE ROUND((earned_runs * 9.0) / (outs / 3.0), 2)
                                END AS era,
                                CASE 
                                  WHEN outs = 0 THEN NULL
                                  ELSE ROUND((base_on_balls + hits) / (outs / 3.0), 2)
                                END AS whip,
                                CASE
                                    WHEN at_bats = 0
                                    THEN 0
                                    ELSE CAST(hits AS FLOAT) / at_bats
                                END AS average,
                                hits - (doubles + triples + home_runs) AS singles,
                                CASE
                                  WHEN outs = 0 THEN NULL
                                  ELSE ROUND(9 * strike_outs / (outs / 3.0), 2)
                                END AS strikeouts_per9,
                                CASE
                                  WHEN outs = 0 THEN NULL
                                  ELSE ROUND(9 * base_on_balls / (outs / 3.0), 2)
                                END AS walks_per9,                            
                                mg.game_date,
                                mg.away_team_id,
                                mg.home_team_id,
                                CASE 
                                    WHEN pgsb.team_side = 'home' THEN mg.away_team_id
                                    WHEN pgsb.team_side = 'away' THEN mg.home_team_id
                                END AS opponent_team_id
                                FROM {tableName} pgsb
                                JOIN {mlbGamesTable} mg
                                    ON pgsb.game_pk = CAST(mg.game_pk AS INT)
                                WHERE person_id = @person_id
                                AND pgsb.games_played > 0
                        )";
                        query += $@"WITH ";
                        query += opponentCTE;
                        //query += $@",";
                        query += $@"
                            SELECT *
                            FROM with_opponent
                            WHERE opponent_team_id = CAST(@selectedOpponent AS INT)
                            ORDER BY with_opponent.id DESC";
                    }
                    else
                    {
                        query += $@"WITH ";
                        query += $@"
                            Games_Played AS (
                                SELECT *,
                                CASE 
                                  WHEN outs = 0 THEN NULL
                                  ELSE ROUND((earned_runs * 9.0) / (outs / 3.0), 2)
                                END AS era,
                                CASE 
                                  WHEN outs = 0 THEN NULL
                                  ELSE ROUND((base_on_balls + hits) / (outs / 3.0), 2)
                                END AS whip,
                                CASE
                                    WHEN at_bats = 0
                                    THEN 0
                                    ELSE CAST(hits AS FLOAT) / at_bats
                                END AS average,
                                hits - (doubles + triples + home_runs) AS singles,
                                CASE
                                  WHEN outs = 0 THEN NULL
                                  ELSE ROUND(9 * strike_outs / (outs / 3.0), 2)
                                END AS strikeouts_per9,
                                CASE
                                  WHEN outs = 0 THEN NULL
                                  ELSE ROUND(9 * base_on_balls / (outs / 3.0), 2)
                                END AS walks_per9
                                FROM {tableName}
                                WHERE person_id = @person_id
                                AND {tableName}.games_played > 0
                            )
                            SELECT Games_Played.*, {mlbGamesTable}.game_date, {mlbGamesTable}.home_team_id, {mlbGamesTable}.away_team_id
                            FROM Games_Played 
                            JOIN {mlbGamesTable}
                            ON Games_Played.game_pk = CAST({mlbGamesTable}.game_pk AS INT)
                            ORDER BY Games_Played.id DESC";
                    }
                    boxScores = await _context.MLBPitchingBoxScoreWithGameDates
                        .FromSqlRaw(query,
                            new NpgsqlParameter("@person_id", player_id),
                            new NpgsqlParameter("@selectedOpponent", selectedOpponentObject?.Team_id)
                        )
                        .ToListAsync<object>();
                }
                Console.WriteLine(query);

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
