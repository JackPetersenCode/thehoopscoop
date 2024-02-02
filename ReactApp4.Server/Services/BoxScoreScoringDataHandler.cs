using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class BoxScoreScoringDataHandler
    {
        private readonly BoxScoreScoringDatabaseHandler _boxScoreScoringDatabaseHandler;
        private readonly BoxScoreScoringFileHandler _boxScoreScoringFileHandler;

        public BoxScoreScoringDataHandler(BoxScoreScoringDatabaseHandler boxScoreScoringDatabaseHandler, BoxScoreScoringFileHandler boxScoreScoringFileHandler)
        {
            _boxScoreScoringDatabaseHandler = boxScoreScoringDatabaseHandler;
            _boxScoreScoringFileHandler = boxScoreScoringFileHandler;
        }

        public async Task<ActionResult<IEnumerable<BoxScoreScoring>>> GetBoxScoreScoringBySeason(string season)
        {
            return await _boxScoreScoringDatabaseHandler.GetBoxScoreScoringBySeason(season);
        }

        public async Task<IActionResult> GetBoxScoreScoringFromFile(string season)
        {
            return await _boxScoreScoringFileHandler.GetBoxScoreScoringFromFile(season);
        }

        public async Task<IActionResult> CreateBoxScoreScoring([FromBody] BoxScoreScoring boxScoreScoring, string season)
        {
            return await _boxScoreScoringDatabaseHandler.CreateBoxScoreScoring(boxScoreScoring, season);
        }
    }
}
