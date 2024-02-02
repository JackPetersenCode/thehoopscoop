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
    public class BoxScoreMiscDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<ActionResult<IEnumerable<BoxScoreMisc>>> GetBoxScoreMiscBySeason(string season)
        {
            var tableName = $"box_score_misc_{season}";

            var query = $"SELECT * FROM {tableName}";

            var boxScoreMiscBySeason = await _context.BoxScoreMiscs.FromSqlRaw(query).ToListAsync();

            return boxScoreMiscBySeason;
        }

        public async Task<IActionResult> CreateBoxScoreMisc([FromBody] BoxScoreMisc boxScoreMisc, string season)
        {
            // Implement logic to create a new league game in the database


            try
            {
                if (boxScoreMisc == null)
                {
                    return BadRequest("Invalid boxScoreMisc data");
                }

                var connectionString = "Server=localhost;Port=5432;Database=hoop_scoop;User Id=postgres;Password=redsox45;\r\n"; // Replace with your actual connection string

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = $"INSERT INTO box_score_misc_{season} (game_id, team_id, team_abbreviation, team_city, player_id, player_name, nickname, start_position, comment, min, pts_off_tov, pts_2nd_chance, pts_fb, pts_paint, opp_pts_off_tov, opp_pts_2nd_chance, opp_pts_fb, opp_pts_paint, blk, blka, pf, pfd) VALUES (@game_id, @team_id, @team_abbreviation, @team_city, @player_id, @player_name, @nickname, @start_position, @comment, @min, @pts_off_tov, @pts_2nd_chance, @pts_fb, @pts_paint, @opp_pts_off_tov, @opp_pts_2nd_chance, @opp_pts_fb, @opp_pts_paint, @blk, @blka, @pf, @pfd);";

                    //var fgaValue = string.IsNullOrEmpty(boxScoreMisc.Fga) ? null : boxScoreMisc.Fga;
                    //boxScoreMisc.CheckAndReplace();



                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@game_id", boxScoreMisc.Game_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_id", boxScoreMisc.Team_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_abbreviation", boxScoreMisc.Team_abbreviation ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_city", boxScoreMisc.Team_city ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_id", boxScoreMisc.Player_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_name", boxScoreMisc.Player_name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@nickname", boxScoreMisc.Nickname ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@start_position", boxScoreMisc.Start_position ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@comment", boxScoreMisc.Comment ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@min", boxScoreMisc.Min ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pts_off_tov", boxScoreMisc.Pts_off_tov ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pts_2nd_chance", boxScoreMisc.Pts_2nd_chance ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pts_fb", boxScoreMisc.Pts_fb ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pts_paint", boxScoreMisc.Pts_paint ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@opp_pts_off_tov", boxScoreMisc.Opp_pts_off_tov ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@opp_pts_2nd_chance", boxScoreMisc.Opp_pts_2nd_chance ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@opp_pts_fb", boxScoreMisc.Opp_pts_fb ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@opp_pts_paint", boxScoreMisc.Opp_pts_paint ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@blk", boxScoreMisc.Blk ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@blka", boxScoreMisc.Blka ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pf", boxScoreMisc.Pf ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pfd", boxScoreMisc.Pfd ?? (object)DBNull.Value);


                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, boxScoreMisc);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }

}

