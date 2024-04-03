using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class Shot
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("grid_type")]
        public string? Grid_type { get; set; }

        [Column("game_id")]
        public string? Game_id { get; set; }

        [Column("game_event_id")]
        public string? Game_event_id { get; set; }

        [Column("player_id")]
        public string? Player_id { get; set; }

        [Column("player_name")]
        public string? Player_name { get; set; }

        [Column("team_id")]
        public string? Team_id { get; set; }

        [Column("team_name")]
        public string? Team_name { get; set; }

        [Column("period")]
        public string? Period { get; set; }

        [Column("minutes_remaining")]
        public string? Minutes_remaining { get; set; }

        [Column("seconds_remaining")]
        public string? Seconds_remaining { get; set; }

        [Column("event_type")]
        public string? Event_type { get; set; }

        [Column("action_type")]
        public string? Action_type { get; set; }

        [Column("shot_type")]
        public string? Shot_type { get; set; }

        [Column("shot_zone_basic")]
        public string? Shot_zone_basic { get; set; }

        [Column("shot_zone_area")]
        public string? Shot_zone_area { get; set; }

        [Column("shot_zone_range")]
        public string? Shot_zone_range { get; set; }

        [Column("shot_distance")]
        public string? Shot_distance { get; set; }

        [Column("loc_x")]
        public string? Loc_x { get; set; }

        [Column("loc_y")]
        public string? Loc_y { get; set; }

        [Column("shot_attempted_flag")]
        public string? Shot_attempted_flag { get; set; }

        [Column("shot_made_flag")]
        public string? Shot_made_flag { get; set; }

        [Column("game_date")]
        public string? Game_date { get; set; }

        [Column("htm")]
        public string? Htm { get; set; }

        [Column("vtm")]
        public string? Vtm { get; set; }
    }
}
