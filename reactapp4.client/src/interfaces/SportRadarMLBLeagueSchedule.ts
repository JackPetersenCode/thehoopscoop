export interface SportRadarMLBLeagueSchedule {
  league_alias: string | null;
  league_name: string | null;
  league_id: string | null;              // uuid
  season_id: string | null;              // uuid
  season_year: number | null;
  season_type: string | null;
  game_id: string | null;                // uuid
  game_status: string | null;
  game_coverage: string | null;
  game_game_number: number | null;
  game_day_night: string | null;
  game_scheduled: string | null;         // ISO string with TZ
  game_home_team: string | null;         // uuid
  game_away_team: string | null;         // uuid
  game_attendance: number | null;
  game_duration: string | null;          // e.g. "2:10"
  game_double_header: boolean | null;
  game_entry_mode: string | null;
  game_reference: string | null;

  venue_name: string | null;
  venue_market: string | null;
  venue_capacity: number | null;
  venue_surface: string | null;
  venue_address: string | null;
  venue_city: string | null;
  venue_state: string | null;
  venue_zip: string | null;
  venue_country: string | null;
  venue_id: string | null;               // uuid
  venue_field_orientation: string | null;
  venue_stadium_type: string | null;
  venue_time_zone: string | null;
  venue_location_lat: number | null;
  venue_location_lng: number | null;

  home_name: string | null;
  home_market: string | null;
  home_abbr: string | null;
  home_id: string | null;                // uuid
  home_win: number | null;
  home_loss: number | null;

  away_name: string | null;
  away_market: string | null;
  away_abbr: string | null;
  away_id: string | null;                // uuid
  away_win: number | null;
  away_loss: number | null;

  broadcast_1_network: string | null;
  broadcast_1_type: string | null;
  broadcast_1_locale: string | null;
  broadcast_1_channel: string | null;
  broadcast_2_network: string | null;
  broadcast_2_type: string | null;
  broadcast_2_locale: string | null;
  broadcast_2_channel: string | null;
  broadcast_3_network: string | null;
  broadcast_3_type: string | null;
  broadcast_3_locale: string | null;
  broadcast_3_channel: string | null;

  game_rescheduled: string | null;       // ISO string with TZ (if present)
  game_parent_id: string | null;         // uuid
}
