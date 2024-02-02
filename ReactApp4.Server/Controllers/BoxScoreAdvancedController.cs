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
    public class BoxScoreAdvancedController : ControllerBase
    {
        private readonly BoxScoreAdvancedDataHandler _boxScoreAdvancedDataHandler;

        public BoxScoreAdvancedController(BoxScoreAdvancedDataHandler boxScoreAdvancedDataHandler)
        {
            _boxScoreAdvancedDataHandler = boxScoreAdvancedDataHandler;
        }

        [HttpGet("{season}")]
        public async Task<ActionResult<IEnumerable<BoxScoreAdvanced>>> GetBoxScoreAdvancedBySeason(string season)
        {
            //System.Diagnostics.Debug.WriteLine("ahahahah");

            return await _boxScoreAdvancedDataHandler.GetBoxScoreAdvancedBySeason(season);
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetBoxScoreAdvancedFromFile(string season)
        {
            return await _boxScoreAdvancedDataHandler.GetBoxScoreAdvancedFromFile(season);
        }

        [HttpPost("{season}")]
        public async Task<IActionResult> CreateBoxScoreAdvanced([FromBody] BoxScoreAdvanced boxScoreAdvanced, string season)
        {
            return await _boxScoreAdvancedDataHandler.CreateBoxScoreAdvanced(boxScoreAdvanced, season);
        }
    }
}


