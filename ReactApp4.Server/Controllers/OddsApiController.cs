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
    public class OddsApiController : ControllerBase
    {
        private readonly OddsApiDataHandler _oddsApiDataHandler;

        public OddsApiController(OddsApiDataHandler oddsApiDataHandler)
        {
            _oddsApiDataHandler = oddsApiDataHandler;
        }

        [HttpGet("{season}")]
        //get all stats (batting, pitching, fielding)
        public async Task<ActionResult<IEnumerable<OddsApiH2H>>> GetOddsApiH2HBySeason(string season)
        {
            if (!SeasonConstants.IsValidMLBSeason(season))
            	return BadRequest("Invalid MLB season.");
            return await _oddsApiDataHandler.GetOddsApiH2HBySeason(season);
        }


        [HttpGet("read/{sport}/{season}/{fileName}")]
        public async Task<IActionResult> GetOddsApiFromFile(string sport, string season, string fileName)
        {
            if (sport == "NBA")
            {
                if (!SeasonConstants.IsValidNBASeason(season))
                	return BadRequest("Invalid NBA season.");                
            }
            if (sport == "MLB") 
            {
                if (!SeasonConstants.IsValidMLBSeason(season))
            	    return BadRequest("Invalid MLB season.");
            }
            return await _oddsApiDataHandler.GetOddsApiFromFile(sport, season, fileName);
        }


        [Authorize]
        [HttpPost("H2H/{sport}/{season}")]
        public async Task<IActionResult> CreateOddsApiH2H([FromBody] List<OddsApiH2H> oddsApiH2H, string sport, string season)
        {
            if (sport == "NBA")
            {
                if (!SeasonConstants.IsValidNBASeason(season))
                	return BadRequest("Invalid NBA season.");                
            }
            if (sport == "MLB")
            {
                if (!SeasonConstants.IsValidMLBSeason(season))
                	return BadRequest("Invalid MLB season.");
            }
            if (!SeasonConstants.IsValidSport(sport))
                return BadRequest("Invalid sport.");
            if (oddsApiH2H == null)
                return BadRequest("Invalid Odds Api H2H");
            return await _oddsApiDataHandler.CreateOddsApiH2H(oddsApiH2H, sport, season);
        }
    }
}