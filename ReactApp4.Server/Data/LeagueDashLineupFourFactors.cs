using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupFourFactors
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

        [Column("efg_pct")]
        public decimal? Efg_pct { get; set; }

        [Column("fta_rate")]
        public decimal? Fta_rate { get; set; }

        [Column("tm_tov_pct")]
        public decimal? Tm_tov_pct { get; set; }

        [Column("oreb_pct")]
        public decimal? Oreb_pct { get; set; }

        [Column("opp_efg_pct")]
        public decimal? Opp_efg_pct { get; set; }

        [Column("opp_fta_rate")]
        public decimal? Opp_fta_rate { get; set; }

        [Column("opp_tov_pct")]
        public decimal? Opp_tov_pct { get; set; }

        [Column("opp_oreb_pct")]
        public decimal? Opp_oreb_pct { get; set; }

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

        [Column("efg_pct_rank")]
        public int? Efg_pct_rank { get; set; }

        [Column("fta_rate_rank")]
        public int? Fta_rate_rank { get; set; }

        [Column("tm_tov_pct_rank")]
        public int? Tm_tov_pct_rank { get; set; }

        [Column("oreb_pct_rank")]
        public int? Oreb_pct_rank { get; set; }

        [Column("opp_efg_pct_rank")]
        public int? Opp_efg_pct_rank { get; set; }

        [Column("opp_fta_rate_rank")]
        public int? Opp_fta_rate_rank { get; set; }

        [Column("opp_tov_pct_rank")]
        public int? Opp_tov_pct_rank { get; set; }

        [Column("opp_oreb_pct_rank")]
        public int? Opp_oreb_pct_rank { get; set; }
    }
}
