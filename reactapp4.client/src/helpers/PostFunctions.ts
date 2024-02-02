import { BoxScoreTraditional, BoxScoreAdvanced } from "../interfaces/BoxScoreTraditional";
import { Player } from "../interfaces/Player";

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

export { postLeagueGamesBySeason, postPlayersNBA, postBoxScoresTraditionalBySeason, postBoxScoresAdvancedBySeason, postLeagueDashLineups }