using System.ComponentModel.DataAnnotations.Schema;
namespace ReactApp4.Server.Data
{
    public class PlayEventsDetailsCall
    {
        [Column("play_events_details_call_code")] public string? PlayEventsDetailsCallCode { get; set; }
        [Column("play_events_details_call_description")] public string? PlayEventsDetailsCallDescription { get; set; }
    
    }

    public class PlayEventsDetailsType
    {
        [Column("play_events_details_type_code")] public string? PlayEventsDetailsTypeCode { get; set; }
        [Column("play_events_details_type_description")] public string? PlayEventsDetailsTypeDescription {get; set; }
    }

    public class PlayEventsDetails
    {
        public PlayEventsDetailsCall? PlayEventsDetailsCall { get; set; }
        [Column("play_events_details_description")] public string? PlayEventsDetailsDescription { get; set; }
        [Column("play_events_details_code")] public string? PlayEventsDetailsCode { get; set; }
        [Column("play_events_details_ball_color")] public string? PlayEventsDetailsBallColor { get; set; }
        [Column("play_events_details_trail_color")] public string? PlayEventsDetailsTrailColor { get; set; }
        [Column("play_events_details_is_in_play")] public bool? PlayEventsDetailsIsInPlay { get; set; }
        [Column("play_events_details_is_strike")] public bool? PlayEventsDetailsIsStrike { get; set; }
        [Column("play_events_details_is_ball")] public bool? PlayEventsDetailsIsBall { get; set; }
        public PlayEventsDetailsType? PlayEventsDetailsType { get; set; }
        [Column("play_events_details_is_out")] public bool? PlayEventsDetailsIsOut { get; set; }
        [Column("play_events_details_has_review")] public bool? PlayEventsDetailsHasReview { get; set; }
    }

    public class PlayEventsCount
    {
        [Column("play_events_count_balls")] public int? PlayEventsCountBalls { get; set; }
        [Column("play_events_count_strikes")] public int? PlayEventsCountStrikes { get; set; }
        [Column("play_events_count_outs")] public int? PlayEventsCountOuts { get; set; }
    }

    public class PlayEventsPitchDataCoordinates
    {
        [Column("play_events_pitch_data_coordinates_aY")] public double? PlayEventsPitchDataCoordinatesAY { get; set; }
        [Column("play_events_pitch_data_coordinates_aZ")] public double? PlayEventsPitchDataCoordinatesAZ { get; set; }
        [Column("play_events_pitch_data_coordinates_pfxX")] public double? PlayEventsPitchDataCoordinatesPfxX { get; set; }
        [Column("play_events_pitch_data_coordinates_pfxZ")] public double? PlayEventsPitchDataCoordinatesPfxZ { get; set; }
        [Column("play_events_pitch_data_coordinates_pX")] public double? PlayEventsPitchDataCoordinatesPX { get; set; }
        [Column("play_events_pitch_data_coordinates_pZ")] public double? PlayEventsPitchDataCoordinatesPZ { get; set; }
        [Column("play_events_pitch_data_coordinates_vX0")] public double? PlayEventsPitchDataCoordinatesVX0 { get; set; }
        [Column("play_events_pitch_data_coordinates_vY0")] public double? PlayEventsPitchDataCoordinatesVY0 { get; set; }
        [Column("play_events_pitch_data_coordinates_vZ0")] public double? PlayEventsPitchDataCoordinatesVZ0 { get; set; }
        [Column("play_events_pitch_data_coordinates_x")] public double? PlayEventsPitchDataCoordinatesX { get; set; }
        [Column("play_events_pitch_data_coordinates_y")] public double? PlayEventsPitchDataCoordinatesY { get; set; }
        [Column("play_events_pitch_data_coordinates_x0")] public double? PlayEventsPitchDataCoordinatesX0 { get; set; }
        [Column("play_events_pitch_data_coordinates_y0")] public double? PlayEventsPitchDataCoordinatesY0 { get; set; }
        [Column("play_events_pitch_data_coordinates_z0")] public double? PlayEventsPitchDataCoordinatesZ0 { get; set; }
        [Column("play_events_pitch_data_coordinates_aX")] public double? PlayEventsPitchDataCoordinatesAX { get; set; }
    }

