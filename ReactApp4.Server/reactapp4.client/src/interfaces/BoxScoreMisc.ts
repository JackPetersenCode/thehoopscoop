interface BoxScoreMisc {
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
    pts_off_tov: number | null;
    pts_2nd_chance: number | null;
    pts_fb: number | null;
    pts_paint: number | null;
    opp_pts_off_tov: number | null;
    opp_pts_2nd_chance: number | null;
    opp_pts_fb: number | null;
    opp_pts_paint: number | null;
    blk: number | null;
    blka: number | null;
    pf: number | null;
    pfd: number | null;
    // Add the remaining properties as needed
}

export type { BoxScoreMisc };
