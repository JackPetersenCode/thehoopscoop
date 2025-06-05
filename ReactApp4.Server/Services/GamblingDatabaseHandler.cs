using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using ReactApp4.Server.Data;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace ReactApp4.Server.Services
{
    public class GamblingDatabaseHandler : ControllerBase
    {
      
        private readonly IConfiguration _configuration;

        public GamblingDatabaseHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> GetUpcomingGames(string season)
        {
            try
            {

                var sql = $@"
                    SELECT
                    DISTINCT ON(commence_time, home_team) 
                    id,
                    commence_time,
                    home_team,
                    away_team, 
                    home_odds, 
                    away_odds, 
                    game_id
                    from new_odds_{season}
                    WHERE commence_time != 'commence_time'
                    order by commence_time desc limit 25   
                ";

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

        public async Task<IActionResult> GetTopTenHistorical(string season)
        {
            try
            {

                var sql = $@"
                    SELECT game_date, home_team, visitor_team  
                    FROM matchup_results_{season}
                    ORDER BY id DESC limit 10    
                ";

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

        public async Task<IActionResult> GetTeamNameFromId(string teamId)
        {
            try
            {
                var sql = @"
                    SELECT DISTINCT team_name 
                    FROM league_games_2022_23
                    WHERE team_id = @teamId";

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        // Safely add the parameter
                        cmd.Parameters.AddWithValue("@teamId", teamId);

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


        public async Task<IActionResult> GetMoneyline(string season, string teamName, string gameDate)
        {
            try
            {
                var sql = $@"
                    SELECT ml 
                    FROM odds_{season}
                    WHERE team = @teamName AND date = @gameDate";
        
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");
        
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
        
                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        // Safely parameterize input values
                        cmd.Parameters.AddWithValue("@teamName", teamName);
                        cmd.Parameters.AddWithValue("@gameDate", DateTime.Parse(gameDate)); // optionally parse to DateTime
        
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


        public async Task<IActionResult> GetNewOdds(string season, string teamName, string gameDate, string H_or_V)
        {
            try
            {

                var sql = $@"
                    SELECT {H_or_V}_odds FROM new_odds_{season}
                    WHERE {H_or_V}_team = @teamName
                    AND SUBSTRING(commence_time, 1, 10) = @gameDate
                ";

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        // Safely parameterize input values
                        cmd.Parameters.AddWithValue("@teamName", teamName);
                        cmd.Parameters.AddWithValue("@gameDate", gameDate); // optionally parse to DateTime
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

        public async Task<IActionResult> GetPreviousGameId(string season, string teamId, string gameDate)
        {
            var realDate = DateTime.Parse(gameDate.Substring(0,10));
            try
            {

                var sql = $@"
                    SELECT game_id, game_date_est 
                    FROM box_score_summary_{season}
                    WHERE game_date_est != 'GAME_DATE_EST'
                    AND CAST(SUBSTRING(game_date_est, 0, 11) AS DATE) < @gameDate
                    AND (home_team_id = @teamId OR visitor_team_id = @teamId)
                    ORDER BY game_date_est DESC LIMIT 1   
                ";

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        // Safely parameterize input values
                        cmd.Parameters.AddWithValue("@teamId", teamId);
                        cmd.Parameters.AddWithValue("@gameDate", realDate); // optionally parse to DateTime
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

        public async Task<IActionResult> GetRosterBySeasonByTeam(string season, string teamId, string gameId)
        {
            try
            {

                var sql = $@"
                    SELECT DISTINCT player_id, player_name FROM box_score_traditional_{season} WHERE team_id = @teamId
                ";
                if (!string.IsNullOrEmpty(gameId))
                {
                    sql += $@"AND game_id = @gameId";
                }

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@teamId", teamId);
                        if (!string.IsNullOrEmpty(gameId))
                        {
                            cmd.Parameters.AddWithValue("@gameId", gameId); // optionally parse to DateTime
                        }
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

        public async Task<IActionResult> GetRosterBySeasonByTeamFromAdvanced(string season, string teamId, string gameId)
        {
            try
            {
                var sql = $@"
                    SELECT DISTINCT player_id, player_name FROM box_score_advanced_{season} WHERE team_id = @teamId
                ";
                if (!string.IsNullOrEmpty(gameId))
                {
                    sql += $@"AND game_id = @gameId";
                }

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@teamId", teamId);
                        if (!string.IsNullOrEmpty(gameId))
                        {
                            cmd.Parameters.AddWithValue("@gameId", gameId); // optionally parse to DateTime
                        }
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

        public async Task<IActionResult> GetWinPctBySeason(string season)
        {
            try
            {

                var sql = $@"
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_{season}
                    GROUP BY green_red
                ";

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

        public async Task<IActionResult> GetWinPctBySeasonByTeam(string season, string team)
        {
            try
            {

                var sql = $@"
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_{season}
                    WHERE home_team = @team
                    OR visitor_team = @team
                    GROUP BY green_red
                ";

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@team", team);
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

        public async Task<IActionResult> GetWinPctOverall()
        {
            try
            {

                var sql = $@"
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_2016_17
                    GROUP BY green_red
                    UNION ALL
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_2017_18
                    GROUP BY green_red
                    UNION ALL
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_2018_19
                    GROUP BY green_red
                    UNION ALL
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_2019_20
                    GROUP BY green_red
                    UNION ALL
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_2020_21
                    GROUP BY green_red
                    UNION ALL
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_2021_22
                    GROUP BY green_red
                    UNION ALL
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_2022_23
                    GROUP BY green_red
                    UNION ALL
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_2023_24
                    GROUP BY green_red
                    UNION ALL
                    SELECT green_red, COUNT(*)
                    FROM matchup_results_2024_25
                    GROUP BY green_red
                ";

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

        public async Task<IActionResult> GetAverageScore(string season, string gameDate)
        {
            //var realDate = gameDate.Substring(0, 10);
            //Console.WriteLine(realDate);
            try
            {

                var sql = $@"
                    SELECT AVG(CAST(pts AS FLOAT))
                    FROM league_games_{season}
                    INNER JOIN box_score_summary_{season}
                    ON box_score_summary_{season}.game_id = league_games_{season}.game_id
                    WHERE game_date_est != 'GAME_DATE_EST'
                ";
                if (!string.IsNullOrEmpty(gameDate))
                {
                    sql += $@"AND (CAST(SUBSTRING(game_date_est, 0, 11) AS DATE) < @gameDate) ";               

                }
                
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(gameDate))
                        {
                            cmd.Parameters.AddWithValue("@gameDate", DateTime.Parse(gameDate));
                        }
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

        public async Task<IActionResult> GetHistoricalResults(string season, string teamName)
        {
            //var realDate = gameDate.Substring(0, 10);
            //Console.WriteLine(realDate);
            try
            {

                var sql = $@"
                    SELECT * FROM matchup_results_{season}
                ";
                if (!string.IsNullOrEmpty(teamName))
                {
                    sql += $@"WHERE home_team = @teamName OR visitor_team = @teamName ";
                }

                sql += $@"ORDER BY id DESC";
                
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(teamName))
                        {
                            cmd.Parameters.AddWithValue("@teamName", teamName);
                        }
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
        public async Task<IActionResult> GetLeagueGamesWithHomeVisitor(string season)
        {
            try
            {

                var sql = $@"
                    SELECT league_games_{season}.*,
                    box_score_summary_{season}.home_team_id, 
                    box_score_summary_{season}.visitor_team_id
                    FROM league_games_{season}
                    INNER JOIN box_score_summary_{season}
                    ON box_score_summary_{season}.game_id = league_games_{season}.game_id
                    WHERE matchup LIKE '%vs.%'
                ";
                
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
        public async Task<IActionResult> PostNewOdds([FromBody] NewOdds newOdds, string season)
        {
            try
            {
                if (newOdds == null)
                {
                    return BadRequest("Invalid newOdds data");
                }

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = $"INSERT INTO new_odds_{season} (game_id, commence_time, home_team, away_team, home_odds, away_odds) VALUES (@game_id, @commence_time, @home_team, @away_team, @home_odds, @away_odds);";

                    //var fgaValue = string.IsNullOrEmpty(boxScoreTraditional.Fga) ? null : boxScoreTraditional.Fga;
                    //boxScoreTraditional.CheckAndReplace();



                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@game_id", newOdds.Game_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@commence_time", newOdds.Commence_time ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_team", newOdds.Home_team ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@away_team", newOdds.Away_team ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_odds", newOdds.Home_odds ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@away_odds", newOdds.Away_odds ?? (object)DBNull.Value);
                        


                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, newOdds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }

        public async Task<IActionResult> PostExpectedMatchup([FromBody] ExpectedMatchup expectedMatchup, string season)
        {
            try
            {
                if (expectedMatchup == null)
                {
                    return BadRequest("Invalid Expected matchup data");
                }

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = $"INSERT INTO matchup_results_{season} (game_date, matchup, home_team, home_team_id, home_expected, visitor_team, visitor_team_id, visitor_expected, home_actual, visitor_actual, home_odds, visitor_odds, green_red) VALUES (@game_date, @matchup, @home_team, @home_team_id, @home_expected, @visitor_team, @visitor_team_id, @visitor_expected, @home_actual, @visitor_actual, @home_odds, @visitor_odds, @green_red)";

                    //var fgaValue = string.IsNullOrEmpty(boxScoreTraditional.Fga) ? null : boxScoreTraditional.Fga;
                    //boxScoreTraditional.CheckAndReplace();



                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@game_date", expectedMatchup.Game_date ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@matchup", expectedMatchup.Matchup ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_team", expectedMatchup.Home_team ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_team_id", expectedMatchup.Home_team_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_expected", expectedMatchup.Home_expected ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@visitor_team", expectedMatchup.Visitor_team ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@visitor_team_id", expectedMatchup.Visitor_team_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@visitor_expected", expectedMatchup.Visitor_expected ?? (object)DBNull.Value); 
                        cmd.Parameters.AddWithValue("@home_actual", expectedMatchup.Home_actual ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@visitor_actual", expectedMatchup.Visitor_actual ?? (object)DBNull.Value); 
                        cmd.Parameters.AddWithValue("@home_odds", expectedMatchup.Home_odds ?? (object)DBNull.Value); 
                        cmd.Parameters.AddWithValue("@visitor_odds", expectedMatchup.Visitor_odds ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@green_red", expectedMatchup.Green_red ?? (object)DBNull.Value);                        


                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, expectedMatchup);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }
}

