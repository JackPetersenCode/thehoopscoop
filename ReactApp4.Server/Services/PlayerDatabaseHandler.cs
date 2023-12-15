using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class PlayerDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers()
        {
            var tableName = $"players";
            Console.WriteLine(tableName);

            var query = $"SELECT * FROM {tableName}";

            var players = await _context.Players.FromSqlRaw(query).ToListAsync();

            Console.WriteLine(players);

            return players;
        }

        public async Task<IActionResult> CreatePlayer([FromBody] Player player)
        {
            // Implement logic to create a new league game in the database
            try
            {
                if (player == null)
                {
                    return BadRequest("Invalid player data");
                }

                var connectionString = "Server=localhost;Port=5432;Database=hoop_scoop;User Id=postgres;Password=redsox45;\r\n"; // Replace with your actual connection string

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = $"INSERT INTO players (full_name, first_name, last_name, is_active, player_id) VALUES (@full_name, @first_name, @last_name, @is_active, @player_id);";

                    //string fullNameString = [1]?.ToString();
                    //decimal? decimalValue = !string.IsNullOrEmpty(jsonString) ? JsonConvert.DeserializeObject<decimal>(jsonString) : (decimal?)null;
                    //string jsonString2 = baller[2]?.ToString();
                    //decimal? decimalValue2 = !string.IsNullOrEmpty(jsonString2) ? JsonConvert.DeserializeObject<decimal>(jsonString2) : (decimal?)null;

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@full_name", player.Full_name);
                        cmd.Parameters.AddWithValue("@first_name", player.First_name);
                        cmd.Parameters.AddWithValue("@last_name", player.Last_name);
                        cmd.Parameters.AddWithValue("@is_active", player.Is_active);
                        cmd.Parameters.AddWithValue("@player_id", player.Player_id);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }

}

