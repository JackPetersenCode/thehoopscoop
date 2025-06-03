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
    public class MLBActivePlayerController : ControllerBase
    {
        private readonly MLBActivePlayerDataHandler _mLBActivePlayerDataHandler;

        public MLBActivePlayerController(MLBActivePlayerDataHandler mLBActivePlayerDataHandler)
        {
            _mLBActivePlayerDataHandler = mLBActivePlayerDataHandler;
        }
    
        [HttpGet("{season}")]
        //get all stats (batting, pitching, fielding)
        public async Task<ActionResult<IEnumerable<MLBActivePlayer>>> GetMLBActivePlayer(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBActivePlayerDataHandler.GetMLBActivePlayer(season);
        }

        //[HttpGet("bySeason/{season}")]
        ////get all stats (batting, pitching, fielding)
        //public async Task<ActionResult<IEnumerable<MLBPlayerBySeason>>> GetMLBPlayersBySeason(string season)
        //{
        //    return await _mLBActivePlayerDataHandler.GetMLBActivePlayer(season);
        //}

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetMLBActivePlayerFromFile(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBActivePlayerDataHandler.GetMLBActivePlayerFromFile(season);
        }

        [Authorize]
        [HttpPost("{season}")]
        public async Task<IActionResult> CreateMLBActivePlayer([FromBody] List<MLBActivePlayer> mLBActivePlayer, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (mLBActivePlayer == null)
                return BadRequest("Invalid active player data");

            return await _mLBActivePlayerDataHandler.CreateMLBActivePlayer(mLBActivePlayer, season);
        }
    }
}