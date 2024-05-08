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
    public class BoxScoreTraditionalDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public BoxScoreTraditionalDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
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

        public async Task<IActionResult> Get82GameAverages(string playerId, string season, string H_or_V, string gameDate)
        {
            Console.WriteLine(gameDate);
            try
            {

                var sql = $@"
                    SELECT player_id, player_name AS NAME, team_id, team_abbreviation AS TEAM,
                    AVG(COALESCE(min, 0.0)) AS MIN, 
                    AVG(COALESCE(fgm, 0.0)) AS FGM,
                    AVG(COALESCE(fga, 0.0)) AS FGA,
                    sum(fgm) / NULLIF(sum(fga), 0) AS FG_PCT,
                    AVG(COALESCE(fg3m, 0.0)) AS FG3M,
                    AVG(COALESCE(fg3a, 0.0)) AS FG3A,
                    sum(fg3m) / NULLIF(sum(fg3a), 0) AS FG3_PCT,
                    AVG(COALESCE(ftm, 0.0)) AS FTM,
                    AVG(COALESCE(fta, 0.0)) AS FTA,
                    sum(ftm) / NULLIF(sum(fta), 0) AS FT_PCT,
                    AVG(COALESCE(oreb, 0.0)) AS OREB,
                    AVG(COALESCE(dreb, 0.0)) AS DREB, 
                    AVG(COALESCE(reb, 0.0)) AS REB, 
                    AVG(COALESCE(ast, 0.0)) AS AST, 
                    AVG(COALESCE(stl, 0.0)) AS STL, 
                    AVG(COALESCE(blk, 0.0)) AS BLK, 
                    AVG(COALESCE(tov, 0.0)) AS TO, 
                    AVG(COALESCE(pf, 0.0)) AS PF, 
                    AVG(COALESCE(pts, 0.0)) AS PTS, 
                    AVG(COALESCE(plus_minus, 0.0)) AS ""+/-"",
                    COUNT(DISTINCT box_score_traditional_{season}.game_id)
                    FROM box_score_traditional_{season}
                    inner join box_score_summary_{season}
                    on box_score_traditional_{season}.game_id = box_score_summary_{season}.game_id
                    WHERE player_id = '{playerId}'
                    AND box_score_traditional_{season}.team_id = box_score_summary_{season}.{H_or_V}_team_id
                    AND game_date_est != 'GAME_DATE_EST' ";

                if (gameDate is not null)
                {
                    sql += $@"AND (CAST(SUBSTRING(game_date_est, 0, 11) AS DATE) < '{gameDate}') ";
                }
                    sql += $@"GROUP BY player_id, player_name, team_id, team_abbreviation
                ";
                
                Console.WriteLine(sql);

                // Ensure your connection string is correct
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            var dataList = new List<Dictionary<string, object>>();
                            while (await reader.ReadAsync())
                            {
                                var dataDict = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    dataDict[reader.GetName(i)] = reader.GetValue(i);
                                }
                                dataList.Add(dataDict);
                            }

                            // Convert the list of dictionaries to JSON

                            // Return the JSON data
                            return Ok(dataList);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
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

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

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

