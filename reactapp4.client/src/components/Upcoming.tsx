import '../App.css';
import React, { useEffect, useState } from "react";
import axios from 'axios';
import ExpectedResults from "./ExpectedResults";
import {
    TOR, DEN, HOU, IND, CHI, GSW, BOS, LAC, POR, ATL, CLE, DAL, NOP, SAC, MIL, WAS, BKN, LAL,
    SAS, OKC, CHA, MIN, PHX, MEM, NYK, PHI, ORL, MIA, UTA, DET
} from 'react-nba-logos';

interface Component {
    [team_abbr: string]: unknown;
}

const components: Component = {
    TOR: TOR,
    DEN: DEN,
    HOU: HOU,
    DET: DET,
    UTA: UTA,
    MIA: MIA,
    ORL: ORL,
    PHI: PHI,
    NYK: NYK,
    MEM: MEM,
    PHX: PHX,
    MIN: MIN,
    CHA: CHA,
    OKC: OKC,
    SAS: SAS,
    LAL: LAL,
    BKN: BKN,
    WAS: WAS,
    MIL: MIL,
    SAC: SAC,
    NOP: NOP,
    DAL: DAL,
    CLE: CLE,
    ATL: ATL,
    POR: POR,
    LAC: LAC,
    BOS: BOS,
    GSW: GSW,
    CHI: CHI,
    IND: IND
};

interface Teams {
    [teamName: string]: string;
}

interface TeamIds {
    [teamName: string]: string;
}

const teams: Teams = {
    "Denver Nuggets": "DEN",
    "Indiana Pacers": "IND",
    "Chicago Bulls": "CHI",
    "Houston Rockets": "HOU",
    "Golden State Warriors": "GSW",
    "Boston Celtics": "BOS",
    "Los Angeles Clippers": "LAC",
    "Atlanta Hawks": "ATL",
    "Cleveland Cavaliers": "CLE",
    "Portland Trail Blazers": "POR",
    "New Orleans Pelicans": "NOP",
    "Dallas Mavericks": "DAL",
    "Sacramento Kings": "SAC",
    "Milwaukee Bucks": "MIL",
    "Washington Wizards": "WAS",
    "Brooklyn Nets": "BKN",
    "Los Angeles Lakers": "LAL",
    "Toronto Raptors": "TOR",
    "San Antonio Spurs": "SAS",
    "Oklahoma City Thunder": "OKC",
    "Charlotte Hornets": "CHA",
    "Minnesota Timberwolves": "MIN",
    "Phoenix Suns": "PHX",
    "New York Knicks": "NYK",
    "Memphis Grizzlies": "MEM",
    "Philadelphia 76ers": "PHI",
    "Orlando Magic": "ORL",
    "Miami Heat": "MIA",
    "Utah Jazz": "UTA",
    "Detroit Pistons": "DET"
}

const teamIds: TeamIds = {
    "Denver Nuggets": "1610612743",
    "Indiana Pacers": "1610612754",
    "Chicago Bulls": "1610612741",
    "Houston Rockets": "1610612745",
    "Golden State Warriors": "1610612744",
    "Boston Celtics": "1610612738",
    "Los Angeles Clippers": "1610612746",
    "Atlanta Hawks": "1610612737",
    "Cleveland Cavaliers": "1610612739",
    "Portland Trail Blazers": "1610612757",
    "New Orleans Pelicans": "1610612740",
    "Dallas Mavericks": "1610612742",
    "Sacramento Kings": "1610612758",
    "Milwaukee Bucks": "1610612749",
    "Washington Wizards": "1610612764",
    "Brooklyn Nets": "1610612751",
    "Los Angeles Lakers": "1610612747",
    "Toronto Raptors": "1610612761",
    "San Antonio Spurs": "1610612759",
    "Oklahoma City Thunder": "1610612760",
    "Charlotte Hornets": "1610612766",
    "Minnesota Timberwolves": "1610612750",
    "Phoenix Suns": "1610612756",
    "New York Knicks": "1610612752",
    "Memphis Grizzlies": "1610612763",
    "Philadelphia 76ers": "1610612755",
    "Orlando Magic": "1610612753",
    "Miami Heat": "1610612748",
    "Utah Jazz": "1610612762",
    "Detroit Pistons": "1610612765"
}

