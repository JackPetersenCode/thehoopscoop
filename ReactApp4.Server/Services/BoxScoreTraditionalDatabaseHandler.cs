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

namespace ReactApp4.Server.Services
{
    public class BoxScoreTraditionalDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<ActionResult<IEnumerable<BoxScoreTraditional>>> GetBoxScoreTraditionalBySeason(string season)
        {
            var tableName = $"box_score_traditional_{season}";
            Console.WriteLine(tableName);

            var query = $"SELECT * FROM {tableName}";

            var boxScoreTraditionalBySeason = await _context.BoxScoreTraditionals.FromSqlRaw(query).ToListAsync();

            Console.WriteLine(boxScoreTraditionalBySeason);

            return boxScoreTraditionalBySeason;
        }

        public async Task<IActionResult> CreateBoxScoreTraditional([FromBody] BoxScoreTraditional boxScoreTraditional, string season)
        {
            // Implement logic to create a new league game in the database


            try
            {
                Console.Write(boxScoreTraditional.Fga);
                if (boxScoreTraditional == null)
                {
                    return BadRequest("Invalid boxScoreTraditional data");
                }



                var connectionString = "Server=localhost;Port=5432;Database=hoop_scoop;User Id=postgres;Password=redsox45;\r\n"; // Replace with your actual connection string

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = $"INSERT INTO box_score_traditional_{season} (game_id, team_id, team_abbreviation, team_city, player_id, player_name, nickname, start_position, comment, min, fgm, fga, fg_pct, fg3m, fg3a, fg3_pct, ftm, fta, ft_pct, oreb, dreb, reb, ast, stl, blk, tov, pf, pts, plus_minus) VALUES (@game_id, @team_id, @team_abbreviation, @team_city, @player_id, @player_name, @nickname, @start_position, @comment, @min, @fgm, @fga, @fg_pct, @fg3m, @fg3a, @fg3_pct, @ftm, @fta, @ft_pct, @oreb, @dreb, @reb, @ast, @stl, @blk, @tov, @pf, @pts, @plus_minus);";

                    Console.Write(sql);
                    //var fgaValue = string.IsNullOrEmpty(boxScoreTraditional.Fga) ? null : boxScoreTraditional.Fga;
                    //boxScoreTraditional.CheckAndReplace();

                    string? gameIdString = boxScoreTraditional.Game_id?.ToString();
                    NpgsqlParameter gameIdParam = new NpgsqlParameter("@game_id", NpgsqlDbType.Text);
                    gameIdParam.Value = gameIdString;

                    string? teamIdString = boxScoreTraditional.Team_id?.ToString();
                    NpgsqlParameter teamIdParam = new NpgsqlParameter("@team_id", NpgsqlDbType.Text);
                    teamIdParam.Value = teamIdString;

                    string? teamAbbreviationString = boxScoreTraditional.Team_abbreviation?.ToString();
                    NpgsqlParameter teamAbbreviationParam = new NpgsqlParameter("@team_abbreviation", NpgsqlDbType.Text);
                    teamAbbreviationParam.Value = teamAbbreviationString;

                    string? teamCityString = boxScoreTraditional.Team_city?.ToString();
                    NpgsqlParameter teamCityParam = new NpgsqlParameter("@team_city", NpgsqlDbType.Text);
                    teamCityParam.Value = teamCityString;

                    string? playerIdString = boxScoreTraditional.Player_id?.ToString();
                    NpgsqlParameter playerIdParam = new NpgsqlParameter("@player_id", NpgsqlDbType.Text);
                    playerIdParam.Value = playerIdString;

                    string? playerNameString = boxScoreTraditional.Player_name?.ToString();
                    NpgsqlParameter playerNameParam = new NpgsqlParameter("@player_name", NpgsqlDbType.Text);
                    playerNameParam.Value = playerNameString;

                    string? nicknameString = boxScoreTraditional.Nickname?.ToString();
                    NpgsqlParameter nicknameParam = new NpgsqlParameter("@nickname", NpgsqlDbType.Text);
                    nicknameParam.Value = nicknameString;

                    string? startPositionString = boxScoreTraditional.Start_position?.ToString();
                    NpgsqlParameter startPositionParam = new NpgsqlParameter("@start_position", NpgsqlDbType.Text);
                    startPositionParam.Value = startPositionString;

                    string? commentString = boxScoreTraditional.Comment?.ToString();
                    NpgsqlParameter commentParam = new NpgsqlParameter("@comment", NpgsqlDbType.Text);
                    commentParam.Value = commentString;

                    string? min_string = boxScoreTraditional.Min?.ToString();
                    decimal? min = !string.IsNullOrEmpty(min_string) ? JsonConvert.DeserializeObject<decimal>(min_string) : (decimal?)null;
                    string? fgm_string = boxScoreTraditional.Fgm?.ToString();
                    decimal? fgm = !string.IsNullOrEmpty(fgm_string) ? JsonConvert.DeserializeObject<decimal>(fgm_string) : (decimal?)null;
                    string? fga_string = boxScoreTraditional.Fga?.ToString();
                    decimal? fga = !string.IsNullOrEmpty(fga_string) ? JsonConvert.DeserializeObject<decimal>(fga_string) : (decimal?)null;
                    string? fg_pct_string = boxScoreTraditional.Fg_pct?.ToString();
                    decimal? fg_pct = !string.IsNullOrEmpty(fg_pct_string) ? JsonConvert.DeserializeObject<decimal>(fg_pct_string) : (decimal?)null;
                    string? fg3m_string = boxScoreTraditional.Fg3m?.ToString();
                    decimal? fg3m = !string.IsNullOrEmpty(fg3m_string) ? JsonConvert.DeserializeObject<decimal>(fg3m_string) : (decimal?)null;
                    string? fg3a_string = boxScoreTraditional.Fg3a?.ToString();
                    decimal? fg3a = !string.IsNullOrEmpty(fg3a_string) ? JsonConvert.DeserializeObject<decimal>(fg3a_string) : (decimal?)null;
                    string? fg3_pct_string = boxScoreTraditional.Fg3_pct?.ToString();
                    decimal? fg3_pct = !string.IsNullOrEmpty(fg3_pct_string) ? JsonConvert.DeserializeObject<decimal>(fg3_pct_string) : (decimal?)null;
                    string? ftm_string = boxScoreTraditional.Ftm?.ToString();
                    decimal? ftm = !string.IsNullOrEmpty(ftm_string) ? JsonConvert.DeserializeObject<decimal>(ftm_string) : (decimal?)null;
                    string? fta_string = boxScoreTraditional.Fta?.ToString();
                    decimal? fta = !string.IsNullOrEmpty(fta_string) ? JsonConvert.DeserializeObject<decimal>(fta_string) : (decimal?)null;
                    string? ft_pct_string = boxScoreTraditional.Ft_pct?.ToString();
                    decimal? ft_pct = !string.IsNullOrEmpty(ft_pct_string) ? JsonConvert.DeserializeObject<decimal>(ft_pct_string) : (decimal?)null;
                    string? oreb_string = boxScoreTraditional.Oreb?.ToString();
                    decimal? oreb = !string.IsNullOrEmpty(oreb_string) ? JsonConvert.DeserializeObject<decimal>(oreb_string) : (decimal?)null;
                    string? dreb_string = boxScoreTraditional.Dreb?.ToString();
                    decimal? dreb = !string.IsNullOrEmpty(dreb_string) ? JsonConvert.DeserializeObject<decimal>(dreb_string) : (decimal?)null;
                    string? reb_string = boxScoreTraditional.Reb?.ToString();
                    decimal? reb = !string.IsNullOrEmpty(reb_string) ? JsonConvert.DeserializeObject<decimal>(reb_string) : (decimal?)null;
                    string? ast_string = boxScoreTraditional.Ast?.ToString();
                    decimal? ast = !string.IsNullOrEmpty(ast_string) ? JsonConvert.DeserializeObject<decimal>(ast_string) : (decimal?)null;
                    string? stl_string = boxScoreTraditional.Stl?.ToString();
                    decimal? stl = !string.IsNullOrEmpty(stl_string) ? JsonConvert.DeserializeObject<decimal>(stl_string) : (decimal?)null;
                    string? blk_string = boxScoreTraditional.Blk?.ToString();
                    decimal? blk = !string.IsNullOrEmpty(blk_string) ? JsonConvert.DeserializeObject<decimal>(blk_string) : (decimal?)null;
                    string? tov_string = boxScoreTraditional.Tov?.ToString();
                    decimal? tov = !string.IsNullOrEmpty(tov_string) ? JsonConvert.DeserializeObject<decimal>(tov_string) : (decimal?)null;
                    string? pf_string = boxScoreTraditional.Pf?.ToString();
                    decimal? pf = !string.IsNullOrEmpty(pf_string) ? JsonConvert.DeserializeObject<decimal>(pf_string) : (decimal?)null;
                    string? pts_string = boxScoreTraditional.Pts?.ToString();
                    decimal? pts = !string.IsNullOrEmpty(pts_string) ? JsonConvert.DeserializeObject<decimal>(pts_string) : (decimal?)null;
                    string? plus_minus_string = boxScoreTraditional.Plus_minus?.ToString();
                    decimal? plus_minus = !string.IsNullOrEmpty(plus_minus_string) ? JsonConvert.DeserializeObject<decimal>(plus_minus_string) : (decimal?)null;

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.Add(gameIdParam);
                        cmd.Parameters.Add(teamIdParam);
                        cmd.Parameters.Add(teamAbbreviationParam);
                        cmd.Parameters.Add(teamCityParam);
                        cmd.Parameters.Add(playerIdParam);
                        cmd.Parameters.Add(playerNameParam);
                        cmd.Parameters.Add(nicknameParam);
                        cmd.Parameters.Add(startPositionParam);
                        cmd.Parameters.Add(commentParam);
                        cmd.Parameters.AddWithValue("@min", min ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fgm", fgm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fga", fga ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fg_pct", fg_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fg3m", fg3m ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fg3a", fg3a ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fg3_pct", fg3_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ftm", ftm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@fta", fta ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ft_pct", ft_pct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@oreb", oreb ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@dreb", dreb ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@reb", reb ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ast", ast ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@stl", stl ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@blk", blk ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@tov", tov ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pf", pf ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@pts", pts ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@plus_minus", plus_minus ?? (object)DBNull.Value);

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

