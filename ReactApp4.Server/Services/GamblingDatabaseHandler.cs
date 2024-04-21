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
                    order by commence_time desc limit 8    
                ";

                // Ensure your connection string is correct
                //var connectionString = "Server=localhost;Port=5432;Database=hoop_scoop;User Id=postgres;Password=redsox45;";
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


        public async Task<IActionResult> GetPreviousGameId(string season, string teamId, string gameDate)
        {
            DateTime date = DateTime.ParseExact(gameDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var realDate = gameDate.Substring(0,10);
            try
            {

                var sql = $@"
                    SELECT game_id, game_date_est 
                    FROM box_score_summary_{season}
                    WHERE game_date_est != 'GAME_DATE_EST'
                    AND CAST(SUBSTRING(game_date_est, 0, 11) AS DATE) < '{realDate}'
                    AND (home_team_id = '{teamId}' OR visitor_team_id = '{teamId}')
                    ORDER BY game_date_est DESC LIMIT 1   
                ";
                Console.WriteLine(sql);

                // Ensure your connection string is correct
                //var connectionString = "Server=localhost;Port=5432;Database=hoop_scoop;User Id=postgres;Password=redsox45;";
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

                var connectionString = "Server=localhost;Port=5432;Database=hoop_scoop;User Id=postgres;Password=redsox45;\r\n"; // Replace with your actual connection string

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
    }
}
