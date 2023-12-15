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

namespace ReactApp4.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueGamesController : ControllerBase
    {
        private readonly LeagueGameDataHandler _leagueGameDataHandler;

        public LeagueGamesController(LeagueGameDataHandler leagueGameDataHandler)
        {
            _leagueGameDataHandler = leagueGameDataHandler;
        }

        [HttpGet("{season}")]
        public async Task<ActionResult<IEnumerable<LeagueGame>>> GetGamesBySeason(string season)
        {
            return await _leagueGameDataHandler.GetGamesBySeason(season);
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetGamesFromFile(string season)
        {
            return await _leagueGameDataHandler.GetGamesFromFile(season);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeagueGame([FromBody] object[] leagueGame)
        {
            return await _leagueGameDataHandler.CreateLeagueGame(leagueGame);
        }
    }
}

