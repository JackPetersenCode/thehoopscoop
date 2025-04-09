import { loadBoxScoresTraditional, loadLeagueGamesBySeason, loadPlayers, loadLeagueDashLineupsFunction, loadBoxScoresAdvanced, loadBoxScoresFourFactors, loadBoxScoresMisc, loadBoxScoresScoring, loadShotsBySeason, loadNewOddsFunction, loadBoxScoreSummary, loadMLBGames, loadMLBPlayerGamesByCategory, loadMLBActivePlayers, loadMLBPlayerGameInfoBySeason, loadMLBTeamInfoBySeason } from '../helpers/Loaders';
import { ExpectedMatchupPostObject, RosterPlayer, teamIds } from '../interfaces/Gambling';
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
        const seasons = ['2024_25'];
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
        try {
            const response = await fetch(url);
            
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
    
            const jsonResponse = await response.json();
            return jsonResponse;
        } catch (err) {
            console.error(`Error fetching ${url}:`, err);
            return null; // Ensure it returns something to avoid undefined issues
        }
    };

    const getPreviousYear = async(season: string) => {
        let split = season.split('_')
        let previous = parseInt(split[1]) - 1;
        let previous2 = parseInt(split[0]) - 1;
        let thisSeason = previous2 + '_' + previous;
        return thisSeason;
    }

    const getAverageScore = async(season: string, previousGameId: string, previousSeason: string, game_date: string) => {

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
        //console.log(moneyline)
        //console.log(oddsTeam)
        //console.log(oddsDate)
        
        if (moneyline.length < 1) {
            if (season === '2024-2025') {
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

    const getPostObject = async(game: LeagueGameWithHomeVisitor, season: string, previousSeason: string, percentageOfTotalHome: number, percentageOfTotalVisitor: number) => {
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
            home_expected: parseFloat(expected[1]) * percentageOfTotalHome,
            visitor_team: visitor_team[0].team_name,
            visitor_team_id: game.visitor_team_id,
            visitor_expected: parseFloat(expected[3]) * percentageOfTotalVisitor,
            home_actual: game.pts,
            visitor_actual: game.pts - game['plus_minus'],
            home_odds: home_odds,
            visitor_odds: visitor_odds,
            green_red: green_red
        }
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
        const homeExpected = await getExpectedFromRosterBackToBack(season, 'home', homeRoster, homePrevious, stat, previousSeason, game.game_date);

        const visitorExpected = await getExpectedFromRosterBackToBack(season, 'visitor', visitorRoster, visitorPrevious, stat, previousSeason, game.game_date);

        //const homeExpected = await getExpectedFromRoster(season, 'home', homeRoster, homePrevious, stat, previousSeason, game.game_date);

        //const visitorExpected = await getExpectedFromRoster(season, 'visitor', visitorRoster, visitorPrevious, stat, previousSeason, game.game_date);
        return [homeTeamId, homeExpected, visitorTeamId, visitorExpected];
    }

    const getExpectedFromRosterBackToBack = async (season: string, H_or_V: string, roster: RosterPlayer[], previousGameId: string, stat: string, previousSeason: string, game_date: string) => {
        let totalMins = 0.0;
        let totalStat = 0.0;
        let averageScore = await getAverageScore(season, previousGameId, previousSeason, game_date)

        for (let i = 0; i < roster.length; i++) {

            let averages;
            if (previousGameId !== '1') {
                averages = await getJsonResponseJackorithm(`/api/BoxScoreTraditional/82GameAverages/${roster[i].player_id}/${season}/${H_or_V}/${game_date}`)
                if (averages.length > 0) {
                    totalMins += parseFloat(averages[0].min);
                    totalStat += parseFloat(averages[0][stat]);
             
                } else {
                    averages = await getJsonResponseJackorithm(`/api/BoxScoreTraditional/82GameAverages/${roster[i].player_id}/${previousSeason}/${H_or_V}`)
                    if (averages.length > 0) {
                        totalMins += parseFloat(averages[0].min);
                        totalStat += parseFloat(averages[0][stat]);
                    } else {
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
                if (averages.length > 0) {
                    
                    totalMins += parseFloat(averages[0].min);
                    totalStat += parseFloat(averages[0][stat]);
                    
                } else {
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

        return (averageScore[0].avg + (totalStat / totalMins * 240)).toFixed(0);
    }

    const getExpectedFromRoster = async (season: string, H_or_V: string, roster: RosterPlayer[], previousGameId: string, stat: string, previousSeason: string, game_date: string) => {
        let totalMins = 0.0;
        let totalStat = 0.0;
        let averageScore = await getAverageScore(season, previousGameId, previousSeason, game_date)

        for (let i = 0; i < roster.length; i++) {

            let averages;
            if (previousGameId !== '1') {
                averages = await getJsonResponseJackorithm(`/api/BoxScoreTraditional/82GameAverages/${roster[i].player_id}/${season}/${H_or_V}/${game_date}`)
                if (averages.length > 0) {
                    totalMins += parseFloat(averages[0].min);
                    totalStat += parseFloat(averages[0][stat]);
             
                } else {
                    averages = await getJsonResponseJackorithm(`/api/BoxScoreTraditional/82GameAverages/${roster[i].player_id}/${previousSeason}/${H_or_V}`)
                    if (averages.length > 0) {
                        totalMins += parseFloat(averages[0].min);
                        totalStat += parseFloat(averages[0][stat]);
                    } else {
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
                if (averages.length > 0) {
                    
                    totalMins += parseFloat(averages[0].min);
                    totalStat += parseFloat(averages[0][stat]);
                    
                } else {
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

        return (averageScore[0].avg + (totalStat / totalMins * 240)).toFixed(0);
    }

    const getGames = async(season: string) => {
        const previousSeason = await getPreviousYear(season);
        const games = await getJsonResponseJackorithm(`/api/Gambling/leagueGamesWithHomeVisitor/${season}`)
        for (let i = 0; i < games.length; i++) {
        //for (let i = 0; i < games.length; i++) {
            console.log(i)
            let B2BObject = await isBackToBack(games[i], season)
            const homeTeam = games[i].home_team_id;
            const visitorTeam = games[i].visitor_team_id;
            let teamIds = [homeTeam, visitorTeam];

            //WITH back_to_back_games AS (
            //    SELECT 
            //        g1.team_id,
            //        g1.game_date AS first_game_date,
            //        g2.game_date AS second_game_date,
            //        -- Determine if the first game was home or away
            //        CASE 
            //            WHEN g1.matchup LIKE '%vs.%' THEN 'home'
            //            ELSE 'away'
            //        END AS first_game_location,
            //        -- Determine if the second game was home or away
            //        CASE 
            //            WHEN g2.matchup LIKE '%vs.%' THEN 'home'
            //            ELSE 'away'
            //        END AS second_game_location,
            //        g2.pts AS second_game_points
            //    FROM league_games_2024_25 g1
            //    JOIN league_games_2024_25 g2 
            //        ON g1.team_id = g2.team_id
            //        AND CAST(g2.game_date AS DATE) = CAST(g1.game_date AS DATE) + INTERVAL '1 day'
            //        WHERE g1.team_id = '1610612752'  -- Filter by team
            //)
            //SELECT 
            //    first_game_location,
            //    second_game_location,
            //    COUNT(*) AS game_count,
            //    AVG(second_game_points) AS avg_points
            //FROM back_to_back_games
            //GROUP BY first_game_location, second_game_location
            //ORDER BY first_game_location, second_game_location;
            
            //SELECT AVG(pts) AS avg_pts_b2b
            //FROM league_games_{season} g
            //WHERE g.team_id = @teamId
            //AND EXISTS (
            //    SELECT 1 
            //    FROM league_games_{season} b2b
            //    WHERE b2b.team_id = g.team_id
            //    AND CAST(b2b.game_date AS DATE) = CAST(g.game_date AS DATE) - INTERVAL '1 day'
            //);

            //SELECT AVG(pts) AS avg_pts_non_b2b
            //FROM league_games_{season} g
            //WHERE g.team_id = @teamId
            //AND NOT EXISTS (
            //    SELECT 1 
            //    FROM league_games_{season} b2b
            //    WHERE b2b.team_id = g.team_id
            //    AND CAST(b2b.game_date AS DATE) = CAST(g.game_date AS DATE) - INTERVAL '1 day'
            //);
            //let TeamPtsAverageResponse = [];
            let percentageOfTotalHome = 1;
            let percentageOfTotalVisitor = 1;
            //if (B2BObject.length > 0) {
            //    console.log(teamIds);
            //    const queryString = teamIds.map(id => `teamIds=${encodeURIComponent(id)}`).join("&");
            //    const url = `/api/LeagueGames/TeamPtsAverage/${season}?${queryString}`;
            //    
            //    console.log(url);
            //    
            //    const response = await getJsonResponseJackorithm(`/api/LeagueGames/B2B_Averages/${season}?${queryString}`);
            //    console.log(response);
            //    const B2BAveragesResponse = response;
            //    //const queryString2 = teamIds.map(id => `teamIds=${encodeURIComponent(id)}`).join("&");
            //    const response2 = await fetch(`/api/LeagueGames/TeamPtsAverage/${season}?${queryString}`);
            //    console.log(response2);
            //    const TeamPtsAverageResponse = await response2.json();
            //    console.log(TeamPtsAverageResponse[0])
            //    console.log(TeamPtsAverageResponse);
//
            //    for (let i = 0; i < B2BObject.length; i++) {
            //        if (B2BObject[i].team_id === homeTeam && B2BObject[i].matchup.includes("vs.")) {
            //            //if b2b team id equals current game home team id, and matchup includes 'vs.', home team's previous game was also home
            //            //current game = home
            //            //previous game = home
            //            if (B2BAveragesResponse.length > 0 && TeamPtsAverageResponse.length > 0) {
            //                const homeGame = B2BAveragesResponse.find((game: { first_game_location: string; second_game_location: string; team_id: string}) => game.first_game_location === "home" && game.second_game_location === "home" && game.team_id === homeTeam);
            //                const totalAverage = TeamPtsAverageResponse.find((game: {avg: number; team_id: string }) => game.team_id === homeTeam)
            //                if (homeGame) {
            //                    console.log(homeGame)
            //                    console.log(totalAverage)
            //                    percentageOfTotalHome = homeGame.avg_points / totalAverage.avg;
            //                    console.log("Home then Home: " + percentageOfTotalHome);
            //                }
            //            }
            //            console.log(B2BObject[i])
            //            console.log(homeTeam)
            //            //homeTeamB2B = 'HH';
            //        } else if (B2BObject[i].team_id === homeTeam && B2BObject[i].matchup.includes("@")) {
            //            //current game = home
            //            //previous game = visitor
            //            if (B2BAveragesResponse.length > 0 && TeamPtsAverageResponse.length > 0) {
            //                const VHGame = B2BAveragesResponse.find((game: { first_game_location: string; second_game_location: string; team_id: string}) => game.first_game_location === "away" && game.second_game_location === "home" && game.team_id === homeTeam);
            //                const totalAverage = TeamPtsAverageResponse.find((game: {avg: number; team_id: string }) => game.team_id === homeTeam)
            //                if (VHGame) {
            //                    console.log(VHGame)
            //                    console.log(totalAverage)
            //                    let percentageOfTotalHome = VHGame.avg_points / totalAverage.avg;
            //                    console.log("Away then Home: " + percentageOfTotalHome);
            //                }
            //            }
            //            console.log(B2BObject[i])
            //            console.log(homeTeam)
            //            //homeTeamB2B = 'VH';
            //        } else if (B2BObject[i].team_id === visitorTeam && B2BObject[i].matchup.includes("vs.")) {
            //            //current game = visitor
            //            //previous game = home
            //            if (B2BAveragesResponse.length > 0 && TeamPtsAverageResponse.length > 0) {
            //                const HVGame = B2BAveragesResponse.find((game: { first_game_location: string; second_game_location: string; team_id: string}) => game.first_game_location === "home" && game.second_game_location === "away" && game.team_id === visitorTeam);
            //                const totalAverage = TeamPtsAverageResponse.find((game: {avg: number; team_id: string }) => game.team_id === visitorTeam)
            //                if (HVGame) {
            //                    console.log(HVGame)
            //                    console.log(totalAverage)
            //                    let percentageOfTotalVisitor = HVGame.avg_points / totalAverage.avg;
            //                    console.log("Home then Away: " + percentageOfTotalVisitor);
            //                }
            //            }
            //            console.log(B2BObject[i])
            //            console.log(visitorTeam)
            //            //visitorTeamB2B = 'HV';
            //        } else if (B2BObject[i].team_id === visitorTeam && B2BObject[i].matchup.includes("@")) {
            //            //current game = visitor
            //            //previous game = visitor
            //            if (B2BAveragesResponse.length > 0 && TeamPtsAverageResponse.length > 0) {
            //                const visitorGame = B2BAveragesResponse.find((game: { first_game_location: string; second_game_location: string; team_id: string}) => game.first_game_location === "away" && game.second_game_location === "away" && game.team_id === visitorTeam);
            //                const totalAverage = TeamPtsAverageResponse.find((game: {avg: number; team_id: string }) => game.team_id === visitorTeam)
            //                if (visitorGame) {
            //                    console.log(visitorGame.avg_points)
            //                    console.log(totalAverage)
            //                    let percentageOfTotalVisitor = visitorGame.avg_points / totalAverage.avg;
            //                    console.log("Away then Away: " + percentageOfTotalVisitor);
            //                }
            //            }
            //            console.log(B2BObject[i])
            //            console.log(visitorTeam)
            //            //visitorTeamB2B = 'VV';
            //        } else {
            //            console.log('???????????????????????????????????')
            //        }
            //    }
            //}
//
//
//
            await getPostObject(games[i], season, previousSeason, percentageOfTotalHome, percentageOfTotalVisitor)
        }
    }

    const isBackToBack = async(game: LeagueGameWithHomeVisitor, season: string) => {
        let date = new Date(game.game_date);
        date.setDate(date.getDate() - 1);
        let formattedDate = date.toISOString().split('T')[0];
    
        // Serialize object properties into query parameters
        let queryParams = new URLSearchParams({
            game_id: game.game_id.toString(),
            home_team_id: game.home_team_id.toString(),
            visitor_team_id: game.visitor_team_id.toString(),
            game_date: game.game_date
        }).toString();
    
        let url = `/api/LeagueGames/BackToBack/${formattedDate}/${season}?${queryParams}`;
    
        let response = await getJsonResponseJackorithm(url);
        return response;
    };

    const loadExpectedBySeason = async() => {
        const season = "2024_25";
        await getGames(season);
    }
    //loadExpectedBySeason();

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
            <button onClick={loadMLBGames}>MLB Games</button>
            <button onClick={() => loadMLBPlayerGamesByCategory("fielding", "2023")}>MLB Player Games</button>
            <button onClick={() => loadMLBActivePlayers("2023")}>MLB Active Players</button>
            <button onClick={() => loadMLBPlayerGameInfoBySeason("2023")}>MLB Player Game Info</button>
            <button onClick={() => loadMLBTeamInfoBySeason("2023")}>MLB Team Info</button>

        </div>
  );
}

export default Admin;