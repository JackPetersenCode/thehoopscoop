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
    public class PlayerResultsController : ControllerBase
    {
        private readonly PlayerResultsDatabaseHandler _playerResultsDatabaseHandler;

        public PlayerResultsController(PlayerResultsDatabaseHandler playerResultsDatabaseHandler)
        {
            _playerResultsDatabaseHandler = playerResultsDatabaseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayerResults(string selectedSeason, decimal overUnderLine, string selectedOpponent, string player_id, string propBetStats)
        {
            return await _playerResultsDatabaseHandler.GetPlayerResults(selectedSeason, overUnderLine, selectedOpponent, player_id, propBetStats);
        }

    }
}
