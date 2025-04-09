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
    public class MLBStatsController : ControllerBase
    {
        private readonly MLBStatsDataHandler _mLBStatsDataHandler;

        public MLBStatsController(MLBStatsDataHandler mLBStatsDataHandler)
        {
            _mLBStatsDataHandler = mLBStatsDataHandler;
        }

        [HttpGet("batting/{season}")]
        public async Task<ActionResult<IEnumerable<MLBStatsBatting>>> GetMLBStatsBattingBySeason(
            string season,
            [FromQuery] string? leagueOption,
            [FromQuery] string? selectedTeam,
            [FromQuery] string? yearToDateOption,
            [FromQuery] string? selectedOpponent,
            [FromQuery] int? personId)
        {
            return await _mLBStatsDataHandler.GetMLBStatsBattingBySeason(
                season, leagueOption, selectedTeam, yearToDateOption, selectedOpponent, personId);
        }

        //[HttpGet("playByPlay/read/{season}/{gamePk}")]
        //public async Task<IActionResult> GetMLBPlayerGamesFromFile(string season, string category)
        //{
        //    return await _mLBPlayerGameDataHandler.GetMLBPlayerGamesFromFile(season, category);
        //}

        //[HttpGet("pitching/{season}")]
        ////get all stats (batting, pitching, fielding)
        //public async Task<ActionResult<IEnumerable<MLBPlayerGamePitching>>> GetMLBStatsPitchingBySeason(string season)
        //{
        //    return await _mLBStatsDataHandler.GetMLBStatsPitchingBySeason(season);
        //}
    //
        //[HttpGet("fielding/{season}")]
        ////get all stats (batting, pitching, fielding)
        //public async Task<ActionResult<IEnumerable<MLBPlayerGameFielding>>> GetMLBStatsFieldingBySeason(string season)
        //{
        //    return await _mLBStatsDataHandler.GetMLBStatsFieldingBySeason(season);
        //}


    }
}