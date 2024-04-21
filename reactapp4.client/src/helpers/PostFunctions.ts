import { BoxScoreTraditional } from "../interfaces/BoxScoreTraditional";
import { BoxScoreAdvanced } from "../interfaces/BoxScoreAdvanced";
import { BoxScoreFourFactors } from "../interfaces/BoxScoreFourFactors";
import { BoxScoreMisc } from "../interfaces/BoxScoreMisc";
import { BoxScoreScoring } from "../interfaces/BoxScoreScoring";
import { Player } from "../interfaces/Player";
import { Shot } from "../interfaces/Shot";
import { BoxScoreSummary } from "../interfaces/BoxScoreSummary";

const postLeagueGamesBySeason = async (obj: [], season: string) => {
    console.log(season);
    const url = `/api/leagueGames`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
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
                'Content-Type': 'application/json'
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
                'Content-Type': 'application/json'
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
                'Content-Type': 'application/json'
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
                'Content-Type': 'application/json'
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
                'Content-Type': 'application/json'
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
    console.log(season);
    const url = `/api/BoxScoreScoring/${season}`;
    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
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
                'Content-Type': 'application/json'
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
                'Content-Type': 'application/json'
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
                'Content-Type': 'application/json'
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
                'Content-Type': 'application/json'
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
    postBoxScoreSummary
}