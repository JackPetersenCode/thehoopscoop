using CsvHelper.Configuration;
using ReactApp4.Server.Data;

public class PlayPlayEventsMap : ClassMap<PlayPlayEvents>
{
    public PlayPlayEventsMap()
    {
        Map(m => m.GamePk).Name("game_pk");
        Map(m => m.AtBatIndex).Name("about_at_bat_index");

        Map(m => m.PlayEventsDetailsCallCode).Name("play_events_details_call_code");
        Map(m => m.PlayEventsDetailsCallDescription).Name("play_events_details_call_description");
        Map(m => m.PlayEventsDetailsDescription).Name("play_events_details_description");
        Map(m => m.PlayEventsDetailsCode).Name("play_events_details_code");
        Map(m => m.PlayEventsDetailsBallColor).Name("play_events_details_ball_color");
        Map(m => m.PlayEventsDetailsTrailColor).Name("play_events_details_trail_color");
        Map(m => m.PlayEventsDetailsIsInPlay).Name("play_events_details_is_in_play")
            .TypeConverterOption.NullValues(string.Empty);
        Map(m => m.PlayEventsDetailsIsStrike).Name("play_events_details_is_strike")
            .TypeConverterOption.NullValues(string.Empty);
        Map(m => m.PlayEventsDetailsIsBall).Name("play_events_details_is_ball")
            .TypeConverterOption.NullValues(string.Empty);
        Map(m => m.PlayEventsDetailsTypeCode).Name("play_events_details_type_code");
        Map(m => m.PlayEventsDetailsTypeDescription).Name("play_events_details_type_description");
        Map(m => m.PlayEventsDetailsIsOut).Name("play_events_details_is_out")
            .TypeConverterOption.NullValues(string.Empty);
        Map(m => m.PlayEventsDetailsHasReview).Name("play_events_details_has_review")
            .TypeConverterOption.NullValues(string.Empty);

        Map(m => m.PlayEventsCountBalls).Name("play_events_count_balls");
        Map(m => m.PlayEventsCountStrikes).Name("play_events_count_strikes");
        Map(m => m.PlayEventsCountOuts).Name("play_events_count_outs");

        Map(m => m.PlayEventsPitchDataStartSpeed).Name("play_events_pitch_data_start_speed");
        Map(m => m.PlayEventsPitchDataEndSpeed).Name("play_events_pitch_data_end_speed");
        Map(m => m.PlayEventsPitchDataStrikeZoneTop).Name("play_events_pitch_data_strike_zone_top");
        Map(m => m.PlayEventsPitchDataStrikeZoneBottom).Name("play_events_pitch_data_strike_zone_bottom");

        Map(m => m.PlayEventsPitchDataCoordinatesAY).Name("play_events_pitch_data_coordinates_aY");
        Map(m => m.PlayEventsPitchDataCoordinatesAZ).Name("play_events_pitch_data_coordinates_aZ");
        Map(m => m.PlayEventsPitchDataCoordinatesPfxX).Name("play_events_pitch_data_coordinates_pfxX");
        Map(m => m.PlayEventsPitchDataCoordinatesPfxZ).Name("play_events_pitch_data_coordinates_pfxZ");
        Map(m => m.PlayEventsPitchDataCoordinatesPX).Name("play_events_pitch_data_coordinates_pX");
        Map(m => m.PlayEventsPitchDataCoordinatesPZ).Name("play_events_pitch_data_coordinates_pZ");
        Map(m => m.PlayEventsPitchDataCoordinatesVX0).Name("play_events_pitch_data_coordinates_vX0");
        Map(m => m.PlayEventsPitchDataCoordinatesVY0).Name("play_events_pitch_data_coordinates_vY0");
        Map(m => m.PlayEventsPitchDataCoordinatesVZ0).Name("play_events_pitch_data_coordinates_vZ0");
        Map(m => m.PlayEventsPitchDataCoordinatesX).Name("play_events_pitch_data_coordinates_x");
        Map(m => m.PlayEventsPitchDataCoordinatesY).Name("play_events_pitch_data_coordinates_y");
        Map(m => m.PlayEventsPitchDataCoordinatesX0).Name("play_events_pitch_data_coordinates_x0");
        Map(m => m.PlayEventsPitchDataCoordinatesY0).Name("play_events_pitch_data_coordinates_y0");
        Map(m => m.PlayEventsPitchDataCoordinatesZ0).Name("play_events_pitch_data_coordinates_z0");
        Map(m => m.PlayEventsPitchDataCoordinatesAX).Name("play_events_pitch_data_coordinates_aX");

        Map(m => m.PlayEventsPitchDataBreaksBreakAngle).Name("play_events_pitch_data_breaks_break_angle");
        Map(m => m.PlayEventsPitchDataBreaksBreakLength).Name("play_events_pitch_data_breaks_break_length");
        Map(m => m.PlayEventsPitchDataBreaksBreakY).Name("play_events_pitch_data_breaks_break_y");
        Map(m => m.PlayEventsPitchDataBreaksBreakVertical).Name("play_events_pitch_data_breaks_break_vertical");
        Map(m => m.PlayEventsPitchDataBreaksBreakVerticalInduced).Name("play_events_pitch_data_breaks_break_vertical_induced");
        Map(m => m.PlayEventsPitchDataBreaksBreakHorizontal).Name("play_events_pitch_data_breaks_break_horizontal");
        Map(m => m.PlayEventsPitchDataBreaksSpinRate).Name("play_events_pitch_data_breaks_spin_rate");
        Map(m => m.PlayEventsPitchDataBreaksSpinDirection).Name("play_events_pitch_data_breaks_spin_direction");

        Map(m => m.PlayEventsPitchDataZone).Name("play_events_pitch_data_zone");
        Map(m => m.PlayEventsPitchDataTypeConfidence).Name("play_events_pitch_data_type_confidence");
        Map(m => m.PlayEventsPitchDataPlateTime).Name("play_events_pitch_data_plate_time");
        Map(m => m.PlayEventsPitchDataExtension).Name("play_events_pitch_data_extension");

        Map(m => m.PlayEventsHitDataLaunchSpeed).Name("play_events_hit_data_launch_speed");
        Map(m => m.PlayEventsHitDataLaunchAngle).Name("play_events_hit_data_launch_angle");
        Map(m => m.PlayEventsHitDataTotalDistance).Name("play_events_hit_data_total_distance");
        Map(m => m.PlayEventsHitDataTrajectory).Name("play_events_hit_data_trajectory");
        Map(m => m.PlayEventsHitDataHardness).Name("play_events_hit_data_hardness");
        Map(m => m.PlayEventsHitDataLocation).Name("play_events_hit_data_location");
        Map(m => m.PlayEventsHitDataCoordinatesCoordX).Name("play_events_hit_data_coordinates_coordX");
        Map(m => m.PlayEventsHitDataCoordinatesCoordY).Name("play_events_hit_data_coordinates_coordY");

        Map(m => m.PlayEventsIndex).Name("play_events_index");
        Map(m => m.PlayEventsPlayId).Name("play_events_play_id");
        Map(m => m.PlayEventsPitchNumber).Name("play_events_pitch_number");
        Map(m => m.PlayEventsStartTime).Name("play_events_start_time");
        Map(m => m.PlayEventsEndTime).Name("play_events_end_time");
        Map(m => m.PlayEventsIsPitch).Name("play_events_is_pitch")
            .TypeConverterOption.NullValues(string.Empty);
        Map(m => m.PlayEventsType).Name("play_events_type");
    }
}
