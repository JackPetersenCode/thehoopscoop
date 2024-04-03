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
    public class ShotController : ControllerBase
    {
        private readonly ShotDataHandler _shotDataHandler;

        public ShotController(ShotDataHandler shotDataHandler)
        {
            _shotDataHandler = shotDataHandler;
        }

        [HttpGet("{playerId}/{season}")]
        public async Task<ActionResult<IEnumerable<Shot>>> GetShotsBySeason(string playerId, string season)
        {
            //System.Diagnostics.Debug.WriteLine("ahahahah");

            return await _shotDataHandler.GetShotsBySeason(playerId, season);
        }

        [HttpGet("{playerId}/{season}/{gameId}")]
        public async Task<ActionResult<IEnumerable<Shot>>> GetShotsByGame(string playerId, string season, string gameId)
        {
            //System.Diagnostics.Debug.WriteLine("ahahahah");

            return await _shotDataHandler.GetShotsByGame(playerId, season, gameId);
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetShotsFromFile(string season)
        {
            return await _shotDataHandler.GetShotsFromFile(season);
        }

        [HttpPost("{season}")]
        public async Task<IActionResult> CreateShot([FromBody] Shot shot, string season)
        {
            return await _shotDataHandler.CreateShot(shot, season);
        }
    }
}

