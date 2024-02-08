WITH PlayerStats AS (	
  SELECT player_id,
  team_id,
	team_abbreviation,
  SUM(ast) AS ast,
  SUM(fgm) AS fgm,
  SUM(fg3m) AS fg3m,
  SUM(pts) AS pts,
  SUM(ftm) AS ftm,
  SUM(fga) AS fga,
  SUM(oreb) AS orb,
  SUM(min) AS min
  FROM "box_score_traditional_2015_16"
  GROUP BY player_id, team_id, team_abbreviation  
),

TeamStats AS (
	SELECT team_id,
  SUM(fgm) AS fgm,
  SUM(fga) AS fga,
  SUM(fg3a) AS fg3a,
  SUM(fg3m) AS fg3m,
  SUM(ftm) AS ftm,
  SUM(fta) AS fta,
  SUM(tov) AS tov,
  SUM(oreb) AS orb,
  SUM(pts) AS pts,
  SUM(min) AS min,
  SUM(ast) AS ast
  FROM "box_score_traditional_2015_16"
  GROUP BY team_id
),
  
Opponent_RB AS (
	SELECT 
  	lg.team_id,
    lg.team_abbreviation,
    SUM(lg.reb) AS Opponent_TRB, 
    SUM(lg.oreb) AS Opponent_ORB 
	FROM 
    "league_games_2015_16" lg
	JOIN (
    SELECT DISTINCT team_abbreviation 
    FROM "league_games_2015_16"
	) AS teams ON lg.matchup LIKE '%vs. ' || teams.team_abbreviation || '%'
					 	 OR lg.matchup LIKE '%@ ' || teams.team_abbreviation || '%'
	GROUP BY 
    lg.team_abbreviation, lg.team_id
),
Team_Scoring_Poss AS (
  SELECT
  PlayerStats.team_id,
	TeamStats.fgm + (1 - (1 - (TeamStats.ftm / TeamStats.fta))^2) * TeamStats.fta * 0.4 AS Team_Scoring_Poss
	FROM PlayerStats
  JOIN TeamStats
  ON PlayerStats.team_id = TeamStats.team_id
  
),
Team_Play_PCT AS (
  SELECT Team_Scoring_Poss.team_id,
  Team_Scoring_Poss.Team_Scoring_Poss / (TeamStats.fga + TeamStats.fta * 0.4 + TeamStats.tov) AS Team_Play_PCT
	FROM Team_Scoring_Poss
  JOIN TeamStats
  ON Team_Scoring_Poss.team_id = TeamStats.team_id
),
Team_ORB_PCT AS (
  SELECT TeamStats.team_id,
  TeamStats.orb / (TeamStats.orb + (Opponent_RB.Opponent_TRB - Opponent_RB.Opponent_ORB)) AS Team_ORB_PCT
  FROM TeamStats
  JOIN Opponent_RB
  ON TeamStats.team_id = Opponent_RB.team_id
),
Team_ORB_Weight AS (
  SELECT Team_ORB_PCT.team_id,
  ((1 - Team_ORB_PCT.Team_ORB_PCT) * Team_Play_PCT.Team_Play_PCT) / ((1 - Team_ORB_PCT.Team_ORB_PCT) * Team_Play_PCT.Team_Play_PCT + Team_ORB_PCT.Team_ORB_PCT * (1 - Team_Play_PCT.Team_Play_PCT)) AS Team_ORB_Weight
	FROM Team_ORB_PCT
  JOIN Team_Play_PCT
  ON Team_ORB_PCT.team_id = Team_Play_PCT.team_id
),

qAST AS (
 	SELECT
	b.player_id,
  b.team_id,
  ((PlayerStats.min / (TeamStats.min / 5)) * (1.14 * ((TeamStats.ast - PlayerStats.ast) / TeamStats.fgm))) + ((((TeamStats.ast / TeamStats.min) * PlayerStats.min * 5 - PlayerStats.ast) / ((TeamStats.fgm / TeamStats.min) * PlayerStats.min * 5 - PlayerStats.fgm)) * (1 - (PlayerStats.min / (TeamStats.min / 5)))) AS qAST
	FROM
  	"box_score_traditional_2015_16" b
  JOIN PlayerStats
  ON b.player_id = PlayerStats.player_id AND b.team_id = PlayerStats.team_id
  JOIN TeamStats
  ON b.team_id = TeamStats.team_id
  GROUP BY
  b.player_id, b.team_id, PlayerStats.min, TeamStats.min, PlayerStats.ast, TeamStats.ast, PlayerStats.fgm, TeamStats.fgm
),

