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
    public class MLBGameDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MLBGameDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ActionResult<IEnumerable<MLBGame>>> GetMLBGamesBySeason(string season)
        {
            var tableName = $"mlb_games_{season}";

            var query = $"SELECT * FROM {tableName} LIMIT 1";

            var mlbGamesBySeason = await _context.MLBGames.FromSqlRaw(query).ToListAsync();

            return mlbGamesBySeason;
        }
        public async Task<IActionResult> CreateMLBGames([FromBody] MLBGame mLBGame, string season)
        {
            // Implement logic to create a new league game in the database
            try
            {
                if (mLBGame == null)
                {
                    return BadRequest("Invalid MLB Games data");
                }

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = $@"
                        INSERT INTO mlb_games_{season} 
                        (game_pk, game_guid, link, game_type, season, game_date, official_date, abstract_game_state, coded_game_state, 
                         detailed_state, status_code, start_time_tbd, abstract_game_code, away_team_id, away_team_name, away_score, away_wins, 
                         away_losses, away_win_pct, away_is_winner, home_team_id, home_team_name, home_score, home_wins, 
                         home_losses, home_win_pct, home_is_winner, venue_id, venue_name, is_tie, game_number, double_header, 
                         day_night, description, scheduled_innings, games_in_series, series_game_number, series_description, 
                         if_necessary, if_necessary_desc) 
                        VALUES 
                        (@game_pk, @game_guid, @link, @game_type, @season, @game_date, @official_date, @abstract_game_state, 
                         @coded_game_state, @detailed_state, @status_code, @start_time_tbd, @abstract_game_code, @away_team_id, @away_team_name, 
                         @away_score, @away_wins, @away_losses, @away_win_pct, @away_is_winner, @home_team_id, @home_team_name, 
                         @home_score, @home_wins, @home_losses, @home_win_pct, @home_is_winner, @venue_id, @venue_name, @is_tie, 
                         @game_number, @double_header, @day_night, @description, @scheduled_innings, @games_in_series, 
                         @series_game_number, @series_description, @if_necessary, @if_necessary_desc);";

                    
                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@game_pk", mLBGame.GamePk);
                        cmd.Parameters.AddWithValue("@game_guid", mLBGame.GameGuid ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@link", mLBGame.Link ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@game_type", mLBGame.GameType ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@season", mLBGame.Season ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@game_date", mLBGame.GameDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@official_date", mLBGame.OfficialDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@abstract_game_state", mLBGame.AbstractGameState ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@coded_game_state", mLBGame.CodedGameState ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@detailed_state", mLBGame.DetailedState ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@status_code", mLBGame.StatusCode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@start_time_tbd", mLBGame.StartTimeTbd ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@abstract_game_code", mLBGame.AbstractGameCode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@away_team_id", mLBGame.AwayTeamId);
                        cmd.Parameters.AddWithValue("@away_team_name", mLBGame.AwayTeamName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@away_score", mLBGame.AwayScore ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@away_wins", mLBGame.AwayWins ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@away_losses", mLBGame.AwayLosses ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@away_win_pct", mLBGame.AwayWinPct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@away_is_winner", mLBGame.AwayIsWinner ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_team_id", mLBGame.HomeTeamId);
                        cmd.Parameters.AddWithValue("@home_team_name", mLBGame.HomeTeamName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_score", mLBGame.HomeScore ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_wins", mLBGame.HomeWins ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_losses", mLBGame.HomeLosses ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_win_pct", mLBGame.HomeWinPct ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@home_is_winner", mLBGame.HomeIsWinner ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@venue_id", mLBGame.VenueId);
                        cmd.Parameters.AddWithValue("@venue_name", mLBGame.VenueName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@is_tie", mLBGame.IsTie ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@game_number", mLBGame.GameNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@double_header", mLBGame.DoubleHeader ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@day_night", mLBGame.DayNight ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@description", mLBGame.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@scheduled_innings", mLBGame.ScheduledInnings ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@games_in_series", mLBGame.GamesInSeries ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@series_game_number", mLBGame.SeriesGameNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@series_description", mLBGame.SeriesDescription ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@if_necessary", mLBGame.IfNecessary ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@if_necessary_desc", mLBGame.IfNecessaryDesc ?? (object)DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, mLBGame);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }
    }

}

