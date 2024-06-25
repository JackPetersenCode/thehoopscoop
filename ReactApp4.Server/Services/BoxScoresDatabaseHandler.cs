using Microsoft.AspNetCore.Mvc;
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
using ReactApp4.Server.Controllers;

namespace ReactApp4.Server.Services
{
    public class BoxScoresDatabaseHandler(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        public async Task<IActionResult> GetBoxScores(string season, string boxType, string order, string sortField, string perMode, string selectedTeam, string selectedOpponent )
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

                var gamesPlayedQuery =

                        $@"WITH GamesPlayed AS (
                        SELECT COUNT(DISTINCT {tableName}.game_id) AS gp, {tableName}.player_id, {tableName}.team_id
                        FROM {tableName}
                        JOIN league_games_{season}
                        ON {tableName}.game_id = league_games_{season}.game_id
                        AND {tableName}.team_id = league_games_{season}.team_id
                        WHERE {tableName}.min > 0 ";
                if (selectedOpponent != "1")
                {
                    gamesPlayedQuery +=
                        $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') ";
                }
                gamesPlayedQuery += $@"GROUP BY {tableName}.player_id, {tableName}.team_id
                    )";

                var offRatingQuery = $@"
                        PlayerStats AS (
                          SELECT box_score_traditional_{season}.player_id,
					                box_score_traditional_{season}.player_name,
                          box_score_traditional_{season}.team_id,
                          box_score_traditional_{season}.team_abbreviation,
                          SUM(box_score_traditional_{season}.ast) AS ast,
                          SUM(box_score_traditional_{season}.fgm) AS fgm,
                          SUM(box_score_traditional_{season}.fg3a) AS fg3a,
                          SUM(box_score_traditional_{season}.fg3m) AS fg3m,
                          SUM(box_score_traditional_{season}.pts) AS pts,
                          SUM(box_score_traditional_{season}.ftm) AS ftm,
                          SUM(box_score_traditional_{season}.fta) AS fta,
                          SUM(box_score_traditional_{season}.fga) AS fga,
                          SUM(box_score_traditional_{season}.oreb) AS orb,
                          SUM(box_score_traditional_{season}.dreb) AS drb,
                          SUM(box_score_traditional_{season}.reb) AS reb,
                          SUM(box_score_traditional_{season}.min) AS min,
                          SUM(box_score_traditional_{season}.tov) AS tov,
                          SUM(box_score_traditional_{season}.stl) AS stl,
                          SUM(box_score_traditional_{season}.blk) AS blk,
                          SUM(box_score_traditional_{season}.pf) AS pf
                          FROM box_score_traditional_{season}
                          JOIN league_games_{season}
                          ON box_score_traditional_{season}.game_id = league_games_{season}.game_id
                          AND box_score_traditional_{season}.team_id = league_games_{season}.team_id
                          WHERE box_score_traditional_{season}.team_id LIKE '%{selectedTeam}%' ";
                if (selectedOpponent != "1")
                {
                    Console.WriteLine("*****************************************************************************************************************************************888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888");
                    offRatingQuery +=
                        $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') ";

                }


                offRatingQuery += $@"GROUP BY box_score_traditional_{season}.player_id, box_score_traditional_{season}.player_name, box_score_traditional_{season}.team_id, box_score_traditional_{season}.team_abbreviation 
                          HAVING SUM(box_score_traditional_{season}.min) > 0
                        ),
                        PlayerStatsAdvanced AS (
                         Select box_score_advanced_{season}.player_id,
					                box_score_advanced_{season}.player_name,
                          box_score_advanced_{season}.team_id,
                          box_score_advanced_{season}.team_abbreviation,
                          ROUND(AVG(pie) * 100, 2) as pie,
                          SUM(poss) as poss
                          FROM box_score_advanced_{season}
                          JOIN league_games_{season}
                          ON box_score_advanced_{season}.game_id = league_games_{season}.game_id
                          AND box_score_advanced_{season}.team_id = league_games_{season}.team_id
                          WHERE box_score_advanced_{season}.team_id LIKE '%{selectedTeam}%' ";
                if (selectedOpponent != "1")
                {
                    offRatingQuery += $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') ";

                }
                offRatingQuery += $@"GROUP BY box_score_advanced_{season}.player_id, box_score_advanced_{season}.player_name, box_score_advanced_{season}.team_id, box_score_advanced_{season}.team_abbreviation
                          HAVING SUM(box_score_advanced_{season}.min) > 0
                        ),
                        TeamStats AS(
                          SELECT box_score_traditional_{season}.team_id,
                          box_score_traditional_{season}.team_abbreviation,
                          SUM(box_score_traditional_{season}.fgm) AS fgm,
                          SUM(box_score_traditional_{season}.fga) AS fga,
                          SUM(box_score_traditional_{season}.fg3a) AS fg3a,
                          SUM(box_score_traditional_{season}.fg3m) AS fg3m,
                          SUM(box_score_traditional_{season}.ftm) AS ftm,
                          SUM(box_score_traditional_{season}.fta) AS fta,
                          SUM(box_score_traditional_{season}.tov) AS tov,
                          SUM(box_score_traditional_{season}.oreb) AS orb,
                          SUM(box_score_traditional_{season}.dreb) AS drb,
                          SUM(box_score_traditional_{season}.reb) AS reb,
                          SUM(box_score_traditional_{season}.pts) AS pts,
                          SUM(box_score_traditional_{season}.min) AS min,
                          SUM(box_score_traditional_{season}.ast) AS ast,
                          SUM(box_score_traditional_{season}.blk) AS blk, 
                          SUM(box_score_traditional_{season}.stl) AS stl,
                          SUM(box_score_traditional_{season}.pf) AS pf
                          FROM box_score_traditional_{season}
                          JOIN league_games_{season}
                          ON box_score_traditional_{season}.game_id = league_games_{season}.game_id
                          AND box_score_traditional_{season}.team_id = league_games_{season}.team_id
                          WHERE box_score_traditional_{season}.team_id LIKE '%{selectedTeam}%' ";

                if (selectedOpponent != "1")
                {
                    offRatingQuery +=
                        $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') ";
                }

                offRatingQuery += $@"GROUP BY box_score_traditional_{season}.team_id, box_score_traditional_{season}.team_abbreviation
                          HAVING SUM(box_score_traditional_{season}.min) > 0
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
                            league_games_{season} lg ";
                if (selectedOpponent != "1")
                {
                    offRatingQuery += $@"ON(lg.matchup LIKE '%{selectedOpponent} vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%{selectedOpponent} @ ' || t.team_abbreviation || '%') ";
                }
                else
                {
                    offRatingQuery += $@"ON(lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%') ";
                }
                offRatingQuery += $@"GROUP BY
                            t.team_abbreviation, t.team_id
                        ),
                        Team_Scoring_Poss AS(
                          SELECT
                          TeamStats.team_id,
                            TeamStats.fgm + (1 - (1 - (TeamStats.ftm / TeamStats.fta)) ^ 2) * TeamStats.fta * 0.4 AS Team_Scoring_Poss
                        
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
                          ((1 - Team_ORB_PCT.Team_ORB_PCT) * Team_Play_PCT.Team_Play_PCT) / ((1 - Team_ORB_PCT.Team_ORB_PCT) * Team_Play_PCT.Team_Play_PCT + Team_ORB_PCT.Team_ORB_PCT * (1 - Team_Play_PCT.Team_Play_PCT)) AS Team_ORB_Weight
                        
                            FROM Team_ORB_PCT
                          JOIN Team_Play_PCT
                          ON Team_ORB_PCT.team_id = Team_Play_PCT.team_id
                        ),
                        qAST AS(
                          SELECT
                          PlayerStats.player_id,
                          PlayerStats.team_id,
                          ((PlayerStats.min / NULLIF((TeamStats.min / 5), 0)) * (1.14 * NULLIF(((TeamStats.ast - PlayerStats.ast) / TeamStats.fgm), 0))) +
                              ((((NULLIF((TeamStats.ast / TeamStats.min), 0)) * PlayerStats.min * 5 - PlayerStats.ast) /
                              NULLIF(((TeamStats.fgm / TeamStats.min) * PlayerStats.min * 5 - PlayerStats.fgm), 0)) *
                              (1 - (PlayerStats.min / NULLIF((TeamStats.min / 5), 0)))) AS qAST
                          FROM
                              PlayerStats
                          JOIN TeamStats
                          ON PlayerStats.team_id = TeamStats.team_id
                          GROUP BY
                          PlayerStats.player_id, PlayerStats.team_id, PlayerStats.min, TeamStats.min, PlayerStats.ast, TeamStats.ast, PlayerStats.fgm, TeamStats.fgm
                        ),
                        PProd_ORB_Part AS(
                          SELECT
                          PlayerStats.player_id,
                          PlayerStats.team_id,
                          PlayerStats.orb * Team_ORB_Weight.Team_ORB_Weight * Team_Play_PCT.Team_Play_PCT * (TeamStats.pts / (TeamStats.fgm + (1 - (1 - (TeamStats.ftm / TeamStats.fta)) ^ 2) * 0.4 * TeamStats.fta)) AS PProd_ORB_Part
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
                            league_games_{season} lg ";
                if (selectedOpponent != "1")
                {
                    defRatingQuery += $@"ON(lg.matchup LIKE '%{selectedOpponent} vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%{selectedOpponent} @ ' || t.team_abbreviation || '%') ";
                }
                else
                {
                    defRatingQuery += $@"ON(lg.matchup LIKE '%vs. ' || t.team_abbreviation || '%' OR lg.matchup LIKE '%@ ' || t.team_abbreviation || '%') ";
                }
                defRatingQuery += $@"GROUP BY 
                            t.team_abbreviation, t.team_id
                        ),
                        DFG_PCT AS (
                          SELECT team_id,
                          OpponentStats.Opponent_FGM / OpponentStats.Opponent_FGA AS DFG_PCT
                          FROM OpponentStats
                          GROUP BY team_id, OpponentStats.Opponent_FGM, OpponentStats.Opponent_FGA
                        ),
                        DOR_PCT AS (
                          SELECT TeamStats.team_id,
                          OpponentStats.Opponent_ORB / (OpponentStats.Opponent_ORB + TeamStats.drb) AS DOR_PCT
                        	FROM TeamStats
                          JOIN OpponentStats
                          ON TeamStats.team_id = OpponentStats.team_id
                          GROUP By TeamStats.team_id, OpponentStats.Opponent_ORB, TeamStats.DRB
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
                          SELECT PlayerStats.player_id,
                          PlayerStats.team_id,
                          PlayerStats.stl + PlayerStats.blk * FMwt.FMwt * (1 - 1.07 * DOR_PCT.DOR_PCT) + PlayerStats.drb * (1 - FMwt.FMwt) AS Stops1
                          FROM PlayerStats
                          JOIN FMwt 
                          ON PlayerStats.team_id = FMwt.team_id
                          JOIN DOR_PCT
                          ON PlayerStats.team_id = DOR_PCT.team_id
                        	GROUP BY PlayerStats.player_id, PlayerStats.team_id, PlayerStats.stl, PlayerStats.blk, PlayerStats.drb, DOR_PCT.DOR_PCT, FMwt.FMwt
                        ),
                        Stops2 AS (
                        	SELECT
                        	PlayerStats.player_id,
                          PlayerStats.team_id,
                          (((OpponentStats.Opponent_FGA - OpponentStats.Opponent_FGM - TeamStats.blk) / TeamStats.min) * FMwt.FMwt * (1 - 1.07 * DOR_PCT.DOR_PCT) + ((OpponentStats.Opponent_TOV - TeamStats.stl) / TeamStats.min)) * PlayerStats.min + (PlayerStats.pf / TeamStats.pf) * 0.4 * OpponentStats.Opponent_FTA * (1 - (OpponentStats.Opponent_FTM / OpponentStats.Opponent_FTA))^2 AS Stops2 
                        	FROM PlayerStats
                          JOIN OpponentStats
                          ON PlayerStats.team_id = OpponentStats.team_id
                          JOIN TeamStats
                          ON PlayerStats.team_id = TeamStats.team_id
                        	JOIN FMwt
                          ON PlayerStats.team_id = FMwt.team_id
                          JOIN DOR_PCT
                          ON PlayerStats.team_id = DOR_PCT.team_id
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
                          TeamStats.team_id,
                          TeamStats.fga - TeamStats.orb + TeamStats.tov + 0.4 * TeamStats.fta AS Team_Possessions
                          FROM TeamStats
                        ),
                        Stop_PCT AS (
                          SELECT
                          PlayerStats.player_id,
                          PlayerStats.team_id,
                          CASE
                          	WHEN (Team_Possessions.Team_Possessions * PlayerStats.min) IS NULL OR (Team_Possessions.Team_Possessions * PlayerStats.min) = 0 THEN 0
                          	ELSE (Stops.Stops * OpponentStats.Opponent_MIN) / (Team_Possessions.Team_Possessions * PlayerStats.min) 
                          END AS Stop_PCT  FROM PlayerStats
                          JOIN Stops
                          ON PlayerStats.player_id = Stops.player_id AND PlayerStats.team_id = Stops.team_id
                          JOIN OpponentStats
                          ON PlayerStats.team_id = OpponentStats.team_id
                          JOIN Team_Possessions
                          ON PlayerStats.team_id = Team_Possessions.team_id
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
                          PlayerStats.player_id, PlayerStats.player_name, PlayerStats.team_id, PlayerStats.team_abbreviation,
                          PlayerStats.min AS min,
                          CASE
                          	WHEN (((PlayerStats.min / (TeamStats.min / 5)) * TeamStats.fgm) - PlayerStats.fgm) IS NULL OR (((PlayerStats.min / (TeamStats.min / 5)) * TeamStats.fgm) - PlayerStats.fgm) = 0 THEN 0
                          	ELSE ROUND(PlayerStats.ast / (((PlayerStats.min / (TeamStats.min / 5)) * TeamStats.fgm) - PlayerStats.fgm), 2)
                          END AS Ast_Pct,
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
                          	ELSE ROUND((PlayerStats.orb * (TeamStats.min / 5)) / (PlayerStats.min * (TeamStats.orb + OpponentStats.Opponent_DRB)), 2)
                          END AS Oreb_Pct,
                          CASE
                          	WHEN (PlayerStats.min * (TeamStats.drb + OpponentStats.Opponent_ORB)) IS NULL OR (PlayerStats.min * (TeamStats.drb + OpponentStats.Opponent_ORB)) = 0 THEN 0
                          	ELSE ROUND((PlayerStats.drb * (TeamStats.min / 5)) / (PlayerStats.min * (TeamStats.drb + OpponentStats.Opponent_ORB)), 2)
                          END AS Dreb_Pct,
                          CASE
                            WHEN (PlayerStats.min * (TeamStats.reb + OpponentStats.Opponent_TRB)) IS NULL OR (PlayerStats.min * (TeamStats.reb + OpponentStats.Opponent_TRB)) = 0 THEN 0
                            ELSE ROUND((PlayerStats.reb * (TeamStats.min / 5)) / (PlayerStats.min * (TeamStats.reb + OpponentStats.Opponent_TRB)), 2)
                          END AS Reb_Pct,
                          CASE
                            WHEN (PlayerStats.fga + 0.44 * PlayerStats.fta + PlayerStats.tov) IS NULL OR (PlayerStats.fga + 0.44 * PlayerStats.fta + PlayerStats.tov) = 0 THEN 0
                            ELSE ROUND(PlayerStats.tov / (PlayerStats.fga + 0.44 * PlayerStats.fta + PlayerStats.tov), 2)
                          END AS Tov_Pct,
                          CASE
                            WHEN PlayerStats.fga IS NULL OR PlayerStats.fga = 0 THEN 0 
                            ELSE ROUND((PlayerStats.fgm + 0.5 * PlayerStats.fg3m) / PlayerStats.fga, 2)
                          END AS Efg_Pct,
                          CASE
                            WHEN (2 * (PlayerStats.fga + 0.44 * PlayerStats.fta)) IS NULL OR (2 * (PlayerStats.fga + 0.44 * PlayerStats.fta)) = 0 THEN 0
                            ELSE ROUND(PlayerStats.pts / (2 * (PlayerStats.fga + 0.44 * PlayerStats.fta)), 2)
                          END AS Ts_Pct,
                          CASE
                            WHEN (PlayerStats.min * (TeamStats.fga + 0.44 * TeamStats.fta + TeamStats.tov)) IS NULL OR (PlayerStats.min * (TeamStats.fga + 0.44 * TeamStats.fta + TeamStats.tov)) = 0 THEN 0
                            ELSE ROUND(((PlayerStats.fga + 0.44 * PlayerStats.fta + PlayerStats.tov) * (TeamStats.min / 5)) / (PlayerStats.min * (TeamStats.fga + 0.44 * TeamStats.fta + TeamStats.tov)), 2)
                          END AS Usg_Pct


                          FROM PlayerStats
                          JOIN TeamStats
                          ON PlayerStats.team_id = TeamStats.team_id
                          JOIN OpponentStats
                          ON PlayerStats.team_id = OpponentStats.team_id
                        )
                    ";


