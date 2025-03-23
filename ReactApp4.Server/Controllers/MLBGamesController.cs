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
    public class MLBGameController : ControllerBase
    {
        private readonly MLBGameDataHandler _mLBGameDataHandler;

        public MLBGameController(MLBGameDataHandler mLBGameDataHandler)
        {
            _mLBGameDataHandler = mLBGameDataHandler;
        }

        [HttpGet("{season}")]
        public async Task<ActionResult<IEnumerable<MLBGame>>> GetMLBGamesBySeason(string season)
        {
            return await _mLBGameDataHandler.GetMLBGamesBySeason(season);
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetMLBGamesFromFile(string season)
        {
            return await _mLBGameDataHandler.GetMLBGamesFromFile(season);
        }

        [HttpPost("{season}")]
        public async Task<IActionResult> CreateMLBGames([FromBody] MLBGame mLBGame, string season)
        {
            return await _mLBGameDataHandler.CreateMLBGames(mLBGame, season);
        }
    }
}