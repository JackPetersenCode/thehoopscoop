using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Net.WebRequestMethods;
using System.Runtime.Intrinsics.X86;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using ReactApp4.Server.Services;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using ReactApp4.Server.Helpers;

namespace ReactApp4.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoxScoreTraditionalController : ControllerBase
    {
        private readonly BoxScoreTraditionalDataHandler _boxScoreTraditionalDataHandler;

        public BoxScoreTraditionalController(BoxScoreTraditionalDataHandler boxScoreTraditionalDataHandler)
        {
            _boxScoreTraditionalDataHandler = boxScoreTraditionalDataHandler;
        }

        [HttpGet("{season}")]
        public async Task<ActionResult<IEnumerable<BoxScoreTraditional>>> GetBoxScoreTraditionalBySeason(string season)
        {
            //System.Diagnostics.Debug.WriteLine("ahahahah");
            if (!SeasonConstants.IsValidNBASeason(season))
            	return BadRequest("Invalid NBA season.");
            return await _boxScoreTraditionalDataHandler.GetBoxScoreTraditionalBySeason(season);
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetBoxScoreTraditionalFromFile(string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
            	return BadRequest("Invalid NBA season.");
            return await _boxScoreTraditionalDataHandler.GetBoxScoreTraditionalFromFile(season);
        }

        [Authorize]
        [HttpPost("{season}")]
        public async Task<IActionResult> CreateBoxScoreTraditional([FromBody] BoxScoreTraditional boxScoreTraditional, string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
            	return BadRequest("Invalid NBA season.");
            if (boxScoreTraditional == null)
                return BadRequest("Invalid boxScoreAdvanced data");

            return await _boxScoreTraditionalDataHandler.CreateBoxScoreTraditional(boxScoreTraditional, season);
        }

        [HttpGet("roster/{season}/{teamId}")]
        public async Task<ActionResult<IEnumerable<SelectedPlayer>>> GetRosterBySeasonByTeam(string season, string teamId)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
            	return BadRequest("Invalid NBA season.");
            return await _boxScoreTraditionalDataHandler.GetRosterBySeasonByTeam(season, teamId);
        }

        [HttpGet("82GameAverages/{playerId}/{season}/{H_or_V}/{gameDate?}")]
        public async Task<IActionResult> Get82GameAverages(string playerId, string season, string H_or_V, string gameDate = null)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
            	return BadRequest("Invalid NBA season.");
            return await _boxScoreTraditionalDataHandler.Get82GameAverages(playerId, season, H_or_V, gameDate);
        }
    }
}