PProd_ORB_Part AS (
  SELECT
  PlayerStats.player_id,
  PlayerStats.team_id,
  PlayerStats.orb * Team_ORB_Weight.Team_ORB_Weight * Team_Play_PCT.Team_Play_PCT * (TeamStats.pts / (TeamStats.fgm + (1 - (1 - (TeamStats.ftm / TeamStats.fta))^2) * 0.4 * TeamStats.fta)) AS PProd_ORB_Part
	FROM PlayerStats
  JOIN Team_ORB_Weight
  ON PlayerStats.team_id = Team_ORB_Weight.team_id
  JOIN Team_Play_PCT
  ON PlayerStats.team_id = Team_Play_PCT.team_id
  JOIN TeamStats
  ON PlayerStats.team_id = TeamStats.team_id
  GROUP BY PlayerStats.player_id, PlayerStats.team_id, PlayerStats.orb, Team_ORB_Weight.Team_ORB_Weight, Team_Play_PCT.Team_Play_PCT, TeamStats.pts, TeamStats.fgm, TeamStats.ftm, TeamStats.fta
),
PProd_FG_Part AS (
  -- Calculate PProd_FG_Part using the calculated player-level stats
  SELECT
  	PlayerStats.player_id,
  	PlayerStats.team_id,
    CASE
    	WHEN fga IS NULL OR fga = 0 THEN 0
    	ELSE 2 * (PlayerStats.fgm + 0.5 * PlayerStats.fg3m) * (1 - 0.5 * ((PlayerStats.pts - PlayerStats.ftm) / (2 * PlayerStats.fga)) * qAST.qAST) 
  	END AS PProd_FG_Part 
  FROM
    qAST
  JOIN PlayerStats ON qAST.player_id = PlayerStats.player_id AND qAST.team_id = PlayerStats.team_id
),
PProd_AST_Part AS (
  SELECT PlayerStats.player_id,
  PlayerStats.team_id,
  2 * ((TeamStats.fgm - PlayerStats.fgm + 0.5 * (TeamStats.fg3m - PlayerStats.fg3m)) / (TeamStats.fgm - PlayerStats.fgm)) * 0.5 * (((TeamStats.pts - TeamStats.ftm) - (PlayerStats.pts - PlayerStats.ftm)) / (2 * (TeamStats.fga - PlayerStats.fga))) * PlayerStats.ast AS PProd_AST_Part
	FROM PlayerStats
  JOIN TeamStats
  ON PlayerStats.team_id = TeamStats.team_id
),
Parts AS (
	SELECT PProd_FG_Part.player_id, PProd_FG_Part.team_id, PProd_FG_Part.PProd_FG_Part AS FG_Part,
				PProd_AST_Part.PProd_AST_Part AS AST_Part,
        PProd_ORB_Part.PProd_ORB_Part AS ORB_Part
	From PProd_FG_Part
	join PProd_AST_Part
	on PProd_FG_Part.player_id = PProd_AST_Part.player_id AND PProd_FG_Part.team_id = PProd_AST_Part.team_id
	join PProd_ORB_Part
	on PProd_FG_Part.player_id = PProd_ORB_Part.player_id AND PProd_FG_Part.team_id = PProd_ORB_Part.team_id
)
SELECT Parts.player_id, Parts.team_id, (Parts.FG_Part + Parts.AST_Part + PlayerStats.FTM) * (1 - (TeamStats.orb / Team_Scoring_Poss.Team_Scoring_Poss) * Team_ORB_Weight.Team_ORB_Weight * Team_Play_PCT.Team_Play_PCT) + Parts.ORB_Part AS PProd
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