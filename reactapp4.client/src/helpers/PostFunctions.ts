import { BoxScoreTraditional } from "../interfaces/BoxScoreTraditional";
import { BoxScoreAdvanced } from "../interfaces/BoxScoreAdvanced";
import { BoxScoreFourFactors } from "../interfaces/BoxScoreFourFactors";
import { BoxScoreMisc } from "../interfaces/BoxScoreMisc";
import { BoxScoreScoring } from "../interfaces/BoxScoreScoring";
import { Player } from "../interfaces/Player";
import { Shot } from "../interfaces/Shot";
import { BoxScoreSummary } from "../interfaces/BoxScoreSummary";
import { MLBGame } from "../interfaces/MLBGame";
import { MLBPlayerGameBatting } from "../interfaces/MLBPlayerGameBatting";
import { MLBPlayerGamePitching } from "../interfaces/MLBPlayerGamePitching";
import { MLBPlayerGameFielding } from "../interfaces/MLBPlayerGameFielding";
import { MLBActivePlayer } from "../interfaces/MLBActivePlayer";
import { MLBPlayerGameInfo } from "../interfaces/MLBPlayerGameInfo";
import { MLBTeamInfo } from "../interfaces/MLBTeamInfo";
import { Play, PlayPlayEvents, PlayRunners, PlayRunnersCredits } from "../interfaces/PlayByPlay";
import { OddsApiH2H } from "../interfaces/OddsApi";
import { SportRadarMLBEGSGameInfo } from "../interfaces/SportRadarMLBEGSGameInfo";
import { SportRadarMLBLeagueSchedule } from "../interfaces/SportRadarMLBLeagueSchedule";
import { SportRadarMLBPBPAtBat } from "../interfaces/SportRadarMLBPBPAtBat";
import { SportRadarMLBPBPPitchEvent } from "../interfaces/SportRadarMLBPBPPitchEvent";

const token = localStorage.getItem('token');

const postLeagueGamesBySeason = async (obj: [], season: string) => {
    console.log(season);
    const url = `/api/leagueGames`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),

        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log(error);
    }
}

const postPlayersNBA = async (obj: Player) => {
    console.log(obj);

    const url = '/api/players';
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj)
        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log(error);
    }
}

const postBoxScoresTraditionalBySeason = async (obj: BoxScoreTraditional, season: string) => {
    console.log(season);
    const url = `/api/BoxScoreTraditional/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log(error);
    }
}

const postBoxScoresAdvancedBySeason = async (obj: BoxScoreAdvanced, season: string) => {
    console.log(season);
    const url = `/api/BoxScoreAdvanced/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log(error);
    }
}

const postBoxScoresFourFactorsBySeason = async (obj: BoxScoreFourFactors, season: string) => {
    console.log(season);
    const url = `/api/BoxScoreFourFactors/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log(error);
    }
}

const postBoxScoresMiscBySeason = async (obj: BoxScoreMisc, season: string) => {
    console.log(season);
    const url = `/api/BoxScoreMisc/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log(error);
    }
}

const postBoxScoresScoringBySeason = async (obj: BoxScoreScoring, season: string) => {
    const url = `/api/BoxScoreScoring/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log(error);
    }
}

const postLeagueDashLineups = async (obj: Record<string, unknown>, season: string, boxType: string, numPlayers: string) => {
    console.log(obj);
    const url = `/api/LeagueDashLineups/${season}/${boxType}/${numPlayers}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log('error!');
        console.log(error);
    }
}

const postShotBySeason = async (obj: Shot, season: string) => {
    console.log(obj)
    console.log('cccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc');
    const url = `/api/Shot/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log(error);
    }
}

const postNewOdds = async (odds: object, season: string) => {

    const url = `/api/gambling/newOdds/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(odds),
        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log('error!');
        console.log(error);
    }
}

const postBoxScoreSummary = async (obj: BoxScoreSummary, season: string) => {
    console.log(obj);
    const url = `/api/BoxScoreSummary/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })
        if (response.ok) {
            const jsonResponse = response.json();
            return jsonResponse;
        }
    } catch (error) {
        console.log('error!');
        console.log(error);
    }
}


const postMLBGamesBySeason = async (obj: MLBGame, season: string) => {
    const url = `/api/MLBGame/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })
        if (response.ok) {
            const jsonResponse = await response.json();
            console.log("✅ game stats uploaded:", jsonResponse);

            return jsonResponse;
        } else {
            console.log("*************************************************************");
            console.log(obj);
        }
    } catch (error) {
        console.log(error);
    }
}

