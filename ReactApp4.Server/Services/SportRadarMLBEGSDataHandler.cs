using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class SportRadarMLBEGSDataHandler
    {
        private readonly SportRadarMLBEGSDatabaseHandler _sportRadarMLBEGSDatabaseHandler;
        private readonly SportRadarMLBEGSFileHandler _sportRadarMLBEGSFileHandler;

        public SportRadarMLBEGSDataHandler(SportRadarMLBEGSDatabaseHandler sportRadarMLBEGSDatabaseHandler, SportRadarMLBEGSFileHandler sportRadarMLBEGSFileHandler)
        {
            _sportRadarMLBEGSDatabaseHandler = sportRadarMLBEGSDatabaseHandler;
            _sportRadarMLBEGSFileHandler = sportRadarMLBEGSFileHandler;
        }
        public async Task<IActionResult> GetSportRadarMLBEGSFromFile(string file)
        {
            return await _sportRadarMLBEGSFileHandler.SportRadarMLBEGSReadCSV(file);
        }
        public async Task<IActionResult> CreateSportRadarMLBEGSGameInfo([FromBody] List<SportRadarMLBEGSGameInfo> sportRadarMLBEGSGameInfo, string season)
        {
            return await _sportRadarMLBEGSDatabaseHandler.CreateSportRadarMLBEGSGameInfo(sportRadarMLBEGSGameInfo, season);
        }
        public async Task<IActionResult> CreateSportRadarMLBLeagueSchedule([FromBody] List<SportRadarMLBLeagueSchedule> sportRadarMLBLeagueSchedule, string season)
        {
            return await _sportRadarMLBEGSDatabaseHandler.CreateSportRadarMLBLeagueSchedule(sportRadarMLBLeagueSchedule, season);
        }
        public async Task<IActionResult> CreateSportRadarMLBPBPAtBats([FromBody] List<SportRadarMLBPBPAtBat> sportRadarMLBPBPAtBats, string season)
        {
            return await _sportRadarMLBEGSDatabaseHandler.CreateSportRadarMLBPBPAtBats(sportRadarMLBPBPAtBats, season);
        }
        public async Task<IActionResult> CreateSportRadarMLBPBPPitchEvents([FromBody] List<SportRadarMLBPBPPitchEvent> sportRadarMLBPBPPitchEvents, string season)
        {
            return await _sportRadarMLBEGSDatabaseHandler.CreateSportRadarMLBPBPPitchEvents(sportRadarMLBPBPPitchEvents, season);
        }
    }
}