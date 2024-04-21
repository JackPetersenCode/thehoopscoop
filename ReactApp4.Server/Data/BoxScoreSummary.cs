using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class BoxScoreSummary
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("game_date_est")]
        public string? Game_date_est { get; set; }

        [Column("game_sequence")]
        public string? Game_sequence { get; set; }

        [Column("game_id")]
        public string? Game_id { get; set; }

        [Column("game_status_id")]
        public string? Game_status_id { get; set; }

        [Column("game_status_text")]
        public string? Game_status_text { get; set; }

        [Column("gamecode")]
        public string? Gamecode { get; set; }

        [Column("home_team_id")]
        public string? Home_team_id { get; set; }

        [Column("visitor_team_id")]
        public string? Visitor_team_id { get; set; }

        [Column("season")]
        public string? Season { get; set; }

        [Column("live_period")]
        public string? Live_period { get; set; }

        [Column("live_pc_time")]
        public string? Live_pc_time { get; set; }

        [Column("natl_tv_broadcaster_abbreviation")]
        public string? Natl_tv_broadcaster_abbreviation { get; set; }

        [Column("live_period_time_bcast")]
        public string? Live_period_time_bcast { get; set; }

        [Column("wh_status")]
        public string? Wh_status { get; set; }

    }
}
