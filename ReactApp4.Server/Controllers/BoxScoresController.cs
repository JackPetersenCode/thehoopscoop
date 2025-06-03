using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using ReactApp4.Server.Services;
using System.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;
using ReactApp4.Server.Helpers;


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
            if (!SeasonConstants.IsValidNBASeason(season))
            	return BadRequest("Invalid NBA season.");
            return await _boxScoresDataHandler.GetBoxScoresFromFile(season, boxType, numPlayers);
        }

        [HttpGet("{season?}/{boxType?}/{order?}/{sortField?}/{perMode?}/{selectedTeam?}/{selectedOpponent?}/{playerId?}")]
        public async Task<IActionResult> GetBoxScores(string season = "2024_25", string boxType = "Traditional", string order = "desc", string sortField = "id", string perMode = "Totals", string selectedTeam = "1", string selectedOpponent = "1", string playerId = "1")
        {
            if (!SeasonConstants.IsValidNBASeason(season))
            	return BadRequest("Invalid NBA season.");
                
            boxType = (boxType == "Base") ? "traditional" : boxType.ToLower();
            if (!SeasonConstants.IsValidBoxType(boxType))
            {
                return BadRequest("Invalid boxType.");
            }
            // Whitelist sort field and order
            if (!SeasonConstants.IsValidSortfield(sortField.ToLower()))
            {
                return BadRequest("Invalid sortField.");
            }
            order = order.ToUpper();
            if (order != "ASC" && order != "DESC")
            {
                return BadRequest("Invalid order direction.");
            }
            return await _boxScoresDataHandler.GetBoxScores(season, boxType, order, sortField, perMode, selectedTeam, selectedOpponent, playerId);
        }

    }
}
