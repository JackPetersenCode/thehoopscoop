using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReactApp4.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using ReactApp4.Server.Data;

namespace ReactApp4.Server.Services
{
    public class LeagueDashLineupsDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

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
                    switch (boxType)
                    {
                        case "Base":
                            sql = $"INSERT INTO league_dash_lineups_base_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, fgm, fga, fg_pct, fg3m, fg3a, fg3_pct, ftm, fta, ft_pct, oreb, dreb, reb, ast, stl, blk, blka, pf, pfd, pts, plus_minus, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, fgm_rank, fga_rank, fg_pct_rank, fg3m_rank, fg3a_rank, fg3_pct_rank, ftm_rank, fta_rank, ft_pct_rank, oreb_rank, dreb_rank, reb_rank, ast_rank, tov_rank, stl_rank, blk_rank, blka_rank, pf_rank, pfd_rank, pts_rank, plus_minus_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @fgm, @fga, @fg_pct, @fg3m, @fg3a, @fg3_pct, @ftm, @fta, @ft_pct, @oreb, @dreb, @reb, @ast, @stl, @blk, @blka, @pf, @pfd, @pts, @plus_minus, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @fgm_rank, @fga_rank, @fg_pct_rank, @fg3m_rank, @fg3a_rank, @fg3_pct_rank, @ftm_rank, @fta_rank, @ft_pct_rank, @oreb_rank, @dreb_rank, @reb_rank, @ast_rank, @tov_rank, @stl_rank, @blk_rank, @blka_rank, @pf_rank, @pfd_rank, @pts_rank, @plus_minus_rank);";

                            Console.WriteLine(leagueDashLineup.Length);
                            Console.WriteLine(leagueDashLineup);

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

                            // Add these parameters to the NpgsqlCommand similar to the previous columns


                            // Add these parameters to the NpgsqlCommand similar to the previous columns


                            // Add these parameters to the NpgsqlCommand similar to the previous columns


                            // Assuming you have declared NpgsqlParameter instances for each of these columns,
                            // add them to the command's parameters similarly to the previous example.
                            // Adjust the parameter names and values accordingly in your NpgsqlCommand.

                            //string video_available_string = leagueGame[28]?.ToString();
                            //decimal? video_available = !string.IsNullOrEmpty(video_available_string) ? JsonConvert.DeserializeObject<decimal>(video_available_string) : (decimal?)null;
                            // Create an NpgsqlParameter for team_name and set the value without quotes

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
                            sql = $"INSERT INTO league_dash_lineups_Advanced_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, e_off_rating, off_rating, e_def_rating, def_rating, e_net_rating, net_rating, ast_pct, ast_to, ast_ratio, oreb_pct, dreb_pct, reb_pct, tm_tov_pct, efg_pct, ts_pct, e_pace, pace, pace_per40, poss, pie, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, off_rating_rank, def_rating_rank, net_rating_rank, ast_pct_rank, ast_to_rank, ast_ratio_rank, oreb_pct_rank, dreb_pct_rank, reb_pct_rank, tm_tov_pct_rank, efg_pct_rank, ts_pct_rank, pace_rank, pie_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @e_off_rating, @off_rating, @e_def_rating, @def_rating, @e_net_rating, @net_rating, @ast_pct, @ast_to, @ast_ratio, @oreb_pct, @dreb_pct, @reb_pct, @tm_tov_pct, @efg_pct, @ts_pct, @e_pace, @pace, @pace_per40, @poss, @pie, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @off_rating_rank, @def_rating_rank, @net_rating_rank, @ast_pct_rank, @ast_to_rank, @ast_ratio_rank, @oreb_pct_rank, @dreb_pct_rank, @reb_pct_rank, @tm_tov_pct_rank, @efg_pct_rank, @ts_pct_rank, @pace_rank, @pie_rank);";
                            break;
                        case "Four Factors":
                            sql = $"INSERT INTO league_dash_lineups_FourFactors_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, efg_pct, fta_rate, tm_tov_pct, oreb_pct, opp_efg_pct, opp_fta_rate, opp_tov_pct, opp_oreb_pct, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, efg_pct_rank, fta_rate_rank, tm_tov_pct_rank, oreb_pct_rank, opp_efg_pct_rank, opp_fta_rate_rank, opp_tov_pct_rank, opp_oreb_pct_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @efg_pct, @fta_rate, @tm_tov_pct, @oreb_pct, @opp_efg_pct, @opp_fta_rate, @opp_tov_pct, @opp_oreb_pct, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @efg_pct_rank, @fta_rate_rank, @tm_tov_pct_rank, @oreb_pct_rank, @opp_efg_pct_rank, @opp_fta_rate_rank, @opp_tov_pct_rank, @opp_oreb_pct_rank);";
                            break;
                        case "Misc":
                            sql = $"INSERT INTO league_dash_lineups_Misc_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, pts_off_tov, pts_2nd_chance, pts_fb, pts_paint, opp_pts_off_tov, opp_pts_2nd_chance, opp_pts_fb, opp_pts_paint, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, pts_off_tov_rank, pts_2nd_chance_rank, pts_fb_rank, pts_paint_rank, opp_pts_off_tov_rank, opp_pts_2nd_chance_rank, opp_pts_fb_rank, opp_pts_paint_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @pts_off_tov, @pts_2nd_chance, @pts_fb, @pts_paint, @opp_pts_off_tov, @opp_pts_2nd_chance, @opp_pts_fb, @opp_pts_paint, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @pts_off_tov_rank, @pts_2nd_chance_rank, @pts_fb_rank, @pts_paint_rank, @opp_pts_off_tov_rank, @opp_pts_2nd_chance_rank, @opp_pts_fb_rank, @opp_pts_paint_rank);";
                            break;
                        case "Scoring":
                            sql = $"INSERT INTO league_dash_lineups_Scoring_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, pct_fga_2pt, pct_fga_3pt, pct_pts_2pt, pct_pts_2pt_mr, pct_pts_3pt, pct_pts_fb, pct_pts_ft, pct_pts_off_tov, pct_pts_paint, pct_ast_2pm, pct_uast_2pm, pct_ast_3pm, pct_uast_3pm, pct_ast_fgm, pct_uast_fgm, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, pct_fga_2pt_rank, pct_fga_3pt_rank, pct_pts_2pt_rank, pct_pts_2pt_mr_rank, pct_pts_3pt_rank, pct_pts_fb_rank, pct_pts_ft_rank, pct_pts_off_tov_rank, pct_pts_paint_rank, pct_ast_2pm_rank, pct_uast_2pm_rank, pct_ast_3pm_rank, pct_uast_3pm_rank, pct_ast_fgm_rank, pct_uast_fgm_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @pct_fga_2pt, @pct_fga_3pt, @pct_pts_2pt, @pct_pts_2pt_mr, @pct_pts_3pt, @pct_pts_fb, @pct_pts_ft, @pct_pts_off_tov, @pct_pts_paint, @pct_ast_2pm, @pct_uast_2pm, @pct_ast_3pm, @pct_uast_3pm, @pct_ast_fgm, @pct_uast_fgm, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @pct_fga_2pt_rank, @pct_fga_3pt_rank, @pct_pts_2pt_rank, @pct_pts_2pt_mr_rank, @pct_pts_3pt_rank, @pct_pts_fb_rank, @pct_pts_ft_rank, @pct_pts_off_tov_rank, @pct_pts_paint_rank, @pct_ast_2pm_rank, @pct_uast_2pm_rank, @pct_ast_3pm_rank, @pct_uast_3pm_rank, @pct_ast_fgm_rank, @pct_uast_fgm_rank);";
                            break;
                        case "Opponent":
                            sql = $"INSERT INTO league_dash_lineups_Opponent_{numPlayers}man_{season} (group_set, group_id, group_name, team_id, team_abbreviation, gp, w, l, w_pct, min, opp_fgm, opp_fga, opp_fg_pct, opp_fg3m, opp_fg3a, opp_fg3_pct, opp_ftm, opp_fta, opp_ft_pct, opp_oreb, opp_dreb, opp_reb, opp_ast, opp_tov, opp_stl, opp_blk, opp_blka, opp_pf, opp_pfd1, opp_pts, plus_minus, gp_rank, w_rank, l_rank, w_pct_rank, min_rank, opp_fgm_rank, opp_fga_rank, opp_fg_pct_rank, opp_fg3m_rank, opp_fg3a_rank, opp_fg3_pct_rank, opp_ftm_rank, opp_fta_rank, opp_ft_pct_rank, opp_oreb_rank, opp_dreb_rank, opp_reb_rank, opp_ast_rank, opp_tov_rank, opp_stl_rank, opp_blk_rank, opp_blka_rank, opp_pf_rank, opp_pfd1_rank, opp_pts_rank, plus_minus_rank) VALUES (@group_set, @group_id, @group_name, @team_id, @team_abbreviation, @gp, @w, @l, @w_pct, @min, @opp_fgm, @opp_fga, @opp_fg_pct, @opp_fg3m, @opp_fg3a, @opp_fg3_pct, @opp_ftm, @opp_fta, @opp_ft_pct, @opp_oreb, @opp_dreb, @opp_reb, @opp_ast, @opp_tov, @opp_stl, @opp_blk, @opp_blka, @opp_pf, @opp_pfd1, @opp_pts, @plus_minus, @gp_rank, @w_rank, @l_rank, @w_pct_rank, @min_rank, @opp_fgm_rank, @opp_fga_rank, @opp_fg_pct_rank, @opp_fg3m_rank, @opp_fg3a_rank, @opp_fg3_pct_rank, @opp_ftm_rank, @opp_fta_rank, @opp_ft_pct_rank, @opp_oreb_rank, @opp_dreb_rank, @opp_reb_rank, @opp_ast_rank, @opp_tov_rank, @opp_stl_rank, @opp_blk_rank, @opp_blka_rank, @opp_pf_rank, @opp_pfd1_rank, @opp_pts_rank, @plus_minus_rank)";
                            break;
                        default:
                            // Handle an unexpected boxType value here
                            throw new ArgumentException("Invalid boxType");
                    }

