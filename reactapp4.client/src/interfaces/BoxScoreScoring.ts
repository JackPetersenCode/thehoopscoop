interface BoxScoreScoring {
    game_id: string;
    team_id: string;
    team_abbreviation: string;
    team_city: string;
    player_id: string;
    player_name: string;
    nickname: string;
    start_position: string;
    comment: string;
    min: number;
    pct_fga_2pt: number;
    pct_fga_3pt: number;
    pct_pts_2pt: number;
    pct_pts_2pt_mr: number;
    pct_pts_3pt: number;
    pct_pts_fb: number;
    pct_pts_ft: number;
    pct_pts_off_tov: number;
    pct_pts_paint: number;
    pct_ast_2pm: number;
    pct_uast_2pm: number;
    pct_ast_3pm: number;
    pct_uast_3pm: number;
    pct_ast_fgm: number;
}

export type { BoxScoreScoring };
