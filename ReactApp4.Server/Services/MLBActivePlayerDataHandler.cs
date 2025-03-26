using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class MLBActivePlayerDataHandler
    {
        private readonly MLBActivePlayerDatabaseHandler _mLBActivePlayerDatabaseHandler;
        private readonly MLBActivePlayerFileHandler _mLBActivePlayerFileHandler;

        public MLBActivePlayerDataHandler(MLBActivePlayerDatabaseHandler mLBActivePlayerDatabaseHandler, MLBActivePlayerFileHandler mLBActivePlayerFileHandler)
        {
            _mLBActivePlayerDatabaseHandler = mLBActivePlayerDatabaseHandler;
            _mLBActivePlayerFileHandler = mLBActivePlayerFileHandler;
        }

        public async Task<ActionResult<IEnumerable<MLBActivePlayer>>> GetMLBActivePlayer(string season)
        {
            return await _mLBActivePlayerDatabaseHandler.GetMLBActivePlayer(season);
        }

        public async Task<IActionResult> GetMLBActivePlayerFromFile(string season)
        {
            return await _mLBActivePlayerFileHandler.GetMLBActivePlayerFromFile(season);
        }

        public async Task<IActionResult> CreateMLBActivePlayer([FromBody] List<MLBActivePlayer> mLBActivePlayer, string season)
        {
            return await _mLBActivePlayerDatabaseHandler.BulkInsertMLBActivePlayer(mLBActivePlayer, season);
        }
    }

}
