using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReactApp4.Server.Services;
using ReactApp4.Server.Data;
using System.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;
using ReactApp4.Server.Helpers;

namespace ReactApp4.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerResultsController : ControllerBase
    {
        private readonly PlayerResultsDatabaseHandler _playerResultsDatabaseHandler;

        public PlayerResultsController(PlayerResultsDatabaseHandler playerResultsDatabaseHandler)
        {
            _playerResultsDatabaseHandler = playerResultsDatabaseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayerResults(string selectedSeason, string selectedOpponent, string player_id, string propBetStats)
        {
            if (!SeasonConstants.IsValidNBASeason(selectedSeason))
            	return BadRequest("Invalid NBA season.");
            return await _playerResultsDatabaseHandler.GetPlayerResults(selectedSeason, selectedOpponent, player_id, propBetStats);
        }

    }
    public class PropBetStats
    {
        public string? Label { get; set; }
        public string? Accessor { get; set; }
    }

    public class NBATeam
    {
        public string? Team_id { get; set; }
        public string? Team_name { get; set; }
        public string? Team_abbreviation { get; set; }
    }
}
