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
    public class SportRadarMLBEGSController : ControllerBase
    {
        private readonly SportRadarMLBEGSDataHandler _sportRadarMLBEGSDataHandler;

        public SportRadarMLBEGSController(SportRadarMLBEGSDataHandler sportRadarMLBEGSDataHandler)
        {
            _sportRadarMLBEGSDataHandler = sportRadarMLBEGSDataHandler;
        }

        [HttpGet("read/{file}")]
        public async Task<IActionResult> GetSportRadarMLBEGSFromFile(string file)
        {
            //if (!SeasonConstants.IsValidMLBSeason(season))
            //    return BadRequest("Invalid MLB season.");
            return await _sportRadarMLBEGSDataHandler.GetSportRadarMLBEGSFromFile(file);
        }

        [Authorize]
        [HttpPost("gameInfo/{season}")]
        public async Task<IActionResult> CreateSportRadarMLBEGSGameInfo([FromBody] List<SportRadarMLBEGSGameInfo> sportRadarMLBEGSGameInfo, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
                return BadRequest("Invalid MLB season.");
            if (sportRadarMLBEGSGameInfo == null)
                return BadRequest("Invalid game info data");
            return await _sportRadarMLBEGSDataHandler.CreateSportRadarMLBEGSGameInfo(sportRadarMLBEGSGameInfo, season);
        }

        [Authorize]
        [HttpPost("leagueSchedule/{season}")]
        public async Task<IActionResult> CreateSportRadarMLBLeagueSchedule([FromBody] List<SportRadarMLBLeagueSchedule> sportRadarMLBLeagueSchedule, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
                return BadRequest("Invalid MLB season.");
            if (sportRadarMLBLeagueSchedule == null)
                return BadRequest("Invalid game info data");
            return await _sportRadarMLBEGSDataHandler.CreateSportRadarMLBLeagueSchedule(sportRadarMLBLeagueSchedule, season);
        }

        [Authorize]
        [HttpPost("pbpAtBats/{season}")]
        public async Task<IActionResult> CreateSportRadarMLBPBPAtBats([FromBody] List<SportRadarMLBPBPAtBat> sportRadarMLBPBPAtBats, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
                return BadRequest("Invalid MLB season.");
            if (sportRadarMLBPBPAtBats == null)
                return BadRequest("Invalid game info data");
            return await _sportRadarMLBEGSDataHandler.CreateSportRadarMLBPBPAtBats(sportRadarMLBPBPAtBats, season);
        }

        [Authorize]
        [HttpPost("pbpPitchEvents/{season}")]
        public async Task<IActionResult> CreateSportRadarMLBPBPPitchEvents([FromBody] List<SportRadarMLBPBPPitchEvent> sportRadarMLBPBPPitchEvents, string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            if (sportRadarMLBPBPPitchEvents == null)
                return BadRequest("Invalid game info data");
            return await _sportRadarMLBEGSDataHandler.CreateSportRadarMLBPBPPitchEvents(sportRadarMLBPBPPitchEvents, season);
        }
    }
}