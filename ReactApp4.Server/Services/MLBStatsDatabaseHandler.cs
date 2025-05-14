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


        public async Task<ActionResult<IEnumerable<IMLBStatsBatting>>> GetMLBStatsBattingBySeason(
            string season,
            string? leagueOption,
            string? selectedTeam,
            string? yearToDateOption,
            string? selectedOpponent,
            string? selectedSplit,
            int? selectedPlayer,
            string? sortField,
            string? order)
        {
            var tableName = IsRecentGamesOption(yearToDateOption) ? 
                "last_n_games_per_player" : $"player_game_stats_batting_{season}";
            var finalTableName = IsSpecificOpponent(selectedOpponent) ? "player_with_opponent" : tableName;
            var activePlayersTable = $"mlb_active_players_{season}";
            var teamInfoTable = $"mlb_team_info_{season}";
            var mlbGameTable = $"mlb_games_{season}";
            var conditions = new List<string>();
            var playerRunsConditions = new List<string>();
            var leftOnBaseConditions = new List<string>();
            var finalSplitConditions = new List<string>();
            var lastNGamesConditions = new List<string>();
            var splitJoinStatement = $@"";
            var query = $@"";
            // if ((selectedTeam != "1" && selectedTeam != "0") || IsSpecificOpponent(selectedOpponent)) {
                // splitJoinStatement += $@"
                    // JOIN mlb_team_info_{season} ti ON p.game_pk = ti.game_pk
                    // JOIN mlb_games_{season} g ON p.game_pk = CAST(g.game_pk AS INT)
                // ";
            // }
            if (leagueOption == "National League" | leagueOption == "American League")
            {
                conditions.Add($"{teamInfoTable}.league_name = '{leagueOption}'");
                finalSplitConditions.Add($"ti.league_name = '{leagueOption}'");
                playerRunsConditions.Add($"team_info.league_name = '{leagueOption}'");
            }
            if (selectedTeam != "1" && selectedTeam != "0")
            {
                conditions.Add($@"{teamInfoTable}.team_id = {selectedTeam}");
                finalSplitConditions.Add($@"
                    ti.team_id = {selectedTeam}
                ");
                playerRunsConditions.Add($@"team_info.team_id = {selectedTeam}");
            }
            if (selectedPlayer.HasValue)
            {
                conditions.Add($"player_id = {selectedPlayer.Value}");
                playerRunsConditions.Add($"r.runners_details_player_id = {selectedPlayer.Value}");
                leftOnBaseConditions.Add($"p.matchup_batter_id = {selectedPlayer.Value}");
                finalSplitConditions.Add($"p.matchup_batter_id = {selectedPlayer.Value}");
            }
            if (IsRecentGamesOption(yearToDateOption)) {
                var rowNumber = yearToDateOption == "Last 7 Games" ? 7 : yearToDateOption == "Last 15 Games" ? 15 :
                    yearToDateOption == "Last 30 Games" ? 30 : 0;
                conditions.Add($"rn <= {rowNumber}");
                splitJoinStatement += $@"
                    JOIN last_n_games_per_player last
                        ON pbs.game_pk = last.game_pk
                        AND pbs.person_id = last.person_id
                ";
                finalSplitConditions.Add($@"
                    last.rn <= {rowNumber}
                ");
            } 
            if (IsSpecificOpponent(selectedOpponent)) {
                conditions.Add($"opponent_team_id = '{selectedOpponent}'");
                finalSplitConditions.Add($@"
                    (
                      CASE
                        WHEN ti.team_id = g.home_team_id THEN g.away_team_id
                        WHEN ti.team_id = g.away_team_id THEN g.home_team_id
                      END
                    ) = {selectedOpponent}
                ");
                playerRunsConditions.Add($@"
                    (
                      CASE
                        WHEN team_info.team_id = g.home_team_id THEN g.away_team_id
                        WHEN team_info.team_id = g.away_team_id THEN g.home_team_id
                      END
                    ) = {selectedOpponent}
                ");
                lastNGamesConditions.Add($@"
                    (               
                      CASE
                        WHEN player_game_stats_batting_{season}.team_side = 'home' THEN mlb_games_{season}.away_team_id
                        WHEN player_game_stats_batting_{season}.team_side = 'away' THEN mlb_games_{season}.home_team_id
                      END
                    ) = {selectedOpponent}        
                ");
            }
            var whereClause = conditions.Count > 0 ? $" AND {string.Join(" AND ", conditions)}" : string.Empty;
            var playerRunsWhereClause = playerRunsConditions.Count > 0 ? $" AND {string.Join(" AND ", playerRunsConditions)}" : string.Empty;
            var leftOnBaseWhereClause = leftOnBaseConditions.Count > 0 ? $" AND {string.Join(" AND ", leftOnBaseConditions)}" : string.Empty;
            var finalSplitWhereClause = finalSplitConditions.Count > 0 ? $" AND {string.Join(" AND ", finalSplitConditions)}" : string.Empty;
            var lastNGamesWhereClause = lastNGamesConditions.Count > 0 ? $" AND {string.Join(" AND ", lastNGamesConditions)}" : string.Empty;

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
                    player_game_stats_batting_{season}.person_id,
                    player_game_stats_batting_{season}.games_played,
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
                    player_game_stats_batting_{season}.fly_outs,
                    player_game_stats_batting_{season}.ground_outs,
                    ROW_NUMBER() OVER (
                      PARTITION BY player_game_stats_batting_{season}.person_id
                      ORDER BY mlb_games_{season}.game_date DESC
                    ) AS rn
                  FROM player_game_stats_batting_{season}
                  JOIN mlb_games_{season}
                    ON player_game_stats_batting_{season}.game_pk = CAST(mlb_games_{season}.game_pk AS int)
                  WHERE player_game_stats_batting_{season}.games_played > 0
                    {lastNGamesWhereClause}
                )
            ";

            if (selectedSplit == "vs. RHP" || selectedSplit == "vs. LHP")
            {
                var split = selectedSplit == "vs. RHP" ? "R" : "L";
                if (IsRecentGamesOption(yearToDateOption)) {
                    query += rowNumberCTETitle;
                    query += rowNumberCTE;
                    //if (IsSpecificOpponent(selectedOpponent)) {
                    //    query += $", ";
                    //    query += opponentCTETitle;
                    //    query += opponentCTE;
                    //}
                }
                query += $@"
                    SELECT 
                        p.matchup_batter_id AS person_id,
                        p.matchup_batter_full_name AS full_name,
                        pbs.team_name,
                        ti.league_name,
                        ap.primary_position_name,
                        COUNT(DISTINCT p.game_pk) AS games_played,
                        COUNT(CASE 
                            WHEN p.result_type = 'atBat' 
                            THEN 1.0 
                        END) AS plate_appearances,
                        COUNT(CASE 
                            WHEN p.result_type = 'atBat' 
                                AND NOT p.result_event_type ILIKE '%sac%'
                                AND NOT p.result_event_type ILIKE '%walk%'
                                AND NOT p.result_event_type ILIKE '%hit_by_pitch%'
                                AND NOT p.result_event_type ILIKE '%catcher_interf%'
                            THEN 1.0 
                        END) AS at_bats,
                        COUNT(CASE 
                            WHEN p.result_event IN ('Single', 'Double', 'Triple', 'Home Run') 
                            THEN 1.0 
                        END) AS hits,
                        CASE 
                            WHEN COUNT(CASE 
                                WHEN p.result_type = 'atBat' 
                                  AND NOT p.result_event_type ILIKE '%sac%'
                                  AND NOT p.result_event_type ILIKE '%walk%'
                                  AND NOT p.result_event_type ILIKE '%hit_by_pitch%'
                                  AND NOT p.result_event_type ILIKE '%catcher_interf%'
                                THEN 1.0 
                            END) = 0 
                            THEN NULL
                            ELSE 
                                COUNT(CASE 
                                    WHEN p.result_event IN ('Single', 'Double', 'Triple', 'Home Run') 
                                    THEN 1.0 
                                END)::FLOAT 
                                / 
                                COUNT(CASE 
                                    WHEN p.result_type = 'atBat' 
                                        AND NOT p.result_event_type ILIKE '%sac%'
                                        AND NOT p.result_event_type ILIKE '%walk%'
                                        AND NOT p.result_event_type ILIKE '%hit_by_pitch%'
                                        AND NOT p.result_event_type ILIKE '%catcher_interf%'
                                    THEN 1.0 
                                END)
                        END AS average,
                        
                        COUNT(CASE 
                               WHEN p.result_event = 'Double' 
                               THEN 1.0 
                            END) AS doubles,
                        COUNT(CASE 
                               WHEN p.result_event = 'Triples' 
                               THEN 1.0 
                            END) AS triples,
                        COUNT(CASE 
                               WHEN p.result_event = 'Home Run' 
                               THEN 1.0 
                            END) AS home_runs,
                        SUM(p.result_rbi) as rbi,
                        COUNT(CASE 
                               WHEN p.result_event = 'Walk' 
                               THEN 1.0 
                            END) AS base_on_balls,
                        COUNT(CASE 
                               WHEN p.result_event = 'Strikeout' 
                               THEN 1.0 
                            END) AS strike_outs,
                        COUNT(CASE 
                               WHEN p.result_event = 'Intent Walk' 
                               THEN 1.0 
                            END) AS intentional_walks,
                        COUNT(CASE 
                               WHEN p.result_event = 'Hit By Pitch' 
                               THEN 1.0 
                            END) AS hit_by_pitch,
                        COUNT(CASE 
                               WHEN p.result_event = 'Grounded Into DP' 
                               THEN 1.0 
                            END) AS ground_into_double_play,
                        COUNT(CASE 
                               WHEN p.result_event = 'Grounded Into TP' 
                               THEN 1.0 
                            END) AS ground_into_triple_play,
                        SUM(
                            CASE p.result_event_type
                                WHEN 'single' THEN 1
                                WHEN 'double' THEN 2
                                WHEN 'triple' THEN 3
                                WHEN 'home_run' THEN 4
                                ELSE 0
                            END
                        ) AS total_bases,
                        COUNT(*) FILTER (WHERE p.result_event_type = 'sac_fly') AS sac_flies,
                        COUNT(*) FILTER (WHERE p.result_event_type = 'sac_bunt') AS sac_bunts,
                        COUNT(*) FILTER (WHERE p.result_event_type = 'catcher_interf') AS catchers_interference,
                        COUNT(*) FILTER (WHERE p.result_event_type LIKE '%pickoff%') AS pickoffs,
                        COUNT(CASE 
                                WHEN p.result_type = 'atBat' 
                                     AND NOT p.result_event_type ILIKE '%sac%' 
                                     AND NOT p.result_event_type ILIKE '%walk%' 
                                     AND NOT p.result_event_type ILIKE '%hit_by_pitch%' 
                                     AND NOT p.result_event_type ILIKE '%catcher_interf%'
                                THEN 1 
                            END)::FLOAT
                            /
                        NULLIF(COUNT(CASE 
                                WHEN p.result_event = 'Home Run' 
                                THEN 1.0 
                            END), 0) as at_bats_per_home_run,
                        COUNT(CASE 
                              WHEN p.result_event = 'Pop Out' 
                              THEN 1.0 
                          END) as pop_outs,
                        COUNT(CASE 
                              WHEN p.result_event = 'Lineout' 
                              THEN 1.0 
                          END) as lineouts,
                        COUNT(CASE 
                              WHEN p.result_event LIKE '%Ground%' 
                              THEN 1.0 
                          END) as groundouts,
                        COUNT(CASE 
                              WHEN p.result_event = 'Flyout' 
                              THEN 1.0 
                          END) as flyouts,
                        -- OBP Calculation (guarding against div by 0)
                        CASE 
                            WHEN 
                              (COUNT(CASE 
                                 WHEN result_type = 'atBat'
                                   AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
                                 THEN 1 END)
                               + COUNT(CASE WHEN result_event_type = 'walk' THEN 1 END)
                               + COUNT(CASE WHEN result_event_type = 'hit_by_pitch' THEN 1 END)
                               + COUNT(CASE WHEN result_event_type = 'sac_fly' THEN 1 END)) = 0
                            THEN NULL
                            ELSE
                              (COUNT(CASE WHEN result_event IN ('Single', 'Double', 'Triple', 'Home Run') THEN 1 END)
                               + COUNT(CASE WHEN result_event_type = 'walk' THEN 1 END)
                               + COUNT(CASE WHEN result_event_type = 'hit_by_pitch' THEN 1 END))::FLOAT
                              /
                              (COUNT(CASE 
                                 WHEN result_type = 'atBat'
                                   AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
                                 THEN 1 END)
                               + COUNT(CASE WHEN result_event_type = 'walk' THEN 1 END)
                               + COUNT(CASE WHEN result_event_type = 'hit_by_pitch' THEN 1 END)
                               + COUNT(CASE WHEN result_event_type = 'sac_fly' THEN 1 END))
                        END AS obp,
                        -- SLG calculation
                        CASE 
                            WHEN COUNT(CASE 
                                         WHEN result_type = 'atBat' 
                                           AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
                                         THEN 1 
                                       END) = 0 
                            THEN NULL
                            ELSE (
                              1 * COUNT(CASE WHEN result_event = 'Single' THEN 1 END) +
                              2 * COUNT(CASE WHEN result_event = 'Double' THEN 1 END) +
                              3 * COUNT(CASE WHEN result_event = 'Triple' THEN 1 END) +
                              4 * COUNT(CASE WHEN result_event = 'Home Run' THEN 1 END)
                            )::FLOAT 
                            /
                            COUNT(CASE 
                                     WHEN result_type = 'atBat' 
                                       AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
                                     THEN 1 
                                 END)
                        END AS slg,
                        -- OPS = OBP + SLG
                        CASE 
                            WHEN 
                              COUNT(CASE WHEN result_type = 'atBat' THEN 1 
                                         WHEN result_event_type IN ('walk', 'hit_by_pitch', 'sac_fly') THEN 1 
                                   END) = 0 
                              OR 
                              COUNT(CASE 
                                       WHEN result_type = 'atBat' 
                                         AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
                                       THEN 1 
                                   END) = 0 
                            THEN NULL
                            ELSE (
                                (
                                  (
                                    COUNT(CASE WHEN result_event IN ('Single', 'Double', 'Triple', 'Home Run') THEN 1 END) +
                                    COUNT(CASE WHEN result_event_type = 'walk' THEN 1 END) +
                                    COUNT(CASE WHEN result_event_type = 'hit_by_pitch' THEN 1 END)
                                  )::FLOAT 
                                  /
                                  COUNT(CASE WHEN result_type = 'atBat' 
                                              THEN 1 
                                              WHEN result_event_type IN ('walk', 'hit_by_pitch', 'sac_fly') THEN 1 
                                        END)
                                )
                                +
                                (
                                  (
                                    1 * COUNT(CASE WHEN result_event = 'Single' THEN 1 END) +
                                    2 * COUNT(CASE WHEN result_event = 'Double' THEN 1 END) +
                                    3 * COUNT(CASE WHEN result_event = 'Triple' THEN 1 END) +
                                    4 * COUNT(CASE WHEN result_event = 'Home Run' THEN 1 END)
                                  )::FLOAT 
                                  /
                                  COUNT(CASE 
                                           WHEN result_type = 'atBat' 
                                             AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
                                           THEN 1 
                                       END)
                                )
                            )
                        END AS ops
                    FROM player_game_stats_batting_{season} pbs
                    LEFT JOIN mlb_plays_{season} p
                      ON p.matchup_batter_id = pbs.person_id
                      AND p.game_pk = pbs.game_pk
                    JOIN mlb_active_players_{season} ap
                    	ON pbs.person_id = ap.player_id
                    JOIN mlb_team_info_{season} ti
                    	ON pbs.game_pk = ti.game_pk
                      AND pbs.team_name = ti.team_name
				    LEFT JOIN mlb_games_{season} g
  				    ON pbs.game_pk = CAST(g.game_pk AS INT)
                    {splitJoinStatement}
                    WHERE p.matchup_pitch_hand_code = '{split}'
                    {finalSplitWhereClause}
                    GROUP BY p.matchup_batter_id, p.matchup_batter_full_name, pbs.team_name, ti.league_name, ap.primary_position_name
                ";
                
                var sortableFields = new HashSet<string> { "stolen_bases", "caught_stealing", "sb_percentage", "runs" };

                if (!sortableFields.Contains(sortField))
                {
                    query += $@"
                        ORDER BY {sortField} {order};
                    ";
                }

                    //WITH player_runs AS (
                    //    SELECT 
                    //      r.runners_details_player_id, 
                    //      COUNT(*) AS runs,
                    //      COUNT(*) FILTER (WHERE r.runners_details_event_type ILIKE 'stolen_base%' AND NOT r.runners_movement_is_out) AS stolen_bases,
                    //      COUNT(*) FILTER (WHERE r.runners_details_event_type ILIKE 'stolen_base%' AND r.runners_movement_is_out) AS caught_stealing,
                    //      ROUND(
                    //          COUNT(*) FILTER (WHERE r.runners_details_event_type ILIKE 'stolen_base%' AND NOT r.runners_movement_is_out) * 100.0
                    //          /
                    //          NULLIF(COUNT(*) FILTER (WHERE r.runners_details_event_type ILIKE 'stolen_base%'), 0),
                    //          1
                    //      ) AS sb_percentage
                    //    FROM mlb_runners_{season} r
                    //      JOIN mlb_plays_2023 p ON r.game_pk = p.game_pk AND r.at_bat_index = p.at_bat_index
                    //      JOIN mlb_team_info_2023 team_info ON p.game_pk = team_info.game_pk
                    //      JOIN mlb_games_2023 g ON p.game_pk = CAST(g.game_pk AS INT)
                    //      {splitJoinStatement}
                    //    WHERE r.runners_movement_end = 'score'
                    //      {playerRunsWhereClause}
                    //      AND p.matchup_pitch_hand_code = '{split}'
                    //    GROUP BY r.runners_details_player_id
                    //),
                    //left_on_base_rows AS (
                    //    SELECT 
                    //        p.matchup_batter_id,
                    //        p.matchup_batter_full_name,
                    //        COUNT(*) FILTER (
                    //            WHERE r.runners_details_player_id IS NOT NULL 
                    //                AND r.runners_movement_end NOT IN ('home', 'score')  -- didn't score
                    //                AND r.runners_movement_is_out = FALSE                -- wasn't out
                    //        ) AS left_on_base
                    //    FROM mlb_plays_{season} p
                    //    JOIN mlb_runners_{season} r 
                    //        ON p.game_pk = r.game_pk 
                    //        AND p.at_bat_index = r.at_bat_index
                    //    WHERE 1 = 1
                    //        {leftOnBaseWhereClause}
                    //        AND p.matchup_pitch_hand_code = '{split}'  -- vs. LHP
                    //        AND p.result_type = 'atBat'
                    //    GROUP BY p.matchup_batter_id, p.matchup_batter_full_name
                    //)
                Console.WriteLine(query);
                var result = await _context.MLBStatsBattingWithSplits.FromSqlRaw(query).ToListAsync();
                return result;
            
            } else {
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
                    SELECT 
                        {finalTableName}.person_id,
                        {activePlayersTable}.full_name,
                        {finalTableName}.team_name,
                        {teamInfoTable}.league_name,
                        {activePlayersTable}.primary_position_name,
                        SUM(games_played) AS games_played,
                        SUM(plate_appearances) AS plate_appearances,
                        SUM(at_bats) AS at_bats,
                        SUM(hits) AS hits,

                        CASE 
                            WHEN SUM(at_bats) = 0 THEN NULL
                            ELSE SUM(hits) * 1.0 / SUM(at_bats)
                        END AS average,
                        SUM(runs) AS runs,
                        SUM(doubles) AS doubles,
                        SUM(triples) AS triples,
                        SUM(home_runs) AS home_runs,
                        SUM(rbi) AS rbi,
                        SUM(caught_stealing) AS caught_stealing,
                        SUM(stolen_bases) AS stolen_bases,
                        CASE 
                            WHEN SUM(stolen_bases) + SUM(caught_stealing) = 0 THEN NULL
                            ELSE 100 * SUM(stolen_bases) * 1.0 / (SUM(stolen_bases) + SUM(caught_stealing))
                        END AS stolen_base_percentage,
                        SUM(base_on_balls) AS base_on_balls,
                        SUM(strike_outs) AS strike_outs,
                        SUM(intentional_walks) AS intentional_walks,
                        SUM(hit_by_pitch) AS hit_by_pitch,
                        SUM(ground_into_double_play) AS ground_into_double_play,
                        SUM(ground_into_triple_play) AS ground_into_triple_play,
                        SUM(total_bases) AS total_bases,
                        SUM(left_on_base) AS left_on_base,
                        SUM(sac_bunts) AS sac_bunts,
                        SUM(sac_flies) AS sac_flies,
                        SUM(catchers_interference) AS catchers_interference,
                        SUM(pickoffs) AS pickoffs,
                        CASE 
                            WHEN SUM(home_runs) = 0 THEN NULL 
                            ELSE SUM(at_bats) * 1.0 / SUM(home_runs) 
                        END AS at_bats_per_home_run,
                        SUM(fly_outs) AS flyouts,
                        SUM(ground_outs) AS groundouts,
                        SUM(pop_outs) AS pop_outs,
                        SUM(line_outs) AS lineouts,
                        -- OBP (On-Base Percentage)
                        CASE 
                          WHEN 
                            (SUM(at_bats) + SUM(base_on_balls) + SUM(hit_by_pitch) + SUM(sac_flies)) = 0 
                          THEN NULL
                          ELSE 
                            (SUM(hits) + SUM(base_on_balls) + SUM(hit_by_pitch))::FLOAT
                            / (SUM(at_bats) + SUM(base_on_balls) + SUM(hit_by_pitch) + SUM(sac_flies))
                        END AS obp,
                        
                        -- SLG (Slugging Percentage)
                        CASE 
                          WHEN SUM(at_bats) = 0 
                          THEN NULL
                          ELSE SUM(total_bases)::FLOAT / SUM(at_bats)
                        END AS slg,
                        
                        -- OPS (On-base + Slugging)
                        CASE 
                          WHEN 
                            (SUM(at_bats) + SUM(base_on_balls) + SUM(hit_by_pitch) + SUM(sac_flies)) = 0 
                            OR SUM(at_bats) = 0
                          THEN NULL
                          ELSE (
                            (SUM(hits) + SUM(base_on_balls) + SUM(hit_by_pitch))::FLOAT
                            / (SUM(at_bats) + SUM(base_on_balls) + SUM(hit_by_pitch) + SUM(sac_flies))
                          ) + (
                            SUM(total_bases)::FLOAT / SUM(at_bats)
                          )
                        END AS ops

                    FROM {finalTableName}
                    JOIN {activePlayersTable}
                        ON {finalTableName}.person_id = {activePlayersTable}.player_id
                    JOIN {teamInfoTable}
                        ON {finalTableName}.game_pk = {teamInfoTable}.game_pk
                        AND {finalTableName}.team_name = {teamInfoTable}.team_name 
                    WHERE games_played > 0
                    {whereClause}
                    GROUP BY {finalTableName}.team_name, {finalTableName}.person_id, 
                        {activePlayersTable}.full_name, {activePlayersTable}.primary_position_name,
                        {teamInfoTable}.league_name
                    ORDER BY {sortField} {order}";

                Console.WriteLine(query);
                var result = await _context.MLBStatsBattings.FromSqlRaw(query).ToListAsync();
                return result;            
            }
        }


        public async Task<ActionResult<IEnumerable<IMLBStatsPitching>>> GetMLBStatsPitchingBySeason(
            string season,
            string? leagueOption,
            string? selectedTeam,
            string? yearToDateOption,
            string? selectedOpponent,
            string? selectedSplit,
            int? selectedPlayer,
            string? sortField,
            string? order)
        {
            var tableName = IsRecentGamesOption(yearToDateOption) ? 
                "last_n_games_per_player" : $"player_game_stats_pitching_{season}";
            var finalTableName = IsSpecificOpponent(selectedOpponent) ? "player_with_opponent" : tableName;
            var activePlayersTable = $"mlb_active_players_{season}";
            var teamInfoTable = $"mlb_team_info_{season}";
            var mlbGameTable = $"mlb_games_{season}";
            var conditions = new List<string>();
            var playerRunsConditions = new List<string>();
            var leftOnBaseConditions = new List<string>();
            var finalSplitConditions = new List<string>();
            var lastNGamesConditions = new List<string>();
            var splitJoinStatement = $@"";

            var split = selectedSplit == "vs. RHB" ? "R" : "L";

            var query = $@"";
            // if ((selectedTeam != "1" && selectedTeam != "0") || IsSpecificOpponent(selectedOpponent)) {
                // splitJoinStatement += $@"
                    // JOIN mlb_team_info_{season} ti ON p.game_pk = ti.game_pk
                    // JOIN mlb_games_{season} g ON p.game_pk = CAST(g.game_pk AS INT)
                // ";
            // }
            if (leagueOption == "National League" | leagueOption == "American League")
            {
                conditions.Add($"{teamInfoTable}.league_name = '{leagueOption}'");
                finalSplitConditions.Add($"ti.league_name = '{leagueOption}'");
                playerRunsConditions.Add($"team_info.league_name = '{leagueOption}'");
            }
            if (selectedTeam != "1" && selectedTeam != "0")
            {
                Console.WriteLine("selected Team");
                Console.WriteLine(selectedTeam);
                conditions.Add($@"{teamInfoTable}.team_id = {selectedTeam}");
                finalSplitConditions.Add($@"
                    ti.team_id = {selectedTeam}
                ");
                playerRunsConditions.Add($@"team_info.team_id = {selectedTeam}");
            }
            if (selectedPlayer.HasValue)
            {
                conditions.Add($"player_id = {selectedPlayer.Value}");
                playerRunsConditions.Add($"r.runners_details_player_id = {selectedPlayer.Value}");
                leftOnBaseConditions.Add($"p.matchup_pitcher_id = {selectedPlayer.Value}");
                finalSplitConditions.Add($"p.matchup_pitcher_id = {selectedPlayer.Value}");
            }
            if (IsRecentGamesOption(yearToDateOption)) {
                Console.WriteLine(yearToDateOption);
                var rowNumber = yearToDateOption == "Last 7 Games" ? 7 : yearToDateOption == "Last 15 Games" ? 15 :
                    yearToDateOption == "Last 30 Games" ? 30 : 0;
                conditions.Add($"rn <= {rowNumber}");
                splitJoinStatement += $@"
                    JOIN last_n_games_per_player last
                        ON pbs.game_pk = last.game_pk
                        AND pbs.person_id = last.person_id
                ";
                finalSplitConditions.Add($@"
                    last.rn <= {rowNumber}
                ");
            } 
            if (IsSpecificOpponent(selectedOpponent)) {
                Console.WriteLine("is specific opponent");
                Console.WriteLine(selectedOpponent);
                conditions.Add($"opponent_team_id = '{selectedOpponent}'");
                finalSplitConditions.Add($@"
                    (
                      CASE
                        WHEN ti.team_id = g.home_team_id THEN g.away_team_id
                        WHEN ti.team_id = g.away_team_id THEN g.home_team_id
                      END
                    ) = {selectedOpponent}
                ");
                playerRunsConditions.Add($@"
                    (
                      CASE
                        WHEN team_info.team_id = g.home_team_id THEN g.away_team_id
                        WHEN team_info.team_id = g.away_team_id THEN g.home_team_id
                      END
                    ) = {selectedOpponent}
                ");
                lastNGamesConditions.Add($@"
                    (               
                      CASE
                        WHEN player_game_stats_pitching_{season}.team_side = 'home' THEN mlb_games_{season}.away_team_id
                        WHEN player_game_stats_pitching_{season}.team_side = 'away' THEN mlb_games_{season}.home_team_id
                      END
                    ) = {selectedOpponent}        
                ");
            }
            var whereClause = conditions.Count > 0 ? $" AND {string.Join(" AND ", conditions)}" : string.Empty;
            var playerRunsWhereClause = playerRunsConditions.Count > 0 ? $" AND {string.Join(" AND ", playerRunsConditions)}" : string.Empty;
            var leftOnBaseWhereClause = leftOnBaseConditions.Count > 0 ? $" AND {string.Join(" AND ", leftOnBaseConditions)}" : string.Empty;
            var finalSplitWhereClause = finalSplitConditions.Count > 0 ? $" AND {string.Join(" AND ", finalSplitConditions)}" : string.Empty;
            var lastNGamesWhereClause = lastNGamesConditions.Count > 0 ? $" AND {string.Join(" AND ", lastNGamesConditions)}" : string.Empty;

            var opponentCTE = $@" player_with_opponent AS (
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
            )";

            var inningsPitchedCTE = $@"
                innings_pitched AS (
                    SELECT 
                        p.matchup_pitcher_id,
                        p.matchup_pitcher_full_name,
                        pbs.team_name,
                        FLOOR(SUM(CASE WHEN r.runners_movement_is_out THEN 1 ELSE 0 END) / 3.0)::int +
                        CASE 
                          WHEN MOD(SUM(CASE WHEN r.runners_movement_is_out THEN 1 ELSE 0 END), 3) = 1 THEN 0.1
                          WHEN MOD(SUM(CASE WHEN r.runners_movement_is_out THEN 1 ELSE 0 END), 3) = 2 THEN 0.2
                          ELSE 0.0 
                        END AS innings_pitched
                    FROM mlb_runners_{season} r
                    JOIN mlb_plays_{season} p
                        ON r.game_pk = p.game_pk
                        AND r.at_bat_index = p.at_bat_index
                    JOIN player_game_stats_pitching_{season} pbs
                        ON pbs.person_id = p.matchup_pitcher_id
                        AND pbs.game_pk = p.game_pk
                    WHERE p.matchup_bat_side_code = '{split}'
                    GROUP BY p.matchup_pitcher_id, p.matchup_pitcher_full_name, pbs.team_name

            )";

            //var ballsCTE = $@"
            //balls AS (
            //    SELECT 
            //        p.matchup_pitcher_id AS person_id,
            //        pgsp.team_name,
            //        COUNT(*) AS balls
            //    FROM mlb_play_events_{season} e
            //    JOIN mlb_plays_{season} p
            //        ON e.game_pk = p.game_pk AND e.at_bat_index = p.at_bat_index
            //    JOIN player_game_stats_pitching_{season} pgsp
            //        ON pgsp.game_pk = p.game_pk AND pgsp.person_id = p.matchup_pitcher_id
            //    WHERE 
            //        e.play_events_is_pitch = true
            //        AND e.play_events_details_is_ball = true
            //        AND p.matchup_bat_side_code = '{split}'
            //    GROUP BY 
            //        p.matchup_pitcher_id, pgsp.team_name
            //)";
//
            //var strikesCTE = $@"
            //strikes AS (
            //    SELECT 
            //        p.matchup_pitcher_id AS person_id,
            //        pgsp.team_name,
            //        COUNT(*) AS strikes
            //    FROM mlb_play_events_{season} e
            //    JOIN mlb_plays_{season} p
            //        ON e.game_pk = p.game_pk AND e.at_bat_index = p.at_bat_index
            //    JOIN player_game_stats_pitching_{season} pgsp
            //        ON pgsp.game_pk = p.game_pk AND pgsp.person_id = p.matchup_pitcher_id
            //    WHERE 
            //        e.play_events_is_pitch = true
            //        AND e.play_events_details_is_strike = true
            //        AND p.matchup_bat_side_code = '{split}'
            //    GROUP BY 
            //        p.matchup_pitcher_id, pgsp.team_name
            //)";

            var rowNumberCTE = $@" last_n_games_per_player AS (
                SELECT 
                  player_game_stats_pitching_{season}.game_pk,
                  player_game_stats_pitching_{season}.team_side,
                  player_game_stats_pitching_{season}.team_name,
                  player_game_stats_pitching_{season}.person_id,
                  player_game_stats_pitching_{season}.note,
                  player_game_stats_pitching_{season}.games_played,
                  player_game_stats_pitching_{season}.games_started,
                  player_game_stats_pitching_{season}.fly_outs,
                  player_game_stats_pitching_{season}.ground_outs,
                  player_game_stats_pitching_{season}.air_outs,
                  player_game_stats_pitching_{season}.runs,
                  player_game_stats_pitching_{season}.doubles,
                  player_game_stats_pitching_{season}.triples,
                  player_game_stats_pitching_{season}.home_runs,
                  player_game_stats_pitching_{season}.strike_outs,
                  player_game_stats_pitching_{season}.base_on_balls,
                  player_game_stats_pitching_{season}.intentional_walks,
                  player_game_stats_pitching_{season}.hits,
                  player_game_stats_pitching_{season}.hit_by_pitch,
                  player_game_stats_pitching_{season}.at_bats,
                  player_game_stats_pitching_{season}.caught_stealing,
                  player_game_stats_pitching_{season}.stolen_bases,
                  player_game_stats_pitching_{season}.number_of_pitches,
                  player_game_stats_pitching_{season}.innings_pitched,
                  player_game_stats_pitching_{season}.wins,
                  player_game_stats_pitching_{season}.losses,
                  player_game_stats_pitching_{season}.saves,
                  player_game_stats_pitching_{season}.save_opportunities,
                  player_game_stats_pitching_{season}.holds,
                  player_game_stats_pitching_{season}.blown_saves,
                  player_game_stats_pitching_{season}.earned_runs,
                  player_game_stats_pitching_{season}.batters_faced,
                  player_game_stats_pitching_{season}.outs,
                  player_game_stats_pitching_{season}.games_pitched,
                  player_game_stats_pitching_{season}.complete_games,
                  player_game_stats_pitching_{season}.shutouts,
                  player_game_stats_pitching_{season}.pitches_thrown,
                  player_game_stats_pitching_{season}.balls,
                  player_game_stats_pitching_{season}.strikes,
                  player_game_stats_pitching_{season}.strike_percentage,
                  player_game_stats_pitching_{season}.hit_batsmen,
                  player_game_stats_pitching_{season}.balks,
                  player_game_stats_pitching_{season}.wild_pitches,
                  player_game_stats_pitching_{season}.pickoffs,
                  player_game_stats_pitching_{season}.rbi,
                  player_game_stats_pitching_{season}.games_finished,
                  player_game_stats_pitching_{season}.runs_scored_per9,
                  player_game_stats_pitching_{season}.home_runs_per9,
                  player_game_stats_pitching_{season}.inheritied_runners,
                  player_game_stats_pitching_{season}.inherited_runners_scored,
                  player_game_stats_pitching_{season}.catchers_interference,
                  player_game_stats_pitching_{season}.sac_bunts,
                  player_game_stats_pitching_{season}.sac_flies,
                  player_game_stats_pitching_{season}.passed_balls,
                  player_game_stats_pitching_{season}.pop_outs,
                  player_game_stats_pitching_{season}.line_outs,
                  ROW_NUMBER() OVER (
                    PARTITION BY player_game_stats_pitching_{season}.person_id
                    ORDER BY mlb_games_{season}.game_date DESC
                  ) AS rn
                FROM player_game_stats_pitching_{season}
                JOIN mlb_games_{season}
                  ON player_game_stats_pitching_{season}.game_pk = CAST(mlb_games_{season}.game_pk AS int)
                WHERE player_game_stats_pitching_{season}.games_played > 0
                  {lastNGamesWhereClause}
            )";

            if (selectedSplit == "vs. RHB" || selectedSplit == "vs. LHB")
            {
                query += $@"WITH ";
                if (IsRecentGamesOption(yearToDateOption)) {
                    query += rowNumberCTE;
                    query += $@",";
                    //if (IsSpecificOpponent(selectedOpponent)) {
                    //    query += $", ";
                    //    query += opponentCTETitle;
                    //    query += opponentCTE;
                    //}
                }

                query += inningsPitchedCTE;

                query += $@"
                    SELECT 
                        p.matchup_pitcher_id AS person_id,
                        p.matchup_pitcher_full_name AS full_name,
                        pbs.team_name,
                        ti.league_name,
                        ap.primary_position_name,
                        COUNT(DISTINCT p.game_pk) AS games_played,
                        innings_pitched.innings_pitched,
                        COUNT(CASE 
                            WHEN p.result_event IN ('Single', 'Double', 'Triple', 'Home Run') 
                            THEN 1.0 
                        END) AS hits,
                        CASE 
                            WHEN COUNT(CASE 
                                WHEN p.result_type = 'atBat' 
                                  AND NOT p.result_event_type ILIKE '%sac%'
                                  AND NOT p.result_event_type ILIKE '%walk%'
                                  AND NOT p.result_event_type ILIKE '%hit_by_pitch%'
                                  AND NOT p.result_event_type ILIKE '%catcher_interf%'
                                THEN 1.0 
                            END) = 0 
                            THEN NULL
                            ELSE 
                                COUNT(CASE 
                                    WHEN p.result_event IN ('Single', 'Double', 'Triple', 'Home Run') 
                                    THEN 1.0 
                                END)::FLOAT 
                                / 
                                COUNT(CASE 
                                    WHEN p.result_type = 'atBat' 
                                        AND NOT p.result_event_type ILIKE '%sac%'
                                        AND NOT p.result_event_type ILIKE '%walk%'
                                        AND NOT p.result_event_type ILIKE '%hit_by_pitch%'
                                        AND NOT p.result_event_type ILIKE '%catcher_interf%'
                                    THEN 1.0 
                                END)
                        END AS average,
                        
                        COUNT(CASE 
                               WHEN p.result_event = 'Double' 
                               THEN 1.0 
                            END) AS doubles,
                        COUNT(CASE 
                               WHEN p.result_event = 'Triples' 
                               THEN 1.0 
                            END) AS triples,
                        COUNT(CASE 
                               WHEN p.result_event = 'Home Run' 
                               THEN 1.0 
                            END) AS home_runs,
                        COUNT(CASE 
                               WHEN p.result_event = 'Walk' 
                               THEN 1.0 
                            END) AS base_on_balls,
                        COUNT(CASE 
                               WHEN p.result_event = 'Strikeout' 
                               THEN 1.0 
                            END) AS strike_outs,
                        COUNT(CASE 
                               WHEN p.result_event = 'Intent Walk' 
                               THEN 1.0 
                            END) AS intentional_walks,
                        COUNT(CASE 
                               WHEN p.result_event = 'Hit By Pitch' 
                               THEN 1.0 
                            END) AS hit_batsmen,
                        COUNT(CASE 
                               WHEN p.result_event = 'Grounded Into DP' 
                               THEN 1.0 
                            END) AS ground_into_double_play,
                        COUNT(CASE 
                               WHEN p.result_event = 'Grounded Into TP' 
                               THEN 1.0 
                            END) AS ground_into_triple_play,
                        SUM(
                            CASE p.result_event_type
                                WHEN 'single' THEN 1
                                WHEN 'double' THEN 2
                                WHEN 'triple' THEN 3
                                WHEN 'home_run' THEN 4
                                ELSE 0
                            END
                        ) AS total_bases,
                        COUNT(*) FILTER (WHERE p.result_event_type = 'sac_fly') AS sac_flies,
                        COUNT(*) FILTER (WHERE p.result_event_type = 'sac_bunt') AS sac_bunts,
                        COUNT(*) FILTER (WHERE p.result_event_type = 'catcher_interf') AS catchers_interference,
                        COUNT(*) FILTER (WHERE p.result_event_type LIKE '%pickoff%') AS pickoffs,

                        COUNT(CASE 
                              WHEN p.result_event = 'Pop Out' 
                              THEN 1.0 
                          END) as pop_outs,
                        COUNT(CASE 
                              WHEN p.result_event = 'Lineout' 
                              THEN 1.0 
                          END) as line_outs,
                        COUNT(CASE 
                              WHEN p.result_event LIKE '%Ground%' 
                              THEN 1.0 
                          END) as ground_outs,
                        COUNT(CASE 
                              WHEN p.result_event = 'Flyout' 
                              THEN 1.0 
                          END) as fly_outs,
                        COUNT(p.*) AS batters_faced,
                        SUM(CASE 
                                WHEN pitch_index IS NOT NULL THEN jsonb_array_length(pitch_index::jsonb)
                                ELSE 0
                            END) AS pitches_thrown,
                        CASE 
                            WHEN CAST(innings_pitched.innings_pitched AS FLOAT) = 0 THEN NULL
                            ELSE 
                                SUM(CASE 
                                        WHEN p.result_type = 'atBat' AND pitch_index IS NOT NULL 
                                        THEN jsonb_array_length(pitch_index::jsonb)
                                        ELSE 0
                                    END) * 1.0 
                                / 
                                CAST(innings_pitched.innings_pitched AS FLOAT)
                        END AS pitches_per_inning,
                        CASE 
                            WHEN CAST(innings_pitched.innings_pitched AS FLOAT) = 0 THEN NULL
                            ELSE 
                                COUNT(*) FILTER (WHERE result_event = 'Strikeout') * 9.0 / CAST(innings_pitched.innings_pitched AS FLOAT)
                        END AS strikeouts_per9,
                        CASE 
                            WHEN CAST(innings_pitched.innings_pitched AS FLOAT) = 0 THEN NULL
                            ELSE 
                                COUNT(*) FILTER (WHERE result_event = 'Home Run') * 9.0 / CAST(innings_pitched.innings_pitched AS FLOAT)
                        END AS home_runs_per9,
                        CASE 
                            WHEN CAST(innings_pitched.innings_pitched AS FLOAT) = 0 THEN NULL
                            ELSE 
                                COUNT(*) FILTER (WHERE result_event IN ('Walk', 'Intent Walk')) * 9.0 / CAST(innings_pitched.innings_pitched AS FLOAT)
                        END AS walks_per9,
                        CASE 
                          WHEN innings_pitched.innings_pitched = 0 THEN NULL
                          ELSE
                            COUNT(CASE WHEN p.result_event IN ('Single', 'Double', 'Triple', 'Home Run', 'Walk', 'Intent Walk') THEN 1.0 END)
                            / 
                            CAST(innings_pitched.innings_pitched AS FLOAT)
                        END AS whip

                    FROM player_game_stats_pitching_{season} pbs
                    LEFT JOIN mlb_plays_{season} p
                        ON p.matchup_pitcher_id = pbs.person_id
                        AND p.game_pk = pbs.game_pk
                    JOIN innings_pitched
                        ON pbs.person_id = innings_pitched.matchup_pitcher_id
                        AND pbs.team_name = innings_pitched.team_name
                    JOIN mlb_active_players_{season} ap
                    	ON pbs.person_id = ap.player_id
                    JOIN mlb_team_info_{season} ti
                    	ON pbs.game_pk = ti.game_pk
                        AND pbs.team_name = ti.team_name
				    LEFT JOIN mlb_games_{season} g
  				        ON pbs.game_pk = CAST(g.game_pk AS INT)
                    {splitJoinStatement}
                    WHERE p.matchup_bat_side_code = '{split}'
                    {finalSplitWhereClause}
                    GROUP BY p.matchup_pitcher_id, p.matchup_pitcher_full_name, pbs.team_name, ti.league_name, ap.primary_position_name,
                    innings_pitched.innings_pitched
                ";
                
                    //JOIN strikes
                    //    ON pbs.person_id = strikes.person_id
                    //    AND pbs.team_name = strikes.team_name
                    //JOIN balls
                    //    ON pbs.person_id = balls.person_id
                    //    AND pbs.team_name = balls.team_name

                var sortableFields = new HashSet<string> { "stolen_bases", "caught_stealing", "sb_percentage", "runs" };

                if (!sortableFields.Contains(sortField))
                {
                    query += $@"
                        ORDER BY {sortField} {order};
                    ";
                }

                    //WITH player_runs AS (
                    //    SELECT 
                    //      r.runners_details_player_id, 
                    //      COUNT(*) AS runs,
                    //      COUNT(*) FILTER (WHERE r.runners_details_event_type ILIKE 'stolen_base%' AND NOT r.runners_movement_is_out) AS stolen_bases,
                    //      COUNT(*) FILTER (WHERE r.runners_details_event_type ILIKE 'stolen_base%' AND r.runners_movement_is_out) AS caught_stealing,
                    //      ROUND(
                    //          COUNT(*) FILTER (WHERE r.runners_details_event_type ILIKE 'stolen_base%' AND NOT r.runners_movement_is_out) * 100.0
                    //          /
                    //          NULLIF(COUNT(*) FILTER (WHERE r.runners_details_event_type ILIKE 'stolen_base%'), 0),
                    //          1
                    //      ) AS sb_percentage
                    //    FROM mlb_runners_{season} r
                    //      JOIN mlb_plays_2023 p ON r.game_pk = p.game_pk AND r.at_bat_index = p.at_bat_index
                    //      JOIN mlb_team_info_2023 team_info ON p.game_pk = team_info.game_pk
                    //      JOIN mlb_games_2023 g ON p.game_pk = CAST(g.game_pk AS INT)
                    //      {splitJoinStatement}
                    //    WHERE r.runners_movement_end = 'score'
                    //      {playerRunsWhereClause}
                    //      AND p.matchup_pitch_hand_code = '{split}'
                    //    GROUP BY r.runners_details_player_id
                    //),
                    //left_on_base_rows AS (
                    //    SELECT 
                    //        p.matchup_batter_id,
                    //        p.matchup_batter_full_name,
                    //        COUNT(*) FILTER (
                    //            WHERE r.runners_details_player_id IS NOT NULL 
                    //                AND r.runners_movement_end NOT IN ('home', 'score')  -- didn't score
                    //                AND r.runners_movement_is_out = FALSE                -- wasn't out
                    //        ) AS left_on_base
                    //    FROM mlb_plays_{season} p
                    //    JOIN mlb_runners_{season} r 
                    //        ON p.game_pk = r.game_pk 
                    //        AND p.at_bat_index = r.at_bat_index
                    //    WHERE 1 = 1
                    //        {leftOnBaseWhereClause}
                    //        AND p.matchup_pitch_hand_code = '{split}'  -- vs. LHP
                    //        AND p.result_type = 'atBat'
                    //    GROUP BY p.matchup_batter_id, p.matchup_batter_full_name
                    //)
                Console.WriteLine(query);
                var result = await _context.MLBStatsPitchingSplitss.FromSqlRaw(query).ToListAsync();
                return result;
            
            } else {
                if (IsRecentGamesOption(yearToDateOption)) {
                    query += $@"WITH ";
                    query += rowNumberCTE;
                    if (IsSpecificOpponent(selectedOpponent)) {
                        query += $", ";
                        query += opponentCTE;
                    }
                } else {
                    if (IsSpecificOpponent(selectedOpponent)) {

                        query += $@"WITH ";
                        query += opponentCTE;
                    }
                }

                query += $@"
                    SELECT 
                        {finalTableName}.person_id,
                        {activePlayersTable}.full_name,
                        {finalTableName}.team_name,
                        {teamInfoTable}.league_name,
                        {activePlayersTable}.primary_position_name,
                        SUM(games_played) AS games_played,
                        SUM(games_started) AS games_started,
                        SUM(fly_outs) AS fly_outs,
                        SUM(ground_outs) AS ground_outs,
                        SUM(air_outs) AS air_outs,
                        SUM(runs) AS runs,
                        SUM(doubles) AS doubles,
                        SUM(triples) AS triples,
                        SUM(home_runs) AS home_runs,
                        CASE
                            WHEN SUM(at_bats) = 0
                            THEN 0
                            ELSE CAST(SUM(hits) AS FLOAT) / SUM(at_bats)
                        END AS average,
                        SUM(strike_outs) AS strike_outs,
                        SUM(base_on_balls) AS base_on_balls,
                        SUM(intentional_walks) AS intentional_walks,
                        SUM(hits) AS hits,
                        SUM(hit_by_pitch) AS hit_by_pitch,
                        SUM(caught_stealing) AS caught_stealing,
                        SUM(stolen_bases) AS stolen_bases,
                        SUM(number_of_pitches) AS number_of_pitches,
                        FLOOR(SUM(outs) / 3.0)::int +
                        CASE 
                          WHEN MOD(SUM(outs), 3) = 1 THEN 0.1
                          WHEN MOD(SUM(outs), 3) = 2 THEN 0.2
                          ELSE 0.0
                        END AS innings_pitched,
                        CASE 
                          WHEN SUM(outs) = 0 THEN NULL
                          ELSE ROUND((SUM(earned_runs) * 9.0) / (SUM(outs) / 3.0), 2)
                        END AS era,
                        CASE 
                          WHEN SUM(outs) = 0 THEN NULL
                          ELSE ROUND((SUM(base_on_balls) + SUM(hits)) / (SUM(outs) / 3.0), 2)
                        END AS whip,

                        SUM(wins) AS wins,
                        SUM(losses) AS losses,
                        SUM(saves) AS saves,
                        SUM(save_opportunities) AS save_opportunities,
                        SUM(holds) AS holds,
                        SUM(blown_saves) AS blown_saves,
                        SUM(earned_runs) AS earned_runs,
                        SUM(batters_faced) AS batters_faced,
                        SUM(outs) AS outs,
                        SUM(games_pitched) AS games_pitched,
                        SUM(complete_games) AS complete_games,
                        SUM(shutouts) AS shutouts,
                        SUM(pitches_thrown) AS pitches_thrown,
                        SUM(balls) AS balls,
                        SUM(strikes) AS strikes,
                        SUM(hit_batsmen) AS hit_batsmen,
                        SUM(balks) AS balks,
                        SUM(wild_pitches) AS wild_pitches,
                        SUM(pickoffs) AS pickoffs,
                        SUM(games_finished) AS games_finished,
                        CASE 
                          WHEN SUM(outs) = 0 THEN NULL
                          ELSE ROUND((SUM(runs) * 9.0) / (SUM(outs) / 3.0), 2)
                        END AS runs_scored_per9,
                        CASE 
                          WHEN SUM(outs) = 0 THEN NULL
                          ELSE ROUND((SUM(home_runs) * 9.0) / (SUM(outs) / 3.0), 2)
                        END AS home_runs_per9,
                        CASE 
                          WHEN SUM(outs) = 0 THEN NULL
                          ELSE ROUND((SUM(strike_outs) * 9.0) / (SUM(outs) / 3.0), 2)
                        END AS strikeouts_per9,
                        CASE 
                          WHEN SUM(outs) = 0 THEN NULL
                          ELSE ROUND((SUM(base_on_balls) * 9.0) / (SUM(outs) / 3.0), 2)
                        END AS walks_per9,

                        SUM(inherited_runners) AS inherited_runners,
                        SUM(inherited_runners_scored) AS inherited_runners_scored,
                        SUM(catchers_interference) AS catchers_interference,
                        SUM(sac_bunts) AS sac_bunts,
                        SUM(sac_flies) AS sac_flies,
                        SUM(passed_ball) AS passed_ball,
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
                    GROUP BY {finalTableName}.team_name, {finalTableName}.person_id, 
                        {activePlayersTable}.full_name, {activePlayersTable}.primary_position_name,
                        {teamInfoTable}.league_name
                    ORDER BY {sortField} {order}";

                Console.WriteLine(query);
                var result = await _context.MLBStatsPitchings.FromSqlRaw(query).ToListAsync();
                return result;            
            }
        }






//WITH player_runs AS (
//  SELECT 
//    r.runners_details_player_id, 
//    COUNT(*) AS runs
//  FROM mlb_runners_2023 r
//  JOIN mlb_plays_2023 p 
//    ON r.game_pk = p.game_pk AND r.at_bat_index = p.at_bat_index
//  WHERE r.runners_movement_end = 'score'
//    AND p.matchup_pitch_hand_code = 'L' -- <-- Only vs. lefties
//    AND r.runners_details_player_full_name = 'Freddie Freeman'
//  GROUP BY r.runners_details_player_id
//)
//
//SELECT 
//  p.matchup_batter_id,
//  p.matchup_batter_full_name,
//  COUNT(DISTINCT p.game_pk) AS games_vs_lefties,
//  COUNT(CASE 
//           WHEN p.result_type = 'atBat' 
//           THEN 1.0 
//        END) AS plate_appearances_vs_lefties,
//  COUNT(CASE 
//           WHEN p.result_type = 'atBat' 
//             AND NOT p.result_event_type ILIKE '%sac%'
//             AND NOT p.result_event_type ILIKE '%walk%'
//             AND NOT p.result_event_type ILIKE '%hit_by_pitch%'
//             AND NOT p.result_event_type ILIKE '%catcher_interf%'
//           THEN 1.0 
//        END) AS at_bats_vs_lefties,
//  COUNT(CASE 
//           WHEN p.result_event IN ('Single', 'Double', 'Triple', 'Home Run') 
//           THEN 1.0 
//        END) AS hits_vs_lefties,
//  CASE 
//    WHEN COUNT(CASE 
//                 WHEN p.result_type = 'atBat' 
//                   AND NOT p.result_event_type ILIKE '%sac%'
//                   AND NOT p.result_event_type ILIKE '%walk%'
//                   AND NOT p.result_event_type ILIKE '%hit_by_pitch%'
//                   AND NOT p.result_event_type ILIKE '%catcher_interf%'
//                 THEN 1.0 
//               END) = 0 
//    THEN NULL
//    ELSE 
//      COUNT(CASE 
//               WHEN p.result_event IN ('Single', 'Double', 'Triple', 'Home Run') 
//               THEN 1.0 
//            END)::FLOAT 
//      / 
//      COUNT(CASE 
//               WHEN p.result_type = 'atBat' 
//                 AND NOT p.result_event_type ILIKE '%sac%'
//                 AND NOT p.result_event_type ILIKE '%walk%'
//                 AND NOT p.result_event_type ILIKE '%hit_by_pitch%'
//                 AND NOT p.result_event_type ILIKE '%catcher_interf%'
//               THEN 1.0 
//            END)
//  END AS avg_vs_lefties,
//  r.runs,
//  COUNT(CASE 
//         WHEN p.result_event = 'Double' 
//         THEN 1.0 
//      END) AS doubles,
//  COUNT(CASE 
//         WHEN p.result_event = 'Triples' 
//         THEN 1.0 
//      END) AS triples,
//  COUNT(CASE 
//         WHEN p.result_event = 'Home Run' 
//         THEN 1.0 
//      END) AS home_runs,
//  SUM(p.result_rbi) as rbi,
//  COUNT(CASE 
//         WHEN p.result_event = 'Walk' 
//         THEN 1.0 
//      END) AS walks,
//  COUNT(CASE 
//         WHEN p.result_event = 'Strikeout' 
//         THEN 1.0 
//      END) AS strikeouts,
//  -- OBP Calculation (guarding against div by 0)
//  CASE 
//    WHEN 
//      (COUNT(CASE 
//         WHEN result_type = 'atBat'
//           AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
//         THEN 1 END)
//       + COUNT(CASE WHEN result_event_type = 'walk' THEN 1 END)
//       + COUNT(CASE WHEN result_event_type = 'hit_by_pitch' THEN 1 END)
//       + COUNT(CASE WHEN result_event_type = 'sac_fly' THEN 1 END)) = 0
//    THEN NULL
//    ELSE
//      (COUNT(CASE WHEN result_event IN ('Single', 'Double', 'Triple', 'Home Run') THEN 1 END)
//       + COUNT(CASE WHEN result_event_type = 'walk' THEN 1 END)
//       + COUNT(CASE WHEN result_event_type = 'hit_by_pitch' THEN 1 END))::FLOAT
//      /
//      (COUNT(CASE 
//         WHEN result_type = 'atBat'
//           AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
//         THEN 1 END)
//       + COUNT(CASE WHEN result_event_type = 'walk' THEN 1 END)
//       + COUNT(CASE WHEN result_event_type = 'hit_by_pitch' THEN 1 END)
//       + COUNT(CASE WHEN result_event_type = 'sac_fly' THEN 1 END))
//  END AS obp,
//  -- SLG calculation
//  CASE 
//    WHEN COUNT(CASE 
//                 WHEN result_type = 'atBat' 
//                   AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
//                 THEN 1 
//               END) = 0 
//    THEN NULL
//    ELSE (
//      1 * COUNT(CASE WHEN result_event = 'Single' THEN 1 END) +
//      2 * COUNT(CASE WHEN result_event = 'Double' THEN 1 END) +
//      3 * COUNT(CASE WHEN result_event = 'Triple' THEN 1 END) +
//      4 * COUNT(CASE WHEN result_event = 'Home Run' THEN 1 END)
//    )::FLOAT 
//    /
//    COUNT(CASE 
//             WHEN result_type = 'atBat' 
//               AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
//             THEN 1 
//         END)
//  END AS slg,
//  -- OPS = OBP + SLG
//  CASE 
//    WHEN 
//      COUNT(CASE WHEN result_type = 'atBat' THEN 1 
//                 WHEN result_event_type IN ('walk', 'hit_by_pitch', 'sac_fly') THEN 1 
//           END) = 0 
//      OR 
//      COUNT(CASE 
//               WHEN result_type = 'atBat' 
//                 AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
//               THEN 1 
//           END) = 0 
//    THEN NULL
//    ELSE (
//      (
//        (
//          COUNT(CASE WHEN result_event IN ('Single', 'Double', 'Triple', 'Home Run') THEN 1 END) +
//          COUNT(CASE WHEN result_event_type = 'walk' THEN 1 END) +
//          COUNT(CASE WHEN result_event_type = 'hit_by_pitch' THEN 1 END)
//        )::FLOAT 
//        /
//        COUNT(CASE WHEN result_type = 'atBat' 
//                    THEN 1 
//                    WHEN result_event_type IN ('walk', 'hit_by_pitch', 'sac_fly') THEN 1 
//              END)
//      )
//      +
//      (
//        (
//          1 * COUNT(CASE WHEN result_event = 'Single' THEN 1 END) +
//          2 * COUNT(CASE WHEN result_event = 'Double' THEN 1 END) +
//          3 * COUNT(CASE WHEN result_event = 'Triple' THEN 1 END) +
//          4 * COUNT(CASE WHEN result_event = 'Home Run' THEN 1 END)
//        )::FLOAT 
//        /
//        COUNT(CASE 
//                 WHEN result_type = 'atBat' 
//                   AND result_event_type NOT IN ('walk', 'hit_by_pitch', 'sac_fly', 'sac_bunt', 'catcher_interf') 
//                 THEN 1 
//             END)
//      )
//    )
//  END AS ops
//FROM mlb_plays_2023 p
//LEFT JOIN player_runs r
//  ON p.matchup_batter_id = r.runners_details_player_id
//WHERE p.matchup_pitch_hand_code = 'L'
//  AND p.matchup_batter_full_name = 'Freddie Freeman'
//GROUP BY p.matchup_batter_id, p.matchup_batter_full_name, r.runs
//ORDER BY hits_vs_lefties DESC;


    }

}

