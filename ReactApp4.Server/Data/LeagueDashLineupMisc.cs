using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueDashLineupMisc
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

        [Column("pts_off_tov")]
        public decimal? Pts_off_tov { get; set; }

        [Column("pts_2nd_chance")]
        public decimal? Pts_2nd_chance { get; set; }

        [Column("pts_fb")]
        public decimal? Pts_fb { get; set; }

        [Column("pts_paint")]
        public decimal? Pts_paint { get; set; }

        [Column("opp_pts_off_tov")]
        public decimal? Opp_pts_off_tov { get; set; }

        [Column("opp_pts_2nd_chance")]
        public decimal? Opp_pts_2nd_chance { get; set; }

        [Column("opp_pts_fb")]
        public decimal? Opp_pts_fb { get; set; }

        [Column("opp_pts_paint")]
        public decimal? Opp_pts_paint { get; set; }

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

        [Column("pts_off_tov_rank")]
        public int? Pts_off_tov_rank { get; set; }

        [Column("pts_2nd_chance_rank")]
        public int? Pts_2nd_chance_rank { get; set; }

        [Column("pts_fb_rank")]
        public int? Pts_fb_rank { get; set; }

        [Column("pts_paint_rank")]
        public int? Pts_paint_rank { get; set; }

        [Column("opp_pts_off_tov_rank")]
        public int? Opp_pts_off_tov_rank { get; set; }

        [Column("opp_pts_2nd_chance_rank")]
        public int? Opp_pts_2nd_chance_rank { get; set; }

        [Column("opp_pts_fb_rank")]
        public int? Opp_pts_fb_rank { get; set; }

        [Column("opp_pts_paint_rank")]
        public int? Opp_pts_paint_rank { get; set; }
    }
}
