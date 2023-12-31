using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupBase
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

        [Column("fgm")]
        public decimal? FGM { get; set; }

        [Column("fga")]
        public decimal? FGA { get; set; }

        [Column("fg_pct")]
        public decimal? FG_PCT { get; set; }

        [Column("fg3m")]
        public decimal? FG3M { get; set; }

        [Column("fg3a")]
        public decimal? FG3A { get; set; }

        [Column("fg3_pct")]
        public decimal? FG3_PCT { get; set; }

        [Column("ftm")]
        public decimal? FTM { get; set; }

        [Column("fta")]
        public decimal? FTA { get; set; }

        [Column("ft_pct")]
        public decimal? FT_PCT { get; set; }

        [Column("oreb")]
        public decimal? OREB { get; set; }

        [Column("dreb")]
        public decimal? DREB { get; set; }

        [Column("reb")]
        public decimal? REB { get; set; }

        [Column("ast")]
        public decimal? AST { get; set; }

        [Column("tov")]
        public decimal? TOV { get; set; }

        [Column("stl")]
        public decimal? STL { get; set; }

        [Column("blk")]
        public decimal? BLK { get; set; }

        [Column("blka")]
        public decimal? BLKA { get; set; }

        [Column("pf")]
        public decimal? PF { get; set; }

        [Column("pfd")]
        public decimal? PFD { get; set; }

        [Column("pts")]
        public decimal? PTS { get; set; }

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

        [Column("fgm_rank")]
        public int? FGM_RANK { get; set; }

        [Column("fga_rank")]
        public int? FGA_RANK { get; set; }

        [Column("fg_pct_rank")]
        public int? FG_PCT_RANK { get; set; }

        [Column("fg3m_rank")]
        public int? FG3M_RANK { get; set; }

        [Column("fg3a_rank")]
        public int? FG3A_RANK { get; set; }

        [Column("fg3_pct_rank")]
        public int? FG3_PCT_RANK { get; set; }

        [Column("ftm_rank")]
        public int? FTM_RANK { get; set; }

        [Column("fta_rank")]
        public int? FTA_RANK { get; set; }

        [Column("ft_pct_rank")]
        public int? FT_PCT_RANK { get; set; }

        [Column("oreb_rank")]
        public int? OREB_RANK { get; set; }

        [Column("dreb_rank")]
        public int? DREB_RANK { get; set; }

        [Column("reb_rank")]
        public int? REB_RANK { get; set; }

        [Column("ast_rank")]
        public int? AST_RANK { get; set; }

        [Column("tov_rank")]
        public int? TOV_RANK { get; set; }

        [Column("stl_rank")]
        public int? STL_RANK { get; set; }

        [Column("blk_rank")]
        public int? BLK_RANK { get; set; }

        [Column("blka_rank")]
        public int? BLKA_RANK { get; set; }

        [Column("pf_rank")]
        public int? PF_RANK { get; set; }

        [Column("pfd_rank")]
        public int? PFD_RANK { get; set; }

        [Column("pts_rank")]
        public int? PTS_RANK { get; set; }

        [Column("plus_minus_rank")]
        public int? PLUS_MINUS_RANK { get; set; }
    }
}
