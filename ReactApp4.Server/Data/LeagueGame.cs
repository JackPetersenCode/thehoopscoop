using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class LeagueGame
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("season_id")]
        public string Season_id { get; set; }

        [Column("team_id")]
        public string Team_id { get; set; }

        [Column("team_abbreviation")]
        public string Team_abbreviation { get; set; }

        [Column("team_name")]
        public string Team_name { get; set; }

        [Column("game_id")]
        public string Game_id { get; set; }

        [Column("game_date")]
        public string Game_date { get; set; }

        [Column("matchup")]
        public string Matchup { get; set; }

        [Column("wl")]
        public string Wl { get; set; }

        [Column("min")]
        public decimal? Min { get; set; }

        [Column("fgm")]
        public decimal? Fgm { get; set; }

        [Column("fga")]
        public decimal? Fga { get; set; }

        [Column("fg_pct")]
        public decimal? Fg_pct { get; set; }

        [Column("fg3m")]
        public decimal? Fg3m { get; set; }

        [Column("fg3a")]
        public decimal? Fg3a { get; set; }

        [Column("fg3_pct")]
        public decimal? Fg3_pct { get; set; }

        [Column("ftm")]
        public decimal? Ftm { get; set; }

        [Column("fta")]
        public decimal? Fta { get; set; }

        [Column("ft_pct")]
        public decimal? Ft_pct { get; set; }

        [Column("oreb")]
        public decimal? Oreb { get; set; }

        [Column("dreb")]
        public decimal? Dreb { get; set; }

        [Column("reb")]
        public decimal? Reb { get; set; }

        [Column("ast")]
        public decimal? Ast { get; set; }

        [Column("stl")]
        public decimal? Stl { get; set; }

        [Column("blk")]
        public decimal? Blk { get; set; }

        [Column("tov")]
        public decimal? Tov { get; set; }

        [Column("pf")]
        public decimal? Pf { get; set; }

        [Column("pts")]
        public decimal? Pts { get; set; }

        [Column("plus_minus")]
        public decimal? Plus_minus { get; set; }

        [Column("video_available")]
        public string Video_available { get; set; }
    }
}
