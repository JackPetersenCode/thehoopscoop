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

        public async Task<IActionResult> GetTeamNameFromId(string teamId)
        {
            return await _gamblingDatabaseHandler.GetTeamNameFromId(teamId);
        }
        public async Task<IActionResult> GetRosterBySeasonByTeam(string season, string teamId, string gameId)
        {
            return await _gamblingDatabaseHandler.GetRosterBySeasonByTeam(season, teamId, gameId);
        }

        public async Task<IActionResult> GetMoneyline(string season, string teamName, string gameDate)
        {
            return await _gamblingDatabaseHandler.GetMoneyline(season, teamName, gameDate);
        }

        public async Task<IActionResult> GetNewOdds(string season, string teamName, string gameDate, string H_or_V)
        {
            return await _gamblingDatabaseHandler.GetNewOdds(season, teamName, gameDate, H_or_V);
        }

        public async Task<IActionResult> GetWinPctBySeason(string season)
        {
            return await _gamblingDatabaseHandler.GetWinPctBySeason(season);
        }

        public async Task<IActionResult> GetWinPctBySeasonByTeam(string season, string team)
        {
            return await _gamblingDatabaseHandler.GetWinPctBySeasonByTeam(season, team);
        }

        public async Task<IActionResult> GetWinPctOverall()
        {
            return await _gamblingDatabaseHandler.GetWinPctOverall();
        }
        
        public async Task<IActionResult> GetHistoricalResults(string season, string teamName)
        {
            return await _gamblingDatabaseHandler.GetHistoricalResults(season, teamName);
        }
        public async Task<IActionResult> GetTopTenHistorical(string season)
        {
            return await _gamblingDatabaseHandler.GetTopTenHistorical(season);
        }
        public async Task<IActionResult> GetRosterBySeasonByTeamFromAdvanced(string season, string teamId, string gameId)
        {
            return await _gamblingDatabaseHandler.GetRosterBySeasonByTeamFromAdvanced(season, teamId, gameId);
        }
        public async Task<IActionResult> GetAverageScore(string season, string gameDate)
        {
            return await _gamblingDatabaseHandler.GetAverageScore(season, gameDate);
        }
        public async Task<IActionResult> GetLeagueGamesWithHomeVisitor(string season)
        {
            return await _gamblingDatabaseHandler.GetLeagueGamesWithHomeVisitor(season);
        }
        public async Task<IActionResult> GetNewOddsFromFile(string season)
        {
            return await _gamblingFileHandler.GetNewOddsFromFile(season);
        }
        public async Task<IActionResult> PostNewOdds([FromBody] NewOdds newOdds, string season)
        {
            return await _gamblingDatabaseHandler.PostNewOdds(newOdds, season);
        }
        public async Task<IActionResult> PostExpectedMatchup([FromBody] ExpectedMatchup expectedMatchup, string season)
        {
            return await _gamblingDatabaseHandler.PostExpectedMatchup(expectedMatchup, season);
        }
    }
}
