using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp4.Server.Data
{
    public class MLBStatsPitchingSplits : IMLBStatsPitching
    {
        [Column("person_id")] public int PersonId { get; set; }
        [Column("full_name")] public string FullName { get; set; } = string.Empty;
        [Column("team_name")] public string TeamName { get; set; } = string.Empty;
        [Column("league_name")] public string LeagueName { get; set; } = string.Empty;
        [Column("primary_position_name")] public string Position { get; set; } = string.Empty;
        [Column("games_played")] public double? GamesPlayed { get; set; }
        [Column("innings_pitched")] public double? InningsPitched { get; set; }
        [Column("hits")] public double? Hits { get; set; }

        [Column("doubles")] public double? Doubles { get; set; }
        [Column("triples")] public double? Triples { get; set; }
        [Column("home_runs")] public double? HomeRuns { get; set; }
        [Column("average")] public double? Average { get; set; }
        // [Column("rbi")] public double? Rbi { get; set; } // Uncomment if needed
        [Column("strike_outs")] public double? StrikeOuts { get; set; }
        [Column("base_on_balls")] public double? BaseOnBalls { get; set; }
        [Column("intentional_walks")] public double? IntentionalWalks { get; set; }
        [Column("hit_batsmen")] public double? HitBatsmen { get; set; }
        [Column("batters_faced")] public double? BattersFaced { get; set; }
        [Column("pitches_thrown")] public double? PitchesThrown { get; set; }
        [Column("sac_flies")] public double? SacFlies { get; set; }
        [Column("sac_bunts")] public double? SacBunts { get; set; }
        [Column("catchers_interference")] public double? CatchersInterference { get; set; }
        [Column("pickoffs")] public double? Pickoffs { get; set; }
        [Column("pop_outs")] public double? PopOuts { get; set; }
        [Column("line_outs")] public double? LineOuts { get; set; }

        [Column("ground_into_double_play")] public double? GroundIntoDoublePlay { get; set; }
        [Column("ground_into_triple_play")] public double? GroundIntoTriplePlay { get; set; }
        [Column("total_bases")] public double? TotalBases { get; set; }

        [Column("ground_outs")] public double? GroundOuts { get; set; }
        [Column("fly_outs")] public double? FlyOuts { get; set; }
        [Column("pitches_per_inning")] public double? PitchesPerInning { get; set; }
        [Column("strikeouts_per9")] public double? StrikeoutsPer9 { get; set; }
        [Column("home_runs_per9")] public double? HomeRunsPer9 { get; set; }
        [Column("walks_per9")] public double? WalksPer9 { get; set; }
        [Column("whip")] public double? Whip { get; set; }

    }
}
