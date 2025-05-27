using System.ComponentModel.DataAnnotations.Schema;
namespace ReactApp4.Server.Data
{
    public class MLBPitchingBoxScoreWithGameDate
    {
        [Column("game_pk")] public int GamePk { get; set; }
        [Column("team_side")] public string TeamSide { get; set; } = string.Empty;
        [Column("team_name")] public string TeamName { get; set; } = string.Empty;
        [Column("team_id")] public int? TeamId { get; set; }
        [Column("home_team_id")] public int? HomeTeamId { get; set; }
        [Column("away_team_id")] public int? AwayTeamId { get; set; }
        [Column("player_id")] public string PlayerId { get; set; } = string.Empty;

        [Column("person_id")]
        public int? PersonId { get; set; }

        [Column("note")]
        public string? Note { get; set; }

        [Column("summary")]
        public string? Summary { get; set; }

        [Column("games_played")]
        public double? GamesPlayed { get; set; }

        [Column("games_started")]
        public double? GamesStarted { get; set; }

        [Column("fly_outs")]
        public double? FlyOuts { get; set; }

        [Column("ground_outs")]
        public double? GroundOuts { get; set; }

        [Column("air_outs")]
        public double? AirOuts { get; set; }

        [Column("runs")]
        public double? Runs { get; set; }

        [Column("doubles")]
        public double? Doubles { get; set; }

        [Column("triples")]
        public double? Triples { get; set; }

        [Column("home_runs")]
        public double? HomeRuns { get; set; }

        [Column("strike_outs")]
        public double? StrikeOuts { get; set; }

        [Column("base_on_balls")]
        public double? BaseOnBalls { get; set; }

        [Column("intentional_walks")]
        public double? IntentionalWalks { get; set; }

        [Column("hits")]
        public double? Hits { get; set; }

        [Column("hit_by_pitch")]
        public double? HitByPitch { get; set; }

        [Column("at_bats")]
        public double? AtBats { get; set; }

        [Column("caught_stealing")]
        public double? CaughtStealing { get; set; }

        [Column("stolen_bases")]
        public double? StolenBases { get; set; }

        [Column("stolen_base_percentage")]
        public double? StolenBasePercentage { get; set; }

        [Column("number_of_pitches")]
        public double? NumberOfPitches { get; set; }

        [Column("innings_pitched")]
        public double? InningsPitched { get; set; }

        [Column("wins")]
        public double? Wins { get; set; }

        [Column("losses")]
        public double? Losses { get; set; }

        [Column("saves")]
        public double? Saves { get; set; }

        [Column("save_opportunities")]
        public double? SaveOpportunities { get; set; }

        [Column("holds")]
        public double? Holds { get; set; }

        [Column("blown_saves")]
        public double? BlownSaves { get; set; }

        [Column("earned_runs")]
        public double? EarnedRuns { get; set; }

        [Column("batters_faced")]
        public double? BattersFaced { get; set; }

        [Column("outs")]
        public double? Outs { get; set; }

        [Column("games_pitched")]
        public double? GamesPitched { get; set; }

        [Column("complete_games")]
        public double? CompleteGames { get; set; }

        [Column("shutouts")]
        public double? Shutouts { get; set; }

        [Column("pitches_thrown")]
        public double? PitchesThrown { get; set; }

        [Column("balls")]
        public double? Balls { get; set; }

        [Column("strikes")]
        public double? Strikes { get; set; }

        [Column("strike_percentage")]
        public double? StrikePercentage { get; set; }

        [Column("hit_batsmen")]
        public double? HitBatsmen { get; set; }

        [Column("balks")]
        public double? Balks { get; set; }

        [Column("wild_pitches")]
        public double? WildPitches { get; set; }

        [Column("pickoffs")]
        public double? Pickoffs { get; set; }

        [Column("rbi")]
        public double? Rbi { get; set; }

        [Column("games_finished")]
        public double? GamesFinished { get; set; }

        [Column("runs_scored_per9")]
        public double? RunsScoredPer9 { get; set; }

        [Column("home_runs_per9")]
        public double? HomeRunsPer9 { get; set; }

        [Column("inherited_runners")]
        public double? InheritedRunners { get; set; }

        [Column("inherited_runners_scored")]
        public double? InheritedRunnersScored { get; set; }

        [Column("catchers_interference")]
        public double? CatchersInterference { get; set; }

        [Column("sac_bunts")]
        public double? SacBunts { get; set; }

        [Column("sac_flies")]
        public double? SacFlies { get; set; }

        [Column("passed_ball")]
        public double? PassedBall { get; set; }

        [Column("pop_outs")]
        public double? PopOuts { get; set; }

        [Column("line_outs")]
        public double? LineOuts { get; set; }
        [Column("game_date")] public DateTime? GameDate { get; set; } 
        [Column("era")] public double? Era { get; set; }
        [Column("whip")] public double? Whip { get; set; }
        [Column("average")] public double? Average { get; set; }
    }
}
