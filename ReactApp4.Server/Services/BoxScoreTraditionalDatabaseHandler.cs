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
    public class BoxScoreTraditionalDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<ActionResult<IEnumerable<BoxScoreTraditional>>> GetBoxScoreTraditionalBySeason(string season)
        {
            var tableName = $"box_score_traditional_{season}";

            var query = $"SELECT * FROM {tableName} LIMIT 1";

            var boxScoreTraditionalBySeason = await _context.BoxScoreTraditionals.FromSqlRaw(query).ToListAsync();

            return boxScoreTraditionalBySeason;
        }
        public async Task<ActionResult<IEnumerable<SelectedPlayer>>> GetRosterBySeasonByTeam(string season, string teamId)
        {
            Console.WriteLine(season);
            Console.WriteLine(teamId);
            var tableName = $"box_score_traditional_{season}";

            var query = $@"SELECT DISTINCT(player_id), player_name, team_id, team_abbreviation FROM {tableName} 
                        WHERE team_id LIKE '%{teamId}%'";

            var roster = await _context.SelectedPlayers.FromSqlRaw(query).ToListAsync();

            return roster;
        }

        public async Task<IActionResult> CreateBoxScoreTraditional([FromBody] BoxScoreTraditional boxScoreTraditional, string season)
        {
            // Implement logic to create a new league game in the database


            try
            {
                if (boxScoreTraditional == null)
                {
                    return BadRequest("Invalid boxScoreTraditional data");
                }

                var connectionString = "Server=localhost;Port=5432;Database=hoop_scoop;User Id=postgres;Password=redsox45;\r\n"; // Replace with your actual connection string

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = $"INSERT INTO box_score_traditional_{season} (game_id, team_id, team_abbreviation, team_city, player_id, player_name, nickname, start_position, comment, min, fgm, fga, fg_pct, fg3m, fg3a, fg3_pct, ftm, fta, ft_pct, oreb, dreb, reb, ast, stl, blk, tov, pf, pts, plus_minus) VALUES (@game_id, @team_id, @team_abbreviation, @team_city, @player_id, @player_name, @nickname, @start_position, @comment, @min, @fgm, @fga, @fg_pct, @fg3m, @fg3a, @fg3_pct, @ftm, @fta, @ft_pct, @oreb, @dreb, @reb, @ast, @stl, @blk, @tov, @pf, @pts, @plus_minus);";

                    //var fgaValue = string.IsNullOrEmpty(boxScoreTraditional.Fga) ? null : boxScoreTraditional.Fga;
                    //boxScoreTraditional.CheckAndReplace();



                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@game_id", boxScoreTraditional.Game_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_id", boxScoreTraditional.Team_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_abbreviation", boxScoreTraditional.Team_abbreviation ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_city", boxScoreTraditional.Team_city ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_id", boxScoreTraditional.Player_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_name", boxScoreTraditional.Player_name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@nickname", boxScoreTraditional.Nickname ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@start_position", boxScoreTraditional.Start_position ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@comment", boxScoreTraditional.Comment ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@min", boxScoreTraditional.Min ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fgm", boxScoreTraditional.Fgm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fga", boxScoreTraditional.Fga ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fg_pct", boxScoreTraditional.Fg_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fg3m", boxScoreTraditional.Fg3m ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fg3a", boxScoreTraditional.Fg3a ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fg3_pct", boxScoreTraditional.Fg3_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ftm", boxScoreTraditional.Ftm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fta", boxScoreTraditional.Fta ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ft_pct", boxScoreTraditional.Ft_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@oreb", boxScoreTraditional.Oreb ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@dreb", boxScoreTraditional.Dreb ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@reb", boxScoreTraditional.Reb ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ast", boxScoreTraditional.Ast ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@stl", boxScoreTraditional.Stl ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@blk", boxScoreTraditional.Blk ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tov", boxScoreTraditional.Tov ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pf", boxScoreTraditional.Pf ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pts", boxScoreTraditional.Pts ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@plus_minus", boxScoreTraditional.Plus_minus ?? (object)DBNull.Value);


                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, boxScoreTraditional);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }

}

