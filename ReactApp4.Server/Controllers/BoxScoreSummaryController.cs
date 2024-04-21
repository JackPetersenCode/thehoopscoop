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
    public class BoxScoreSummaryController : ControllerBase
    {
        private readonly BoxScoreSummaryDataHandler _boxScoreSummaryDataHandler;

        public BoxScoreSummaryController(BoxScoreSummaryDataHandler boxScoreSummaryDataHandler)
        {
            _boxScoreSummaryDataHandler = boxScoreSummaryDataHandler;
        }

        [HttpGet("{season}")]
        public async Task<ActionResult<IEnumerable<BoxScoreSummary>>> GetBoxScoreSummaryBySeason(string season)
        {
            //System.Diagnostics.Debug.WriteLine("ahahahah");

            return await _boxScoreSummaryDataHandler.GetBoxScoreSummaryBySeason(season);
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetBoxScoreSummaryFromFile(string season)
        {
            return await _boxScoreSummaryDataHandler.GetBoxScoreSummaryFromFile(season);
        }

        [HttpPost("{season}")]
        public async Task<IActionResult> CreateBoxScoreSummary([FromBody] BoxScoreSummary boxScoreSummary, string season)
        {
            return await _boxScoreSummaryDataHandler.CreateBoxScoreSummary(boxScoreSummary, season);
        }
    }
}


