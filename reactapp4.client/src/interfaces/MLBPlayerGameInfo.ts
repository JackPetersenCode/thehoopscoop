export interface MLBPlayerGameInfo {
    gamePk: number;
    teamSide: string;
    teamName: string;
    playerId: string;
    personId: number;
    fullName: string;
    boxscoreName: string;
    jerseyNumber: string;
    position: string;
    positionAbbr: string;
    statusCode: string;
    statusDescription: string;
    isCurrentBatter: boolean;
    isCurrentPitcher: boolean;
    isOnBench: boolean;
    isSubstitute: boolean;
  }
  