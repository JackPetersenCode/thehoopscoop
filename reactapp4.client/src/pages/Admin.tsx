import { loadBoxScoresTraditional, loadLeagueGamesBySeason, loadPlayers, loadLeagueDashLineupsFunction, loadBoxScoresAdvanced, loadBoxScoresFourFactors, loadBoxScoresMisc, loadBoxScoresScoring, loadShotsBySeason, loadNewOddsFunction, loadBoxScoreSummary } from '../helpers/Loaders';
import { ExpectedMatchupPostObject, RosterPlayer } from '../interfaces/Gambling';
import axios from 'axios';



interface LeagueGameWithHomeVisitor {
    season_id: string,
    team_id: string,
    team_abbreviation: string,
    team_name: string,
    game_id: string,
    game_date: string,
    matchup: string,
    wl: string,
    min: number,
    fgm: number,
    fga: number,
    fg_pct: number,
    fg3m: number,
    fg3a: number,
    fg3_pct: number,
    ftm: number,
    fta: number,
    ft_pct: number,
    oreb: number,
    dreb: number,
    reb: number,
    ast: number,
    stl: number,
    blk: number,
    tov: number,
    pf: number,
    pts: number,
    plus_minus: number,
    video_available: string,
    home_team_id: string,
    visitor_team_id: string
}

