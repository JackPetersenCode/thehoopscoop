using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using ReactApp4.Server.Data;
using Microsoft.AspNetCore.Http;
using System;
using static System.Net.WebRequestMethods;
using System.Runtime.Intrinsics.X86;
using System.Diagnostics;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class BoxScoreFourFactorsDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public BoxScoreFourFactorsDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ActionResult<IEnumerable<BoxScoreFourFactors>>> GetBoxScoreFourFactorsBySeason(string season)
        {
            var tableName = $"box_score_fourfactors_{season}";

            var query = $"SELECT * FROM {tableName}";

            var boxScoreFourFactorsBySeason = await _context.BoxScoreFourFactorss.FromSqlRaw(query).ToListAsync();

            return boxScoreFourFactorsBySeason;
        }

        public async Task<IActionResult> CreateBoxScoreFourFactors([FromBody] BoxScoreFourFactors boxScoreFourFactors, string season)
        {
            // Implement logic to create a new league game in the database


            try
            {
                if (boxScoreFourFactors == null)
                {
                    return BadRequest("Invalid boxScoreFourFactors data");
                }

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = $"INSERT INTO box_score_fourfactors_{season} (game_id, team_id, team_abbreviation, team_city, player_id, player_name, nickname, start_position, comment, min, efg_pct, fta_rate, tm_tov_pct, oreb_pct, opp_efg_pct, opp_fta_rate, opp_tov_pct, opp_oreb_pct) VALUES (@game_id, @team_id, @team_abbreviation, @team_city, @player_id, @player_name, @nickname, @start_position, @comment, @min, @efg_pct, @fta_rate, @tm_tov_pct, @oreb_pct, @opp_efg_pct, @opp_fta_rate, @opp_tov_pct, @opp_oreb_pct);";

                    //var fgaValue = string.IsNullOrEmpty(boxScoreFourFactors.Fga) ? null : boxScoreFourFactors.Fga;
                    //boxScoreFourFactors.CheckAndReplace();



                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@game_id", boxScoreFourFactors.Game_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_id", boxScoreFourFactors.Team_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_abbreviation", boxScoreFourFactors.Team_abbreviation ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_city", boxScoreFourFactors.Team_city ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_id", boxScoreFourFactors.Player_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_name", boxScoreFourFactors.Player_name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@nickname", boxScoreFourFactors.Nickname ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@start_position", boxScoreFourFactors.Start_position ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@comment", boxScoreFourFactors.Comment ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@min", boxScoreFourFactors.Min ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@efg_pct", boxScoreFourFactors.Efg_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fta_rate", boxScoreFourFactors.Fta_rate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tm_tov_pct", boxScoreFourFactors.Tm_tov_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@oreb_pct", boxScoreFourFactors.Oreb_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@opp_efg_pct", boxScoreFourFactors.Opp_efg_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@opp_fta_rate", boxScoreFourFactors.Opp_fta_rate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@opp_tov_pct", boxScoreFourFactors.Opp_tov_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@opp_oreb_pct", boxScoreFourFactors.Opp_oreb_pct ?? (object)DBNull.Value);



                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, boxScoreFourFactors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }

}


