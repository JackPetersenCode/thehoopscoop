import { BoxScoreTraditional } from "../interfaces/BoxScoreTraditional";
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

export { postLeagueGamesBySeason, postPlayersNBA, postBoxScoresTraditionalBySeason }