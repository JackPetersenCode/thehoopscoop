using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class MLBPlayByPlayDataHandler
    {
        private readonly MLBPlayByPlayDatabaseHandler _mLBPlayByPlayDatabaseHandler;
        private readonly MLBPlayByPlayFileHandler _mLBPlayByPlayFileHandler;

        public MLBPlayByPlayDataHandler(MLBPlayByPlayDatabaseHandler mLBPlayByPlayDatabaseHandler, MLBPlayByPlayFileHandler mLBPlayByPlayFileHandler)
        {
            _mLBPlayByPlayDatabaseHandler = mLBPlayByPlayDatabaseHandler;
            _mLBPlayByPlayFileHandler = mLBPlayByPlayFileHandler;
        }
        public async Task<IActionResult> GetMLBPlaysBySeasonFromFile(string season)
        {
            return await _mLBPlayByPlayFileHandler.ReadCSVPlaysBySeason(season);
        }
        public async Task<IActionResult> GetMLBPlayEventsBySeasonFromFile(string season)
        {
            return await _mLBPlayByPlayFileHandler.ReadCSVPlayEventsBySeason(season);
        }        
        public async Task<IActionResult> GetMLBRunnersBySeasonFromFile(string season)
        {
            return await _mLBPlayByPlayFileHandler.ReadCSVRunnersBySeason(season);
        }
        public async Task<IActionResult> GetMLBRunnersCreditsBySeasonFromFile(string season)
        {
            return await _mLBPlayByPlayFileHandler.ReadCSVRunnersCreditsBySeason(season);
        }
    
    }
}