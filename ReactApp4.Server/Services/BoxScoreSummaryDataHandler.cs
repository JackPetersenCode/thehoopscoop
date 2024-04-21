using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class BoxScoreSummaryDataHandler
    {
        private readonly BoxScoreSummaryDatabaseHandler _boxScoreSummaryDatabaseHandler;
        private readonly BoxScoreSummaryFileHandler _boxScoreSummaryFileHandler;

        public BoxScoreSummaryDataHandler(BoxScoreSummaryDatabaseHandler boxScoreSummaryDatabaseHandler, BoxScoreSummaryFileHandler boxScoreSummaryFileHandler)
        {
            _boxScoreSummaryDatabaseHandler = boxScoreSummaryDatabaseHandler;
            _boxScoreSummaryFileHandler = boxScoreSummaryFileHandler;
        }

        public async Task<ActionResult<IEnumerable<BoxScoreSummary>>> GetBoxScoreSummaryBySeason(string season)
        {
            return await _boxScoreSummaryDatabaseHandler.GetBoxScoreSummaryBySeason(season);
        }

        public async Task<IActionResult> GetBoxScoreSummaryFromFile(string season)
        {
            return await _boxScoreSummaryFileHandler.GetBoxScoreSummaryFromFile(season);
        }

        public async Task<IActionResult> CreateBoxScoreSummary([FromBody] BoxScoreSummary boxScoreSummary, string season)
        {
            return await _boxScoreSummaryDatabaseHandler.CreateBoxScoreSummary(boxScoreSummary, season);
        }
    }
}
