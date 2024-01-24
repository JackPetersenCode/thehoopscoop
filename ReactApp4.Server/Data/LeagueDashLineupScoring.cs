using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupScoring
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

        [Column("pct_fga_2pt")]
        public decimal? Pct_fga_2pt { get; set; }

        [Column("pct_fga_3pt")]
        public decimal? Pct_fga_3pt { get; set; }

        [Column("pct_pts_2pt")]
        public decimal? Pct_pts_2pt { get; set; }

        [Column("pct_pts_2pt_mr")]
        public decimal? Pct_pts_2pt_mr { get; set; }

        [Column("pct_pts_3pt")]
        public decimal? Pct_pts_3pt { get; set; }

        [Column("pct_pts_fb")]
        public decimal? Pct_pts_fb { get; set; }

        [Column("pct_pts_ft")]
        public decimal? Pct_pts_ft { get; set; }

        [Column("pct_pts_off_tov")]
        public decimal? Pct_pts_off_tov { get; set; }

        [Column("pct_pts_paint")]
        public decimal? Pct_pts_paint { get; set; }

        [Column("pct_ast_2pm")]
        public decimal? Pct_ast_2pm { get; set; }

        [Column("pct_uast_2pm")]
        public decimal? Pct_uast_2pm { get; set; }

        [Column("pct_ast_3pm")]
        public decimal? Pct_ast_3pm { get; set; }

        [Column("pct_uast_3pm")]
        public decimal? Pct_uast_3pm { get; set; }

        [Column("pct_ast_fgm")]
        public decimal? Pct_ast_fgm { get; set; }

        [Column("pct_uast_fgm")]
        public decimal? Pct_uast_fgm { get; set; }

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

        [Column("pct_fga_2pt_rank")]
        public int? Pct_fga_2pt_rank { get; set; }

        [Column("pct_fga_3pt_rank")]
        public int? Pct_fga_3pt_rank { get; set; }

        [Column("pct_pts_2pt_rank")]
        public int? Pct_pts_2pt_rank { get; set; }

        [Column("pct_pts_2pt_mr_rank")]
        public int? Pct_pts_2pt_mr_rank { get; set; }

        [Column("pct_pts_3pt_rank")]
        public int? Pct_pts_3pt_rank { get; set; }

        [Column("pct_pts_fb_rank")]
        public int? Pct_pts_fb_rank { get; set; }

        [Column("pct_pts_ft_rank")]
        public int? Pct_pts_ft_rank { get; set; }

        [Column("pct_pts_off_tov_rank")]
        public int? Pct_pts_off_tov_rank { get; set; }

        [Column("pct_pts_paint_rank")]
        public int? Pct_pts_paint_rank { get; set; }

        [Column("pct_ast_2pm_rank")]
        public int? Pct_ast_2pm_rank { get; set; }

        [Column("pct_uast_2pm_rank")]
        public int? Pct_uast_2pm_rank { get; set; }

        [Column("pct_ast_3pm_rank")]
        public int? Pct_ast_3pm_rank { get; set; }

        [Column("pct_uast_3pm_rank")]
        public int? Pct_uast_3pm_rank { get; set; }

        [Column("pct_ast_fgm_rank")]
        public int? Pct_ast_fgm_rank { get; set; }

        [Column("pct_uast_fgm_rank")]
        public int? Pct_uast_fgm_rank { get; set; }
    }
}
