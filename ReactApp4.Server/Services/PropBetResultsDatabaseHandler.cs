using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReactApp4.Server.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NpgsqlTypes;
using static System.Net.WebRequestMethods;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Numerics;
using System.Reflection.Emit;
using ReactApp4.Server.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ReactApp4.Server.Services
{
    public class PropBetResultsDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<IActionResult> GetPropBetResults(string selectedSeason, decimal overUnderLine, string selectedOpponent, [FromQuery] List<Player> roster, [FromQuery] List<PropBetStats> propBetStats)
        {
            try
            {
                var query = $@"SELECT * FROM box_score_traditional_2023_24";
                var boxScores = await _context.BoxScoreTraditionals.FromSqlRaw(query).ToListAsync();
                Console.WriteLine(boxScores.Count);
                return Ok(boxScores);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, log, and return an error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
