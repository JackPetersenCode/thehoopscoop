import { useEffect, useState } from 'react';
import { Player } from '../interfaces/Player';
import styled from "styled-components";
import SeasonsDropDown from '../components/SeasonsDropDown';
import SearchBar from '../components/SearchBar';
import LineupPlayerSelect from '../components/LineupPlayerSelect';
import StatsTable from '../components/StatsTable';
import DropDown from '../components/DropDown';
import { NBATeam } from '../interfaces/Teams';
import { PropBetStats } from '../interfaces/PropBetStats';
import PropBetStatsDropDown from '../components/PropBetStatsDropDown';
import GameOptionDropDown from '../components/GameOptionDropDown';
import DragNDropRoster from '../components/DragNDropRoster';
import OverUnderLineInput from '../components/OverUnderLineInput';
import PropBetStatsDragNDrop from '../components/PropBetStatsDragNDrop';
import PropBetResultsTable from '../components/PropBetResultsTable';
import { Stats } from '../interfaces/StatsTable';
import FindPlayerBottom from '../components/FindPlayerBottom';
import PropBetResults from '../components/PropBetResults';
import OverUnderLine from '../components/OverUnderLine';
import PropBetOpponent from '../components/PropBetOpponent';
import PropBetHomeOrVisitor from '../components/PropBetHomeOrVisitor';
import { numPlayersOptions, lineupPlayerOptions, statOptionsLineups, statOptionsPlayers, nbaTeams, perModeOptions } from '../interfaces/DropDownOptions';
import PropBetOpponentDropDown from '../components/PropBetOpponentDropDown';
import HomeOrVisitorDropDown from '../components/HomeOrVisitorDropDown';
import Footer from '../components/Footer';
import ShotCharts from '../components/ShotCharts';
import LineupsPlayersSelect from '../components/LineupsPlayersSelect';
import BoxTypeSelect from '../components/LineupPlayerSelect';
import PropBet from '../components/PropBet';

const DataFlex = styled.div`
    display: flex;
    align-items: center;
    white-space: nowrap;
    justify-content: space-between;
    max-width: 50%;
    margin-top: 50px;
`

function Home() {

    const [selectedSeason, setSelectedSeason] = useState('2023_24');
    const [activePlayers, setActivePlayers] = useState<Player[]>([]);
    //const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
    const [inputTextBottom, setInputTextBottom] = useState('');
    const [selectedPlayerBottom, setSelectedPlayerBottom] = useState<Player | null>(null);
    const [selectedLineupPlayer, setSelectedLineupPlayer] = useState('Lineups');
    const [selectedBoxType, setSelectedBoxType] = useState('Base');
    const [numPlayers, setNumPlayers] = useState('5');
    const [perMode, setPerMode] = useState('Totals');
    const [selectedTeam, setSelectedTeam] = useState({team_id: '1', team_name: 'All Teams', team_abbreviation: '1'});
    const [sortField, setSortField] = useState("min");
    //const [selectedStat, setSelectedStat] = useState<PropBetStats | null>(null);
    const [gameOption, setGameOption] = useState<string>('Prop Bet');
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

            const activePlayersResponse = await fetch('api/players/active');
            const activePlayersData = await activePlayersResponse.json();
            console.log(activePlayersData)
            setActivePlayers(activePlayersData);


        }

        //populateWeatherData();
        //getEmployeesData();
        getData();
    }, []);


    return (
        <div className="home-container">

            <div className="mb-4">
                <GameOptionDropDown gameOption={gameOption} setGameOption={setGameOption} />
            </div>

            {gameOption === "Prop Bet" ?
                <div>
                    <PropBet activePlayers={activePlayers} roster={roster} setRoster={setRoster} usedPlayers={usedPlayers} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
            : gameOption === "Shot Charts" ?
                <div>
                    <ShotCharts />
                </div>
            :
            ""}
            <DataFlex>

                <div className="drop-down">
                    <LineupsPlayersSelect options={lineupPlayerOptions} selectedLineupPlayer={selectedLineupPlayer} setSelectedLineupPlayer={setSelectedLineupPlayer} setSelectedBoxType={setSelectedBoxType} setSortField={setSortField} />
                </div>
                <div className="drop-down">
                    <BoxTypeSelect options={selectedLineupPlayer === 'Players' ? statOptionsPlayers : statOptionsLineups} selectedBoxType={selectedBoxType} setSelectedBoxType={setSelectedBoxType} setSortField={setSortField} />
                </div>

            </DataFlex>
            <div className="d-flex">
                <div className="drop-down">
                    <SeasonsDropDown
                        selectedSeason={selectedSeason}
                        setSelectedSeason={setSelectedSeason}
                        setSelectedPlayerShotCharts={() => { }}
                        setSelectedGame={() => { }}
                        isShotCharts={false}
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
                    <DropDown
                        options={nbaTeams}
                        setPerMode={setPerMode}
                        setNumPlayers={setNumPlayers}
                        setSelectedTeam={setSelectedTeam}
                        setSelectedOpponent={setSelectedOpponent}
                        dropDownType="Opponent"
                    />
                </div>
                <div className="drop-down" style={{marginRight: "0px"}}>
                    <FindPlayerBottom activePlayers={activePlayers} inputTextBottom={inputTextBottom} setInputTextBottom={setInputTextBottom} selectedPlayerBottom={selectedPlayerBottom} setSelectedPlayerBottom={setSelectedPlayerBottom} roster={roster} setRoster={setRoster} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
            </div>
            
            <StatsTable selectedSeason={selectedSeason} selectedLineupPlayer={selectedLineupPlayer} selectedBoxType={selectedBoxType} numPlayers={numPlayers} perMode={perMode} selectedTeam={selectedTeam} sortField={sortField} setSortField={setSortField} inputText={inputTextBottom} setInputText={setInputTextBottom} selectedOpponent={selectedOpponent} />
            <Footer />
        </div>
    );
}

export default Home;