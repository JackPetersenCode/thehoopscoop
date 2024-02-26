using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class BoxScoreMiscPlayer
    {

        [Column("team_id")]
        public string? Team_id { get; set; }

        [Column("team_abbreviation")]
        public string? Team_abbreviation { get; set; }

        [Column("player_id")]
        public string? Player_id { get; set; }

        [Column("player_name")]
        public string? Player_name { get; set; }

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

        [Column("blk")]
        public decimal? Blk { get; set; }

        [Column("blka")]
        public decimal? Blka { get; set; }

        [Column("pf")]
        public decimal? Pf { get; set; }

        [Column("pfd")]
        public decimal? Pfd { get; set; }

        // Add the remaining properties as needed
    }
}
