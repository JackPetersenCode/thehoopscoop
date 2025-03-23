using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class MLBGameDataHandler
    {
        private readonly MLBGameDatabaseHandler _mLBGameDatabaseHandler;
        private readonly MLBGameFileHandler _mLBGameFileHandler;

        public MLBGameDataHandler(MLBGameDatabaseHandler mLBGameDatabaseHandler, MLBGameFileHandler mLBGameFileHandler)
        {
            _mLBGameDatabaseHandler = mLBGameDatabaseHandler;
            _mLBGameFileHandler = mLBGameFileHandler;
        }

        public async Task<ActionResult<IEnumerable<MLBGame>>> GetMLBGamesBySeason(string season)
        {
            return await _mLBGameDatabaseHandler.GetMLBGamesBySeason(season);
        }

        public async Task<IActionResult> GetMLBGamesFromFile(string season)
        {
            return await _mLBGameFileHandler.GetMLBGamesFromFile(season);
        }

        public async Task<IActionResult> CreateMLBGames([FromBody] MLBGame mLBGame, string season)
        {
            return await _mLBGameDatabaseHandler.CreateMLBGames(mLBGame, season);
        }
    }

}
