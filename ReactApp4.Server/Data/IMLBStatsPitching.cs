namespace ReactApp4.Server.Data
{
    public interface IMLBStatsPitching
    {
        int PersonId { get; set; }
        string FullName { get; set; }
        string TeamName { get; set; }
        string LeagueName { get; set; }
        string Position { get; set; }
        double? GamesPlayed { get; set; }
        double? InningsPitched { get; set; }
        double? Hits { get; set; }
        double? Doubles { get; set; }
        double? Triples { get; set; }
        double? HomeRuns { get; set; }
        double? Average { get; set; }
        double? StrikeOuts { get; set; }
        double? BaseOnBalls { get; set; }
        double? IntentionalWalks { get; set; }
        double? HitBatsmen { get; set; }
        double? BattersFaced { get; set; }
        double? PitchesThrown { get; set; }
        double? SacBunts { get; set; }
        double? SacFlies { get; set; }
        double? CatchersInterference { get; set; }
        double? Pickoffs { get; set; }
        double? PopOuts { get; set; }
        double? LineOuts { get; set; }
        //double? Rbi { get; set; }
        double? Whip { get; set; }
        double? StrikeoutsPer9 { get; set; }
        double? HomeRunsPer9 { get; set; }
        double? WalksPer9 { get; set; }
    }
}
