export interface OddsApiH2H {
  game_id: string;
  sport_key: string;
  sport_title: string;
  commence_time: string; // ISO datetime string
  home_team: string;
  away_team: string;
  bookmaker_key: string;
  bookmaker_title: string;
  bookmaker_last_update: string; // ISO datetime string
  market_key: string;
  market_last_update: string; // ISO datetime string
  outcome_name: string;
  outcome_price: number;
}
