export interface SportRadarMLBPBPAtBat {
  game_id: string | null;                // uuid
  inning: number | null;
  half: string | null;                   // "T"/"B"
  hitter_id: string | null;              // uuid
  at_bat_id: string | null;              // uuid
  hitter_hand: string | null;            // "L"/"R"/"S"
  pitcher_id: string | null;             // uuid
  pitcher_hand: string | null;           // "L"/"R"/"S"
  sequence_number: number | null;
  description: string | null;

  hitter_preferred_name: string | null;
  hitter_first_name: string | null;
  hitter_last_name: string | null;
  hitter_jersey_number: number | null;
  hitter_full_name: string | null;

  pitcher_preferred_name: string | null;
  pitcher_first_name: string | null;
  pitcher_last_name: string | null;
  pitcher_jersey_number: number | null;
  pitcher_full_name: string | null;

  home_team_runs: number | null;
  away_team_runs: number | null;
}
