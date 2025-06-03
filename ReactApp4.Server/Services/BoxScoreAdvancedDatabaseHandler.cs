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
using ReactApp4.Server.Helpers;


namespace ReactApp4.Server.Services
{
    public class BoxScoreAdvancedDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public BoxScoreAdvancedDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ActionResult<IEnumerable<BoxScoreAdvanced>>> GetBoxScoreAdvancedBySeason(string season)
        {
            // Whitelist validation to prevent SQL injection
            if (!SeasonConstants.NBAAllowedSeasons.Contains(season))
            {
                return BadRequest("Invalid season format.");
            }

            var tableName = $"box_score_advanced_{season}";
            var query = $"SELECT * FROM {tableName}";

            // Execute safe raw SQL query
            var boxScoreAdvancedBySeason = await _context.BoxScoreAdvanceds
                .FromSqlRaw(query)
                .ToListAsync();

            return boxScoreAdvancedBySeason;
        }


        public async Task<IActionResult> CreateBoxScoreAdvanced([FromBody] BoxScoreAdvanced boxScoreAdvanced, string season)
        {
            // Implement logic to create a new league game in the database
            try
            {

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
            		var tableName = $"box_score_advanced_{season}";
                    var sql = $"INSERT INTO {tableName} (game_id, team_id, team_abbreviation, team_city, player_id, player_name, nickname, start_position, comment, min, e_off_rating, off_rating, e_def_rating, def_rating, e_net_rating, net_rating, ast_pct, ast_tov, ast_ratio, oreb_pct, dreb_pct, reb_pct, tm_tov_pct, efg_pct, ts_pct, usg_pct, e_usg_pct, e_pace, pace, pace_per40, poss, pie ) VALUES (@game_id, @team_id, @team_abbreviation, @team_city, @player_id, @player_name, @nickname, @start_position, @comment, @min, @e_off_rating, @off_rating, @e_def_rating, @def_rating, @e_net_rating, @net_rating, @ast_pct, @ast_tov, @ast_ratio, @oreb_pct, @dreb_pct, @reb_pct, @tm_tov_pct, @efg_pct, @ts_pct, @usg_pct, @e_usg_pct, @e_pace, @pace, @pace_per40, @poss, @pie);";

                    //var fgaValue = string.IsNullOrEmpty(boxScoreAdvanced.Fga) ? null : boxScoreAdvanced.Fga;
                    //boxScoreAdvanced.CheckAndReplace();

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@game_id", boxScoreAdvanced.Game_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_id", boxScoreAdvanced.Team_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_abbreviation", boxScoreAdvanced.Team_abbreviation ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_city", boxScoreAdvanced.Team_city ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_id", boxScoreAdvanced.Player_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_name", boxScoreAdvanced.Player_name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@nickname", boxScoreAdvanced.Nickname ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@start_position", boxScoreAdvanced.Start_position ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@comment", boxScoreAdvanced.Comment ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@min", boxScoreAdvanced.Min ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@e_off_rating", boxScoreAdvanced.E_off_rating ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@off_rating", boxScoreAdvanced.Off_rating ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@e_def_rating", boxScoreAdvanced.E_def_rating ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@def_rating", boxScoreAdvanced.Def_rating ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@e_net_rating", boxScoreAdvanced.E_net_rating ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@net_rating", boxScoreAdvanced.Net_rating ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ast_pct", boxScoreAdvanced.Ast_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ast_tov", boxScoreAdvanced.Ast_tov ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ast_ratio", boxScoreAdvanced.Ast_ratio ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@oreb_pct", boxScoreAdvanced.Oreb_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@dreb_pct", boxScoreAdvanced.Dreb_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@reb_pct", boxScoreAdvanced.Reb_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tm_tov_pct", boxScoreAdvanced.Tm_tov_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@efg_pct", boxScoreAdvanced.Efg_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ts_pct", boxScoreAdvanced.Ts_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@usg_pct", boxScoreAdvanced.Usg_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@e_usg_pct", boxScoreAdvanced.E_usg_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@e_pace", boxScoreAdvanced.E_pace ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pace", boxScoreAdvanced.Pace ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pace_per40", boxScoreAdvanced.Pace_per40 ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@poss", boxScoreAdvanced.Poss ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pie", boxScoreAdvanced.Pie ?? (object)DBNull.Value);


                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, boxScoreAdvanced);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }

}

