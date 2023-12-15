using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReactApp4.Server.Data;
using Newtonsoft.Json;

namespace ReactApp4.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class BallersController : ControllerBase
    {
        private readonly AppDbContext _context;


        public BallersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Baller>>> Get()
        {
            return await _context.Ballers.ToListAsync();
            // Retrieve all rows of your model from the database
        }

        [HttpPost]
        public async Task<IActionResult> CreateBaller([FromBody] object[] baller)
        {
            var connectionString = "Server=localhost;Port=5432;Database=hoop_scoop;User Id=postgres;Password=redsox45;\r\n"; // Replace with your actual connection string

            try
            {
                if (baller == null)
                {
                    return BadRequest("Invalid baller data");
                }

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = "INSERT INTO ballers (name, score, salary) VALUES (@name, @score, @salary);";

                    string jsonString = baller[1]?.ToString();
                    decimal? decimalValue = !string.IsNullOrEmpty(jsonString) ? JsonConvert.DeserializeObject<decimal>(jsonString) : (decimal?)null;
                    string jsonString2 = baller[2]?.ToString();
                    decimal? decimalValue2 = !string.IsNullOrEmpty(jsonString2) ? JsonConvert.DeserializeObject<decimal>(jsonString2) : (decimal?)null;

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", baller[0]);
                        cmd.Parameters.AddWithValue("@score", decimalValue ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@salary", decimalValue2 ?? (object)DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, baller);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
