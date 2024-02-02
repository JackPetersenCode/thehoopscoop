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
    public class BoxScoreScoringDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<ActionResult<IEnumerable<BoxScoreScoring>>> GetBoxScoreScoringBySeason(string season)
        {
            var tableName = $"box_score_scoring_{season}";

            var query = $"SELECT * FROM {tableName}";

            var boxScoreScoringBySeason = await _context.BoxScoreScorings.FromSqlRaw(query).ToListAsync();

            return boxScoreScoringBySeason;
        }

        public async Task<IActionResult> CreateBoxScoreScoring([FromBody] BoxScoreScoring boxScoreScoring, string season)
        {
            // Implement logic to create a new league game in the database


            try
            {
                if (boxScoreScoring == null)
                {
                    return BadRequest("Invalid boxScoreScoring data");
                }

                var connectionString = "Server=localhost;Port=5432;Database=hoop_scoop;User Id=postgres;Password=redsox45;\r\n"; // Replace with your actual connection string

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = $"INSERT INTO box_score_scoring_{season} (game_id, team_id, team_abbreviation, team_city, player_id, player_name, nickname, start_position, comment, min, pct_fga_2pt, pct_fga_3pt, pct_pts_2pt, pct_pts_2pt_mr, pct_pts_3pt, pct_pts_fb, pct_pts_ft, pct_pts_off_tov, pct_pts_paint, pct_ast_2pm, pct_uast_2pm, pct_ast_3pm, pct_uast_3pm, pct_ast_fgm ) VALUES (@game_id, @team_id, @team_abbreviation, @team_city, @player_id, @player_name, @nickname, @start_position, @comment, @min, @pct_fga_2pt, @pct_fga_3pt, @pct_pts_2pt, @pct_pts_2pt_mr, @pct_pts_3pt, @pct_pts_fb, @pct_pts_ft, @pct_pts_off_tov, @pct_pts_paint, @pct_ast_2pm, @pct_uast_2pm, @pct_ast_3pm, @pct_uast_3pm, @pct_ast_fgm);";

                    //var fgaValue = string.IsNullOrEmpty(boxScoreScoring.Fga) ? null : boxScoreScoring.Fga;
                    //boxScoreScoring.CheckAndReplace();



                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@game_id", boxScoreScoring.Game_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_id", boxScoreScoring.Team_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_abbreviation", boxScoreScoring.Team_abbreviation ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_city", boxScoreScoring.Team_city ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_id", boxScoreScoring.Player_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_name", boxScoreScoring.Player_name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@nickname", boxScoreScoring.Nickname ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@start_position", boxScoreScoring.Start_position ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@comment", boxScoreScoring.Comment ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@min", boxScoreScoring.Min ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_fga_2pt", boxScoreScoring.Pct_fga_2pt ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_fga_3pt", boxScoreScoring.Pct_fga_3pt ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_pts_2pt", boxScoreScoring.Pct_pts_2pt ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_pts_2pt_mr", boxScoreScoring.Pct_pts_2pt_mr ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_pts_3pt", boxScoreScoring.Pct_pts_3pt ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_pts_fb", boxScoreScoring.Pct_pts_fb ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_pts_ft", boxScoreScoring.Pct_pts_ft ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_pts_off_tov", boxScoreScoring.Pct_pts_off_tov ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_pts_paint", boxScoreScoring.Pct_pts_paint ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_ast_2pm", boxScoreScoring.Pct_ast_2pm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_uast_2pm", boxScoreScoring.Pct_uast_2pm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_ast_3pm", boxScoreScoring.Pct_ast_3pm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_uast_3pm", boxScoreScoring.Pct_uast_3pm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pct_ast_fgm", boxScoreScoring.Pct_ast_fgm ?? (object)DBNull.Value);


                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, boxScoreScoring);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }

}

