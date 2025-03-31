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
    public class MLBTeamDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MLBTeamDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public async Task<ActionResult<IEnumerable<MLBTeamInfo>>> GetMLBTeamInfoBySeason(string season)
        {
            var tableName = $"mlb_team_info_{season}";

            var query = $"SELECT * FROM {tableName} LIMIT 1";

            var mlbTeamInfoBySeason = await _context.MLBTeamInfos.FromSqlRaw(query).ToListAsync();

            return mlbTeamInfoBySeason;
        }

        public async Task<IActionResult> PostTeamInfoBySeason([FromBody] List<MLBTeamInfo> records, string season)
        {
            if (records == null || records.Count == 0)
                return BadRequest("Empty list");

            var tableName = $"mlb_team_info_{season}";
            var connectionString = _configuration.GetConnectionString("WebApiDatabase");

            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            foreach (var r in records)
            {
                var cmd = new NpgsqlCommand($@"
                    INSERT INTO {tableName} (
                        game_pk, team_side, team_id, team_name, season, spring_league_id, spring_league_name, spring_league_abbreviation, 
                        all_star_status, venue_id, venue_name, team_code, file_code, abbreviation, location_name, first_year_of_play, 
                        league_id, league_name, sport_id, sport_name, short_name, record_games_played, record_wild_card_games_back, 
                        record_league_games_back, record_spring_league_games_back, record_sport_games_back, record_division_games_back, 
                        record_conference_games_back, record_league_record_wins, record_league_record_losses, record_league_record_ties, 
                        record_league_record_pct, record_division_leader, record_wins, record_losses, record_winning_percentage, 
                        franchise_name, club_name, active
                    ) VALUES (
                        @game_pk, @team_side, @team_id, @team_name, @season, @spring_league_id, @spring_league_name, @spring_league_abbreviation, 
                        @all_star_status, @venue_id, @venue_name, @team_code, @file_code, @abbreviation, @location_name, @first_year_of_play, 
                        @league_id, @league_name, @sport_id, @sport_name, @short_name, @record_games_played, @record_wild_card_games_back, 
                        @record_league_games_back, @record_spring_league_games_back, @record_sport_games_back, @record_division_games_back, 
                        @record_conference_games_back, @record_league_record_wins, @record_league_record_losses, @record_league_record_ties, 
                        @record_league_record_pct, @record_division_leader, @record_wins, @record_losses, @record_winning_percentage, 
                        @franchise_name, @club_name, @active
                    )
                    ON CONFLICT (game_pk, team_id) DO NOTHING;", conn);

                cmd.Parameters.AddWithValue("@game_pk", r.GamePk);
                cmd.Parameters.AddWithValue("@team_side", r.TeamSide);
                cmd.Parameters.AddWithValue("@team_id", r.TeamId);
                cmd.Parameters.AddWithValue("@team_name", r.TeamName);
                cmd.Parameters.AddWithValue("@season", r.Season);
                cmd.Parameters.AddWithValue("@spring_league_id", (object?)r.SpringLeagueId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@spring_league_name", r.SpringLeagueName);
                cmd.Parameters.AddWithValue("@spring_league_abbreviation", r.SpringLeagueAbbreviation);
                cmd.Parameters.AddWithValue("@all_star_status", r.AllStarStatus);
                cmd.Parameters.AddWithValue("@venue_id", (object?)r.VenueId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@venue_name", r.VenueName);
                cmd.Parameters.AddWithValue("@team_code", r.TeamCode);
                cmd.Parameters.AddWithValue("@file_code", r.FileCode);
                cmd.Parameters.AddWithValue("@abbreviation", r.Abbreviation);
                cmd.Parameters.AddWithValue("@location_name", r.LocationName);
                cmd.Parameters.AddWithValue("@first_year_of_play", r.FirstYearOfPlay);
                cmd.Parameters.AddWithValue("@league_id", (object?)r.LeagueId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@league_name", r.LeagueName);
                cmd.Parameters.AddWithValue("@sport_id", (object?)r.SportId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sport_name", r.SportName);
                cmd.Parameters.AddWithValue("@short_name", r.ShortName);
                cmd.Parameters.AddWithValue("@record_games_played", (object?)r.RecordGamesPlayed ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@record_wild_card_games_back", r.RecordWildCardGamesBack);
                cmd.Parameters.AddWithValue("@record_league_games_back", r.RecordLeagueGamesBack);
                cmd.Parameters.AddWithValue("@record_spring_league_games_back", r.RecordSpringLeagueGamesBack);
                cmd.Parameters.AddWithValue("@record_sport_games_back", r.RecordSportGamesBack);
                cmd.Parameters.AddWithValue("@record_division_games_back", r.RecordDivisionGamesBack);
                cmd.Parameters.AddWithValue("@record_conference_games_back", r.RecordConferenceGamesBack);
                cmd.Parameters.AddWithValue("@record_league_record_wins", (object?)r.RecordLeagueRecordWins ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@record_league_record_losses", (object?)r.RecordLeagueRecordLosses ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@record_league_record_ties", (object?)r.RecordLeagueRecordTies ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@record_league_record_pct", (object?)r.RecordLeagueRecordPct ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@record_division_leader", (object?)r.RecordDivisionLeader ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@record_wins", (object?)r.RecordWins ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@record_losses", (object?)r.RecordLosses ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@record_winning_percentage", (object?)r.RecordWinningPercentage ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@franchise_name", r.FranchiseName);
                cmd.Parameters.AddWithValue("@club_name", r.ClubName);
                cmd.Parameters.AddWithValue("@active", r.Active);

                await cmd.ExecuteNonQueryAsync();
            }

            return Ok(new { message = "Team info inserted", count = records.Count });
        }


    }

}

