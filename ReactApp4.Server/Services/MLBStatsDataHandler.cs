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

        public async Task<ActionResult<IEnumerable<IMLBStatsBatting>>> GetMLBStatsBattingBySeason(
            string season,
            string? leagueOption,
            string? selectedTeam,
            string? yearToDateOption,
            string? selectedOpponent,
            string? selectedSplit,
            int? selectedPlayer,
            string? sortField,
            string? order)
        {
            return await _mLBStatsDatabaseHandler.GetMLBStatsBattingBySeason(
                season, leagueOption, selectedTeam, yearToDateOption, selectedOpponent, selectedSplit, selectedPlayer, sortField, order);
        }

        public async Task<ActionResult<IEnumerable<IMLBStatsBatting>>> GetMLBStatsBattingBySeasonSplits(
            string season,
            string? leagueOption,
            string? selectedTeam,
            string? yearToDateOption,
            string? selectedOpponent,
            string? selectedSplit,
            int? selectedPlayer,
            string? sortField,
            string? order)
        {
            return await _mLBStatsDatabaseHandler.GetMLBStatsBattingBySeasonSplits(
                season, leagueOption, selectedTeam, yearToDateOption, selectedOpponent, selectedSplit, selectedPlayer, sortField, order);
        }

        public async Task<ActionResult<IEnumerable<IMLBStatsPitching>>> GetMLBStatsPitchingBySeason(
            string season,
            string? leagueOption,
            string? selectedTeam,
            string? yearToDateOption,
            string? selectedOpponent,
            string? selectedSplit,
            int? selectedPlayer,
            string? sortField,
            string? order)
        {
            return await _mLBStatsDatabaseHandler.GetMLBStatsPitchingBySeason(
                season, leagueOption, selectedTeam, yearToDateOption, selectedOpponent, selectedSplit, selectedPlayer, sortField, order);
        }

        public async Task<ActionResult<IEnumerable<IMLBStatsPitching>>> GetMLBStatsPitchingBySeasonSplits(
            string season,
            string? leagueOption,
            string? selectedTeam,
            string? yearToDateOption,
            string? selectedOpponent,
            string? selectedSplit,
            int? selectedPlayer,
            string? sortField,
            string? order)
        {
            return await _mLBStatsDatabaseHandler.GetMLBStatsPitchingBySeasonSplits(
                season, leagueOption, selectedTeam, yearToDateOption, selectedOpponent, selectedSplit, selectedPlayer, sortField, order);
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
