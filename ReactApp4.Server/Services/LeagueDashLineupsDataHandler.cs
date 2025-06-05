


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
            return await _leagueDashLineupsFileHandler.GetLeagueDashLineupsFromFile(season, boxType, numPlayers);
        }

        public async Task<IActionResult> GetLeagueDashLineups(string season, string boxType, string numPlayers, string order, string sortField, string perMode, string selectedTeam)
        {
            return await _leagueDashLineupsDatabaseHandler.GetLeagueDashLineups(season, boxType, numPlayers, order, sortField, perMode, selectedTeam);
        }
        public async Task<IActionResult> CreateLeagueDashLineup([FromBody] object[] leagueDashLineup, string season, string boxType, string numPlayers)
        {
            return await _leagueDashLineupsDatabaseHandler.CreateLeagueDashLineup(leagueDashLineup, season, boxType, numPlayers);
        }


        public async Task<IActionResult> DeleteLeagueDashLineup(string season, string boxType, string numPlayers)
        {
            return await _leagueDashLineupsDatabaseHandler.DeleteLeagueDashLineup(season, boxType, numPlayers);
        }


    }
}
