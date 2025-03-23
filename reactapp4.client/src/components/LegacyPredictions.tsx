import { useState } from "react";
import SeasonsDropdown from "../components/SeasonsDropDown";
import Upcoming from "../components/Upcoming";
import '../App.css'
import HistoricalResults from "../components/HistoricalResults";
import WinPct from "../components/WinPct";
import TeamsDropDown from "./TeamsDropDown";
import { NBATeam } from "../interfaces/Teams";

const LegacyPredictions = () => {

    const [selectedSeason, setSelectedSeason] = useState('2024_25');
    const [selectedTeam, setSelectedTeam] = useState<NBATeam | string>("");
    /*
        useEffect(() => {
            const postMatchups = async() => {
                <PostMatchups selectedSeason={selectedSeason}
                              setSelectedSeason={setSelectedSeason}
                              homeExpectedResults={homeExpectedResults}
                              setHomeExpectedResults={setHomeExpectedResults}
                              visitorExpectedResults={setVisitorExpectedResults}
                              setVisitorExpectedResults={setVisitorExpectedResults}/>
            }
            postMatchups()
        }, [])*/
    //<Schedule selectedSeason={selectedSeason} setSelectedSeason={setSelectedSeason} scheduleData={scheduleData} setScheduleData={setScheduleData}/>

    return (
        <div>
            <br></br>
            <div className='statistics-title'>
                NBA Odds & Predictions
            </div>
            <div className='yellow-line'>
            </div>
            <div className="main-predictions-flex">
                <div className="historical-div">
                    <div className="predictions-drop-flex">
        
                        <div className="predictions-season-flex">
                            <SeasonsDropdown selectedSeason={selectedSeason} setSelectedSeason={setSelectedSeason} setSelectedPlayerShotCharts={() => { }} setSelectedGame={() => { }} isShotCharts={false} isPredictions={true} />
                        </div>
        
                        <div className="predictions-season-flex">
                            <TeamsDropDown selectedTeam={selectedTeam} setSelectedTeam={setSelectedTeam} setGameData={() => { }} setSeasonShotsData={() => {}} setGameShotsData={() => {}} />
                        </div>
        
                    </div>
                    <div>
                        <WinPct selectedSeason={selectedSeason} selectedTeam={selectedTeam} />
                        <div className='shot-colors'>
                            <div className='light-green-block'>
                            </div>
        
                            <div>
                                - Correct Prediction
                            </div>
                        </div>
                        <div className='shot-colors'>
                            <div className='red-block'>
                            </div>
        
                            <div>
                                - Incorrect Prediction
                            </div>
                        </div>
                        <br></br>
                        <h2>
                            Historical Games
                        </h2>
                        <HistoricalResults selectedSeason={selectedSeason} selectedTeam={selectedTeam as string} />
                    </div>
                </div>
                <div className="upcoming-div">
                    <h4 className='expected-info'>EXPECTED SCORE:<br></br>
                        - HOME AND VISITOR ROSTERS ARE PULLED FROM EACH TEAM'S PREVIOUS GAME'S BOX SCORE<br></br>
                        - EACH PLAYER'S 82-GAME PLUS-MINUS AND MINUTES AVERAGES ARE TOTALLED <br></br>
                        (82-GAME AVERAGES ARE CALCULATED USING EVERY GAME'S BOX SCORE UP UNTIL THE GAME DATE IN QUESTION, EVEN IF PLAYER DID NOT PLAY)<br></br>
                        - THE TOTAL OF EACH TEAM'S PLUS-MINUS AVERAGES ARE THEN DIVIDED BY THEIR RESPECTIVE TOTALS OF MINUTES AVERAGES <br></br>
                        - BOTH QUOTIENTS ARE THEN MULTIPLIED BY 240 (5 PLAYERS ON THE FLOOR, 48 MINUTES EACH) TO ESTIMATE A TEAM'S TOTAL PLUS-MINUS AVERAGE PER GAME<br></br>
                        - TOTAL CALCULATED PLUS-MINUS AVERAGE IS THEN ADDED TO THE CURRENT AVERAGE SCORE OF ANY GIVEN GAME IN THE SEASON IN QUESTION UP UNTIL THE GAME DATE IN QUESTION TO GET TOTAL ESTIMATED TEAM SCORES
        
                    </h4>
                    
                    <h2>
                        Upcoming Games
                    </h2>
                    <Upcoming />
                </div>
            </div>
        </div>
    )
}

export default LegacyPredictions;
