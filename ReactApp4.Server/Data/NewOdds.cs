using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class NewOdds
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("game_id")]
        public string? Game_id { get; set; }

        [Column("commence_time")]
        public string? Commence_time { get; set; }

        [Column("home_team")]
        public string? Home_team { get; set; }

        [Column("away_team")]
        public string? Away_team { get; set; }

        [Column("home_odds")]
        public string? Home_odds { get; set; }

        [Column("away_odds")]
        public string? Away_odds { get; set; }

    }
}

