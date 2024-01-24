using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupOpponent
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("group_set")]
        public string? Group_set { get; set; }

        [Column("group_id")]
        public string? Group_id { get; set; }

        [Column("group_name")]
        public string? Group_name { get; set; }

        [Column("team_id")]
        public string? Team_id { get; set; }

        [Column("team_abbreviation")]
        public string? Team_abbreviation { get; set; }

        [Column("gp")]
        public int? Gp { get; set; }

        [Column("w")]
        public int? W { get; set; }

        [Column("l")]
        public int? L { get; set; }

        [Column("w_pct")]
        public decimal? W_pct { get; set; }

        [Column("min")]
        public decimal? Min { get; set; }

        [Column("opp_fgm")]
        public decimal? Opp_fgm { get; set; }

        [Column("opp_fga")]
        public decimal? Opp_fga { get; set; }

        [Column("opp_fg_pct")]
        public decimal? Opp_fg_pct { get; set; }

        [Column("opp_fg3m")]
        public decimal? Opp_fg3m { get; set; }

        [Column("opp_fg3a")]
        public decimal? Opp_fg3a { get; set; }

        [Column("opp_fg3_pct")]
        public decimal? Opp_fg3_pct { get; set; }

        [Column("opp_ftm")]
        public decimal? Opp_ftm { get; set; }

        [Column("opp_fta")]
        public decimal? Opp_fta { get; set; }

        [Column("opp_ft_pct")]
        public decimal? Opp_ft_pct { get; set; }

        [Column("opp_oreb")]
        public decimal? Opp_oreb { get; set; }

        [Column("opp_dreb")]
        public decimal? Opp_dreb { get; set; }

        [Column("opp_reb")]
        public decimal? Opp_reb { get; set; }

        [Column("opp_ast")]
        public decimal? Opp_ast { get; set; }

        [Column("opp_tov")]
        public decimal? Opp_tov { get; set; }

        [Column("opp_stl")]
        public decimal? Opp_stl { get; set; }

        [Column("opp_blk")]
        public decimal? Opp_blk { get; set; }

        [Column("opp_blka")]
        public decimal? Opp_blka { get; set; }

        [Column("opp_pf")]
        public decimal? Opp_pf { get; set; }

        [Column("opp_pfd")]
        public decimal? Opp_pfd { get; set; }

        [Column("opp_pts")]
        public decimal? Opp_pts { get; set; }

        [Column("plus_minus")]
        public decimal? Plus_minus { get; set; }

        [Column("gp_rank")]
        public int? Gp_rank { get; set; }

        [Column("w_rank")]
        public int? W_rank { get; set; }

        [Column("l_rank")]
        public int? L_rank { get; set; }

        [Column("w_pct_rank")]
        public int? W_pct_rank { get; set; }

        [Column("min_rank")]
        public int? Min_rank { get; set; }

        [Column("opp_fgm_rank")]
        public int? Opp_fgm_rank { get; set; }

        [Column("opp_fga_rank")]
        public int? Opp_fga_rank { get; set; }

        [Column("opp_fg_pct_rank")]
        public int? Opp_fg_pct_rank { get; set; }

        [Column("opp_fg3m_rank")]
        public int? Opp_fg3m_rank { get; set; }

        [Column("opp_fg3a_rank")]
        public int? Opp_fg3a_rank { get; set; }

        [Column("opp_fg3_pct_rank")]
        public int? Opp_fg3_pct_rank { get; set; }

        [Column("opp_ftm_rank")]
        public int? Opp_ftm_rank { get; set; }

        [Column("opp_fta_rank")]
        public int? Opp_fta_rank { get; set; }

        [Column("opp_ft_pct_rank")]
        public int? Opp_ft_pct_rank { get; set; }

        [Column("opp_oreb_rank")]
        public int? Opp_oreb_rank { get; set; }

        [Column("opp_dreb_rank")]
        public int? Opp_dreb_rank { get; set; }

        [Column("opp_reb_rank")]
        public int? Opp_reb_rank { get; set; }

        [Column("opp_ast_rank")]
        public int? Opp_ast_rank { get; set; }

        [Column("opp_tov_rank")]
        public int? Opp_tov_rank { get; set; }

        [Column("opp_stl_rank")]
        public int? Opp_stl_rank { get; set; }

        [Column("opp_blk_rank")]
        public int? Opp_blk_rank { get; set; }

        [Column("opp_blka_rank")]
        public int? Opp_blka_rank { get; set; }

        [Column("opp_pf_rank")]
        public int? Opp_pf_rank { get; set; }

        [Column("opp_pfd1_rank")]
        public int? Opp_pfd1_rank { get; set; }

        [Column("opp_pts_rank")]
        public int? Opp_pts_rank { get; set; }

        [Column("plus_minus_rank")]
        public int? Plus_minus_rank { get; set; }
    }
}
