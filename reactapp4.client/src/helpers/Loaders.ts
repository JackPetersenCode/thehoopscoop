import { getJsonResponseStartup } from './GetJsonResponse';
import { postLeagueGamesBySeason, postPlayersNBA } from './PostFunctions';

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

export { loadLeagueGamesBySeason, loadPlayers }