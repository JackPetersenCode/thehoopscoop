using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactApp4.Server.Services;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class MLBTeamDataHandler
    {
        private readonly MLBTeamDatabaseHandler _mLBTeamDatabaseHandler;
        private readonly MLBTeamFileHandler _mLBTeamFileHandler;

        public MLBTeamDataHandler(MLBTeamDatabaseHandler mLBTeamDatabaseHandler, MLBTeamFileHandler mLBTeamFileHandler)
        {
            _mLBTeamDatabaseHandler = mLBTeamDatabaseHandler;
            _mLBTeamFileHandler = mLBTeamFileHandler;
        }

        public async Task<ActionResult<IEnumerable<MLBTeamInfo>>> GetMLBTeamInfoBySeason(string season)
        {
            return await _mLBTeamDatabaseHandler.GetMLBTeamInfoBySeason(season);
        }
        public async Task<IActionResult> GetMLBTeamInfoFromFile(string season)
        {
            return await _mLBTeamFileHandler.ReadCSVTeamInfoBySeason(season);
        }

        public async Task<IActionResult> CreateMLBTeamInfoBySeason([FromBody] List<MLBTeamInfo> mLBTeamInfo, string season)
        {
            return await _mLBTeamDatabaseHandler.PostTeamInfoBySeason(mLBTeamInfo, season);
        }

    }

}
