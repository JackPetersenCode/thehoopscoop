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
    public class PropBetResultsDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<IActionResult> GetPropBetResults(string selectedSeason, decimal overUnderLine, string selectedOpponent, string roster, string propBetStats)
        {
            //percentage of games over/under the line
            var decodedRosterJson = System.Net.WebUtility.UrlDecode(roster);
            var decodedPropBetStatsJson = System.Net.WebUtility.UrlDecode(propBetStats);

            // Deserialize the JSON strings into lists of objects
            var rosterList = JsonConvert.DeserializeObject<List<Player>>(decodedRosterJson);
            var propBetStatsList = JsonConvert.DeserializeObject<List<PropBetStats>>(decodedPropBetStatsJson);

            // Now you can work with 'rosterList' and 'propBetStatsList' as lists of objects

            // Example: Logging the contents of the 'rosterList'
            if (rosterList != null)
            {
                var playerIds = rosterList.Select(player => player.Player_id).ToList();

                var query = @"
                            WITH box_scores AS (
                                SELECT *
                                FROM box_score_traditional_2023_24
                                WHERE player_id = ANY(@player_ids)
                                GROUP BY player_id
                            )
                            ";
                            
                // Create parameter for the array of strings
                var parameter = new NpgsqlParameter("@player_ids", NpgsqlDbType.Array | NpgsqlDbType.Text);
                parameter.Value = playerIds.ToArray();

                var boxScores = await _context.BoxScoreTraditionals
                    .FromSqlRaw(query, parameter)
                    .ToListAsync();

                return Ok(boxScores);

            }

            return Ok("roster is null");
        }
    }
}
