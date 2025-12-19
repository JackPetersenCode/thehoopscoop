using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class OddsApiDataHandler
    {
        private readonly OddsApiDatabaseHandler _oddsApiDatabaseHandler;
        private readonly OddsApiFileHandler _oddsApiFileHandler;

        public OddsApiDataHandler(OddsApiDatabaseHandler oddsApiDatabaseHandler, OddsApiFileHandler oddsApiFileHandler)
        {
            _oddsApiDatabaseHandler = oddsApiDatabaseHandler;
            _oddsApiFileHandler = oddsApiFileHandler;
        }

        public async Task<ActionResult<IEnumerable<OddsApiH2H>>> GetOddsApiH2HBySeason(string season)
        {
            return await _oddsApiDatabaseHandler.GetOddsApiH2HBySeason(season);
        }

        public async Task<IActionResult> GetOddsApiFromFile(string sport, string season, string fileName)
        {
            Console.WriteLine("butthole");
            return await _oddsApiFileHandler.ReadCSVFile(sport, season, fileName);
        }

        public async Task<IActionResult> CreateOddsApiH2H([FromBody] List<OddsApiH2H> oddsApiH2H, string sport, string season)
        {
            return await _oddsApiDatabaseHandler.BulkInsertOddsApiH2H(oddsApiH2H, sport, season);
        }

        public async Task<IActionResult> CreateOddsApiPlayerProp([FromBody] List<OddsApiPlayerProp> oddsApiPlayerProp, string season)
        {
            return await _oddsApiDatabaseHandler.InsertPlayerPropOdds(oddsApiPlayerProp, season);
        }
    }
}