    public class PlayEventsPitchDataBreaks
    {
        [Column("play_events_pitch_data_breaks_break_angle")] public double? PlayEventsPitchDataBreaksBreakAngle { get; set; }
        [Column("play_events_pitch_data_breaks_break_length")] public double? PlayEventsPitchDataBreaksBreakLength { get; set; }
        [Column("play_events_pitch_data_breaks_break_y")] public double? PlayEventsPitchDataBreaksBreakY { get; set; }
        [Column("play_events_pitch_data_breaks_break_vertical")] public double? PlayEventsPitchDataBreaksBreakVertical { get; set; }
        [Column("play_events_pitch_data_breaks_break_vertical_induced")] public double? PlayEventsPitchDataBreaksBreakVerticalInduced { get; set; }
        [Column("play_events_pitch_data_breaks_break_horizontal")] public double? PlayEventsPitchDataBreaksBreakHorizontal { get; set; }
        [Column("play_events_pitch_data_breaks_spin_rate")] public int? PlayEventsPitchDataBreaksSpinRate { get; set; }
        [Column("play_events_pitch_data_breaks_spin_direction")] public int? PlayEventsPitchDataBreaksSpinDirection { get; set; }

    }
    public class PlayEventsPitchData
    {
        [Column("play_events_pitch_data_start_speed")] public double? PlayEventsPitchDataStartSpeed { get; set; }
        [Column("play_events_pitch_data_end_speed")] public double? PlayEventsPitchDataEndSpeed { get; set; }
        [Column("play_events_pitch_data_strike_zone_top")] public double? PlayEventsPitchDataStrikeZoneTop { get; set; }
        [Column("play_events_pitch_data_strike_zone_bottom")] public double? PlayEventsPitchDataStrikeZoneBottom { get; set; }
        public PlayEventsPitchDataCoordinates? PlayEventsPitchDataCoordinates { get; set; }
        public PlayEventsPitchDataBreaks? PlayEventsPitchDataBreaks { get; set; }
        [Column("play_events_pitch_data_zone")] public double? PlayEventsPitchDataZone { get; set; }
        [Column("play_events_pitch_data_type_confidence")] public double? PlayEventsPitchDataTypeConfidence { get; set; }
        [Column("play_events_pitch_data_plate_time")] public double? PlayEventsPitchDataPlateTime { get; set; }
        [Column("play_events_pitch_data_extension")] public double? PlayEventsPitchDataExtension { get; set; }

    }

    public class PlayEventsHitData
    {
        [Column("play_events_hit_data_launch_speed")] public double? PlayEventsHitDataLaunchSpeed { get; set; }
        [Column("play_events_hit_data_launch_angle")] public double? PlayEventsHitDataLaunchAngle { get; set; }
        [Column("play_events_hit_data_total_distance")] public double? PlayEventsHitDataTotalDistance { get; set; }
        [Column("play_events_hit_data_trajectory")] public string? PlayEventsHitDataTrajectory { get; set; }
        [Column("play_events_hit_data_hardness")] public string? PlayEventsHitDataHardness { get; set; }
        [Column("play_events_hit_data_location")] public string? PlayEventsHitDataLocation { get; set; }
        public PlayEventsHitDataCoordinates? PlayEventsHitDataCoordinates { get; set; }
    }

    public class PlayEventsHitDataCoordinates
    {
        [Column("play_events_hit_data_coordinates_coordX")] public double? PlayEventsHitDataCoordinatesCoordX { get; set; }
        [Column("play_events_hit_data_coordinates_coordY")] public double? PlayEventsHitDataCoordinatesCoordY { get; set; }
    }

    public class PlayResult {
        [Column("result_type")] public string? ResultType { get; set; }
        [Column("result_event")] public string? ResultEvent { get; set; }
        [Column("result_event_type")] public string? ResultEventType { get; set; }
        [Column("result_description")] public string? ResultDescription { get; set; }
        [Column("result_rbi")] public int? ResultRbi { get; set; }
        [Column("result_away_score")] public int? ResultAwayScore { get; set; }
        [Column("result_home_score")] public int? ResultHomeScore { get; set; }
        [Column("result_is_out")] public bool? ResultIsOut { get; set; }
    }

