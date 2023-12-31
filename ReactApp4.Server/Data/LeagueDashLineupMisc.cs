using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupMisc
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

        [Column("pts_off_tov")]
        public decimal? PTS_OFF_TOV { get; set; }

        [Column("pts_2nd_chance")]
        public decimal? PTS_2ND_CHANCE { get; set; }

        [Column("pts_fb")]
        public decimal? PTS_FB { get; set; }

        [Column("pts_paint")]
        public decimal? PTS_PAINT { get; set; }

        [Column("opp_pts_off_tov")]
        public decimal? OPP_PTS_OFF_TOV { get; set; }

        [Column("opp_pts_2nd_chance")]
        public decimal? OPP_PTS_2ND_CHANCE { get; set; }

        [Column("opp_pts_fb")]
        public decimal? OPP_PTS_FB { get; set; }

        [Column("opp_pts_paint")]
        public decimal? OPP_PTS_PAINT { get; set; }

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

        [Column("pts_off_tov_rank")]
        public int? PTS_OFF_TOV_RANK { get; set; }

        [Column("pts_2nd_chance_rank")]
        public int? PTS_2ND_CHANCE_RANK { get; set; }

        [Column("pts_fb_rank")]
        public int? PTS_FB_RANK { get; set; }

        [Column("pts_paint_rank")]
        public int? PTS_PAINT_RANK { get; set; }

        [Column("opp_pts_off_tov_rank")]
        public int? OPP_PTS_OFF_TOV_RANK { get; set; }

        [Column("opp_pts_2nd_chance_rank")]
        public int? OPP_PTS_2ND_CHANCE_RANK { get; set; }

        [Column("opp_pts_fb_rank")]
        public int? OPP_PTS_FB_RANK { get; set; }

        [Column("opp_pts_paint_rank")]
        public int? OPP_PTS_PAINT_RANK { get; set; }
    }
}
