using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class BoxScoreTraditionalPlayer
    {

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

    }
}