    public class PlayAbout
    {
        [Column("about_at_bat_index")] public int? AboutAtBatIndex { get; set; }
        [Column("about_half_inning")] public string? AboutHalfInning { get; set; }
        [Column("about_is_top_inning")] public bool? AboutIsTopInning { get; set; }
        [Column("about_inning")] public int? AboutInning { get; set; }
        [Column("about_start_time")] public DateTime? AboutStartTime { get; set; }
        [Column("about_end_time")] public DateTime? AboutEndTime { get; set; }
        [Column("about_is_complete")] public bool? AboutIsComplete { get; set; }
        [Column("about_is_scoring_play")] public bool? AboutIsScoringPlay { get; set; }
        [Column("about_has_review")] public bool? AboutHasReview { get; set; }
        [Column("about_has_out")] public bool? AboutHasOut { get; set; }
        [Column("about_captivating_index")] public int? AboutCaptivatingIndex { get; set; }

    }

    public class PlayCount
    {
        [Column("count_balls")] public int? CountBalls { get; set; }
        [Column("count_strikes")] public int? CountStrikes { get; set; }
        [Column("count_outs")] public int? CountOuts { get; set; }
    }

    public class PlayMatchupBatter
    {
        [Column("matchup_batter_id")] public int? MatchupBatterId { get; set; }
        [Column("matchup_batter_full_name")] public string? MatchupBatterFullName { get; set; }
    }

    public class PlayMatchupBatSide
    {
        [Column("matchup_bat_side_code")] public string? MatchupBatSideCode { get; set; }
        [Column("matchup_bat_side_description")] public string? MatchupBatSideDescription { get; set; }

    }

    public class PlayMatchupPitcher
    {
        [Column("matchup_pitcher_id")] public int? MatchupPitcherId { get; set; }
        [Column("matchup_pitcher_full_name")] public string? MatchupPitcherFullName { get; set; }
    }
    public class PlayMatchupPitchHand
    {
        [Column("matchup_pitch_hand_code")] public string? MatchupPitchHandCode { get; set; }
        [Column("matchup_pitch_hand_description")] public string? MatchupPitchHandDescription { get; set; }
    }

    public class PlayMatchupSplits
    {
        [Column("matchup_splits_batter")] public string? MatchupSplitsBatter { get; set; }
        [Column("matchup_splits_pitcher")] public string? MatchupSplitsPitcher { get; set; }
        [Column("matchup_splits_men_on_base")] public string? MatchupSplitsMenOnBase { get; set; }
    }

    public class PlayMatchupPostOnFirst
    {
        [Column("matchup_post_on_first_id")] public int? MatchupPostOnFirstId { get; set; }
        [Column("matchup_post_on_first_full_name")] public string? MatchupPostOnFirstFullName { get; set; }
    }

    public class PlayMatchup
    {
        public PlayMatchupBatter? PlayMatchupBatter { get; set; }
        public PlayMatchupBatSide? PlayMatchupBatSide { get; set; }
        public PlayMatchupPitcher? PlayMatchupPitcher { get; set; }
        public PlayMatchupPitchHand? PlayMatchupPitchHand { get; set; }
        public PlayMatchupSplits? PlayMatchupSplits { get; set; }
        public PlayMatchupPostOnFirst? PlayMatchupPostOnFirst { get; set; }
        [Column("matchup_batter_hot_cold_zones")] public string? MatchupBatterHotColdZones { get; set; }
        [Column("matchup_pitcher_hot_cold_zones")] public string? MatchupPitcherHotColdZones { get; set; }

    }

