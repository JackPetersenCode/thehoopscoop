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

        public async Task<IActionResult> GetPlayerResults(string selectedSeason, decimal overUnderLine, string selectedOpponent, string player_id, string propBetStats)
        {
            //percentage of games over/under the line
            var decodedPropBetStatsJson = System.Net.WebUtility.UrlDecode(propBetStats);

            // Deserialize the JSON strings into lists of objects
            var propBetStatsList = JsonConvert.DeserializeObject<List<PropBetStats>>(decodedPropBetStatsJson);

            var decodedSelectedOpponentJson = System.Net.WebUtility.UrlDecode(selectedOpponent);

            // Deserialize the JSON strings into lists of objects
            var selectedOpponentObject = JsonConvert.DeserializeObject<NBATeam>(decodedSelectedOpponentJson);

            // Now you can work with 'rosterList' and 'propBetStatsList' as lists of objects
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
                        
                if (propBetStatsList != null)
                {
                    if (selectedOpponentObject != null)
                    {
                        query += " AND ";
                    } else
                    {
                        query += " WHERE ";
                    }
                    foreach (var stat in propBetStatsList)
                    {
                        query += $"Games_Played.{stat.Accessor} + ";
                    }
                    // Remove the trailing comma and space
                    query = query.TrimEnd(' ', '+');
                    query += $@" > @overUnderLine";

                }

                Console.WriteLine(query);
                //var playerId = new NpgsqlParameter("@player_id", NpgsqlDbType.Text);
                //playerId.Value = player_id.ToString();
                //var OULine = new NpgsqlParameter("@overUnderLine", NpgsqlDbType.Numeric);
                //OULine.Value = overUnderLine;

                var boxScores = await _context.BoxScoreWithGameDates.FromSqlRaw(query,
                        new NpgsqlParameter("@player_id", player_id),
                        new NpgsqlParameter("@overUnderLine", overUnderLine))
                    .ToListAsync();

                Console.WriteLine(boxScores.ToString());
                Console.WriteLine(boxScores);
                Console.Write(boxScores);
                return Ok(boxScores);
                //using (var cmd = new NpgsqlCommand(query, connection))
                //{
                //    cmd.Parameters.Add(playerId);
                //    cmd.Parameters.Add(OULine);
                //    // Create parameter for the array of strings
                //    using (var reader = await cmd.ExecuteReaderAsync())
                //    {
                //        var resultList = new List<object[]>();
                //
                //        while (await reader.ReadAsync())
                //        {
                //            var rowData = new object[reader.FieldCount];
                //            reader.GetValues(rowData);
                //            resultList.Add(rowData);
                //        }
                //        if (resultList != null)
                //        {
                //            return Ok(resultList);
                //
                //        } else
                //        {
                //            return NotFound(); // Return 404 if no games played found
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }
}
