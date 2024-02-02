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
    public class BoxScoreMiscController : ControllerBase
    {
        private readonly BoxScoreMiscDataHandler _boxScoreMiscDataHandler;

        public BoxScoreMiscController(BoxScoreMiscDataHandler boxScoreMiscDataHandler)
        {
            _boxScoreMiscDataHandler = boxScoreMiscDataHandler;
        }

        [HttpGet("{season}")]
        public async Task<ActionResult<IEnumerable<BoxScoreMisc>>> GetBoxScoreMiscBySeason(string season)
        {
            //System.Diagnostics.Debug.WriteLine("ahahahah");

            return await _boxScoreMiscDataHandler.GetBoxScoreMiscBySeason(season);
        }

        [HttpGet("read/{season}")]
        public async Task<IActionResult> GetBoxScoreMiscFromFile(string season)
        {
            return await _boxScoreMiscDataHandler.GetBoxScoreMiscFromFile(season);
        }

        [HttpPost("{season}")]
        public async Task<IActionResult> CreateBoxScoreMisc([FromBody] BoxScoreMisc boxScoreMisc, string season)
        {
            return await _boxScoreMiscDataHandler.CreateBoxScoreMisc(boxScoreMisc, season);
        }
    }
}


