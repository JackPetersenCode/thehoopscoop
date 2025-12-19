using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public class SportRadarMLBPBPPitchEventMap : ClassMap<SportRadarMLBPBPPitchEvent>
{
    public SportRadarMLBPBPPitchEventMap()
    {
        Map(m => m.GameId).Name("game_id");
        Map(m => m.AtBatId).Name("at_bat_id");
        Map(m => m.Inning).Name("inning");
        Map(m => m.Half).Name("half");

        Map(m => m.HitLocation).Name("hit_location");
        Map(m => m.HitType).Name("hit_type");
        Map(m => m.Status).Name("status");
        Map(m => m.EventId).Name("event_id");
        Map(m => m.OutcomeId).Name("outcome_id");
        Map(m => m.CreatedAt).Name("created_at");
        Map(m => m.UpdatedAt).Name("updated_at");
        Map(m => m.SequenceNumber).Name("sequence_number");
        Map(m => m.Official).Name("official");
        Map(m => m.Type).Name("type");
        Map(m => m.WallClockStartTime).Name("wall_clock_start_time");
        Map(m => m.WallClockEndTime).Name("wall_clock_end_time");

        Map(m => m.IsAbOver).Name("is_ab_over");
        Map(m => m.IsBunt).Name("is_bunt");
        Map(m => m.IsHit).Name("is_hit");
        Map(m => m.IsWildPitch).Name("is_wild_pitch");
        Map(m => m.IsPassedBall).Name("is_passed_ball");
        Map(m => m.IsDoublePlay).Name("is_double_play");
        Map(m => m.IsTriplePlay).Name("is_triple_play");

        Map(m => m.Balls).Name("balls");
        Map(m => m.Strikes).Name("strikes");
        Map(m => m.Outs).Name("outs");

        Map(m => m.PitchCount).Name("pitch_count");
        Map(m => m.PitchType).Name("pitch_type");
        Map(m => m.PitchSpeed).Name("pitch_speed");
        Map(m => m.PitchZone).Name("pitch_zone");

        Map(m => m.PitcherHand).Name("pitcher_hand");
        Map(m => m.HitterHand).Name("hitter_hand");
        Map(m => m.PitcherId).Name("pitcher_id");
        Map(m => m.PitchX).Name("pitch_x");
        Map(m => m.PitchY).Name("pitch_y");

        Map(m => m.PitcherPreferredName).Name("pitcher_preferred_name");
        Map(m => m.PitcherFirstName).Name("pitcher_first_name");
        Map(m => m.PitcherLastName).Name("pitcher_last_name");
        Map(m => m.PitcherJerseyNumber).Name("pitcher_jersey_number");
        Map(m => m.PitcherFullName).Name("pitcher_full_name");

        Map(m => m.HitterPreferredName).Name("hitter_preferred_name");
        Map(m => m.HitterFirstName).Name("hitter_first_name");
        Map(m => m.HitterLastName).Name("hitter_last_name");
        Map(m => m.HitterJerseyNumber).Name("hitter_jersey_number");
        Map(m => m.HitterFullName).Name("hitter_full_name");
        Map(m => m.HitterId).Name("hitter_id");

        Map(m => m.HomeTeamRuns).Name("home_team_runs");
        Map(m => m.AwayTeamRuns).Name("away_team_runs");

        Map(m => m.MlbPitchSpeed).Name("mlb_pitch_speed");
        Map(m => m.MlbStrikeZoneTop).Name("mlb_strike_zone_top");
        Map(m => m.MlbStrikeZoneBottom).Name("mlb_strike_zone_bottom");
        Map(m => m.MlbPitchZone).Name("mlb_pitch_zone");
        Map(m => m.MlbPitchCode).Name("mlb_pitch_code");
        Map(m => m.MlbPitchDescription).Name("mlb_pitch_description");
        Map(m => m.MlbPitchX).Name("mlb_pitch_x");
        Map(m => m.MlbPitchY).Name("mlb_pitch_y");
        Map(m => m.MlbHitTrajectory).Name("mlb_hit_trajectory");
        Map(m => m.MlbHitHardness).Name("mlb_hit_hardness");
        Map(m => m.MlbHitX).Name("mlb_hit_x");
        Map(m => m.MlbHitY).Name("mlb_hit_y");
    }
}
