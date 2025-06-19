using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactApp4.Server.Data;
using ReactApp4.Server.Helpers;
using ReactApp4.Server.Services;

namespace ReactApp4.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamblingController : ControllerBase
    {
        private readonly GamblingDataHandler _gamblingDataHandler;

        public GamblingController(GamblingDataHandler gamblingDataHandler)
        {
            _gamblingDataHandler = gamblingDataHandler;
        }
        [HttpGet("upcomingGames/{season}")]
        public async Task<IActionResult> GetUpcomingGames(string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetUpcomingGames(season);
        }

        [HttpGet("teamNameFromId/{teamId}")]
        public async Task<IActionResult> GetTeamNameFromId(string teamId)
        {
            return await _gamblingDataHandler.GetTeamNameFromId(teamId);
        }

        [HttpGet("previousGameId/{season}/{teamId}/{gameDate}")]
        public async Task<IActionResult> GetPreviousGameId(string season, string teamId, string gameDate)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetPreviousGameId(season, teamId, gameDate);
        }

        [HttpGet("moneyline/{season}/{teamName}/{gameDate}")]
        public async Task<IActionResult> GetMoneyline(string season, string teamName, string gameDate)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetMoneyline(season, teamName, gameDate);
        }

        [HttpGet("newOdds/{season}/{teamName}/{gameDate}/{H_or_V}")]
        public async Task<IActionResult> GetNewOdds(string season, string teamName, string gameDate, string H_or_V)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            if (!SeasonConstants.IsValidH_or_V(H_or_V))
                return BadRequest("Invalid H_or_V");
            return await _gamblingDataHandler.GetNewOdds(season, teamName, gameDate, H_or_V);
        }

        [HttpGet("getRoster/{season}/{teamId}/{gameId?}")]
        public async Task<IActionResult> GetRosterBySeasonByTeam(string season, string teamId, string gameId = null)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetRosterBySeasonByTeam(season, teamId, gameId);
        }

        [HttpGet("getRosterFromAdvanced/{season}/{teamId}/{gameId?}")]
        public async Task<IActionResult> GetRosterBySeasonByTeamFromAdvanced(string season, string teamId, string gameId = null)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetRosterBySeasonByTeamFromAdvanced(season, teamId, gameId);
        }

        [HttpGet("averageScore/{season}/{gameDate?}")]
        public async Task<IActionResult> GetAverageScore(string season, string gameDate = null)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetAverageScore(season, gameDate);
        }

        [HttpGet("historicalResults/{season}/{teamName?}")]
        public async Task<IActionResult> GetHistoricalResults(string season, string teamName = null)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetHistoricalResults(season, teamName);
        }

        [HttpGet("topTenHistorical/{season}")]
        public async Task<IActionResult> GetTopTenHistorical(string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetTopTenHistorical(season);
        }

        [HttpGet("leagueGamesWithHomeVisitor/{season}")]
        public async Task<IActionResult> GetLeagueGamesWithHomeVisitor(string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetLeagueGamesWithHomeVisitor(season);
        }

        [HttpGet("winPct/{season}")]
        public async Task<IActionResult> GetWinPctBySeason(string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetWinPctBySeason(season);
        }

        [HttpGet("winPctByTeam/{season}/{teamName}")]
        public async Task<IActionResult> GetWinPctBySeasonByTeam(string season, string teamName)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetWinPctBySeasonByTeam(season, teamName);
        }

        [HttpGet("winPctOverall")]
        public async Task<IActionResult> GetWinPctOverall()
        {
            return await _gamblingDataHandler.GetWinPctOverall();
        }

        [HttpGet("read/newOdds/{season}")]
        public async Task<IActionResult> GetNewOddsFromFile(string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            return await _gamblingDataHandler.GetNewOddsFromFile(season);
        }

        [Authorize]
        [HttpPost("newOdds/{season}")]
        public async Task<IActionResult> PostNewOdds([FromBody] NewOdds newOdds, string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            if (newOdds == null)
                return BadRequest("Invalid boxScoreAdvanced data");

            return await _gamblingDataHandler.PostNewOdds(newOdds, season);
        }

        [Authorize]
        [HttpPost("Jackorithm/{season}")]
        public async Task<IActionResult> PostExpectedMatchup([FromBody] ExpectedMatchup expectedMatchup, string season)
        {
            if (!SeasonConstants.IsValidNBASeason(season))
                return BadRequest("Invalid NBA season.");
            if (expectedMatchup == null)
                return BadRequest("Invalid boxScoreAdvanced data");

            return await _gamblingDataHandler.PostExpectedMatchup(expectedMatchup, season);
        }
        

        [HttpGet("uniqueGamePks/{table}")]
        public async Task<IActionResult> GetUniqueMLBGamePks(string table)
        {
            //if (!SeasonConstants.IsValidMLBSeason(table))
            //	return BadRequest("Invalid MLB season.");
            return await _gamblingDataHandler.GetUniqueMLBGamePks(table);
        }

    }
}
