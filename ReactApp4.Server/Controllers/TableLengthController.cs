using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactApp4.Server.Data;
using ReactApp4.Server.Helpers;



namespace ReactApp4.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableLengthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TableLengthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{table}")]
        public IActionResult GetTableLength(string table)
        {
            try
            {
                var query = $"SELECT COUNT(*) FROM \"{table}\"";

                int count;
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    _context.Database.OpenConnection();
                    count = Convert.ToInt32(command.ExecuteScalar());
                }

                return Ok(new { Table = table, Count = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("box/{table}")]
        public IActionResult GetTableLengthBox(string table)
        {
            try
            {
                var query = $"SELECT COUNT(DISTINCT(game_id)) FROM \"{table}\"";

                int count;
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    _context.Database.OpenConnection();
                    count = Convert.ToInt32(command.ExecuteScalar());
                }

                return Ok(new { Table = table, Count = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
