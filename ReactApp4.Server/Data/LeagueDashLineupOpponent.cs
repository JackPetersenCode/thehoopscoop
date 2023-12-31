using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupOpponent
    {
        [Column("group_set")]
        public string? GROUP_SET { get; set; }

        [Column("group_id")]
        public string? GROUP_ID { get; set; }

        [Column("group_name")]
        public string? GROUP_NAME { get; set; }

        [Column("team_id")]
        public string? TEAM_ID { get; set; }

        [Column("team_abbreviation")]
        public string? TEAM_ABBREVIATION { get; set; }

        [Column("gp")]
        public int? GP { get; set; }

        [Column("w")]
        public int? W { get; set; }

        [Column("l")]
        public int? L { get; set; }

        [Column("w_pct")]
        public decimal? W_PCT { get; set; }

        [Column("min")]
        public decimal? MIN { get; set; }

        [Column("opp_fgm")]
        public decimal? OPP_FGM { get; set; }

        [Column("opp_fga")]
        public decimal? OPP_FGA { get; set; }

        [Column("opp_fg_pct")]
        public decimal? OPP_FG_PCT { get; set; }

        [Column("opp_fg3m")]
        public decimal? OPP_FG3M { get; set; }

        [Column("opp_fg3a")]
        public decimal? OPP_FG3A { get; set; }

        [Column("opp_fg3_pct")]
        public decimal? OPP_FG3_PCT { get; set; }

        [Column("opp_ftm")]
        public decimal? OPP_FTM { get; set; }

        [Column("opp_fta")]
        public decimal? OPP_FTA { get; set; }

        [Column("opp_ft_pct")]
        public decimal? OPP_FT_PCT { get; set; }

        [Column("opp_oreb")]
        public decimal? OPP_OREB { get; set; }

        [Column("opp_dreb")]
        public decimal? OPP_DREB { get; set; }

        [Column("opp_reb")]
        public decimal? OPP_REB { get; set; }

        [Column("opp_ast")]
        public decimal? OPP_AST { get; set; }

        [Column("opp_tov")]
        public decimal? OPP_TOV { get; set; }

        [Column("opp_stl")]
        public decimal? OPP_STL { get; set; }

        [Column("opp_blk")]
        public decimal? OPP_BLK { get; set; }

        [Column("opp_blka")]
        public decimal? OPP_BLKA { get; set; }

        [Column("opp_pf")]
        public decimal? OPP_PF { get; set; }

        [Column("opp_pfd")]
        public decimal? OPP_PFD { get; set; }

        [Column("opp_pts")]
        public decimal? OPP_PTS { get; set; }

        [Column("plus_minus")]
        public decimal? PLUS_MINUS { get; set; }

        [Column("gp_rank")]
        public int? GP_RANK { get; set; }

        [Column("w_rank")]
        public int? W_RANK { get; set; }

        [Column("l_rank")]
        public int? L_RANK { get; set; }

        [Column("w_pct_rank")]
        public int? W_PCT_RANK { get; set; }

        [Column("min_rank")]
        public int? MIN_RANK { get; set; }

        [Column("opp_fgm_rank")]
        public int? OPP_FGM_RANK { get; set; }

        [Column("opp_fga_rank")]
        public int? OPP_FGA_RANK { get; set; }

        [Column("opp_fg_pct_rank")]
        public int? OPP_FG_PCT_RANK { get; set; }

        [Column("opp_fg3m_rank")]
        public int? OPP_FG3M_RANK { get; set; }

        [Column("opp_fg3a_rank")]
        public int? OPP_FG3A_RANK { get; set; }

        [Column("opp_fg3_pct_rank")]
        public int? OPP_FG3_PCT_RANK { get; set; }

        [Column("opp_ftm_rank")]
        public int? OPP_FTM_RANK { get; set; }

        [Column("opp_fta_rank")]
        public int? OPP_FTA_RANK { get; set; }

        [Column("opp_ft_pct_rank")]
        public int? OPP_FT_PCT_RANK { get; set; }

        [Column("opp_oreb_rank")]
        public int? OPP_OREB_RANK { get; set; }

        [Column("opp_dreb_rank")]
        public int? OPP_DREB_RANK { get; set; }

        [Column("opp_reb_rank")]
        public int? OPP_REB_RANK { get; set; }

        [Column("opp_ast_rank")]
        public int? OPP_AST_RANK { get; set; }

        [Column("opp_tov_rank")]
        public int? OPP_TOV_RANK { get; set; }

        [Column("opp_stl_rank")]
        public int? OPP_STL_RANK { get; set; }

        [Column("opp_blk_rank")]
        public int? OPP_BLK_RANK { get; set; }

        [Column("opp_blka_rank")]
        public int? OPP_BLKA_RANK { get; set; }

        [Column("opp_pf_rank")]
        public int? OPP_PF_RANK { get; set; }

        [Column("opp_pfd1")]
        public int? OPP_PFD1 { get; set; }

        [Column("opp_pts_rank")]
        public int? OPP_PTS_RANK { get; set; }

        [Column("plus_minus_rank")]
        public int? PLUS_MINUS_RANK { get; set; }
    }
}