    public class PlayPlayEvents
    {
        [Column("game_pk")] public int GamePk { get; set; }
        [Column("at_bat_index")] public int? AtBatIndex { get; set; }
        //public PlayEventsDetails? PlayEventsDetails { get; set; }
        [Column("play_events_details_call_code")] public string? PlayEventsDetailsCallCode { get; set; }
        [Column("play_events_details_call_description")] public string? PlayEventsDetailsCallDescription { get; set; }
        [Column("play_events_details_description")] public string? PlayEventsDetailsDescription { get; set; }
        [Column("play_events_details_code")] public string? PlayEventsDetailsCode { get; set; }
        [Column("play_events_details_ball_color")] public string? PlayEventsDetailsBallColor { get; set; }
        [Column("play_events_details_trail_color")] public string? PlayEventsDetailsTrailColor { get; set; }
        [Column("play_events_details_is_in_play")] public bool? PlayEventsDetailsIsInPlay { get; set; }
        [Column("play_events_details_is_strike")] public bool? PlayEventsDetailsIsStrike { get; set; }
        [Column("play_events_details_is_ball")] public bool? PlayEventsDetailsIsBall { get; set; }
        [Column("play_events_details_type_code")] public string? PlayEventsDetailsTypeCode { get; set; }
        [Column("play_events_details_type_description")] public string? PlayEventsDetailsTypeDescription {get; set; }
        [Column("play_events_details_is_out")] public bool? PlayEventsDetailsIsOut { get; set; }
        [Column("play_events_details_has_review")] public bool? PlayEventsDetailsHasReview { get; set; }
        //public PlayEventsCount? PlayEventsCount { get; set; }
        [Column("play_events_count_balls")] public int? PlayEventsCountBalls { get; set; }
        [Column("play_events_count_strikes")] public int? PlayEventsCountStrikes { get; set; }
        [Column("play_events_count_outs")] public int? PlayEventsCountOuts { get; set; }
        //public PlayEventsPitchData? PlayEventsPitchData { get; set; }
        [Column("play_events_pitch_data_start_speed")] public double? PlayEventsPitchDataStartSpeed { get; set; }
        [Column("play_events_pitch_data_end_speed")] public double? PlayEventsPitchDataEndSpeed { get; set; }
        [Column("play_events_pitch_data_strike_zone_top")] public double? PlayEventsPitchDataStrikeZoneTop { get; set; }
        [Column("play_events_pitch_data_strike_zone_bottom")] public double? PlayEventsPitchDataStrikeZoneBottom { get; set; }
        [Column("play_events_pitch_data_coordinates_aY")] public double? PlayEventsPitchDataCoordinatesAY { get; set; }
        [Column("play_events_pitch_data_coordinates_aZ")] public double? PlayEventsPitchDataCoordinatesAZ { get; set; }
        [Column("play_events_pitch_data_coordinates_pfxX")] public double? PlayEventsPitchDataCoordinatesPfxX { get; set; }
        [Column("play_events_pitch_data_coordinates_pfxZ")] public double? PlayEventsPitchDataCoordinatesPfxZ { get; set; }
        [Column("play_events_pitch_data_coordinates_pX")] public double? PlayEventsPitchDataCoordinatesPX { get; set; }
        [Column("play_events_pitch_data_coordinates_pZ")] public double? PlayEventsPitchDataCoordinatesPZ { get; set; }
        [Column("play_events_pitch_data_coordinates_vX0")] public double? PlayEventsPitchDataCoordinatesVX0 { get; set; }
        [Column("play_events_pitch_data_coordinates_vY0")] public double? PlayEventsPitchDataCoordinatesVY0 { get; set; }
        [Column("play_events_pitch_data_coordinates_vZ0")] public double? PlayEventsPitchDataCoordinatesVZ0 { get; set; }
        [Column("play_events_pitch_data_coordinates_x")] public double? PlayEventsPitchDataCoordinatesX { get; set; }
        [Column("play_events_pitch_data_coordinates_y")] public double? PlayEventsPitchDataCoordinatesY { get; set; }
        [Column("play_events_pitch_data_coordinates_x0")] public double? PlayEventsPitchDataCoordinatesX0 { get; set; }
        [Column("play_events_pitch_data_coordinates_y0")] public double? PlayEventsPitchDataCoordinatesY0 { get; set; }
        [Column("play_events_pitch_data_coordinates_z0")] public double? PlayEventsPitchDataCoordinatesZ0 { get; set; }
        [Column("play_events_pitch_data_coordinates_aX")] public double? PlayEventsPitchDataCoordinatesAX { get; set; }

        [Column("play_events_pitch_data_breaks_break_angle")] public double? PlayEventsPitchDataBreaksBreakAngle { get; set; }
        [Column("play_events_pitch_data_breaks_break_length")] public double? PlayEventsPitchDataBreaksBreakLength { get; set; }
        [Column("play_events_pitch_data_breaks_break_y")] public double? PlayEventsPitchDataBreaksBreakY { get; set; }
        [Column("play_events_pitch_data_breaks_break_vertical")] public double? PlayEventsPitchDataBreaksBreakVertical { get; set; }
        [Column("play_events_pitch_data_breaks_break_vertical_induced")] public double? PlayEventsPitchDataBreaksBreakVerticalInduced { get; set; }
        [Column("play_events_pitch_data_breaks_break_horizontal")] public double? PlayEventsPitchDataBreaksBreakHorizontal { get; set; }
        [Column("play_events_pitch_data_breaks_spin_rate")] public double? PlayEventsPitchDataBreaksSpinRate { get; set; }
        [Column("play_events_pitch_data_breaks_spin_direction")] public double? PlayEventsPitchDataBreaksSpinDirection { get; set; }

