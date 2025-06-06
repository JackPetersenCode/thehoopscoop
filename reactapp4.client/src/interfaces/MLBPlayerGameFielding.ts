export interface MLBPlayerGameFielding {
    gamePk: number;
    teamSide: string;
    teamName: string;
    teamId: number;
    playerId: string;
    personId: number | null;
    caughtStealing: number | null;
    stolenBases: number | null;
    stolenBasePercentage: number | null;
    assists: number | null;
    putOuts: number | null;
    errors: number | null;
    chances: number | null;
    fielding: number | null;
    passedBall: number | null;
    pickoffs: number | null;
    gamesStarted: number | null;
}