                //int pageSize = 100;

                Console.WriteLine("BoxType: ==> ");
                Console.WriteLine(boxType);
                if (boxType == "Traditional")
                {
                    if (perMode == "Totals")
                    {
                        query =
                            $@"SELECT 
                                {tableName}.team_id, {tableName}.team_abbreviation, {tableName}.team_city, 
                                player_id, player_name, 
                                SUM({tableName}.min) AS min, 
                                SUM({tableName}.fgm) AS fgm, 
                                SUM({tableName}.fga) AS fga, 
                                SUM({tableName}.fgm) / NULLIF(SUM({tableName}.fga), 0) AS fg_pct, 
                                SUM({tableName}.fg3m) AS fg3m, 
                                SUM({tableName}.fg3a) AS fg3a, 
                                SUM({tableName}.fg3m) / NULLIF(SUM({tableName}.fg3a), 0) AS fg3_pct, 
                                SUM({tableName}.ftm) AS ftm, 
                                SUM({tableName}.fta) AS fta, 
                                SUM({tableName}.ftm) / NULLIF(SUM({tableName}.fta), 0) AS ft_pct, 
                                SUM({tableName}.oreb) AS oreb, 
                                SUM({tableName}.dreb) AS dreb, 
                                SUM({tableName}.reb) AS reb, 
                                SUM({tableName}.ast) AS ast, 
                                SUM({tableName}.stl) AS stl, 
                                SUM({tableName}.blk) AS blk, 
                                SUM({tableName}.tov) AS tov, 
                                SUM({tableName}.pf) AS pf, 
                                SUM({tableName}.pts) AS pts, 
                                SUM({tableName}.plus_minus) AS plus_minus 
                            FROM 
                                {tableName}
                            JOIN league_games_{season}
                            ON {tableName}.game_id = league_games_{season}.game_id
                            AND {tableName}.team_id = league_games_{season}.team_id
                            WHERE 
                                {tableName}.min > 0 ";
                        if (selectedOpponent != "1")
                        {
                            query += $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') 
                                        AND {tableName}.team_abbreviation != '{selectedOpponent}' ";

                        }

                        query += $@"AND {tableName}.team_id LIKE '%{selectedTeam}%' 
                            GROUP BY player_id, player_name, {tableName}.team_id, {tableName}.team_abbreviation, team_city 
                            ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per Game")
                    {

                        query = gamesPlayedQuery +
                        $@"
                        SELECT
                            {tableName}.team_id, {tableName}.team_abbreviation, team_city,
                            {tableName}.player_id, player_name, 
                            SUM({tableName}.min) / GamesPlayed.gp AS min,
                            SUM({tableName}.fgm) / GamesPlayed.gp AS fgm,
                            SUM({tableName}.fga) / GamesPlayed.gp AS fga,
                            SUM({tableName}.fgm) / NULLIF(SUM({tableName}.fga), 0) AS fg_pct,
                            SUM({tableName}.fg3m) / GamesPlayed.gp AS fg3m,
                            SUM({tableName}.fg3a) / GamesPlayed.gp AS fg3a,
                            SUM({tableName}.fg3m) / NULLIF(SUM({tableName}.fg3a), 0) AS fg3_pct,
                            SUM({tableName}.ftm) / GamesPlayed.gp AS ftm,
                            SUM({tableName}.fta) / GamesPlayed.gp AS fta,
                            SUM({tableName}.ftm) / NULLIF(SUM({tableName}.fta), 0) AS ft_pct,
                            SUM({tableName}.oreb) / GamesPlayed.gp AS oreb,
                            SUM({tableName}.dreb) / GamesPlayed.gp AS dreb,
                            SUM({tableName}.reb) / GamesPlayed.gp AS reb,
                            SUM({tableName}.ast) / GamesPlayed.gp AS ast,
                            SUM({tableName}.stl) / GamesPlayed.gp AS stl,
                            SUM({tableName}.blk) / GamesPlayed.gp AS blk,
                            SUM({tableName}.tov) / GamesPlayed.gp AS tov,
                            SUM({tableName}.pf) / GamesPlayed.gp AS pf,
                            SUM({tableName}.pts) / GamesPlayed.gp AS pts,
                            SUM({tableName}.plus_minus) / GamesPlayed.gp AS plus_minus
                        FROM
                            {tableName}
                        JOIN GamesPlayed
                            ON {tableName}.player_id = GamesPlayed.player_id
                            AND {tableName}.team_id = GamesPlayed.team_id
                        JOIN league_games_{season}
                            ON {tableName}.game_id = league_games_{season}.game_id
                            AND {tableName}.team_id = league_games_{season}.team_id
                        WHERE
                            {tableName}.min > 0
                        AND 
                            {tableName}.team_id LIKE '%{selectedTeam}%' ";
                        if (selectedOpponent != "1")
                        {
                            query += $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') 
                                        AND {tableName}.team_abbreviation != '{selectedOpponent}' ";

                        }
                        query += $@"GROUP BY  
                            {tableName}.player_id, {tableName}.player_name, {tableName}.team_id, {tableName}.team_abbreviation, team_city, GamesPlayed.gp
                        ORDER BY {sortField} {order}";

                        Console.WriteLine("HERE");
                        Console.WriteLine(query);

                    }
                    else if (perMode == "Per Minute" || perMode == "Per 12 Minutes" || perMode == "Per 24 Minutes")
                    {
                        var nMinutes = 1;

                        if (perMode == "Per 12 Minutes")
                        {
                            nMinutes = 12;
                        }
                        else if (perMode == "Per 24 Minutes")
                        {
                            nMinutes = 24;
                        }

                        query = gamesPlayedQuery +
                        $@"
                        SELECT
                            {tableName}.team_id, {tableName}.team_abbreviation, team_city,
                            {tableName}.player_id, player_name, 
                            SUM({tableName}.min) / GamesPlayed.gp AS min,
                            {nMinutes} * (SUM({tableName}.fgm) / NULLIF(SUM({tableName}.min), 0)) AS fgm,
                            {nMinutes} * (SUM({tableName}.fga) / NULLIF(SUM({tableName}.min), 0)) AS fga,
                            SUM({tableName}.fgm) / NULLIF(SUM({tableName}.fga), 0) AS fg_pct,
                            {nMinutes} * (SUM({tableName}.fg3m) / NULLIF(SUM({tableName}.min), 0)) AS fg3m,
                            {nMinutes} * (SUM({tableName}.fg3a) / NULLIF(SUM({tableName}.min), 0)) AS fg3a,
                            SUM({tableName}.fg3m) / NULLIF(SUM({tableName}.fg3a), 0) AS fg3_pct,
                            {nMinutes} * (SUM({tableName}.ftm) / NULLIF(SUM({tableName}.min), 0)) AS ftm,
                            {nMinutes} * (SUM({tableName}.fta) / NULLIF(SUM({tableName}.min), 0)) AS fta,
                            SUM({tableName}.ftm) / NULLIF(SUM({tableName}.fta), 0) AS ft_pct,
                            {nMinutes} * (SUM({tableName}.oreb) / NULLIF(SUM({tableName}.min), 0)) AS oreb,
                            {nMinutes} * (SUM({tableName}.dreb) / NULLIF(SUM({tableName}.min), 0)) AS dreb,
                            {nMinutes} * (SUM({tableName}.reb) / NULLIF(SUM({tableName}.min), 0)) AS reb,
                            {nMinutes} * (SUM({tableName}.ast) / NULLIF(SUM({tableName}.min), 0)) AS ast,
                            {nMinutes} * (SUM({tableName}.stl) / NULLIF(SUM({tableName}.min), 0)) AS stl,
                            {nMinutes} * (SUM({tableName}.blk) / NULLIF(SUM({tableName}.min), 0)) AS blk,
                            {nMinutes} * (SUM({tableName}.tov) / NULLIF(SUM({tableName}.min), 0)) AS tov,
                            {nMinutes} * (SUM({tableName}.pf) / NULLIF(SUM({tableName}.min), 0)) AS pf,
                            {nMinutes} * (SUM({tableName}.pts) / NULLIF(SUM({tableName}.min), 0)) AS pts,
                            {nMinutes} * (SUM({tableName}.plus_minus) / NULLIF(SUM({tableName}.min), 0)) AS plus_minus
                        FROM
                            {tableName}
                        JOIN GamesPlayed
                            ON {tableName}.player_id = GamesPlayed.player_id
                            AND {tableName}.team_id = GamesPlayed.team_id
                        JOIN league_games_{season}
                            ON {tableName}.game_id = league_games_{season}.game_id
                            AND {tableName}.team_id = league_games_{season}.team_id
                        WHERE
                            {tableName}.min > 0
                        AND
                            {tableName}.team_id LIKE '%{selectedTeam}%' ";
                        if (selectedOpponent != "1")
                        {
                            query += $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') 
                                        AND {tableName}.team_abbreviation != '{selectedOpponent}' ";
                        }
                        query += $@"GROUP BY {tableName}.player_id, {tableName}.player_name, {tableName}.team_id, {tableName}.team_abbreviation, team_city, GamesPlayed.gp 
                        ORDER BY {sortField} {order}";
                        Console.WriteLine(query);

                    }
                    else if (perMode == "Per 100 Poss")
                    {
                        var joinedTable = $"box_score_advanced_{season}";

                        query = gamesPlayedQuery +
                        $@"
                        SELECT
                            {tableName}.team_id, {tableName}.team_abbreviation, {tableName}.team_city,
                            {tableName}.player_id, {tableName}.player_name, 
                            100 * SUM({tableName}.min) / NULLIF(SUM({joinedTable}.poss), 0) AS min,
                            100 * SUM({tableName}.fgm) / NULLIF(SUM({joinedTable}.poss), 0) AS fgm,
                            100 * SUM({tableName}.fga) / NULLIF(SUM({joinedTable}.poss), 0) AS fga,
                            SUM({tableName}.fgm) / NULLIF(SUM({tableName}.fga), 0) AS fg_pct,
                            100 * SUM({tableName}.fg3m) / NULLIF(SUM({joinedTable}.poss), 0) AS fg3m,
                            100 * SUM({tableName}.fg3a) / NULLIF(SUM({joinedTable}.poss), 0) AS fg3a,
                            SUM({tableName}.fg3m) / NULLIF(SUM({tableName}.fg3a), 0) AS fg3_pct,
                            100 * SUM({tableName}.ftm) / NULLIF(SUM({joinedTable}.poss), 0) AS ftm,
                            100 * SUM({tableName}.fta) / NULLIF(SUM({joinedTable}.poss), 0) AS fta,
                            SUM({tableName}.ftm) / NULLIF(SUM({tableName}.fta), 0) AS ft_pct,
                            100 * SUM({tableName}.oreb) / NULLIF(SUM({joinedTable}.poss), 0) AS oreb,
                            100 * SUM({tableName}.dreb) / NULLIF(SUM({joinedTable}.poss), 0) AS dreb,
                            100 * SUM({tableName}.reb) / NULLIF(SUM({joinedTable}.poss), 0) AS reb,
                            100 * SUM({tableName}.ast) / NULLIF(SUM({joinedTable}.poss), 0) AS ast,
                            100 * SUM({tableName}.stl) / NULLIF(SUM({joinedTable}.poss), 0) AS stl,
                            100 * SUM({tableName}.blk) / NULLIF(SUM({joinedTable}.poss), 0) AS blk,
                            100 * SUM({tableName}.tov) / NULLIF(SUM({joinedTable}.poss), 0) AS tov,
                            100 * SUM({tableName}.pf) / NULLIF(SUM({joinedTable}.poss), 0) AS pf,
                            100 * SUM({tableName}.pts) / NULLIF(SUM({joinedTable}.poss), 0) AS pts,
                            100 * SUM({tableName}.plus_minus) / NULLIF(SUM({joinedTable}.poss), 0) AS plus_minus
                        FROM
                            {tableName}
                        JOIN {joinedTable}
                            ON {tableName}.player_id = {joinedTable}.player_id
                            AND {tableName}.team_id = {joinedTable}.team_id
                            AND {tableName}.game_id = {joinedTable}.game_id
                        JOIN league_games_{season}
                            ON {tableName}.game_id = league_games_{season}.game_id
                            AND {tableName}.team_id = league_games_{season}.team_id
                        WHERE
                            {tableName}.min > 0
                        AND
                            {tableName}.team_id LIKE '%{selectedTeam}%' ";
                        if (selectedOpponent != "1")
                        {
                            query += $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') 
                                        AND {tableName}.team_abbreviation != '{selectedOpponent}' ";
                        }
                        query += $@"GROUP BY {tableName}.player_id, {tableName}.player_name, {tableName}.team_id, {tableName}.team_abbreviation, {tableName}.team_city 
                        ORDER BY {sortField} {order}";
                    }
                    var boxScores = await _context.BoxScoreTraditionalPlayers.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(boxScores.Count);
                    return Ok(boxScores);

                }
                else if (boxType == "Advanced")
                {
                    Console.WriteLine(selectedTeam);
                    Console.WriteLine(selectedOpponent);

                    if (sortField == "min")
                    {
                        sortField = "min";
                    }

                    query = $@"WITH " + offRatingQuery + ", " + defRatingQuery + ", " + advancedStats +
                    $@"
                    SELECT
                        Advanced_Stats.team_id, Advanced_Stats.team_abbreviation, 
                        Advanced_Stats.player_id, Advanced_Stats.player_name,
                        Advanced_Stats.min,
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
                        Advanced_Stats.Usg_Pct,
                        PlayerStatsAdvanced.pie AS Pie,
                        PlayerStatsAdvanced.poss AS Poss
                        FROM Advanced_Stats
                        JOIN Offensive_Rating
                        ON Advanced_Stats.player_id = Offensive_Rating.player_id AND Advanced_Stats.team_id = Offensive_Rating.team_id
                        JOIN Defensive_Rating
                        ON Advanced_Stats.player_id = Defensive_Rating.player_id AND Advanced_Stats.team_id = Defensive_Rating.team_id
                        JOIN PlayerStatsAdvanced
                        ON Advanced_Stats.player_id = PlayerStatsAdvanced.player_id AND Advanced_Stats.team_id = PlayerStatsAdvanced.team_id
                        GROUP BY Advanced_Stats.player_id, Advanced_Stats.player_name, Advanced_Stats.team_id, Advanced_Stats.team_abbreviation, Advanced_Stats.min, 
                        Offensive_Rating.Offensive_Rating, Defensive_Rating.Defensive_Rating, Advanced_Stats.Ast_Pct, Advanced_Stats.Ast_Tov, Advanced_Stats.Ast_Ratio,
                        Advanced_Stats.Oreb_Pct, Advanced_Stats.Dreb_Pct, Advanced_Stats.Reb_Pct, Advanced_Stats.Tov_Pct, Advanced_Stats.Efg_Pct, Advanced_Stats.Ts_Pct,
                        Advanced_Stats.Usg_Pct, PlayerStatsAdvanced.Pie, PlayerStatsAdvanced.Poss
                        HAVING Advanced_Stats.min > 0
                    ";
                    Console.WriteLine(query);
                    var boxScores = await _context.BoxScoreAdvancedPlayers.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(boxScores.Count);
                    return Ok(boxScores);

                }
                else if (boxType == "FourFactors")
                {
                    query = $@"WITH " + offRatingQuery + ", " + defRatingQuery + ", " + advancedStats +
                        $@"
                        SELECT
                        PlayerStats.player_id, 
                        PlayerStats.team_id,
                        PlayerStats.team_abbreviation,
                        PlayerStats.player_name,
                        PlayerStats.min,
                        Advanced_Stats.Efg_Pct,
                        CASE
                            WHEN PlayerStats.fga IS NULL OR PlayerStats.fga = 0 THEN 0
                            ELSE ROUND(100 * (PlayerStats.fta / PlayerStats.fga), 2)
                        END AS Fta_Rate,                        
                        Advanced_Stats.Tov_Pct,
                        Advanced_Stats.Oreb_Pct
                        FROM PlayerStats
                        JOIN Advanced_Stats
                        ON PlayerStats.player_id = Advanced_Stats.player_id AND PlayerStats.team_id = Advanced_Stats.team_id
                        WHERE PlayerStats.min > 0
                        ORDER BY {sortField} {order}
                        ";

                    var boxScores = await _context.BoxScoreFourFactorsPlayers.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(boxScores.Count);
                    return Ok(boxScores);
                }
                else if (boxType == "Misc")
                {
                    var joinedTable = $"box_score_advanced_{season}";

                    if (perMode == "Totals")
                    {
                        query = $@"SELECT
                                {tableName}.player_id, {tableName}.player_name,
                                {tableName}.team_id,
                                {tableName}.team_abbreviation,
                                SUM({tableName}.min) AS min,
                                SUM({tableName}.pts_off_tov) AS pts_off_tov,
                                SUM({tableName}.pts_2nd_chance) AS pts_2nd_chance,
                                SUM({tableName}.pts_fb) AS pts_fb,
                                SUM({tableName}.pts_paint) AS pts_paint,
                                SUM({tableName}.opp_pts_off_tov) AS opp_pts_off_tov,
                                SUM({tableName}.opp_pts_2nd_chance) AS opp_pts_2nd_chance,
                                SUM({tableName}.opp_pts_fb) AS opp_pts_fb,
                                SUM({tableName}.opp_pts_paint) AS opp_pts_paint,
                                SUM({tableName}.blk) AS blk,
                                SUM({tableName}.blka) AS blka,
                                SUM({tableName}.pf) AS pf,
                                SUM({tableName}.pfd) AS pfd
                                FROM 
                                {tableName}
                                JOIN league_games_{season}
                                ON {tableName}.game_id = league_games_{season}.game_id
                                AND {tableName}.team_id = league_games_{season}.team_id
                                WHERE 
                                {tableName}.min > 0 ";
                        if (selectedOpponent != "1")
                        {
                            query += $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') ";

                        }

                        query += $@"AND {tableName}.team_id LIKE '%{selectedTeam}%' 
                            GROUP BY player_id, player_name, {tableName}.team_id, {tableName}.team_abbreviation, team_city 
                            ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per 100 Poss")
                    {
                        query = $@"SELECT
                                {tableName}.player_id, {tableName}.player_name,
                                {tableName}.team_id,
                                {tableName}.team_abbreviation,
                                100 * SUM({tableName}.min) / NULLIF(SUM(poss), 0) AS min,
                                100 * SUM({tableName}.pts_off_tov) / NULLIF(SUM(poss), 0) AS pts_off_tov,
                                100 * SUM({tableName}.pts_2nd_chance) / NULLIF(SUM(poss), 0) AS pts_2nd_chance,
                                100 * SUM({tableName}.pts_fb) / NULLIF(SUM(poss), 0) AS pts_fb,
                                100 * SUM({tableName}.pts_paint) / NULLIF(SUM(poss), 0) AS pts_paint,
                                100 * SUM({tableName}.opp_pts_off_tov) / NULLIF(SUM(poss), 0) AS opp_pts_off_tov,
                                100 * SUM({tableName}.opp_pts_2nd_chance) / NULLIF(SUM(poss), 0) AS opp_pts_2nd_chance,
                                100 * SUM({tableName}.opp_pts_fb) / NULLIF(SUM(poss), 0) AS opp_pts_fb,
                                100 * SUM({tableName}.opp_pts_paint) / NULLIF(SUM(poss), 0) AS opp_pts_paint,
                                100 * SUM({tableName}.blk) / NULLIF(SUM(poss), 0) AS blk,
                                100 * SUM({tableName}.blka) / NULLIF(SUM(poss), 0) AS blka,
                                100 * SUM({tableName}.pf) / NULLIF(SUM(poss), 0) AS pf,
                                100 * SUM({tableName}.pfd) / NULLIF(SUM(poss), 0) AS pfd
                                FROM {tableName}
                                JOIN {joinedTable}
                                ON {tableName}.player_id = {joinedTable}.player_id
                                AND {tableName}.team_id = {joinedTable}.team_id
                                AND {tableName}.game_id = {joinedTable}.game_id
                                JOIN league_games_{season}
                                ON {tableName}.game_id = league_games_{season}.game_id
                                AND {tableName}.team_id = league_games_{season}.team_id
                                WHERE {tableName}.team_id LIKE '%{selectedTeam}%' ";
                        if (selectedOpponent != "1")
                        {
                            query += $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') ";

                        }

                        query += $@"GROUP BY {tableName}.player_id, {tableName}.player_name, {tableName}.team_id, {tableName}.team_abbreviation
                                HAVING SUM({tableName}.min) > 0
                                ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per Minute" || perMode == "Per 12 Minutes" || perMode == "Per 24 Minutes")
                    {
                        var nMinutes = 1;

                        if (perMode == "Per 12 Minutes")
                        {
                            nMinutes = 12;
                        }
                        else if (perMode == "Per 24 Minutes")
                        {
                            nMinutes = 24;
                        }
                        query = $@"SELECT
                                {tableName}.player_id, {tableName}.player_name,
                                {tableName}.team_id,
                                {tableName}.team_abbreviation,
                                SUM({tableName}.min) AS min,
                                {nMinutes} * (SUM({tableName}.pts_off_tov) / NULLIF(SUM({tableName}.min), 0)) AS pts_off_tov,
                                {nMinutes} * (SUM({tableName}.pts_2nd_chance) / NULLIF(SUM({tableName}.min), 0)) AS pts_2nd_chance,
                                {nMinutes} * (SUM({tableName}.pts_fb) / NULLIF(SUM({tableName}.min), 0)) AS pts_fb,
                                {nMinutes} * (SUM({tableName}.pts_paint) / NULLIF(SUM({tableName}.min), 0)) AS pts_paint,
                                {nMinutes} * (SUM({tableName}.opp_pts_off_tov) / NULLIF(SUM({tableName}.min), 0)) AS opp_pts_off_tov,
                                {nMinutes} * (SUM({tableName}.opp_pts_2nd_chance) / NULLIF(SUM({tableName}.min), 0)) AS opp_pts_2nd_chance,
                                {nMinutes} * (SUM({tableName}.opp_pts_fb) / NULLIF(SUM({tableName}.min), 0)) AS opp_pts_fb,
                                {nMinutes} * (SUM({tableName}.opp_pts_paint) / NULLIF(SUM({tableName}.min), 0)) AS opp_pts_paint,
                                {nMinutes} * (SUM({tableName}.blk) / NULLIF(SUM({tableName}.min), 0)) AS blk,
                                {nMinutes} * (SUM({tableName}.blka) / NULLIF(SUM({tableName}.min), 0)) AS blka,
                                {nMinutes} * (SUM({tableName}.pf) / NULLIF(SUM({tableName}.min), 0)) AS pf,
                                {nMinutes} * (SUM({tableName}.pfd) / NULLIF(SUM({tableName}.min), 0)) AS pfd
                                FROM {tableName}
                                JOIN league_games_{season}
                                ON {tableName}.game_id = league_games_{season}.game_id
                                AND {tableName}.team_id = league_games_{season}.team_id
                                WHERE {tableName}.team_id LIKE '%{selectedTeam}%' ";
                        if (selectedOpponent != "1")
                        {
                            query += $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') ";

                        }
                                query += $@"GROUP BY {tableName}.player_id, {tableName}.player_name, {tableName}.team_id, {tableName}.team_abbreviation
                                HAVING SUM({tableName}.min) > 0
                                ORDER BY {sortField} {order}";
                    }
                    else if (perMode == "Per Game")
                    {
                        query = gamesPlayedQuery + $@"SELECT
                                {tableName}.player_id, {tableName}.player_name,
                                {tableName}.team_id,
                                {tableName}.team_abbreviation,
                                SUM({tableName}.min) / GamesPlayed.gp AS min,
                                SUM({tableName}.pts_off_tov) / GamesPlayed.gp AS pts_off_tov,
                                SUM({tableName}.pts_2nd_chance) / GamesPlayed.gp AS pts_2nd_chance,
                                SUM({tableName}.pts_fb) / GamesPlayed.gp AS pts_fb,
                                SUM({tableName}.pts_paint) / GamesPlayed.gp AS pts_paint,
                                SUM({tableName}.opp_pts_off_tov) / GamesPlayed.gp AS opp_pts_off_tov,
                                SUM({tableName}.opp_pts_2nd_chance) / GamesPlayed.gp AS opp_pts_2nd_chance,
                                SUM({tableName}.opp_pts_fb) / GamesPlayed.gp AS opp_pts_fb,
                                SUM({tableName}.opp_pts_paint) / GamesPlayed.gp AS opp_pts_paint,
                                SUM({tableName}.blk) / GamesPlayed.gp AS blk,
                                SUM({tableName}.blka) / GamesPlayed.gp AS blka,
                                SUM({tableName}.pf) / GamesPlayed.gp AS pf,
                                SUM({tableName}.pfd) / GamesPlayed.gp AS pfd
                                FROM
                                    {tableName}
                                JOIN GamesPlayed
                                    ON {tableName}.player_id = GamesPlayed.player_id
                                    AND {tableName}.team_id = GamesPlayed.team_id
                                JOIN league_games_{season}
                                    ON {tableName}.game_id = league_games_{season}.game_id
                                    AND {tableName}.team_id = league_games_{season}.team_id
                                WHERE
                                    {tableName}.min > 0
                                AND 
                                    {tableName}.team_id LIKE '%{selectedTeam}%' ";
                                if (selectedOpponent != "1")
                                {
                            query += $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') ";

                                }
                                query += $@"GROUP BY  
                                    {tableName}.player_id, {tableName}.player_name, {tableName}.team_id, {tableName}.team_abbreviation, team_city, GamesPlayed.gp
                                ORDER BY {sortField} {order}";
                    }
                    Console.WriteLine(boxType);
                    var boxScores = await _context.BoxScoreMiscPlayers.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(boxScores.Count);
                    return Ok(boxScores);
                }
                else if (boxType == "Scoring")
                {
                    Console.WriteLine("Scoring!");
                    query =
                        gamesPlayedQuery + ", " + offRatingQuery + 
                        $@"                       
                        SELECT PlayerStats.player_name, PlayerStats.player_id,
                        PlayerStats.team_id, PlayerStats.team_abbreviation,
                        PlayerStats.min / GamesPlayed.gp AS min,
                        CASE
                          WHEN PlayerStats.fga IS NULL OR PlayerStats.fga = 0 THEN 0
                          ELSE (PlayerStats.fga - PlayerStats.fg3a) / PlayerStats.fga
                        END AS pct_fga_2pt,
                        CASE
                          WHEN PlayerStats.fga IS NULL OR PlayerStats.fga = 0 THEN 0
                          ELSE PlayerStats.fg3a / PlayerStats.fga
                        END AS pct_fga_3pt,
                        CASE
                          WHEN PlayerStats.pts IS NULL OR PlayerStats.pts = 0 THEN 0
                          ELSE ((PlayerStats.fgm - PlayerStats.fg3m) * 2) / PlayerStats.pts
                        END AS pct_pts_2pt,
                        CASE
                          WHEN PlayerStats.pts IS NULL OR PlayerStats.pts = 0 THEN 0
                          ELSE (PlayerStats.fg3m * 3) / PlayerStats.pts
                        END AS pct_pts_3pt,
                        CASE
                          WHEN PlayerStats.pts IS NULL OR PlayerStats.pts = 0 THEN 0
                          ELSE SUM(box_score_scoring_{season}.pct_pts_2pt_mr * box_score_traditional_{season}.pts) / PlayerStats.pts
                        END AS pct_pts_2pt_mr,
                        CASE
                          WHEN PlayerStats.pts IS NULL OR PlayerStats.pts = 0 THEN 0
                          ELSE SUM(box_score_scoring_{season}.pct_pts_fb * box_score_traditional_{season}.pts) / PlayerStats.pts
                        END AS pct_pts_fb,
                        CASE
                          WHEN PlayerStats.pts IS NULL OR PlayerStats.pts = 0 THEN 0
                          ELSE PlayerStats.ftm / PlayerStats.pts
                        END AS pct_pts_ft,
                        CASE
                          WHEN PlayerStats.pts IS NULL OR PlayerStats.pts = 0 THEN 0
                          ELSE SUM(box_score_scoring_{season}.pct_pts_off_tov * box_score_traditional_{season}.pts) / PlayerStats.pts
                        END AS pct_pts_off_tov,
                        CASE
                          WHEN PlayerStats.pts IS NULL OR PlayerStats.pts = 0 THEN 0
                          ELSE SUM(box_score_scoring_{season}.pct_pts_paint * box_score_traditional_{season}.pts) / PlayerStats.pts
                        END AS pct_pts_paint,
                        CASE
                          WHEN SUM(box_score_traditional_{season}.fgm - box_score_traditional_{season}.fg3m) IS NULL OR SUM(box_score_traditional_{season}.fgm - box_score_traditional_{season}.fg3m) = 0 THEN 0
                          ELSE SUM(box_score_scoring_{season}.pct_ast_2pm * (box_score_traditional_{season}.fgm - box_score_traditional_{season}.fg3m)) / SUM(box_score_traditional_{season}.fgm - box_score_traditional_{season}.fg3m)
                        END AS pct_ast_2pm,
                        CASE
                          WHEN SUM(box_score_traditional_{season}.fgm - box_score_traditional_{season}.fg3m) IS NULL OR SUM(box_score_traditional_{season}.fgm - box_score_traditional_{season}.fg3m) = 0 THEN 0
                          ELSE SUM(box_score_scoring_{season}.pct_uast_2pm * (box_score_traditional_{season}.fgm - box_score_traditional_{season}.fg3m)) / SUM(box_score_traditional_{season}.fgm - box_score_traditional_{season}.fg3m)
                        END AS pct_uast_2pm,
                        CASE
                          WHEN SUM(box_score_traditional_{season}.fg3m) IS NULL OR SUM(box_score_traditional_{season}.fg3m) = 0 THEN 0
                          ELSE SUM(box_score_scoring_{season}.pct_ast_3pm * box_score_traditional_{season}.fg3m) / SUM(box_score_traditional_{season}.fg3m)
                        END AS pct_ast_3pm,
                        CASE
                          WHEN SUM(box_score_traditional_{season}.fg3m) IS NULL OR SUM(box_score_traditional_{season}.fg3m) = 0 THEN 0
                          ELSE SUM(box_score_scoring_{season}.pct_uast_3pm * box_score_traditional_{season}.fg3m) / SUM(box_score_traditional_{season}.fg3m)
                        END AS pct_uast_3pm,
                        CASE
                          WHEN SUM(box_score_traditional_{season}.fgm) IS NULL OR SUM(box_score_traditional_{season}.fgm) = 0 THEN 0
                          ELSE SUM(box_score_scoring_{season}.pct_ast_fgm * box_score_traditional_{season}.fgm) / SUM(box_score_traditional_{season}.fgm)
                        END AS pct_ast_fgm,
                        CASE
                          WHEN SUM(box_score_traditional_{season}.fgm) IS NULL OR SUM(box_score_traditional_{season}.fgm) = 0 THEN 0
                          ELSE SUM(box_score_scoring_{season}.pct_uast_fgm * box_score_traditional_{season}.fgm) / SUM(box_score_traditional_{season}.fgm)
                        END AS pct_uast_fgm
                        FROM PlayerStats
                        JOIN box_score_scoring_{season}
                        ON PlayerStats.player_id = box_score_scoring_{season}.player_id
                        AND PlayerStats.team_id = box_score_scoring_{season}.team_id
                        JOIN box_score_traditional_{season}
                        ON box_score_scoring_{season}.player_id = box_score_traditional_{season}.player_id
                        AND box_score_scoring_{season}.team_id = box_score_traditional_{season}.team_id
                        AND box_score_scoring_{season}.game_id = box_score_traditional_{season}.game_id
                        JOIN league_games_{season}
                        ON box_score_scoring_{season}.game_id = league_games_{season}.game_id
                        AND box_score_scoring_{season}.team_id = league_games_{season}.team_id
                        JOIN GamesPlayed
                        ON {tableName}.player_id = GamesPlayed.player_id
                        AND PlayerStats.team_id = GamesPlayed.team_id
                        WHERE PlayerStats.team_id LIKE '%{selectedTeam}%' ";
                if (selectedOpponent != "1")
                {
                    query +=
                        $@"AND (league_games_{season}.matchup LIKE '%vs. {selectedOpponent}%' OR league_games_{season}.matchup LIKE '%@ {selectedOpponent}%') ";

                }
                     query += $@"GROUP BY PlayerStats.player_name, PlayerStats.player_id, PlayerStats.team_id, PlayerStats.team_abbreviation, PlayerStats.min, GamesPlayed.gp, PlayerStats.fga,
                        PlayerStats.fg3a, PlayerStats.fgm, PlayerStats.fg3m, PlayerStats.pts, PlayerStats.ftm
                        HAVING SUM(box_score_scoring_{season}.min) > 0
                        ORDER BY {sortField} {order}
                    ";
                    var boxScores = await _context.BoxScoreScoringPlayers.FromSqlRaw(query).ToListAsync();
                    Console.WriteLine(boxScores.Count);
                    return Ok(boxScores);
                }
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