const postMLBPlayerGamesBattingBySeason = async (obj: MLBPlayerGameBatting[], season: string) => {
    const url = `/api/MLBPlayerGame/batting/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            
            console.log(errorText);
            console.log(obj);
            //throw new Error(`Upload failed: ${response.status} - ${errorText}`);
        } else {
            const result = await response.json();
            console.log("✅ Batting stats uploaded:", result);
        }
    } catch (error) {
        console.error("❌ Error uploading batting stats:", error);
    }
}


const postSportRadarMLBEGSGameInfoBySeason = async (obj: SportRadarMLBEGSGameInfo[], season: string) => {
    const url = `/api/SportRadarMLBEGS/gameInfo/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            
            console.log(errorText);
            console.log(obj);
            //throw new Error(`Upload failed: ${response.status} - ${errorText}`);
        } else {
            const result = await response.json();
            console.log("✅ Sport Radar stats uploaded:", result);
        }
    } catch (error) {
        console.error("❌ Error uploading sport radar stats:", error);
    }
}

const postSportRadarMLBLeagueScheduleBySeason = async (obj: SportRadarMLBLeagueSchedule[], season: string) => {
    const url = `/api/SportRadarMLBEGS/leagueSchedule/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            
            console.log(errorText);
            console.log(obj);
            //throw new Error(`Upload failed: ${response.status} - ${errorText}`);
        } else {
            const result = await response.json();
            console.log("✅ Sport Radar stats uploaded:", result);
        }
    } catch (error) {
        console.error("❌ Error uploading sport radar stats:", error);
    }
}

const postSportRadarMLBPBPAtBatsBySeason = async (obj: SportRadarMLBPBPAtBat[], season: string) => {
    const url = `/api/SportRadarMLBEGS/pbpAtBats/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            
            console.log(errorText);
            console.log(obj);
            //throw new Error(`Upload failed: ${response.status} - ${errorText}`);
        } else {
            const result = await response.json();
            console.log("✅ Sport Radar stats uploaded:", result);
        }
    } catch (error) {
        console.error("❌ Error uploading sport radar stats:", error);
    }
}

const postSportRadarMLBPBPPitchEventsBySeason = async (obj: SportRadarMLBPBPPitchEvent[], season: string) => {
    const url = `/api/SportRadarMLBEGS/pbpPitchEvents/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            
            console.log(errorText);
            console.log(obj);
            //throw new Error(`Upload failed: ${response.status} - ${errorText}`);
        } else {
            const result = await response.json();
            console.log("✅ Sport Radar stats uploaded:", result);
        }
    } catch (error) {
        console.error("❌ Error uploading sport radar stats:", error);
    }
}

const postMLBPlayerGamesPitchingBySeason = async (obj: MLBPlayerGamePitching[], season: string) => {
    const url = `/api/MLBPlayerGame/pitching/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (response.ok) {
            const jsonResponse = await response.json();
            console.log("✅ Batting stats uploaded:", jsonResponse);

            return jsonResponse;
        }
    } catch (error) {
        console.log(error);
    }
}

const postMLBPlayerGamesFieldingBySeason = async (obj: MLBPlayerGameFielding[], season: string) => {
    console.log(obj);
    const url = `/api/MLBPlayerGame/fielding/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            console.log(errorText);
            console.log(obj);
        } else {
            const result = await response.json();
            console.log("✅ Fielding stats uploaded:", result);
        }

    } catch (error) {
        console.error("❌ Error uploading fielding stats:", error);
    }
}

const postMLBActivePlayer = async (obj: MLBActivePlayer[], season: string) => {
    const url = `/api/MLBActivePlayer/${season}`;
    console.log(obj)
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            
            console.log(errorText);
            console.log(obj);
            //throw new Error(`Upload failed: ${response.status} - ${errorText}`);
        } else {
            console.log(obj)
            const result = await response.json();
            console.log("✅ Active player stats uploaded:", result);
        }
    } catch (error) {
        console.error("❌ Error uploading active player stats:", error);
    }
}

const postMLBPlayerGameInfoBySeason = async (obj: MLBPlayerGameInfo[], season: string) => {
    const url = `/api/MLBPlayerGame/playerGameInfo/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            console.log(errorText);
            console.log(obj);
        } else {
            const result = await response.json();
            console.log("✅ Player game info uploaded:", result);
        }

    } catch (error) {
        console.error("❌ Error uploading fielding stats:", error);
    }
}

