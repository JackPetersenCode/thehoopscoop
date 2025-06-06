interface Shot {
    grid_type: string;
    game_id: string;
    game_event_id: string;
    player_id: string;
    player_name: string;
    team_id: string;
    team_name: string;
    period: string;
    minutes_remaining: string;
    seconds_remaining: string;
    event_type: string;
    action_type: string;
    shot_type: string;
    shot_zone_basic: string;
    shot_zone_area: string;
    shot_zone_range: string;
    shot_distance: string;
    loc_x: string;
    loc_y: string;
    shot_attempted_flag: string;
    shot_made_flag: string;
    game_date: string;
    htm: string;
    vtm: string;
}

interface ShotChartsGamesData {
    game_id: string;
    game_date: string;
    matchup: string;
}

export type { Shot, ShotChartsGamesData }
