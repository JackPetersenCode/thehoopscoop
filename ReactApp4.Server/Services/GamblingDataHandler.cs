using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class GamblingDataHandler
    {
        private readonly GamblingDatabaseHandler _gamblingDatabaseHandler;
        private readonly GamblingFileHandler _gamblingFileHandler;

        public GamblingDataHandler(GamblingDatabaseHandler gamblingDatabaseHandler, GamblingFileHandler gamblingFileHandler)
        {
            _gamblingDatabaseHandler = gamblingDatabaseHandler;
            _gamblingFileHandler = gamblingFileHandler;
        }
        public async Task<IActionResult> GetUpcomingGames(string season)
        {
            return await _gamblingDatabaseHandler.GetUpcomingGames(season);
        }

        public async Task<IActionResult> GetPreviousGameId(string season, string teamId, string gameDate)
        {
            return await _gamblingDatabaseHandler.GetPreviousGameId(season, teamId, gameDate);
        }

        public async Task<IActionResult> GetTeamIdFromTeamName(string season)
        {
            return await _gamblingDatabaseHandler.GetUpcomingGames(season);
        }
        public async Task<IActionResult> GetNewOddsFromFile(string season)
        {
            return await _gamblingFileHandler.GetNewOddsFromFile(season);
        }
        public async Task<IActionResult> PostNewOdds([FromBody] NewOdds newOdds, string season)
        {
            return await _gamblingDatabaseHandler.PostNewOdds(newOdds, season);
        }
    }
}
