using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReactApp4.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using ReactApp4.Server.Data;
using static System.Net.WebRequestMethods;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ReactApp4.Server.Services
{
    public class LeagueDashLineupsDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<IActionResult> GetLeagueDashLineups(string season, string boxType, string numPlayers, string order, string sortField, string perMode, string selectedTeam)
        {
            try
            {
                Console.WriteLine(selectedTeam);
                var tableName = $"league_dash_lineups_{boxType.ToLower()}_{numPlayers}man_{season}";

                var query = $"SELECT * FROM {tableName} WHERE team_id LIKE '%{selectedTeam}%' ORDER BY {sortField} {order}";

                //int pageSize = 100;

                if (boxType == "Advanced")
                {
                    var leagueDashLineups = await _context.LeagueDashLineupAdvanceds.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(leagueDashLineups.Count);
                    return Ok(leagueDashLineups); // Wrap the result in OkObjectResult
                } else if (boxType == "Base")
                {
                    if (perMode == "Totals")
                    {
                        query = $"SELECT * FROM {tableName} WHERE team_id LIKE '%{selectedTeam}%' ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per Game")
                    {
                        Console.WriteLine("HERE");
                        query = $"SELECT id, group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min / gp AS min, " +
                                $"fgm / gp AS fgm, fga / gp AS fga, fg_pct, fg3m / gp AS fg3m, fg3a / gp AS fg3a, fg3_pct, " +
                                $"ftm / gp AS ftm, fta / gp AS fta, ft_pct, oreb / gp AS oreb, dreb / gp AS dreb, reb / gp AS reb, " +
                                $"ast / gp AS ast, tov / gp AS tov, stl / gp AS stl, blk / gp AS blk, blka / gp AS blka, pf / gp AS pf, pfd / gp AS pfd, " +
                                $"pts / gp AS pts, plus_minus / gp AS plus_minus, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, fgm_rank, fga_rank, fg_pct_rank, " +
                                $"fg3m_rank, fg3a_rank, fg3_pct_rank, ftm_rank, fta_rank, ft_pct_rank, oreb_rank, dreb_rank, reb_rank, ast_rank, tov_rank, " +
                                $"stl_rank, blk_rank, blka_rank, pf_rank, pfd_rank, pts_rank, plus_minus_rank " +
                                $"FROM {tableName} " +
                                $"WHERE team_id LIKE '%{selectedTeam}%' " +
                                $"ORDER BY {sortField} {order} ";
                                
                        Console.WriteLine(query);

                    }
                    else if (perMode == "Per Minute")
                    {
                        query = $"SELECT id, group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, " +
                                $"fgm / min AS fgm, fga / min AS fga, fg_pct, fg3m / min AS fg3m, fg3a / min AS fg3a, fg3_pct, " +
                                $"ftm / min AS ftm, fta / min AS fta, ft_pct, oreb / min AS oreb, dreb / min AS dreb, reb / min AS reb, " +
                                $"ast / min AS ast, tov / min AS tov, stl / min AS stl, blk / min AS blk, blka / min AS blka, pf / min AS pf, pfd / min AS pfd, " +
                                $"pts / min AS pts, plus_minus / min AS plus_minus, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, fgm_rank, fga_rank, fg_pct_rank, " +
                                $"fg3m_rank, fg3a_rank, fg3_pct_rank, ftm_rank, fta_rank, ft_pct_rank, oreb_rank, dreb_rank, reb_rank, ast_rank, tov_rank, " +
                                $"stl_rank, blk_rank, blka_rank, pf_rank, pfd_rank, pts_rank, plus_minus_rank " +
                                $"FROM {tableName} " +
                                $"WHERE team_id LIKE '%{selectedTeam}%' " +
                                $"ORDER BY {sortField} {order}";
                        Console.WriteLine(query);

                    } else if (perMode == "Per 100 Poss")
                    {
                        var joinedTable = $"league_dash_lineups_advanced_{numPlayers}man_{season}";

                        query = $"SELECT {tableName}.id, {tableName}.group_set, {tableName}.group_id, {tableName}.group_name, {tableName}.team_id, {tableName}.team_abbreviation, {tableName}.gp, {tableName}.w, {tableName}.l, {tableName}.w_pct, {tableName}.min / {joinedTable}.poss AS min, " +
                                $"fgm / {joinedTable}.poss AS fgm, fga / {joinedTable}.poss AS fga, fg_pct, fg3m / {joinedTable}.poss AS fg3m, fg3a / {joinedTable}.poss AS fg3a, fg3_pct, " +
                                $"ftm / {joinedTable}.poss AS ftm, fta / {joinedTable}.poss AS fta, ft_pct, oreb / {joinedTable}.poss AS oreb, dreb / {joinedTable}.poss AS dreb, reb / {joinedTable}.poss AS reb, " +
                                $"ast / {joinedTable}.poss AS ast, tov / {joinedTable}.poss AS tov, stl / {joinedTable}.poss AS stl, blk / {joinedTable}.poss AS blk, blka / {joinedTable}.poss AS blka, pf / {joinedTable}.poss AS pf, pfd / {joinedTable}.poss AS pfd, " +
                                $"pts / {joinedTable}.poss AS pts, plus_minus / {joinedTable}.poss AS plus_minus, {tableName}.gp_rank, {tableName}.w_rank, {tableName}.l_rank, {tableName}.w_pct_rank, {tableName}.min_rank, fgm_rank, fga_rank, fg_pct_rank, " +
                                $"fg3m_rank, fg3a_rank, fg3_pct_rank, ftm_rank, fta_rank, ft_pct_rank, oreb_rank, dreb_rank, reb_rank, ast_rank, tov_rank, " +
                                $"stl_rank, blk_rank, blka_rank, pf_rank, pfd_rank, pts_rank, plus_minus_rank " +
                                $"FROM {tableName} " +
                                $"INNER JOIN {joinedTable} " +
                                $"ON {tableName}.group_id = {joinedTable}.group_id " +
                                $"WHERE {tableName}.team_id LIKE '%{selectedTeam}%' " +
                                $"ORDER BY {sortField} {order}";
                    }
                    var leagueDashLineups = await _context.LeagueDashLineupBases.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(leagueDashLineups.Count);
                    return Ok(leagueDashLineups);
            
                } else if (boxType == "FourFactors")
                {
                    var leagueDashLineups = await _context.LeagueDashLineupFourFactors.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(leagueDashLineups.Count);
                    return Ok(leagueDashLineups);
                } else if (boxType == "Misc")
                {
                    var joinedTable = $"league_dash_lineups_advanced_{numPlayers}man_{season}";

                    if (perMode == "Totals")
                    {
                        query = $"SELECT * FROM {tableName} WHERE team_id LIKE '%{selectedTeam}%' ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per 100 Poss")
                    {
                        query = $"SELECT {tableName}.id, {tableName}.group_set, {tableName}.group_id, {tableName}.group_name, " +
                                $"{tableName}.team_id, {tableName}.team_abbreviation, {tableName}.gp, {tableName}.w, {tableName}.l, " +
                                $"{tableName}.w_pct, {tableName}.min / {joinedTable}.poss AS min, {tableName}.pts_off_tov / {joinedTable}.poss AS pts_off_tov, " +
                                $"{tableName}.pts_2nd_chance / {joinedTable}.poss AS pts_2nd_chance, {tableName}.pts_fb / {joinedTable}.poss AS pts_fb, " +
                                $"{tableName}.pts_paint / {joinedTable}.poss AS pts_paint, {tableName}.opp_pts_off_tov / {joinedTable}.poss AS opp_pts_off_tov, {tableName}.opp_pts_2nd_chance / {joinedTable}.poss AS opp_pts_2nd_chance, " +
                                $"{tableName}.opp_pts_fb / {joinedTable}.poss AS opp_pts_fb, {tableName}.opp_pts_paint / {joinedTable}.poss AS opp_pts_paint, " +
                                $"{tableName}.gp_rank, {tableName}.w_rank, {tableName}.l_rank, {tableName}.w_pct_rank, {tableName}.min_rank, " +
                                $"pts_off_tov_rank, pts_2nd_chance_rank, pts_fb_rank, pts_paint_rank, opp_pts_off_tov_rank, opp_pts_2nd_chance_rank, opp_pts_fb_rank, opp_pts_paint_rank " +
                                $"FROM {tableName} " +
                                $"INNER JOIN {joinedTable} " +
                                $"ON {tableName}.group_id = {joinedTable}.group_id " +
                                $"WHERE {tableName}.team_id LIKE '%{selectedTeam}%' " +
                                $"ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per Minute")
                    {
                        query = $"SELECT id, group_set, group_id, group_name, " +
                                $"team_id, team_abbreviation, gp, w, l, " +
                                $"w_pct, min, pts_off_tov / min AS pts_off_tov, " +
                                $"pts_2nd_chance / min AS pts_2nd_chance, pts_fb / min AS pts_fb, " +
                                $"pts_paint / min AS pts_paint, opp_pts_off_tov / min AS opp_pts_off_tov, opp_pts_2nd_chance / min AS opp_pts_2nd_chance, " +
                                $"opp_pts_fb / min AS opp_pts_fb, opp_pts_paint / min AS opp_pts_paint, " +
                                $"gp_rank, w_rank, l_rank, w_pct_rank, min_rank, " +
                                $"pts_off_tov_rank, pts_2nd_chance_rank, pts_fb_rank, pts_paint_rank, opp_pts_off_tov_rank, opp_pts_2nd_chance_rank, opp_pts_fb_rank, opp_pts_paint_rank " +
                                $"FROM {tableName} " +
                                $"WHERE team_id LIKE '%{selectedTeam}%' " +
                                $"ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per Game")
                    {
                        query = $"SELECT id, group_set, group_id, group_name, " +
                                $"team_id, team_abbreviation, gp, w, l, " +
                                $"w_pct, min / gp AS min, pts_off_tov / gp AS pts_off_tov, " +
                                $"pts_2nd_chance / gp AS pts_2nd_chance, pts_fb / gp AS pts_fb, " +
                                $"pts_paint / gp AS pts_paint, opp_pts_off_tov / gp AS opp_pts_off_tov, opp_pts_2nd_chance / gp AS opp_pts_2nd_chance, " +
                                $"opp_pts_fb / gp AS opp_pts_fb, opp_pts_paint / gp AS opp_pts_paint, " +
                                $"gp_rank, w_rank, l_rank, w_pct_rank, min_rank, " +
                                $"pts_off_tov_rank, pts_2nd_chance_rank, pts_fb_rank, pts_paint_rank, opp_pts_off_tov_rank, opp_pts_2nd_chance_rank, opp_pts_fb_rank, opp_pts_paint_rank " +
                                $"FROM {tableName} " +
                                $"WHERE team_id LIKE '%{selectedTeam}%' " +
                                $"ORDER BY {sortField} {order}";
                    }
                    Console.WriteLine(boxType);
                    var leagueDashLineups = await _context.LeagueDashLineupMiscs.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(leagueDashLineups.Count);
                    return Ok(leagueDashLineups);
                } else if (boxType == "Scoring")
                {
                    var leagueDashLineups = await _context.LeagueDashLineupScorings.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(leagueDashLineups.Count);
                    return Ok(leagueDashLineups);
                } else if (boxType == "Opponent")
                {
                    var joinedTable = $"league_dash_lineups_advanced_{numPlayers}man_{season}";

                    if (perMode == "Totals")
                    {
                        query = $"SELECT * FROM {tableName} WHERE team_id LIKE '%{selectedTeam}%' ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per 100 Poss")
                    {
                        query = $"SELECT {tableName}.id, {tableName}.group_set, {tableName}.group_id, {tableName}.group_name, " +
                                $"{tableName}.team_id, {tableName}.team_abbreviation, {tableName}.gp, {tableName}.w, {tableName}.l, " +
                                $"{tableName}.w_pct, {tableName}.min / {joinedTable}.poss AS min, {tableName}.opp_fgm / {joinedTable}.poss AS opp_fgm, " +
                                $"{tableName}.opp_fga / {joinedTable}.poss AS opp_fga, {tableName}.opp_fg_pct, {tableName}.opp_fg3m / {joinedTable}.poss AS opp_fg3m, " +
                                $"{tableName}.opp_fg3a / {joinedTable}.poss AS opp_fg3a, {tableName}.opp_fg3_pct, " +
                                $"{tableName}.opp_ftm / {joinedTable}.poss AS opp_ftm, {tableName}.opp_fta / {joinedTable}.poss AS opp_fta, " +
                                $"{tableName}.opp_ft_pct, {tableName}.opp_oreb / {joinedTable}.poss AS opp_oreb, {tableName}.opp_dreb / {joinedTable}.poss AS opp_dreb, " +
                                $"{tableName}.opp_reb / {joinedTable}.poss AS opp_reb, {tableName}.opp_ast / {joinedTable}.poss AS opp_ast, " +
                                $"{tableName}.opp_tov / {joinedTable}.poss AS opp_tov, {tableName}.opp_stl / {joinedTable}.poss AS opp_stl, " +
                                $"{tableName}.opp_blk / {joinedTable}.poss AS opp_blk, {tableName}.opp_blka / {joinedTable}.poss AS opp_blka, " +
                                $"{tableName}.opp_pf / {joinedTable}.poss AS opp_pf, {tableName}.opp_pfd / {joinedTable}.poss AS opp_pfd, " +
                                $"{tableName}.opp_pts / {joinedTable}.poss AS opp_pts, {tableName}.plus_minus / {joinedTable}.poss AS plus_minus, " +
                                $"{tableName}.gp_rank, {tableName}.w_rank, {tableName}.l_rank, {tableName}.w_pct_rank, {tableName}.min_rank, " +
                                $"opp_fgm_rank, opp_fga_rank, opp_fg_pct_rank, opp_fg3m_rank, opp_fg3a_rank, opp_fg3_pct_rank, opp_ftm_rank, opp_fta_rank, " +
                                $"opp_ft_pct_rank, opp_oreb_rank, opp_dreb_rank, opp_reb_rank, opp_ast_rank, opp_tov_rank, opp_stl_rank, opp_blk_rank, opp_blka_rank, " +
                                $"opp_pf_rank, opp_pfd1_rank, opp_pts_rank, plus_minus_rank " +
                                $"FROM {tableName} " +
                                $"INNER JOIN {joinedTable} " +
                                $"ON {tableName}.group_id = {joinedTable}.group_id " +
                                $"WHERE {tableName}.team_id LIKE '%{selectedTeam}%' " +
                                $"ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per Minute")
                    {
                        query = $"SELECT id, group_set, group_id, group_name, " +
                                $"team_id, team_abbreviation, gp, w, l, " +
                                $"w_pct, min, opp_fgm / min AS opp_fgm, " +
                                $"opp_fga / min AS opp_fga, opp_fg_pct, opp_fg3m / min AS opp_fg3m, " +
                                $"opp_fg3a / min AS opp_fg3a, opp_fg3_pct, " +
                                $"opp_ftm / min AS opp_ftm, opp_fta / min AS opp_fta, " +
                                $"opp_ft_pct, opp_oreb / min AS opp_oreb, opp_dreb / min AS opp_dreb, " +
                                $"opp_reb / min AS opp_reb, opp_ast / min AS opp_ast, " +
                                $"opp_tov / min AS opp_tov, opp_stl / min AS opp_stl, " +
                                $"opp_blk / min AS opp_blk, opp_blka / min AS opp_blka, " +
                                $"opp_pf / min AS opp_pf, opp_pfd / min AS opp_pfd, " +
                                $"opp_pts / min AS opp_pts, plus_minus / min AS plus_minus, " +
                                $"gp_rank, w_rank, l_rank, w_pct_rank, min_rank, " +
                                $"opp_fgm_rank, opp_fga_rank, opp_fg_pct_rank, opp_fg3m_rank, opp_fg3a_rank, opp_fg3_pct_rank, opp_ftm_rank, opp_fta_rank, " +
                                $"opp_ft_pct_rank, opp_oreb_rank, opp_dreb_rank, opp_reb_rank, opp_ast_rank, opp_tov_rank, opp_stl_rank, opp_blk_rank, opp_blka_rank, " +
                                $"opp_pf_rank, opp_pfd1_rank, opp_pts_rank, plus_minus_rank " +
                                $"FROM {tableName} " +
                                $"WHERE team_id LIKE '%{selectedTeam}%' " +
                                $"ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per Game")
                    {
                        query = $"SELECT id, group_set, group_id, group_name, " +
                                $"team_id, team_abbreviation, gp, w, l, " +
                                $"w_pct, min, opp_fgm / gp AS opp_fgm, " +
                                $"opp_fga / gp AS opp_fga, opp_fg_pct, opp_fg3m / gp AS opp_fg3m, " +
                                $"opp_fg3a / gp AS opp_fg3a, opp_fg3_pct, " +
                                $"opp_ftm / gp AS opp_ftm, opp_fta / gp AS opp_fta, " +
                                $"opp_ft_pct, opp_oreb / gp AS opp_oreb, opp_dreb / gp AS opp_dreb, " +
                                $"opp_reb / gp AS opp_reb, opp_ast / gp AS opp_ast, " +
                                $"opp_tov / gp AS opp_tov, opp_stl / gp AS opp_stl, " +
                                $"opp_blk / gp AS opp_blk, opp_blka / gp AS opp_blka, " +
                                $"opp_pf / gp AS opp_pf, opp_pfd / gp AS opp_pfd, " +
                                $"opp_pts / gp AS opp_pts, plus_minus / min AS plus_minus, " +
                                $"gp_rank, w_rank, l_rank, w_pct_rank, min_rank, " +
                                $"opp_fgm_rank, opp_fga_rank, opp_fg_pct_rank, opp_fg3m_rank, opp_fg3a_rank, opp_fg3_pct_rank, opp_ftm_rank, opp_fta_rank, " +
                                $"opp_ft_pct_rank, opp_oreb_rank, opp_dreb_rank, opp_reb_rank, opp_ast_rank, opp_tov_rank, opp_stl_rank, opp_blk_rank, opp_blka_rank, " +
                                $"opp_pf_rank, opp_pfd1_rank, opp_pts_rank, plus_minus_rank " +
                                $"FROM {tableName} " +
                                $"WHERE team_id LIKE '%{selectedTeam}%' " +
                                $"ORDER BY {sortField} {order}";
                    }
                    var leagueDashLineups = await _context.LeagueDashLineupOpponents.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(leagueDashLineups.Count);
                    return Ok(leagueDashLineups);
                }
            {
                    return Ok("Not setup yet");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, log, and return an error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        public async Task<IActionResult> CreateLeagueDashLineup([FromBody] object[] leagueDashLineup, string season, string boxType, string numPlayers)
        {
            // Implement logic to create a new league game in the database
            try
            {
                if (leagueDashLineup == null)
                {
                    return BadRequest("Invalid leagueGame data");
                }


                var connectionString = "Server=localhost;Port=5432;Database=hoop_scoop;User Id=postgres;Password=redsox45;\r\n"; // Replace with your actual connection string

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string sql = "";

                    string? groupSetString = leagueDashLineup[0]?.ToString();
                    NpgsqlParameter groupSetParam = new NpgsqlParameter("@group_set", NpgsqlDbType.Text);
                    groupSetParam.Value = groupSetString;

                    string? groupIdString = leagueDashLineup[1]?.ToString();
                    NpgsqlParameter groupIdParam = new NpgsqlParameter("@group_id", NpgsqlDbType.Text);
                    groupIdParam.Value = groupIdString;

                    string? groupNameString = leagueDashLineup[2]?.ToString();
                    NpgsqlParameter groupNameParam = new NpgsqlParameter("@group_name", NpgsqlDbType.Text);
                    groupNameParam.Value = groupNameString;

                    string? teamIdString = leagueDashLineup[3]?.ToString();
                    NpgsqlParameter teamIdParam = new NpgsqlParameter("@team_id", NpgsqlDbType.Text);
                    teamIdParam.Value = teamIdString;

                    string? teamAbbreviationString = leagueDashLineup[4]?.ToString();
                    NpgsqlParameter teamAbbreviationParam = new NpgsqlParameter("@team_abbreviation", NpgsqlDbType.Text);
                    teamAbbreviationParam.Value = teamAbbreviationString;

                    string? gp_string = leagueDashLineup[5]?.ToString();
                    decimal? gp = !string.IsNullOrEmpty(gp_string) ? JsonConvert.DeserializeObject<decimal>(gp_string) : (decimal?)null;
                    string? w_string = leagueDashLineup[6]?.ToString();
                    decimal? w = !string.IsNullOrEmpty(w_string) ? JsonConvert.DeserializeObject<decimal>(w_string) : (decimal?)null;
                    string? l_string = leagueDashLineup[7]?.ToString();
                    decimal? l = !string.IsNullOrEmpty(l_string) ? JsonConvert.DeserializeObject<decimal>(l_string) : (decimal?)null;
                    string? w_pct_string = leagueDashLineup[8]?.ToString();
                    decimal? w_pct = !string.IsNullOrEmpty(w_pct_string) ? JsonConvert.DeserializeObject<decimal>(w_pct_string) : (decimal?)null;
                    string? min_string = leagueDashLineup[9]?.ToString();
                    decimal? min = !string.IsNullOrEmpty(min_string) ? JsonConvert.DeserializeObject<decimal>(min_string) : (decimal?)null;

                    switch (boxType)
                    {

                        case "Base":
                            sql = $"INSERT INTO league_dash_lineups_base_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, fgm, fga, fg_pct, fg3m, fg3a, fg3_pct, ftm, fta, ft_pct, oreb, dreb, reb, ast, stl, blk, blka, pf, pfd, pts, plus_minus, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, fgm_rank, fga_rank, fg_pct_rank, fg3m_rank, fg3a_rank, fg3_pct_rank, ftm_rank, fta_rank, ft_pct_rank, oreb_rank, dreb_rank, reb_rank, ast_rank, tov_rank, stl_rank, blk_rank, blka_rank, pf_rank, pfd_rank, pts_rank, plus_minus_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @fgm, @fga, @fg_pct, @fg3m, @fg3a, @fg3_pct, @ftm, @fta, @ft_pct, @oreb, @dreb, @reb, @ast, @stl, @blk, @blka, @pf, @pfd, @pts, @plus_minus, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @fgm_rank, @fga_rank, @fg_pct_rank, @fg3m_rank, @fg3a_rank, @fg3_pct_rank, @ftm_rank, @fta_rank, @ft_pct_rank, @oreb_rank, @dreb_rank, @reb_rank, @ast_rank, @tov_rank, @stl_rank, @blk_rank, @blka_rank, @pf_rank, @pfd_rank, @pts_rank, @plus_minus_rank);";

                            Console.WriteLine(leagueDashLineup.Length);
                            Console.WriteLine(leagueDashLineup);

                            string? fgm_string = leagueDashLineup[10]?.ToString();
                            decimal? fgm = !string.IsNullOrEmpty(fgm_string) ? JsonConvert.DeserializeObject<decimal>(fgm_string) : (decimal?)null;
                            string? fga_string = leagueDashLineup[11]?.ToString();
                            decimal? fga = !string.IsNullOrEmpty(fga_string) ? JsonConvert.DeserializeObject<decimal>(fga_string) : (decimal?)null;

                            string? fg_pct_string = leagueDashLineup[12]?.ToString();
                            decimal? fg_pct = !string.IsNullOrEmpty(fg_pct_string) ? JsonConvert.DeserializeObject<decimal>(fg_pct_string) : (decimal?)null;

                            string? fg3m_string = leagueDashLineup[13]?.ToString();
                            decimal? fg3m = !string.IsNullOrEmpty(fg3m_string) ? JsonConvert.DeserializeObject<decimal>(fg3m_string) : (decimal?)null;

                            string? fg3a_string = leagueDashLineup[14]?.ToString();
                            decimal? fg3a = !string.IsNullOrEmpty(fg3a_string) ? JsonConvert.DeserializeObject<decimal>(fg3a_string) : (decimal?)null;

                            string? fg3_pct_string = leagueDashLineup[15]?.ToString();
                            decimal? fg3_pct = !string.IsNullOrEmpty(fg3_pct_string) ? JsonConvert.DeserializeObject<decimal>(fg3_pct_string) : (decimal?)null;

                            string? ftm_string = leagueDashLineup[16]?.ToString();
                            decimal? ftm = !string.IsNullOrEmpty(ftm_string) ? JsonConvert.DeserializeObject<decimal>(ftm_string) : (decimal?)null;

                            string? fta_string = leagueDashLineup[17]?.ToString();
                            decimal? fta = !string.IsNullOrEmpty(fta_string) ? JsonConvert.DeserializeObject<decimal>(fta_string) : (decimal?)null;

                            string? ft_pct_string = leagueDashLineup[18]?.ToString();
                            decimal? ft_pct = !string.IsNullOrEmpty(ft_pct_string) ? JsonConvert.DeserializeObject<decimal>(ft_pct_string) : (decimal?)null;

                            string? oreb_string = leagueDashLineup[19]?.ToString();
                            decimal? oreb = !string.IsNullOrEmpty(oreb_string) ? JsonConvert.DeserializeObject<decimal>(oreb_string) : (decimal?)null;

                            string? dreb_string = leagueDashLineup[20]?.ToString();
                            decimal? dreb = !string.IsNullOrEmpty(dreb_string) ? JsonConvert.DeserializeObject<decimal>(dreb_string) : (decimal?)null;

                            string? reb_string = leagueDashLineup[21]?.ToString();
                            decimal? reb = !string.IsNullOrEmpty(reb_string) ? JsonConvert.DeserializeObject<decimal>(reb_string) : (decimal?)null;

                            string? ast_string = leagueDashLineup[22]?.ToString();
                            decimal? ast = !string.IsNullOrEmpty(ast_string) ? JsonConvert.DeserializeObject<decimal>(ast_string) : (decimal?)null;

                            string? stl_string = leagueDashLineup[23]?.ToString();
                            decimal? stl = !string.IsNullOrEmpty(stl_string) ? JsonConvert.DeserializeObject<decimal>(stl_string) : (decimal?)null;

                            string? blk_string = leagueDashLineup[24]?.ToString();
                            decimal? blk = !string.IsNullOrEmpty(blk_string) ? JsonConvert.DeserializeObject<decimal>(blk_string) : (decimal?)null;

                            string? blka_string = leagueDashLineup[25]?.ToString();
                            decimal? blka = !string.IsNullOrEmpty(blka_string) ? JsonConvert.DeserializeObject<decimal>(blka_string) : (decimal?)null;

                            string? pf_string = leagueDashLineup[26]?.ToString();
                            decimal? pf = !string.IsNullOrEmpty(pf_string) ? JsonConvert.DeserializeObject<decimal>(pf_string) : (decimal?)null;

                            string? pfd_string = leagueDashLineup[27]?.ToString();
                            decimal? pfd = !string.IsNullOrEmpty(pfd_string) ? JsonConvert.DeserializeObject<decimal>(pfd_string) : (decimal?)null;

                            string? pts_string = leagueDashLineup[28]?.ToString();
                            decimal? pts = !string.IsNullOrEmpty(pts_string) ? JsonConvert.DeserializeObject<decimal>(pts_string) : (decimal?)null;

                            string? plus_minus_string = leagueDashLineup[29]?.ToString();
                            decimal? plus_minus = !string.IsNullOrEmpty(plus_minus_string) ? JsonConvert.DeserializeObject<decimal>(plus_minus_string) : (decimal?)null;

                            string? gp_rank_string = leagueDashLineup[30]?.ToString();
                            decimal? gp_rank = !string.IsNullOrEmpty(gp_rank_string) ? JsonConvert.DeserializeObject<decimal>(gp_rank_string) : (decimal?)null;

                            string? w_rank_string = leagueDashLineup[31]?.ToString();
                            decimal? w_rank = !string.IsNullOrEmpty(w_rank_string) ? JsonConvert.DeserializeObject<decimal>(w_rank_string) : (decimal?)null;

                            string? l_rank_string = leagueDashLineup[32]?.ToString();
                            decimal? l_rank = !string.IsNullOrEmpty(l_rank_string) ? JsonConvert.DeserializeObject<decimal>(l_rank_string) : (decimal?)null;

                            string? w_pct_rank_string = leagueDashLineup[33]?.ToString();
                            decimal? w_pct_rank = !string.IsNullOrEmpty(w_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(w_pct_rank_string) : (decimal?)null;

                            string? min_rank_string = leagueDashLineup[34]?.ToString();
                            decimal? min_rank = !string.IsNullOrEmpty(min_rank_string) ? JsonConvert.DeserializeObject<decimal>(min_rank_string) : (decimal?)null;

                            string? fgm_rank_string = leagueDashLineup[35]?.ToString();
                            decimal? fgm_rank = !string.IsNullOrEmpty(fgm_rank_string) ? JsonConvert.DeserializeObject<decimal>(fgm_rank_string) : (decimal?)null;

                            string? fga_rank_string = leagueDashLineup[36]?.ToString();
                            decimal? fga_rank = !string.IsNullOrEmpty(fga_rank_string) ? JsonConvert.DeserializeObject<decimal>(fga_rank_string) : (decimal?)null;

                            string? fg_pct_rank_string = leagueDashLineup[37]?.ToString();
                            decimal? fg_pct_rank = !string.IsNullOrEmpty(fg_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(fg_pct_rank_string) : (decimal?)null;

                            string? fg3m_rank_string = leagueDashLineup[38]?.ToString();
                            decimal? fg3m_rank = !string.IsNullOrEmpty(fg3m_rank_string) ? JsonConvert.DeserializeObject<decimal>(fg3m_rank_string) : (decimal?)null;

                            string? fg3a_rank_string = leagueDashLineup[39]?.ToString();
                            decimal? fg3a_rank = !string.IsNullOrEmpty(fg3a_rank_string) ? JsonConvert.DeserializeObject<decimal>(fg3a_rank_string) : (decimal?)null;

                            string? fg3_pct_rank_string = leagueDashLineup[40]?.ToString();
                            decimal? fg3_pct_rank = !string.IsNullOrEmpty(fg3_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(fg3_pct_rank_string) : (decimal?)null;

                            string? ftm_rank_string = leagueDashLineup[41]?.ToString();
                            decimal? ftm_rank = !string.IsNullOrEmpty(ftm_rank_string) ? JsonConvert.DeserializeObject<decimal>(ftm_rank_string) : (decimal?)null;

                            string? fta_rank_string = leagueDashLineup[42]?.ToString();
                            decimal? fta_rank = !string.IsNullOrEmpty(fta_rank_string) ? JsonConvert.DeserializeObject<decimal>(fta_rank_string) : (decimal?)null;

                            string? ft_pct_rank_string = leagueDashLineup[43]?.ToString();
                            decimal? ft_pct_rank = !string.IsNullOrEmpty(ft_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(ft_pct_rank_string) : (decimal?)null;

                            string? oreb_rank_string = leagueDashLineup[44]?.ToString();
                            decimal? oreb_rank = !string.IsNullOrEmpty(oreb_rank_string) ? JsonConvert.DeserializeObject<decimal>(oreb_rank_string) : (decimal?)null;

                            string? dreb_rank_string = leagueDashLineup[45]?.ToString();
                            decimal? dreb_rank = !string.IsNullOrEmpty(dreb_rank_string) ? JsonConvert.DeserializeObject<decimal>(dreb_rank_string) : (decimal?)null;

                            string? reb_rank_string = leagueDashLineup[46]?.ToString();
                            decimal? reb_rank = !string.IsNullOrEmpty(reb_rank_string) ? JsonConvert.DeserializeObject<decimal>(reb_rank_string) : (decimal?)null;

                            string? ast_rank_string = leagueDashLineup[47]?.ToString();
                            decimal? ast_rank = !string.IsNullOrEmpty(ast_rank_string) ? JsonConvert.DeserializeObject<decimal>(ast_rank_string) : (decimal?)null;

                            string? tov_rank_string = leagueDashLineup[48]?.ToString();
                            decimal? tov_rank = !string.IsNullOrEmpty(tov_rank_string) ? JsonConvert.DeserializeObject<decimal>(tov_rank_string) : (decimal?)null;

                            string? stl_rank_string = leagueDashLineup[49]?.ToString();
                            decimal? stl_rank = !string.IsNullOrEmpty(stl_rank_string) ? JsonConvert.DeserializeObject<decimal>(stl_rank_string) : (decimal?)null;

                            string? blk_rank_string = leagueDashLineup[50]?.ToString();
                            decimal? blk_rank = !string.IsNullOrEmpty(blk_rank_string) ? JsonConvert.DeserializeObject<decimal>(blk_rank_string) : (decimal?)null;

                            string? blka_rank_string = leagueDashLineup[51]?.ToString();
                            decimal? blka_rank = !string.IsNullOrEmpty(blka_rank_string) ? JsonConvert.DeserializeObject<decimal>(blka_rank_string) : (decimal?)null;

                            string? pf_rank_string = leagueDashLineup[52]?.ToString();
                            decimal? pf_rank = !string.IsNullOrEmpty(pf_rank_string) ? JsonConvert.DeserializeObject<decimal>(pf_rank_string) : (decimal?)null;

                            string? pfd_rank_string = leagueDashLineup[53]?.ToString();
                            decimal? pfd_rank = !string.IsNullOrEmpty(pfd_rank_string) ? JsonConvert.DeserializeObject<decimal>(pfd_rank_string) : (decimal?)null;

                            string? pts_rank_string = leagueDashLineup[54]?.ToString();
                            decimal? pts_rank = !string.IsNullOrEmpty(pts_rank_string) ? JsonConvert.DeserializeObject<decimal>(pts_rank_string) : (decimal?)null;

                            string? plus_minus_rank_string = leagueDashLineup[55]?.ToString();
                            decimal? plus_minus_rank = !string.IsNullOrEmpty(plus_minus_rank_string) ? JsonConvert.DeserializeObject<decimal>(plus_minus_rank_string) : (decimal?)null;

                            using (var cmd = new NpgsqlCommand(sql, connection))
                            {
                                cmd.Parameters.Add(groupSetParam);
                                cmd.Parameters.Add(groupIdParam);
                                cmd.Parameters.Add(groupNameParam);
                                cmd.Parameters.Add(teamIdParam);
                                cmd.Parameters.Add(teamAbbreviationParam);
                                cmd.Parameters.AddWithValue("@gp", gp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w", w ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l", l ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct", w_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min", min ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fgm", fgm ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fga", fga ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fg_pct", fg_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fg3m", fg3m ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fg3a", fg3a ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fg3_pct", fg3_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ftm", ftm ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fta", fta ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ft_pct", ft_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@oreb", oreb ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@dreb", dreb ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@reb", reb ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ast", ast ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@stl", stl ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@blk", blk ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@blka", blka ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pf", pf ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pfd", pfd ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pts", pts ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@plus_minus", plus_minus ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@gp_rank", gp_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_rank", w_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l_rank", l_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct_rank", w_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min_rank", min_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fgm_rank", fgm_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fga_rank", fga_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fg_pct_rank", fg_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fg3m_rank", fg3m_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fg3a_rank", fg3a_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fg3_pct_rank", fg3_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ftm_rank", ftm_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fta_rank", fta_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ft_pct_rank", ft_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@oreb_rank", oreb_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@dreb_rank", dreb_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@reb_rank", reb_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ast_rank", ast_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@tov_rank", tov_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@stl_rank", stl_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@blk_rank", blk_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@blka_rank", blka_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pf_rank", pf_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pfd_rank", pfd_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pts_rank", pts_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@plus_minus_rank", plus_minus_rank ?? (object)DBNull.Value);
                                await cmd.ExecuteNonQueryAsync();
                            }

                            break;
                        
                        case "Advanced":
                            sql = $"INSERT INTO league_dash_lineups_advanced_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, e_off_rating, off_rating, e_def_rating, def_rating, e_net_rating, net_rating, ast_pct, ast_to, ast_ratio, oreb_pct, dreb_pct, reb_pct, tm_tov_pct, efg_pct, ts_pct, e_pace, pace, pace_per40, poss, pie, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, off_rating_rank, def_rating_rank, net_rating_rank, ast_pct_rank, ast_to_rank, ast_ratio_rank, oreb_pct_rank, dreb_pct_rank, reb_pct_rank, tm_tov_pct_rank, efg_pct_rank, ts_pct_rank, pace_rank, pie_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @e_off_rating, @off_rating, @e_def_rating, @def_rating, @e_net_rating, @net_rating, @ast_pct, @ast_to, @ast_ratio, @oreb_pct, @dreb_pct, @reb_pct, @tm_tov_pct, @efg_pct, @ts_pct, @e_pace, @pace, @pace_per40, @poss, @pie, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @off_rating_rank, @def_rating_rank, @net_rating_rank, @ast_pct_rank, @ast_to_rank, @ast_ratio_rank, @oreb_pct_rank, @dreb_pct_rank, @reb_pct_rank, @tm_tov_pct_rank, @efg_pct_rank, @ts_pct_rank, @pace_rank, @pie_rank);";

                            Console.WriteLine(leagueDashLineup.Length);
                            Console.WriteLine(leagueDashLineup);

                            string? e_off_rating_string = leagueDashLineup[10]?.ToString();
                            decimal? e_off_rating = !string.IsNullOrEmpty(e_off_rating_string) ? JsonConvert.DeserializeObject<decimal>(e_off_rating_string) : (decimal?)null;

                            string? off_rating_string = leagueDashLineup[11]?.ToString();
                            decimal? off_rating = !string.IsNullOrEmpty(off_rating_string) ? JsonConvert.DeserializeObject<decimal>(off_rating_string) : (decimal?)null;

                            string? e_def_rating_string = leagueDashLineup[12]?.ToString();
                            decimal? e_def_rating = !string.IsNullOrEmpty(e_def_rating_string) ? JsonConvert.DeserializeObject<decimal>(e_def_rating_string) : (decimal?)null;

                            string? def_rating_string = leagueDashLineup[13]?.ToString();
                            decimal? def_rating = !string.IsNullOrEmpty(def_rating_string) ? JsonConvert.DeserializeObject<decimal>(def_rating_string) : (decimal?)null;

                            string? e_net_rating_string = leagueDashLineup[14]?.ToString();
                            decimal? e_net_rating = !string.IsNullOrEmpty(e_net_rating_string) ? JsonConvert.DeserializeObject<decimal>(e_net_rating_string) : (decimal?)null;

                            string? net_rating_string = leagueDashLineup[15]?.ToString();
                            decimal? net_rating = !string.IsNullOrEmpty(net_rating_string) ? JsonConvert.DeserializeObject<decimal>(net_rating_string) : (decimal?)null;

                            string? ast_pct_string = leagueDashLineup[16]?.ToString();
                            decimal? ast_pct = !string.IsNullOrEmpty(ast_pct_string) ? JsonConvert.DeserializeObject<decimal>(ast_pct_string) : (decimal?)null;

                            string? ast_to_string = leagueDashLineup[17]?.ToString();
                            decimal? ast_to = !string.IsNullOrEmpty(ast_to_string) ? JsonConvert.DeserializeObject<decimal>(ast_to_string) : (decimal?)null;

                            string? ast_ratio_string = leagueDashLineup[18]?.ToString();
                            decimal? ast_ratio = !string.IsNullOrEmpty(ast_ratio_string) ? JsonConvert.DeserializeObject<decimal>(ast_ratio_string) : (decimal?)null;

                            string? oreb_pct_string = leagueDashLineup[19]?.ToString();
                            decimal? oreb_pct = !string.IsNullOrEmpty(oreb_pct_string) ? JsonConvert.DeserializeObject<decimal>(oreb_pct_string) : (decimal?)null;

                            string? dreb_pct_string = leagueDashLineup[20]?.ToString();
                            decimal? dreb_pct = !string.IsNullOrEmpty(dreb_pct_string) ? JsonConvert.DeserializeObject<decimal>(dreb_pct_string) : (decimal?)null;

                            string? reb_pct_string = leagueDashLineup[21]?.ToString();
                            decimal? reb_pct = !string.IsNullOrEmpty(reb_pct_string) ? JsonConvert.DeserializeObject<decimal>(reb_pct_string) : (decimal?)null;

                            string? tm_tov_pct_string = leagueDashLineup[22]?.ToString();
                            decimal? tm_tov_pct = !string.IsNullOrEmpty(tm_tov_pct_string) ? JsonConvert.DeserializeObject<decimal>(tm_tov_pct_string) : (decimal?)null;

                            string? efg_pct_string = leagueDashLineup[23]?.ToString();
                            decimal? efg_pct = !string.IsNullOrEmpty(efg_pct_string) ? JsonConvert.DeserializeObject<decimal>(efg_pct_string) : (decimal?)null;

                            string? ts_pct_string = leagueDashLineup[24]?.ToString();
                            decimal? ts_pct = !string.IsNullOrEmpty(ts_pct_string) ? JsonConvert.DeserializeObject<decimal>(ts_pct_string) : (decimal?)null;

                            string? e_pace_string = leagueDashLineup[25]?.ToString();
                            decimal? e_pace = !string.IsNullOrEmpty(e_pace_string) ? JsonConvert.DeserializeObject<decimal>(e_pace_string) : (decimal?)null;

                            string? pace_string = leagueDashLineup[26]?.ToString();
                            decimal? pace = !string.IsNullOrEmpty(pace_string) ? JsonConvert.DeserializeObject<decimal>(pace_string) : (decimal?)null;

                            string? pace_per40_string = leagueDashLineup[27]?.ToString();
                            decimal? pace_per40 = !string.IsNullOrEmpty(pace_per40_string) ? JsonConvert.DeserializeObject<decimal>(pace_per40_string) : (decimal?)null;

                            string? poss_string = leagueDashLineup[28]?.ToString();
                            decimal? poss = !string.IsNullOrEmpty(poss_string) ? JsonConvert.DeserializeObject<decimal>(poss_string) : (decimal?)null;

                            string? pie_string = leagueDashLineup[29]?.ToString();
                            decimal? pie = !string.IsNullOrEmpty(pie_string) ? JsonConvert.DeserializeObject<decimal>(pie_string) : (decimal?)null;

                            string? gp_rank_string_advanced = leagueDashLineup[30]?.ToString();
                            decimal? gp_rank_advanced = !string.IsNullOrEmpty(gp_rank_string_advanced) ? JsonConvert.DeserializeObject<decimal>(gp_rank_string_advanced) : (decimal?)null;

                            string? w_rank_string_advanced = leagueDashLineup[31]?.ToString();
                            decimal? w_rank_advanced = !string.IsNullOrEmpty(w_rank_string_advanced) ? JsonConvert.DeserializeObject<decimal>(w_rank_string_advanced) : (decimal?)null;

                            string? l_rank_string_advanced = leagueDashLineup[32]?.ToString();
                            decimal? l_rank_advanced = !string.IsNullOrEmpty(l_rank_string_advanced) ? JsonConvert.DeserializeObject<decimal>(l_rank_string_advanced) : (decimal?)null;

                            string? w_pct_rank_string_advanced = leagueDashLineup[33]?.ToString();
                            decimal? w_pct_rank_advanced = !string.IsNullOrEmpty(w_pct_rank_string_advanced) ? JsonConvert.DeserializeObject<decimal>(w_pct_rank_string_advanced) : (decimal?)null;

                            string? min_rank_string_advanced = leagueDashLineup[34]?.ToString();
                            decimal? min_rank_advanced = !string.IsNullOrEmpty(min_rank_string_advanced) ? JsonConvert.DeserializeObject<decimal>(min_rank_string_advanced) : (decimal?)null;

                            string? off_rating_rank_string = leagueDashLineup[35]?.ToString();
                            decimal? off_rating_rank = !string.IsNullOrEmpty(off_rating_rank_string) ? JsonConvert.DeserializeObject<decimal>(off_rating_rank_string) : (decimal?)null;

                            string? def_rating_rank_string = leagueDashLineup[36]?.ToString();
                            decimal? def_rating_rank = !string.IsNullOrEmpty(def_rating_rank_string) ? JsonConvert.DeserializeObject<decimal>(def_rating_rank_string) : (decimal?)null;

                            string? net_rating_rank_string = leagueDashLineup[37]?.ToString();
                            decimal? net_rating_rank = !string.IsNullOrEmpty(net_rating_rank_string) ? JsonConvert.DeserializeObject<decimal>(net_rating_rank_string) : (decimal?)null;

                            string? ast_pct_rank_string = leagueDashLineup[38]?.ToString();
                            decimal? ast_pct_rank = !string.IsNullOrEmpty(ast_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(ast_pct_rank_string) : (decimal?)null;

                            string? ast_to_rank_string = leagueDashLineup[39]?.ToString();
                            decimal? ast_to_rank = !string.IsNullOrEmpty(ast_to_rank_string) ? JsonConvert.DeserializeObject<decimal>(ast_to_rank_string) : (decimal?)null;

                            string? ast_ratio_rank_string = leagueDashLineup[40]?.ToString();
                            decimal? ast_ratio_rank = !string.IsNullOrEmpty(ast_ratio_rank_string) ? JsonConvert.DeserializeObject<decimal>(ast_ratio_rank_string) : (decimal?)null;

                            string? oreb_pct_rank_string = leagueDashLineup[41]?.ToString();
                            decimal? oreb_pct_rank = !string.IsNullOrEmpty(oreb_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(oreb_pct_rank_string) : (decimal?)null;

                            string? dreb_pct_rank_string = leagueDashLineup[42]?.ToString();
                            decimal? dreb_pct_rank = !string.IsNullOrEmpty(dreb_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(dreb_pct_rank_string) : (decimal?)null;

                            string? reb_pct_rank_string = leagueDashLineup[43]?.ToString();
                            decimal? reb_pct_rank = !string.IsNullOrEmpty(reb_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(reb_pct_rank_string) : (decimal?)null;

                            string? tm_tov_pct_rank_string = leagueDashLineup[44]?.ToString();
                            decimal? tm_tov_pct_rank = !string.IsNullOrEmpty(tm_tov_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(tm_tov_pct_rank_string) : (decimal?)null;

                            string? efg_pct_rank_string = leagueDashLineup[45]?.ToString();
                            decimal? efg_pct_rank = !string.IsNullOrEmpty(efg_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(efg_pct_rank_string) : (decimal?)null;

                            string? ts_pct_rank_string = leagueDashLineup[46]?.ToString();
                            decimal? ts_pct_rank = !string.IsNullOrEmpty(ts_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(ts_pct_rank_string) : (decimal?)null;

                            string? pace_rank_string = leagueDashLineup[47]?.ToString();
                            decimal? pace_rank = !string.IsNullOrEmpty(pace_rank_string) ? JsonConvert.DeserializeObject<decimal>(pace_rank_string) : (decimal?)null;

                            string? pie_rank_string = leagueDashLineup[48]?.ToString();
                            decimal? pie_rank = !string.IsNullOrEmpty(pie_rank_string) ? JsonConvert.DeserializeObject<decimal>(pie_rank_string) : (decimal?)null;


                            using (var cmd = new NpgsqlCommand(sql, connection))
                            {
                                cmd.Parameters.Add(groupSetParam);
                                cmd.Parameters.Add(groupIdParam);
                                cmd.Parameters.Add(groupNameParam);
                                cmd.Parameters.Add(teamIdParam);
                                cmd.Parameters.Add(teamAbbreviationParam);
                                cmd.Parameters.AddWithValue("@gp", gp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w", w ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l", l ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct", w_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min", min ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@e_off_rating", e_off_rating ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@off_rating", off_rating ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@e_def_rating", e_def_rating ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@def_rating", def_rating ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@e_net_rating", e_net_rating ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@net_rating", net_rating ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ast_pct", ast_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ast_to", ast_to ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ast_ratio", ast_ratio ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@oreb_pct", oreb_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@dreb_pct", dreb_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@reb_pct", reb_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@tm_tov_pct", tm_tov_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@efg_pct", efg_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ts_pct", ts_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@e_pace", e_pace ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pace", pace ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pace_per40", pace_per40 ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@poss", poss ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pie", pie ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@gp_rank", gp_rank_advanced ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_rank", w_rank_advanced ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l_rank", l_rank_advanced ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct_rank", w_pct_rank_advanced ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min_rank", min_rank_advanced ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@off_rating_rank", off_rating_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@def_rating_rank", def_rating_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@net_rating_rank", net_rating_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ast_pct_rank", ast_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ast_to_rank", ast_to_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ast_ratio_rank", ast_ratio_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@oreb_pct_rank", oreb_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@dreb_pct_rank", dreb_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@reb_pct_rank", reb_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@tm_tov_pct_rank", tm_tov_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@efg_pct_rank", efg_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@ts_pct_rank", ts_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pace_rank", pace_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pie_rank", pie_rank ?? (object)DBNull.Value);

                                await cmd.ExecuteNonQueryAsync();
                            }
                            break;

                        case "FourFactors":
                            sql = $"INSERT INTO league_dash_lineups_fourfactors_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, efg_pct, fta_rate, tm_tov_pct, oreb_pct, opp_efg_pct, opp_fta_rate, opp_tov_pct, opp_oreb_pct, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, efg_pct_rank, fta_rate_rank, tm_tov_pct_rank, oreb_pct_rank, opp_efg_pct_rank, opp_fta_rate_rank, opp_tov_pct_rank, opp_oreb_pct_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @efg_pct, @fta_rate, @tm_tov_pct, @oreb_pct, @opp_efg_pct, @opp_fta_rate, @opp_tov_pct, @opp_oreb_pct, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @efg_pct_rank, @fta_rate_rank, @tm_tov_pct_rank, @oreb_pct_rank, @opp_efg_pct_rank, @opp_fta_rate_rank, @opp_tov_pct_rank, @opp_oreb_pct_rank);";

                            string? efg_pct_string_ff = leagueDashLineup[10]?.ToString();
                            decimal? efg_pct_ff = !string.IsNullOrEmpty(efg_pct_string_ff) ? JsonConvert.DeserializeObject<decimal>(efg_pct_string_ff) : (decimal?)null;

                            string? fta_rate_string = leagueDashLineup[11]?.ToString();
                            decimal? fta_rate = !string.IsNullOrEmpty(fta_rate_string) ? JsonConvert.DeserializeObject<decimal>(fta_rate_string) : (decimal?)null;

                            string? tm_tov_pct_string_ff = leagueDashLineup[12]?.ToString();
                            decimal? tm_tov_pct_ff = !string.IsNullOrEmpty(tm_tov_pct_string_ff) ? JsonConvert.DeserializeObject<decimal>(tm_tov_pct_string_ff) : (decimal?)null;

                            string? oreb_pct_string_ff = leagueDashLineup[13]?.ToString();
                            decimal? oreb_pct_ff = !string.IsNullOrEmpty(oreb_pct_string_ff) ? JsonConvert.DeserializeObject<decimal>(oreb_pct_string_ff) : (decimal?)null;

                            string? opp_efg_pct_string = leagueDashLineup[14]?.ToString();
                            decimal? opp_efg_pct = !string.IsNullOrEmpty(opp_efg_pct_string) ? JsonConvert.DeserializeObject<decimal>(opp_efg_pct_string) : (decimal?)null;

                            string? opp_fta_rate_string = leagueDashLineup[15]?.ToString();
                            decimal? opp_fta_rate = !string.IsNullOrEmpty(opp_fta_rate_string) ? JsonConvert.DeserializeObject<decimal>(opp_fta_rate_string) : (decimal?)null;

                            string? opp_tov_pct_string = leagueDashLineup[16]?.ToString();
                            decimal? opp_tov_pct = !string.IsNullOrEmpty(opp_tov_pct_string) ? JsonConvert.DeserializeObject<decimal>(opp_tov_pct_string) : (decimal?)null;

                            string? opp_oreb_pct_string = leagueDashLineup[17]?.ToString();
                            decimal? opp_oreb_pct = !string.IsNullOrEmpty(opp_oreb_pct_string) ? JsonConvert.DeserializeObject<decimal>(opp_oreb_pct_string) : (decimal?)null;

                            string? gp_rank_string_ff = leagueDashLineup[18]?.ToString();
                            decimal? gp_rank_ff = !string.IsNullOrEmpty(gp_rank_string_ff) ? JsonConvert.DeserializeObject<decimal>(gp_rank_string_ff) : (decimal?)null;

                            string? w_rank_string_ff = leagueDashLineup[19]?.ToString();
                            decimal? w_rank_ff = !string.IsNullOrEmpty(w_rank_string_ff) ? JsonConvert.DeserializeObject<decimal>(w_rank_string_ff) : (decimal?)null;

                            string? l_rank_string_ff = leagueDashLineup[20]?.ToString();
                            decimal? l_rank_ff = !string.IsNullOrEmpty(l_rank_string_ff) ? JsonConvert.DeserializeObject<decimal>(l_rank_string_ff) : (decimal?)null;

                            string? w_pct_rank_string_ff = leagueDashLineup[21]?.ToString();
                            decimal? w_pct_rank_ff = !string.IsNullOrEmpty(w_pct_rank_string_ff) ? JsonConvert.DeserializeObject<decimal>(w_pct_rank_string_ff) : (decimal?)null;

                            string? min_rank_string_ff = leagueDashLineup[22]?.ToString();
                            decimal? min_rank_ff = !string.IsNullOrEmpty(min_rank_string_ff) ? JsonConvert.DeserializeObject<decimal>(min_rank_string_ff) : (decimal?)null;

                            string? efg_pct_rank_string_ff = leagueDashLineup[23]?.ToString();
                            decimal? efg_pct_rank_ff = !string.IsNullOrEmpty(efg_pct_rank_string_ff) ? JsonConvert.DeserializeObject<decimal>(efg_pct_rank_string_ff) : (decimal?)null;

                            string? fta_rate_rank_string = leagueDashLineup[24]?.ToString();
                            decimal? fta_rate_rank = !string.IsNullOrEmpty(fta_rate_rank_string) ? JsonConvert.DeserializeObject<decimal>(fta_rate_rank_string) : (decimal?)null;

                            string? tm_tov_pct_rank_string_ff = leagueDashLineup[25]?.ToString();
                            decimal? tm_tov_pct_rank_ff = !string.IsNullOrEmpty(tm_tov_pct_rank_string_ff) ? JsonConvert.DeserializeObject<decimal>(tm_tov_pct_rank_string_ff) : (decimal?)null;

                            string? oreb_pct_rank_string_ff = leagueDashLineup[26]?.ToString();
                            decimal? oreb_pct_rank_ff = !string.IsNullOrEmpty(oreb_pct_rank_string_ff) ? JsonConvert.DeserializeObject<decimal>(oreb_pct_rank_string_ff) : (decimal?)null;

                            string? opp_efg_pct_rank_string = leagueDashLineup[27]?.ToString();
                            decimal? opp_efg_pct_rank = !string.IsNullOrEmpty(opp_efg_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_efg_pct_rank_string) : (decimal?)null;

                            string? opp_fta_rate_rank_string = leagueDashLineup[28]?.ToString();
                            decimal? opp_fta_rate_rank = !string.IsNullOrEmpty(opp_fta_rate_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_fta_rate_rank_string) : (decimal?)null;

                            string? opp_tov_pct_rank_string = leagueDashLineup[29]?.ToString();
                            decimal? opp_tov_pct_rank = !string.IsNullOrEmpty(opp_tov_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_tov_pct_rank_string) : (decimal?)null;

                            string? opp_oreb_pct_rank_string = leagueDashLineup[30]?.ToString();
                            decimal? opp_oreb_pct_rank = !string.IsNullOrEmpty(opp_oreb_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_oreb_pct_rank_string) : (decimal?)null;


                            using (var cmd = new NpgsqlCommand(sql, connection))
                            {
                                cmd.Parameters.Add(groupSetParam);
                                cmd.Parameters.Add(groupIdParam);
                                cmd.Parameters.Add(groupNameParam);
                                cmd.Parameters.Add(teamIdParam);
                                cmd.Parameters.Add(teamAbbreviationParam);
                                cmd.Parameters.AddWithValue("@gp", gp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w", w ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l", l ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct", w_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min", min ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@efg_pct", efg_pct_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fta_rate", fta_rate ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@tm_tov_pct", tm_tov_pct_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@oreb_pct", oreb_pct_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_efg_pct", opp_efg_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fta_rate", opp_fta_rate ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_tov_pct", opp_tov_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_oreb_pct", opp_oreb_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@gp_rank", gp_rank_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_rank", w_rank_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l_rank", l_rank_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct_rank", w_pct_rank_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min_rank", min_rank_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@efg_pct_rank", efg_pct_rank_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@fta_rate_rank", fta_rate_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@tm_tov_pct_rank", tm_tov_pct_rank_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@oreb_pct_rank", oreb_pct_rank_ff ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_efg_pct_rank", opp_efg_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fta_rate_rank", opp_fta_rate_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_tov_pct_rank", opp_tov_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_oreb_pct_rank", opp_oreb_pct_rank ?? (object)DBNull.Value);
                                await cmd.ExecuteNonQueryAsync();
                            }

                        break;
                        case "Misc":
                            sql = $"INSERT INTO league_dash_lineups_Misc_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, pts_off_tov, pts_2nd_chance, pts_fb, pts_paint, opp_pts_off_tov, opp_pts_2nd_chance, opp_pts_fb, opp_pts_paint, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, pts_off_tov_rank, pts_2nd_chance_rank, pts_fb_rank, pts_paint_rank, opp_pts_off_tov_rank, opp_pts_2nd_chance_rank, opp_pts_fb_rank, opp_pts_paint_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @pts_off_tov, @pts_2nd_chance, @pts_fb, @pts_paint, @opp_pts_off_tov, @opp_pts_2nd_chance, @opp_pts_fb, @opp_pts_paint, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @pts_off_tov_rank, @pts_2nd_chance_rank, @pts_fb_rank, @pts_paint_rank, @opp_pts_off_tov_rank, @opp_pts_2nd_chance_rank, @opp_pts_fb_rank, @opp_pts_paint_rank);";

                            string? pts_off_tov_string = leagueDashLineup[10]?.ToString();
                            decimal? pts_off_tov = !string.IsNullOrEmpty(pts_off_tov_string) ? JsonConvert.DeserializeObject<decimal>(pts_off_tov_string) : (decimal?)null;

                            string? pts_2nd_chance_string = leagueDashLineup[11]?.ToString();
                            decimal? pts_2nd_chance = !string.IsNullOrEmpty(pts_2nd_chance_string) ? JsonConvert.DeserializeObject<decimal>(pts_2nd_chance_string) : (decimal?)null;

                            string? pts_fb_string = leagueDashLineup[12]?.ToString();
                            decimal? pts_fb = !string.IsNullOrEmpty(pts_fb_string) ? JsonConvert.DeserializeObject<decimal>(pts_fb_string) : (decimal?)null;

                            string? pts_paint_string = leagueDashLineup[13]?.ToString();
                            decimal? pts_paint = !string.IsNullOrEmpty(pts_paint_string) ? JsonConvert.DeserializeObject<decimal>(pts_paint_string) : (decimal?)null;

                            string? opp_pts_off_tov_string = leagueDashLineup[14]?.ToString();
                            decimal? opp_pts_off_tov = !string.IsNullOrEmpty(opp_pts_off_tov_string) ? JsonConvert.DeserializeObject<decimal>(opp_pts_off_tov_string) : (decimal?)null;

                            string? opp_pts_2nd_chance_string = leagueDashLineup[15]?.ToString();
                            decimal? opp_pts_2nd_chance = !string.IsNullOrEmpty(opp_pts_2nd_chance_string) ? JsonConvert.DeserializeObject<decimal>(opp_pts_2nd_chance_string) : (decimal?)null;

                            string? opp_pts_fb_string = leagueDashLineup[16]?.ToString();
                            decimal? opp_pts_fb = !string.IsNullOrEmpty(opp_pts_fb_string) ? JsonConvert.DeserializeObject<decimal>(opp_pts_fb_string) : (decimal?)null;

                            string? opp_pts_paint_string = leagueDashLineup[17]?.ToString();
                            decimal? opp_pts_paint = !string.IsNullOrEmpty(opp_pts_paint_string) ? JsonConvert.DeserializeObject<decimal>(opp_pts_paint_string) : (decimal?)null;

                            string? gp_rank_string_misc = leagueDashLineup[18]?.ToString();
                            decimal? gp_rank_misc = !string.IsNullOrEmpty(gp_rank_string_misc) ? JsonConvert.DeserializeObject<decimal>(gp_rank_string_misc) : (decimal?)null;

                            string? w_rank_string_misc = leagueDashLineup[19]?.ToString();
                            decimal? w_rank_misc = !string.IsNullOrEmpty(w_rank_string_misc) ? JsonConvert.DeserializeObject<decimal>(w_rank_string_misc) : (decimal?)null;

                            string? l_rank_string_misc = leagueDashLineup[20]?.ToString();
                            decimal? l_rank_misc = !string.IsNullOrEmpty(l_rank_string_misc) ? JsonConvert.DeserializeObject<decimal>(l_rank_string_misc) : (decimal?)null;

                            string? w_pct_rank_string_misc = leagueDashLineup[21]?.ToString();
                            decimal? w_pct_rank_misc = !string.IsNullOrEmpty(w_pct_rank_string_misc) ? JsonConvert.DeserializeObject<decimal>(w_pct_rank_string_misc) : (decimal?)null;

                            string? min_rank_string_misc = leagueDashLineup[22]?.ToString();
                            decimal? min_rank_misc = !string.IsNullOrEmpty(min_rank_string_misc) ? JsonConvert.DeserializeObject<decimal>(min_rank_string_misc) : (decimal?)null;

                            string? pts_off_tov_rank_string = leagueDashLineup[23]?.ToString();
                            decimal? pts_off_tov_rank = !string.IsNullOrEmpty(pts_off_tov_rank_string) ? JsonConvert.DeserializeObject<decimal>(pts_off_tov_rank_string) : (decimal?)null;

                            string? pts_2nd_chance_rank_string = leagueDashLineup[24]?.ToString();
                            decimal? pts_2nd_chance_rank = !string.IsNullOrEmpty(pts_2nd_chance_rank_string) ? JsonConvert.DeserializeObject<decimal>(pts_2nd_chance_rank_string) : (decimal?)null;

                            string? pts_fb_rank_string = leagueDashLineup[25]?.ToString();
                            decimal? pts_fb_rank = !string.IsNullOrEmpty(pts_fb_rank_string) ? JsonConvert.DeserializeObject<decimal>(pts_fb_rank_string) : (decimal?)null;

                            string? pts_paint_rank_string = leagueDashLineup[26]?.ToString();
                            decimal? pts_paint_rank = !string.IsNullOrEmpty(pts_paint_rank_string) ? JsonConvert.DeserializeObject<decimal>(pts_paint_rank_string) : (decimal?)null;

                            string? opp_pts_off_tov_rank_string = leagueDashLineup[27]?.ToString();
                            decimal? opp_pts_off_tov_rank = !string.IsNullOrEmpty(opp_pts_off_tov_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_pts_off_tov_rank_string) : (decimal?)null;

                            string? opp_pts_2nd_chance_rank_string = leagueDashLineup[28]?.ToString();
                            decimal? opp_pts_2nd_chance_rank = !string.IsNullOrEmpty(opp_pts_2nd_chance_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_pts_2nd_chance_rank_string) : (decimal?)null;

                            string? opp_pts_fb_rank_string = leagueDashLineup[29]?.ToString();
                            decimal? opp_pts_fb_rank = !string.IsNullOrEmpty(opp_pts_fb_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_pts_fb_rank_string) : (decimal?)null;

                            string? opp_pts_paint_rank_string = leagueDashLineup[30]?.ToString();
                            decimal? opp_pts_paint_rank = !string.IsNullOrEmpty(opp_pts_paint_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_pts_paint_rank_string) : (decimal?)null;

                            using (var cmd = new NpgsqlCommand(sql, connection))
                            {
                                cmd.Parameters.Add(groupSetParam);
                                cmd.Parameters.Add(groupIdParam);
                                cmd.Parameters.Add(groupNameParam);
                                cmd.Parameters.Add(teamIdParam);
                                cmd.Parameters.Add(teamAbbreviationParam);
                                cmd.Parameters.AddWithValue("@gp", gp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w", w ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l", l ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct", w_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min", min ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pts_off_tov", pts_off_tov ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pts_2nd_chance", pts_2nd_chance ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pts_fb", pts_fb ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pts_paint", pts_paint ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pts_off_tov", opp_pts_off_tov ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pts_2nd_chance", opp_pts_2nd_chance ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pts_fb", opp_pts_fb ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pts_paint", opp_pts_paint ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@gp_rank", gp_rank_misc ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_rank", w_rank_misc ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l_rank", l_rank_misc ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct_rank", w_pct_rank_misc ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min_rank", min_rank_misc ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pts_off_tov_rank", pts_off_tov_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pts_2nd_chance_rank", pts_2nd_chance_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pts_fb_rank", pts_fb_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pts_paint_rank", pts_paint_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pts_off_tov_rank", opp_pts_off_tov_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pts_2nd_chance_rank", opp_pts_2nd_chance_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pts_fb_rank", opp_pts_fb_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pts_paint_rank", opp_pts_paint_rank ?? (object)DBNull.Value);


                                await cmd.ExecuteNonQueryAsync();
                            }

                        break;
                        case "Scoring":
                            sql = $"INSERT INTO league_dash_lineups_Scoring_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, pct_fga_2pt, pct_fga_3pt, pct_pts_2pt, pct_pts_2pt_mr, pct_pts_3pt, pct_pts_fb, pct_pts_ft, pct_pts_off_tov, pct_pts_paint, pct_ast_2pm, pct_uast_2pm, pct_ast_3pm, pct_uast_3pm, pct_ast_fgm, pct_uast_fgm, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, pct_fga_2pt_rank, pct_fga_3pt_rank, pct_pts_2pt_rank, pct_pts_2pt_mr_rank, pct_pts_3pt_rank, pct_pts_fb_rank, pct_pts_ft_rank, pct_pts_off_tov_rank, pct_pts_paint_rank, pct_ast_2pm_rank, pct_uast_2pm_rank, pct_ast_3pm_rank, pct_uast_3pm_rank, pct_ast_fgm_rank, pct_uast_fgm_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @pct_fga_2pt, @pct_fga_3pt, @pct_pts_2pt, @pct_pts_2pt_mr, @pct_pts_3pt, @pct_pts_fb, @pct_pts_ft, @pct_pts_off_tov, @pct_pts_paint, @pct_ast_2pm, @pct_uast_2pm, @pct_ast_3pm, @pct_uast_3pm, @pct_ast_fgm, @pct_uast_fgm, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @pct_fga_2pt_rank, @pct_fga_3pt_rank, @pct_pts_2pt_rank, @pct_pts_2pt_mr_rank, @pct_pts_3pt_rank, @pct_pts_fb_rank, @pct_pts_ft_rank, @pct_pts_off_tov_rank, @pct_pts_paint_rank, @pct_ast_2pm_rank, @pct_uast_2pm_rank, @pct_ast_3pm_rank, @pct_uast_3pm_rank, @pct_ast_fgm_rank, @pct_uast_fgm_rank);";

                            string? pct_fga_2pt_string = leagueDashLineup[10]?.ToString();
                            decimal? pct_fga_2pt = !string.IsNullOrEmpty(pct_fga_2pt_string) ? JsonConvert.DeserializeObject<decimal>(pct_fga_2pt_string) : (decimal?)null;

                            string? pct_fga_3pt_string = leagueDashLineup[11]?.ToString();
                            decimal? pct_fga_3pt = !string.IsNullOrEmpty(pct_fga_3pt_string) ? JsonConvert.DeserializeObject<decimal>(pct_fga_3pt_string) : (decimal?)null;

                            string? pct_pts_2pt_string = leagueDashLineup[12]?.ToString();
                            decimal? pct_pts_2pt = !string.IsNullOrEmpty(pct_pts_2pt_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_2pt_string) : (decimal?)null;

                            string? pct_pts_2pt_mr_string = leagueDashLineup[13]?.ToString();
                            decimal? pct_pts_2pt_mr = !string.IsNullOrEmpty(pct_pts_2pt_mr_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_2pt_mr_string) : (decimal?)null;

                            string? pct_pts_3pt_string = leagueDashLineup[14]?.ToString();
                            decimal? pct_pts_3pt = !string.IsNullOrEmpty(pct_pts_3pt_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_3pt_string) : (decimal?)null;

                            string? pct_pts_fb_string = leagueDashLineup[15]?.ToString();
                            decimal? pct_pts_fb = !string.IsNullOrEmpty(pct_pts_fb_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_fb_string) : (decimal?)null;

                            string? pct_pts_ft_string = leagueDashLineup[16]?.ToString();
                            decimal? pct_pts_ft = !string.IsNullOrEmpty(pct_pts_ft_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_ft_string) : (decimal?)null;

                            string? pct_pts_off_tov_string = leagueDashLineup[17]?.ToString();
                            decimal? pct_pts_off_tov = !string.IsNullOrEmpty(pct_pts_off_tov_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_off_tov_string) : (decimal?)null;

                            string? pct_pts_paint_string = leagueDashLineup[18]?.ToString();
                            decimal? pct_pts_paint = !string.IsNullOrEmpty(pct_pts_paint_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_paint_string) : (decimal?)null;

                            string? pct_ast_2pm_string = leagueDashLineup[19]?.ToString();
                            decimal? pct_ast_2pm = !string.IsNullOrEmpty(pct_ast_2pm_string) ? JsonConvert.DeserializeObject<decimal>(pct_ast_2pm_string) : (decimal?)null;

                            string? pct_uast_2pm_string = leagueDashLineup[20]?.ToString();
                            decimal? pct_uast_2pm = !string.IsNullOrEmpty(pct_uast_2pm_string) ? JsonConvert.DeserializeObject<decimal>(pct_uast_2pm_string) : (decimal?)null;

                            string? pct_ast_3pm_string = leagueDashLineup[21]?.ToString();
                            decimal? pct_ast_3pm = !string.IsNullOrEmpty(pct_ast_3pm_string) ? JsonConvert.DeserializeObject<decimal>(pct_ast_3pm_string) : (decimal?)null;

                            string? pct_uast_3pm_string = leagueDashLineup[22]?.ToString();
                            decimal? pct_uast_3pm = !string.IsNullOrEmpty(pct_uast_3pm_string) ? JsonConvert.DeserializeObject<decimal>(pct_uast_3pm_string) : (decimal?)null;

                            string? pct_ast_fgm_string = leagueDashLineup[23]?.ToString();
                            decimal? pct_ast_fgm = !string.IsNullOrEmpty(pct_ast_fgm_string) ? JsonConvert.DeserializeObject<decimal>(pct_ast_fgm_string) : (decimal?)null;

                            string? pct_uast_fgm_string = leagueDashLineup[24]?.ToString();
                            decimal? pct_uast_fgm = !string.IsNullOrEmpty(pct_uast_fgm_string) ? JsonConvert.DeserializeObject<decimal>(pct_uast_fgm_string) : (decimal?)null;

                            string? gp_rank_string_scoring = leagueDashLineup[25]?.ToString();
                            decimal? gp_rank_scoring = !string.IsNullOrEmpty(gp_rank_string_scoring) ? JsonConvert.DeserializeObject<decimal>(gp_rank_string_scoring) : (decimal?)null;

                            string? w_rank_string_scoring = leagueDashLineup[26]?.ToString();
                            decimal? w_rank_scoring = !string.IsNullOrEmpty(w_rank_string_scoring) ? JsonConvert.DeserializeObject<decimal>(w_rank_string_scoring) : (decimal?)null;

                            string? l_rank_string_scoring = leagueDashLineup[27]?.ToString();
                            decimal? l_rank_scoring = !string.IsNullOrEmpty(l_rank_string_scoring) ? JsonConvert.DeserializeObject<decimal>(l_rank_string_scoring) : (decimal?)null;

                            string? w_pct_rank_string_scoring = leagueDashLineup[28]?.ToString();
                            decimal? w_pct_rank_scoring = !string.IsNullOrEmpty(w_pct_rank_string_scoring) ? JsonConvert.DeserializeObject<decimal>(w_pct_rank_string_scoring) : (decimal?)null;

                            string? min_rank_string_scoring = leagueDashLineup[29]?.ToString();
                            decimal? min_rank_scoring = !string.IsNullOrEmpty(min_rank_string_scoring) ? JsonConvert.DeserializeObject<decimal>(min_rank_string_scoring) : (decimal?)null;

                            string? pct_fga_2pt_rank_string = leagueDashLineup[30]?.ToString();
                            decimal? pct_fga_2pt_rank = !string.IsNullOrEmpty(pct_fga_2pt_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_fga_2pt_rank_string) : (decimal?)null;

                            string? pct_fga_3pt_rank_string = leagueDashLineup[31]?.ToString();
                            decimal? pct_fga_3pt_rank = !string.IsNullOrEmpty(pct_fga_3pt_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_fga_3pt_rank_string) : (decimal?)null;

                            string? pct_pts_2pt_rank_string = leagueDashLineup[32]?.ToString();
                            decimal? pct_pts_2pt_rank = !string.IsNullOrEmpty(pct_pts_2pt_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_2pt_rank_string) : (decimal?)null;

                            string? pct_pts_2pt_mr_rank_string = leagueDashLineup[33]?.ToString();
                            decimal? pct_pts_2pt_mr_rank = !string.IsNullOrEmpty(pct_pts_2pt_mr_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_2pt_mr_rank_string) : (decimal?)null;

                            string? pct_pts_3pt_rank_string = leagueDashLineup[34]?.ToString();
                            decimal? pct_pts_3pt_rank = !string.IsNullOrEmpty(pct_pts_3pt_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_3pt_rank_string) : (decimal?)null;

                            string? pct_pts_fb_rank_string = leagueDashLineup[35]?.ToString();
                            decimal? pct_pts_fb_rank = !string.IsNullOrEmpty(pct_pts_fb_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_fb_rank_string) : (decimal?)null;

                            string? pct_pts_ft_rank_string = leagueDashLineup[36]?.ToString();
                            decimal? pct_pts_ft_rank = !string.IsNullOrEmpty(pct_pts_ft_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_ft_rank_string) : (decimal?)null;

                            string? pct_pts_off_tov_rank_string = leagueDashLineup[37]?.ToString();
                            decimal? pct_pts_off_tov_rank = !string.IsNullOrEmpty(pct_pts_off_tov_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_off_tov_rank_string) : (decimal?)null;

                            string? pct_pts_paint_rank_string = leagueDashLineup[38]?.ToString();
                            decimal? pct_pts_paint_rank = !string.IsNullOrEmpty(pct_pts_paint_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_pts_paint_rank_string) : (decimal?)null;

                            string? pct_ast_2pm_rank_string = leagueDashLineup[39]?.ToString();
                            decimal? pct_ast_2pm_rank = !string.IsNullOrEmpty(pct_ast_2pm_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_ast_2pm_rank_string) : (decimal?)null;

                            string? pct_uast_2pm_rank_string = leagueDashLineup[40]?.ToString();
                            decimal? pct_uast_2pm_rank = !string.IsNullOrEmpty(pct_uast_2pm_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_uast_2pm_rank_string) : (decimal?)null;

                            string? pct_ast_3pm_rank_string = leagueDashLineup[41]?.ToString();
                            decimal? pct_ast_3pm_rank = !string.IsNullOrEmpty(pct_ast_3pm_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_ast_3pm_rank_string) : (decimal?)null;

                            string? pct_uast_3pm_rank_string = leagueDashLineup[42]?.ToString();
                            decimal? pct_uast_3pm_rank = !string.IsNullOrEmpty(pct_uast_3pm_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_uast_3pm_rank_string) : (decimal?)null;

                            string? pct_ast_fgm_rank_string = leagueDashLineup[43]?.ToString();
                            decimal? pct_ast_fgm_rank = !string.IsNullOrEmpty(pct_ast_fgm_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_ast_fgm_rank_string) : (decimal?)null;

                            string? pct_uast_fgm_rank_string = leagueDashLineup[44]?.ToString();
                            decimal? pct_uast_fgm_rank = !string.IsNullOrEmpty(pct_uast_fgm_rank_string) ? JsonConvert.DeserializeObject<decimal>(pct_uast_fgm_rank_string) : (decimal?)null;


                            using (var cmd = new NpgsqlCommand(sql, connection))
                            {
                                cmd.Parameters.Add(groupSetParam);
                                cmd.Parameters.Add(groupIdParam);
                                cmd.Parameters.Add(groupNameParam);
                                cmd.Parameters.Add(teamIdParam);
                                cmd.Parameters.Add(teamAbbreviationParam);
                                cmd.Parameters.AddWithValue("@gp", gp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w", w ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l", l ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct", w_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min", min ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_fga_2pt", pct_fga_2pt ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_fga_3pt", pct_fga_3pt ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_2pt", pct_pts_2pt ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_2pt_mr", pct_pts_2pt_mr ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_3pt", pct_pts_3pt ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_fb", pct_pts_fb ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_ft", pct_pts_ft ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_off_tov", pct_pts_off_tov ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_paint", pct_pts_paint ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_ast_2pm", pct_ast_2pm ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_uast_2pm", pct_uast_2pm ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_ast_3pm", pct_ast_3pm ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_uast_3pm", pct_uast_3pm ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_ast_fgm", pct_ast_fgm ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_uast_fgm", pct_uast_fgm ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@gp_rank", gp_rank_scoring ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_rank", w_rank_scoring ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l_rank", l_rank_scoring ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct_rank", w_pct_rank_scoring ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min_rank", min_rank_scoring ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_fga_2pt_rank", pct_fga_2pt_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_fga_3pt_rank", pct_fga_3pt_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_2pt_rank", pct_pts_2pt_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_2pt_mr_rank", pct_pts_2pt_mr_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_3pt_rank", pct_pts_3pt_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_fb_rank", pct_pts_fb_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_ft_rank", pct_pts_ft_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_off_tov_rank", pct_pts_off_tov_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_pts_paint_rank", pct_pts_paint_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_ast_2pm_rank", pct_ast_2pm_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_uast_2pm_rank", pct_uast_2pm_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_ast_3pm_rank", pct_ast_3pm_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_uast_3pm_rank", pct_uast_3pm_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_ast_fgm_rank", pct_ast_fgm_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@pct_uast_fgm_rank", pct_ast_fgm_rank ?? (object)DBNull.Value);

                                await cmd.ExecuteNonQueryAsync();
                            }

                        break;
                        case "Opponent":
                            sql = $"INSERT INTO league_dash_lineups_Opponent_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, opp_fgm, opp_fga, opp_fg_pct, opp_fg3m, opp_fg3a, opp_fg3_pct, opp_ftm, opp_fta, opp_ft_pct, opp_oreb, opp_dreb, opp_reb, opp_ast, opp_tov, opp_stl, opp_blk, opp_blka, opp_pf, opp_pfd1, opp_pts, plus_minus, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, opp_fgm_rank, opp_fga_rank, opp_fg_pct_rank, opp_fg3m_rank, opp_fg3a_rank, opp_fg3_pct_rank, opp_ftm_rank, opp_fta_rank, opp_ft_pct_rank, opp_oreb_rank, opp_dreb_rank, opp_reb_rank, opp_ast_rank, opp_tov_rank, opp_stl_rank, opp_blk_rank, opp_blka_rank, opp_pf_rank, opp_pfd1_rank, opp_pts_rank, plus_minus_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @opp_fgm, @opp_fga, @opp_fg_pct, @opp_fg3m, @opp_fg3a, @opp_fg3_pct, @opp_ftm, @opp_fta, @opp_ft_pct, @opp_oreb, @opp_dreb, @opp_reb, @opp_ast, @opp_tov, @opp_stl, @opp_blk, @opp_blka, @opp_pf, @opp_pfd1, @opp_pts, @plus_minus, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @opp_fgm_rank, @opp_fga_rank, @opp_fg_pct_rank, @opp_fg3m_rank, @opp_fg3a_rank, @opp_fg3_pct_rank, @opp_ftm_rank, @opp_fta_rank, @opp_ft_pct_rank, @opp_oreb_rank, @opp_dreb_rank, @opp_reb_rank, @opp_ast_rank, @opp_tov_rank, @opp_stl_rank, @opp_blk_rank, @opp_blka_rank, @opp_pf_rank, @opp_pfd1_rank, @opp_pts_rank, @plus_minus_rank)";

                            string? opp_fgm_string = leagueDashLineup[10]?.ToString();
                            decimal? opp_fgm = !string.IsNullOrEmpty(opp_fgm_string) ? JsonConvert.DeserializeObject<decimal>(opp_fgm_string) : (decimal?)null;

                            string? opp_fga_string = leagueDashLineup[11]?.ToString();
                            decimal? opp_fga = !string.IsNullOrEmpty(opp_fga_string) ? JsonConvert.DeserializeObject<decimal>(opp_fga_string) : (decimal?)null;

                            string? opp_fg_pct_string = leagueDashLineup[12]?.ToString();
                            decimal? opp_fg_pct = !string.IsNullOrEmpty(opp_fg_pct_string) ? JsonConvert.DeserializeObject<decimal>(opp_fg_pct_string) : (decimal?)null;

                            string? opp_fg3m_string = leagueDashLineup[13]?.ToString();
                            decimal? opp_fg3m = !string.IsNullOrEmpty(opp_fg3m_string) ? JsonConvert.DeserializeObject<decimal>(opp_fg3m_string) : (decimal?)null;

                            string? opp_fg3a_string = leagueDashLineup[14]?.ToString();
                            decimal? opp_fg3a = !string.IsNullOrEmpty(opp_fg3a_string) ? JsonConvert.DeserializeObject<decimal>(opp_fg3a_string) : (decimal?)null;

                            string? opp_fg3_pct_string = leagueDashLineup[15]?.ToString();
                            decimal? opp_fg3_pct = !string.IsNullOrEmpty(opp_fg3_pct_string) ? JsonConvert.DeserializeObject<decimal>(opp_fg3_pct_string) : (decimal?)null;

                            string? opp_ftm_string = leagueDashLineup[16]?.ToString();
                            decimal? opp_ftm = !string.IsNullOrEmpty(opp_ftm_string) ? JsonConvert.DeserializeObject<decimal>(opp_ftm_string) : (decimal?)null;

                            string? opp_fta_string = leagueDashLineup[17]?.ToString();
                            decimal? opp_fta = !string.IsNullOrEmpty(opp_fta_string) ? JsonConvert.DeserializeObject<decimal>(opp_fta_string) : (decimal?)null;

                            string? opp_ft_pct_string = leagueDashLineup[18]?.ToString();
                            decimal? opp_ft_pct = !string.IsNullOrEmpty(opp_ft_pct_string) ? JsonConvert.DeserializeObject<decimal>(opp_ft_pct_string) : (decimal?)null;

                            string? opp_oreb_string = leagueDashLineup[19]?.ToString();
                            decimal? opp_oreb = !string.IsNullOrEmpty(opp_oreb_string) ? JsonConvert.DeserializeObject<decimal>(opp_oreb_string) : (decimal?)null;

                            string? opp_dreb_string = leagueDashLineup[20]?.ToString();
                            decimal? opp_dreb = !string.IsNullOrEmpty(opp_dreb_string) ? JsonConvert.DeserializeObject<decimal>(opp_dreb_string) : (decimal?)null;

                            string? opp_reb_string = leagueDashLineup[21]?.ToString();
                            decimal? opp_reb = !string.IsNullOrEmpty(opp_reb_string) ? JsonConvert.DeserializeObject<decimal>(opp_reb_string) : (decimal?)null;

                            string? opp_ast_string = leagueDashLineup[22]?.ToString();
                            decimal? opp_ast = !string.IsNullOrEmpty(opp_ast_string) ? JsonConvert.DeserializeObject<decimal>(opp_ast_string) : (decimal?)null;

                            string? opp_tov_string = leagueDashLineup[23]?.ToString();
                            decimal? opp_tov = !string.IsNullOrEmpty(opp_tov_string) ? JsonConvert.DeserializeObject<decimal>(opp_tov_string) : (decimal?)null;

                            string? opp_stl_string = leagueDashLineup[24]?.ToString();
                            decimal? opp_stl = !string.IsNullOrEmpty(opp_stl_string) ? JsonConvert.DeserializeObject<decimal>(opp_stl_string) : (decimal?)null;

                            string? opp_blk_string = leagueDashLineup[25]?.ToString();
                            decimal? opp_blk = !string.IsNullOrEmpty(opp_blk_string) ? JsonConvert.DeserializeObject<decimal>(opp_blk_string) : (decimal?)null;

                            string? opp_blka_string = leagueDashLineup[26]?.ToString();
                            decimal? opp_blka = !string.IsNullOrEmpty(opp_blka_string) ? JsonConvert.DeserializeObject<decimal>(opp_blka_string) : (decimal?)null;

                            string? opp_pf_string = leagueDashLineup[27]?.ToString();
                            decimal? opp_pf = !string.IsNullOrEmpty(opp_pf_string) ? JsonConvert.DeserializeObject<decimal>(opp_pf_string) : (decimal?)null;

                            string? opp_pfd1_string = leagueDashLineup[28]?.ToString();
                            decimal? opp_pfd1 = !string.IsNullOrEmpty(opp_pfd1_string) ? JsonConvert.DeserializeObject<decimal>(opp_pfd1_string) : (decimal?)null;

                            string? opp_pts_string = leagueDashLineup[29]?.ToString();
                            decimal? opp_pts = !string.IsNullOrEmpty(opp_pts_string) ? JsonConvert.DeserializeObject<decimal>(opp_pts_string) : (decimal?)null;

                            string? plus_minus_string_opp = leagueDashLineup[30]?.ToString();
                            decimal? plus_minus_opp = !string.IsNullOrEmpty(plus_minus_string_opp) ? JsonConvert.DeserializeObject<decimal>(plus_minus_string_opp) : (decimal?)null;

                            string? gp_rank_string_opp = leagueDashLineup[31]?.ToString();
                            decimal? gp_rank_opp = !string.IsNullOrEmpty(gp_rank_string_opp) ? JsonConvert.DeserializeObject<decimal>(gp_rank_string_opp) : (decimal?)null;

                            string? w_rank_string_opp = leagueDashLineup[32]?.ToString();
                            decimal? w_rank_opp = !string.IsNullOrEmpty(w_rank_string_opp) ? JsonConvert.DeserializeObject<decimal>(w_rank_string_opp) : (decimal?)null;

                            string? l_rank_string_opp = leagueDashLineup[33]?.ToString();
                            decimal? l_rank_opp = !string.IsNullOrEmpty(l_rank_string_opp) ? JsonConvert.DeserializeObject<decimal>(l_rank_string_opp) : (decimal?)null;

                            string? w_pct_rank_string_opp = leagueDashLineup[34]?.ToString();
                            decimal? w_pct_rank_opp = !string.IsNullOrEmpty(w_pct_rank_string_opp) ? JsonConvert.DeserializeObject<decimal>(w_pct_rank_string_opp) : (decimal?)null;

                            string? min_rank_string_opp = leagueDashLineup[35]?.ToString();
                            decimal? min_rank_opp = !string.IsNullOrEmpty(min_rank_string_opp) ? JsonConvert.DeserializeObject<decimal>(min_rank_string_opp) : (decimal?)null;

                            string? opp_fgm_rank_string = leagueDashLineup[36]?.ToString();
                            decimal? opp_fgm_rank = !string.IsNullOrEmpty(opp_fgm_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_fgm_rank_string) : (decimal?)null;

                            string? opp_fga_rank_string = leagueDashLineup[37]?.ToString();
                            decimal? opp_fga_rank = !string.IsNullOrEmpty(opp_fga_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_fga_rank_string) : (decimal?)null;

                            string? opp_fg_pct_rank_string = leagueDashLineup[38]?.ToString();
                            decimal? opp_fg_pct_rank = !string.IsNullOrEmpty(opp_fg_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_fg_pct_rank_string) : (decimal?)null;

                            string? opp_fg3m_rank_string = leagueDashLineup[39]?.ToString();
                            decimal? opp_fg3m_rank = !string.IsNullOrEmpty(opp_fg3m_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_fg3m_rank_string) : (decimal?)null;

                            string? opp_fg3a_rank_string = leagueDashLineup[40]?.ToString();
                            decimal? opp_fg3a_rank = !string.IsNullOrEmpty(opp_fg3a_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_fg3a_rank_string) : (decimal?)null;

                            string? opp_fg3_pct_rank_string = leagueDashLineup[41]?.ToString();
                            decimal? opp_fg3_pct_rank = !string.IsNullOrEmpty(opp_fg3_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_fg3_pct_rank_string) : (decimal?)null;

                            string? opp_ftm_rank_string = leagueDashLineup[42]?.ToString();
                            decimal? opp_ftm_rank = !string.IsNullOrEmpty(opp_ftm_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_ftm_rank_string) : (decimal?)null;

                            string? opp_fta_rank_string = leagueDashLineup[43]?.ToString();
                            decimal? opp_fta_rank = !string.IsNullOrEmpty(opp_fta_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_fta_rank_string) : (decimal?)null;

                            string? opp_ft_pct_rank_string = leagueDashLineup[44]?.ToString();
                            decimal? opp_ft_pct_rank = !string.IsNullOrEmpty(opp_ft_pct_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_ft_pct_rank_string) : (decimal?)null;

                            string? opp_oreb_rank_string = leagueDashLineup[45]?.ToString();
                            decimal? opp_oreb_rank = !string.IsNullOrEmpty(opp_oreb_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_oreb_rank_string) : (decimal?)null;

                            string? opp_dreb_rank_string = leagueDashLineup[46]?.ToString();
                            decimal? opp_dreb_rank = !string.IsNullOrEmpty(opp_dreb_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_dreb_rank_string) : (decimal?)null;

                            string? opp_reb_rank_string = leagueDashLineup[47]?.ToString();
                            decimal? opp_reb_rank = !string.IsNullOrEmpty(opp_reb_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_reb_rank_string) : (decimal?)null;

                            string? opp_ast_rank_string = leagueDashLineup[48]?.ToString();
                            decimal? opp_ast_rank = !string.IsNullOrEmpty(opp_ast_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_ast_rank_string) : (decimal?)null;

                            string? opp_tov_rank_string = leagueDashLineup[49]?.ToString();
                            decimal? opp_tov_rank = !string.IsNullOrEmpty(opp_tov_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_tov_rank_string) : (decimal?)null;

                            string? opp_stl_rank_string = leagueDashLineup[50]?.ToString();
                            decimal? opp_stl_rank = !string.IsNullOrEmpty(opp_stl_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_stl_rank_string) : (decimal?)null;

                            string? opp_blk_rank_string = leagueDashLineup[51]?.ToString();
                            decimal? opp_blk_rank = !string.IsNullOrEmpty(opp_blk_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_blk_rank_string) : (decimal?)null;

                            string? opp_blka_rank_string = leagueDashLineup[52]?.ToString();
                            decimal? opp_blka_rank = !string.IsNullOrEmpty(opp_blka_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_blka_rank_string) : (decimal?)null;

                            string? opp_pf_rank_string = leagueDashLineup[53]?.ToString();
                            decimal? opp_pf_rank = !string.IsNullOrEmpty(opp_pf_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_pf_rank_string) : (decimal?)null;

                            string? opp_pfd1_rank_string = leagueDashLineup[54]?.ToString();
                            decimal? opp_pfd1_rank = !string.IsNullOrEmpty(opp_pfd1_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_pfd1_rank_string) : (decimal?)null;

                            string? opp_pts_rank_string = leagueDashLineup[55]?.ToString();
                            decimal? opp_pts_rank = !string.IsNullOrEmpty(opp_pts_rank_string) ? JsonConvert.DeserializeObject<decimal>(opp_pts_rank_string) : (decimal?)null;

                            string? plus_minus_rank_string_opp = leagueDashLineup[56]?.ToString();
                            decimal? plus_minus_rank_opp = !string.IsNullOrEmpty(plus_minus_rank_string_opp) ? JsonConvert.DeserializeObject<decimal>(plus_minus_rank_string_opp) : (decimal?)null;

                            using (var cmd = new NpgsqlCommand(sql, connection))
                            {
                                cmd.Parameters.Add(groupSetParam);
                                cmd.Parameters.Add(groupIdParam);
                                cmd.Parameters.Add(groupNameParam);
                                cmd.Parameters.Add(teamIdParam);
                                cmd.Parameters.Add(teamAbbreviationParam);
                                cmd.Parameters.AddWithValue("@gp", gp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w", w ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l", l ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct", w_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min", min ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fgm", opp_fgm ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fga", opp_fga ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fg_pct", opp_fg_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fg3m", opp_fg3m ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fg3a", opp_fg3a ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fg3_pct", opp_fg3_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_ftm", opp_ftm ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fta", opp_fta ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_ft_pct", opp_ft_pct ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_oreb", opp_oreb ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_dreb", opp_dreb ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_reb", opp_reb ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_ast", opp_ast ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_tov", opp_tov ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_stl", opp_stl ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_blk", opp_blk ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_blka", opp_blka ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pf", opp_pf ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pfd1", opp_pfd1 ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pts", opp_pts ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@plus_minus", plus_minus_opp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@gp_rank", gp_rank_opp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_rank", w_rank_opp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@l_rank", l_rank_opp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@w_pct_rank", w_pct_rank_opp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@min_rank", min_rank_opp ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fgm_rank", opp_fgm_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fga_rank", opp_fga_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fg_pct_rank", opp_fg_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fg3m_rank", opp_fg3m_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fg3a_rank", opp_fg3a_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fg3_pct_rank", opp_fg3_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_ftm_rank", opp_ftm_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_fta_rank", opp_fta_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_ft_pct_rank", opp_ft_pct_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_oreb_rank", opp_oreb_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_dreb_rank", opp_dreb_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_reb_rank", opp_reb_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_ast_rank", opp_ast_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_tov_rank", opp_tov_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_stl_rank", opp_stl_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_blk_rank", opp_blk_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_blka_rank", opp_blka_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pf_rank", opp_pf_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pfd1_rank", opp_pfd1_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@opp_pts_rank", opp_pts_rank ?? (object)DBNull.Value);
                                cmd.Parameters.AddWithValue("@plus_minus_rank", plus_minus_rank_opp ?? (object)DBNull.Value);
                            // ... (continuing for other columns)
                                await cmd.ExecuteNonQueryAsync();
                            }
                        break;
                        default:
                            // Handle an unexpected boxType value here
                            throw new ArgumentException("Invalid boxType");
                    }

                }

                return StatusCode(201, leagueDashLineup);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
