﻿using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ReactApp4.Server.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NpgsqlTypes;
using static System.Net.WebRequestMethods;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Numerics;
using System.Reflection.Emit;

namespace ReactApp4.Server.Services
{
    public class BoxScoresDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<IActionResult> GetBoxScores(string season, string boxType, string order, string sortField, int page, string perMode, string selectedTeam)
        {
            try
            {
                if (boxType == "Base")
                {
                    boxType = "Traditional";
                }
                Console.WriteLine(selectedTeam);
                var tableName = $"box_score_{boxType.ToLower()}_{season}";

                var query = $"SELECT * FROM {tableName} WHERE team_id LIKE '%{selectedTeam}%' ORDER BY {sortField} {order}";

                var gamesPlayedQuery = $"WITH GamesPlayed AS( " +
                    $"SELECT COUNT(DISTINCT game_id) AS gp " +
                    $"FROM {tableName} " +
                    $"WHERE min > 0 " +
                    $"GROUP BY player_id " +
                $") ";

                int pageSize = 100;

                if (boxType == "Base")
                {
                    if (perMode == "Totals")
                    {
                        query =
                            $"SELECT " +
                                $"team_id, team_abbreviation, team_city, " +
                                $"player_id, player_name, nickname, start_position, comment, " +
                                $"SUM(min) AS min, " +
                                $"SUM(fgm) AS fgm, " +
                                $"SUM(fga) AS fga, " +
                                $"SUM(fgm) / NULLIF(SUM(fga), 0) AS fg_pct, " +
                                $"SUM(fg3m) AS fg3m, " +
                                $"SUM(fg3a) AS fg3a, " +
                                $"SUM(fg3m) / NULLIF(SUM(fg3a), 0) AS fg3_pct, " +
                                $"SUM(ftm) AS ftm, " +
                                $"SUM(fta) AS fta, " +
                                $"SUM(ftm) / NULLIF(SUM(fta), 0) AS ft_pct, " +
                                $"SUM(oreb) AS oreb, " +
                                $"SUM(dreb) AS dreb, " +
                                $"SUM(reb) AS reb, " +
                                $"SUM(ast) AS ast, " +
                                $"SUM(stl) AS stl, " +
                                $"SUM(blk) AS blk, " +
                                $"SUM(tov) AS tov, " +
                                $"SUM(pf) AS pf, " +
                                $"SUM(pts) AS pts, " +
                                $"SUM(plus_minus) AS plus_minus " +
                            $"FROM " +
                                $"{tableName} boxScoreTable " +
                            $"WHERE " +
                                $"min > 0 " +
                            $"GROUP BY player_id, player_name, team_id, team_abbreviation, team_city, nickname";
                    }
                    else if (perMode == "PerGame")
                    {

                        query = gamesPlayedQuery +
                        $@"
                        SELECT
                            team_id, team_abbreviation, team_city,
                            {tableName}.player_id, player_name, nickname,
                            SUM(min) / GamesPlayed.gp AS min,
                            SUM(fgm) / GamesPlayed.gp AS fgm,
                            SUM(fga) / GamesPlayed.gp AS fga,
                            SUM(fgm) / NULLIF(SUM(fga), 0) AS fg_pct,
                            SUM(fg3m) / GamesPlayed.gp AS fg3m,
                            SUM(fg3a) / GamesPlayed.gp AS fg3a,
                            SUM(fg3m) / NULLIF(SUM(fg3a), 0) AS fg3_pct,
                            SUM(ftm) / GamesPlayed.gp AS ftm,
                            SUM(fta) / GamesPlayed.gp AS fta,
                            SUM(ftm) / NULLIF(SUM(fta), 0) AS ft_pct,
                            SUM(oreb) / GamesPlayed.gp AS oreb,
                            SUM(dreb) / GamesPlayed.gp AS dreb,
                            SUM(reb) / GamesPlayed.gp AS reb,
                            SUM(ast) / GamesPlayed.gp AS ast,
                            SUM(stl) / GamesPlayed.gp AS stl,
                            SUM(blk) / GamesPlayed.gp AS blk,
                            SUM(tov) / GamesPlayed.gp AS tov,
                            SUM(pf) / GamesPlayed.gp AS pf,
                            SUM(pts) / GamesPlayed.gp AS pts,
                            SUM(plus_minus) / GamesPlayed.gp AS plus_minus
                        FROM
                            {tableName}
                        JOIN GamesPlayed
                            ON {tableName}.player_id = GamesPlayed.player_id
                        WHERE
                            min > 0
                        GROUP BY  
                            {tableName}.player_id, player_name, team_id, team_abbreviation, team_city, nickname, GamesPlayed.gp
                        ";

                        Console.WriteLine("HERE");
                        Console.WriteLine(query);

                    }
                    else if (perMode == "PerMinute")
                    {
                        query = gamesPlayedQuery +
                        $@"
                        SELECT
                            team_id, team_abbreviation, team_city,
                            {tableName}.player_id, player_name, nickname,
                            SUM(min) / GamesPlayed.gp AS min,
                            SUM(fgm) / SUM(min) AS fgm,
                            SUM(fga) / SUM(min) AS fga,
                            SUM(fgm) / NULLIF(SUM(fga), 0) AS fg_pct,
                            SUM(fg3m) / SUM(min) AS fg3m,
                            SUM(fg3a) / SUM(min) AS fg3a,
                            SUM(fg3m) / NULLIF(SUM(fg3a), 0) AS fg3_pct,
                            SUM(ftm) / SUM(min) AS ftm,
                            SUM(fta) / SUM(min) AS fta,
                            SUM(ftm) / NULLIF(SUM(fta), 0) AS ft_pct,
                            SUM(oreb) / SUM(min) AS oreb,
                            SUM(dreb) / SUM(min) AS dreb,
                            SUM(reb) / SUM(min) AS reb,
                            SUM(ast) / SUM(min) AS ast,
                            SUM(stl) / SUM(min) AS stl,
                            SUM(blk) / SUM(min) AS blk,
                            SUM(tov) / SUM(min) AS tov,
                            SUM(pf) / SUM(min) AS pf,
                            SUM(pts) / SUM(min) AS pts,
                            SUM(plus_minus) / SUM(min) AS plus_minus
                        FROM
                            {tableName}
                        JOIN GamesPlayed
                            ON {tableName}.player_id = GamesPlayed.player_id
                        WHERE
                            min > 0
                        GROUP BY  
                            {tableName}.player_id, player_name, team_id, team_abbreviation, team_city, nickname, GamesPlayed.gp
                        ";
                        Console.WriteLine(query);

                    }
                    else if (perMode == "PerPossession")
                    {
                        var joinedTable = $"box_score_advanced_{season}";

                        query = gamesPlayedQuery +
                        $@"
                        SELECT
                            team_id, team_abbreviation, team_city,
                            {tableName}.player_id, player_name, nickname,
                            SUM(min) / {joinedTable}.poss AS min,
                            SUM(fgm) / {joinedTable}.poss AS fgm,
                            SUM(fga) / {joinedTable}.poss AS fga,
                            SUM(fgm) / NULLIF(SUM(fga), 0) AS fg_pct,
                            SUM(fg3m) / {joinedTable}.poss AS fg3m,
                            SUM(fg3a) / {joinedTable}.poss AS fg3a,
                            SUM(fg3m) / NULLIF(SUM(fg3a), 0) AS fg3_pct,
                            SUM(ftm) / {joinedTable}.poss AS ftm,
                            SUM(fta) / {joinedTable}.poss AS fta,
                            SUM(ftm) / NULLIF(SUM(fta), 0) AS ft_pct,
                            SUM(oreb) / {joinedTable}.poss AS oreb,
                            SUM(dreb) / {joinedTable}.poss AS dreb,
                            SUM(reb) / {joinedTable}.poss AS reb,
                            SUM(ast) / {joinedTable}.poss AS ast,
                            SUM(stl) / {joinedTable}.poss AS stl,
                            SUM(blk) / {joinedTable}.poss AS blk,
                            SUM(tov) / {joinedTable}.poss AS tov,
                            SUM(pf) / {joinedTable}.poss AS pf,
                            SUM(pts) / {joinedTable}.poss AS pts,
                            SUM(plus_minus) / {joinedTable}.poss AS plus_minus
                        FROM
                            {tableName}
                        JOIN {joinedTable}
                            ON {tableName}.player_id = {joinedTable}.player_id
                        WHERE
                            min > 0
                        GROUP BY  
                            {tableName}.player_id, player_name, team_id, team_abbreviation, team_city, nickname, {joinedTable}.poss";
                    }
                    var boxScores = await _context.BoxScoreTraditionals.FromSqlRaw(query).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    Console.WriteLine(boxScores.Count);
                    return Ok(boxScores);

                }
                else if (boxType == "Advanced")
                {
                    var offRatingQuery = $@"
                        WITH PlayerStats AS (
                        SELECT player_id,
                          team_id,
                          team_abbreviation,
                          SUM(ast) AS ast,
                          SUM(fgm) AS fgm,
                          SUM(fg3m) AS fg3m,
                          SUM(pts) AS pts,
                          SUM(ftm) AS ftm,
                          SUM(fta) AS fta,
                          SUM(fga) AS fga,
                          SUM(oreb) AS orb,
                          SUM(dreb) AS drb,
                          SUM(reb) AS reb,
                          SUM(min) AS min,
                          SUM(tov) AS tov
                          FROM box_score_traditional_{season}
                          GROUP BY player_id, team_id, team_abbreviation
                        ),
                        
                        TeamStats AS(
                          SELECT team_id,
                          SUM(fgm) AS fgm,
                          SUM(fga) AS fga,
                          SUM(fg3a) AS fg3a,
                          SUM(fg3m) AS fg3m,
                          SUM(ftm) AS ftm,
                          SUM(fta) AS fta,
                          SUM(tov) AS tov,
                          SUM(oreb) AS orb,
                          SUM(dreb) AS drb,
                          SUM(reb) AS reb,
                          SUM(pts) AS pts,
                          SUM(min) AS min,
                          SUM(ast) AS ast
                          FROM box_score_traditional_{season}
                          GROUP BY team_id
                        ),  
                        Opponent_RB AS(
                            SELECT
                            t.team_abbreviation,
                            t.team_id,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.reb ELSE 0 END) AS Opponent_TRB,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.oreb ELSE 0 END) AS Opponent_ORB
                        FROM
                            (
                                SELECT DISTINCT team_abbreviation,
                                  team_id
                                FROM league_games_{season}
                            ) AS t
                        JOIN
                            league_games_{season} lg
                            ON(lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%')
                        GROUP BY
                            t.team_abbreviation, t.team_id
                        ),
                        Team_Scoring_Poss AS(
                          SELECT
                          TeamStats.team_id,
                            TeamStats.fgm +(1 - (1 - (TeamStats.ftm / TeamStats.fta)) ^ 2) * TeamStats.fta * 0.4 AS Team_Scoring_Poss
                        
                            FROM TeamStats
                        ),
                        Team_Play_PCT AS(
                          SELECT Team_Scoring_Poss.team_id,
                          Team_Scoring_Poss.Team_Scoring_Poss / (TeamStats.fga + TeamStats.fta * 0.4 + TeamStats.tov) AS Team_Play_PCT
                        
                            FROM Team_Scoring_Poss
                          JOIN TeamStats
                          ON Team_Scoring_Poss.team_id = TeamStats.team_id
                        ),
                        Team_ORB_PCT AS(
                          SELECT TeamStats.team_id,
                          TeamStats.orb / (TeamStats.orb + (Opponent_RB.Opponent_TRB - Opponent_RB.Opponent_ORB)) AS Team_ORB_PCT
                          FROM TeamStats
                          JOIN Opponent_RB
                          ON TeamStats.team_id = Opponent_RB.team_id
                        ),
                        Team_ORB_Weight AS(
                          SELECT Team_ORB_PCT.team_id,
                          ((1 - Team_ORB_PCT.Team_ORB_PCT) *Team_Play_PCT.Team_Play_PCT) / ((1 - Team_ORB_PCT.Team_ORB_PCT) * Team_Play_PCT.Team_Play_PCT + Team_ORB_PCT.Team_ORB_PCT * (1 - Team_Play_PCT.Team_Play_PCT)) AS Team_ORB_Weight
                        
                            FROM Team_ORB_PCT
                          JOIN Team_Play_PCT
                          ON Team_ORB_PCT.team_id = Team_Play_PCT.team_id
                        ),
                        qAST AS(
                             SELECT
                            b.player_id,
                          b.team_id,
                          ((PlayerStats.min / (TeamStats.min / 5)) *(1.14 * ((TeamStats.ast - PlayerStats.ast) / TeamStats.fgm))) +((((TeamStats.ast / TeamStats.min) * PlayerStats.min * 5 - PlayerStats.ast) / ((TeamStats.fgm / TeamStats.min) * PlayerStats.min * 5 - PlayerStats.fgm)) * (1 - (PlayerStats.min / (TeamStats.min / 5)))) AS qAST
                        
                            FROM
                        
                              box_score_traditional_{season} b
                          JOIN PlayerStats
                          ON b.player_id = PlayerStats.player_id AND b.team_id = PlayerStats.team_id
                          JOIN TeamStats
                          ON b.team_id = TeamStats.team_id
                          GROUP BY
                          b.player_id, b.team_id, PlayerStats.min, TeamStats.min, PlayerStats.ast, TeamStats.ast, PlayerStats.fgm, TeamStats.fgm
                        ),
                        PProd_ORB_Part AS(
                          SELECT
                          PlayerStats.player_id,
                          PlayerStats.team_id,
                          PlayerStats.orb* Team_ORB_Weight.Team_ORB_Weight* Team_Play_PCT.Team_Play_PCT* (TeamStats.pts / (TeamStats.fgm + (1 - (1 - (TeamStats.ftm / TeamStats.fta)) ^ 2) * 0.4 * TeamStats.fta)) AS PProd_ORB_Part
                        
                            FROM PlayerStats
                          JOIN Team_ORB_Weight
                          ON PlayerStats.team_id = Team_ORB_Weight.team_id
                          JOIN Team_Play_PCT
                          ON PlayerStats.team_id = Team_Play_PCT.team_id
                          JOIN TeamStats
                          ON PlayerStats.team_id = TeamStats.team_id
                          GROUP BY PlayerStats.player_id, PlayerStats.team_id, PlayerStats.orb, Team_ORB_Weight.Team_ORB_Weight, Team_Play_PCT.Team_Play_PCT, TeamStats.pts, TeamStats.fgm, TeamStats.ftm, TeamStats.fta
                        ),
                        PProd_FG_Part AS(
                          SELECT
                        
                              PlayerStats.player_id,
                          	PlayerStats.team_id,
                            CASE
                                WHEN PlayerStats.fga IS NULL OR PlayerStats.fga = 0 THEN 2 * (PlayerStats.fgm + 0.5 * PlayerStats.fg3m) * (1 - 0.5 * (0) * qAST.qAST)
                        
                                ELSE 2 * (PlayerStats.fgm + 0.5 * PlayerStats.fg3m) * (1 - 0.5 * ((PlayerStats.pts - PlayerStats.ftm) / (2 * PlayerStats.fga)) * qAST.qAST)
                        
                              END AS PProd_FG_Part
                          FROM
                            qAST
                          JOIN PlayerStats ON qAST.player_id = PlayerStats.player_id AND qAST.team_id = PlayerStats.team_id
                        
                             GROUP BY PlayerStats.player_id, PlayerStats.team_id, PlayerStats.fga, PlayerStats.fgm, PlayerStats.fg3m, PlayerStats.pts, PlayerStats.ftm, qAST.qAST
                        ),
                        PProd_AST_Part AS(
                          SELECT PlayerStats.player_id,
                          PlayerStats.team_id,
                          2 * ((TeamStats.fgm - PlayerStats.fgm + 0.5 * (TeamStats.fg3m - PlayerStats.fg3m)) / (TeamStats.fgm - PlayerStats.fgm)) * 0.5 * (((TeamStats.pts - TeamStats.ftm) - (PlayerStats.pts - PlayerStats.ftm)) / (2 * (TeamStats.fga - PlayerStats.fga))) * PlayerStats.ast AS PProd_AST_Part
                        
                            FROM PlayerStats
                          JOIN TeamStats
                          ON PlayerStats.team_id = TeamStats.team_id
                          GROUP BY PlayerStats.player_id, PlayerStats.team_id, TeamStats.fgm, PlayerStats.fgm, TeamStats.fg3m, PlayerStats.fg3m, TeamStats.pts, TeamStats.ftm, PlayerStats.pts, PlayerStats.ftm, TeamStats.fga, PlayerStats.fga, PlayerStats.ast
                        ),
                        Parts AS(
                            SELECT PProd_FG_Part.player_id, PProd_FG_Part.team_id, PProd_FG_Part.PProd_FG_Part AS FG_Part,
                                        PProd_AST_Part.PProd_AST_Part AS AST_Part,
                                PProd_ORB_Part.PProd_ORB_Part AS ORB_Part
                            From PProd_FG_Part
                            join PProd_AST_Part
                            on PProd_FG_Part.player_id = PProd_AST_Part.player_id AND PProd_FG_Part.team_id = PProd_AST_Part.team_id
                        
                            join PProd_ORB_Part
                        
                            on PProd_FG_Part.player_id = PProd_ORB_Part.player_id AND PProd_FG_Part.team_id = PProd_ORB_Part.team_id
                        ),
                        PProd AS(
                            SELECT Parts.player_id, Parts.team_id, (Parts.FG_Part +Parts.AST_Part + PlayerStats.FTM) *(1 - (TeamStats.orb / Team_Scoring_Poss.Team_Scoring_Poss) * Team_ORB_Weight.Team_ORB_Weight * Team_Play_PCT.Team_Play_PCT) + Parts.ORB_Part AS PProd
                            FROM Parts
                            JOIN PlayerStats
                            ON Parts.player_id = PlayerStats.player_id
                            AND Parts.team_id = PlayerStats.team_id
                            JOIN TeamStats
                            ON Parts.team_id = TeamStats.team_id
                            JOIN Team_Scoring_Poss
                            ON Parts.team_id = Team_Scoring_Poss.team_id
                            JOIN Team_ORB_Weight
                            ON Parts.team_id = Team_ORB_Weight.team_id
                            JOIN Team_Play_PCT
                            ON Parts.team_id = Team_Play_PCT.team_id
                            GROUP BY Parts.player_id, Parts.team_id, Parts.FG_Part, Parts.AST_Part, PlayerStats.FTM, TeamStats.orb, Team_Scoring_Poss.Team_Scoring_Poss, Team_ORB_Weight.Team_ORB_Weight, Team_Play_PCT.Team_Play_PCT, Parts.ORB_Part
                        ), 
                        Poss_Parts AS(
                          SELECT PlayerStats.player_id, PlayerStats.team_id,
                          CASE
                                WHEN PlayerStats.fta IS NULL OR PlayerStats.fta = 0 THEN 0
                        
                                    ELSE(1 - (1 - (PlayerStats.ftm / PlayerStats.fta)) ^ 2) * 0.4 * PlayerStats.fta
                          END AS Poss_FT_Part,
                          CASE
                              WHEN PlayerStats.fga IS NULL OR PlayerStats.fga = 0 THEN PlayerStats.fgm * (1 - 0.5 * (0) * qAST.qAST)
                        
                                  ELSE PlayerStats.fgm * (1 - 0.5 * ((PlayerStats.pts - PlayerStats.ftm) / (2 * PlayerStats.fga)) * qAST.qAST)
                        
                          END AS Poss_FG_Part,
                          0.5 * (((TeamStats.pts - TeamStats.ftm) - (PlayerStats.pts - PlayerStats.ftm)) / (2 * (TeamStats.fga - PlayerStats.fga))) * PlayerStats.ast AS Poss_AST_Part,
                          PlayerStats.orb* Team_ORB_Weight.Team_ORB_Weight* Team_Play_PCT.Team_Play_PCT AS Poss_ORB_Part
                          FROM PlayerStats
                          JOIN qAST
                          ON PlayerStats.player_id = qAST.player_id AND PlayerStats.team_id = qAST.team_id
                          JOIN TeamStats
                          ON PlayerStats.team_id = TeamStats.team_id
                          JOIN Team_ORB_Weight
                          ON PlayerStats.team_id = Team_ORB_Weight.team_id
                          JOIN Team_Play_PCT
                          ON PlayerStats.team_id = Team_Play_PCT.team_id
                          GROUP BY PlayerStats.player_id, PlayerStats.team_id, PlayerStats.fta, PlayerStats.ftm, PlayerStats.fga, PlayerStats.fgm, PlayerStats.pts, qAST.qAST, TeamStats.pts, TeamStats.ftm, TeamStats.fga, PlayerStats.ast, PlayerStats.orb, Team_ORB_Weight.Team_ORB_Weight, Team_Play_PCT.Team_Play_PCT
                        ),
                        Scoring_Poss AS(
                          SELECT Poss_Parts.player_id, Poss_Parts.team_id,
                          (Poss_Parts.Poss_FG_Part +Poss_Parts.Poss_AST_Part + Poss_Parts.Poss_FT_Part) *(1 - (TeamStats.orb / Team_Scoring_Poss.Team_Scoring_Poss) * Team_ORB_Weight.Team_ORB_Weight * Team_Play_PCT.Team_Play_PCT) + Poss_Parts.Poss_ORB_Part AS Scoring_Poss
                        
                            FROM Poss_Parts
                          JOIN TeamStats
                          ON Poss_Parts.team_id = TeamStats.team_id
                          JOIN Team_Scoring_Poss
                          ON Poss_Parts.team_id = Team_Scoring_Poss.team_id
                          JOIN Team_ORB_Weight
                          ON Poss_Parts.team_id = Team_ORB_Weight.team_id
                          JOIN Team_Play_PCT
                          ON Poss_Parts.team_id = Team_Play_PCT.team_id
                          GROUP BY Poss_Parts.player_id, Poss_Parts.team_id, Poss_Parts.Poss_FG_Part, Poss_Parts.Poss_AST_Part, Poss_Parts.Poss_FT_Part, TeamStats.ORB, Team_Scoring_Poss.Team_Scoring_Poss, Team_ORB_Weight.Team_ORB_Weight, Team_Play_PCT.Team_Play_PCT, Poss_Parts.Poss_ORB_Part
                        ),
                        xPoss AS(
                            SELECT PlayerStats.player_id, PlayerStats.team_id,
                            (PlayerStats.fga -PlayerStats.fgm) *(1 - 1.07 * Team_ORB_PCT.Team_ORB_PCT) AS FGxPoss,
                            CASE
                        
                                WHEN PlayerStats.fta IS NULL OR PlayerStats.fta = 0 THEN 0
                        
                                    ELSE((1 - (PlayerStats.ftm / PlayerStats.fta)) ^ 2) * 0.4 * PlayerStats.fta
                            END AS FTxPoss
                          FROM PlayerStats
                          JOIN Team_ORB_PCT
                          ON PlayerStats.team_id = Team_ORB_PCT.team_id
                        ),
                        Total_Poss AS(
                            SELECT Scoring_Poss.player_id, Scoring_Poss.team_id,
                            (Scoring_Poss.Scoring_Poss +xPoss.FGxPoss + xPoss.FTxPoss + PlayerStats.tov) AS Total_Poss
                        
                            FROM Scoring_Poss
                          JOIN xPoss
                          ON Scoring_Poss.player_id = xPoss.player_id AND Scoring_Poss.team_id = xPoss.team_id
                          JOIN PlayerStats
                          ON Scoring_Poss.player_id = PlayerStats.player_id AND Scoring_Poss.team_id = PlayerStats.team_id
                        ),
                        Offensive_Rating AS (
                        SELECT PProd.player_id, PProd.team_id,
                          CASE
                              WHEN Total_Poss.Total_Poss IS NULL OR Total_Poss.Total_Poss = 0 THEN 0
                        
                                ELSE 100 * (PProd.PProd / Total_Poss.Total_Poss)
                          END AS Offensive_Rating
                          FROM PProd
                          JOIN Total_Poss
                          ON PProd.player_id = Total_Poss.player_id AND PProd.team_id = Total_Poss.team_id
                        )";

                    var defRatingQuery = $@"
                        TeamStatsD AS (
                          SELECT team_id,
                          team_abbreviation,
                          SUM(dreb) AS drb,
                          SUM(blk) AS blk, 
                          SUM(min) AS min,
                          SUM(stl) AS stl,
                          SUM(pf) AS pf,
                          SUM(fga) AS fga,
                          SUM(tov) AS tov,
                          SUM(oreb) AS orb,
                          SUM(fta) AS fta
                          FROM box_score_traditional_{season}
                        	GROUP BY team_id, team_abbreviation
                        ),
                        
                        PlayerStatsD AS (
                          SELECT player_id,
                          team_id,
                          SUM(stl) AS stl,
                          SUM(blk) AS blk,
                          SUM(dreb) AS drb,
                          SUM(min) AS min,
                          SUM(pf) AS pf
                          FROM box_score_traditional_{season}
                          GROUP BY player_id, team_id
                        ),
                        OpponentStats AS (
                        SELECT 
                            t.team_abbreviation,
                            t.team_id,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.pts ELSE 0 END) AS Opponent_PTS,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.min ELSE 0 END) AS Opponent_MIN,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.fga ELSE 0 END) AS Opponent_FGA,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.fgm ELSE 0 END) AS Opponent_FGM,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.fta ELSE 0 END) AS Opponent_FTA,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.ftm ELSE 0 END) AS Opponent_FTM,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.oreb ELSE 0 END) AS Opponent_ORB,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.tov ELSE 0 END) AS Opponent_TOV,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.dreb ELSE 0 END) AS Opponent_DRB,
                            SUM(CASE WHEN lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%' THEN lg.reb ELSE 0 END) AS Opponent_TRB

                        FROM 
                            (
                                SELECT DISTINCT team_abbreviation,
                              	team_id
                                FROM league_games_{season}
                            ) AS t
                        JOIN 
                            league_games_{season} lg 
                            ON (lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%')
                        GROUP BY 
                            t.team_abbreviation, t.team_id
                        ),
                        DFG_PCT AS (
                          SELECT team_id,
                          OpponentStats.Opponent_FGM / OpponentStats.Opponent_FGA AS DFG_PCT
                          FROM OpponentStats
                          GROUP BY team_id, OpponentStats.Opponent_FGM, OpponentStats.Opponent_FGA
                        ),
                        DOR_PCT AS (
                          SELECT TeamStatsD.team_id,
                          OpponentStats.Opponent_ORB / (OpponentStats.Opponent_ORB + TeamStatsD.drb) AS DOR_PCT
                        	FROM TeamStatsD
                          JOIN OpponentStats
                          ON TeamStatsD.team_id = OpponentStats.team_id
                          GROUP By TeamStatsD.team_id, OpponentStats.Opponent_ORB, TeamStatsD.DRB
                        ),
                        FMwt AS (
                          SELECT DFG_PCT.team_id,
                          (DFG_PCT.DFG_PCT * (1 - DOR_PCT.DOR_PCT)) / (DFG_PCT.DFG_PCT * (1 - DOR_PCT.DOR_PCT) + (1 - DFG_PCT.DFG_PCT) * DOR_PCT.DOR_PCT) AS FMwt
                          FROM DFG_PCT
                          JOIN DOR_PCT
                          ON DFG_PCT.team_id = DOR_PCT.team_id
                          GROUP BY DFG_PCT.team_id, DFG_PCT.DFG_PCT, DOR_PCT.DOR_PCT
                        ),
                        Stops1 AS (
                          SELECT PlayerStatsD.player_id,
                          PlayerStatsD.team_id,
                          PlayerStatsD.stl + PlayerStatsD.blk * FMwt.FMwt * (1 - 1.07 * DOR_PCT.DOR_PCT) + PlayerStatsD.drb * (1 - FMwt.FMwt) AS Stops1
                          FROM PlayerStatsD
                          JOIN FMwt 
                          ON PlayerStatsD.team_id = FMwt.team_id
                          JOIN DOR_PCT
                          ON PlayerStatsD.team_id = DOR_PCT.team_id
                        	GROUP BY PlayerStatsD.player_id, PlayerStatsD.team_id, PlayerStatsD.stl, PlayerStatsD.blk, PlayerStatsD.drb, DOR_PCT.DOR_PCT, FMwt.FMwt
                        ),
                        Stops2 AS (
                        	SELECT
                        	PlayerStatsD.player_id,
                          PlayerStatsD.team_id,
                          (((OpponentStats.Opponent_FGA - OpponentStats.Opponent_FGM - TeamStatsD.blk) / TeamStatsD.min) * FMwt.FMwt * (1 - 1.07 * DOR_PCT.DOR_PCT) + ((OpponentStats.Opponent_TOV - TeamStatsD.stl) / TeamStatsD.min)) * PlayerStatsD.min + (PlayerStatsD.pf / TeamStatsD.pf) * 0.4 * OpponentStats.Opponent_FTA * (1 - (OpponentStats.Opponent_FTM / OpponentStats.Opponent_FTA))^2 AS Stops2 
                        	FROM PlayerStatsD
                          JOIN OpponentStats
                          ON PlayerStatsD.team_id = OpponentStats.team_id
                          JOIN TeamStatsD
                          ON PlayerStatsD.team_id = TeamStatsD.team_id
                        	JOIN FMwt
                          ON PlayerStatsD.team_id = FMwt.team_id
                          JOIN DOR_PCT
                          ON PlayerStatsD.team_id = DOR_PCT.team_id
                        ),
                        Stops AS (
                          SELECT
                          Stops1.player_id,
                          Stops1.team_id,
                          Stops1.Stops1 + Stops2.Stops2 AS Stops
                          FROM Stops1
                          JOIN Stops2
                          ON Stops1.player_id = Stops2.player_id AND Stops1.team_id = Stops2.team_id
                        ),
                        Team_Possessions AS (
                          SELECT
                          TeamStatsD.team_id,
                          TeamStatsD.fga - TeamStatsD.orb + TeamStatsD.tov + 0.4 * TeamStatsD.fta AS Team_Possessions
                          FROM TeamStatsD
                        ),
                        Stop_PCT AS (
                          SELECT
                          PlayerStatsD.player_id,
                          PlayerStatsD.team_id,
                          (Stops.Stops * OpponentStats.Opponent_MIN) / (Team_Possessions.Team_Possessions * PlayerStatsD.min) AS Stop_PCT
                          FROM PlayerStatsD
                          JOIN Stops
                          ON PlayerStatsD.player_id = Stops.player_id AND PlayerStatsD.team_id = Stops.team_id
                          JOIN OpponentStats
                          ON PlayerStatsD.team_id = OpponentStats.team_id
                          JOIN Team_Possessions
                          ON PlayerStatsD.team_id = Team_Possessions.team_id
                        ),
                        Team_Defensive_Rating AS (
                        	SELECT
                          Team_Possessions.team_id,
                          100 * (OpponentStats.Opponent_PTS / Team_Possessions.Team_Possessions) AS Team_Defensive_Rating
                          FROM Team_Possessions
                         	JOIN OpponentStats
                          ON Team_Possessions.team_id = OpponentStats.team_id
                        ),
                        D_Pts_Per_ScPoss AS (
                        	SELECT 
                          OpponentStats.team_id,
                          OpponentStats.Opponent_PTS / (OpponentStats.Opponent_FGM + (1 - (1 - (OpponentStats.Opponent_FTM / OpponentStats.Opponent_FTA))^2) * OpponentStats.Opponent_FTA * 0.4) AS D_Pts_Per_ScPoss
                          FROM OpponentStats
                        ),
                        Defensive_Rating AS (
                          SELECT Stop_PCT.player_id,
                          Stop_PCT.team_id,
                          Team_Defensive_Rating.Team_Defensive_Rating + 0.2 * (100 * D_Pts_Per_ScPoss.D_Pts_Per_ScPoss * (1 - Stop_PCT.Stop_PCT) - Team_Defensive_Rating.Team_Defensive_Rating) AS Defensive_Rating
                          FROM Stop_PCT
                          JOIN Team_Defensive_Rating
                          ON Stop_PCT.team_id = Team_Defensive_Rating.team_id
                          JOIN D_Pts_Per_ScPoss
                          ON Stop_PCT.team_id = D_Pts_Per_ScPoss.team_id
                        )";

                    var advancedStats = $@"
                        Player_Possessions AS (
                            SELECT
                            PlayerStats.player_id,
                            PlayerStats.team_id,
                            (PlayerStats.min / TeamStats.min) * Team_Possessions.Team_Possessions AS Player_Possessions
                            FROM PlayerStats
                            JOIN TeamStats
                            ON PlayerStats.team_id = TeamStats.team_id
                            JOIN Team_Possessions
                            ON PlayerStats.team_id = Team_Possessions.team_id
                        ),
                        
                        Advanced_Stats AS (
                          SELECT
                          PlayerStats.player_id, PlayerStats.team_id,
                          ROUND(100 * PlayerStats.ast / (((PlayerStats.min / (TeamStats.min / 5)) * TeamStats.fgm) - PlayerStats.fgm), 2) AS Ast_Pct,
                          CASE 
                            WHEN PlayerStats.tov IS NULL OR PlayerStats.tov = 0 THEN 0
                            ELSE ROUND(PlayerStats.ast / PlayerStats.tov, 2)
                          END AS Ast_Tov,
                          CASE
                            WHEN (PlayerStats.fga + (0.44 * PlayerStats.fta) + PlayerStats.ast + PlayerStats.tov) IS NULL OR (PlayerStats.fga + (0.44 * PlayerStats.fta) + PlayerStats.ast + PlayerStats.tov) = 0 THEN 0
                            ELSE ROUND((PlayerStats.ast / (PlayerStats.fga + (0.44 * PlayerStats.fta) + PlayerStats.ast + PlayerStats.tov)) * 100, 2)
                          END AS Ast_Ratio,
                          CASE
                          	WHEN (PlayerStats.min * (TeamStats.orb + OpponentStats.Opponent_DRB)) IS NULL OR (PlayerStats.min * (TeamStats.orb + OpponentStats.Opponent_DRB)) = 0 THEN 0
                          	ELSE ROUND(100 * (PlayerStats.orb * (TeamStats.min / 5)) / (PlayerStats.min * (TeamStats.orb + OpponentStats.Opponent_DRB)), 2)
                          END AS Oreb_Pct,
                          CASE
                          	WHEN (PlayerStats.min * (TeamStats.drb + OpponentStats.Opponent_ORB)) IS NULL OR (PlayerStats.min * (TeamStats.drb + OpponentStats.Opponent_ORB)) = 0 THEN 0
                          	ELSE ROUND(100 * (PlayerStats.drb * (TeamStats.min / 5)) / (PlayerStats.min * (TeamStats.drb + OpponentStats.Opponent_ORB)), 2)
                          END AS Dreb_Pct,
                          CASE
                            WHEN (PlayerStats.min * (TeamStats.reb + OpponentStats.Opponent_TRB)) IS NULL OR (PlayerStats.min * (TeamStats.reb + OpponentStats.Opponent_TRB)) = 0 THEN 0
                            ELSE ROUND(100 * (PlayerStats.reb * (TeamStats.min / 5)) / (PlayerStats.min * (TeamStats.reb + OpponentStats.Opponent_TRB)), 2)
                          END AS Reb_Pct,
                          CASE
                            WHEN (PlayerStats.fga + 0.44 * PlayerStats.fta + PlayerStats.tov) IS NULL OR (PlayerStats.fga + 0.44 * PlayerStats.fta + PlayerStats.tov) = 0 THEN 0
                            ELSE ROUND(100 * PlayerStats.tov / (PlayerStats.fga + 0.44 * PlayerStats.fta + PlayerStats.tov), 2)
                          END AS Tov_Pct,
                          CASE
                            WHEN PlayerStats.fga IS NULL OR PlayerStats.fga = 0 THEN 0 
                            ELSE ROUND(100 * (PlayerStats.fgm + 0.5 * PlayerStats.fg3m) / PlayerStats.fga, 2)
                          END AS Efg_Pct,
                          CASE
                            WHEN (2 * (PlayerStats.fga + 0.44 * PlayerStats.fta)) IS NULL OR (2 * (PlayerStats.fga + 0.44 * PlayerStats.fta)) = 0 THEN 0
                            ELSE ROUND(100 * PlayerStats.pts / (2 * (PlayerStats.fga + 0.44 * PlayerStats.fta)), 2)
                          END AS Ts_Pct,
                          CASE
                            WHEN (PlayerStats.min * (TeamStats.fga + 0.44 * TeamStats.fta + TeamStats.tov)) IS NULL OR (PlayerStats.min * (TeamStats.fga + 0.44 * TeamStats.fta + TeamStats.tov)) = 0 THEN 0
                            ELSE ROUND(100 * ((PlayerStats.fga + 0.44 * PlayerStats.fta + PlayerStats.tov) * (TeamStats.min / 5)) / (PlayerStats.min * (TeamStats.fga + 0.44 * TeamStats.fta + TeamStats.tov)), 2)
                          END AS Usg_Pct

                          FROM PlayerStats
                          JOIN TeamStats
                          ON PlayerStats.team_id = TeamStats.team_id
                          JOIN OpponentStats
                          ON PlayerStats.team_id = OpponentStats.team_id

                        )
                    ";

                    if (perMode == "Totals")
                    {
                        query = offRatingQuery + ", " + defRatingQuery + ", " + advancedStats +
                        $@"
                        SELECT
                            box_score_advanced_{season}.team_id, box_score_advanced_{season}.team_abbreviation, box_score_advanced_{season}.team_city,
                            box_score_advanced_{season}.player_id, box_score_advanced_{season}.player_name,
                            SUM(box_score_advanced_{season}.min) AS min,
                            ROUND(Offensive_Rating.Offensive_Rating, 2) AS off_rating,
                            ROUND(Defensive_Rating.Defensive_Rating, 2) AS def_rating,
                            ROUND(Offensive_Rating.Offensive_Rating - Defensive_Rating.Defensive_Rating, 2) AS net_rating,
                            Advanced_Stats.Ast_Pct, 
                            Advanced_Stats.Ast_Tov,
                            Advanced_Stats.Ast_Ratio,
                            Advanced_Stats.Oreb_Pct,
                            Advanced_Stats.Dreb_Pct,
                            Advanced_Stats.Reb_Pct,
                            Advanced_Stats.Tov_Pct,
                            Advanced_Stats.Efg_Pct,
                            Advanced_Stats.Ts_Pct,
                            Advanced_Stats.Usg_Pct
                         
                            FROM box_score_advanced_{season}
                            JOIN Offensive_Rating
                            ON box_score_advanced_{season}.player_id = Offensive_Rating.player_id AND box_score_advanced_{season}.team_id = Offensive_Rating.team_id
                            JOIN Defensive_Rating
                            ON box_score_advanced_{season}.player_id = Defensive_Rating.player_id AND box_score_advanced_{season}.team_id = Defensive_Rating.team_id
						    JOIN Advanced_Stats
                            ON box_score_advanced_{season}.player_id = Advanced_Stats.player_id AND box_score_advanced_{season}.team_id = Advanced_Stats.team_id
                            GROUP BY box_score_advanced_{season}.player_id, box_score_advanced_{season}.player_name, box_score_advanced_{season}.team_id, box_score_advanced_{season}.team_abbreviation, box_score_advanced_{season}.team_city,
                            Offensive_Rating.Offensive_Rating, Defensive_Rating.Defensive_Rating, Advanced_Stats.Ast_Pct, Advanced_Stats.Ast_Tov, Advanced_Stats.Ast_Ratio,
                            Advanced_Stats.Oreb_Pct, Advanced_Stats.Dreb_Pct, Advanced_Stats.Reb_Pct, Advanced_Stats.Tov_Pct, Advanced_Stats.Efg_Pct, Advanced_Stats.Ts_Pct,
                            Advanced_Stats.Usg_Pct
                        ";
                    }

                    Console.WriteLine(query);
                    var boxScores = await _context.BoxScoreAdvancedPlayer.FromSqlRaw(query).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    Console.WriteLine(boxScores.Count);
                    return Ok(boxScores);

                }
                else if (boxType == "FourFactors")
                {
                    var leagueDashLineups = await _context.LeagueDashLineupFourFactors.FromSqlRaw(query).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                    Console.WriteLine(leagueDashLineups.Count);
                    return Ok(leagueDashLineups);
                }
                //else if (boxType == "Misc")
                //{
                //    var joinedTable = $"league_dash_lineups_advanced_{numPlayers}man_{season}";
                //
                //    if (perMode == "Totals")
                //    {
                //        query = $"SELECT * FROM {tableName} WHERE team_id LIKE '%{selectedTeam}%' ORDER BY {sortField} {order}";
                //    }
                //    else if (perMode == "PerPossession")
                //    {
                //        query = $"SELECT {tableName}.id, {tableName}.group_set, {tableName}.group_id, {tableName}.group_name, " +
                //                $"{tableName}.team_id, {tableName}.team_abbreviation, {tableName}.gp, {tableName}.w, {tableName}.l, " +
                //                $"{tableName}.w_pct, {tableName}.min / {joinedTable}.poss AS min, {tableName}.pts_off_tov / {joinedTable}.poss AS pts_off_tov, " +
                //                $"{tableName}.pts_2nd_chance / {joinedTable}.poss AS pts_2nd_chance, {tableName}.pts_fb / {joinedTable}.poss AS pts_fb, " +
                //                $"{tableName}.pts_paint / {joinedTable}.poss AS pts_paint, {tableName}.opp_pts_off_tov / {joinedTable}.poss AS opp_pts_off_tov, {tableName}.opp_pts_2nd_chance / {joinedTable}.poss AS opp_pts_2nd_chance, " +
                //                $"{tableName}.opp_pts_fb / {joinedTable}.poss AS opp_pts_fb, {tableName}.opp_pts_paint / {joinedTable}.poss AS opp_pts_paint, " +
                //                $"{tableName}.gp_rank, {tableName}.w_rank, {tableName}.l_rank, {tableName}.w_pct_rank, {tableName}.min_rank, " +
                //                $"pts_off_tov_rank, pts_2nd_chance_rank, pts_fb_rank, pts_paint_rank, opp_pts_off_tov_rank, opp_pts_2nd_chance_rank, opp_pts_fb_rank, opp_pts_paint_rank " +
                //                $"FROM {tableName} " +
                //                $"INNER JOIN {joinedTable} " +
                //                $"ON {tableName}.group_id = {joinedTable}.group_id " +
                //                $"WHERE {tableName}.team_id LIKE '%{selectedTeam}%' " +
                //                $"ORDER BY {sortField} {order}";
                //    }
                //    else if (perMode == "PerMinute")
                //    {
                //        query = $"SELECT id, group_set, group_id, group_name, " +
                //                $"team_id, team_abbreviation, gp, w, l, " +
                //                $"w_pct, min, pts_off_tov / min AS pts_off_tov, " +
                //                $"pts_2nd_chance / min AS pts_2nd_chance, pts_fb / min AS pts_fb, " +
                //                $"pts_paint / min AS pts_paint, opp_pts_off_tov / min AS opp_pts_off_tov, opp_pts_2nd_chance / min AS opp_pts_2nd_chance, " +
                //                $"opp_pts_fb / min AS opp_pts_fb, opp_pts_paint / min AS opp_pts_paint, " +
                //                $"gp_rank, w_rank, l_rank, w_pct_rank, min_rank, " +
                //                $"pts_off_tov_rank, pts_2nd_chance_rank, pts_fb_rank, pts_paint_rank, opp_pts_off_tov_rank, opp_pts_2nd_chance_rank, opp_pts_fb_rank, opp_pts_paint_rank " +
                //                $"FROM {tableName} " +
                //                $"WHERE team_id LIKE '%{selectedTeam}%' " +
                //                $"ORDER BY {sortField} {order}";
                //    }
                //    else if (perMode == "PerGame")
                //    {
                //        query = $"SELECT id, group_set, group_id, group_name, " +
                //                $"team_id, team_abbreviation, gp, w, l, " +
                //                $"w_pct, min / gp AS min, pts_off_tov / gp AS pts_off_tov, " +
                //                $"pts_2nd_chance / gp AS pts_2nd_chance, pts_fb / gp AS pts_fb, " +
                //                $"pts_paint / gp AS pts_paint, opp_pts_off_tov / gp AS opp_pts_off_tov, opp_pts_2nd_chance / gp AS opp_pts_2nd_chance, " +
                //                $"opp_pts_fb / gp AS opp_pts_fb, opp_pts_paint / gp AS opp_pts_paint, " +
                //                $"gp_rank, w_rank, l_rank, w_pct_rank, min_rank, " +
                //                $"pts_off_tov_rank, pts_2nd_chance_rank, pts_fb_rank, pts_paint_rank, opp_pts_off_tov_rank, opp_pts_2nd_chance_rank, opp_pts_fb_rank, opp_pts_paint_rank " +
                //                $"FROM {tableName} " +
                //                $"WHERE team_id LIKE '%{selectedTeam}%' " +
                //                $"ORDER BY {sortField} {order}";
                //    }
                //    Console.WriteLine(boxType);
                //    var leagueDashLineups = await _context.LeagueDashLineupMiscs.FromSqlRaw(query).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                //    Console.WriteLine(leagueDashLineups.Count);
                //    return Ok(leagueDashLineups);
                //}
                //else if (boxType == "Scoring")
                //{
                //    var leagueDashLineups = await _context.LeagueDashLineupScorings.FromSqlRaw(query).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                //    Console.WriteLine(leagueDashLineups.Count);
                //    return Ok(leagueDashLineups);
                //}
                //else if (boxType == "Opponent")
                //{
                //    var joinedTable = $"league_dash_lineups_advanced_{numPlayers}man_{season}";
                //
                //    if (perMode == "Totals")
                //    {
                //        query = $"SELECT * FROM {tableName} WHERE team_id LIKE '%{selectedTeam}%' ORDER BY {sortField} {order}";
                //    }
                //    else if (perMode == "PerPossession")
                //    {
                //        query = $"SELECT {tableName}.id, {tableName}.group_set, {tableName}.group_id, {tableName}.group_name, " +
                //                $"{tableName}.team_id, {tableName}.team_abbreviation, {tableName}.gp, {tableName}.w, {tableName}.l, " +
                //                $"{tableName}.w_pct, {tableName}.min / {joinedTable}.poss AS min, {tableName}.opp_fgm / {joinedTable}.poss AS opp_fgm, " +
                //                $"{tableName}.opp_fga / {joinedTable}.poss AS opp_fga, {tableName}.opp_fg_pct, {tableName}.opp_fg3m / {joinedTable}.poss AS opp_fg3m, " +
                //                $"{tableName}.opp_fg3a / {joinedTable}.poss AS opp_fg3a, {tableName}.opp_fg3_pct, " +
                //                $"{tableName}.opp_ftm / {joinedTable}.poss AS opp_ftm, {tableName}.opp_fta / {joinedTable}.poss AS opp_fta, " +
                //                $"{tableName}.opp_ft_pct, {tableName}.opp_oreb / {joinedTable}.poss AS opp_oreb, {tableName}.opp_dreb / {joinedTable}.poss AS opp_dreb, " +
                //                $"{tableName}.opp_reb / {joinedTable}.poss AS opp_reb, {tableName}.opp_ast / {joinedTable}.poss AS opp_ast, " +
                //                $"{tableName}.opp_tov / {joinedTable}.poss AS opp_tov, {tableName}.opp_stl / {joinedTable}.poss AS opp_stl, " +
                //                $"{tableName}.opp_blk / {joinedTable}.poss AS opp_blk, {tableName}.opp_blka / {joinedTable}.poss AS opp_blka, " +
                //                $"{tableName}.opp_pf / {joinedTable}.poss AS opp_pf, {tableName}.opp_pfd / {joinedTable}.poss AS opp_pfd, " +
                //                $"{tableName}.opp_pts / {joinedTable}.poss AS opp_pts, {tableName}.plus_minus / {joinedTable}.poss AS plus_minus, " +
                //                $"{tableName}.gp_rank, {tableName}.w_rank, {tableName}.l_rank, {tableName}.w_pct_rank, {tableName}.min_rank, " +
                //                $"opp_fgm_rank, opp_fga_rank, opp_fg_pct_rank, opp_fg3m_rank, opp_fg3a_rank, opp_fg3_pct_rank, opp_ftm_rank, opp_fta_rank, " +
                //                $"opp_ft_pct_rank, opp_oreb_rank, opp_dreb_rank, opp_reb_rank, opp_ast_rank, opp_tov_rank, opp_stl_rank, opp_blk_rank, opp_blka_rank, " +
                //                $"opp_pf_rank, opp_pfd1_rank, opp_pts_rank, plus_minus_rank " +
                //                $"FROM {tableName} " +
                //                $"INNER JOIN {joinedTable} " +
                //                $"ON {tableName}.group_id = {joinedTable}.group_id " +
                //                $"WHERE {tableName}.team_id LIKE '%{selectedTeam}%' " +
                //                $"ORDER BY {sortField} {order}";
                //    }
                //    else if (perMode == "PerMinute")
                //    {
                //        query = $"SELECT id, group_set, group_id, group_name, " +
                //                $"team_id, team_abbreviation, gp, w, l, " +
                //                $"w_pct, min, opp_fgm / min AS opp_fgm, " +
                //                $"opp_fga / min AS opp_fga, opp_fg_pct, opp_fg3m / min AS opp_fg3m, " +
                //                $"opp_fg3a / min AS opp_fg3a, opp_fg3_pct, " +
                //                $"opp_ftm / min AS opp_ftm, opp_fta / min AS opp_fta, " +
                //                $"opp_ft_pct, opp_oreb / min AS opp_oreb, opp_dreb / min AS opp_dreb, " +
                //                $"opp_reb / min AS opp_reb, opp_ast / min AS opp_ast, " +
                //                $"opp_tov / min AS opp_tov, opp_stl / min AS opp_stl, " +
                //                $"opp_blk / min AS opp_blk, opp_blka / min AS opp_blka, " +
                //                $"opp_pf / min AS opp_pf, opp_pfd / min AS opp_pfd, " +
                //                $"opp_pts / min AS opp_pts, plus_minus / min AS plus_minus, " +
                //                $"gp_rank, w_rank, l_rank, w_pct_rank, min_rank, " +
                //                $"opp_fgm_rank, opp_fga_rank, opp_fg_pct_rank, opp_fg3m_rank, opp_fg3a_rank, opp_fg3_pct_rank, opp_ftm_rank, opp_fta_rank, " +
                //                $"opp_ft_pct_rank, opp_oreb_rank, opp_dreb_rank, opp_reb_rank, opp_ast_rank, opp_tov_rank, opp_stl_rank, opp_blk_rank, opp_blka_rank, " +
                //                $"opp_pf_rank, opp_pfd1_rank, opp_pts_rank, plus_minus_rank " +
                //                $"FROM {tableName} " +
                //                $"WHERE team_id LIKE '%{selectedTeam}%' " +
                //                $"ORDER BY {sortField} {order}";
                //    }
                //    else if (perMode == "PerGame")
                //    {
                //        query = $"SELECT id, group_set, group_id, group_name, " +
                //                $"team_id, team_abbreviation, gp, w, l, " +
                //                $"w_pct, min, opp_fgm / gp AS opp_fgm, " +
                //                $"opp_fga / gp AS opp_fga, opp_fg_pct, opp_fg3m / gp AS opp_fg3m, " +
                //                $"opp_fg3a / gp AS opp_fg3a, opp_fg3_pct, " +
                //                $"opp_ftm / gp AS opp_ftm, opp_fta / gp AS opp_fta, " +
                //                $"opp_ft_pct, opp_oreb / gp AS opp_oreb, opp_dreb / gp AS opp_dreb, " +
                //                $"opp_reb / gp AS opp_reb, opp_ast / gp AS opp_ast, " +
                //                $"opp_tov / gp AS opp_tov, opp_stl / gp AS opp_stl, " +
                //                $"opp_blk / gp AS opp_blk, opp_blka / gp AS opp_blka, " +
                //                $"opp_pf / gp AS opp_pf, opp_pfd / gp AS opp_pfd, " +
                //                $"opp_pts / gp AS opp_pts, plus_minus / min AS plus_minus, " +
                //                $"gp_rank, w_rank, l_rank, w_pct_rank, min_rank, " +
                //                $"opp_fgm_rank, opp_fga_rank, opp_fg_pct_rank, opp_fg3m_rank, opp_fg3a_rank, opp_fg3_pct_rank, opp_ftm_rank, opp_fta_rank, " +
                //                $"opp_ft_pct_rank, opp_oreb_rank, opp_dreb_rank, opp_reb_rank, opp_ast_rank, opp_tov_rank, opp_stl_rank, opp_blk_rank, opp_blka_rank, " +
                //                $"opp_pf_rank, opp_pfd1_rank, opp_pts_rank, plus_minus_rank " +
                //                $"FROM {tableName} " +
                //                $"WHERE team_id LIKE '%{selectedTeam}%' " +
                //                $"ORDER BY {sortField} {order}";
                //    }
                //    var leagueDashLineups = await _context.LeagueDashLineupOpponents.FromSqlRaw(query).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                //    Console.WriteLine(leagueDashLineups.Count);
                //    return Ok(leagueDashLineups);

                {
                    return Ok("Not setup yet");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, log, and return an error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
