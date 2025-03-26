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
    public class MLBActivePlayerDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MLBActivePlayerDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ActionResult<IEnumerable<MLBActivePlayer>>> GetMLBActivePlayer(string season)
        {
            var tableName = $"mlb_active_players_{season}";

            var query = $"SELECT * FROM {tableName}";

            var mlbActivePlayer = await _context.MLBActivePlayers.FromSqlRaw(query).ToListAsync();

            return mlbActivePlayer;
        }

        public async Task<IActionResult> BulkInsertMLBActivePlayer([FromBody] List<MLBActivePlayer> players, string season)
        {
            if (players == null || players.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                var tableName = $"mlb_active_players_{season}";

                foreach (var p in players)
                {
                    var sql = $@"
                        INSERT INTO {tableName} (
                            player_id, full_name, first_name, last_name, primary_number, birth_date, current_age, birth_city, 
                            birth_state_province, birth_country, height, weight, active, mlb_debut_date, draft_year, 
                            team_id, team_name, team_link, primary_position_code, primary_position_name, position_type, 
                            bat_side_code, bat_side_description, pitch_hand_code, pitch_hand_description, boxscore_name, 
                            nick_name, strike_zone_top, strike_zone_bottom, name_slug
                        )
                        VALUES (
                            @player_id, @full_name, @first_name, @last_name, @primary_number, @birth_date, @current_age, @birth_city, 
                            @birth_state_province, @birth_country, @height, @weight, @active, @mlb_debut_date, @draft_year, 
                            @team_id, @team_name, @team_link, @primary_position_code, @primary_position_name, @position_type, 
                            @bat_side_code, @bat_side_description, @pitch_hand_code, @pitch_hand_description, @boxscore_name, 
                            @nick_name, @strike_zone_top, @strike_zone_bottom, @name_slug
                        )
                    ";

                    using var cmd = new NpgsqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@player_id", p.PlayerId);
                    cmd.Parameters.AddWithValue("@full_name", p.FullName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@first_name", p.FirstName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@last_name", p.LastName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@primary_number", p.PrimaryNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@birth_date", p.BirthDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@current_age", p.CurrentAge ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@birth_city", p.BirthCity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@birth_state_province", p.BirthStateProvince ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@birth_country", p.BirthCountry ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@height", p.Height ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@weight", p.Weight ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@active", p.Active);
                    cmd.Parameters.AddWithValue("@mlb_debut_date", p.MlbDebutDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@draft_year", p.DraftYear ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@team_id", p.TeamId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@team_name", p.TeamName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@team_link", p.TeamLink ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@primary_position_code", p.PrimaryPositionCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@primary_position_name", p.PrimaryPositionName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@position_type", p.PositionType ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@bat_side_code", p.BatSideCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@bat_side_description", p.BatSideDescription ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitch_hand_code", p.PitchHandCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@pitch_hand_description", p.PitchHandDescription ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@boxscore_name", p.BoxscoreName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nick_name", p.NickName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@strike_zone_top", p.StrikeZoneTop ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@strike_zone_bottom", p.StrikeZoneBottom ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@name_slug", p.NameSlug ?? (object)DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                }

                return Ok(new { message = "Active MLB players inserted", count = players.Count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }

}

