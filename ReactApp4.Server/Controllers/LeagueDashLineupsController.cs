using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using ReactApp4.Server.Services;
using System.Reflection;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace ReactApp4.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueDashLineupsController : ControllerBase
    {
        private readonly LeagueDashLineupsDataHandler _leagueDashLineupsDataHandler;

        public LeagueDashLineupsController(LeagueDashLineupsDataHandler leagueDashLineupsDataHandler)
        {
            _leagueDashLineupsDataHandler = leagueDashLineupsDataHandler;
        }

        [HttpGet("read/{season}/{boxType}/{numPlayers}")]
        public async Task<IActionResult> GetLeagueDashLineupsFromFile(string season, string boxType, string numPlayers)
        {
            return await _leagueDashLineupsDataHandler.GetLeagueDashLineupsFromFile(season, boxType, numPlayers);
        }

        [HttpGet("{season}/{boxType}/{numPlayers}")]
        public async Task<IActionResult> GetLeagueDashLineups(string season, string boxType, string numPlayers)
        {
            return await _leagueDashLineupsDataHandler.GetLeagueDashLineups(season, boxType, numPlayers);
        }

        [HttpPost("{season}/{boxType}/{numPlayers}")]
        public async Task<IActionResult> CreateLeagueDashLineup([FromBody] object[] leagueDashLineup, string season, string boxType, string numPlayers)
        {
            return await _leagueDashLineupsDataHandler.CreateLeagueDashLineup(leagueDashLineup, season, boxType, numPlayers);
        }

    }
}
