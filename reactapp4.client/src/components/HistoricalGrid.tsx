import React from 'react';
import {TOR, DEN, HOU, IND, CHI, GSW, BOS, LAC, POR, ATL, CLE, DAL, NOP, SAC, MIL, WAS, BKN, LAL,
    SAS, OKC, CHA, MIN, PHX, MEM, NYK, PHI, ORL, MIA, UTA, DET } from 'react-nba-logos';
import { ExpectedMatchupPostObject } from '../interfaces/Gambling';
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

interface HistoricalGridProps {
    game: ExpectedMatchupPostObject;
}

const HistoricalGrid: React.FC<HistoricalGridProps> = ({game}) => {


    console.log('historical grid')
    console.log(game);
    let HomeLogo;
    let VisitorLogo;

    HomeLogo = components[game.matchup.substring(0,3)]
    VisitorLogo = components[game.matchup.substring(8,11)]

    const getGreenRed = (green_red: string) => ({
        backgroundColor: green_red === 'green' ? 'rgb(204, 255, 200)' : 'rgb(255, 208, 208)'
    }) 

    return (
        <div className="historical-grid" style={getGreenRed(game.green_red)} >
            <div className='historical-game-date' style={{fontSize: 'small'}}>{game.game_date.charAt(5) === '0' ?
                                                    <>
                                                    {game.game_date.substring(6,10)}
                                                    <br></br>
                                                    {game.game_date.substring(0,4)}
                                                    </>
                                                    :
                                                    <>
                                                    {game.game_date.substring(5,10)}
                                                    <br></br>
                                                    {game.game_date.substring(0,4)}
                                                    </>}                                                        
            </div>
            <div></div>
            <div className='historical-headers'>
                EXPECTED SCORE
            </div>
            <div className='historical-headers'>
                ACTUAL SCORE
            </div>
            <div className='historical-headers'>
                MONEYLINE
            </div>
            <div className='inner-upcoming-flex'>
                {game.matchup && HomeLogo ? 
                <div className='team-logo-flex'>
                    <HomeLogo size={35} /> 
                    <div>
                        {' ' + game.matchup.substring(0,3)}
                    </div>
                </div>

                 : 'loading'}
            </div>
            <div>
                {game.home_expected}
            </div>
            <div>
                {game.home_actual}
            </div>
            <div>
            {game.home_odds !== 'unavailable' && game.visitor_odds !== 'unavailable' ?
                <div>
                    {game.visitor_odds}
                </div>
                :
                <div style={{fontSize: 'x-small'}}>
                    unavailable
                </div>
            }
            </div>
            <div className='inner-upcoming-flex'>
                {game.matchup && VisitorLogo ? 
                <div className='team-logo-flex'>
                    <VisitorLogo size={35} /> 
                    <div>
                        {' ' + game.matchup.substring(8,11)}
                    </div>
                </div>

                 : 'loading'}
            </div>
            <div>
                {game.visitor_expected}
            </div>
            <div>
                {game.visitor_actual}
            </div>
            {game.home_odds !== 'unavailable' && game.visitor_odds !== 'unavailable' ?
                <div>
                    {game.home_odds}
                </div>
                :
            <div style={{fontSize: 'x-small'}}>
                unavailable
            </div>}
        </div>
    )   
}

export default HistoricalGrid;