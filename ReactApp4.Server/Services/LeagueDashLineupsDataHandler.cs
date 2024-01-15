


using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class LeagueDashLineupsDataHandler
    {
        private readonly LeagueDashLineupsFileHandler _leagueDashLineupsFileHandler;
        private readonly LeagueDashLineupsDatabaseHandler _leagueDashLineupsDatabaseHandler;

        public LeagueDashLineupsDataHandler(LeagueDashLineupsFileHandler leagueDashLineupsFileHandler, LeagueDashLineupsDatabaseHandler leagueDashLineupsDatabaseHandler)
        {

            _leagueDashLineupsFileHandler = leagueDashLineupsFileHandler;
            _leagueDashLineupsDatabaseHandler = leagueDashLineupsDatabaseHandler;
         }

        public async Task<IActionResult> GetLeagueDashLineupsFromFile(string season, string boxType, string numPlayers)
        {
            Console.WriteLine(season, boxType, numPlayers);
            Console.WriteLine("PARAMETERS");
            return await _leagueDashLineupsFileHandler.GetLeagueDashLineupsFromFile(season, boxType, numPlayers);
        }

        public async Task<IActionResult> GetLeagueDashLineups(string season, string boxType, string numPlayers)
        {
            return await _leagueDashLineupsDatabaseHandler.GetLeagueDashLineups(season, boxType, numPlayers);
        }
        public async Task<IActionResult> CreateLeagueDashLineup([FromBody] object[] leagueDashLineup, string season, string boxType, string numPlayers)
        {
            return await _leagueDashLineupsDatabaseHandler.CreateLeagueDashLineup(leagueDashLineup, season, boxType, numPlayers);
        }


    }
}
