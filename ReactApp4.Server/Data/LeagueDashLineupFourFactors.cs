using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupFourFactors
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

        [Column("efg_pct")]
        public decimal? EFG_PCT { get; set; }

        [Column("fta_rate")]
        public decimal? FTA_RATE { get; set; }

        [Column("tm_tov_pct")]
        public decimal? TM_TOV_PCT { get; set; }

        [Column("oreb_pct")]
        public decimal? OREB_PCT { get; set; }

        [Column("opp_efg_pct")]
        public decimal? OPP_EFG_PCT { get; set; }

        [Column("opp_fta_rate")]
        public decimal? OPP_FTA_RATE { get; set; }

        [Column("opp_tov_pct")]
        public decimal? OPP_TOV_PCT { get; set; }

        [Column("opp_oreb_pct")]
        public decimal? OPP_OREB_PCT { get; set; }

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

        [Column("efg_pct_rank")]
        public int? EFG_PCT_RANK { get; set; }

        [Column("fta_rate_rank")]
        public int? FTA_RATE_RANK { get; set; }

        [Column("tm_tov_pct_rank")]
        public int? TM_TOV_PCT_RANK { get; set; }

        [Column("oreb_pct_rank")]
        public int? OREB_PCT_RANK { get; set; }

        [Column("opp_efg_pct_rank")]
        public int? OPP_EFG_PCT_RANK { get; set; }

        [Column("opp_fta_rate_rank")]
        public int? OPP_FTA_RATE_RANK { get; set; }

        [Column("opp_tov_pct_rank")]
        public int? OPP_TOV_PCT_RANK { get; set; }

        [Column("opp_oreb_pct_rank")]
        public int? OPP_OREB_PCT_RANK { get; set; }
    }
}
