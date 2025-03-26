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
    public class MLBPlayerGameDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MLBPlayerGameDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ActionResult<IEnumerable<MLBPlayerGameBatting>>> GetMLBPlayerGamesBattingBySeason(string season)
        {
            var tableName = $"player_game_stats_batting_{season}";

            var query = $"SELECT * FROM {tableName} LIMIT 1";

            var mlbPlayerGamesBattingBySeason = await _context.MLBPlayerGamesBatting.FromSqlRaw(query).ToListAsync();

            return mlbPlayerGamesBattingBySeason;
        }

        public async Task<ActionResult<IEnumerable<MLBPlayerGamePitching>>> GetMLBPlayerGamesPitchingBySeason(string season)
        {
            var tableName = $"player_game_stats_pitching_{season}";

            var query = $"SELECT * FROM {tableName} LIMIT 1";

            var mlbPlayerGamesPitchingBySeason = await _context.MLBPlayerGamesPitching.FromSqlRaw(query).ToListAsync();

            return mlbPlayerGamesPitchingBySeason;
        }

        public async Task<ActionResult<IEnumerable<MLBPlayerGameFielding>>> GetMLBPlayerGamesFieldingBySeason(string season)
        {
            var tableName = $"player_game_stats_fielding_{season}";

            var query = $"SELECT * FROM {tableName} LIMIT 1";

            var mlbPlayerGamesFieldingBySeason = await _context.MLBPlayerGamesFielding.FromSqlRaw(query).ToListAsync();

            return mlbPlayerGamesFieldingBySeason;
        }

        

        public async Task<IActionResult> BulkInsertPlayerGameBatting([FromBody] List<MLBPlayerGameBatting> playerStats, [FromQuery] string season)
        {
            if (playerStats == null || playerStats.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var stat in playerStats)
                {
                    var sql = $@"
                        INSERT INTO player_game_stats_batting_{season}
                            (game_pk, team_side, team_name, player_id, person_id, summary, games_played, fly_outs, ground_outs, air_outs, runs, doubles, triples, 
                             home_runs, strike_outs, base_on_balls, intentional_walks, hits, hit_by_pitch, at_bats, caught_stealing, stolen_bases, 
                             stolen_base_percentage, ground_into_double_play, ground_into_triple_play, plate_appearances, total_bases, rbi, left_on_base, 
                             sac_bunts, sac_flies, catchers_interference, pickoffs, at_bats_per_home_run, pop_outs, line_outs, note)
                        VALUES
                            (@game_pk, @team_side, @team_name, @player_id, @person_id, @summary, @games_played, @fly_outs, @ground_outs, @air_outs, @runs, @doubles, @triples, 
                             @home_runs, @strike_outs, @base_on_balls, @intentional_walks, @hits, @hit_by_pitch, @at_bats, @caught_stealing, @stolen_bases, 
                             @stolen_base_percentage, @ground_into_double_play, @ground_into_triple_play, @plate_appearances, @total_bases, @rbi, @left_on_base, 
                             @sac_bunts, @sac_flies, @catchers_interference, @pickoffs, @at_bats_per_home_run, @pop_outs, @line_outs, @note)
                        ON CONFLICT (game_pk, player_id) DO NOTHING;";

                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@game_pk", stat.GamePk);
                    cmd.Parameters.AddWithValue("@team_side", stat.TeamSide ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@team_name", stat.TeamName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@player_id", stat.PlayerId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@person_id", stat.PersonId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@summary", stat.Summary ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@games_played", stat.GamesPlayed ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fly_outs", stat.FlyOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ground_outs", stat.GroundOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@air_outs", stat.AirOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@runs", stat.Runs ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@doubles", stat.Doubles ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@triples", stat.Triples ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_runs", stat.HomeRuns ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@strike_outs", stat.StrikeOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@base_on_balls", stat.BaseOnBalls ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@intentional_walks", stat.IntentionalWalks ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@hits", stat.Hits ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@hit_by_pitch", stat.HitByPitch ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@at_bats", stat.AtBats ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@caught_stealing", stat.CaughtStealing ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@stolen_bases", stat.StolenBases ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@stolen_base_percentage", stat.StolenBasePercentage ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ground_into_double_play", stat.GroundIntoDoublePlay ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ground_into_triple_play", stat.GroundIntoTriplePlay ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@plate_appearances", stat.PlateAppearances ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@total_bases", stat.TotalBases ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@rbi", stat.Rbi ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@left_on_base", stat.LeftOnBase ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sac_bunts", stat.SacBunts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sac_flies", stat.SacFlies ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@catchers_interference", stat.CatchersInterference ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pickoffs", stat.Pickoffs ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@at_bats_per_home_run", stat.AtBatsPerHomeRun ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pop_outs", stat.PopOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@line_outs", stat.LineOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@note", stat.Note ?? (object)DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                }

                return Ok(new { message = "Inserted batting stats", inserted = playerStats.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        //public async Task<IActionResult> CreateMLBPlayerGamesBatting([FromBody] MLBPlayerGameBatting mLBPlayerGameBatting, string season)
        //{
        //    // Implement logic to create a new league game in the database
        //    try
        //    {
        //        if (mLBPlayerGameBatting == null)
        //        {
        //            return BadRequest("Invalid MLB Player Games data");
        //        }
//
        //        var connectionString = _configuration.GetConnectionString("WebApiDatabase");
//
        //        using (var connection = new NpgsqlConnection(connectionString))
        //        {
        //            await connection.OpenAsync();
//
        //            var sql = $@"
        //                INSERT INTO player_game_stats_batting_{season}
        //                    (game_pk, team_side, team_name, player_id, person_id, at_bats, runs, hits, 
        //                    doubles, triples, home_runs, rbi, strike_outs, base_on_balls, intentional_walks, 
        //                    hit_by_pitch, sac_bunts, sac_flies, ground_into_double_play, stolen_bases, caught_stealing, avg, obp, slg, ops)
        //                VALUES
        //                    (@game_pk, @team_side, @team_name, @player_id, @person_id, @at_bats, @runs, @hits, 
        //                    @doubles, @triples, @home_runs, @rbi, @strike_outs, @base_on_balls, @intentional_walks, 
        //                    @hit_by_pitch, @sac_bunts, @sac_flies, @ground_into_double_play, @stolen_bases, @caught_stealing, 
        //                    @avg, @obp, @slg, @ops);";
        //            
        //            using (var cmd = new NpgsqlCommand(sql, connection))
        //            {
        //                cmd.Parameters.AddWithValue("@game_pk", mLBPlayerGameBatting.GamePk);
        //                cmd.Parameters.AddWithValue("@team_side", mLBPlayerGameBatting.TeamSide ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@team_name", mLBPlayerGameBatting.TeamName ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@player_id", mLBPlayerGameBatting.PlayerId ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@person_id", mLBPlayerGameBatting.PersonId ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@at_bats", mLBPlayerGameBatting.AtBats ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@runs", mLBPlayerGameBatting.Runs ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@hits", mLBPlayerGameBatting.Hits ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@doubles", mLBPlayerGameBatting.Doubles ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@triples", mLBPlayerGameBatting.Triples ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@home_runs", mLBPlayerGameBatting.HomeRuns ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@rbi", mLBPlayerGameBatting.Rbi ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@strike_outs", mLBPlayerGameBatting.StrikeOuts ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@base_on_balls", mLBPlayerGameBatting.BaseOnBalls ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@intentional_walks", mLBPlayerGameBatting.IntentionalWalks ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@hit_by_pitch", mLBPlayerGameBatting.HitByPitch ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@sac_bunts", mLBPlayerGameBatting.SacBunts ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@sac_flies", mLBPlayerGameBatting.SacFlies ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@ground_into_double_play", mLBPlayerGameBatting.GroundIntoDoublePlay ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@stolen_bases", mLBPlayerGameBatting.StolenBases ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@caught_stealing", mLBPlayerGameBatting.CaughtStealing ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@avg", mLBPlayerGameBatting.Avg ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@obp", mLBPlayerGameBatting.Obp ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@slg", mLBPlayerGameBatting.Slg ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@ops", mLBPlayerGameBatting.Ops ?? (object)DBNull.Value);
        //            
        //                await cmd.ExecuteNonQueryAsync();
        //            }
        //        }
//
        //        return StatusCode(201, mLBPlayerGameBatting);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal Server Error: {ex}");
        //    }
        //}

        public async Task<IActionResult> BulkInsertPlayerGamePitching([FromBody] List<MLBPlayerGamePitching> playerStats, string season)
        {
            if (playerStats == null || playerStats.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var stat in playerStats)
                {
                    var sql = $@"
                        INSERT INTO player_game_stats_pitching_{season}
                            (game_pk, team_side, team_name, player_id, person_id, note, summary, games_played, games_started,
                             fly_outs, ground_outs, air_outs, runs, doubles, triples, home_runs, strike_outs, base_on_balls,
                             intentional_walks, hits, hit_by_pitch, at_bats, caught_stealing, stolen_bases, stolen_base_percentage,
                             number_of_pitches, innings_pitched, wins, losses, saves, save_opportunities, holds, blown_saves,
                             earned_runs, batters_faced, outs, games_pitched, complete_games, shutouts, pitches_thrown, balls,
                             strikes, strike_percentage, hit_batsmen, balks, wild_pitches, pickoffs, rbi, games_finished,
                             runs_scored_per9, home_runs_per9, inherited_runners, inherited_runners_scored, catchers_interference,
                             sac_bunts, sac_flies, passed_ball, pop_outs, line_outs)
                        VALUES
                            (@game_pk, @team_side, @team_name, @player_id, @person_id, @note, @summary, @games_played, @games_started,
                             @fly_outs, @ground_outs, @air_outs, @runs, @doubles, @triples, @home_runs, @strike_outs, @base_on_balls,
                             @intentional_walks, @hits, @hit_by_pitch, @at_bats, @caught_stealing, @stolen_bases, @stolen_base_percentage,
                             @number_of_pitches, @innings_pitched, @wins, @losses, @saves, @save_opportunities, @holds, @blown_saves,
                             @earned_runs, @batters_faced, @outs, @games_pitched, @complete_games, @shutouts, @pitches_thrown, @balls,
                             @strikes, @strike_percentage, @hit_batsmen, @balks, @wild_pitches, @pickoffs, @rbi, @games_finished,
                             @runs_scored_per9, @home_runs_per9, @inherited_runners, @inherited_runners_scored, @catchers_interference,
                             @sac_bunts, @sac_flies, @passed_ball, @pop_outs, @line_outs)
                        ON CONFLICT (game_pk, player_id) DO NOTHING;";

                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@game_pk", stat.GamePk);
                    cmd.Parameters.AddWithValue("@team_side", stat.TeamSide ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@team_name", stat.TeamName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@player_id", stat.PlayerId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@person_id", stat.PersonId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@note", stat.Note ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@summary", stat.Summary ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@games_played", stat.GamesPlayed ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@games_started", stat.GamesStarted ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fly_outs", stat.FlyOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ground_outs", stat.GroundOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@air_outs", stat.AirOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@runs", stat.Runs ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@doubles", stat.Doubles ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@triples", stat.Triples ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_runs", stat.HomeRuns ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@strike_outs", stat.StrikeOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@base_on_balls", stat.BaseOnBalls ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@intentional_walks", stat.IntentionalWalks ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@hits", stat.Hits ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@hit_by_pitch", stat.HitByPitch ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@at_bats", stat.AtBats ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@caught_stealing", stat.CaughtStealing ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@stolen_bases", stat.StolenBases ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@stolen_base_percentage", stat.StolenBasePercentage ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@number_of_pitches", stat.NumberOfPitches ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@innings_pitched", stat.InningsPitched ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@wins", stat.Wins ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@losses", stat.Losses ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@saves", stat.Saves ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@save_opportunities", stat.SaveOpportunities ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@holds", stat.Holds ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@blown_saves", stat.BlownSaves ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@earned_runs", stat.EarnedRuns ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@batters_faced", stat.BattersFaced ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@outs", stat.Outs ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@games_pitched", stat.GamesPitched ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@complete_games", stat.CompleteGames ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@shutouts", stat.Shutouts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitches_thrown", stat.PitchesThrown ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@balls", stat.Balls ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@strikes", stat.Strikes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@strike_percentage", stat.StrikePercentage ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@hit_batsmen", stat.HitBatsmen ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@balks", stat.Balks ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@wild_pitches", stat.WildPitches ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pickoffs", stat.Pickoffs ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@rbi", stat.Rbi ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@games_finished", stat.GamesFinished ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@runs_scored_per9", stat.RunsScoredPer9 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@home_runs_per9", stat.HomeRunsPer9 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@inherited_runners", stat.InheritedRunners ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@inherited_runners_scored", stat.InheritedRunnersScored ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@catchers_interference", stat.CatchersInterference ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sac_bunts", stat.SacBunts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sac_flies", stat.SacFlies ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@passed_ball", stat.PassedBall ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pop_outs", stat.PopOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@line_outs", stat.LineOuts ?? (object)DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                }

                return Ok(new { message = "Inserted pitching stats", inserted = playerStats.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



        //public async Task<IActionResult> CreateMLBPlayerGamesPitching([FromBody] MLBPlayerGamePitching mLBPlayerGamePitching, string season)
        //{
        //    // Implement logic to create a new league game in the database
        //    try
        //    {
        //        if (mLBPlayerGamePitching == null)
        //        {
        //            return BadRequest("Invalid MLB Player Games data");
        //        }
//
        //        var connectionString = _configuration.GetConnectionString("WebApiDatabase");
//
        //        using (var connection = new NpgsqlConnection(connectionString))
        //        {
        //            await connection.OpenAsync();
//
        //            var sql = $@"
        //                INSERT INTO player_game_stats_pitching_{season}
        //                    (game_pk, team_side, team_name, player_id, person_id, wins, losses, saves, 
        //                    save_opportunities, holds, blown_saves, innings_pitched, hits, runs, earned_runs, 
        //                    home_runs, base_on_balls, strike_outs, hit_batsmen, wild_pitches, balks, 
        //                    era, whip, baa)
        //                VALUES
        //                    (@game_pk, @team_side, @team_name, @player_id, @person_id, @wins, @losses, @saves, 
        //                    @save_opportunities, @holds, @blown_saves, @innings_pitched, @hits, @runs, @earned_runs, 
        //                    @home_runs, @base_on_balls, @strike_outs, @hit_batsmen, @wild_pitches, @balks, 
        //                    @era, @whip, @baa);";
//
        //            using (var cmd = new NpgsqlCommand(sql, connection))
        //            {
        //                cmd.Parameters.AddWithValue("@game_pk", mLBPlayerGamePitching.GamePk);
        //                cmd.Parameters.AddWithValue("@team_side", mLBPlayerGamePitching.TeamSide ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@team_name", mLBPlayerGamePitching.TeamName ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@player_id", mLBPlayerGamePitching.PlayerId ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@person_id", mLBPlayerGamePitching.PersonId ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@wins", mLBPlayerGamePitching.Wins ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@losses", mLBPlayerGamePitching.Losses ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@saves", mLBPlayerGamePitching.Saves ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@save_opportunities", mLBPlayerGamePitching.SaveOpportunities ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@holds", mLBPlayerGamePitching.Holds ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@blown_saves", mLBPlayerGamePitching.BlownSaves ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@innings_pitched", mLBPlayerGamePitching.InningsPitched ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@hits", mLBPlayerGamePitching.HitsAllowed ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@runs", mLBPlayerGamePitching.RunsAllowed ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@earned_runs", mLBPlayerGamePitching.EarnedRuns ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@home_runs", mLBPlayerGamePitching.HomeRunsAllowed ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@base_on_balls", mLBPlayerGamePitching.BaseOnBallsAllowed ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@strike_outs", mLBPlayerGamePitching.StrikeOuts ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@hit_batsmen", mLBPlayerGamePitching.HitBatsmen ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@wild_pitches", mLBPlayerGamePitching.WildPitches ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@balks", mLBPlayerGamePitching.Balks ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@era", mLBPlayerGamePitching.Era ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@whip", mLBPlayerGamePitching.Whip ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@baa", mLBPlayerGamePitching.Baa ?? (object)DBNull.Value);
//
        //                await cmd.ExecuteNonQueryAsync();
        //            }
        //        }
//
        //        return StatusCode(201, mLBPlayerGamePitching);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal Server Error: {ex}");
        //    }
        //}

        public async Task<IActionResult> BulkInsertPlayerGameFielding([FromBody] List<MLBPlayerGameFielding> playerStats, string season)
        {
            if (playerStats == null || playerStats.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var stat in playerStats)
                {
                    var sql = $@"
                        INSERT INTO player_game_stats_fielding_{season}
                            (game_pk, team_side, team_name, player_id, person_id,
                             caught_stealing, stolen_bases, stolen_base_percentage,
                             assists, put_outs, errors, chances,
                             fielding, passed_ball, pickoffs, games_started)
                        VALUES
                            (@game_pk, @team_side, @team_name, @player_id, @person_id,
                             @caught_stealing, @stolen_bases, @stolen_base_percentage,
                             @assists, @put_outs, @errors, @chances,
                             @fielding, @passed_ball, @pickoffs, @games_started)
                        ON CONFLICT (game_pk, player_id) DO NOTHING;";


                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@game_pk", stat.GamePk);
                    cmd.Parameters.AddWithValue("@team_side", stat.TeamSide ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@team_name", stat.TeamName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@player_id", stat.PlayerId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@person_id", stat.PersonId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@caught_stealing", stat.CaughtStealing ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@stolen_bases", stat.StolenBases ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@stolen_base_percentage", stat.StolenBasePercentage ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@assists", stat.Assists ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@put_outs", stat.PutOuts ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@errors", stat.Errors ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@chances", stat.Chances ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@fielding", stat.Fielding ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@passed_ball", stat.PassedBall ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pickoffs", stat.Pickoffs ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@games_started", stat.GamesStarted ?? (object)DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                }

                return Ok(new { message = "Inserted fielding stats", inserted = playerStats.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



        //public async Task<IActionResult> CreateMLBPlayerGamesFielding([FromBody] MLBPlayerGameFielding mLBPlayerGameFielding, string season)
        //{
        //    // Implement logic to create a new league game in the database
        //    try
        //    {
        //        if (mLBPlayerGameFielding == null)
        //        {
        //            return BadRequest("Invalid MLB Player Games Fielding data");
        //        }
//
        //        var connectionString = _configuration.GetConnectionString("WebApiDatabase");
//
        //        using (var connection = new NpgsqlConnection(connectionString))
        //        {
        //            await connection.OpenAsync();
//
        //            var sql = $@"
        //                INSERT INTO player_game_stats_fielding_{season}
        //                    (game_pk, team_side, team_name, player_id, person_id, position, assists, 
        //                    put_outs, errors, double_plays, passed_balls, wild_pitches, stolen_bases_allowed, 
        //                    caught_stealing, fielding_percentage, range_factor_per_game, range_factor_per_9_innings, innings)
        //                VALUES
        //                    (@game_pk, @team_side, @team_name, @player_id, @person_id, @position, @assists, 
        //                    @put_outs, @errors, @double_plays, @passed_balls, @wild_pitches, @stolen_bases_allowed, 
        //                    @caught_stealing, @fielding_percentage, @range_factor_per_game, @range_factor_per_9_innings, @innings);";
//
        //            using (var cmd = new NpgsqlCommand(sql, connection))
        //            {
        //                cmd.Parameters.AddWithValue("@game_pk", mLBPlayerGameFielding.GamePk);
        //                cmd.Parameters.AddWithValue("@team_side", mLBPlayerGameFielding.TeamSide ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@team_name", mLBPlayerGameFielding.TeamName ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@player_id", mLBPlayerGameFielding.PlayerId ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@person_id", mLBPlayerGameFielding.PersonId ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@position", mLBPlayerGameFielding.Position ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@assists", mLBPlayerGameFielding.Assists ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@put_outs", mLBPlayerGameFielding.PutOuts ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@errors", mLBPlayerGameFielding.Errors ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@double_plays", mLBPlayerGameFielding.DoublePlays ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@passed_balls", mLBPlayerGameFielding.PassedBalls ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@wild_pitches", mLBPlayerGameFielding.WildPitches ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@stolen_bases_allowed", mLBPlayerGameFielding.StolenBasesAllowed ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@caught_stealing", mLBPlayerGameFielding.CaughtStealing ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@fielding_percentage", mLBPlayerGameFielding.FieldingPercentage ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@range_factor_per_game", mLBPlayerGameFielding.RangeFactorPerGame ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@range_factor_per_9_innings", mLBPlayerGameFielding.RangeFactorPerNineInnings ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@innings", mLBPlayerGameFielding.Innings ?? (object)DBNull.Value);
//
//
        //            
        //                await cmd.ExecuteNonQueryAsync();
        //            }
        //        }
//
        //        return StatusCode(201, mLBPlayerGameFielding);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal Server Error: {ex}");
        //    }
        //}
    }

}

