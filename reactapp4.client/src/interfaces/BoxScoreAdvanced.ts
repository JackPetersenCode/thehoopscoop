interface BoxScoreAdvanced {
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
    e_off_rating: number;
    off_rating: number;
    e_def_rating: number;
    def_rating: number;
    e_net_rating: number;
    net_rating: number;
    ast_pct: number;
    ast_tov: number;
    ast_ratio: number;
    oreb_pct: number;
    dreb_pct: number;
    reb_pct: number;
    tm_tov_pct: number;
    efg_pct: number;
    ts_pct: number;
    usg_pct: number;
    e_usg_pct: number;
    e_pace: number;
    pace: number;
    pace_per40: number;
    poss: number;
    pie: number;
}

export type { BoxScoreAdvanced };
