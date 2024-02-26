using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class BoxScoreFourFactorsPlayer
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

        [Column("efg_pct")]
        public decimal? Efg_pct { get; set; }

        [Column("fta_rate")]
        public decimal? Fta_rate { get; set; }

        [Column("tov_pct")]
        public decimal? Tov_pct { get; set; }

        [Column("oreb_pct")]
        public decimal? Oreb_pct { get; set; }

    }
}