interface UpcomingGame {
    game_id: string,
    commence_time: string,
    home_odds: string,
    away_odds: string,
    home_team: string,
    away_team: string
}

const Upcoming = () => {


    const [upcomingGames, setUpcomingGames] = useState([]);
    const [selectedSeason, setSelectedSeason] = useState('2023_24')
    //
    //useEffect(() => {
    //    const getUpcoming = async() => {
    //        const results = await axios.get(`/api/gambling/upcominggames/${selectedSeason}`)
    //        setUpcomingGames(results.data);
    //        setSelectedSeason('2023-2024')
    //    }
    //    if (selectedSeason) {
    //        getUpcoming();
    //    }
    //}, [selectedSeason])
    useEffect(() => {
        const getGames = async () => {


            const getJsonResponseJackorithm = async (url: string) => {
                //console.log(url)
                //const response = await fetch(url);
                //console.log(response)
                //try{
                //    if (response.ok){
                //        const jsonResponse = await response.json();
                //        console.log(jsonResponse)
                //        return jsonResponse;
                //    }
                //} catch(err){
                //    console.log(err);
                //}
                const results = await axios.get(url);
                return results.data;
            }

            const getPreviousYear = async (season: string) => {
                const split = season.split('_')
                const previous = parseInt(split[1]) - 1;
                const previous2 = parseInt(split[0]) - 1;
                const thisSeason = previous2 + '_' + previous;
                return thisSeason;
            }

            const getAverageScore = async (season, previousGameId, previousSeason, game_date) => {

                let averageScore;
                if (previousGameId !== '1' && previousGameId.slice(-2) !== '01') {
                    averageScore = await getJsonResponseJackorithm(`/api/leagueGames/averageScore/${game_date}/${season}`);
                } else {
                    averageScore = await getJsonResponseJackorithm(`/api/leagueGames/averageScore/${previousSeason}`);
                }
                return averageScore;
            }


            const getRoster = async (season, teamId, previousGameId) => {
                if (previousGameId === '1') {
                    let roster = await getJsonResponseJackorithm(`/api/boxPlayers/getroster/${previousSeason}/${teamId}`)
                    return roster;
                } else {
                    let roster = await getJsonResponseJackorithm(`/api/boxPlayers/previousgame/gameid/${season}/${teamId}/${previousGameId}`);
                    if (!roster || roster.length === 0) {
                        roster = await getJsonResponseJackorithm(`/api/boxPlayers/previousgame/gameid/boxscores/${season}/${teamId}/${previousGameId}`);
                    }
                    return roster;
                }

            }


            const postExpectedMatchup = async (obj, season) => {
                const url = `/api/gambling/jackorithm/${season}`;
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

            const fixTheName = async (name) => {
                let newName = name;
                if (name === 'Los Angeles Lakers') {
                    newName = 'LALakers'
                }
                else if (name === 'LA Clippers') {
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

            const fixTheGameDate = async (date) => {
                let splitDate = date.split('-');
                let fixedDate;
                if (splitDate[1].substring(0, 1) === '0') {
                    fixedDate = (splitDate[1] + splitDate[2]).substring(1)
                } else {
                    fixedDate = (splitDate[1] + splitDate[2]);
                }
                return fixedDate;
            }

            const getOdds = async (season, oddsTeam, fullTeam, oddsDate, fullDate, H_or_V) => {

                let moneyline = await getJsonResponseJackorithm(`/api/gambling/moneyline/home/${season}/${oddsTeam}/${oddsDate}`)
                if (moneyline.length < 1) {
                    if (season === '2023-2024') {
                        moneyline = await getJsonResponseJackorithm(`/api/gambling/newOdds/${season}/${fullTeam}/${fullDate}/${H_or_V}`)
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

            const getGreenRed = async (home_expected, visitor_expected, home_actual, visitor_actual) => {

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

            const getPostObject = async (game: UpcomingGame,
                season: string,
                previousSeason: string,
                homeTeamId: string,
                visitorTeamId: string,
                home_abbr: string,
                visitor_abbr: string,
                homeTeamName: string,
                visitorTeamName: string
            ) => {

                const expected = await getExpected(game, season, previousSeason, homeTeamId, visitorTeamId);

                const newHome = await fixTheName(homeTeamName);
                const newVisitor = await fixTheName(visitorTeamName);
                //let game_date = await fixTheGameDate(game.commence_time)
                //console.log(game_date)

                //let home_odds = await getOdds(season, newHome, home_team[0].team_name, game_date, game.game_date, 'home')
                //let home_odds = game.home_odds;

                //let visitor_odds = await getOdds(season, newVisitor, visitor_team[0].team_name, game_date, game.game_date, 'away')
                //let visitor_odds = game.visitor_odds;


                //let green_red = await getGreenRed(parseFloat(expected[1]), parseFloat(expected[3]), parseFloat(game.pts), (parseFloat(game.pts) - parseFloat(game['plus_minus'])))

                const HomeLogo = components[home_abbr];
                const VisitorLogo = components[visitor_abbr];

                const obj = {
                    game_date: game.commence_time,
                    matchup: `${home_abbr} vs. ${visitor_abbr}`,
                    home_team: homeTeamName,
                    home_team_id: homeTeamId,
                    home_expected: parseFloat(expected[1]).toFixed(0),
                    visitor_team: visitorTeamName,
                    visitor_team_id: visitorTeamId,
                    visitor_expected: parseFloat(expected[3]).toFixed(0),
                    //home_actual: game.pts,
                    //visitor_actual: parseInt(game.pts) - parseInt(game['plus_minus']),
                    home_odds: game.home_odds,
                    visitor_odds: game.away_odds,
                    HomeLogo: HomeLogo,
                    VisitorLogo: VisitorLogo,
                    home_abbr: home_abbr,
                    visitor_abbr: visitor_abbr
                    //green_red: green_red
                }
                setUpcomingGames((currentGames) => [...currentGames, obj]);
                //postExpectedMatchup(obj, season)
            }

            const getExpected = async (game: UpcomingGame, season: string, previousSeason: string, homeTeamId: string, visitorTeamId: string) => {
                const stat = "+/-";
                //let homeTeamId = game.home_team_id;

                let homePrevious = await getJsonResponseJackorithm(`/api/Gambling/previousGameId/${game.game_id}/${season}/${homeTeamId}/${game.commence_time}`)
                console.log(homePrevious);
                if (homePrevious.length < 1) {
                    homePrevious = '1';
                } else {
                    homePrevious = homePrevious[0].game_id;
                }

                const homeRoster = await getRoster(season, homeTeamId, homePrevious);
                let visitorPrevious = await getJsonResponseJackorithm(`/api/boxScoresTraditional/previousgameid/${game.game_id}/${season}/${visitorTeamId}/${game.commence_time}`)
                if (visitorPrevious.length < 1) {
                    visitorPrevious = '1';
                } else {
                    visitorPrevious = visitorPrevious[0].game_id
                }

                let visitorRoster = await getRoster(season, visitorTeamId, visitorPrevious);
                let homeExpected = await getExpectedFromRoster(season, 'home', homeRoster, homePrevious, stat, previousSeason, game.commence_time);
                let visitorExpected = await getExpectedFromRoster(season, 'visitor', visitorRoster, visitorPrevious, stat, previousSeason, game.commence_time);

                return [homeTeamId, homeExpected, visitorTeamId, visitorExpected];
            }

            const getExpectedFromRoster = async (season, H_or_V, roster, previousGameId, stat, previousSeason, game_date) => {
                let totalMins = 0.0;
                let totalStat = 0.0;
                let averageScore = await getAverageScore(season, previousGameId, previousSeason, game_date)
                for (let i = 0; i < roster.length; i++) {

                    let averages;
                    if (previousGameId !== '1') {
                        averages = await getJsonResponseJackorithm(`/api/boxScoresTraditional/averages/82games/${previousGameId}/${roster[i].player_id}/${season}/${H_or_V}/${game_date}`)

                        if (averages.length > 0) {
                            totalMins += parseFloat(averages[0].min);
                            totalStat += parseFloat(averages[0][stat]);

                        } else {
                            averages = await getJsonResponseJackorithm(`/api/boxScoresTraditional/averages/82games/${roster[i].player_id}/${previousSeason}/${H_or_V}`)
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
                        averages = await getJsonResponseJackorithm(`/api/boxScoresTraditional/averages/82games/${roster[i].player_id}/${previousSeason}/${H_or_V}`);
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

            const previousSeason = await getPreviousYear(selectedSeason);
            const games = await axios.get(`/api/Gambling/upcomingGames/${selectedSeason}`);
            console.log(games.data);
            //let games = {
            //    data: [{
            //        home_team: "Atlanta Hawks",
            //        home_odds: "-110",
            //        game_id: "upcoming",
            //        commence_time: "2023-11-17",
            //        away_team: "Philadelphia 76ers",
            //        away_odds: "-110"
            //    }]
            //}

            for (let i = 0; i < games.data.length; i++) {
                const homeTeam = games.data[i].home_team;
                const visitorTeam = games.data[i].away_team;
                const homeTeamId = teamIds[homeTeam];
                const visitorTeamId = teamIds[visitorTeam];
                console.log(games.data[i]);
                await getPostObject(games.data[i], selectedSeason, previousSeason, homeTeamId, visitorTeamId, teams[homeTeam], teams[visitorTeam], homeTeam, visitorTeam);
            }
        }
        getGames();
    }, [])

    return (
        //<div>
        //{upcomingGames.map((game, index) => (
        //    <div key={index}>{<ExpectedResults game={game} 
        //                                        selectedSeason={selectedSeason} 
        //                                        setSelectedSeason={setSelectedSeason} />}</div>
        //))}
        //</div>
        //<div>
        //    {upcomingGames.map((game, index) => (
        //        <div>
        //            {game.home_expected}<br></br>
        //            {game.visitor_expected}
        //        </div>
        //    ))}
        //</div>

        <div>
            upcoming
        </div>
    )
}


export default Upcoming;


//{
//    upcomingGames.map((game, index) => (
//
//        <div key={index} className='upcoming-grid'>
//            <div className='upcoming-game-date'>
//                {game.game_date}
//            </div>
//            <div></div>
//            <div className='upcoming-headers'>EXPECTED SCORE</div>
//            <div className='upcoming-headers'>MONEYLINE</div>
//            <div className='inner-upcoming-flex'>
//                {game.matchup && game.HomeLogo ?
//                    <div className='logo-flex'>
//                        <game.HomeLogo size={50} />
//
//                        <div>
//                            {' ' + game.home_abbr}
//                        </div>
//                    </div>
//
//                    : 'loading'}
//            </div>
//            <div>
//                {game.home_expected}
//            </div>
//            <div>
//                {game.home_odds}
//            </div>
//            <div className='inner-upcoming-flex'>
//                {game.matchup && game.VisitorLogo ?
//                    <div className='logo-flex'>
//                        <game.VisitorLogo size={50} />
//                        <div>
//                            {' ' + game.visitor_abbr}
//                        </div>
//                    </div>
//
//                    : 'loading'}
//            </div>
//            <div>
//                {game.visitor_expected}
//            </div>
//            <div>
//                {game.visitor_odds}
//            </div>
//
//        </div>
//    ))
//}