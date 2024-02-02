using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class BoxScoreAdvanced
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

        [Column("e_off_rating")]
        public decimal? E_off_rating { get; set; }

        [Column("off_rating")]
        public decimal? Off_rating { get; set; }

        [Column("e_def_rating")]
        public decimal? E_def_rating { get; set; }

        [Column("def_rating")]
        public decimal? Def_rating { get; set; }

        [Column("e_net_rating")]
        public decimal? E_net_rating { get; set; }

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

        [Column("tm_tov_pct")]
        public decimal? Tm_tov_pct { get; set; }

        [Column("efg_pct")]
        public decimal? Efg_pct { get; set; }

        [Column("ts_pct")]
        public decimal? Ts_pct { get; set; }

        [Column("usg_pct")]
        public decimal? Usg_pct { get; set; }

        [Column("e_usg_pct")]
        public decimal? E_usg_pct { get; set; }

        [Column("e_pace")]
        public decimal? E_pace { get; set; }

        [Column("pace")]
        public decimal? Pace { get; set; }

        [Column("pace_per40")]
        public decimal? Pace_per40 { get; set; }
        
        [Column("poss")]
        public decimal? Poss { get; set; }

        [Column("pie")]
        public decimal? Pie { get; set; }

    }
}
