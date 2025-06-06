import {
    TOR, DEN, HOU, IND, CHI, GSW, BOS, LAC, POR, ATL, CLE, DAL, NOP, SAC, MIL, WAS, BKN, LAL,
    SAS, OKC, CHA, MIN, PHX, MEM, NYK, PHI, ORL, MIA, UTA, DET
} from 'react-nba-logos';

type NBALogoType = typeof TOR | typeof DEN | typeof HOU | typeof IND | typeof CHI | typeof GSW | typeof BOS | typeof LAC | typeof POR | typeof ATL | typeof CLE | typeof DAL | typeof NOP | typeof SAC | typeof MIL | typeof WAS | typeof BKN | typeof LAL | typeof SAS | typeof OKC | typeof CHA | typeof MIN | typeof PHX | typeof MEM | typeof NYK | typeof PHI | typeof ORL | typeof MIA | typeof UTA | typeof DET;


interface RosterPlayer {
    player_id: string;
    player_name: string;
}

interface UpcomingPostObject {
        game_date: string,
        matchup: string,
        home_team: string,
        home_team_id: string,
        home_expected: number,
        visitor_team: string,
        visitor_team_id: string,
        visitor_expected: number,
        home_odds: string,
        visitor_odds: string,
        HomeLogo: NBALogoType,
        VisitorLogo: NBALogoType,
        home_abbr: string,
        visitor_abbr: string
}

interface Teams {
    [teamName: string]: string;
}

interface TeamIds {
    [teamName: string]: string;
}

const teams: Teams = {
    "Denver Nuggets": "DEN",
    "Indiana Pacers": "IND",
    "Chicago Bulls": "CHI",
    "Houston Rockets": "HOU",
    "Golden State Warriors": "GSW",
    "Boston Celtics": "BOS",
    "Los Angeles Clippers": "LAC",
    "Atlanta Hawks": "ATL",
    "Cleveland Cavaliers": "CLE",
    "Portland Trail Blazers": "POR",
    "New Orleans Pelicans": "NOP",
    "Dallas Mavericks": "DAL",
    "Sacramento Kings": "SAC",
    "Milwaukee Bucks": "MIL",
    "Washington Wizards": "WAS",
    "Brooklyn Nets": "BKN",
    "Los Angeles Lakers": "LAL",
    "Toronto Raptors": "TOR",
    "San Antonio Spurs": "SAS",
    "Oklahoma City Thunder": "OKC",
    "Charlotte Hornets": "CHA",
    "Minnesota Timberwolves": "MIN",
    "Phoenix Suns": "PHX",
    "New York Knicks": "NYK",
    "Memphis Grizzlies": "MEM",
    "Philadelphia 76ers": "PHI",
    "Orlando Magic": "ORL",
    "Miami Heat": "MIA",
    "Utah Jazz": "UTA",
    "Detroit Pistons": "DET"
}

const teamIds: TeamIds = {
    "Denver Nuggets": "1610612743",
    "Indiana Pacers": "1610612754",
    "Chicago Bulls": "1610612741",
    "Houston Rockets": "1610612745",
    "Golden State Warriors": "1610612744",
    "Boston Celtics": "1610612738",
    "Los Angeles Clippers": "1610612746",
    "Atlanta Hawks": "1610612737",
    "Cleveland Cavaliers": "1610612739",
    "Portland Trail Blazers": "1610612757",
    "New Orleans Pelicans": "1610612740",
    "Dallas Mavericks": "1610612742",
    "Sacramento Kings": "1610612758",
    "Milwaukee Bucks": "1610612749",
    "Washington Wizards": "1610612764",
    "Brooklyn Nets": "1610612751",
    "Los Angeles Lakers": "1610612747",
    "Toronto Raptors": "1610612761",
    "San Antonio Spurs": "1610612759",
    "Oklahoma City Thunder": "1610612760",
    "Charlotte Hornets": "1610612766",
    "Minnesota Timberwolves": "1610612750",
    "Phoenix Suns": "1610612756",
    "New York Knicks": "1610612752",
    "Memphis Grizzlies": "1610612763",
    "Philadelphia 76ers": "1610612755",
    "Orlando Magic": "1610612753",
    "Miami Heat": "1610612748",
    "Utah Jazz": "1610612762",
    "Detroit Pistons": "1610612765"
}

interface UpcomingGame {
    game_id: string,
    commence_time: string,
    home_odds: string,
    away_odds: string,
    home_team: string,
    away_team: string
}

interface ExpectedMatchupPostObject {
    game_date: string,
    matchup: string,
    home_team: string,
    home_team_id: string,
    home_expected: number,
    visitor_team: string,
    visitor_team_id: string,
    visitor_expected: number,
    home_actual: number,
    visitor_actual: number,
    home_odds: string,
    visitor_odds: string,
    green_red: string
}

export type { RosterPlayer, UpcomingPostObject, NBALogoType, UpcomingGame, Teams, TeamIds, ExpectedMatchupPostObject };
export { teamIds, teams };