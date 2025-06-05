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

namespace ReactApp4.Server.Services
{
    public class ShotDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ShotDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        } 
        public async Task<ActionResult<IEnumerable<Shot>>> GetShotsBySeason(string playerId, string season)
        {
            var tableName = $"shots_{season}";

            var query = $@"SELECT * FROM {tableName}
                        WHERE player_id = '{playerId}'";

            var shotsBySeason = await _context.Shots.FromSqlRaw(query).ToListAsync();

            return shotsBySeason;
        }
        public async Task<ActionResult<IEnumerable<Shot>>> GetShotsByGame(string playerId, string season, string gameId)
        {
            var tableName = $"shots_{season}";

            var query = $@"SELECT * FROM {tableName}
                        WHERE player_id = '{playerId}'
                        AND game_id = '{gameId}'";

            var shotsByGame = await _context.Shots.FromSqlRaw(query).ToListAsync();

            return shotsByGame;
        }

        public async Task<IActionResult> CreateShot([FromBody] Shot shot, string season)
        {
            // Implement logic to create a new league game in the database


            try
            {
                if (shot == null)
                {
                    return BadRequest("Invalid shot data");
                }

                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var sql = $@"INSERT INTO shots_{season} (
                        grid_type,
                        game_id,
                        game_event_id,
                        player_id,
                        player_name,
                        team_id,
                        team_name,
                        period,
                        minutes_remaining,
                        seconds_remaining,
                        event_type,
                        action_type,
                        shot_type,
                        shot_zone_basic,
                        shot_zone_area,
                        shot_zone_range,
                        shot_distance,
                        loc_x,
                        loc_y,
                        shot_attempted_flag,
                        shot_made_flag,
                        game_date,
                        htm,
                        vtm
                    ) VALUES(
                        @grid_type,
                        @game_id,
                        @game_event_id,
                        @player_id,
                        @player_name,
                        @team_id,
                        @team_name,
                        @period,
                        @minutes_remaining,
                        @seconds_remaining,
                        @event_type,
                        @action_type,
                        @shot_type,
                        @shot_zone_basic,
                        @shot_zone_area,
                        @shot_zone_range,
                        @shot_distance,
                        @loc_x,
                        @loc_y,
                        @shot_attempted_flag,
                        @shot_made_flag,
                        @game_date,
                        @htm,
                        @vtm
                    )";

                    //var fgaValue = string.IsNullOrEmpty(boxScoreTraditional.Fga) ? null : boxScoreTraditional.Fga;
                    //boxScoreTraditional.CheckAndReplace();



                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@grid_type", shot.Grid_type ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@game_id", shot.Game_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@game_event_id", shot.Game_event_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_id", shot.Player_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@player_name", shot.Player_name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_id", shot.Team_id ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@team_name", shot.Team_name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@period", shot.Period ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@minutes_remaining", shot.Minutes_remaining ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@seconds_remaining", shot.Seconds_remaining ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@event_type", shot.Event_type ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@action_type", shot.Action_type ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@shot_type", shot.Shot_type ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@shot_zone_basic", shot.Shot_zone_basic ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@shot_zone_area", shot.Shot_zone_area ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@shot_zone_range", shot.Shot_zone_range ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@shot_distance", shot.Shot_distance ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@loc_x", shot.Loc_x ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@loc_y", shot.Loc_y ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@shot_attempted_flag", shot.Shot_attempted_flag ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@shot_made_flag", shot.Shot_made_flag ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@game_date", shot.Game_date ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@htm", shot.Htm ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@vtm", shot.Vtm ?? (object)DBNull.Value);



                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return StatusCode(201, shot);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex}");
            }
        }

    }
}
