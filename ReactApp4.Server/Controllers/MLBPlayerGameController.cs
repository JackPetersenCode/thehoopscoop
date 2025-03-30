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
            return await _mLBPlayerGameDataHandler.GetMLBPlayerGamesBattingBySeason(season);
        }

        [HttpGet("pitching/{season}")]
        //get all stats (batting, pitching, fielding)
        public async Task<ActionResult<IEnumerable<MLBPlayerGamePitching>>> GetMLBPlayerGamesPitchingBySeason(string season)
        {
            return await _mLBPlayerGameDataHandler.GetMLBPlayerGamesPitchingBySeason(season);
        }
    
        [HttpGet("fielding/{season}")]
        //get all stats (batting, pitching, fielding)
        public async Task<ActionResult<IEnumerable<MLBPlayerGameFielding>>> GetMLBPlayerGamesFieldingBySeason(string season)
        {
            return await _mLBPlayerGameDataHandler.GetMLBPlayerGamesFieldingBySeason(season);
        }

        [HttpGet("read/{season}/{category}")]
        public async Task<IActionResult> GetMLBPlayerGamesFromFile(string season, string category)
        {
            return await _mLBPlayerGameDataHandler.GetMLBPlayerGamesFromFile(season, category);
        }

        [HttpGet("readPlayerGameInfo/{season}")]
        public async Task<IActionResult> GetMLBPlayerGameInfoFromFile(string season)
        {
            return await _mLBPlayerGameDataHandler.GetMLBPlayerGameInfoFromFile(season);
        }

        [HttpPost("batting/{season}")]
        public async Task<IActionResult> CreateMLBPlayerGamesBatting([FromBody] List<MLBPlayerGameBatting> mLBPlayerGameBatting, string season)
        {
            return await _mLBPlayerGameDataHandler.CreateMLBPlayerGamesBatting(mLBPlayerGameBatting, season);
        }

        [HttpPost("pitching/{season}")]
        public async Task<IActionResult> CreateMLBPlayerGamesPitching([FromBody] List<MLBPlayerGamePitching> mLBPlayerGamePitching, string season)
        {
            return await _mLBPlayerGameDataHandler.CreateMLBPlayerGamesPitching(mLBPlayerGamePitching, season);
        }

        [HttpPost("fielding/{season}")]
        public async Task<IActionResult> CreateMLBPlayerGamesFielding([FromBody] List<MLBPlayerGameFielding> mLBPlayerGameFielding, string season)
        {
            return await _mLBPlayerGameDataHandler.CreateMLBPlayerGamesFielding(mLBPlayerGameFielding, season);
        }

        [HttpPost("playerGameInfo/{season}")]
        public async Task<IActionResult> CreateMLBPlayerGameInfo([FromBody] List<MLBPlayerGameInfo> mLBPlayerGameInfo, string season)
        {
            return await _mLBPlayerGameDataHandler.CreateMLBPlayerGameInfo(mLBPlayerGameInfo, season);
        }
    }
}