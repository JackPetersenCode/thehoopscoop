using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class BoxScoreFourFactors
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("game_id")]
        public string? Game_id { get; set; }

        [Column("team_id")]
        public string? Team_id { get; set; }

        [Column("team_abbreviation")]
        public string? Team_abbreviation { get; set; }

        [Column("team_city")]
        public string? Team_city { get; set; }

        [Column("player_id")]
        public string? Player_id { get; set; }

        [Column("player_name")]
        public string? Player_name { get; set; }

        [Column("nickname")]
        public string? Nickname { get; set; }

        [Column("start_position")]
        public string? Start_position { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [Column("min")]
        public decimal? Min { get; set; }

        [Column("efg_pct")]
        public decimal? Efg_pct { get; set; }

        [Column("fta_rate")]
        public decimal? Fta_rate { get; set; }

        [Column("tm_tov_pct")]
        public decimal? Tm_tov_pct { get; set; }

        [Column("oreb_pct")]
        public decimal? Oreb_pct { get; set; }

        [Column("opp_efg_pct")]
        public decimal? Opp_efg_pct { get; set; }

        [Column("opp_fta_rate")]
        public decimal? Opp_fta_rate { get; set; }

        [Column("opp_tov_pct")]
        public decimal? Opp_tov_pct { get; set; }

        [Column("opp_oreb_pct")]
        public decimal? Opp_oreb_pct { get; set; }

        // Add the remaining properties as needed
    }
}
