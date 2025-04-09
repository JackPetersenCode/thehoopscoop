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
            return await _mLBPlayByPlayDataHandler.GetMLBPlaysBySeasonFromFile(season);
        }

        [HttpGet("read/playEvents/{season}")]
        public async Task<IActionResult> GetMLBPlayEventsBySeasonFromFile(string season)
        {
            return await _mLBPlayByPlayDataHandler.GetMLBPlayEventsBySeasonFromFile(season);
        }

        [HttpGet("read/runners/{season}")]
        public async Task<IActionResult> GetMLBRunnersBySeasonFromFile(string season)
        {
            return await _mLBPlayByPlayDataHandler.GetMLBRunnersBySeasonFromFile(season);
        }

        [HttpGet("read/runnersCredits/{season}")]
        public async Task<IActionResult> GetMLBRunnersCreditsBySeasonFromFile(string season)
        {
            return await _mLBPlayByPlayDataHandler.GetMLBRunnersCreditsBySeasonFromFile(season);
        }
    }
}