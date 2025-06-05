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
    public class BoxScoreSummaryDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public BoxScoreSummaryDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ActionResult<IEnumerable<BoxScoreSummary>>> GetBoxScoreSummaryBySeason(string season)
        {
            var tableName = $"box_score_summary_{season}";

            var query = $"SELECT * FROM {tableName}";

            var boxScoreSummaryBySeason = await _context.BoxScoreSummarys.FromSqlRaw(query).ToListAsync();

            return boxScoreSummaryBySeason;
        }

        public async Task<IActionResult> CreateBoxScoreSummary([FromBody] BoxScoreSummary boxScoreSummary, string season)
        {
            // Implement logic to create a new league game in the database
            try
            {
                if (boxScoreSummary == null)
                {
                    return BadRequest("Invalid boxScoreSummary data");
                }

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();


                    var sql = $@"INSERT INTO box_score_summary_{season} (
                    game_date_est,
                    game_sequence, 
                    game_id,
                    game_status_id, 
                    game_status_text, 
                    gamecode,
                    home_team_id,
                    visitor_team_id,
                    season,
                    live_period,
                    live_pc_time,
                    natl_tv_broadcaster_abbreviation,
                    live_period_time_bcast,
                    wh_status
                    ) VALUES (
                        @game_date_est,
                        @game_sequence,
                        @game_id,
                        @game_status_id,
                        @game_status_text,
                        @gamecode,
                        @home_team_id,
                        @visitor_team_id,
                        @season,
                        @live_period,
                        @live_pc_time,
                        @natl_tv_broadcaster_abbreviation,
                        @live_period_time_bcast, 
                        @wh_status)";  
                    //var fgaValue = string.IsNullOrEmpty(boxScoreSummary.Fga) ? null : boxScoreSummary.Fga;
                    //boxScoreSummary.CheckAndReplace();



                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@game_date_est", boxScoreSummary.Game_date_est ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@game_sequence", boxScoreSummary.Game_sequence ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@game_id", boxScoreSummary.Game_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@game_status_id", boxScoreSummary.Game_status_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@game_status_text", boxScoreSummary.Game_status_text ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@gamecode", boxScoreSummary.Gamecode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_team_id", boxScoreSummary.Home_team_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@visitor_team_id", boxScoreSummary.Visitor_team_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@season", boxScoreSummary.Season ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@live_period", boxScoreSummary.Live_period ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@live_pc_time", boxScoreSummary.Live_pc_time ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@natl_tv_broadcaster_abbreviation", boxScoreSummary.Natl_tv_broadcaster_abbreviation ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@live_period_time_bcast", boxScoreSummary.Live_period_time_bcast ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@wh_status", boxScoreSummary.Wh_status ?? (object)DBNull.Value);



                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, boxScoreSummary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }

}

