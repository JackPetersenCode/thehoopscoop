using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using ReactApp4.Server.Data;
using Microsoft.AspNetCore.Http;
using System;
using static System.Net.WebRequestMethods;
using System.Runtime.Intrinsics.X86;
using System.Diagnostics;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json.Linq;

namespace ReactApp4.Server.Services
{
    public class MLBPlayByPlayDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MLBPlayByPlayDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> InsertPlayAsync(List<Play> plays, string season)
        {
            if (plays == null || plays.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var play in plays)
                {
                    var sql = $@"
                        INSERT INTO mlb_plays_{season} (
                            game_pk, at_bat_index, play_end_time, result_type, result_event, result_event_type,
                            result_description, result_rbi, result_away_score, result_home_score, result_is_out,
                            about_at_bat_index, about_half_inning, about_is_top_inning, about_inning,
                            about_start_time, about_end_time, about_is_complete, about_is_scoring_play,
                            about_has_review, about_has_out, about_captivating_index,
                            count_balls, count_strikes, count_outs,
                            matchup_batter_id, matchup_batter_full_name, matchup_bat_side_code, matchup_bat_side_description,
                            matchup_pitcher_id, matchup_pitcher_full_name, matchup_pitch_hand_code, matchup_pitch_hand_description,
                            matchup_splits_batter, matchup_splits_pitcher, matchup_splits_men_on_base,
                            matchup_post_on_first_id, matchup_post_on_first_full_name,
                            matchup_batter_hot_cold_zones, matchup_pitcher_hot_cold_zones,
                            pitch_index, action_index, runner_index
                        ) VALUES (
                            @game_pk, @at_bat_index, @play_end_time, @result_type, @result_event, @result_event_type,
                            @result_description, @result_rbi, @result_away_score, @result_home_score, @result_is_out,
                            @about_at_bat_index, @about_half_inning, @about_is_top_inning, @about_inning,
                            @about_start_time, @about_end_time, @about_is_complete, @about_is_scoring_play,
                            @about_has_review, @about_has_out, @about_captivating_index,
                            @count_balls, @count_strikes, @count_outs,
                            @matchup_batter_id, @matchup_batter_full_name, @matchup_bat_side_code, @matchup_bat_side_description,
                            @matchup_pitcher_id, @matchup_pitcher_full_name, @matchup_pitch_hand_code, @matchup_pitch_hand_description,
                            @matchup_splits_batter, @matchup_splits_pitcher, @matchup_splits_men_on_base,
                            @matchup_post_on_first_id, @matchup_post_on_first_full_name,
                            @matchup_batter_hot_cold_zones, @matchup_pitcher_hot_cold_zones,
                            @pitch_index, @action_index, @runner_index
                        );";

                    using var cmd = new NpgsqlCommand(sql, connection);


                    cmd.Parameters.AddWithValue("game_pk", play.GamePk);
                    cmd.Parameters.AddWithValue("at_bat_index", (object?)play.AtBatIndex ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("play_end_time", play.PlayEndTime);
                    cmd.Parameters.AddWithValue("result_type", (object?)play.ResultType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("result_event", (object?)play.ResultEvent ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("result_event_type", (object?)play.ResultEventType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("result_description", (object?)play.ResultDescription ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("result_rbi", (object?)play.ResultRbi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("result_away_score", (object?)play.ResultAwayScore ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("result_home_score", (object?)play.ResultHomeScore ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("result_is_out", (object?)play.ResultIsOut ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_at_bat_index", (object?)play.AboutAtBatIndex ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_half_inning", (object?)play.AboutHalfInning ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_is_top_inning", (object?)play.AboutIsTopInning ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_inning", (object?)play.AboutInning ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_start_time", (object?)play.AboutStartTime ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_end_time", (object?)play.AboutEndTime ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_is_complete", (object?)play.AboutIsComplete ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_is_scoring_play", (object?)play.AboutIsScoringPlay ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_has_review", (object?)play.AboutHasReview ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_has_out", (object?)play.AboutHasOut ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("about_captivating_index", (object?)play.AboutCaptivatingIndex ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("count_balls", (object?)play.CountBalls ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("count_strikes", (object?)play.CountStrikes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("count_outs", (object?)play.CountOuts ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_batter_id", (object?)play.MatchupBatterId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_batter_full_name", (object?)play.MatchupBatterFullName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_bat_side_code", (object?)play.MatchupBatSideCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_bat_side_description", (object?)play.MatchupBatSideDescription ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_pitcher_id", (object?)play.MatchupPitcherId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_pitcher_full_name", (object?)play.MatchupPitcherFullName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_pitch_hand_code", (object?)play.MatchupPitchHandCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_pitch_hand_description", (object?)play.MatchupPitchHandDescription ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_splits_batter", (object?)play.MatchupSplitsBatter ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_splits_pitcher", (object?)play.MatchupSplitsPitcher ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_splits_men_on_base", (object?)play.MatchupSplitsMenOnBase ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_post_on_first_id", (object?)play.MatchupPostOnFirstId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_post_on_first_full_name", (object?)play.MatchupPostOnFirstFullName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_batter_hot_cold_zones", (object?)play.MatchupBatterHotColdZones ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("matchup_pitcher_hot_cold_zones", (object?)play.MatchupPitcherHotColdZones ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("pitch_index", (object?)play.PitchIndex ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("action_index", (object?)play.ActionIndex ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("runner_index", (object?)play.RunnerIndex ?? DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                }
                return Ok(new { message = "Inserted batting stats", inserted = plays.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        private object ToDbValue(string? value) =>
            string.IsNullOrWhiteSpace(value) ? DBNull.Value : value;

        public async Task<IActionResult> InsertPlayEventAsync(List<PlayPlayEvents> playEvents, string season)
        {
            if (playEvents == null || playEvents.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var playEvent in playEvents)
                {
                    NormalizeEmptyStringsToNull(playEvent);
                    var sql = $@"
                        INSERT INTO mlb_play_events_{season} (
                            game_pk, at_bat_index,
                            play_events_details_call_code, play_events_details_call_description,
                            play_events_details_description, play_events_details_code, play_events_details_ball_color, play_events_details_trail_color,
                            play_events_details_is_in_play, play_events_details_is_strike, play_events_details_is_ball,
                            play_events_details_type_code, play_events_details_type_description,
                            play_events_details_is_out, play_events_details_has_review,
                            play_events_count_balls, play_events_count_strikes, play_events_count_outs,
                            play_events_pitch_data_start_speed, play_events_pitch_data_end_speed,
                            play_events_pitch_data_strike_zone_top, play_events_pitch_data_strike_zone_bottom,
                            play_events_pitch_data_coordinates_aY, play_events_pitch_data_coordinates_aZ,
                            play_events_pitch_data_coordinates_pfxX, play_events_pitch_data_coordinates_pfxZ,
                            play_events_pitch_data_coordinates_pX, play_events_pitch_data_coordinates_pZ,
                            play_events_pitch_data_coordinates_vX0, play_events_pitch_data_coordinates_vY0, play_events_pitch_data_coordinates_vZ0,
                            play_events_pitch_data_coordinates_x, play_events_pitch_data_coordinates_y,
                            play_events_pitch_data_coordinates_x0, play_events_pitch_data_coordinates_y0, play_events_pitch_data_coordinates_z0,
                            play_events_pitch_data_coordinates_aX,
                            play_events_pitch_data_breaks_break_angle, play_events_pitch_data_breaks_break_length,
                            play_events_pitch_data_breaks_break_y, play_events_pitch_data_breaks_break_vertical,
                            play_events_pitch_data_breaks_break_vertical_induced, play_events_pitch_data_breaks_break_horizontal,
                            play_events_pitch_data_breaks_spin_rate, play_events_pitch_data_breaks_spin_direction,
                            play_events_pitch_data_zone, play_events_pitch_data_type_confidence,
                            play_events_pitch_data_plate_time, play_events_pitch_data_extension,
                            play_events_hit_data_launch_speed, play_events_hit_data_launch_angle,
                            play_events_hit_data_total_distance, play_events_hit_data_trajectory,
                            play_events_hit_data_hardness, play_events_hit_data_location,
                            play_events_hit_data_coordinates_coordX, play_events_hit_data_coordinates_coordY,
                            play_events_index, play_events_play_id, play_events_pitch_number,
                            play_events_start_time, play_events_end_time, play_events_is_pitch, play_events_type
                        ) VALUES (
                            @play_events_game_pk, @play_events_at_bat_index,
                            @play_events_details_call_code, @play_events_details_call_description,
                            @play_events_details_description, @play_events_details_code, @play_events_details_ball_color, @play_events_details_trail_color,
                            @play_events_details_is_in_play, @play_events_details_is_strike, @play_events_details_is_ball,
                            @play_events_details_type_code, @play_events_details_type_description,
                            @play_events_details_is_out, @play_events_details_has_review,
                            @play_events_count_balls, @play_events_count_strikes, @play_events_count_outs,
                            @play_events_pitch_data_start_speed, @play_events_pitch_data_end_speed,
                            @play_events_pitch_data_strike_zone_top, @play_events_pitch_data_strike_zone_bottom,
                            @play_events_pitch_data_coordinates_aY, @play_events_pitch_data_coordinates_aZ,
                            @play_events_pitch_data_coordinates_pfxX, @play_events_pitch_data_coordinates_pfxZ,
                            @play_events_pitch_data_coordinates_pX, @play_events_pitch_data_coordinates_pZ,
                            @play_events_pitch_data_coordinates_vX0, @play_events_pitch_data_coordinates_vY0, @play_events_pitch_data_coordinates_vZ0,
                            @play_events_pitch_data_coordinates_x, @play_events_pitch_data_coordinates_y,
                            @play_events_pitch_data_coordinates_x0, @play_events_pitch_data_coordinates_y0, @play_events_pitch_data_coordinates_z0,
                            @play_events_pitch_data_coordinates_aX,
                            @play_events_pitch_data_breaks_break_angle, @play_events_pitch_data_breaks_break_length,
                            @play_events_pitch_data_breaks_break_y, @play_events_pitch_data_breaks_break_vertical,
                            @play_events_pitch_data_breaks_break_vertical_induced, @play_events_pitch_data_breaks_break_horizontal,
                            @play_events_pitch_data_breaks_spin_rate, @play_events_pitch_data_breaks_spin_direction,
                            @play_events_pitch_data_zone, @play_events_pitch_data_type_confidence,
                            @play_events_pitch_data_plate_time, @play_events_pitch_data_extension,
                            @play_events_hit_data_launch_speed, @play_events_hit_data_launch_angle,
                            @play_events_hit_data_total_distance, @play_events_hit_data_trajectory,
                            @play_events_hit_data_hardness, @play_events_hit_data_location,
                            @play_events_hit_data_coordinates_coordX, @play_events_hit_data_coordinates_coordY,
                            @play_events_index, @play_events_play_id, @play_events_pitch_number,
                            @play_events_start_time, @play_events_end_time, @play_events_is_pitch, @play_events_type
                        );";


                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@play_events_game_pk", playEvent.GamePk);
                    cmd.Parameters.AddWithValue("@play_events_at_bat_index", (object?)playEvent.AtBatIndex ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@play_events_details_call_code", (object?)playEvent.PlayEventsDetailsCallCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_call_description", (object?)playEvent.PlayEventsDetailsCallDescription ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_description", (object?)playEvent.PlayEventsDetailsDescription ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_code", (object?)playEvent.PlayEventsDetailsCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_ball_color", (object?)playEvent.PlayEventsDetailsBallColor ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_trail_color", (object?)playEvent.PlayEventsDetailsTrailColor ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_is_in_play", (object?)playEvent.PlayEventsDetailsIsInPlay ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_is_strike", (object?)playEvent.PlayEventsDetailsIsStrike ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_is_ball", (object?)playEvent.PlayEventsDetailsIsBall ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_type_code", (object?)playEvent.PlayEventsDetailsTypeCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_type_description", (object?)playEvent.PlayEventsDetailsTypeDescription ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_is_out", (object?)playEvent.PlayEventsDetailsIsOut ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_details_has_review", (object?)playEvent.PlayEventsDetailsHasReview ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@play_events_count_balls", (object?)playEvent.PlayEventsCountBalls ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_count_strikes", (object?)playEvent.PlayEventsCountStrikes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_count_outs", (object?)playEvent.PlayEventsCountOuts ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@play_events_pitch_data_start_speed", (object?)playEvent.PlayEventsPitchDataStartSpeed ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_end_speed", (object?)playEvent.PlayEventsPitchDataEndSpeed ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_strike_zone_top", (object?)playEvent.PlayEventsPitchDataStrikeZoneTop ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_strike_zone_bottom", (object?)playEvent.PlayEventsPitchDataStrikeZoneBottom ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_aY", (object?)playEvent.PlayEventsPitchDataCoordinatesAY ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_aZ", (object?)playEvent.PlayEventsPitchDataCoordinatesAZ ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_pfxX", (object?)playEvent.PlayEventsPitchDataCoordinatesPfxX ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_pfxZ", (object?)playEvent.PlayEventsPitchDataCoordinatesPfxZ ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_pX", (object?)playEvent.PlayEventsPitchDataCoordinatesPX ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_pZ", (object?)playEvent.PlayEventsPitchDataCoordinatesPZ ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_vX0", (object?)playEvent.PlayEventsPitchDataCoordinatesVX0 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_vY0", (object?)playEvent.PlayEventsPitchDataCoordinatesVY0 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_vZ0", (object?)playEvent.PlayEventsPitchDataCoordinatesVZ0 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_x", (object?)playEvent.PlayEventsPitchDataCoordinatesX ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_y", (object?)playEvent.PlayEventsPitchDataCoordinatesY ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_x0", (object?)playEvent.PlayEventsPitchDataCoordinatesX0 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_y0", (object?)playEvent.PlayEventsPitchDataCoordinatesY0 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_z0", (object?)playEvent.PlayEventsPitchDataCoordinatesZ0 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_coordinates_aX", (object?)playEvent.PlayEventsPitchDataCoordinatesAX ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@play_events_pitch_data_breaks_break_angle", (object?)playEvent.PlayEventsPitchDataBreaksBreakAngle ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_breaks_break_length", (object?)playEvent.PlayEventsPitchDataBreaksBreakLength ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_breaks_break_y", (object?)playEvent.PlayEventsPitchDataBreaksBreakY ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_breaks_break_vertical", (object?)playEvent.PlayEventsPitchDataBreaksBreakVertical ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_breaks_break_vertical_induced", (object?)playEvent.PlayEventsPitchDataBreaksBreakVerticalInduced ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_breaks_break_horizontal", (object?)playEvent.PlayEventsPitchDataBreaksBreakHorizontal ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_breaks_spin_rate", (object?)playEvent.PlayEventsPitchDataBreaksSpinRate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_breaks_spin_direction", (object?)playEvent.PlayEventsPitchDataBreaksSpinDirection ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@play_events_pitch_data_zone", (object?)playEvent.PlayEventsPitchDataZone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_type_confidence", (object?)playEvent.PlayEventsPitchDataTypeConfidence ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_plate_time", (object?)playEvent.PlayEventsPitchDataPlateTime ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_data_extension", (object?)playEvent.PlayEventsPitchDataExtension ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@play_events_hit_data_launch_speed", (object?)playEvent.PlayEventsHitDataLaunchSpeed ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_hit_data_launch_angle", (object?)playEvent.PlayEventsHitDataLaunchAngle ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_hit_data_total_distance", (object?)playEvent.PlayEventsHitDataTotalDistance ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_hit_data_trajectory", (object?)playEvent.PlayEventsHitDataTrajectory ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_hit_data_hardness", (object?)playEvent.PlayEventsHitDataHardness ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_hit_data_location", (object?)playEvent.PlayEventsHitDataLocation ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_hit_data_coordinates_coordX", (object?)playEvent.PlayEventsHitDataCoordinatesCoordX ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_hit_data_coordinates_coordY", (object?)playEvent.PlayEventsHitDataCoordinatesCoordY ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@play_events_index", (object?)playEvent.PlayEventsIndex ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_play_id", (object?)playEvent.PlayEventsPlayId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_pitch_number", (object?)playEvent.PlayEventsPitchNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_start_time", (object?)playEvent.PlayEventsStartTime ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_end_time", (object?)playEvent.PlayEventsEndTime ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_is_pitch", (object?)playEvent.PlayEventsIsPitch ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@play_events_type", (object?)playEvent.PlayEventsType ?? DBNull.Value);


                    await cmd.ExecuteNonQueryAsync();
                }

                return Ok(new { message = "Inserted batting stats", inserted = playEvents.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    
        public async Task<IActionResult> InsertPlayRunnersAsync(List<PlayRunners> playRunners, string season)
        {

            if (playRunners == null || playRunners.Count == 0)
                return BadRequest("No records provided.");

            try
            {

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var playRunner in playRunners)
                {           
                    NormalizeEmptyStringsToNull(playRunner); // ✅ sanitize strings to null

                    var sql = $@"
                        INSERT INTO mlb_runners_{season} (
                            game_pk, at_bat_index,
                            runners_movement_origin_base, runners_movement_start, runners_movement_end,
                            runners_movement_out_base, runners_movement_is_out, runners_movement_out_number,
                            runners_details_event, runners_details_event_type, runners_details_movement_reason,
                            runners_details_player_id, runners_details_player_full_name,
                            runners_details_responsible_pitcher_id, runners_details_is_scoring_event,
                            runners_details_rbi, runners_details_earned, runners_details_team_unearned,
                            runners_details_play_index, runners_credits
                        ) VALUES (
                            @game_pk, @at_bat_index,
                            @runners_movement_origin_base, @runners_movement_start, @runners_movement_end,
                            @runners_movement_out_base, @runners_movement_is_out, @runners_movement_out_number,
                            @runners_details_event, @runners_details_event_type, @runners_details_movement_reason,
                            @runners_details_player_id, @runners_details_player_full_name,
                            @runners_details_responsible_pitcher_id, @runners_details_is_scoring_event,
                            @runners_details_rbi, @runners_details_earned, @runners_details_team_unearned,
                            @runners_details_play_index, @runners_credits
                        );";

                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@game_pk", playRunner.GamePk);
                    cmd.Parameters.AddWithValue("@at_bat_index", (object?)playRunner.AtBatIndex ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@runners_movement_origin_base", (object?)playRunner.RunnersMovementOriginBase ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_movement_start", (object?)playRunner.RunnersMovementStart ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_movement_end", (object?)playRunner.RunnersMovementEnd ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_movement_out_base", (object?)playRunner.RunnersMovementOutBase ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_movement_is_out", (object?)playRunner.RunnersMovementIsOut ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_movement_out_number", (object?)playRunner.RunnersMovementOutNumber ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@runners_details_event", (object?)playRunner.RunnersDetailsEvent ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_event_type", (object?)playRunner.RunnersDetailsEventType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_movement_reason", (object?)playRunner.RunnersDetailsMovementReason ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_player_id", (object?)playRunner.RunnersDetailsPlayerId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_player_full_name", (object?)playRunner.RunnersDetailsPlayerFullName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_responsible_pitcher_id", (object?)playRunner.RunnersDetailsResponsiblePitcherId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_is_scoring_event", (object?)playRunner.RunnersDetailsIsScoringEvent ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_rbi", (object?)playRunner.RunnersDetailsRbi ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_earned", (object?)playRunner.RunnersDetailsEarned ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_team_unearned", (object?)playRunner.RunnersDetailsTeamUnearned ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_play_index", (object?)playRunner.RunnersDetailsPlayIndex ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_credits", (object?)playRunner.RunnersCredits ?? DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                }
                return Ok(new { message = "Inserted runner stats", inserted = playRunners.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }  


        public async Task<IActionResult> InsertPlayRunnersCreditsAsync(List<PlayRunnersCredits> playRunnersCredits, string season)
        {
            if (playRunnersCredits == null || playRunnersCredits.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var credit in playRunnersCredits)
                {
                    var sql = $@"
                        INSERT INTO mlb_credits_{season} (
                            game_pk, at_bat_index, runners_details_play_index,
                            runners_details_player_id, runners_credits_player_id,
                            runners_credits_position_code, runners_credits_position_name,
                            runners_credits_position_type, runners_credits_position_abbreviation,
                            runners_credits_credit
                        ) VALUES (
                            @game_pk, @at_bat_index, @runners_details_play_index,
                            @runners_details_player_id, @runners_credits_player_id,
                            @runners_credits_position_code, @runners_credits_position_name,
                            @runners_credits_position_type, @runners_credits_position_abbreviation,
                            @runners_credits_credit
                        );";

                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@game_pk", credit.GamePk);
                    cmd.Parameters.AddWithValue("@at_bat_index", (object?)credit.AtBatIndex ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_play_index", (object?)credit.RunnersDetailsPlayIndex ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_details_player_id", (object?)credit.RunnersDetailsPlayerId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_credits_player_id", (object?)credit.RunnersCreditsPlayerId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_credits_position_code", (object?)credit.RunnersCreditsPositionCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_credits_position_name", (object?)credit.RunnersCreditsPositionName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_credits_position_type", (object?)credit.RunnersCreditsPositionType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_credits_position_abbreviation", (object?)credit.RunnersCreditsPositionAbbreviation ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@runners_credits_credit", (object?)credit.RunnersCreditsCredit ?? DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                }
                return Ok(new { message = "Inserted credits stats", inserted = playRunnersCredits.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }   

        private void NormalizeEmptyStringsToNull<T>(T obj)
        {
            var stringProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

            foreach (var prop in stringProperties)
            {
                var value = prop.GetValue(obj) as string;
                if (string.IsNullOrWhiteSpace(value))
                {
                    prop.SetValue(obj, null);
                }
            }
        }

        //public async Task<ActionResult<IEnumerable<MLBPlayerGameBatting>>> GetMLBPlayerGamesBattingBySeason(string season)
        //{
        //    var tableName = $"player_game_stats_batting_{season}";
//
        //    var query = $"SELECT * FROM {tableName} LIMIT 1";
//
        //    var mlbPlayerGamesBattingBySeason = await _context.MLBPlayerGamesBatting.FromSqlRaw(query).ToListAsync();
//
        //    return mlbPlayerGamesBattingBySeason;
        //}
    }
}