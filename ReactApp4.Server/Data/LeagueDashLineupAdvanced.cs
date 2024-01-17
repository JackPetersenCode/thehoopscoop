using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupAdvanced
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
        public decimal? Gp { get; set; }

        [Column("w")]
        public decimal? W { get; set; }

        [Column("l")]
        public decimal? L { get; set; }

        [Column("w_pct")]
        public decimal? W_pct { get; set; }

        [Column("min")]
        public decimal? Min { get; set; }

        [Column("e_off_rating")]
        public decimal? E_off_rating { get; set; }

        [Column("off_rating")]
        public decimal? Off_rating { get; set; }

        [Column("e_def_rating")]
        public decimal? E_def_rating { get; set; }

        [Column("def_rating")]
        public decimal? Def_rating { get; set; }

        [Column("e_net_rating")]
        public decimal? E_net_rating { get; set; }

        [Column("net_rating")]
        public decimal? Net_rating { get; set; }

        [Column("ast_pct")]
        public decimal? Ast_pct { get; set; }

        [Column("ast_to")]
        public decimal? Ast_to { get; set; }

        [Column("ast_ratio")]
        public decimal? Ast_ratio { get; set; }

        [Column("oreb_pct")]
        public decimal? Oreb_pct { get; set; }

        [Column("dreb_pct")]
        public decimal? Dreb_pct { get; set; }

        [Column("reb_pct")]
        public decimal? Reb_pct { get; set; }

        [Column("tm_tov_pct")]
        public decimal? Tm_tov_pct { get; set; }

        [Column("efg_pct")]
        public decimal? Efg_pct { get; set; }

        [Column("ts_pct")]
        public decimal? Ts_pct { get; set; }

        [Column("e_pace")]
        public decimal? E_pace { get; set; }

        [Column("pace")]
        public decimal? Pace { get; set; }

        [Column("pace_per40")]
        public decimal? Pace_per40 { get; set; }

        [Column("poss")]
        public decimal? Poss { get; set; }

        [Column("pie")]
        public decimal? Pie { get; set; }

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

        [Column("off_rating_rank")]
        public int? Off_rating_rank { get; set; }

        [Column("def_rating_rank")]
        public int? Def_rating_rank { get; set; }

        [Column("net_rating_rank")]
        public int? Net_rating_rank { get; set; }

        [Column("ast_pct_rank")]
        public int? Ast_pct_rank { get; set; }

        [Column("ast_to_rank")]
        public int? Ast_to_rank { get; set; }

        [Column("ast_ratio_rank")]
        public int? Ast_ratio_rank { get; set; }

        [Column("oreb_pct_rank")]
        public int? Oreb_pct_rank { get; set; }

        [Column("dreb_pct_rank")]
        public int? Dreb_pct_rank { get; set; }

        [Column("reb_pct_rank")]
        public int? Reb_pct_rank { get; set; }

        [Column("tm_tov_pct_rank")]
        public int? Tm_tov_pct_rank { get; set; }

        [Column("efg_pct_rank")]
        public int? Efg_pct_rank { get; set; }

        [Column("ts_pct_rank")]
        public int? Ts_pct_rank { get; set; }

        [Column("pace_rank")]
        public int? Pace_rank { get; set; }

        [Column("pie_rank")]
        public int? Pie_rank { get; set; }
    }
}
