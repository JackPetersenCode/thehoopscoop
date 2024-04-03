using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;

namespace ReactApp4.Server.Services
{
    public class ShotDataHandler
    {
        private readonly ShotDatabaseHandler _shotsDatabaseHandler;
        private readonly ShotFileHandler _shotsFileHandler;

        public ShotDataHandler(ShotDatabaseHandler shotsDatabaseHandler, ShotFileHandler shotsFileHandler)
        {
            _shotsDatabaseHandler = shotsDatabaseHandler;
            _shotsFileHandler = shotsFileHandler;
        }

        public async Task<ActionResult<IEnumerable<Shot>>> GetShotsBySeason(string playerId, string season)
        {
            return await _shotsDatabaseHandler.GetShotsBySeason(playerId, season);
        }

        public async Task<ActionResult<IEnumerable<Shot>>> GetShotsByGame(string playerId, string season, string gameId)
        {
            return await _shotsDatabaseHandler.GetShotsByGame(playerId, season, gameId);
        }

        public async Task<IActionResult> GetShotsFromFile(string season)
        {
            return await _shotsFileHandler.GetShotsFromFile(season);
        }

        public async Task<IActionResult> CreateShot([FromBody] Shot shot, string season)
        {
            return await _shotsDatabaseHandler.CreateShot(shot, season);
        }
    }

}
