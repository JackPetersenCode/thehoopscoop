using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class MLBPlayerGameDataHandler
    {
        private readonly MLBPlayerGameDatabaseHandler _mLBPlayerGameDatabaseHandler;
        private readonly MLBPlayerGameFileHandler _mLBPlayerGameFileHandler;

        public MLBPlayerGameDataHandler(MLBPlayerGameDatabaseHandler mLBPlayerGameDatabaseHandler, MLBPlayerGameFileHandler mLBPlayerGameFileHandler)
        {
            _mLBPlayerGameDatabaseHandler = mLBPlayerGameDatabaseHandler;
            _mLBPlayerGameFileHandler = mLBPlayerGameFileHandler;
        }

        public async Task<ActionResult<IEnumerable<MLBPlayerGameBatting>>> GetMLBPlayerGamesBattingBySeason(string season)
        {
            return await _mLBPlayerGameDatabaseHandler.GetMLBPlayerGamesBattingBySeason(season);
        }

        public async Task<ActionResult<IEnumerable<MLBPlayerGamePitching>>> GetMLBPlayerGamesPitchingBySeason(string season)
        {
            return await _mLBPlayerGameDatabaseHandler.GetMLBPlayerGamesPitchingBySeason(season);
        }

        public async Task<ActionResult<IEnumerable<MLBPlayerGameFielding>>> GetMLBPlayerGamesFieldingBySeason(string season)
        {
            return await _mLBPlayerGameDatabaseHandler.GetMLBPlayerGamesFieldingBySeason(season);
        }
        public async Task<IActionResult> GetMLBPlayerGamesFromFile(string season, string category)
        {
            return await _mLBPlayerGameFileHandler.ReadCSVPlayerGameStats(season, category);
        }

        public async Task<IActionResult> CreateMLBPlayerGamesBatting([FromBody] List<MLBPlayerGameBatting> mLBPlayerGameBatting, string season)
        {
            return await _mLBPlayerGameDatabaseHandler.BulkInsertPlayerGameBatting(mLBPlayerGameBatting, season);
        }

        public async Task<IActionResult> CreateMLBPlayerGamesPitching([FromBody] List<MLBPlayerGamePitching> mLBPlayerGamePitching, string season)
        {
            return await _mLBPlayerGameDatabaseHandler.BulkInsertPlayerGamePitching(mLBPlayerGamePitching, season);
        }

        public async Task<IActionResult> CreateMLBPlayerGamesFielding([FromBody] List<MLBPlayerGameFielding> mLBPlayerGameFielding, string season)
        {
            return await _mLBPlayerGameDatabaseHandler.BulkInsertPlayerGameFielding(mLBPlayerGameFielding, season);
        }
    }

}
