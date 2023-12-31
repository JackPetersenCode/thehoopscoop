using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupAdvanced
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

        [Column("e_off_rating")]
        public decimal? E_OFF_RATING { get; set; }

        [Column("off_rating")]
        public decimal? OFF_RATING { get; set; }

        [Column("e_def_rating")]
        public decimal? E_DEF_RATING { get; set; }

        [Column("def_rating")]
        public decimal? DEF_RATING { get; set; }

        [Column("e_net_rating")]
        public decimal? E_NET_RATING { get; set; }

        [Column("net_rating")]
        public decimal? NET_RATING { get; set; }

        [Column("ast_pct")]
        public decimal? AST_PCT { get; set; }

        [Column("ast_to")]
        public decimal? AST_TO { get; set; }

        [Column("ast_ratio")]
        public decimal? AST_RATIO { get; set; }

        [Column("oreb_pct")]
        public decimal? OREB_PCT { get; set; }

        [Column("dreb_pct")]
        public decimal? DREB_PCT { get; set; }

        [Column("reb_pct")]
        public decimal? REB_PCT { get; set; }

        [Column("tm_tov_pct")]
        public decimal? TM_TOV_PCT { get; set; }

        [Column("efg_pct")]
        public decimal? EFG_PCT { get; set; }

        [Column("ts_pct")]
        public decimal? TS_PCT { get; set; }

        [Column("e_pace")]
        public decimal? E_PACE { get; set; }

        [Column("pace")]
        public decimal? PACE { get; set; }

        [Column("pace_per40")]
        public decimal? PACE_PER40 { get; set; }

        [Column("poss")]
        public decimal? POSS { get; set; }

        [Column("pie")]
        public decimal? PIE { get; set; }

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

        [Column("off_rating_rank")]
        public int? OFF_RATING_RANK { get; set; }

        [Column("def_rating_rank")]
        public int? DEF_RATING_RANK { get; set; }

        [Column("net_rating_rank")]
        public int? NET_RATING_RANK { get; set; }

        [Column("ast_pct_rank")]
        public int? AST_PCT_RANK { get; set; }

        [Column("ast_to_rank")]
        public int? AST_TO_RANK { get; set; }

        [Column("ast_ratio_rank")]
        public int? AST_RATIO_RANK { get; set; }

        [Column("oreb_pct_rank")]
        public int? OREB_PCT_RANK { get; set; }

        [Column("dreb_pct_rank")]
        public int? DREB_PCT_RANK { get; set; }

        [Column("reb_pct_rank")]
        public int? REB_PCT_RANK { get; set; }

        [Column("tm_tov_pct_rank")]
        public int? TM_TOV_PCT_RANK { get; set; }

        [Column("efg_pct_rank")]
        public int? EFG_PCT_RANK { get; set; }

        [Column("ts_pct_rank")]
        public int? TS_PCT_RANK { get; set; }

        [Column("pace_rank")]
        public int? PACE_RANK { get; set; }

        [Column("pie_rank")]
        public int? PIE_RANK { get; set; }
    }
}
