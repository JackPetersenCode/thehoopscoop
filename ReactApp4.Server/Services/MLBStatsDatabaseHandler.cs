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
        public async Task<ActionResult<IEnumerable<MLBStatsBatting>>> GetMLBStatsBattingBySeason(
            string season,
            string? leagueOption,
            string? selectedTeam,
            string? yearToDateOption,
            int? personId)
        {
            var tableName = $"player_game_stats_batting_{season}";

            var conditions = new List<string>();

            if (!string.IsNullOrEmpty(leagueOption))
            {
                conditions.Add($"league = '{leagueOption}'");
            }
            if (!string.IsNullOrEmpty(selectedTeam))
            {
                conditions.Add($"team_name = '{selectedTeam}'");
            }
            if (personId.HasValue)
            {
                conditions.Add($"player_id = {personId.Value}");
            }

            var whereClause = conditions.Count > 0 ? $"WHERE {string.Join(" AND ", conditions)}" : string.Empty;

            var query = $@"
                SELECT team_name, player_id, person_id,
                    SUM(games_played) AS games_played,
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
                        ELSE SUM(stolen_bases) * 1.0 / (SUM(stolen_bases) + SUM(caught_stealing))
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
                        ELSE SUM(home_runs) * 1.0 / SUM(at_bats) 
                    END AS at_bats_per_home_run,
                    SUM(pop_outs) AS pop_outs,
                    SUM(line_outs) AS line_outs
                FROM {tableName}
                {whereClause}
                GROUP BY team_name, player_id, person_id";

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

