using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class BoxScoreAdvancedPlayer
    {

        [Column("player_id")]
        public string? Player_id { get; set; }

        [Column("team_id")]
        public string? Team_id { get; set; }

        [Column("player_name")]
        public string? Player_name { get; set; }

        [Column("team_abbreviation")]
        public string? Team_abbreviation { get; set; }

        [Column("team_city")]
        public string? Team_city { get; set; }

        [Column("min")]
        public decimal? Min { get; set; }

        [Column("off_rating")]
        public decimal? Off_rating { get; set; }
        
        [Column("def_rating")]
        public decimal? Def_rating { get; set; }

        [Column("net_rating")]
        public decimal? Net_rating { get; set; }

        [Column("ast_pct")]
        public decimal? Ast_pct { get; set; }

        [Column("ast_tov")]
        public decimal? Ast_tov { get; set; }
    }

}