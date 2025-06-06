interface BoxScoreSummary {
    game_date_est: string;
    game_sequence: string;
    game_id: string;
    game_status_id: string;
    game_status_text: string;
    gamecode: string;
    home_team_id: string;
    visitor_team_id: string;
    season: string;
    live_period: string;
    live_pc_time: string;
    natl_tv_broadcaster_abbreviation: string;
    live_period_time_bcast: string;
    wh_status: string;
}

export type { BoxScoreSummary };