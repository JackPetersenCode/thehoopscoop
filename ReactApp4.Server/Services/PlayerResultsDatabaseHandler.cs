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

            // Now you can work with 'rosterList' and 'propBetStatsList' as lists of objects
            try
            {

                var tableName = "box_score_traditional_" + selectedSeason;

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
                        SELECT *
                        FROM Games_Played 
                        ";
                if (propBetStatsList != null)
                {
                    query += " WHERE ";
                    foreach (var stat in propBetStatsList)
                    {
                        query += $"{stat.Accessor} + ";
                    }
                    // Remove the trailing comma and space
                    query = query.TrimEnd(' ', '+');
                    query += $@" > @overUnderLine";

                }
                else
                {

                }
                Console.WriteLine(query);
                //var playerId = new NpgsqlParameter("@player_id", NpgsqlDbType.Text);
                //playerId.Value = player_id.ToString();
                //var OULine = new NpgsqlParameter("@overUnderLine", NpgsqlDbType.Numeric);
                //OULine.Value = overUnderLine;

                var boxScores = await _context.BoxScoreTraditionals.FromSqlRaw(query, new NpgsqlParameter("@player_id", player_id), new NpgsqlParameter("@overUnderLine", overUnderLine)).ToListAsync();
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
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }
}
