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

namespace ReactApp4.Server.Services
{
    public class LeagueGameDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public LeagueGameDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ActionResult<IEnumerable<LeagueGame>>> GetGamesBySeason(string season)
        {
            var tableName = $"league_games_{season}";
            Console.WriteLine(tableName);

            var query = $"SELECT * FROM {tableName}";

            var gamesBySeason = await _context.LeagueGames.FromSqlRaw(query).ToListAsync();

            Console.WriteLine(gamesBySeason);

            return gamesBySeason;
        }

        public async Task<ActionResult<IEnumerable<ShotChartsGame>>> GetShotChartsGames(string playerId, string season)
        {
            var tableName = $"league_games_{season}";
            Console.WriteLine(tableName);

            var query = $@"SELECT DISTINCT(league_games_{season}.game_id), 
                        league_games_{season}.game_date, 
                        matchup 
                        FROM league_games_{season}
                        INNER JOIN shots_{season}
                        ON league_games_{season}.game_id = shots_{season}.game_id
                        WHERE shots_{season}.player_id = '{playerId}'
                        AND matchup LIKE '%vs.%'
                        ORDER BY league_games_{season}.game_date";

            var games = await _context.ShotChartsGames.FromSqlRaw(query).ToListAsync();

            Console.WriteLine(games);

            return games;
        }
        public async Task<IActionResult> CreateLeagueGame([FromBody] object[] leagueGame)
        {
            // Implement logic to create a new league game in the database
            try
            {
                if (leagueGame == null)
                {
                    return BadRequest("Invalid leagueGame data");
                }

                var seasonString = leagueGame[0]?.ToString();
                var sub = seasonString?.Substring(1);
                if (!int.TryParse(sub, out int seasonId))
                {
                    return BadRequest("Invalid season_id value");
                }

                var season = $"{sub}_{(seasonId + 1).ToString().Substring(2)}";

                Console.WriteLine(season);

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    Console.WriteLine(season);
                    var sql = $"INSERT INTO league_games_{season} (season_id, team_id, team_abbreviation, team_name, game_id, game_date, matchup, wl, min, fgm, fga, fg_pct, fg3m, fg3a, fg3_pct, ftm, fta, ft_pct, oreb, dreb, reb, ast, stl, blk, tov, pf, pts, plus_minus, video_available) VALUES (@season_id, @team_id, @team_abbreviation, @team_name, @game_id, @game_date, @matchup, @wl, @min, @fgm, @fga, @fg_pct, @fg3m, @fg3a, @fg3_pct, @ftm, @fta, @ft_pct, @oreb, @dreb, @reb, @ast, @stl, @blk, @tov, @pf, @pts, @plus_minus, @video_available);";
                    Console.WriteLine(sql);
                    string? seasonIdString = leagueGame[0]?.ToString();
                    NpgsqlParameter seasonIdParam = new NpgsqlParameter("@season_id", NpgsqlDbType.Text);
                    seasonIdParam.Value = seasonIdString;

                    string? teamIdString = leagueGame[1]?.ToString();
                    NpgsqlParameter teamIdParam = new NpgsqlParameter("@team_id", NpgsqlDbType.Text);
                    teamIdParam.Value = teamIdString;

                    string? teamAbbreviationString = leagueGame[2]?.ToString();
                    NpgsqlParameter teamAbbreviationParam = new NpgsqlParameter("@team_abbreviation", NpgsqlDbType.Text);
                    teamAbbreviationParam.Value = teamAbbreviationString;

                    string? teamNameString = leagueGame[3]?.ToString();
                    NpgsqlParameter teamNameParam = new NpgsqlParameter("@team_name", NpgsqlDbType.Text);
                    teamNameParam.Value = teamNameString;

                    string? gameIdString = leagueGame[4]?.ToString();
                    NpgsqlParameter gameIdParam = new NpgsqlParameter("@game_id", NpgsqlDbType.Text);
                    gameIdParam.Value = gameIdString;

                    string? gameDateString = leagueGame[5]?.ToString();
                    NpgsqlParameter gameDateParam = new NpgsqlParameter("@game_date", NpgsqlDbType.Text);
                    gameDateParam.Value = gameDateString;

                    string? matchupString = leagueGame[6]?.ToString();
                    NpgsqlParameter matchupParam = new NpgsqlParameter("@matchup", NpgsqlDbType.Text);
                    matchupParam.Value = matchupString;

                    string? wlString = leagueGame[7]?.ToString();
                    NpgsqlParameter wlParam = new NpgsqlParameter("@wl", NpgsqlDbType.Text);
                    wlParam.Value = wlString;

                    string? min_string = leagueGame[8]?.ToString();
                    decimal? min = !string.IsNullOrEmpty(min_string) ? JsonConvert.DeserializeObject<decimal>(min_string) : (decimal?)null;
                    string fgm_string = leagueGame[9]?.ToString();
                    decimal? fgm = !string.IsNullOrEmpty(fgm_string) ? JsonConvert.DeserializeObject<decimal>(fgm_string) : (decimal?)null;
                    string fga_string = leagueGame[10]?.ToString();
                    decimal? fga = !string.IsNullOrEmpty(fga_string) ? JsonConvert.DeserializeObject<decimal>(fga_string) : (decimal?)null;
                    string fg_pct_string = leagueGame[11]?.ToString();
                    decimal? fg_pct = !string.IsNullOrEmpty(fg_pct_string) ? JsonConvert.DeserializeObject<decimal>(fg_pct_string) : (decimal?)null;
                    string fg3m_string = leagueGame[12]?.ToString();
                    decimal? fg3m = !string.IsNullOrEmpty(fg3m_string) ? JsonConvert.DeserializeObject<decimal>(fg3m_string) : (decimal?)null;
                    string fg3a_string = leagueGame[13]?.ToString();
                    decimal? fg3a = !string.IsNullOrEmpty(fg3a_string) ? JsonConvert.DeserializeObject<decimal>(fg3a_string) : (decimal?)null;
                    string fg3_pct_string = leagueGame[14]?.ToString();
                    decimal? fg3_pct = !string.IsNullOrEmpty(fg3_pct_string) ? JsonConvert.DeserializeObject<decimal>(fg3_pct_string) : (decimal?)null;
                    string ftm_string = leagueGame[15]?.ToString();
                    decimal? ftm = !string.IsNullOrEmpty(ftm_string) ? JsonConvert.DeserializeObject<decimal>(ftm_string) : (decimal?)null;
                    string fta_string = leagueGame[16]?.ToString();
                    decimal? fta = !string.IsNullOrEmpty(fta_string) ? JsonConvert.DeserializeObject<decimal>(fta_string) : (decimal?)null;
                    string ft_pct_string = leagueGame[17]?.ToString();
                    decimal? ft_pct = !string.IsNullOrEmpty(ft_pct_string) ? JsonConvert.DeserializeObject<decimal>(ft_pct_string) : (decimal?)null;
                    string oreb_string = leagueGame[18]?.ToString();
                    decimal? oreb = !string.IsNullOrEmpty(oreb_string) ? JsonConvert.DeserializeObject<decimal>(oreb_string) : (decimal?)null;
                    string dreb_string = leagueGame[19]?.ToString();
                    decimal? dreb = !string.IsNullOrEmpty(dreb_string) ? JsonConvert.DeserializeObject<decimal>(dreb_string) : (decimal?)null;
                    string reb_string = leagueGame[20]?.ToString();
                    decimal? reb = !string.IsNullOrEmpty(reb_string) ? JsonConvert.DeserializeObject<decimal>(reb_string) : (decimal?)null;
                    string ast_string = leagueGame[21]?.ToString();
                    decimal? ast = !string.IsNullOrEmpty(ast_string) ? JsonConvert.DeserializeObject<decimal>(ast_string) : (decimal?)null;
                    string stl_string = leagueGame[22]?.ToString();
                    decimal? stl = !string.IsNullOrEmpty(stl_string) ? JsonConvert.DeserializeObject<decimal>(stl_string) : (decimal?)null;
                    string blk_string = leagueGame[23]?.ToString();
                    decimal? blk = !string.IsNullOrEmpty(blk_string) ? JsonConvert.DeserializeObject<decimal>(blk_string) : (decimal?)null;
                    string tov_string = leagueGame[24]?.ToString();
                    decimal? tov = !string.IsNullOrEmpty(tov_string) ? JsonConvert.DeserializeObject<decimal>(tov_string) : (decimal?)null;
                    string pf_string = leagueGame[25]?.ToString();
                    decimal? pf = !string.IsNullOrEmpty(pf_string) ? JsonConvert.DeserializeObject<decimal>(pf_string) : (decimal?)null;
                    string pts_string = leagueGame[26]?.ToString();
                    decimal? pts = !string.IsNullOrEmpty(pts_string) ? JsonConvert.DeserializeObject<decimal>(pts_string) : (decimal?)null;
                    string plus_minus_string = leagueGame[27]?.ToString();
                    decimal? plus_minus = !string.IsNullOrEmpty(plus_minus_string) ? JsonConvert.DeserializeObject<decimal>(plus_minus_string) : (decimal?)null;
                    //string video_available_string = leagueGame[28]?.ToString();
                    //decimal? video_available = !string.IsNullOrEmpty(video_available_string) ? JsonConvert.DeserializeObject<decimal>(video_available_string) : (decimal?)null;
                    string? videoAvailableString = leagueGame[28]?.ToString();
                    // Create an NpgsqlParameter for team_name and set the value without quotes
                    NpgsqlParameter videoAvailableParam = new NpgsqlParameter("@video_available", NpgsqlDbType.Text);
                    videoAvailableParam.Value = videoAvailableString;

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.Add(seasonIdParam);
                        cmd.Parameters.Add(teamIdParam);
                        cmd.Parameters.Add(teamAbbreviationParam);
                        cmd.Parameters.Add(teamNameParam);
                        cmd.Parameters.Add(gameIdParam);
                        cmd.Parameters.Add(gameDateParam);
                        cmd.Parameters.Add(matchupParam);
                        cmd.Parameters.Add(wlParam);
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
                        cmd.Parameters.Add(videoAvailableParam);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, leagueGame);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }

}
