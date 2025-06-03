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
    public class MLBPlayerGameController : ControllerBase
    {
        private readonly MLBPlayerGameDataHandler _mLBPlayerGameDataHandler;

        public MLBPlayerGameController(MLBPlayerGameDataHandler mLBPlayerGameDataHandler)
        {
            _mLBPlayerGameDataHandler = mLBPlayerGameDataHandler;
        }

        [HttpGet("batting/{season}")]
        //get all stats (batting, pitching, fielding)
        public async Task<ActionResult<IEnumerable<MLBPlayerGameBatting>>> GetMLBPlayerGamesBattingBySeason(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBPlayerGameDataHandler.GetMLBPlayerGamesBattingBySeason(season);
        }

        [HttpGet("pitching/{season}")]
        //get all stats (batting, pitching, fielding)
        public async Task<ActionResult<IEnumerable<MLBPlayerGamePitching>>> GetMLBPlayerGamesPitchingBySeason(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBPlayerGameDataHandler.GetMLBPlayerGamesPitchingBySeason(season);
        }
    
        [HttpGet("fielding/{season}")]
        //get all stats (batting, pitching, fielding)
        public async Task<ActionResult<IEnumerable<MLBPlayerGameFielding>>> GetMLBPlayerGamesFieldingBySeason(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBPlayerGameDataHandler.GetMLBPlayerGamesFieldingBySeason(season);
        }

        [HttpGet("read/{season}/{category}")]
        public async Task<IActionResult> GetMLBPlayerGamesFromFile(string season, string category)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBPlayerGameDataHandler.GetMLBPlayerGamesFromFile(season, category);
        }

        [HttpGet("readPlayerGameInfo/{season}")]
        public async Task<IActionResult> GetMLBPlayerGameInfoFromFile(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBPlayerGameDataHandler.GetMLBPlayerGameInfoFromFile(season);
        }

        [Authorize]
        [HttpPost("batting/{season}")]
        public async Task<IActionResult> CreateMLBPlayerGamesBatting([FromBody] List<MLBPlayerGameBatting> mLBPlayerGameBatting, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (mLBPlayerGameBatting == null)
                return BadRequest("Invalid player game batting data");
            return await _mLBPlayerGameDataHandler.CreateMLBPlayerGamesBatting(mLBPlayerGameBatting, season);
        }

        [Authorize]
        [HttpPost("pitching/{season}")]
        public async Task<IActionResult> CreateMLBPlayerGamesPitching([FromBody] List<MLBPlayerGamePitching> mLBPlayerGamePitching, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (mLBPlayerGamePitching == null)
                return BadRequest("Invalid player game pitching data");
            return await _mLBPlayerGameDataHandler.CreateMLBPlayerGamesPitching(mLBPlayerGamePitching, season);
        }

        [Authorize]
        [HttpPost("fielding/{season}")]
        public async Task<IActionResult> CreateMLBPlayerGamesFielding([FromBody] List<MLBPlayerGameFielding> mLBPlayerGameFielding, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (mLBPlayerGameFielding == null)
                return BadRequest("Invalid player game fielding data");
            return await _mLBPlayerGameDataHandler.CreateMLBPlayerGamesFielding(mLBPlayerGameFielding, season);
        }

        [Authorize]
        [HttpPost("playerGameInfo/{season}")]
        public async Task<IActionResult> CreateMLBPlayerGameInfo([FromBody] List<MLBPlayerGameInfo> mLBPlayerGameInfo, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (mLBPlayerGameInfo == null)
                return BadRequest("Invalid player game info data");
            return await _mLBPlayerGameDataHandler.CreateMLBPlayerGameInfo(mLBPlayerGameInfo, season);
        }
    }
}