function Admin() {

    const loadLineupsLoop = async() => {
        //const seasons = ['2015_16', '2016_17', '2017_18', '2018_19', '2019_20', '2020_21', '2021_22', '2022_23', '2023_24'];
        const seasons = ['2023_24'];
        const boxTypes = ['Base', 'Advanced', 'FourFactors', 'Misc', 'Scoring', 'Opponent'];
        const numPlayers = ['2', '3', '4', '5'];
        for (let j = 0; j < seasons.length; j++) {
            for (let i = 0; i < boxTypes.length; i++) {
                for (let k = 0; k < numPlayers.length; k++) {
                    await axios.delete(`/api/LeagueDashLineups/${seasons[j]}/${boxTypes[i]}/${numPlayers[k]}`)
                }
            }
        }

        for (let j = 0; j < seasons.length; j++) {
            for (let i = 0; i < boxTypes.length; i++) {
                for (let k = 0; k < numPlayers.length; k++) {
                    loadLeagueDashLineupsFunction(seasons[j], boxTypes[i], numPlayers[k]);
                }
            }
        }
    }

    //const loadUpExpectedBySeasonButton = document.getElementById("loadExpectedBySeasonButton");


    const getJsonResponseJackorithm = async (url: string) => {
        console.log(url)
        const response = await fetch(url);
        try{
            if (response.ok){
                const jsonResponse = await response.json();
                return jsonResponse;
            }
        } catch(err){
            console.log(err);
        }
    }

    const getPreviousYear = async(season: string) => {
        let split = season.split('_')
        let previous = parseInt(split[1]) - 1;
        let previous2 = parseInt(split[0]) - 1;
        let thisSeason = previous2 + '_' + previous;
        return thisSeason;
    }

    const getAverageScore = async(season: string, previousGameId: string, previousSeason: string, game_date: string) => {
        console.log(season)

        let averageScore;
        if (previousGameId !== '1') {
            averageScore = await getJsonResponseJackorithm(`/api/Gambling/averageScore/${season}/${game_date}`);
        } else {
            averageScore = await getJsonResponseJackorithm(`/api/Gambling/averageScore/${previousSeason}`);
        }
        return averageScore;
    }


    const getRoster = async (season: string, teamId: string, previousGameId: string, previousSeason: string) => {
        if (previousGameId === '1') {
            let roster = await getJsonResponseJackorithm(`/api/Gambling/getRoster/${previousSeason}/${teamId}`)
            return roster;
        } else {
            let roster = await getJsonResponseJackorithm(`/api/Gambling/getRoster/${season}/${teamId}/${previousGameId}`);
            if (!roster || roster.length === 0) {
                roster = await getJsonResponseJackorithm(`/api/Gambling/getRosterFromAdvanced/${season}/${teamId}/${previousGameId}`);
            }
            return roster;
        }

    }


    const postExpectedMatchup = async(obj: ExpectedMatchupPostObject, season: string) => {
        const url = `/api/Gambling/Jackorithm/${season}`;
        try{
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

    const fixTheName = async(name: string) => {
        let newName = name;
        if (name === 'Los Angeles Lakers') {
            newName = 'LALakers'
        } 
        else if ( name === 'LA Clippers') {
            newName = 'LAClippers';
        }
        else if (name === 'Portland Trail Blazers') {
            newName = 'Portland';
        } else {
            let namesplit = name.split(' ');
            if (namesplit.length === 3) {
                newName = namesplit[0] + namesplit[1];
            } else {
                newName = namesplit[0];
            }
        }
        return newName;
    }

    const fixTheGameDate = async(date: string) => {
        let splitDate = date.split('-');
        let fixedDate;
        if (splitDate[1].substring(0, 1) === '0') {
            fixedDate = (splitDate[1] + splitDate[2]).substring(1)
        } else {
            fixedDate = (splitDate[1] + splitDate[2]);
        }
        return fixedDate;
    }

    const getOdds = async(season: string, oddsTeam: string, fullTeam: string, oddsDate: string, fullDate: string, H_or_V: string) => {

        let moneyline = await getJsonResponseJackorithm(`/api/Gambling/moneyline/${season}/${oddsTeam}/${oddsDate}`)
        if (moneyline.length < 1) {
            if (season === '2023-2024') {
                moneyline = await getJsonResponseJackorithm(`/api/Gambling/newOdds/${season}/${fullTeam}/${fullDate}/${H_or_V}`)
                if (moneyline.length > 0) {
                    if (H_or_V === 'home') {
                        moneyline = moneyline[0].home_odds;
                    } else {
                        moneyline = moneyline[0].away_odds;
                    }
                } else {
                    moneyline = 'unavailable';
                }
            } else {
                moneyline = 'unavailable';
            }
        } else {
            moneyline = moneyline[0].ml;
        }
        return moneyline;
    }

    const getGreenRed = async(home_expected: number, visitor_expected: number, home_actual: number, visitor_actual: number) => {
        console.log(home_expected)
        console.log(visitor_expected)
        console.log(home_actual)
        console.log(visitor_actual)
        console.log(home_expected > visitor_expected)
        console.log(home_actual > visitor_actual)
        /*if (home_expected === visitor_expected) {
            return 'green';
        }*/
        if (home_expected > visitor_expected) {
            if (home_actual > visitor_actual) {
                return 'green';
            } else {
                return 'red';
            }
        } else {
            if (home_actual < visitor_actual) {
                return 'green';
            } else {
                return 'red';
            }
        }
    }

    const getPostObject = async(game: LeagueGameWithHomeVisitor, season: string, previousSeason: string) => {
        console.log(game);
        const expected = await getExpected(game, season, previousSeason);
        const home_team = await getJsonResponseJackorithm(`/api/Gambling/teamNameFromId/${game.home_team_id}`)
        const visitor_team = await getJsonResponseJackorithm(`/api/Gambling/teamNameFromId/${game.visitor_team_id}`)    

        const newHome = await fixTheName(home_team[0].team_name);
        const newVisitor = await fixTheName(visitor_team[0].team_name);

        let game_date = await fixTheGameDate(game.game_date);

        let home_odds = await getOdds(season, newHome, home_team[0].team_name, game_date, game.game_date, 'home')

        let visitor_odds = await getOdds(season, newVisitor, visitor_team[0].team_name, game_date, game.game_date, 'away')



        let green_red = await getGreenRed(parseFloat(expected[1]), parseFloat(expected[3]), game.pts, game.pts - game['plus_minus']);

        let obj: ExpectedMatchupPostObject = {
            game_date: game.game_date,
            matchup: game.matchup,
            home_team: home_team[0].team_name,
            home_team_id: game.home_team_id,
            home_expected: parseFloat(expected[1]),
            visitor_team: visitor_team[0].team_name,
            visitor_team_id: game.visitor_team_id,
            visitor_expected: parseFloat(expected[3]),
            home_actual: game.pts,
            visitor_actual: game.pts - game['plus_minus'],
            home_odds: home_odds,
            visitor_odds: visitor_odds,
            green_red: green_red
        }
        console.log(obj);
        postExpectedMatchup(obj, season)
    }

    const getExpected = async(game: LeagueGameWithHomeVisitor, season: string, previousSeason: string) => {
        const stat = "+/-";
        const homeTeamId = game.home_team_id;

        let homePrevious = await getJsonResponseJackorithm(`/api/Gambling/previousGameId/${season}/${homeTeamId}/${game.game_date}`)
        if (homePrevious.length < 1) {
            homePrevious = '1';
        } else {
            homePrevious = homePrevious[0].game_id;
        }

        const homeRoster = await getRoster(season, homeTeamId, homePrevious, previousSeason);
        const visitorTeamId = game.visitor_team_id;
        let visitorPrevious = await getJsonResponseJackorithm(`/api/Gambling/previousGameId/${season}/${visitorTeamId}/${game.game_date}`)
        if (visitorPrevious.length < 1) {
            visitorPrevious = '1';
        } else {
            visitorPrevious = visitorPrevious[0].game_id
        }

        const visitorRoster = await getRoster(season, visitorTeamId, visitorPrevious, previousSeason);
        console.log(visitorRoster);
        const homeExpected = await getExpectedFromRoster(season, 'home', homeRoster, homePrevious, stat, previousSeason, game.game_date);

        const visitorExpected = await getExpectedFromRoster(season, 'visitor', visitorRoster, visitorPrevious, stat, previousSeason, game.game_date);
        return [homeTeamId, homeExpected, visitorTeamId, visitorExpected];
    }

    const getExpectedFromRoster = async (season: string, H_or_V: string, roster: RosterPlayer[], previousGameId: string, stat: string, previousSeason: string, game_date: string) => {
        let totalMins = 0.0;
        let totalStat = 0.0;
        let averageScore = await getAverageScore(season, previousGameId, previousSeason, game_date)
        console.log(averageScore)

        for (let i = 0; i < roster.length; i++) {

            let averages;
            if (previousGameId !== '1') {
                averages = await getJsonResponseJackorithm(`/api/BoxScoreTraditional/82GameAverages/${roster[i].player_id}/${season}/${H_or_V}/${game_date}`)
                console.log(averages);
                if (averages.length > 0) {
                    totalMins += parseFloat(averages[0].min);
                    totalStat += parseFloat(averages[0][stat]);
                    console.log(totalMins);
                    console.log(totalStat);
                } else {
                    averages = await getJsonResponseJackorithm(`/api/BoxScoreTraditional/82GameAverages/${roster[i].player_id}/${previousSeason}/${H_or_V}`)
                    console.log(averages);
                    if (averages.length > 0) {
                        totalMins += parseFloat(averages[0].min);
                        totalStat += parseFloat(averages[0][stat]);
                    } else {
                        console.log('is it working?');
                        console.log(roster[i].player_id);
                        console.log(game_date)
                        console.log(season);
                        console.log(previousSeason);
                        averages = [{
                            "+/-": 0,
                            ast: 0,
                            blk: 0,
                            dreb: 0,
                            fg3_pct: 0,
                            fg3a: 0,
                            fg3m: 0,
                            fg_pct: 0,
                            fga: 0,
                            fgm: 0,
                            ft_pct: 0,
                            fta: 0,
                            ftm: 0,
                            min: 0,
                            oreb: 0,
                            pf: 0,
                            playerId: 0,
                            player_name: 'NO STATS FOR PLAYER',
                            pts: 0,
                            reb: 0,
                            stl: 0,
                            team_abbreviation: 'NO STATS FOR PLAYER',
                            team_id: 0,
                            to: 0
                        }]
                    }
                }
            } else {
                averages = await getJsonResponseJackorithm(`/api/BoxScoreTraditional/82GameAverages/${roster[i].player_id}/${previousSeason}/${H_or_V}`);
                console.log(averages);
                if (averages.length > 0) {
                    
                    totalMins += parseFloat(averages[0].min);
                    totalStat += parseFloat(averages[0][stat]);
                    
                    console.log(totalMins)
                    console.log(totalStat)

                } else {
                    console.log('previous game equals 1, player zeroed out.');
                    console.log(roster[i].player_id);
                    console.log(game_date)
                    console.log(season);
                    console.log(previousSeason);
                    averages = [{
                        "+/-": 0,
                        ast: 0,
                        blk: 0,
                        dreb: 0,
                        fg3_pct: 0,
                        fg3a: 0,
                        fg3m: 0,
                        fg_pct: 0,
                        fga: 0,
                        fgm: 0,
                        ft_pct: 0,
                        fta: 0,
                        ftm: 0,
                        min: 0,
                        oreb: 0,
                        pf: 0,
                        playerId: 0,
                        player_name: 'NO STATS FOR PLAYER',
                        pts: 0,
                        reb: 0,
                        stl: 0,
                        team_abbreviation: 'NO STATS FOR PLAYER',
                        team_id: 0,
                        to: 0
                    }]
                }
            }
        }

        console.log(averageScore[0].avg)
        console.log(totalStat)
        console.log(totalMins)
        console.log(averageScore[0].avg + (totalStat / totalMins * 240))

        return (averageScore[0].avg + (totalStat / totalMins * 240)).toFixed(0);
    }

    const getGames = async(season: string) => {
        const previousSeason = await getPreviousYear(season);
        const games = await getJsonResponseJackorithm(`/api/Gambling/leagueGamesWithHomeVisitor/${season}`)

        console.log(games);
        console.log(games.length)
        for (let i = 0; i < games.length; i++) {
            console.log(i)
            await getPostObject(games[i], season, previousSeason)
        }
    }

    const loadExpectedBySeason = async() => {
        const season = "2023_24";
        await getGames(season);
    }
    loadExpectedBySeason();

    return (
        <div>
            <button onClick={loadLeagueGamesBySeason}>League Games</button>
            <button onClick={loadPlayers}>Players</button>
            <button onClick={loadBoxScoresTraditional}>Box Score Traditional</button>
            <button onClick={loadBoxScoresAdvanced}>Box Score Advanced</button>
            <button onClick={loadBoxScoresFourFactors}>Box Score Four Factors</button>
            <button onClick={loadBoxScoresMisc}>Box Score Misc</button>
            <button onClick={loadBoxScoresScoring}>Box Score Scoring</button>
            <button onClick={loadBoxScoreSummary}>Box Score Summary</button>

            <button onClick={loadShotsBySeason}>Shots</button>

            <button onClick={loadLineupsLoop}>League Dash Lineups</button>
            <button onClick={loadNewOddsFunction}>New Odds</button>
            <button onClick={loadExpectedBySeason}>Expected Matchups</button>
        </div>
  );
}

export default Admin;