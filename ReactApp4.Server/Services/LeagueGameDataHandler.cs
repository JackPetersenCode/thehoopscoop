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

        public async Task<IActionResult> GetGamesFromFile(string season)
        {
            return await _leagueGameFileHandler.GetGamesFromFile(season);
        }

        public async Task<IActionResult> CreateLeagueGame([FromBody] object[] leagueGame)
        {
            return await _leagueGameDatabaseHandler.CreateLeagueGame(leagueGame);
        }
    }

}
