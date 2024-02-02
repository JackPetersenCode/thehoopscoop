using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class BoxScoreScoring
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

        [Column("pct_fga_2pt")]
        public decimal? Pct_fga_2pt { get; set; }

        [Column("pct_fga_3pt")]
        public decimal? Pct_fga_3pt { get; set; }

        [Column("pct_pts_2pt")]
        public decimal? Pct_pts_2pt { get; set; }

        [Column("pct_pts_2pt_mr")]
        public decimal? Pct_pts_2pt_mr { get; set; }

        [Column("pct_pts_3pt")]
        public decimal? Pct_pts_3pt { get; set; }

        [Column("pct_pts_fb")]
        public decimal? Pct_pts_fb { get; set; }

        [Column("pct_pts_ft")]
        public decimal? Pct_pts_ft { get; set; }

        [Column("pct_pts_off_tov")]
        public decimal? Pct_pts_off_tov { get; set; }

        [Column("pct_pts_paint")]
        public decimal? Pct_pts_paint { get; set; }

        [Column("pct_ast_2pm")]
        public decimal? Pct_ast_2pm { get; set; }

        [Column("pct_uast_2pm")]
        public decimal? Pct_uast_2pm { get; set; }

        [Column("pct_ast_3pm")]
        public decimal? Pct_ast_3pm { get; set; }

        [Column("pct_uast_3pm")]
        public decimal? Pct_uast_3pm { get; set; }

        [Column("pct_ast_fgm")]
        public decimal? Pct_ast_fgm { get; set; }
    }
}
