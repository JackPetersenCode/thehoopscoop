﻿using Microsoft.AspNetCore.Http;
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
    public class BoxScoreScoringController : ControllerBase
    {
        private readonly BoxScoreScoringDataHandler _boxScoreScoringDataHandler;

        public BoxScoreScoringController(BoxScoreScoringDataHandler boxScoreScoringDataHandler)
        {
            _boxScoreScoringDataHandler = boxScoreScoringDataHandler;
        }

        [HttpGet("{season}")]
        public async Task<ActionResult<IEnumerable<BoxScoreScoring>>> GetBoxScoreScoringBySeason(string season)
        {
            //System.Diagnostics.Debug.WriteLine("ahahahah");
            if (!SeasonConstants.IsValidNBASeason(season))
            	return BadRequest("Invalid NBA season.");
            return await _boxScoreScoringDataHandler.GetBoxScoreScoringBySeason(season);
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetBoxScoreScoringFromFile(string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
            	return BadRequest("Invalid NBA season.");
            return await _boxScoreScoringDataHandler.GetBoxScoreScoringFromFile(season);
        }

        [Authorize]
        [HttpPost("{season}")]
        public async Task<IActionResult> CreateBoxScoreScoring([FromBody] BoxScoreScoring boxScoreScoring, string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
            {
            	return BadRequest("Invalid NBA season.");
            }
            if (boxScoreScoring == null)
            {
                return BadRequest("Invalid boxScoreAdvanced data");
            }

            return await _boxScoreScoringDataHandler.CreateBoxScoreScoring(boxScoreScoring, season);
        }
    }
}


