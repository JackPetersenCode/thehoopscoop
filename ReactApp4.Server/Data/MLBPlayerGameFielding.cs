using System.ComponentModel.DataAnnotations.Schema;
namespace ReactApp4.Server.Data
{
    public class MLBPlayerGameFielding
    {
        [Column("game_pk")]
        public int GamePk { get; set; }
        [Column("team_side")] public string TeamSide { get; set; } = string.Empty;
        [Column("team_name")] public string TeamName { get; set; } = string.Empty;
        [Column("player_id")] public string PlayerId { get; set; } = string.Empty;

        [Column("person_id")]
        public int? PersonId { get; set; }

        [Column("caught_stealing")]
        public double? CaughtStealing { get; set; }

        [Column("stolen_bases")]
        public double? StolenBases { get; set; }

        [Column("stolen_base_percentage")]
        public double? StolenBasePercentage { get; set; }

        [Column("assists")]
        public double? Assists { get; set; }

        [Column("put_outs")]
        public double? PutOuts { get; set; }

        [Column("errors")]
        public double? Errors { get; set; }

        [Column("chances")]
        public double? Chances { get; set; }

        [Column("fielding")]
        public double? Fielding { get; set; }

        [Column("passed_ball")]
        public double? PassedBall { get; set; }

        [Column("pickoffs")]
        public double? Pickoffs { get; set; }

        [Column("games_started")]
        public double? GamesStarted { get; set; }
    }
}   