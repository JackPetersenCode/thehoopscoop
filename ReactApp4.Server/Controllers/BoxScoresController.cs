using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using ReactApp4.Server.Services;
using System.Reflection;
using System;
using System.Linq;
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

        [HttpGet("{season?}/{boxType?}/{order?}/{sortField?}/{perMode?}/{selectedTeam?}/{selectedOpponent?}/{playerId?}")]
        public async Task<IActionResult> GetBoxScores(string season = "2024_25", string boxType = "Traditional", string order = "desc", string sortField = "id", string perMode = "Totals", string selectedTeam = "1", string selectedOpponent = "1", string playerId = "1")
        {
            return await _boxScoresDataHandler.GetBoxScores(season, boxType, order, sortField, perMode, selectedTeam, selectedOpponent, playerId);
        }

    }
}
