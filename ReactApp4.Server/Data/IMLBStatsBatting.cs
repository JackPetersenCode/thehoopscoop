namespace ReactApp4.Server.Data
{
    public interface IMLBStatsBatting
    {
        string TeamName { get; set; }
        int PersonId { get; set; }
        string FullName { get; set; }
        string Position { get; set; }
        string LeagueName { get; set; }
        double? GamesPlayed { get; set; }
        double? Average { get; set; }
        double? Doubles { get; set; }
        double? Triples { get; set; }
        double? HomeRuns { get; set; }
        double? StrikeOuts { get; set; }
        double? BaseOnBalls { get; set; }
        double? IntentionalWalks { get; set; }
        double? Hits { get; set; }
        double? HitByPitch { get; set; }
        double? AtBats { get; set; }
        double? GroundIntoDoublePlay { get; set; }
        double? GroundIntoTriplePlay { get; set; }
        double? PlateAppearances { get; set; }
        double? TotalBases { get; set; }
        double? Rbi { get; set; }
        double? SacBunts { get; set; }
        double? SacFlies { get; set; }
        double? CatchersInterference { get; set; }
        double? Pickoffs { get; set; }
        double? AtBatsPerHomeRun { get; set; }
        double? PopOuts { get; set; }
        double? LineOuts { get; set; }
        double? FlyOuts { get; set; }
        double? GroundOuts { get; set; }
        double? Obp { get; set; }
        double? Slg { get; set; }
        double? Ops { get; set; }
    }
}
