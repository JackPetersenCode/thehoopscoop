using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using ReactApp4.Server.Services;

namespace ReactApp4.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamblingController : ControllerBase
    {
        private readonly GamblingDataHandler _gamblingDataHandler;

        public GamblingController(GamblingDataHandler gamblingDataHandler)
        {
            _gamblingDataHandler = gamblingDataHandler;
        }
        [HttpGet("upcomingGames/{season}")]
        public async Task<IActionResult> GetUpcomingGames(string season)
        {
            return await _gamblingDataHandler.GetUpcomingGames(season);
        }

        [HttpGet("previousGameId/{season}/{teamId}/{gameDate}")]
        public async Task<IActionResult> GetPreviousGameId(string season, string teamId, string gameDate)
        {
            return await _gamblingDataHandler.GetPreviousGameId(season, teamId, gameDate);
        }

        [HttpGet("read/newOdds/{season}")]
        public async Task<IActionResult> GetNewOddsFromFile(string season)
        {
            return await _gamblingDataHandler.GetNewOddsFromFile(season);
        }

        [HttpPost("newOdds/{season}")]
        public async Task<IActionResult> PostNewOdds([FromBody] NewOdds newOdds, string season)
        {
            return await _gamblingDataHandler.PostNewOdds(newOdds, season);
        }

    }
}
