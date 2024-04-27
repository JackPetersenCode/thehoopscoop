using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class ExpectedMatchup
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("game_date")]
        public string? Game_date { get; set; }

        [Column("matchup")]
        public string? Matchup { get; set; }

        [Column("home_team")]
        public string? Home_team { get; set; }

        [Column("home_team_id")]
        public string? Home_team_id { get; set; }

        [Column("home_expected")]
        public decimal? Home_expected { get; set; }

        [Column("visitor_team")]
        public string? Visitor_team { get; set; }
        [Column("home_team_id")]
        public string? Visitor_team_id { get; set; }

        [Column("visitor_expected")]
        public decimal? Visitor_expected { get; set; }
                
        [Column("home_actual")]
        public decimal? Home_actual { get; set; }

        [Column("visitor_actual")]
        public decimal? Visitor_actual { get; set; }

        [Column("home_odds")]
        public string? Home_odds { get; set; }

        [Column("visitor_odds")]
        public string? Visitor_odds { get; set; }

        [Column("green_red")]
        public string? Green_red { get; set; }
    }
}

