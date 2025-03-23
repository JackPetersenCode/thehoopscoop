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

        [HttpGet("shotChartsGames/{playerId}/{season}")]
        public async Task<ActionResult<IEnumerable<ShotChartsGame>>> GetShotChartsGames(string playerId, string season)
        {
            return await _leagueGameDataHandler.GetShotChartsGames(playerId, season);
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetGamesFromFile(string season)
        {
            return await _leagueGameDataHandler.GetGamesFromFile(season);
        }

        [HttpGet("BackToBack/{previousDate}/{season}")]
        public async Task<IActionResult> GetBackToBacks(
            [FromQuery] string game_id, 
            [FromQuery] string home_team_id, 
            [FromQuery] string visitor_team_id, 
            [FromQuery] string game_date, 
            string previousDate, 
            string season)
        {


            var game = new LeagueGameWithHomeVisitor
            {
                GameId = game_id,
                HomeTeamId = home_team_id,
                VisitorTeamId = visitor_team_id,
                GameDate = game_date
            };

            return await _leagueGameDataHandler.GetBackToBacks(game, previousDate, season);
        }


        public class B2BRequest
        {
            public string Season { get; set; }
            public string[] TeamIds { get; set; }
        }

        
        [HttpGet("B2B_Averages/{season}")]
        public async Task<IActionResult> B2BAverages([FromRoute] string season, [FromQuery] string[] teamIds)
        {
            if (string.IsNullOrEmpty(teamIds[0]))
            {
                return BadRequest("Team name is required.");
            }
            return await _leagueGameDataHandler.B2BAverages(teamIds, season);
        }

        [HttpGet("TeamPtsAverage/{season}")]
        public async Task<IActionResult> TeamPtsAverage([FromRoute] string season, [FromQuery] string[] teamIds)
        {
            if (string.IsNullOrEmpty(teamIds[0]))
            {
                return BadRequest("Team name is required.");
            }
            return await _leagueGameDataHandler.TeamPtsAverage(teamIds, season);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateLeagueGame([FromBody] object[] leagueGame)
        {
            return await _leagueGameDataHandler.CreateLeagueGame(leagueGame);
        }
    }


}

