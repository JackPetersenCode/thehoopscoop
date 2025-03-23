using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;

namespace ReactApp4.Server.Services
{
    public class LeagueGameDataHandler
    {
        private readonly LeagueGameDatabaseHandler _leagueGameDatabaseHandler;
        private readonly LeagueGameFileHandler _leagueGameFileHandler;

        public LeagueGameDataHandler(LeagueGameDatabaseHandler leagueGameDatabaseHandler, LeagueGameFileHandler leagueGameFileHandler)
        {
            _leagueGameDatabaseHandler = leagueGameDatabaseHandler;
            _leagueGameFileHandler = leagueGameFileHandler;
        }

        public async Task<ActionResult<IEnumerable<LeagueGame>>> GetGamesBySeason(string season)
        {
            return await _leagueGameDatabaseHandler.GetGamesBySeason(season);
        }

        public async Task<ActionResult<IEnumerable<ShotChartsGame>>> GetShotChartsGames(string playerId, string season)
        {
            return await _leagueGameDatabaseHandler.GetShotChartsGames(playerId, season);
        }

        public async Task<IActionResult> GetGamesFromFile(string season)
        {
            return await _leagueGameFileHandler.GetGamesFromFile(season);
        }

        public async Task<IActionResult> GetBackToBacks(LeagueGameWithHomeVisitor game, string previousDate, string season)
        {
            return await _leagueGameDatabaseHandler.GetBackToBacks(game, previousDate, season);
        }

        public async Task<IActionResult> CreateLeagueGame([FromBody] object[] leagueGame)
        {
            return await _leagueGameDatabaseHandler.CreateLeagueGame(leagueGame);
        }
        
        public async Task<IActionResult> B2BAverages(string[] team_ids, string season)
        {
            return await _leagueGameDatabaseHandler.B2BAverages(team_ids, season);
        }
        public async Task<IActionResult> TeamPtsAverage(string[] team_ids, string season)
        {
            return await _leagueGameDatabaseHandler.TeamPtsAverage(team_ids, season);
        }
        
    }

}
