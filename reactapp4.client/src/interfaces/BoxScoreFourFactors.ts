interface BoxScoreFourFactors {
    game_id: string | null;
    team_id: string | null;
    team_abbreviation: string | null;
    team_city: string | null;
    player_id: string | null;
    player_name: string | null;
    nickname: string | null;
    start_position: string | null;
    comment: string | null;
    min: number | null;
    efg_pct: number | null;
    fta_rate: number | null;
    tm_tov_pct: number | null;
    oreb_pct: number | null;
    opp_efg_pct: number | null;
    opp_fta_rate: number | null;
    opp_tov_pct: number | null;
    opp_oreb_pct: number | null;
    // Add the remaining properties as needed
}

export type { BoxScoreFourFactors };
