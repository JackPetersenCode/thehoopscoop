using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class MLBStatsDataHandler
    {
        private readonly MLBStatsDatabaseHandler _mLBStatsDatabaseHandler;
        //private readonly MLBStatsFileHandler _mLBStatsFileHandler;

        public MLBStatsDataHandler(MLBStatsDatabaseHandler mLBStatsDatabaseHandler)
        {
            _mLBStatsDatabaseHandler = mLBStatsDatabaseHandler;
           // _mLBPlayerGameFileHandler = mLBPlayerGameFileHandler;
        }

        public async Task<ActionResult<IEnumerable<MLBStatsBatting>>> GetMLBStatsBattingBySeason(
            string season,
            string? leagueOption,
            string? selectedTeam,
            string? yearToDateOption,
            int? personId)
        {
            return await _mLBStatsDatabaseHandler.GetMLBStatsBattingBySeason(
                season, leagueOption, selectedTeam, yearToDateOption, personId);
        }


        //public async Task<ActionResult<IEnumerable<MLBPlayerGamePitching>>> GetMLBPlayerGamesPitchingBySeason(string season)
        //{
        //    return await _mLBPlayerGameDatabaseHandler.GetMLBPlayerGamesPitchingBySeason(season);
        //}
//
        //public async Task<ActionResult<IEnumerable<MLBPlayerGameFielding>>> GetMLBPlayerGamesFieldingBySeason(string season)
        //{
        //    return await _mLBPlayerGameDatabaseHandler.GetMLBPlayerGamesFieldingBySeason(season);
        //}


    }

}
