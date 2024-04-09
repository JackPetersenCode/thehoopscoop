


using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class BoxScoresDataHandler
    {
        private readonly BoxScoresFileHandler _boxScoresFileHandler;
        private readonly BoxScoresDatabaseHandler _boxScoresDatabaseHandler;

        public BoxScoresDataHandler(BoxScoresFileHandler boxScoresFileHandler, BoxScoresDatabaseHandler boxScoresDatabaseHandler)
        {

            _boxScoresFileHandler = boxScoresFileHandler;
            _boxScoresDatabaseHandler = boxScoresDatabaseHandler;
        }

        public async Task<IActionResult> GetBoxScoresFromFile(string season, string boxType, string numPlayers)
        {
            Console.WriteLine(season, boxType, numPlayers);
            Console.WriteLine("PARAMETERS");
            return await _boxScoresFileHandler.GetBoxScoresFromFile(season, boxType, numPlayers);
        }

        public async Task<IActionResult> GetBoxScores(string season, string boxType, string order, string sortField, string perMode, string selectedTeam, string selectedOpponent)
        {
            return await _boxScoresDatabaseHandler.GetBoxScores(season, boxType, order, sortField, perMode, selectedTeam, selectedOpponent);
        }


    }
}