        [Column("play_events_pitch_data_zone")] public double? PlayEventsPitchDataZone { get; set; }
        [Column("play_events_pitch_data_type_confidence")] public double? PlayEventsPitchDataTypeConfidence { get; set; }
        [Column("play_events_pitch_data_plate_time")] public double? PlayEventsPitchDataPlateTime { get; set; }
        [Column("play_events_pitch_data_extension")] public double? PlayEventsPitchDataExtension { get; set; }

        //public PlayEventsHitData? PlayEventsHitData { get; set; }
        [Column("play_events_hit_data_launch_speed")] public double? PlayEventsHitDataLaunchSpeed { get; set; }
        [Column("play_events_hit_data_launch_angle")] public double? PlayEventsHitDataLaunchAngle { get; set; }
        [Column("play_events_hit_data_total_distance")] public double? PlayEventsHitDataTotalDistance { get; set; }
        [Column("play_events_hit_data_trajectory")] public string? PlayEventsHitDataTrajectory { get; set; }
        [Column("play_events_hit_data_hardness")] public string? PlayEventsHitDataHardness { get; set; }
        [Column("play_events_hit_data_location")] public string? PlayEventsHitDataLocation { get; set; }
        [Column("play_events_hit_data_coordinates_coordX")] public double? PlayEventsHitDataCoordinatesCoordX { get; set; }
        [Column("play_events_hit_data_coordinates_coordY")] public double? PlayEventsHitDataCoordinatesCoordY { get; set; }

        [Column("play_events_index")] public int? PlayEventsIndex { get; set; }
        [Column("play_events_play_id")] public string? PlayEventsPlayId { get; set; }
        [Column("play_events_pitch_number")] public double? PlayEventsPitchNumber { get; set; }
        [Column("play_events_start_time")] public DateTime? PlayEventsStartTime { get; set; }
        [Column("play_events_end_time")] public DateTime? PlayEventsEndTime { get; set; }
        [Column("play_events_is_pitch")] public bool? PlayEventsIsPitch { get; set; }
        [Column("play_events_type")] public string? PlayEventsType { get; set; }
    }

    public class PlayRunnersMovement
    {
        [Column("runners_movement_origin_base")] public string? RunnersMovementOriginBase { get; set; }
        [Column("runners_movement_start")] public string? RunnersMovementStart { get; set; }
        [Column("runners_movement_end")] public string? RunnersMovementEnd { get; set; }
        [Column("runners_movement_out_base")] public string? RunnersMovementOutBase { get; set; }
        [Column("runners_movement_is_out")] public bool? RunnersMovementIsOut { get; set; }
        [Column("runners_movement_out_number")] public int? RunnersMovementOutNumber { get; set; }
    }

    public class PlayRunnersDetailsRunner
    {
        [Column("runners_details_runner_id")] public int? RunnersDetailsRunnerId { get; set; }
        [Column("runners_details_runner_full_name")] public string? RunnersDetailsRunnerFullName { get; set; }

    }
    public class PlayRunnersDetails
    {
        [Column("runners_details_event")] public string? RunnersDetailsEvent { get; set; }
        [Column("runners_details_event_type")] public string? RunnersDetailsEventType { get; set; }
        [Column("runners_details_movement_reason")] public string? RunnersDetailsMovementReason { get; set; }
        public PlayRunnersDetailsRunner? PlayRunnersDetailsRunner { get; set; }        
        [Column("runners_details_responsible_pitcher")] public string? RunnersDetailsResponsiblePitcher { get; set; }
        [Column("runners_details_is_scoring_event")] public bool? RunnersDetailsIsScoringEvent { get; set; }
        [Column("runners_details_rbi")] public bool? RunnersDetailsRbi { get; set; }
        [Column("runners_details_earned")] public bool? RunnersDetailsEarned { get; set; }
        [Column("runners_details_team_unearned")] public bool? RunnersDetailsTeamUnearned { get; set; }
        [Column("runners_details_play_index")] public int? RunnersDetailsPlayIndex { get; set; } 
    }

