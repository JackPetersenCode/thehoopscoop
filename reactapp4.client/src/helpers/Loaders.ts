import { getJsonResponseStartup } from './GetJsonResponse';
import { postLeagueGamesBySeason, postPlayersNBA, postBoxScoresTraditionalBySeason, postLeagueDashLineups } from './PostFunctions';

const loadLeagueGamesBySeason = async () => {
    const years = ['2015_2016', '2016_2017', '2017_2018', '2018_2019', '2019_2020', '2020_2021', '2021_2022', '2022_2023', '2023_2024'];
    //let years = ['2016-2017'];
    //let tableLength = await getJsonResponseStartup(`/api/tablelength/leagueGames${years[0]}`);
    //tableLength = (tableLength[0].count)
    //console.log(tableLength)
    for (let i = 0; i < years.length; i++) {
        const tableLength = await getJsonResponseStartup(`api/tablelength/league_games_${years[i]}`);

        const gamesArray = await getJsonResponseStartup(`api/leagueGames/read/${years[i]}`);
        //console.log(gamesArray.resultSets.length)
        for (let j = 0; j < gamesArray.resultSets.length; j++) {
            for (let m = tableLength.count; m < gamesArray.resultSets[j].rowSet.length; m++) {
                console.log(m)
                //ACTIVATE CODE IF YOU NEED TO LOAD SHOTS INTO YOUR DATABASE

                const results = await postLeagueGamesBySeason(gamesArray.resultSets[j].rowSet[m], years[i]);
                console.log(gamesArray.resultSets[j].rowSet[m]);
            }
        }
    }
    console.log('FINISHED!!!!!!!!!!!!!!!!!!!!!!1');
}


const loadPlayers = async () => {
    const players = await getJsonResponseStartup('/api/players/read');
    console.log(players);


    for (let i = 0; i < players.length; i++) {
        console.log(players[i])
        console.log(typeof players[i].is_active)
        console.log(typeof players[i].full_name)

        const player = {
            full_name: players[i].full_name,
            first_name: players[i].first_name,
            last_name: players[i].last_name,
            is_active: players[i].is_active,
            player_id: players[i].id.toString()
        }
        console.log(player);
        let results = await postPlayersNBA(player);
    }
    console.log('FINISHED!');
}

function minutesToDecimal(minutesString: string) {
    // Split the input string into minutes and seconds
    const [minutes, seconds] = minutesString.split(':').map(Number);

    // Calculate the decimal value
    const decimalValue = minutes + seconds / 60;
    console.log(decimalValue)
    // Return the decimal value rounded to 2 decimal places
    return decimalValue.toFixed(2);
}

const loadBoxScoresTraditional = async () => {
    const season = "2022_2023";
    const tablelength = await getJsonResponseStartup(`/api/tablelength/box_score_traditional_${season}`)
    //tablelength = tablelength[0].count
    const data = await getJsonResponseStartup(`/api/BoxScoreTraditional/read/${season}`);
    for (let i = tablelength.count; i < data.length; i++) {
        if (data[i].MIN === 'MIN') {
            continue;
        }
        const mins = minutesToDecimal(data[i].MIN)

        const boxScore = {
            game_id: data[i].GAME_ID,
            team_id: data[i].TEAM_ID,
            team_abbreviation: data[i].TEAM_ABBREVIATION,
            team_city: data[i].TEAM_CITY,
            player_id: data[i].PLAYER_ID,
            player_name: data[i].PLAYER_NAME,
            nickname: data[i].NICKNAME,
            start_position: data[i].START_POSITION,
            comment: data[i].COMMENT,
            min: parseFloat(mins),
            fgm: parseFloat(data[i].FGM),
            fga: parseFloat(data[i].FGA),
            fg_pct: parseFloat(data[i].FG_PCT),
            fg3m: parseFloat(data[i].FG3M),
            fg3a: parseFloat(data[i].FG3A),
            fg3_pct: parseFloat(data[i].FG3_PCT),
            ftm: parseFloat(data[i].FTM),
            fta: parseFloat(data[i].FTA),
            ft_pct: parseFloat(data[i].FT_PCT),
            oreb: parseFloat(data[i].OREB),
            dreb: parseFloat(data[i].DREB),
            reb: parseFloat(data[i].REB),
            ast: parseFloat(data[i].AST),
            stl: parseFloat(data[i].STL),
            blk: parseFloat(data[i].BLK),
            tov: parseFloat(data[i].TO),
            pf: parseFloat(data[i].PF),
            pts: parseFloat(data[i].PTS),
            plus_minus: parseFloat(data[i].PLUS_MINUS)
        }

        for (const key in boxScore) {
            if (Object.prototype.hasOwnProperty.call(boxScore, key)) {
                if (boxScore[key as keyof typeof boxScore] === "") {
                    boxScore[key as keyof typeof boxScore] = null;
                }
            }
        }
        console.log(boxScore)

        await postBoxScoresTraditionalBySeason(boxScore, season);
    }
    //let data = await getJsonResponseStartup(`/boxScoresTraditional/read/${season}`);

    //for (let i = 0; i < data.length; i++) {
    //    await postBoxScoresTraditionalBySeason(data[i], season);
    //} 
}

const loadUpLeagueDashLineupsFunction = async (season: string, boxType: string, numPlayers: string) => {
    const results = await getJsonResponseStartup(`/api/LeagueDashLineups/read/${season}/${boxType}/${numPlayers}`);
    console.log(results);
    console.log(results.resultSets[0].rowSet.length)
    console.log(results.resultSets[0].rowSet)

    for (let i = 0; i < results.resultSets[0].rowSet.length; i++) {
        console.log(results.resultSets[0].rowSet[i])
        console.log(typeof results.resultSets[0].rowSet[i])

        const postedResults = await postLeagueDashLineups(results.resultSets[0].rowSet[i], season, boxType, numPlayers);
        console.log(postedResults);
    }
}


export { loadLeagueGamesBySeason, loadPlayers, loadBoxScoresTraditional, loadUpLeagueDashLineupsFunction }