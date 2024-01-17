using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupBase
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

        [Column("fgm")]
        public decimal? Fgm { get; set; }

        [Column("fga")]
        public decimal? Fga { get; set; }

        [Column("fg_pct")]
        public decimal? Fg_pct { get; set; }

        [Column("fg3m")]
        public decimal? Fg3m { get; set; }

        [Column("fg3a")]
        public decimal? Fg3a { get; set; }

        [Column("fg3_pct")]
        public decimal? Fg3_pct { get; set; }

        [Column("ftm")]
        public decimal? Ftm { get; set; }

        [Column("fta")]
        public decimal? Fta { get; set; }

        [Column("ft_pct")]
        public decimal? Ft_pct { get; set; }

        [Column("oreb")]
        public decimal? Oreb { get; set; }

        [Column("dreb")]
        public decimal? Dreb { get; set; }

        [Column("reb")]
        public decimal? Reb { get; set; }

        [Column("ast")]
        public decimal? Ast { get; set; }

        [Column("tov")]
        public decimal? Tov { get; set; }

        [Column("stl")]
        public decimal? Stl { get; set; }

        [Column("blk")]
        public decimal? Blk { get; set; }

        [Column("blka")]
        public decimal? Blka { get; set; }

        [Column("pf")]
        public decimal? Pf { get; set; }

        [Column("pfd")]
        public decimal? Pfd { get; set; }

        [Column("pts")]
        public decimal? Pts { get; set; }

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

        [Column("fgm_rank")]
        public int? Fgm_rank { get; set; }

        [Column("fga_rank")]
        public int? Fga_rank { get; set; }

        [Column("fg_pct_rank")]
        public int? Fg_pct_rank { get; set; }

        [Column("fg3m_rank")]
        public int? Fg3m_rank { get; set; }

        [Column("fg3a_rank")]
        public int? Fg3a_rank { get; set; }

        [Column("fg3_pct_rank")]
        public int? Fg3_pct_rank { get; set; }

        [Column("ftm_rank")]
        public int? Ftm_rank { get; set; }

        [Column("fta_rank")]
        public int? Fta_rank { get; set; }

        [Column("ft_pct_rank")]
        public int? Ft_pct_rank { get; set; }

        [Column("oreb_rank")]
        public int? Oreb_rank { get; set; }

        [Column("dreb_rank")]
        public int? Dreb_rank { get; set; }

        [Column("reb_rank")]
        public int? Reb_rank { get; set; }

        [Column("ast_rank")]
        public int? Ast_rank { get; set; }

        [Column("tov_rank")]
        public int? Tov_rank { get; set; }

        [Column("stl_rank")]
        public int? Stl_rank { get; set; }

        [Column("blk_rank")]
        public int? Blk_rank { get; set; }

        [Column("blka_rank")]
        public int? Blka_rank { get; set; }

        [Column("pf_rank")]
        public int? Pf_rank { get; set; }

        [Column("pfd_rank")]
        public int? Pfd_rank { get; set; }

        [Column("pts_rank")]
        public int? Pts_rank { get; set; }

        [Column("plus_minus_rank")]
        public int? Plus_minus_rank { get; set; }
    }
}
