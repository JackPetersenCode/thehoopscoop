using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupScoring
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

        [Column("pct_fga_2pt")]
        public decimal? PCT_FGA_2PT { get; set; }

        [Column("pct_fga_3pt")]
        public decimal? PCT_FGA_3PT { get; set; }

        [Column("pct_pts_2pt")]
        public decimal? PCT_PTS_2PT { get; set; }

        [Column("pct_pts_2pt_mr")]
        public decimal? PCT_PTS_2PT_MR { get; set; }

        [Column("pct_pts_3pt")]
        public decimal? PCT_PTS_3PT { get; set; }

        [Column("pct_pts_fb")]
        public decimal? PCT_PTS_FB { get; set; }

        [Column("pct_pts_ft")]
        public decimal? PCT_PTS_FT { get; set; }

        [Column("pct_pts_off_tov")]
        public decimal? PCT_PTS_OFF_TOV { get; set; }

        [Column("pct_pts_paint")]
        public decimal? PCT_PTS_PAINT { get; set; }

        [Column("pct_ast_2pm")]
        public decimal? PCT_AST_2PM { get; set; }

        [Column("pct_uast_2pm")]
        public decimal? PCT_UAST_2PM { get; set; }

        [Column("pct_ast_3pm")]
        public decimal? PCT_AST_3PM { get; set; }

        [Column("pct_uast_3pm")]
        public decimal? PCT_UAST_3PM { get; set; }

        [Column("pct_ast_fgm")]
        public decimal? PCT_AST_FGM { get; set; }

        [Column("pct_uast_fgm")]
        public decimal? PCT_UAST_FGM { get; set; }

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

        [Column("pct_fga_2pt_rank")]
        public int? PCT_FGA_2PT_RANK { get; set; }

        [Column("pct_fga_3pt_rank")]
        public int? PCT_FGA_3PT_RANK { get; set; }

        [Column("pct_pts_2pt_rank")]
        public int? PCT_PTS_2PT_RANK { get; set; }

        [Column("pct_pts_2pt_mr_rank")]
        public int? PCT_PTS_2PT_MR_RANK { get; set; }

        [Column("pct_pts_3pt_rank")]
        public int? PCT_PTS_3PT_RANK { get; set; }

        [Column("pct_pts_fb_rank")]
        public int? PCT_PTS_FB_RANK { get; set; }

        [Column("pct_pts_ft_rank")]
        public int? PCT_PTS_FT_RANK { get; set; }

        [Column("pct_pts_off_tov_rank")]
        public int? PCT_PTS_OFF_TOV_RANK { get; set; }

        [Column("pct_pts_paint_rank")]
        public int? PCT_PTS_PAINT_RANK { get; set; }

        [Column("pct_ast_2pm_rank")]
        public int? PCT_AST_2PM_RANK { get; set; }

        [Column("pct_uast_2pm_rank")]
        public int? PCT_UAST_2PM_RANK { get; set; }

        [Column("pct_ast_3pm_rank")]
        public int? PCT_AST_3PM_RANK { get; set; }

        [Column("pct_uast_3pm_rank")]
        public int? PCT_UAST_3PM_RANK { get; set; }

        [Column("pct_ast_fgm_rank")]
        public int? PCT_AST_FGM_RANK { get; set; }

        [Column("pct_uast_fgm_rank")]
        public int? PCT_UAST_FGM_RANK { get; set; }
    }
}
