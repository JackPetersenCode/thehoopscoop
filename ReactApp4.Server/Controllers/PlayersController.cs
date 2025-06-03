using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using ReactApp4.Server.Data;
using ReactApp4.Server.Helpers;
using ReactApp4.Server.Services;

namespace ReactApp4.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerDataHandler _playerDataHandler;

        public PlayersController(PlayerDataHandler playerDataHandler)
        {
            _playerDataHandler = playerDataHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers()
        {
            return await _playerDataHandler.GetAllPlayers();
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Player>>> GetAllActivePlayers()
        {
            return await _playerDataHandler.GetAllActivePlayers();
        }

        [HttpGet("historical")]
        public async Task<ActionResult<IEnumerable<Player>>> GetAllHistoricalPlayers()
        {
            return await _playerDataHandler.GetAllHistoricalPlayers();
        }

        [HttpGet("read")]
        public async Task<IActionResult> GetPlayersFromFile()
        {
            return await _playerDataHandler.GetPlayersFromFile();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromBody] Player player)
        {
            if (player == null)
                return BadRequest("Invalid active player data");
            return await _playerDataHandler.CreatePlayer(player);
        }
    }
}