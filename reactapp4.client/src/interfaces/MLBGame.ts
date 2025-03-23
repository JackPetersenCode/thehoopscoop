export interface MLBGame {
    gamePk: number;
    gameGuid: string;
    link: string;  // ✅ Added missing field
    gameType: string;
    season: string;
    gameDate: string;  // Using string for ISO date format
    officialDate: string;  // Using string for ISO date format
    abstractGameState: string;
    codedGameState: string;
    detailedState: string;
    statusCode: string;
    startTimeTbd: boolean;
    abstractGameCode: string;
    awayTeamId: number;
    awayTeamName: string;
    awayScore: number | null;
    awayWins: number;
    awayLosses: number;
    awayWinPct: number;
    awayIsWinner: boolean;
    homeTeamId: number;
    homeTeamName: string;
    homeScore: number | null;
    homeWins: number;
    homeLosses: number;
    homeWinPct: number;
    homeIsWinner: boolean;
    venueId: number;
    venueName: string;
    isTie: boolean | null;
    gameNumber: number;
    doubleHeader: string;
    dayNight: string;
    description: string;
    scheduledInnings: number;
    gamesInSeries: number;
    seriesGameNumber: number;
    seriesDescription: string;
    ifNecessary: string;
    ifNecessaryDesc: string;
}
