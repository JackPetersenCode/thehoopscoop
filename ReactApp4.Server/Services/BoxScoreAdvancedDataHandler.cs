using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class BoxScoreAdvancedDataHandler
    {
        private readonly BoxScoreAdvancedDatabaseHandler _boxScoreAdvancedDatabaseHandler;
        private readonly BoxScoreAdvancedFileHandler _boxScoreAdvancedFileHandler;

        public BoxScoreAdvancedDataHandler(BoxScoreAdvancedDatabaseHandler boxScoreAdvancedDatabaseHandler, BoxScoreAdvancedFileHandler boxScoreAdvancedFileHandler)
        {
            _boxScoreAdvancedDatabaseHandler = boxScoreAdvancedDatabaseHandler;
            _boxScoreAdvancedFileHandler = boxScoreAdvancedFileHandler;
        }

        public async Task<ActionResult<IEnumerable<BoxScoreAdvanced>>> GetBoxScoreAdvancedBySeason(string season)
        {
            return await _boxScoreAdvancedDatabaseHandler.GetBoxScoreAdvancedBySeason(season);
        }

        public async Task<IActionResult> GetBoxScoreAdvancedFromFile(string season)
        {
            return await _boxScoreAdvancedFileHandler.GetBoxScoreAdvancedFromFile(season);
        }

        public async Task<IActionResult> CreateBoxScoreAdvanced([FromBody] BoxScoreAdvanced boxScoreAdvanced, string season)
        {
            return await _boxScoreAdvancedDatabaseHandler.CreateBoxScoreAdvanced(boxScoreAdvanced, season);
        }
    }
}