    public class PlayRunnersCreditsPlayer
    {
        [Column("runners_credits_player_id")] public int? RunnersCreditsPlayerId { get; set; }
    }

    public class PlayRunnersCreditsPosition
    {
        [Column("runners_credits_position_code")] public string? RunnersCreditsPositionCode { get; set; }
        [Column("runners_credits_position_name")] public string? RunnersCreditsPositionName { get; set; }
        [Column("runners_credits_position_type")] public string? RunnersCreditsPositionType { get; set; }
        [Column("runners_credits_position_abbreviation")] public string? RunnersCreditsPositionAbbreviation { get; set; }
    }
    public class PlayRunnersCredits
    {
        [Column("game_pk")] public int GamePk { get; set; }
        [Column("at_bat_index")] public int? AtBatIndex { get; set; }
        //public PlayRunnersCreditsPlayer? PlayRunnersCreditsPlayer { get; set; }
        [Column("runners_details_play_index")] public int? RunnersDetailsPlayIndex { get; set; } 
        [Column("runners_details_player_id")] public int? RunnersDetailsPlayerId { get; set; }        
        [Column("runners_credits_player_id")] public int? RunnersCreditsPlayerId { get; set; }        
        //public PlayRunnersCreditsPosition? PlayRunnersCreditsPosition { get; set; }
        [Column("runners_credits_position_code")] public string? RunnersCreditsPositionCode { get; set; }
        [Column("runners_credits_position_name")] public string? RunnersCreditsPositionName { get; set; }
        [Column("runners_credits_position_type")] public string? RunnersCreditsPositionType { get; set; }
        [Column("runners_credits_position_abbreviation")] public string? RunnersCreditsPositionAbbreviation { get; set; }
        [Column("runners_credits_credit")] public string? RunnersCreditsCredit { get; set; }
    }
    public class PlayRunners
    {
        [Column("game_pk")] public int GamePk { get; set; }
        [Column("at_bat_index")] public int? AtBatIndex { get; set; }
        //public PlayRunnersMovement? PlayRunnersMovement { get; set; }
        [Column("runners_movement_origin_base")] public string? RunnersMovementOriginBase { get; set; }
        [Column("runners_movement_start")] public string? RunnersMovementStart { get; set; }
        [Column("runners_movement_end")] public string? RunnersMovementEnd { get; set; }
        [Column("runners_movement_out_base")] public string? RunnersMovementOutBase { get; set; }
        [Column("runners_movement_is_out")] public bool? RunnersMovementIsOut { get; set; }
        [Column("runners_movement_out_number")] public double? RunnersMovementOutNumber { get; set; }

        //public PlayRunnersDetails? PlayRunnersDetails { get; set; }
        [Column("runners_details_event")] public string? RunnersDetailsEvent { get; set; }
        [Column("runners_details_event_type")] public string? RunnersDetailsEventType { get; set; }
        [Column("runners_details_movement_reason")] public string? RunnersDetailsMovementReason { get; set; }
        //public PlayRunnersDetailsRunner? PlayRunnersDetailsRunner { get; set; }        
        [Column("runners_details_player_id")] public int? RunnersDetailsPlayerId { get; set; }
        [Column("runners_details_player_full_name")] public string? RunnersDetailsPlayerFullName { get; set; }
        [Column("runners_details_responsible_pitcher_id")] public string? RunnersDetailsResponsiblePitcherId { get; set; }
        [Column("runners_details_is_scoring_event")] public bool? RunnersDetailsIsScoringEvent { get; set; }
        [Column("runners_details_rbi")] public bool? RunnersDetailsRbi { get; set; }
        [Column("runners_details_earned")] public bool? RunnersDetailsEarned { get; set; }
        [Column("runners_details_team_unearned")] public bool? RunnersDetailsTeamUnearned { get; set; }
        [Column("runners_details_play_index")] public int? RunnersDetailsPlayIndex { get; set; }
        //public PlayRunnersCredits? PlayRunnersCredits { get; set; }
        [Column("runners_credits")] public string? RunnersCredits { get; set; }
    }
    public class Play
    {
        [Column("game_pk")] public int GamePk { get; set; }
        [Column("at_bat_index")] public int? AtBatIndex { get; set; }
        [Column("play_end_time")] public DateTime PlayEndTime { get; set; }
        //public PlayResult? PlayResult { get; set; }
        [Column("result_type")] public string? ResultType { get; set; }
        [Column("result_event")] public string? ResultEvent { get; set; }
        [Column("result_event_type")] public string? ResultEventType { get; set; }
        [Column("result_description")] public string? ResultDescription { get; set; }
        [Column("result_rbi")] public int? ResultRbi { get; set; }
        [Column("result_away_score")] public int? ResultAwayScore { get; set; }
        [Column("result_home_score")] public int? ResultHomeScore { get; set; }
        [Column("result_is_out")] public bool? ResultIsOut { get; set; }

