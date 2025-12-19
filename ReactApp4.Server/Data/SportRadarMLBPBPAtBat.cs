using System;

namespace ReactApp4.Server.Data
{
    public class SportRadarMLBPBPAtBat
    {
        public Guid? GameId { get; set; }
        public int? Inning { get; set; }
        public string? Half { get; set; }                    // "T" or "B"
        public Guid? HitterId { get; set; }
        public Guid? AtBatId { get; set; }
        public string? HitterHand { get; set; }              // "L","R","S"
        public Guid? PitcherId { get; set; }
        public string? PitcherHand { get; set; }             // "L","R","S"
        public int? SequenceNumber { get; set; }
        public string? Description { get; set; }

        public string? HitterPreferredName { get; set; }
        public string? HitterFirstName { get; set; }
        public string? HitterLastName { get; set; }
        public int? HitterJerseyNumber { get; set; }
        public string? HitterFullName { get; set; }

        public string? PitcherPreferredName { get; set; }
        public string? PitcherFirstName { get; set; }
        public string? PitcherLastName { get; set; }
        public int? PitcherJerseyNumber { get; set; }
        public string? PitcherFullName { get; set; }

        public int? HomeTeamRuns { get; set; }
        public int? AwayTeamRuns { get; set; }
    }
}
