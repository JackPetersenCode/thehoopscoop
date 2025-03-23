import '../App.css';
import { useEffect, useState } from "react";
import axios from 'axios';
//import ExpectedResults from "./ExpectedResults";
import {
    TOR, DEN, HOU, IND, CHI, GSW, BOS, LAC, POR, ATL, CLE, DAL, NOP, SAC, MIL, WAS, BKN, LAL,
    SAS, OKC, CHA, MIN, PHX, MEM, NYK, PHI, ORL, MIA, UTA, DET
} from 'react-nba-logos';
import { RosterPlayer, UpcomingGame, UpcomingPostObject, teamIds, teams } from '../interfaces/Gambling';
import {NBALogoType} from '../interfaces/Gambling';

interface Component {
    [team_abbr: string]: NBALogoType;
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


const Upcoming = () => {


    const [upcomingGames, setUpcomingGames] = useState<UpcomingPostObject[]>([]);
    const [selectedSeason] = useState('2024_25')
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

            const getAverageScore = async (season: string, previousGameId: string, previousSeason: string, game_date: string) => {

                let averageScore;
                if (previousGameId !== '1' && previousGameId.slice(-2) !== '01') {
                    averageScore = await getJsonResponseJackorithm(`/api/Gambling/averageScore/${season}/${game_date}`);
                    console.log(averageScore)
                } else {
                    averageScore = await getJsonResponseJackorithm(`/api/Gambling/averageScore/${previousSeason}`);
                    console.log(averageScore)
                }
                return averageScore;
            }


            const getRoster = async (season: string, teamId: string, previousGameId: string) => {
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


            //const fixTheName = async (name: string) => {
            //    let newName = name;
            //    if (name === 'Los Angeles Lakers') {
            //        newName = 'LALakers'
            //    }
            //    else if (name === 'LA Clippers') {
            //        newName = 'LAClippers';
            //    }
            //    else if (name === 'Portland Trail Blazers') {
            //        newName = 'Portland';
            //    } else {
            //        let namesplit = name.split(' ');
            //        if (namesplit.length === 3) {
            //            newName = namesplit[0] + namesplit[1];
            //        } else {
            //            newName = namesplit[0];
            //        }
            //    }
            //    return newName;
            //}

            //const fixTheGameDate = async (date: string) => {
            //    let splitDate = date.split('-');
            //    let fixedDate;
            //    if (splitDate[1].substring(0, 1) === '0') {
            //        fixedDate = (splitDate[1] + splitDate[2]).substring(1)
            //    } else {
            //        fixedDate = (splitDate[1] + splitDate[2]);
            //    }
            //    return fixedDate;
            //}

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

                //let game_date = await fixTheGameDate(game.commence_time)
                //console.log(game_date)

                //let home_odds = await getOdds(season, newHome, home_team[0].team_name, game_date, game.game_date, 'home')
                //let home_odds = game.home_odds;

                //let visitor_odds = await getOdds(season, newVisitor, visitor_team[0].team_name, game_date, game.game_date, 'away')
                //let visitor_odds = game.visitor_odds;


                //let green_red = await getGreenRed(parseFloat(expected[1]), parseFloat(expected[3]), parseFloat(game.pts), (parseFloat(game.pts) - parseFloat(game['plus_minus'])))
                
                console.log(expected[1]);
                console.log(expected[3]);
                
                const HomeLogo = components[home_abbr];
                const VisitorLogo = components[visitor_abbr];

                const obj: UpcomingPostObject = {
                    game_date: game.commence_time,
                    matchup: `${home_abbr} vs. ${visitor_abbr}`,
                    home_team: homeTeamName,
                    home_team_id: homeTeamId,
                    home_expected: parseFloat(expected[1]),
                    visitor_team: visitorTeamName,
                    visitor_team_id: visitorTeamId,
                    visitor_expected: parseFloat(expected[3]),
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

                let homePrevious = await getJsonResponseJackorithm(`/api/Gambling/previousGameId/${season}/${homeTeamId}/${game.commence_time}`)
                console.log(homePrevious);
                if (homePrevious.length < 1) {
                    homePrevious = '1';
                } else {
                    homePrevious = homePrevious[0].game_id;
                }

                const homeRoster = await getRoster(season, homeTeamId, homePrevious);
                let visitorPrevious = await getJsonResponseJackorithm(`/api/Gambling/previousgameid/${season}/${visitorTeamId}/${game.commence_time}`)
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

            const getExpectedFromRoster = async (season: string, H_or_V: string, roster: RosterPlayer[], previousGameId: string, stat: string, previousSeason: string, game_date: string) => {
                let totalMins = 0.0;
                let totalStat = 0.0;
                console.log(season);
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
                        if (averages.length > 0) {

                            totalMins += parseFloat(averages[0].min);
                            totalStat += parseFloat(averages[0][stat]);

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

                return (averageScore[0].avg + (totalStat / totalMins * 240)).toFixed(0);
            }

            const previousSeason = await getPreviousYear(selectedSeason);
            console.log(selectedSeason)
            let results = await axios.get(`/api/Gambling/upcomingGames/${selectedSeason}`);
            console.log(results);
            let games = results.data;
            console.log(games)
            const filteredGames = games.filter((game: UpcomingGame) => {
                const dateString: string = game.commence_time;
                const dateParts: string[] = dateString.split("-");
                const year: number = parseInt(dateParts[0]);
                const month: number = parseInt(dateParts[1]) - 1; // Months are 0-indexed in JavaScript
                const day: number = parseInt(dateParts[2]);
                const dateFromString: Date = new Date(year, month, day);

                // Get current date and time
                const currentDate: Date = new Date();
                //return dateFromString >= currentDate;
                return dateFromString;
            })

            if (filteredGames.length <= 0) {
                const results = await axios.get(`/api/Gambling/topTenHistorical/${selectedSeason}`);
                games = results.data.forEach((obj: UpcomingGame) => {
                    obj.home_odds = "unavailable";
                    obj.away_odds = "unavailable";
                    obj.game_id = "upcoming";
                });
                console.log(games)
            } else {
                games = filteredGames;
                console.log(games)
            }

            for (let i = 0; i < games.length; i++) {
                const homeTeam = games[i].home_team;
                const visitorTeam = games[i].away_team;
                const homeTeamId = teamIds[homeTeam];
                const visitorTeamId = teamIds[visitorTeam];
                console.log(games[i]);
                await getPostObject(games[i], selectedSeason, previousSeason, homeTeamId, visitorTeamId, teams[homeTeam], teams[visitorTeam], homeTeam, visitorTeam);
            }
        }
        getGames();
    }, [])

    return (
        <div>

        {upcomingGames.map((game, index) => (
        
            <div key={index} className='upcoming-grid'>
                <div className='upcoming-game-date'>
                    {game.game_date}                                                    
                </div>                
                <div></div>
                <div className='upcoming-headers'>EXPECTED SCORE</div>
                <div className='upcoming-headers'>MONEYLINE</div>
                <div className='inner-upcoming-flex'>
                    {game.matchup && game.HomeLogo ? 
                    <div className='team-logo-flex'>
                        <game.HomeLogo size={50} />

                        <div className='upcoming-team-abbr'>
                            {' ' + game.home_abbr}
                        </div>
                    </div>

                     : 'loading'}
                </div>
                <div className='upcoming-scores'>
                    {game.home_expected}
                </div>
                <div className='upcoming-scores'>
                    {game.home_odds}
                </div>
                <div className='inner-upcoming-flex'>
                    {game.matchup && game.VisitorLogo ? 
                    <div className='team-logo-flex'>
                        <game.VisitorLogo size={50} /> 
                        <div className='upcoming-team-abbr'>
                            {' ' + game.visitor_abbr}
                        </div>
                    </div>

                     : 'loading'}
                </div>
                <div className='upcoming-scores'>
                    {game.visitor_expected}
                </div>
                <div className='upcoming-scores'>
                    {game.visitor_odds}
                </div>
                    
            </div>
        ))}
        </div>
)} 



export default Upcoming;