        //public PlayAbout? PlayAbout { get; set; }
        [Column("about_at_bat_index")] public int? AboutAtBatIndex { get; set; }
        [Column("about_half_inning")] public string? AboutHalfInning { get; set; }
        [Column("about_is_top_inning")] public bool? AboutIsTopInning { get; set; }
        [Column("about_inning")] public int? AboutInning { get; set; }
        [Column("about_start_time")] public DateTime? AboutStartTime { get; set; }
        [Column("about_end_time")] public DateTime? AboutEndTime { get; set; }
        [Column("about_is_complete")] public bool? AboutIsComplete { get; set; }
        [Column("about_is_scoring_play")] public bool? AboutIsScoringPlay { get; set; }
        [Column("about_has_review")] public bool? AboutHasReview { get; set; }
        [Column("about_has_out")] public bool? AboutHasOut { get; set; }
        [Column("about_captivating_index")] public int? AboutCaptivatingIndex { get; set; }

        //public PlayCount? PlayCount { get; set; }
        [Column("count_balls")] public int? CountBalls { get; set; }
        [Column("count_strikes")] public int? CountStrikes { get; set; }
        [Column("count_outs")] public int? CountOuts { get; set; }

        //public PlayMatchup? PlayMatchup { get; set; }
        //public PlayMatchupBatter? PlayMatchupBatter { get; set; }
        [Column("matchup_batter_id")] public int? MatchupBatterId { get; set; }
        [Column("matchup_batter_full_name")] public string? MatchupBatterFullName { get; set; }

        //public PlayMatchupBatSide? PlayMatchupBatSide { get; set; }
        [Column("matchup_bat_side_code")] public string? MatchupBatSideCode { get; set; }
        [Column("matchup_bat_side_description")] public string? MatchupBatSideDescription { get; set; }

        //public PlayMatchupPitcher? PlayMatchupPitcher { get; set; }
        [Column("matchup_pitcher_id")] public int? MatchupPitcherId { get; set; }
        [Column("matchup_pitcher_full_name")] public string? MatchupPitcherFullName { get; set; }

        //public PlayMatchupPitchHand? PlayMatchupPitchHand { get; set; }
        [Column("matchup_pitch_hand_code")] public string? MatchupPitchHandCode { get; set; }
        [Column("matchup_pitch_hand_description")] public string? MatchupPitchHandDescription { get; set; }

        //public PlayMatchupSplits? PlayMatchupSplits { get; set; }
        [Column("matchup_splits_batter")] public string? MatchupSplitsBatter { get; set; }
        [Column("matchup_splits_pitcher")] public string? MatchupSplitsPitcher { get; set; }
        [Column("matchup_splits_men_on_base")] public string? MatchupSplitsMenOnBase { get; set; }

        //public PlayMatchupPostOnFirst? PlayMatchupPostOnFirst { get; set; }
        [Column("matchup_post_on_first_id")] public double? MatchupPostOnFirstId { get; set; }
        [Column("matchup_post_on_first_full_name")] public string? MatchupPostOnFirstFullName { get; set; }

        [Column("matchup_batter_hot_cold_zones")] public string? MatchupBatterHotColdZones { get; set; }
        [Column("matchup_pitcher_hot_cold_zones")] public string? MatchupPitcherHotColdZones { get; set; }

        [Column("pitch_index")] public string? PitchIndex { get; set; }
        [Column("action_index")] public string? ActionIndex { get; set; }
        [Column("runner_index")] public string? RunnerIndex { get; set; }
        //public PlayRunners? PlayRunners { get; set; }
        //public PlayPlayEvents? PlayPlayEvents { get; set; }
    }
}
