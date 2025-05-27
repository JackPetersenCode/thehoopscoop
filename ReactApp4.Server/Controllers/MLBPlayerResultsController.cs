using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReactApp4.Server.Services;
using ReactApp4.Server.Data;
using System.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApp4.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MLBPlayerResultsController : ControllerBase
    {
        private readonly MLBPlayerResultsDatabaseHandler _mlbPlayerResultsDatabaseHandler;

        public MLBPlayerResultsController(MLBPlayerResultsDatabaseHandler mlbPlayerResultsDatabaseHandler)
        {
            _mlbPlayerResultsDatabaseHandler = mlbPlayerResultsDatabaseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> MLBGetPlayerResults(string hittingPitching, string selectedSeason, string selectedOpponent, int player_id, string propBetStats)
        {
            return await _mlbPlayerResultsDatabaseHandler.MLBGetPlayerResults(hittingPitching, selectedSeason, selectedOpponent, player_id, propBetStats);
        }

    }
    public class MLBPropBetStats
    {
        public string? Label { get; set; }
        public string? Accessor { get; set; }
    }

    public class MLBTeam
    {
        public string? Team_id { get; set; }
        public string? Team_name { get; set; }
    }
}