                    //var sql = $"INSERT INTO league_dash_lineups_{boxType}_{numPlayers}man_{season} (season_id, team_id, team_abbreviation, team_name, game_id, game_date, matchup, wl, min, fgm, fga, fg_pct, fg3m, fg3a, fg3_pct, ftm, fta, ft_pct, oreb, dreb, reb, ast, stl, blk, tov, pf, pts, plus_minus, video_available) VALUES (@season_id, @team_id, @team_abbreviation, @team_name, @game_id, @game_date, @matchup, @wl, @min, @fgm, @fga, @fg_pct, @fg3m, @fg3a, @fg3_pct, @ftm, @fta, @ft_pct, @oreb, @dreb, @reb, @ast, @stl, @blk, @tov, @pf, @pts, @plus_minus, @video_available);";

                    //string? groupSetString = ?.ToString();
                    //NpgsqlParameter seasonIdParam = new NpgsqlParameter("@season_id", NpgsqlDbType.Text);
                    //seasonIdParam.Value = seasonIdString;
                    //
                    //string? teamIdString = leagueGame[1]?.ToString();
                    //NpgsqlParameter teamIdParam = new NpgsqlParameter("@team_id", NpgsqlDbType.Text);
                    //teamIdParam.Value = teamIdString;
                    //
                    //string? teamAbbreviationString = leagueGame[2]?.ToString();
                    //NpgsqlParameter teamAbbreviationParam = new NpgsqlParameter("@team_abbreviation", NpgsqlDbType.Text);
                    //teamAbbreviationParam.Value = teamAbbreviationString;
                    //
                    //string? teamNameString = leagueGame[3]?.ToString();
                    //NpgsqlParameter teamNameParam = new NpgsqlParameter("@team_name", NpgsqlDbType.Text);
                    //teamNameParam.Value = teamNameString;
                    //
                    //string? gameIdString = leagueGame[4]?.ToString();
                    //NpgsqlParameter gameIdParam = new NpgsqlParameter("@game_id", NpgsqlDbType.Text);
                    //gameIdParam.Value = gameIdString;
                    //
                    //string? gameDateString = leagueGame[5]?.ToString();
                    //NpgsqlParameter gameDateParam = new NpgsqlParameter("@game_date", NpgsqlDbType.Text);
                    //gameDateParam.Value = gameDateString;
                    //
                    //string? matchupString = leagueGame[6]?.ToString();
                    //NpgsqlParameter matchupParam = new NpgsqlParameter("@matchup", NpgsqlDbType.Text);
                    //matchupParam.Value = matchupString;
                    //
                    //string? wlString = leagueGame[7]?.ToString();
                    //NpgsqlParameter wlParam = new NpgsqlParameter("@wl", NpgsqlDbType.Text);
                    //wlParam.Value = wlString;
                    //
                    //string min_string = leagueGame[8]?.ToString();
                    //decimal? min = !string.IsNullOrEmpty(min_string) ? JsonConvert.DeserializeObject<decimal>(min_string) : (decimal?)null;
                    //string fgm_string = leagueGame[9]?.ToString();
                    //decimal? fgm = !string.IsNullOrEmpty(fgm_string) ? JsonConvert.DeserializeObject<decimal>(fgm_string) : (decimal?)null;
                    //string fga_string = leagueGame[10]?.ToString();
                    //decimal? fga = !string.IsNullOrEmpty(fga_string) ? JsonConvert.DeserializeObject<decimal>(fga_string) : (decimal?)null;
                    //string fg_pct_string = leagueGame[11]?.ToString();
                    //decimal? fg_pct = !string.IsNullOrEmpty(fg_pct_string) ? JsonConvert.DeserializeObject<decimal>(fg_pct_string) : (decimal?)null;
                    //string fg3m_string = leagueGame[12]?.ToString();
                    //decimal? fg3m = !string.IsNullOrEmpty(fg3m_string) ? JsonConvert.DeserializeObject<decimal>(fg3m_string) : (decimal?)null;
                    //string fg3a_string = leagueGame[13]?.ToString();
                    //decimal? fg3a = !string.IsNullOrEmpty(fg3a_string) ? JsonConvert.DeserializeObject<decimal>(fg3a_string) : (decimal?)null;
                    //string fg3_pct_string = leagueGame[14]?.ToString();
                    //decimal? fg3_pct = !string.IsNullOrEmpty(fg3_pct_string) ? JsonConvert.DeserializeObject<decimal>(fg3_pct_string) : (decimal?)null;
                    //string ftm_string = leagueGame[15]?.ToString();
                    //decimal? ftm = !string.IsNullOrEmpty(ftm_string) ? JsonConvert.DeserializeObject<decimal>(ftm_string) : (decimal?)null;
                    //string fta_string = leagueGame[16]?.ToString();
                    //decimal? fta = !string.IsNullOrEmpty(fta_string) ? JsonConvert.DeserializeObject<decimal>(fta_string) : (decimal?)null;
                    //string ft_pct_string = leagueGame[17]?.ToString();
                    //decimal? ft_pct = !string.IsNullOrEmpty(ft_pct_string) ? JsonConvert.DeserializeObject<decimal>(ft_pct_string) : (decimal?)null;
                    //string oreb_string = leagueGame[18]?.ToString();
                    //decimal? oreb = !string.IsNullOrEmpty(oreb_string) ? JsonConvert.DeserializeObject<decimal>(oreb_string) : (decimal?)null;
                    //string dreb_string = leagueGame[19]?.ToString();
                    //decimal? dreb = !string.IsNullOrEmpty(dreb_string) ? JsonConvert.DeserializeObject<decimal>(dreb_string) : (decimal?)null;
                    //string reb_string = leagueGame[20]?.ToString();
                    //decimal? reb = !string.IsNullOrEmpty(reb_string) ? JsonConvert.DeserializeObject<decimal>(reb_string) : (decimal?)null;
                    //string ast_string = leagueGame[21]?.ToString();
                    //decimal? ast = !string.IsNullOrEmpty(ast_string) ? JsonConvert.DeserializeObject<decimal>(ast_string) : (decimal?)null;
                    //string stl_string = leagueGame[22]?.ToString();
                    //decimal? stl = !string.IsNullOrEmpty(stl_string) ? JsonConvert.DeserializeObject<decimal>(stl_string) : (decimal?)null;
                    //string blk_string = leagueGame[23]?.ToString();
                    //decimal? blk = !string.IsNullOrEmpty(blk_string) ? JsonConvert.DeserializeObject<decimal>(blk_string) : (decimal?)null;
                    //string tov_string = leagueGame[24]?.ToString();
                    //decimal? tov = !string.IsNullOrEmpty(tov_string) ? JsonConvert.DeserializeObject<decimal>(tov_string) : (decimal?)null;
                    //string pf_string = leagueGame[25]?.ToString();
                    //decimal? pf = !string.IsNullOrEmpty(pf_string) ? JsonConvert.DeserializeObject<decimal>(pf_string) : (decimal?)null;
                    //string pts_string = leagueGame[26]?.ToString();
                    //decimal? pts = !string.IsNullOrEmpty(pts_string) ? JsonConvert.DeserializeObject<decimal>(pts_string) : (decimal?)null;
                    //string plus_minus_string = leagueGame[27]?.ToString();
                    //decimal? plus_minus = !string.IsNullOrEmpty(plus_minus_string) ? JsonConvert.DeserializeObject<decimal>(plus_minus_string) : (decimal?)null;
                    ////string video_available_string = leagueGame[28]?.ToString();
                    ////decimal? video_available = !string.IsNullOrEmpty(video_available_string) ? JsonConvert.DeserializeObject<decimal>(video_available_string) : (decimal?)null;
                    //string? videoAvailableString = leagueGame[28]?.ToString();
                    //// Create an NpgsqlParameter for team_name and set the value without quotes
                    //NpgsqlParameter videoAvailableParam = new NpgsqlParameter("@video_available", NpgsqlDbType.Text);
                    //videoAvailableParam.Value = videoAvailableString;

