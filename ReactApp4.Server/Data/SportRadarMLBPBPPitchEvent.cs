using System;

namespace ReactApp4.Server.Data
{
    public class SportRadarMLBPBPPitchEvent
    {
        public Guid? GameId { get; set; }
        public Guid? AtBatId { get; set; }
        public int? Inning { get; set; }
        public string? Half { get; set; }                      // "T"/"B"

        public string? HitLocation { get; set; }
        public string? HitType { get; set; }
        public string? Status { get; set; }
        public Guid? EventId { get; set; }                     // looks like a UUID in sample
        public string? OutcomeId { get; set; }                 // "kKL" (not a UUID)
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public int? SequenceNumber { get; set; }
        public bool? Official { get; set; }
        public string? Type { get; set; }

        public DateTimeOffset? WallClockStartTime { get; set; }
        public DateTimeOffset? WallClockEndTime { get; set; }

        public bool? IsAbOver { get; set; }
        public bool? IsBunt { get; set; }
        public bool? IsHit { get; set; }
        public bool? IsWildPitch { get; set; }
        public bool? IsPassedBall { get; set; }
        public bool? IsDoublePlay { get; set; }
        public bool? IsTriplePlay { get; set; }

        public int? Balls { get; set; }
        public int? Strikes { get; set; }
        public int? Outs { get; set; }

        public double? PitchCount { get; set; }                // sample shows 1.0
        public string? PitchType { get; set; }                 // "FA"
        public double? PitchSpeed { get; set; }                // 92.5
        public double? PitchZone { get; set; }                 // 11.0

        public string? PitcherHand { get; set; }               // "R"
        public string? HitterHand { get; set; }                // "L"
        public Guid? PitcherId { get; set; }
        public double? PitchX { get; set; }                    // 129.0
        public double? PitchY { get; set; }                    // 39.0

        public string? PitcherPreferredName { get; set; }
        public string? PitcherFirstName { get; set; }
        public string? PitcherLastName { get; set; }
        public int? PitcherJerseyNumber { get; set; }
        public string? PitcherFullName { get; set; }

        public string? HitterPreferredName { get; set; }
        public string? HitterFirstName { get; set; }
        public string? HitterLastName { get; set; }
        public int? HitterJerseyNumber { get; set; }
        public string? HitterFullName { get; set; }
        public Guid? HitterId { get; set; }

        public int? HomeTeamRuns { get; set; }
        public int? AwayTeamRuns { get; set; }

        public double? MlbPitchSpeed { get; set; }             // 92.5
        public double? MlbStrikeZoneTop { get; set; }          // 3.2235…
        public double? MlbStrikeZoneBottom { get; set; }       // 1.4902…
        public double? MlbPitchZone { get; set; }              // 11.0
        public string? MlbPitchCode { get; set; }              // "FF"
        public string? MlbPitchDescription { get; set; }       // "Four-Seam Fastball"
        public double? MlbPitchX { get; set; }                 // 151.69
        public double? MlbPitchY { get; set; }                 // 166.65
        public string? MlbHitTrajectory { get; set; }          // may be empty
        public string? MlbHitHardness { get; set; }            // may be empty
        public double? MlbHitX { get; set; }                   // may be empty
        public double? MlbHitY { get; set; }                   // may be empty
    }
}
