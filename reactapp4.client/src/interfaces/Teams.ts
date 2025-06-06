interface NBATeam {
    team_id: string;
    team_name: string;
    team_abbreviation: string;
}

interface MLBTeam {
    team_id: string;
    team_name: string;
}

const MLBTeamIds: Record<number, string> = {
  108: "Angels",
  117: "Astros",
  133: "Athletics",
  141: "Blue Jays",
  144: "Braves",
  158: "Brewers",
  138: "Cardinals",
  112: "Cubs",
  109: "D-backs",
  119: "Dodgers",
  137: "Giants",
  114: "Guardians",
  136: "Mariners",
  146: "Marlins",
  121: "Mets",
  120: "Nationals",
  110: "Orioles",
  135: "Padres",
  143: "Phillies",
  134: "Pirates",
  140: "Rangers",
  139: "Rays",
  111: "Red Sox",
  113: "Reds",
  115: "Rockies",
  118: "Royals",
  116: "Tigers",
  142: "Twins",
  145: "White Sox",
  147: "Yankees"
};




export type { NBATeam, MLBTeam }
export { MLBTeamIds }