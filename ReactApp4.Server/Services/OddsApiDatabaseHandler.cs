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
    public class OddsApiDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public OddsApiDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ActionResult<IEnumerable<OddsApiH2H>>> GetOddsApiH2HBySeason(string season)
        {
            var tableName = $"odds_api_h2h_{season}";

            var query = $"SELECT * FROM {tableName} LIMIT 1";

            var oddsApiH2HBySeason = await _context.OddsApiH2Hs.FromSqlRaw(query).ToListAsync();

            return oddsApiH2HBySeason;
        }
        public async Task<IActionResult> BulkInsertOddsApiH2H([FromBody] List<OddsApiH2H> oddsLines, string sport, string season)
        {
            if (oddsLines == null || oddsLines.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var line in oddsLines)
                {
                    Console.WriteLine(line.GameId);
                    Console.WriteLine(line.MarketLastUpdate);
                    var table = $@"odds_api_h2h_{season}";
                    if (sport == "NBA")
                    {
                        table = $@"nba_odds_api_h2h_{season}";
                    }
                    var sql = $@"
                        INSERT INTO {table}
                            (game_id, sport_key, sport_title, commence_time, home_team, away_team,
                             bookmaker_key, bookmaker_title, bookmaker_last_update,
                             market_key, market_last_update, outcome_name, outcome_price)
                        VALUES
                            (@game_id, @sport_key, @sport_title, @commence_time, @home_team, @away_team,
                             @bookmaker_key, @bookmaker_title, @bookmaker_last_update,
                             @market_key, @market_last_update, @outcome_name, @outcome_price)
                        ON CONFLICT (game_id, bookmaker_key, market_key, market_last_update, outcome_name) DO NOTHING;";

                    using var cmd = new NpgsqlCommand(sql, connection);

                    cmd.Parameters.AddWithValue("@game_id", line.GameId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sport_key", line.SportKey ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sport_title", line.SportTitle ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@commence_time", line.CommenceTime);
                    cmd.Parameters.AddWithValue("@home_team", line.HomeTeam ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@away_team", line.AwayTeam ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@bookmaker_key", line.BookmakerKey ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@bookmaker_title", line.BookmakerTitle ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@bookmaker_last_update", line.BookmakerLastUpdate);
                    cmd.Parameters.AddWithValue("@market_key", line.MarketKey ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@market_last_update", line.MarketLastUpdate);
                    cmd.Parameters.AddWithValue("@outcome_name", line.OutcomeName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@outcome_price", line.OutcomePrice);

                    await cmd.ExecuteNonQueryAsync();
                }

                return Ok(new { message = "Inserted H2H odds lines", inserted = oddsLines.Count });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> InsertPlayerPropOdds([FromBody] List<OddsApiPlayerProp> odds, string season)
        {
            if (odds == null || odds.Count == 0)
                return BadRequest("No records provided.");

            try
            {
                var connectionString = _configuration.GetConnectionString("WebApiDatabase");

                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                foreach (var prop in odds)
                {
                    var sql = @$"
                        INSERT INTO mlb_player_prop_odds_{season} (
                            game_id, commence_time, home_team, away_team,
                            bookmaker, market, player, line, bet_type, decimal_odds
                        )
                        VALUES (
                            @GameId, @CommenceTime, @HomeTeam, @AwayTeam,
                            @Bookmaker, @Market, @Player, @Line, @BetType, @DecimalOdds
                        );";

                    using var cmd = new NpgsqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@GameId", prop.GameId);
                    cmd.Parameters.AddWithValue("@CommenceTime", prop.CommenceTime);
                    cmd.Parameters.AddWithValue("@HomeTeam", prop.HomeTeam);
                    cmd.Parameters.AddWithValue("@AwayTeam", prop.AwayTeam);
                    cmd.Parameters.AddWithValue("@Bookmaker", prop.Bookmaker);
                    cmd.Parameters.AddWithValue("@Market", prop.Market);
                    cmd.Parameters.AddWithValue("@Player", prop.Player);
                    cmd.Parameters.AddWithValue("@Line", prop.Line);
                    cmd.Parameters.AddWithValue("@BetType", prop.BetType);
                    cmd.Parameters.AddWithValue("@DecimalOdds", prop.DecimalOdds);

                    await cmd.ExecuteNonQueryAsync();
                }

                return Ok("Insert successful.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Insert failed: {ex.Message}");
            }
        }


    }
}