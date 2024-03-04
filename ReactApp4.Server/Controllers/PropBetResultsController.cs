using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReactApp4.Server.Services;
using ReactApp4.Server.Data;
using System.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReactApp4.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropBetResultsController : ControllerBase
    {
        private readonly PropBetResultsDatabaseHandler _propBetResultsDatabaseHandler;

        public PropBetResultsController(PropBetResultsDatabaseHandler propBetResultsDatabaseHandler)
        {
            _propBetResultsDatabaseHandler = propBetResultsDatabaseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetPropBetResults(string selectedSeason, decimal overUnderLine, string selectedOpponent, string roster, string propBetStats)
        {
            return await _propBetResultsDatabaseHandler.GetPropBetResults(selectedSeason, overUnderLine, selectedOpponent, roster, propBetStats);
        }

    }
    public class PropBetStats
    {
        public string? Label { get; set; }
        public string? Accessor { get; set; }
    }
}