const postMLBTeamInfoBySeason = async(data: MLBTeamInfo[], season: string) => {
    const response = await fetch(`/api/MLBTeam/teamInfo/${season}`, {
        method: "POST",
        headers: { 
            "Content-Type": "application/json",
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });

    if (!response.ok) {
        throw new Error("Failed to upload team info");
    }

    return await response.json();
}

const postMLBPlaysBySeason = async (obj: Play[], season: string) => {
    const url = `/api/MLBPlayByPlay/plays/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            console.log(errorText);
            console.log(obj);
        } else {
            const result = await response.json();
            console.log("✅ Player game info uploaded:", result);
        }

    } catch (error) {
        console.error("❌ Error uploading fielding stats:", error);
    }
}

const postMLBPlayEventsBySeason = async (obj: PlayPlayEvents[], season: string) => {
    const url = `/api/MLBPlayByPlay/playEvents/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            console.log(errorText);
            console.log(obj);
        } else {
            const result = await response.json();
            console.log("✅ Player game info uploaded:", result);
        }

    } catch (error) {
        console.error("❌ Error uploading fielding stats:", error);
    }
}

const postMLBPlayRunnersBySeason = async (obj: PlayRunners[], season: string) => {
    const url = `/api/MLBPlayByPlay/runners/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            console.log(errorText);
            console.log(obj);
        } else {
            const result = await response.json();
            console.log("✅ Player game info uploaded:", result);
        }

    } catch (error) {
        console.error("❌ Error uploading fielding stats:", error);
    }
}

const postMLBPlayRunnersCreditsBySeason = async (obj: PlayRunnersCredits[], season: string) => {
    const url = `/api/MLBPlayByPlay/credits/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })

        if (!response.ok) {
            const errorText = await response.text();
            console.log(errorText);
            console.log(obj);
        } else {
            const result = await response.json();
            console.log("✅ Player game info uploaded:", result);
        }

    } catch (error) {
        console.error("❌ Error uploading fielding stats:", error);
    }
}

const postOddsApiH2HBySeason = async (obj: OddsApiH2H[], sport: string, season: string) => {
    console.log(obj)
    const url = `/api/OddsApi/H2H/${sport}/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            mode: 'cors',
            body: JSON.stringify(obj),
        })
        console.log(response)

        if (response.ok) {
            const jsonResponse = await response.json();
            console.log("✅ Odds H2H updated:", jsonResponse);

            return jsonResponse;
        }
    } catch (error) {
        console.log(error);
    }
}

export {
    postLeagueGamesBySeason,
    postPlayersNBA,
    postBoxScoresTraditionalBySeason,
    postBoxScoresAdvancedBySeason,
    postLeagueDashLineups,
    postBoxScoresFourFactorsBySeason,
    postBoxScoresMiscBySeason,
    postBoxScoresScoringBySeason,
    postShotBySeason,
    postNewOdds,
    postBoxScoreSummary,
    postMLBGamesBySeason,
    postMLBPlayerGamesBattingBySeason,
    postMLBPlayerGamesPitchingBySeason,
    postMLBPlayerGamesFieldingBySeason,
    postMLBActivePlayer,
    postMLBPlayerGameInfoBySeason,
    postMLBTeamInfoBySeason,
    postMLBPlaysBySeason,
    postMLBPlayEventsBySeason,
    postMLBPlayRunnersBySeason,
    postMLBPlayRunnersCreditsBySeason,
    postOddsApiH2HBySeason,
    postSportRadarMLBEGSGameInfoBySeason,
    postSportRadarMLBLeagueScheduleBySeason,
    postSportRadarMLBPBPAtBatsBySeason,
    postSportRadarMLBPBPPitchEventsBySeason,
}