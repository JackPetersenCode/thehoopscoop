// models/playbyplay.ts

export interface Play {
    game_pk: number;
    at_bat_index: number | null;
    play_end_time: string;
    
    result_type: string | null;
    result_event: string | null;
    result_event_type: string | null;
    result_description: string | null;
    result_rbi: number | null;
    result_away_score: number | null;
    result_home_score: number | null;
    result_is_out: boolean;
    
    about_at_bat_index: number | null;
    about_half_inning: string | null;
    about_is_top_inning: boolean | null;
    about_inning: number | null;
    about_start_time: string | null;
    about_end_time: string | null;
    about_is_complete: boolean | null;
    about_is_scoring_play: boolean | null;
    about_has_review: boolean | null;
    about_has_out: boolean | null;
    about_captivating_index: number | null;
    
    count_balls: number | null;
    count_strikes: number | null;
    count_outs: number | null;
    
    matchup_batter_id: number | null;
    matchup_batter_full_name: string | null;
    matchup_bat_side_code: string | null;
    matchup_bat_side_description: string | null;
    matchup_pitcher_id: number | null;
    matchup_pitcher_full_name: string | null;
    matchup_pitch_hand_code: string | null;
    matchup_pitch_hand_description: string | null;
    matchup_splits_batter: string | null;
    matchup_splits_pitcher: string | null;
    matchup_splits_men_on_base: string | null;
    matchup_post_on_first_id: number | null;
    matchup_post_on_first_full_name: string | null;
    
    matchup_batter_hot_cold_zones: string | null;
    matchup_pitcher_hot_cold_zones: string | null;
    
    pitch_index: string | null;
    action_index: string | null;
    runner_index: string | null;
}

export interface PlayPlayEvents {
    game_pk: number;
    at_bat_index: number | null;

    play_events_details_call_code: string | null;
    play_events_details_call_description: string | null;
    play_events_details_description: string | null;
    play_events_details_code: string | null;
    play_events_details_ball_color: string | null;
    play_events_details_trail_color: string | null;
    play_events_details_is_in_play: boolean;
    play_events_details_is_strike: boolean;
    play_events_details_is_ball: boolean;
    play_events_details_type_code: string | null;
    play_events_details_type_description: string | null;
    play_events_details_is_out: boolean;
    play_events_details_has_review: boolean;

    play_events_count_balls: number | null;
    play_events_count_strikes: number | null;
    play_events_count_outs: number | null;

    play_events_pitch_data_start_speed: number | null;
    play_events_pitch_data_end_speed: number | null;
    play_events_pitch_data_strike_zone_top: number | null;
    play_events_pitch_data_strike_zone_bottom: number | null;

    play_events_pitch_data_coordinates_aY: number | null;
    play_events_pitch_data_coordinates_aZ: number | null;
    play_events_pitch_data_coordinates_pfxX: number | null;
    play_events_pitch_data_coordinates_pfxZ: number | null;
    play_events_pitch_data_coordinates_pX: number | null;
    play_events_pitch_data_coordinates_pZ: number | null;
    play_events_pitch_data_coordinates_vX0: number | null;
    play_events_pitch_data_coordinates_vY0: number | null;
    play_events_pitch_data_coordinates_vZ0: number | null;
    play_events_pitch_data_coordinates_x: number | null;
    play_events_pitch_data_coordinates_y: number | null;
    play_events_pitch_data_coordinates_x0: number | null;
    play_events_pitch_data_coordinates_y0: number | null;
    play_events_pitch_data_coordinates_z0: number | null;
    play_events_pitch_data_coordinates_aX: number | null;

    play_events_pitch_data_breaks_break_angle: number | null;
    play_events_pitch_data_breaks_break_length: number | null;
    play_events_pitch_data_breaks_break_y: number | null;
    play_events_pitch_data_breaks_break_vertical: number | null;
    play_events_pitch_data_breaks_break_vertical_induced: number | null;
    play_events_pitch_data_breaks_break_horizontal: number | null;
    play_events_pitch_data_breaks_spin_rate: number | null;
    play_events_pitch_data_breaks_spin_direction: number | null;

    play_events_pitch_data_zone: number | null;
    play_events_pitch_data_type_confidence: number | null;
    play_events_pitch_data_plate_time: number | null;
    play_events_pitch_data_extension: number | null;

    play_events_hit_data_launch_speed: number | null;
    play_events_hit_data_launch_angle: number | null;
    play_events_hit_data_total_distance: number | null;
    play_events_hit_data_trajectory: string | null;
    play_events_hit_data_hardness: string | null;
    play_events_hit_data_location: string | null;
    play_events_hit_data_coordinates_coordX: number | null;
    play_events_hit_data_coordinates_coordY: number | null;

    play_events_index: number | null;
    play_events_play_id: string | null;
    play_events_pitch_number: number | null;
    play_events_start_time: string | null;
    play_events_end_time: string | null;
    play_events_is_pitch: boolean | null;
    play_events_type: string | null;
}

export interface PlayRunners {
    game_pk: number;
    at_bat_index: number | null;

    runners_movement_origin_base: string | null;
    runners_movement_start: string | null;
    runners_movement_end: string | null;
    runners_movement_out_base: string | null;
    runners_movement_is_out: boolean;
    runners_movement_out_number: number | null;

    runners_details_event: string | null;
    runners_details_event_type: string | null;
    runners_details_movement_reason: string | null;
    runners_details_player_id: number | null;
    runners_details_player_full_name: string | null;
    runners_details_responsible_pitcher_id: string | null;
    runners_details_is_scoring_event: boolean;
    runners_details_rbi: boolean;
    runners_details_earned: boolean;
    runners_details_team_unearned: boolean;
    runners_details_play_index: number | null;
    runners_credits: string | null;
}

export interface PlayRunnersCredits {
    game_pk: number;
    at_bat_index: number | null;
    runners_details_play_index: number | null;
    runners_details_player_id: number | null;
    runners_credits_player_id: number | null;
    runners_credits_position_code: string | null;
    runners_credits_position_name: string | null;
    runners_credits_position_type: string | null;
    runners_credits_position_abbreviation: string | null;
    runners_credits_credit: string | null;
}
