using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class BoxScoreMiscDataHandler
    {
        private readonly BoxScoreMiscDatabaseHandler _boxScoreMiscDatabaseHandler;
        private readonly BoxScoreMiscFileHandler _boxScoreMiscFileHandler;

        public BoxScoreMiscDataHandler(BoxScoreMiscDatabaseHandler boxScoreMiscDatabaseHandler, BoxScoreMiscFileHandler boxScoreMiscFileHandler)
        {
            _boxScoreMiscDatabaseHandler = boxScoreMiscDatabaseHandler;
            _boxScoreMiscFileHandler = boxScoreMiscFileHandler;
        }

        public async Task<ActionResult<IEnumerable<BoxScoreMisc>>> GetBoxScoreMiscBySeason(string season)
        {
            return await _boxScoreMiscDatabaseHandler.GetBoxScoreMiscBySeason(season);
        }

        public async Task<IActionResult> GetBoxScoreMiscFromFile(string season)
        {
            return await _boxScoreMiscFileHandler.GetBoxScoreMiscFromFile(season);
        }

        public async Task<IActionResult> CreateBoxScoreMisc([FromBody] BoxScoreMisc boxScoreMisc, string season)
        {
            return await _boxScoreMiscDatabaseHandler.CreateBoxScoreMisc(boxScoreMisc, season);
        }
    }
}
