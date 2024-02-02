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
    public class BoxScoreFourFactorsController : ControllerBase
    {
        private readonly BoxScoreFourFactorsDataHandler _boxScoreFourFactorsDataHandler;

        public BoxScoreFourFactorsController(BoxScoreFourFactorsDataHandler boxScoreFourFactorsDataHandler)
        {
            _boxScoreFourFactorsDataHandler = boxScoreFourFactorsDataHandler;
        }

        [HttpGet("{season}")]
        public async Task<ActionResult<IEnumerable<BoxScoreFourFactors>>> GetBoxScoreFourFactorsBySeason(string season)
        {
            //System.Diagnostics.Debug.WriteLine("ahahahah");

            return await _boxScoreFourFactorsDataHandler.GetBoxScoreFourFactorsBySeason(season);
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetBoxScoreFourFactorsFromFile(string season)
        {
            return await _boxScoreFourFactorsDataHandler.GetBoxScoreFourFactorsFromFile(season);
        }

        [HttpPost("{season}")]
        public async Task<IActionResult> CreateBoxScoreFourFactors([FromBody] BoxScoreFourFactors boxScoreFourFactors, string season)
        {
            return await _boxScoreFourFactorsDataHandler.CreateBoxScoreFourFactors(boxScoreFourFactors, season);
        }
    }
}


