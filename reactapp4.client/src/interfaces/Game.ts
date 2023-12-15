interface Game {
    season_id: string;
    team_id: string;
    team_name: string;
    team_abbreviation: string;
    game_id: string;
    game_date: string;
    matchup: string;
    wl: string;
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
    video_available: string;
}

export type { Game }