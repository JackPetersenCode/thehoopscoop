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
    public class SportRadarMLBEGSDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public SportRadarMLBEGSDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> CreateSportRadarMLBEGSGameInfo([FromBody] List<SportRadarMLBEGSGameInfo> eGSGameInfo, [FromQuery] string season)
        {
            if (eGSGameInfo == null || eGSGameInfo.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var game in eGSGameInfo)
                {
                    var table = $@"sportradar_mlb_egs_game_info_{season}";
                    var sql = $@"
                        INSERT INTO {table} (
                            game_id, status, coverage, game_number, day_night, scheduled,
                            home_team_id, away_team_id, attendance, duration,
                            season_id, season_type, season_year, double_header, entry_mode, reference,
                            time_zones_venue, time_zones_home, time_zones_away,
                            venue_name, venue_market, venue_capacity, venue_surface, venue_address, venue_city, venue_state, venue_zip, venue_country,
                            venue_id, venue_field_orientation, venue_stadium_type, venue_time_zone, venue_location_lat, venue_location_lng
                        ) VALUES (
                            @game_id, @status, @coverage, @game_number, @day_night, @scheduled,
                            @home_team_id, @away_team_id, @attendance, @duration,
                            @season_id, @season_type, @season_year, @double_header, @entry_mode, @reference,
                            @time_zones_venue, @time_zones_home, @time_zones_away,
                            @venue_name, @venue_market, @venue_capacity, @venue_surface, @venue_address, @venue_city, @venue_state, @venue_zip, @venue_country,
                            @venue_id, @venue_field_orientation, @venue_stadium_type, @venue_time_zone, @venue_location_lat, @venue_location_lng
                        )
                        ON CONFLICT (game_id) DO NOTHING;";
                    ;

                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@game_id", (object?)game.GameId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", (object?)game.Status ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@coverage", (object?)game.Coverage ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_number", (object?)game.GameNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@day_night", (object?)game.DayNight ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@scheduled", (object?)game.Scheduled ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_team_id", (object?)game.HomeTeamId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@away_team_id", (object?)game.AwayTeamId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@attendance", (object?)game.Attendance ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@duration", (object?)game.Duration ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@season_id", (object?)game.SeasonId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@season_type", (object?)game.SeasonType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@season_year", (object?)game.SeasonYear ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@double_header", (object?)game.DoubleHeader ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@entry_mode", (object?)game.EntryMode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@reference", (object?)game.Reference ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@time_zones_venue", (object?)game.TimeZonesVenue ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@time_zones_home", (object?)game.TimeZonesHome ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@time_zones_away", (object?)game.TimeZonesAway ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_name", (object?)game.VenueName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_market", (object?)game.VenueMarket ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_capacity", (object?)game.VenueCapacity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_surface", (object?)game.VenueSurface ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_address", (object?)game.VenueAddress ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_city", (object?)game.VenueCity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_state", (object?)game.VenueState ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_zip", (object?)game.VenueZip ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_country", (object?)game.VenueCountry ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_id", (object?)game.VenueId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_field_orientation", (object?)game.VenueFieldOrientation ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_stadium_type", (object?)game.VenueStadiumType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_time_zone", (object?)game.VenueTimeZone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_location_lat", (object?)game.VenueLocationLat ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_location_lng", (object?)game.VenueLocationLng ?? DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();

                }

                return Ok(new { message = "Inserted egs game info", inserted = eGSGameInfo.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        public async Task<IActionResult> CreateSportRadarMLBLeagueSchedule([FromBody] List<SportRadarMLBLeagueSchedule> leagueSchedule, [FromQuery] string season)
        {
            if (leagueSchedule == null || leagueSchedule.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var game in leagueSchedule)
                {
                    var table = $@"sportradar_mlb_league_schedule_{season}";
                    var sql = $@"
                        INSERT INTO {table}
                            (league_alias, league_name, league_id, season_id, season_year, season_type,
                             game_id, game_status, game_coverage, game_game_number, game_day_night, game_scheduled,
                             game_home_team, game_away_team, game_attendance, game_duration, game_double_header,
                             game_entry_mode, game_reference,
                             venue_name, venue_market, venue_capacity, venue_surface, venue_address, venue_city, venue_state,
                             venue_zip, venue_country, venue_id, venue_field_orientation, venue_stadium_type, venue_time_zone,
                             venue_location_lat, venue_location_lng,
                             home_name, home_market, home_abbr, home_id, home_win, home_loss,
                             away_name, away_market, away_abbr, away_id, away_win, away_loss,
                             broadcast_1_network, broadcast_1_type, broadcast_1_locale, broadcast_1_channel,
                             broadcast_2_network, broadcast_2_type, broadcast_2_locale, broadcast_2_channel,
                             broadcast_3_network, broadcast_3_type, broadcast_3_locale, broadcast_3_channel,
                             game_rescheduled, game_parent_id)
                        VALUES
                            (@league_alias, @league_name, @league_id, @season_id, @season_year, @season_type,
                             @game_id, @game_status, @game_coverage, @game_game_number, @game_day_night, @game_scheduled,
                             @game_home_team, @game_away_team, @game_attendance, @game_duration, @game_double_header,
                             @game_entry_mode, @game_reference,
                             @venue_name, @venue_market, @venue_capacity, @venue_surface, @venue_address, @venue_city, @venue_state,
                             @venue_zip, @venue_country, @venue_id, @venue_field_orientation, @venue_stadium_type, @venue_time_zone,
                             @venue_location_lat, @venue_location_lng,
                             @home_name, @home_market, @home_abbr, @home_id, @home_win, @home_loss,
                             @away_name, @away_market, @away_abbr, @away_id, @away_win, @away_loss,
                             @broadcast_1_network, @broadcast_1_type, @broadcast_1_locale, @broadcast_1_channel,
                             @broadcast_2_network, @broadcast_2_type, @broadcast_2_locale, @broadcast_2_channel,
                             @broadcast_3_network, @broadcast_3_type, @broadcast_3_locale, @broadcast_3_channel,
                             @game_rescheduled, @game_parent_id);";

                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@league_alias", (object?)game.LeagueAlias ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@league_name", (object?)game.LeagueName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@league_id", (object?)game.LeagueId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@season_id", (object?)game.SeasonId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@season_year", (object?)game.SeasonYear ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@season_type", (object?)game.SeasonType ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@game_id", (object?)game.GameId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_status", (object?)game.GameStatus ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_coverage", (object?)game.GameCoverage ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_game_number", (object?)game.GameGameNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_day_night", (object?)game.GameDayNight ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_scheduled", (object?)game.GameScheduled ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_home_team", (object?)game.GameHomeTeam ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_away_team", (object?)game.GameAwayTeam ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_attendance", (object?)game.GameAttendance ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_duration", (object?)game.GameDuration ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_double_header", (object?)game.GameDoubleHeader ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_entry_mode", (object?)game.GameEntryMode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_reference", (object?)game.GameReference ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@venue_name", (object?)game.VenueName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_market", (object?)game.VenueMarket ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_capacity", (object?)game.VenueCapacity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_surface", (object?)game.VenueSurface ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_address", (object?)game.VenueAddress ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_city", (object?)game.VenueCity ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_state", (object?)game.VenueState ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_zip", (object?)game.VenueZip ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_country", (object?)game.VenueCountry ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_id", (object?)game.VenueId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_field_orientation", (object?)game.VenueFieldOrientation ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_stadium_type", (object?)game.VenueStadiumType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_time_zone", (object?)game.VenueTimeZone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_location_lat", (object?)game.VenueLocationLat ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@venue_location_lng", (object?)game.VenueLocationLng ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@home_name", (object?)game.HomeName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_market", (object?)game.HomeMarket ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_abbr", (object?)game.HomeAbbr ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_id", (object?)game.HomeId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_win", (object?)game.HomeWin ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_loss", (object?)game.HomeLoss ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@away_name", (object?)game.AwayName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@away_market", (object?)game.AwayMarket ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@away_abbr", (object?)game.AwayAbbr ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@away_id", (object?)game.AwayId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@away_win", (object?)game.AwayWin ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@away_loss", (object?)game.AwayLoss ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@broadcast_1_network", (object?)game.Broadcast1Network ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_1_type", (object?)game.Broadcast1Type ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_1_locale", (object?)game.Broadcast1Locale ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_1_channel", (object?)game.Broadcast1Channel ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_2_network", (object?)game.Broadcast2Network ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_2_type", (object?)game.Broadcast2Type ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_2_locale", (object?)game.Broadcast2Locale ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_2_channel", (object?)game.Broadcast2Channel ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_3_network", (object?)game.Broadcast3Network ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_3_type", (object?)game.Broadcast3Type ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_3_locale", (object?)game.Broadcast3Locale ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@broadcast_3_channel", (object?)game.Broadcast3Channel ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@game_rescheduled", (object?)game.GameRescheduled ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@game_parent_id", (object?)game.GameParentId ?? DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();

                }

                return Ok(new { message = "Inserted mlb league schedule", inserted = leagueSchedule.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> CreateSportRadarMLBPBPAtBats([FromBody] List<SportRadarMLBPBPAtBat> sportRadarMLBPBPAtBats, [FromQuery] string season)
        {
            if (sportRadarMLBPBPAtBats == null || sportRadarMLBPBPAtBats.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var game in sportRadarMLBPBPAtBats)
                {
                    var table = $@"sportradar_mlb_pbp_at_bats_{season}";
                    var sql = $@"
                    INSERT INTO {table} (
                      game_id, inning, half, hitter_id, at_bat_id, hitter_hand, pitcher_id, pitcher_hand,
                      sequence_number, description,
                      hitter_preferred_name, hitter_first_name, hitter_last_name, hitter_jersey_number, hitter_full_name,
                      pitcher_preferred_name, pitcher_first_name, pitcher_last_name, pitcher_jersey_number, pitcher_full_name,
                      home_team_runs, away_team_runs
                    ) VALUES (
                      @game_id, @inning, @half, @hitter_id, @at_bat_id, @hitter_hand, @pitcher_id, @pitcher_hand,
                      @sequence_number, @description,
                      @hitter_preferred_name, @hitter_first_name, @hitter_last_name, @hitter_jersey_number, @hitter_full_name,
                      @pitcher_preferred_name, @pitcher_first_name, @pitcher_last_name, @pitcher_jersey_number, @pitcher_full_name,
                      @home_team_runs, @away_team_runs
                    );";

                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@game_id", (object?)game.GameId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@inning", (object?)game.Inning ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@half", (object?)game.Half ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_id", (object?)game.HitterId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@at_bat_id", (object?)game.AtBatId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_hand", (object?)game.HitterHand ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_id", (object?)game.PitcherId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_hand", (object?)game.PitcherHand ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@sequence_number", (object?)game.SequenceNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@description", (object?)game.Description ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@hitter_preferred_name", (object?)game.HitterPreferredName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_first_name", (object?)game.HitterFirstName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_last_name", (object?)game.HitterLastName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_jersey_number", (object?)game.HitterJerseyNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_full_name", (object?)game.HitterFullName ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@pitcher_preferred_name", (object?)game.PitcherPreferredName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_first_name", (object?)game.PitcherFirstName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_last_name", (object?)game.PitcherLastName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_jersey_number", (object?)game.PitcherJerseyNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_full_name", (object?)game.PitcherFullName ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@home_team_runs", (object?)game.HomeTeamRuns ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@away_team_runs", (object?)game.AwayTeamRuns ?? DBNull.Value);


                    await cmd.ExecuteNonQueryAsync();

                }

                return Ok(new { message = "Inserted pbp at bats info", inserted = sportRadarMLBPBPAtBats.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        public async Task<IActionResult> CreateSportRadarMLBPBPPitchEvents([FromBody] List<SportRadarMLBPBPPitchEvent> sportRadarMLBPBPPitchEvents, [FromQuery] string season)
        {
            if (sportRadarMLBPBPPitchEvents == null || sportRadarMLBPBPPitchEvents.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var game in sportRadarMLBPBPPitchEvents)
                {
                    var table = $@"sportradar_mlb_pbp_pitch_events_{season}";
                    var sql = $@"
                        INSERT INTO {table} (
                          game_id, at_bat_id, inning, half, hit_location, hit_type, status, event_id, outcome_id,
                          created_at, updated_at, sequence_number, official, type, wall_clock_start_time, wall_clock_end_time,
                          is_ab_over, is_bunt, is_hit, is_wild_pitch, is_passed_ball, is_double_play, is_triple_play,
                          balls, strikes, outs, pitch_count, pitch_type, pitch_speed, pitch_zone, pitcher_hand, hitter_hand,
                          pitcher_id, pitch_x, pitch_y, pitcher_preferred_name, pitcher_first_name, pitcher_last_name,
                          pitcher_jersey_number, pitcher_full_name, hitter_preferred_name, hitter_first_name, hitter_last_name,
                          hitter_jersey_number, hitter_full_name, hitter_id, home_team_runs, away_team_runs,
                          mlb_pitch_speed, mlb_strike_zone_top, mlb_strike_zone_bottom, mlb_pitch_zone, mlb_pitch_code,
                          mlb_pitch_description, mlb_pitch_x, mlb_pitch_y, mlb_hit_trajectory, mlb_hit_hardness, mlb_hit_x, mlb_hit_y
                        ) VALUES (
                          @game_id, @at_bat_id, @inning, @half, @hit_location, @hit_type, @status, @event_id, @outcome_id,
                          @created_at, @updated_at, @sequence_number, @official, @type, @wall_clock_start_time, @wall_clock_end_time,
                          @is_ab_over, @is_bunt, @is_hit, @is_wild_pitch, @is_passed_ball, @is_double_play, @is_triple_play,
                          @balls, @strikes, @outs, @pitch_count, @pitch_type, @pitch_speed, @pitch_zone, @pitcher_hand, @hitter_hand,
                          @pitcher_id, @pitch_x, @pitch_y, @pitcher_preferred_name, @pitcher_first_name, @pitcher_last_name,
                          @pitcher_jersey_number, @pitcher_full_name, @hitter_preferred_name, @hitter_first_name, @hitter_last_name,
                          @hitter_jersey_number, @hitter_full_name, @hitter_id, @home_team_runs, @away_team_runs,
                          @mlb_pitch_speed, @mlb_strike_zone_top, @mlb_strike_zone_bottom, @mlb_pitch_zone, @mlb_pitch_code,
                          @mlb_pitch_description, @mlb_pitch_x, @mlb_pitch_y, @mlb_hit_trajectory, @mlb_hit_hardness, @mlb_hit_x, @mlb_hit_y
                        );";

                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@game_id", (object?)game.GameId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@at_bat_id", (object?)game.AtBatId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@inning", (object?)game.Inning ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@half", (object?)game.Half ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@hit_location", (object?)game.HitLocation ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hit_type", (object?)game.HitType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", (object?)game.Status ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@event_id", (object?)game.EventId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@outcome_id", (object?)game.OutcomeId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@created_at", (object?)game.CreatedAt ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@updated_at", (object?)game.UpdatedAt ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@sequence_number", (object?)game.SequenceNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@official", (object?)game.Official ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@type", (object?)game.Type ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@wall_clock_start_time", (object?)game.WallClockStartTime ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@wall_clock_end_time", (object?)game.WallClockEndTime ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@is_ab_over", (object?)game.IsAbOver ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@is_bunt", (object?)game.IsBunt ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@is_hit", (object?)game.IsHit ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@is_wild_pitch", (object?)game.IsWildPitch ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@is_passed_ball", (object?)game.IsPassedBall ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@is_double_play", (object?)game.IsDoublePlay ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@is_triple_play", (object?)game.IsTriplePlay ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@balls", (object?)game.Balls ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@strikes", (object?)game.Strikes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@outs", (object?)game.Outs ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@pitch_count", (object?)game.PitchCount ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitch_type", (object?)game.PitchType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitch_speed", (object?)game.PitchSpeed ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitch_zone", (object?)game.PitchZone ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@pitcher_hand", (object?)game.PitcherHand ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_hand", (object?)game.HitterHand ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_id", (object?)game.PitcherId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitch_x", (object?)game.PitchX ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitch_y", (object?)game.PitchY ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@pitcher_preferred_name", (object?)game.PitcherPreferredName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_first_name", (object?)game.PitcherFirstName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_last_name", (object?)game.PitcherLastName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_jersey_number", (object?)game.PitcherJerseyNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitcher_full_name", (object?)game.PitcherFullName ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@hitter_preferred_name", (object?)game.HitterPreferredName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_first_name", (object?)game.HitterFirstName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_last_name", (object?)game.HitterLastName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_jersey_number", (object?)game.HitterJerseyNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_full_name", (object?)game.HitterFullName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@hitter_id", (object?)game.HitterId ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@home_team_runs", (object?)game.HomeTeamRuns ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@away_team_runs", (object?)game.AwayTeamRuns ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@mlb_pitch_speed", (object?)game.MlbPitchSpeed ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_strike_zone_top", (object?)game.MlbStrikeZoneTop ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_strike_zone_bottom", (object?)game.MlbStrikeZoneBottom ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_pitch_zone", (object?)game.MlbPitchZone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_pitch_code", (object?)game.MlbPitchCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_pitch_description", (object?)game.MlbPitchDescription ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_pitch_x", (object?)game.MlbPitchX ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_pitch_y", (object?)game.MlbPitchY ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_hit_trajectory", (object?)game.MlbHitTrajectory ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_hit_hardness", (object?)game.MlbHitHardness ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_hit_x", (object?)game.MlbHitX ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@mlb_hit_y", (object?)game.MlbHitY ?? DBNull.Value);


                    await cmd.ExecuteNonQueryAsync();

                }

                return Ok(new { message = "Inserted pbp pitch events info", inserted = sportRadarMLBPBPPitchEvents.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}