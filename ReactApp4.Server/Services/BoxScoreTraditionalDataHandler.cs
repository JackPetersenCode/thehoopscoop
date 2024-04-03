using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class BoxScoreTraditionalDataHandler
    {
        private readonly BoxScoreTraditionalDatabaseHandler _boxScoreTraditionalDatabaseHandler;
        private readonly BoxScoreTraditionalFileHandler _boxScoreTraditionalFileHandler;

        public BoxScoreTraditionalDataHandler(BoxScoreTraditionalDatabaseHandler boxScoreTraditionalDatabaseHandler, BoxScoreTraditionalFileHandler boxScoreTraditionalFileHandler)
        {
            _boxScoreTraditionalDatabaseHandler = boxScoreTraditionalDatabaseHandler;
            _boxScoreTraditionalFileHandler = boxScoreTraditionalFileHandler;
        }

        public async Task<ActionResult<IEnumerable<BoxScoreTraditional>>> GetBoxScoreTraditionalBySeason(string season)
        {
            return await _boxScoreTraditionalDatabaseHandler.GetBoxScoreTraditionalBySeason(season);
        }

        public async Task<ActionResult<IEnumerable<SelectedPlayer>>> GetRosterBySeasonByTeam(string season, string teamId)
        {
            return await _boxScoreTraditionalDatabaseHandler.GetRosterBySeasonByTeam(season, teamId);
        }

        public async Task<IActionResult> GetBoxScoreTraditionalFromFile(string season)
        {
            return await _boxScoreTraditionalFileHandler.GetBoxScoreTraditionalFromFile(season);
        }

        public async Task<IActionResult> CreateBoxScoreTraditional([FromBody] BoxScoreTraditional boxScoreTraditional, string season)
        {
            return await _boxScoreTraditionalDatabaseHandler.CreateBoxScoreTraditional(boxScoreTraditional, season);
        }
    }

}
