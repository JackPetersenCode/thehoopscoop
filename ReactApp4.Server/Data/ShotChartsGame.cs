using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class ShotChartsGame
    {
        [Column("game_id")]
        public string? Game_id { get; set; }

        [Column("game_date")]
        public string? Game_date { get; set; }

        [Column("matchup")]
        public string? Matchup { get; set; }
    }
}
