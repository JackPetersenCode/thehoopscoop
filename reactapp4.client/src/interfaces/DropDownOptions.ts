import { NBATeam } from "./Teams";

const lineupPlayerOptions = [
    'Lineups',
    'Players'
];

const statOptionsLineups = [
    'Traditional',
    'Advanced',
    'FourFactors',
    'Misc',
    'Scoring',
    'Opponent'
];

const statOptionsPlayers = [
    'Traditional',
    'Advanced',
    'FourFactors',
    'Misc',
    'Scoring',
];

const perModeOptions = [
    'Per Game',
    'Per 100 Poss',
    'Per Minute',
    'Per 12 Minutes',
    'Per 24 Minutes',
    'Totals'
];

const numPlayersOptions = [
    '2',
    '3',
    '4',
    '5'
];

const homeOrVisitorOptions = [
    'All Games',
    'Home',
    'Visitor'
];

const nbaTeams: NBATeam[] = [
    { team_id: "1", team_name: "All Teams", team_abbreviation: '' },
    { team_id: "1610612744", team_name: "Golden State Warriors", team_abbreviation: "GSW" },
    { team_id: "1610612757", team_name: "Portland Trail Blazers", team_abbreviation: "POR" },
    { team_id: "1610612751", team_name: "Brooklyn Nets", team_abbreviation: "BKN" },
    { team_id: "1610612748", team_name: "Miami Heat", team_abbreviation: "MIA" },
    { team_id: "1610612747", team_name: "Los Angeles Lakers", team_abbreviation: "LAL" },
    { team_id: "1610612761", team_name: "Toronto Raptors", team_abbreviation: "TOR" },
    { team_id: "1610612738", team_name: "Boston Celtics", team_abbreviation: "BOS" },
    { team_id: "1610612752", team_name: "New York Knicks", team_abbreviation: "NYK" },
    { team_id: "1610612762", team_name: "Utah Jazz", team_abbreviation: "UTA" },
    { team_id: "1610612756", team_name: "Phoenix Suns", team_abbreviation: "PHX" },
    { team_id: "1610612753", team_name: "Orlando Magic", team_abbreviation: "ORL" },
    { team_id: "1610612749", team_name: "Milwaukee Bucks", team_abbreviation: "MIL" },
    { team_id: "1610612741", team_name: "Chicago Bulls", team_abbreviation: "CHI" },
    { team_id: "1610612745", team_name: "Houston Rockets", team_abbreviation: "HOU" },
    { team_id: "1610612737", team_name: "Atlanta Hawks", team_abbreviation: "ATL" },
    { team_id: "1610612754", team_name: "Indiana Pacers", team_abbreviation: "IND" },
    { team_id: "1610612759", team_name: "San Antonio Spurs", team_abbreviation: "SAS" },
    { team_id: "1610612765", team_name: "Detroit Pistons", team_abbreviation: "DET" },
    { team_id: "1610612763", team_name: "Memphis Grizzlies", team_abbreviation: "MEM" },
    { team_id: "1610612739", team_name: "Cleveland Cavaliers", team_abbreviation: "CLE" },
    { team_id: "1610612758", team_name: "Sacramento Kings", team_abbreviation: "SAC" },
    { team_id: "1610612743", team_name: "Denver Nuggets", team_abbreviation: "DEN" },
    { team_id: "1610612742", team_name: "Dallas Mavericks", team_abbreviation: "DAL" },
    { team_id: "1610612766", team_name: "Charlotte Hornets", team_abbreviation: "CHA" },
    { team_id: "1610612750", team_name: "Minnesota Timberwolves", team_abbreviation: "MIN" },
    { team_id: "1610612740", team_name: "New Orleans Pelicans", team_abbreviation: "NOP" },
    { team_id: "1610612764", team_name: "Washington Wizards", team_abbreviation: "WAS" },
    { team_id: "1610612755", team_name: "Philadelphia 76ers", team_abbreviation: "PHI" },
    { team_id: "1610612746", team_name: "Los Angeles Clippers", team_abbreviation: "LAC" },
    { team_id: "1610612760", team_name: "Oklahoma City Thunder", team_abbreviation: "OKC" },
];

export { lineupPlayerOptions, statOptionsLineups, statOptionsPlayers, perModeOptions, numPlayersOptions, homeOrVisitorOptions, nbaTeams }