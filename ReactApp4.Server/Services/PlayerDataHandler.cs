using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class PlayerDataHandler
    {
        private readonly PlayerDatabaseHandler _playerDatabaseHandler;
        private readonly PlayerFileHandler _playerFileHandler;

        public PlayerDataHandler(PlayerDatabaseHandler playerDatabaseHandler, PlayerFileHandler playerFileHandler)
        {
            _playerDatabaseHandler = playerDatabaseHandler;
            _playerFileHandler = playerFileHandler;
        }

        public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers()
        {
            return await _playerDatabaseHandler.GetAllPlayers();
        }
        public async Task<IActionResult> GetPlayersFromFile()
        {
            return await _playerFileHandler.GetPlayersFromFile();
        }

        public async Task<IActionResult> CreatePlayer([FromBody] Player player)
        {
            return await _playerDatabaseHandler.CreatePlayer(player);
        }
    }
}
