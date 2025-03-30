import { MLBTeam } from "./Teams";

const mlbLeagueOptions = [
    "MLB",
    "American League",
    "National League"
];

const yearToDateOptions = [
    "Year To Date",
    "Last 7",
    "Last 15",
    "Last 30"
]

const mlbTeams: MLBTeam[] = [
    { team_id: "0", team_name: "All Teams" },
    { team_id: "109", team_name: "Arizona Diamondbacks" },
    { team_id: "144", team_name: "Atlanta Braves" },
    { team_id: "110", team_name: "Baltimore Orioles" },
    { team_id: "111", team_name: "Boston Red Sox" },
    { team_id: "112", team_name: "Chicago Cubs" },
    { team_id: "145", team_name: "Chicago White Sox" },
    { team_id: "113", team_name: "Cincinnati Reds" },
    { team_id: "114", team_name: "Cleveland Guardians" },
    { team_id: "115", team_name: "Colorado Rockies" },
    { team_id: "116", team_name: "Detroit Tigers" },
    { team_id: "117", team_name: "Houston Astros" },
    { team_id: "118", team_name: "Kansas City Royals" },
    { team_id: "108", team_name: "Los Angeles Angels" },
    { team_id: "119", team_name: "Los Angeles Dodgers" },
    { team_id: "146", team_name: "Miami Marlins" },
    { team_id: "158", team_name: "Milwaukee Brewers" },
    { team_id: "142", team_name: "Minnesota Twins" },
    { team_id: "121", team_name: "New York Mets" },
    { team_id: "147", team_name: "New York Yankees" },
    { team_id: "133", team_name: "Oakland Athletics" },
    { team_id: "143", team_name: "Philadelphia Phillies" },
    { team_id: "134", team_name: "Pittsburgh Pirates" },
    { team_id: "135", team_name: "San Diego Padres" },
    { team_id: "137", team_name: "San Francisco Giants" },
    { team_id: "136", team_name: "Seattle Mariners" },
    { team_id: "138", team_name: "St. Louis Cardinals" },
    { team_id: "139", team_name: "Tampa Bay Rays" },
    { team_id: "140", team_name: "Texas Rangers" },
    { team_id: "141", team_name: "Toronto Blue Jays" },
    { team_id: "120", team_name: "Washington Nationals" },
];


export { mlbLeagueOptions, yearToDateOptions, mlbTeams }