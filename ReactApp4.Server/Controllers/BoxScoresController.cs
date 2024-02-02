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
    public class BoxScoresController : ControllerBase
    {
        private readonly BoxScoresDataHandler _boxScoresDataHandler;

        public BoxScoresController(BoxScoresDataHandler boxScoresDataHandler)
        {
            _boxScoresDataHandler = boxScoresDataHandler;
        }

        [HttpGet("read/{season}/{boxType}/{numPlayers}")]
        public async Task<IActionResult> GetBoxScoresFromFile(string season, string boxType, string numPlayers)
        {
            return await _boxScoresDataHandler.GetBoxScoresFromFile(season, boxType, numPlayers);
        }

        [HttpGet("{season?}/{boxType?}/{numPlayers?}/{order?}/{sortField?}/{page?}/{perMode?}/{selectedTeam?}")]
        public async Task<IActionResult> GetBoxScores(string season = "2023_24", string boxType = "Traditional", string numPlayers = "5", string order = "desc", string sortField = "id", int page = 1, string perMode = "Totals", string selectedTeam = "1")
        {
            return await _boxScoresDataHandler.GetBoxScores(season, boxType, numPlayers, order, sortField, page, perMode, selectedTeam);
        }

        [HttpPost("{season}/{boxType}/{numPlayers}")]
        public async Task<IActionResult> CreateLeagueDashLineup([FromBody] object[] leagueDashLineup, string season, string boxType, string numPlayers)
        {
            return await _boxScoresDataHandler.CreateBoxScore(leagueDashLineup, season, boxType, numPlayers);
        }

    }
}
