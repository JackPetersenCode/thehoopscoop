using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class MLBPlayerGameInfo
    {
        [Column("game_pk")]
        public long GamePk { get; set; }

        [Column("team_side")]
        public string TeamSide { get; set; } = "";

        [Column("team_name")]
        public string TeamName { get; set; } = "";

        [Column("player_id")]
        public string PlayerId { get; set; } = "";

        [Column("person_id")]
        public int PersonId { get; set; }

        [Column("full_name")]
        public string FullName { get; set; } = "";

        [Column("boxscore_name")]
        public string BoxscoreName { get; set; } = "";

        [Column("jersey_number")]
        public string JerseyNumber { get; set; } = "";

        [Column("position")]
        public string Position { get; set; } = "";

        [Column("position_abbr")]
        public string PositionAbbr { get; set; } = "";

        [Column("status_code")]
        public string StatusCode { get; set; } = "";

        [Column("status_description")]
        public string StatusDescription { get; set; } = "";

        [Column("is_current_batter")]
        public bool IsCurrentBatter { get; set; }

        [Column("is_current_pitcher")]
        public bool IsCurrentPitcher { get; set; }

        [Column("is_on_bench")]
        public bool IsOnBench { get; set; }

        [Column("is_substitute")]
        public bool IsSubstitute { get; set; }
    }
}