                    //using (var cmd = new NpgsqlCommand(sql, connection))
                    //{
                    //    cmd.Parameters.Add(seasonIdParam);
                    //    cmd.Parameters.Add(teamIdParam);
                    //    cmd.Parameters.Add(teamAbbreviationParam);
                    //    cmd.Parameters.Add(teamNameParam);
                    //    cmd.Parameters.Add(gameIdParam);
                    //    cmd.Parameters.Add(gameDateParam);
                    //    cmd.Parameters.Add(matchupParam);
                    //    cmd.Parameters.Add(wlParam);
                    //    cmd.Parameters.AddWithValue("@min", min ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@fgm", fgm ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@fga", fga ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@fg_pct", fg_pct ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@fg3m", fg3m ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@fg3a", fg3a ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@fg3_pct", fg3_pct ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@ftm", ftm ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@fta", fta ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@ft_pct", ft_pct ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@oreb", oreb ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@dreb", dreb ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@reb", reb ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@ast", ast ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@stl", stl ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@blk", blk ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@tov", tov ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@pf", pf ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@pts", pts ?? (object)DBNull.Value);
                    //    cmd.Parameters.AddWithValue("@plus_minus", plus_minus ?? (object)DBNull.Value);
                    //    cmd.Parameters.Add(videoAvailableParam);
                    //
                    //    await cmd.ExecuteNonQueryAsync();
                    //}
                }

                return Ok("League game created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
