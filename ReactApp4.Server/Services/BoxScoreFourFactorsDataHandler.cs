using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class BoxScoreFourFactorsDataHandler
    {
        private readonly BoxScoreFourFactorsDatabaseHandler _boxScoreFourFactorsDatabaseHandler;
        private readonly BoxScoreFourFactorsFileHandler _boxScoreFourFactorsFileHandler;

        public BoxScoreFourFactorsDataHandler(BoxScoreFourFactorsDatabaseHandler boxScoreFourFactorsDatabaseHandler, BoxScoreFourFactorsFileHandler boxScoreFourFactorsFileHandler)
        {
            _boxScoreFourFactorsDatabaseHandler = boxScoreFourFactorsDatabaseHandler;
            _boxScoreFourFactorsFileHandler = boxScoreFourFactorsFileHandler;
        }

        public async Task<ActionResult<IEnumerable<BoxScoreFourFactors>>> GetBoxScoreFourFactorsBySeason(string season)
        {
            return await _boxScoreFourFactorsDatabaseHandler.GetBoxScoreFourFactorsBySeason(season);
        }

        public async Task<IActionResult> GetBoxScoreFourFactorsFromFile(string season)
        {
            return await _boxScoreFourFactorsFileHandler.GetBoxScoreFourFactorsFromFile(season);
        }

        public async Task<IActionResult> CreateBoxScoreFourFactors([FromBody] BoxScoreFourFactors boxScoreFourFactors, string season)
        {
            return await _boxScoreFourFactorsDatabaseHandler.CreateBoxScoreFourFactors(boxScoreFourFactors, season);
        }
    }
}

