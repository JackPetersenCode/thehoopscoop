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
import { numPlayersOptions, lineupPlayerOptions, statOptions, nbaTeams, perModeOptions } from '../interfaces/DropDownOptions';
import PropBetOpponentDropDown from '../components/PropBetOpponentDropDown';
import HomeOrVisitorDropDown from '../components/HomeOrVisitorDropDown';
import Footer from '../components/Footer';
import ShotCharts from '../components/ShotCharts';

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
    const [inputText, setInputText] = useState('');
    const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
    const [inputTextBottom, setInputTextBottom] = useState('');
    const [selectedPlayerBottom, setSelectedPlayerBottom] = useState<Player | null>(null);
    const [selectedLineupPlayer, setSelectedLineupPlayer] = useState('Lineups');
    const [selectedBoxType, setSelectedBoxType] = useState('Base');
    const [numPlayers, setNumPlayers] = useState('5');
    const [perMode, setPerMode] = useState('Totals');
    const [selectedTeam, setSelectedTeam] = useState({team_id: '1', team_name: 'All Teams', team_abbreviation: '1'});
    const [sortField, setSortField] = useState("min");
    const [selectedStat, setSelectedStat] = useState<PropBetStats | null>(null);
    const [gameOption, setGameOption] = useState<string>('Prop Bet');
    const [roster, setRoster] = useState<Player[]>([]);
    const [usedPlayers, setUsedPlayers] = useState<Player[]>([]);
    const [overUnderLine, setOverUnderLine] = useState<number | string>(0);
    const [selectedOpponent, setSelectedOpponent] = useState({ team_id: '1', team_name: 'All Teams', team_abbreviation: ''});
    const [propBetStats, setPropBetStats] = useState<PropBetStats[]>([]);
    const [playerBoxScores, setPlayerBoxScores] = useState<Stats[]>([]);
    const [careerPlayerBoxScores, setCareerPlayerBoxScores] = useState<Stats[]>([]);

    const [gamesPlayed, setGamesPlayed] = useState<Stats[]>([]);
    const [careerGamesPlayed, setCareerGamesPlayed] = useState<Stats[]>([]);
    const [homeOrVisitor, setHomeOrVisitor] = useState<string>("");
    const [showOpponent, setShowOpponent] = useState(false);
    const [showHomeOrVisitor, setShowHomeOrVisitor] = useState(false);




    const deletePlayer = (player: Player) => {
        const rows = [...roster];
        const newRows = rows.filter(eachPlayer =>
            eachPlayer.player_id !== player.player_id
        )
        setRoster(newRows);
        setUsedPlayers(usedPlayers.filter(eachPlayer => eachPlayer.player_id !== player.player_id))
        setSelectedPlayer(null);
    }

    const deletePropBetStat = (stat: PropBetStats) => {
        const rows = [...propBetStats];
        const newRows = rows.filter(eachStat =>
            eachStat.label !== stat.label
        )
        setPropBetStats(newRows);
    }


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
                <div className="flex">
                    <div className="drop-down">
                        <SearchBar activePlayers={activePlayers} inputText={inputText} setInputText={setInputText} selectedPlayer={selectedPlayer} setSelectedPlayer={setSelectedPlayer} roster={roster} setRoster={setRoster} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                    </div>
                    <div className="drop-down">
                        <PropBetStatsDropDown selectedStat={selectedStat} setSelectedStat={setSelectedStat} propBetStats={propBetStats} setPropBetStats={setPropBetStats} />
                    </div>
                    <div className="drop-down">
                        <OverUnderLineInput overUnderLine={overUnderLine} setOverUnderLine={setOverUnderLine} />
                    </div>
                    <div className="drop-down">
                        <HomeOrVisitorDropDown homeOrVisitor={homeOrVisitor} setHomeOrVisitor={setHomeOrVisitor} showHomeOrVisitor={showHomeOrVisitor} setShowHomeOrVisitor={setShowHomeOrVisitor} />
                    </div>
                    <div className="drop-down" style={{ marginRight: "0px" }}>
                        <PropBetOpponentDropDown selectedOpponent={selectedOpponent} setSelectedOpponent={setSelectedOpponent} showOpponent={showOpponent} setShowOpponent={setShowOpponent} />
                    </div>
                </div>

                <div className="results-flex">
                    <div className="w-100 m-2">
                        <DragNDropRoster roster={roster} setRoster={setRoster} deletePlayer={deletePlayer} />
                    </div>
                    <div className="w-100 m-2">
                        <PropBetStatsDragNDrop propBetStats={propBetStats} setPropBetStats={setPropBetStats} deletePropBetStat={deletePropBetStat} />
                    </div>
                    <div className="w-100 m-2">
                        {overUnderLine ?
                            <OverUnderLine overUnderLine={overUnderLine} setOverUnderLine={setOverUnderLine} />
                            :
                            ""
                        }
                    </div>
                    <div className="w-100 m-2">
                        <PropBetHomeOrVisitor homeOrVisitor={homeOrVisitor} setHomeOrVisitor={setHomeOrVisitor} />
                    </div>
                    <div className="w-100 m-2">
                        <PropBetOpponent selectedOpponent={selectedOpponent} setSelectedOpponent={setSelectedOpponent} showOpponent={showOpponent} setShowOpponent={setShowOpponent} />
                    </div>

                </div>
                <div className="prop-bet-results-wrapper">
                    <PropBetResults careerPlayerBoxScores={careerPlayerBoxScores} setCareerPlayerBoxScores={setCareerPlayerBoxScores} gamesPlayed={gamesPlayed} careerGamesPlayed={careerGamesPlayed} setCareerGamesPlayed={setCareerGamesPlayed} overUnderLine={overUnderLine} propBetStats={propBetStats} selectedOpponent={selectedOpponent} roster={roster} playerBoxScores={playerBoxScores} homeOrVisitor={homeOrVisitor} selectedSeason={selectedSeason} />
                </div>

                <PropBetResultsTable selectedSeason={selectedSeason} overUnderLine={overUnderLine} selectedOpponent={selectedOpponent} roster={roster} propBetStats={propBetStats} setPlayerBoxScores={setPlayerBoxScores} playerBoxScores={playerBoxScores} setGamesPlayed={setGamesPlayed} homeOrVisitor={homeOrVisitor} />
            </div>
            : gameOption === "Shot Charts" ?
            <div>
                <ShotCharts />
            </div>
            :
            ""}
            <DataFlex>

                <div className="drop-down">
                    <LineupPlayerSelect options={lineupPlayerOptions} selectedOption={selectedLineupPlayer} setSelectedOption={setSelectedLineupPlayer} setSortField={setSortField} />
                </div>
                <div className="drop-down">
                    <LineupPlayerSelect options={statOptions} selectedOption={selectedBoxType} setSelectedOption={setSelectedBoxType} setSortField={setSortField} />
                </div>

            </DataFlex>
            <div className="d-flex">
                <div className="drop-down">
                    <SeasonsDropDown
                        selectedSeason={selectedSeason}
                        setSelectedSeason={setSelectedSeason}
                        setSelectedPlayer={() => { }}
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
                        dropDownType="Team"
                    />
                </div>
                <div className="drop-down" style={{marginRight: "0px"}}>
                    <FindPlayerBottom activePlayers={activePlayers} inputTextBottom={inputTextBottom} setInputTextBottom={setInputTextBottom} selectedPlayerBottom={selectedPlayerBottom} setSelectedPlayerBottom={setSelectedPlayerBottom} roster={roster} setRoster={setRoster} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
            </div>
            
            <StatsTable selectedSeason={selectedSeason} selectedLineupPlayer={selectedLineupPlayer} selectedBoxType={selectedBoxType} numPlayers={numPlayers} perMode={perMode} selectedTeam={selectedTeam} sortField={sortField} setSortField={setSortField} inputText={inputTextBottom} setInputText={setInputTextBottom} />
            <Footer />
        </div>
    );
}

export default Home;