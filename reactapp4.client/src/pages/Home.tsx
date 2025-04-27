import { useEffect, useState } from 'react';
import { Player } from '../interfaces/Player';
import SeasonsDropDown from '../components/SeasonsDropDown';
import StatsTable from '../components/StatsTable';
import GameOptionDropDown from '../components/GameOptionDropDown';
import FindPlayerBottom from '../components/FindPlayerBottom';
import { numPlayersOptions, lineupPlayerOptions, statOptionsLineups, statOptionsPlayers, nbaTeams, perModeOptions } from '../interfaces/DropDownOptions';
import ShotCharts from '../components/ShotCharts';
import LineupsPlayersSelect from '../components/LineupsPlayersSelect';
import BoxTypeSelect from '../components/BoxTypeSelect';
import PropBet from '../components/PropBet';
import LegacyPredictions from '../components/LegacyPredictions';
import DropDown from '../components/DropDown';
import Head2Head from '../components/Head2Head';
import axios from 'axios';
import SportOptionDropDown from '../components/SportOptionDropDown';

interface HomeProps {
    selectedSport: string;
    setSelectedSport: React.Dispatch<React.SetStateAction<string>>
}

const Home: React.FC<HomeProps> = ({ selectedSport, setSelectedSport }) => {

    console.log("NBA page")
    const [selectedSeason, setSelectedSeason] = useState('2024_25');
    const [activePlayers, setActivePlayers] = useState<Player[]>([]);
    //const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
    const [inputTextBottom, setInputTextBottom] = useState('');
    const [selectedPlayerBottom, setSelectedPlayerBottom] = useState<Player | null>(null);
    const [selectedLineupPlayer, setSelectedLineupPlayer] = useState('Players');
    const [selectedBoxType, setSelectedBoxType] = useState('Base');
    const [numPlayers, setNumPlayers] = useState('5');
    const [perMode, setPerMode] = useState('Totals');
    const [selectedTeam, setSelectedTeam] = useState({team_id: '1', team_name: 'All Teams', team_abbreviation: '1'});
    const [sortField, setSortField] = useState("min");
    //const [selectedStat, setSelectedStat] = useState<PropBetStats | null>(null);
    const [gameOption, setGameOption] = useState<string>('Prop Bet');
    //const [selectedSport, setSelectedSport] = useState<string>('NBA');

    const [roster, setRoster] = useState<Player[]>([]);
    const [usedPlayers, setUsedPlayers] = useState<Player[]>([]);
    //const [overUnderLine, setOverUnderLine] = useState<number | string>(0);
    const [selectedOpponent, setSelectedOpponent] = useState({ team_id: '1', team_name: 'All Teams', team_abbreviation: ''});
    //const [propBetStats, setPropBetStats] = useState<PropBetStats[]>([]);
    //const [playerBoxScores, setPlayerBoxScores] = useState<Stats[]>([]);
    //const [careerPlayerBoxScores, setCareerPlayerBoxScores] = useState<Stats[]>([]);

    //const [gamesPlayed, setGamesPlayed] = useState<Stats[]>([]);
    //const [careerGamesPlayed, setCareerGamesPlayed] = useState<Stats[]>([]);
    //const [homeOrVisitor, setHomeOrVisitor] = useState<string>("");
    //const [showOpponent, setShowOpponent] = useState(false);
    //const [showHomeOrVisitor, setShowHomeOrVisitor] = useState(false);






    useEffect(() => {
        async function getData() {

            const activePlayersResponse = await axios.get('api/players/active');
            const activePlayersData = await activePlayersResponse.data;
            console.log(activePlayersData)
            setActivePlayers(activePlayersData);


        }

        //populateWeatherData();
        //getEmployeesData();
        getData();
    }, []);


    return (
        <>
        <div className='logo-flex'>
            <div>
                <img src="/images/ball7.png" className="ball" alt="Home"/>
            </div>
            <div>
                <SportOptionDropDown selectedSport={selectedSport} setSelectedSport={setSelectedSport} />
            </div>
            <div>
                <GameOptionDropDown gameOption={gameOption} setGameOption={setGameOption} />
            </div>
        </div>

        <div className="home-container">

            {gameOption === "Prop Bet" ?
                <div>
                    <PropBet activePlayers={activePlayers} roster={roster} setRoster={setRoster} usedPlayers={usedPlayers} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
            : gameOption === "Shot Charts" ?
                <div>
                    <ShotCharts />
                </div>
            : gameOption === "Legacy Predictions" ?
                <div>
                    <LegacyPredictions />
                </div>
            :
            gameOption === "Head 2 Head" ?
            <div>
                <Head2Head />
            </div>
            :
            ""}
            <div className='lineup-player-boxtype-dropdown-container'>

                <div className="drop-down">
                    <LineupsPlayersSelect options={lineupPlayerOptions} selectedLineupPlayer={selectedLineupPlayer} setSelectedLineupPlayer={setSelectedLineupPlayer} setSelectedBoxType={setSelectedBoxType} setSortField={setSortField} />
                </div>
                <div className="drop-down">
                    <BoxTypeSelect options={selectedLineupPlayer === 'Players' ? statOptionsPlayers : statOptionsLineups} selectedBoxType={selectedBoxType} setSelectedBoxType={setSelectedBoxType} setSortField={setSortField} />
                </div>

            </div>
            <div className="display-flex">
                <div className="drop-down">
                    <SeasonsDropDown
                        selectedSeason={selectedSeason}
                        setSelectedSeason={setSelectedSeason}
                        setSelectedPlayerShotCharts={() => { }}
                        setSelectedGame={() => { }}
                        isShotCharts={false}
                        isPredictions={false}
                    />
                </div>
                {selectedBoxType === "Base" || selectedBoxType === "Misc" || selectedBoxType === "Opponent" ?
                    <div className="drop-down">
                    <DropDown
                            options={perModeOptions}
                            setPerMode={setPerMode}
                            setNumPlayers={setNumPlayers}
                            setSelectedTeam={setSelectedTeam}
                            setSelectedOpponent={setSelectedOpponent}
                            dropDownType="Per Mode"
                    />
                    </div>
                    :
                    ""}
                {selectedLineupPlayer === "Lineups" ?
                    <div className="drop-down">
                    <DropDown
                            options={numPlayersOptions}
                            setPerMode={setPerMode}
                            setNumPlayers={setNumPlayers}
                            setSelectedTeam={setSelectedTeam}
                            setSelectedOpponent={setSelectedOpponent}
                            dropDownType="# of Players"
                    />
                    </div>
                    :
                    ""}
                <div className="drop-down">
                <DropDown
                        options={nbaTeams}
                        setPerMode={setPerMode}
                        setNumPlayers={setNumPlayers}
                        setSelectedTeam={setSelectedTeam}
                        setSelectedOpponent={setSelectedOpponent}
                        dropDownType="Team"
                    />
                </div>
                <div className="drop-down">
                    <FindPlayerBottom activePlayers={activePlayers} inputTextBottom={inputTextBottom} setInputTextBottom={setInputTextBottom} selectedPlayerBottom={selectedPlayerBottom} setSelectedPlayerBottom={setSelectedPlayerBottom} roster={roster} setRoster={setRoster} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
                {selectedLineupPlayer === 'Players' ?
                <div className="drop-down" style={{marginRight: "0px"}}>
                    <DropDown
                        options={nbaTeams}
                        setPerMode={setPerMode}
                        setNumPlayers={setNumPlayers}
                        setSelectedTeam={setSelectedTeam}
                        setSelectedOpponent={setSelectedOpponent}
                        dropDownType="Opponent"
                    />
                </div>
                :
                ""
                }
            </div>
            
            <StatsTable selectedSeason={selectedSeason} selectedLineupPlayer={selectedLineupPlayer} selectedBoxType={selectedBoxType} numPlayers={numPlayers} perMode={perMode} selectedTeam={selectedTeam} sortField={sortField} setSortField={setSortField} inputText={inputTextBottom} setInputText={setInputTextBottom} selectedOpponent={selectedOpponent} />
        </div>
        </>
    );
}

export default Home;