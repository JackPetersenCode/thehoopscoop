using System.ComponentModel.DataAnnotations.Schema;
namespace ReactApp4.Server.Data
{
    public class MLBPlayerGamePitching
    {
        [Column("gamePk")]
        public int GamePk { get; set; }


        [Column("team_side")] public string TeamSide { get; set; } = string.Empty;
        [Column("team_name")] public string TeamName { get; set; } = string.Empty;
        [Column("player_id")] public string PlayerId { get; set; } = string.Empty;

        [Column("personId")]
        public int? PersonId { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [Column("summary")]
        public string? Summary { get; set; }

        [Column("gamesPlayed")]
        public double? GamesPlayed { get; set; }

        [Column("gamesStarted")]
        public double? GamesStarted { get; set; }

        [Column("flyOuts")]
        public double? FlyOuts { get; set; }

        [Column("groundOuts")]
        public double? GroundOuts { get; set; }

        [Column("airOuts")]
        public double? AirOuts { get; set; }

        [Column("runs")]
        public double? Runs { get; set; }

        [Column("doubles")]
        public double? Doubles { get; set; }

        [Column("triples")]
        public double? Triples { get; set; }

        [Column("homeRuns")]
        public double? HomeRuns { get; set; }

        [Column("strikeOuts")]
        public double? StrikeOuts { get; set; }

        [Column("baseOnBalls")]
        public double? BaseOnBalls { get; set; }

        [Column("intentionalWalks")]
        public double? IntentionalWalks { get; set; }

        [Column("hits")]
        public double? Hits { get; set; }

        [Column("hitByPitch")]
        public double? HitByPitch { get; set; }

        [Column("atBats")]
        public double? AtBats { get; set; }

        [Column("caughtStealing")]
        public double? CaughtStealing { get; set; }

        [Column("stolenBases")]
        public double? StolenBases { get; set; }

        [Column("stolenBasePercentage")]
        public double? StolenBasePercentage { get; set; }

        [Column("numberOfPitches")]
        public double? NumberOfPitches { get; set; }

        [Column("inningsPitched")]
        public double? InningsPitched { get; set; }

        [Column("wins")]
        public double? Wins { get; set; }

        [Column("losses")]
        public double? Losses { get; set; }

        [Column("saves")]
        public double? Saves { get; set; }

        [Column("saveOpportunities")]
        public double? SaveOpportunities { get; set; }

        [Column("holds")]
        public double? Holds { get; set; }

        [Column("blownSaves")]
        public double? BlownSaves { get; set; }

        [Column("earnedRuns")]
        public double? EarnedRuns { get; set; }

        [Column("battersFaced")]
        public double? BattersFaced { get; set; }

        [Column("outs")]
        public double? Outs { get; set; }

        [Column("gamesPitched")]
        public double? GamesPitched { get; set; }

        [Column("completeGames")]
        public double? CompleteGames { get; set; }

        [Column("shutouts")]
        public double? Shutouts { get; set; }

        [Column("pitchesThrown")]
        public double? PitchesThrown { get; set; }

        [Column("balls")]
        public double? Balls { get; set; }

        [Column("strikes")]
        public double? Strikes { get; set; }

        [Column("strikePercentage")]
        public double? StrikePercentage { get; set; }

        [Column("hitBatsmen")]
        public double? HitBatsmen { get; set; }

        [Column("balks")]
        public double? Balks { get; set; }

        [Column("wildPitches")]
        public double? WildPitches { get; set; }

        [Column("pickoffs")]
        public double? Pickoffs { get; set; }

        [Column("rbi")]
        public double? Rbi { get; set; }

        [Column("gamesFinished")]
        public double? GamesFinished { get; set; }

        [Column("runsScoredPer9")]
        public double? RunsScoredPer9 { get; set; }

        [Column("homeRunsPer9")]
        public double? HomeRunsPer9 { get; set; }

        [Column("inheritedRunners")]
        public double? InheritedRunners { get; set; }

        [Column("inheritedRunnersScored")]
        public double? InheritedRunnersScored { get; set; }

        [Column("catchersInterference")]
        public double? CatchersInterference { get; set; }

        [Column("sacBunts")]
        public double? SacBunts { get; set; }

        [Column("sacFlies")]
        public double? SacFlies { get; set; }

        [Column("passedBall")]
        public double? PassedBall { get; set; }

        [Column("popOuts")]
        public double? PopOuts { get; set; }

        [Column("lineOuts")]
        public double? LineOuts { get; set; }
    }
}
