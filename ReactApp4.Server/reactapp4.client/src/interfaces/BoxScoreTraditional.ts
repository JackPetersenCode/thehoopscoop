interface BoxScoreTraditional {
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
    fgm: number;
    fga: number;
    fg_pct: number;
    fg3m: number;
    fg3a: number;
    fg3_pct: number;
    ftm: number;
    fta: number;
    ft_pct: number;
    oreb: number;
    dreb: number;
    reb: number;
    ast: number;
    stl: number;
    blk: number;
    tov: number;
    pf: number;
    pts: number;
    plus_minus: number;
}

export type { BoxScoreTraditional }