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
    public class MLBPlayByPlayController : ControllerBase
    {
        private readonly MLBPlayByPlayDataHandler _mLBPlayByPlayDataHandler;

        public MLBPlayByPlayController(MLBPlayByPlayDataHandler mLBPlayByPlayDataHandler)
        {
            _mLBPlayByPlayDataHandler = mLBPlayByPlayDataHandler;
        }
    
        //[HttpGet("fielding/{season}")]
        ////get all stats (batting, pitching, fielding)
        //public async Task<ActionResult<IEnumerable<MLBPlayerGameFielding>>> GetMLBPlayerGamesFieldingBySeason(string season)
        //{
        //    return await _mLBPlayerGameDataHandler.GetMLBPlayerGamesFieldingBySeason(season);
        //}

        [HttpGet("read/plays/{season}")]
        public async Task<IActionResult> GetMLBPlaysBySeasonFromFile(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBPlayByPlayDataHandler.GetMLBPlaysBySeasonFromFile(season);
        }

        [HttpGet("read/playEvents/{season}")]
        public async Task<IActionResult> GetMLBPlayEventsBySeasonFromFile(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBPlayByPlayDataHandler.GetMLBPlayEventsBySeasonFromFile(season);
        }

        [HttpGet("read/runners/{season}")]
        public async Task<IActionResult> GetMLBRunnersBySeasonFromFile(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBPlayByPlayDataHandler.GetMLBRunnersBySeasonFromFile(season);
        }

        [HttpGet("read/credits/{season}")]
        public async Task<IActionResult> GetMLBRunnersCreditsBySeasonFromFile(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBPlayByPlayDataHandler.GetMLBRunnersCreditsBySeasonFromFile(season);
        }
 
        [Authorize]
        [HttpPost("plays/{season}")]
        public async Task<IActionResult> InsertPlayAsync([FromBody] List<Play> plays, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (plays == null)
                return BadRequest("Invalid plays data");
            return await _mLBPlayByPlayDataHandler.InsertPlayAsync(plays, season);
        }

        [Authorize]
        [HttpPost("playEvents/{season}")]
        public async Task<IActionResult> InsertPlayEventAsync([FromBody] List<PlayPlayEvents> playEvents, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (playEvents == null)
                return BadRequest("Invalid play events data");
            return await _mLBPlayByPlayDataHandler.InsertPlayEventAsync(playEvents, season);
        }

        [Authorize]
        [HttpPost("runners/{season}")]
        public async Task<IActionResult> InsertPlayRunnersAsync([FromBody] List<PlayRunners> playRunners, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (playRunners == null)
                return BadRequest("Invalid play runners data");
            return await _mLBPlayByPlayDataHandler.InsertPlayRunnersAsync(playRunners, season);
        }

        [Authorize]
        [HttpPost("runnerCredits/{season}")]
        public async Task<IActionResult> InsertPlayRunnersCreditsAsync([FromBody] List<PlayRunnersCredits> playRunnersCredits, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (playRunnersCredits == null)
                return BadRequest("Invalid runners credits data");
            return await _mLBPlayByPlayDataHandler.InsertPlayRunnersCreditsAsync(playRunnersCredits, season);
        }

    }
}