export interface SportRadarMLBPBPPitchEvent {
  game_id: string | null;                 // uuid
  at_bat_id: string | null;               // uuid
  inning: number | null;
  half: string | null;

  hit_location: string | null;
  hit_type: string | null;
  status: string | null;
  event_id: string | null;                // uuid-ish
  outcome_id: string | null;
  created_at: string | null;              // ISO with TZ
  updated_at: string | null;              // ISO with TZ
  sequence_number: number | null;
  official: boolean | null;
  type: string | null;

  wall_clock_start_time: string | null;   // ISO with TZ
  wall_clock_end_time: string | null;     // ISO with TZ

  is_ab_over: boolean | null;
  is_bunt: boolean | null;
  is_hit: boolean | null;
  is_wild_pitch: boolean | null;
  is_passed_ball: boolean | null;
  is_double_play: boolean | null;
  is_triple_play: boolean | null;

  balls: number | null;
  strikes: number | null;
  outs: number | null;

  pitch_count: number | null;
  pitch_type: string | null;
  pitch_speed: number | null;
  pitch_zone: number | null;

  pitcher_hand: string | null;
  hitter_hand: string | null;
  pitcher_id: string | null;              // uuid
  pitch_x: number | null;
  pitch_y: number | null;

  pitcher_preferred_name: string | null;
  pitcher_first_name: string | null;
  pitcher_last_name: string | null;
  pitcher_jersey_number: number | null;
  pitcher_full_name: string | null;

  hitter_preferred_name: string | null;
  hitter_first_name: string | null;
  hitter_last_name: string | null;
  hitter_jersey_number: number | null;
  hitter_full_name: string | null;
  hitter_id: string | null;               // uuid

  home_team_runs: number | null;
  away_team_runs: number | null;

  mlb_pitch_speed: number | null;
  mlb_strike_zone_top: number | null;
  mlb_strike_zone_bottom: number | null;
  mlb_pitch_zone: number | null;
  mlb_pitch_code: string | null;
  mlb_pitch_description: string | null;
  mlb_pitch_x: number | null;
  mlb_pitch_y: number | null;
  mlb_hit_trajectory: string | null;
  mlb_hit_hardness: string | null;
  mlb_hit_x: number | null;
  mlb_hit_y: number | null;
}
