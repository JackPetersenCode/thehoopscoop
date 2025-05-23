using System.ComponentModel.DataAnnotations.Schema;
namespace ReactApp4.Server.Data
{
    public class MLBStatsBatting : IMLBStatsBatting
    {
        [Column("team_name")] public string TeamName { get; set; } = string.Empty;
        [Column("person_id")] public int PersonId { get; set; }
        [Column("full_name")] public string FullName { get; set; } = string.Empty;
        [Column("primary_position_name")] public string Position { get; set; } = string.Empty;
        [Column("league_name")] public string LeagueName { get; set; } = string.Empty;
        [Column("games_played")] public double? GamesPlayed { get; set; }
        [Column("average")] public double? Average { get; set; }
        [Column("runs")] public double? Runs { get; set; }
        [Column("doubles")] public double? Doubles { get; set; }
        [Column("triples")] public double? Triples { get; set; }
        [Column("home_runs")] public double? HomeRuns { get; set; }
        [Column("strike_outs")] public double? StrikeOuts { get; set; }
        [Column("base_on_balls")] public double? BaseOnBalls { get; set; }
        [Column("intentional_walks")] public double? IntentionalWalks { get; set; }
        [Column("hits")] public double? Hits { get; set; }
        [Column("hit_by_pitch")] public double? HitByPitch { get; set; }
        [Column("at_bats")] public double? AtBats { get; set; }
        [Column("caught_stealing")] public double? CaughtStealing { get; set; }
        [Column("stolen_bases")] public double? StolenBases { get; set; }
        [Column("stolen_base_percentage")] public double? StolenBasePercentage { get; set; }
        [Column("ground_into_double_play")] public double? GroundIntoDoublePlay { get; set; }
        [Column("ground_into_triple_play")] public double? GroundIntoTriplePlay { get; set; }
        [Column("plate_appearances")] public double? PlateAppearances { get; set; }
        [Column("total_bases")] public double? TotalBases { get; set; }
        [Column("rbi")] public double? Rbi { get; set; }
        [Column("left_on_base")] public double? LeftOnBase { get; set; }
        [Column("sac_bunts")] public double? SacBunts { get; set; }
        [Column("sac_flies")] public double? SacFlies { get; set; }
        [Column("catchers_interference")] public double? CatchersInterference { get; set; }
        [Column("pickoffs")] public double? Pickoffs { get; set; }
        [Column("at_bats_per_home_run")] public double? AtBatsPerHomeRun { get; set; }
        [Column("pop_outs")] public double? PopOuts { get; set; }
        [Column("lineouts")] public double? LineOuts { get; set; }
        [Column("flyouts")] public double? FlyOuts { get; set; }
        [Column("groundouts")] public double? GroundOuts { get; set; }
        [Column("obp")] public double? Obp { get; set; }
        [Column("slg")] public double? Slg { get; set; }
        [Column("ops")] public double? Ops { get; set; }
    }
}
