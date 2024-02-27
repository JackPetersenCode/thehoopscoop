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

        [Column("ast_ratio")]
        public decimal? Ast_ratio { get; set; }

        [Column("oreb_pct")]
        public decimal? Oreb_pct { get; set; }

        [Column("dreb_pct")]
        public decimal? Dreb_pct { get; set; }

        [Column("reb_pct")]
        public decimal? Reb_pct { get; set; }

        [Column("tov_pct")]
        public decimal? Tov_pct { get; set; }

        [Column("efg_pct")]
        public decimal? Efg_pct {  get; set; }

        [Column("ts_pct")]
        public decimal? Ts_pct { get; set; }

        [Column("usg_pct")]
        public decimal? Usg_pct { get; set; }

        [Column("pie")]
        public decimal? Pie { get; set; }

        [Column("poss")]
        public decimal? Poss { get; set; }
    }

}