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
            string? order,
            string? sortfield,
            int? selectedPlayer)
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

            if (selectedSplit == "vs. LHP" || selectedSplit == "vs. RHP")
            {
                var split = selectedSplit == "vs. LHP" ? "L" : "R";
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
                    ORDER BY hits DESC;
                ";

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
                        {teamInfoTable}.league_name";

                Console.WriteLine(query);
                var result = await _context.MLBStatsBattings.FromSqlRaw(query).ToListAsync();
                return result;            
            }
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

