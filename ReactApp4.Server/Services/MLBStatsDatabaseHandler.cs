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
    public class MLBStatsDatabaseHandler : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public MLBStatsDatabaseHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public bool IsRecentGamesOption(string? yearToDateOption)
        {
            return 
                   yearToDateOption == "Last 7 Games" ||
                   yearToDateOption == "Last 15 Games" ||
                   yearToDateOption == "Last 30 Games";
        }

        public bool IsSpecificOpponent(string? selectedOpponent)
        {
            return selectedOpponent != "1" && selectedOpponent != "0";
        }


        public async Task<ActionResult<IEnumerable<MLBStatsBatting>>> GetMLBStatsBattingBySeason(
            string season,
            string? leagueOption,
            string? selectedTeam,
            string? yearToDateOption,
            string? selectedOpponent,
            int? personId)
        {
            var tableName = IsRecentGamesOption(yearToDateOption) ? 
                "last_n_games_per_player" : $"player_game_stats_batting_{season}";
            var finalTableName = IsSpecificOpponent(selectedOpponent) ? "player_with_opponent" : tableName;
            var activePlayersTable = $"mlb_active_players_{season}";
            var teamInfoTable = $"mlb_team_info_{season}";
            var mlbGameTable = $"mlb_games_{season}";
            var conditions = new List<string>();

            if (leagueOption == "National League" | leagueOption == "American League")
            {
                conditions.Add($"{teamInfoTable}.league_name = '{leagueOption}'");
            }
            if (selectedTeam != "1" && selectedTeam != "0")
            {
                conditions.Add($"{teamInfoTable}.team_id = '{selectedTeam}'");
            }
            if (personId.HasValue)
            {
                conditions.Add($"player_id = {personId.Value}");
            }
            if (IsRecentGamesOption(yearToDateOption)) {
                var rowNumber = yearToDateOption == "Last 7 Games" ? 7 : yearToDateOption == "Last 15 Games" ? 15 :
                    yearToDateOption == "Last 30 Games" ? 30 : 0;
                conditions.Add($"rn <= {rowNumber}");
            } 
            if (IsSpecificOpponent(selectedOpponent)) {
                conditions.Add($"opponent_team_id = '{selectedOpponent}'");
            }

            var whereClause = conditions.Count > 0 ? $" AND {string.Join(" AND ", conditions)}" : string.Empty;

            var query = $@"";

            var opponentCTETitle = $@" player_with_opponent AS ( ";
            var opponentCTE = $@"
                    SELECT 
                        pgsb.*,
                        mg.away_team_id,
                        mg.home_team_id,
                    CASE 
                        WHEN pgsb.team_side = 'home' THEN mg.away_team_id
                        WHEN pgsb.team_side = 'away' THEN mg.home_team_id
                    END AS opponent_team_id
                    FROM {tableName} pgsb
                    JOIN {mlbGameTable} mg
                        ON pgsb.game_pk = CAST(mg.game_pk AS INT)
                )               
                ";

            var rowNumberCTETitle = $@"WITH last_n_games_per_player AS ( ";

            var rowNumberCTE = $@"
                  SELECT 
                    player_game_stats_batting_{season}.game_pk,
                    player_game_stats_batting_{season}.team_side,
                    player_game_stats_batting_{season}.team_name,
                    player_game_stats_batting_{season}.player_id,
                    player_game_stats_batting_{season}.person_id,
                    player_game_stats_batting_{season}.games_played,
                    player_game_stats_batting_{season}.fly_outs,
                    player_game_stats_batting_{season}.ground_outs,
                    player_game_stats_batting_{season}.air_outs,
                    player_game_stats_batting_{season}.runs,
                    player_game_stats_batting_{season}.doubles,
                    player_game_stats_batting_{season}.triples,
                    player_game_stats_batting_{season}.home_runs,
                    player_game_stats_batting_{season}.strike_outs,
                    player_game_stats_batting_{season}.base_on_balls,
                    player_game_stats_batting_{season}.intentional_walks,
                    player_game_stats_batting_{season}.hits,
                    player_game_stats_batting_{season}.hit_by_pitch,
                    player_game_stats_batting_{season}.at_bats,
                    player_game_stats_batting_{season}.caught_stealing,
                    player_game_stats_batting_{season}.stolen_bases,
                    player_game_stats_batting_{season}.ground_into_double_play,
                    player_game_stats_batting_{season}.ground_into_triple_play,
                    player_game_stats_batting_{season}.plate_appearances,
                    player_game_stats_batting_{season}.total_bases,
                    player_game_stats_batting_{season}.rbi,
                    player_game_stats_batting_{season}.left_on_base,
                    player_game_stats_batting_{season}.sac_bunts,
                    player_game_stats_batting_{season}.sac_flies,
                    player_game_stats_batting_{season}.catchers_interference,
                    player_game_stats_batting_{season}.pickoffs,
                    player_game_stats_batting_{season}.pop_outs,
                    player_game_stats_batting_{season}.line_outs,
                    ROW_NUMBER() OVER (
                      PARTITION BY player_game_stats_batting_{season}.player_id
                      ORDER BY mlb_games_{season}.game_date DESC
                    ) AS rn
                  FROM player_game_stats_batting_{season}
                  JOIN mlb_games_{season}
                    ON player_game_stats_batting_{season}.game_pk = CAST(mlb_games_{season}.game_pk AS int)
                )

            ";
            if (IsRecentGamesOption(yearToDateOption)) {
                query += rowNumberCTETitle;
                query += rowNumberCTE;
                if (IsSpecificOpponent(selectedOpponent)) {
                    query += $", ";
                    query += opponentCTETitle;
                    query += opponentCTE;
                }
            } else {
                if (IsSpecificOpponent(selectedOpponent)) {
                    query += $@"WITH " + opponentCTETitle;
                    query += opponentCTE;
                }
            }



            query += $@"
                SELECT {finalTableName}.team_name, 
                    {finalTableName}.player_id, 
                    {finalTableName}.person_id,
                    {activePlayersTable}.full_name,
                    {activePlayersTable}.primary_position_name,
                    {teamInfoTable}.league_name,
                    SUM(games_played) AS games_played,
                    CASE 
                        WHEN SUM(at_bats) = 0 THEN NULL
                        ELSE SUM(hits) * 1.0 / SUM(at_bats)
                    END AS average,
                    SUM(fly_outs) AS fly_outs,
                    SUM(ground_outs) AS ground_outs,
                    SUM(air_outs) AS air_outs,
                    SUM(runs) AS runs,
                    SUM(doubles) AS doubles,
                    SUM(triples) AS triples,
                    SUM(home_runs) AS home_runs,
                    SUM(strike_outs) AS strike_outs,
                    SUM(base_on_balls) AS base_on_balls,
                    SUM(intentional_walks) AS intentional_walks,
                    SUM(hits) AS hits,
                    SUM(hit_by_pitch) AS hit_by_pitch,
                    SUM(at_bats) AS at_bats,
                    SUM(caught_stealing) AS caught_stealing,
                    SUM(stolen_bases) AS stolen_bases,
                    CASE 
                        WHEN SUM(stolen_bases) + SUM(caught_stealing) = 0 THEN NULL
                        ELSE 100 * SUM(stolen_bases) * 1.0 / (SUM(stolen_bases) + SUM(caught_stealing))
                    END AS stolen_base_percentage,
                    SUM(ground_into_double_play) AS ground_into_double_play,
                    SUM(ground_into_triple_play) AS ground_into_triple_play,
                    SUM(plate_appearances) AS plate_appearances,
                    SUM(total_bases) AS total_bases,
                    SUM(rbi) AS rbi,
                    SUM(left_on_base) AS left_on_base,
                    SUM(sac_bunts) AS sac_bunts,
                    SUM(sac_flies) AS sac_flies,
                    SUM(catchers_interference) AS catchers_interference,
                    SUM(pickoffs) AS pickoffs,
                    CASE 
                        WHEN SUM(at_bats) = 0 THEN NULL 
                        ELSE 100 * SUM(home_runs) * 1.0 / SUM(at_bats) 
                    END AS at_bats_per_home_run,
                    SUM(pop_outs) AS pop_outs,
                    SUM(line_outs) AS line_outs
                FROM {finalTableName}
                JOIN {activePlayersTable}
                    ON {finalTableName}.person_id = {activePlayersTable}.player_id
                JOIN {teamInfoTable}
                    ON {finalTableName}.game_pk = {teamInfoTable}.game_pk
                    AND {finalTableName}.team_name = {teamInfoTable}.team_name 
                WHERE games_played > 0
                {whereClause}
                GROUP BY {finalTableName}.team_name, {finalTableName}.player_id, {finalTableName}.person_id, 
                    {activePlayersTable}.full_name, {activePlayersTable}.primary_position_name,
                    {teamInfoTable}.league_name";
            Console.WriteLine(query);
            var result = await _context.MLBStatsBattings.FromSqlRaw(query).ToListAsync();
            return result;
        }


        //public async Task<ActionResult<IEnumerable<MLBPlayerGamePitching>>> GetMLBPlayerGamesPitchingBySeason(string season)
        //{
        //    var tableName = $"player_game_stats_pitching_{season}";
//
        //    var query = $"SELECT * FROM {tableName} LIMIT 1";
//
        //    var mlbPlayerGamesPitchingBySeason = await _context.MLBPlayerGamesPitching.FromSqlRaw(query).ToListAsync();
//
        //    return mlbPlayerGamesPitchingBySeason;
        //}
//
        //public async Task<ActionResult<IEnumerable<MLBPlayerGameFielding>>> GetMLBPlayerGamesFieldingBySeason(string season)
        //{
        //    var tableName = $"player_game_stats_fielding_{season}";
//
        //    var query = $"SELECT * FROM {tableName} LIMIT 1";
//
        //    var mlbPlayerGamesFieldingBySeason = await _context.MLBPlayerGamesFielding.FromSqlRaw(query).ToListAsync();
//
        //    return mlbPlayerGamesFieldingBySeason;
        //}

    }

}

