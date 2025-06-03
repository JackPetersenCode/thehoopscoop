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
    public class MLBTeamController : ControllerBase
    {
        private readonly MLBTeamDataHandler _mLBTeamDataHandler;

        public MLBTeamController(MLBTeamDataHandler mLBTeamDataHandler)
        {
            _mLBTeamDataHandler = mLBTeamDataHandler;
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetMLBTeamInfoFromFile(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBTeamDataHandler.GetMLBTeamInfoFromFile(season);
        }

        [HttpGet("teamInfo/{season}")]
        public async Task<ActionResult<IEnumerable<MLBTeamInfo>>> GetMLBTeamInfoBySeason(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _mLBTeamDataHandler.GetMLBTeamInfoBySeason(season);
        }

        [Authorize]
        [HttpPost("teamInfo/{season}")]
        public async Task<IActionResult> CreateMLBTeamInfoBySeason([FromBody] List<MLBTeamInfo> mLBTeamInfo, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (mLBTeamInfo == null)
                return BadRequest("Invalid mlb team info data");
            return await _mLBTeamDataHandler.CreateMLBTeamInfoBySeason(mLBTeamInfo, season);
        }
    }
}