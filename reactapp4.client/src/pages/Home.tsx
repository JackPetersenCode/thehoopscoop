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

const DataFlex = styled.div`
    display: flex;
    align-items: center;
    white-space: nowrap;
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
    const [selectedOption, setSelectedOption] = useState<string | NBATeam>("");


    const lineupPlayerOptions = [
        'Lineups',
        'Players'
    ];

    const statOptions = [
        'Traditional',
        'Advanced',
        'FourFactors',
        'Misc',
        'Scoring',
        'Opponent'
    ];

    const perModeOptions = [
        'Per Game',
        'Per 100 Poss',
        'Per Minute',
        'Totals'
    ];

    const numPlayersOptions = [
        '2',
        '3',
        '4',
        '5'
    ];

    const homeOrVisitorOptions = [
        'All Games',
        'Home',
        'Visitor'
    ];

    const nbaTeams: NBATeam[] = [
        { team_id: "1", team_name: "All Teams", team_abbreviation: '' },
        { team_id: "1610612744", team_name: "Golden State Warriors", team_abbreviation: "GSW" },
        { team_id: "1610612757", team_name: "Portland Trail Blazers", team_abbreviation: "POR" },
        { team_id: "1610612751", team_name: "Brooklyn Nets", team_abbreviation: "BKN" },
        { team_id: "1610612748", team_name: "Miami Heat", team_abbreviation: "MIA" },
        { team_id: "1610612747", team_name: "Los Angeles Lakers", team_abbreviation: "LAL" },
        { team_id: "1610612761", team_name: "Toronto Raptors", team_abbreviation: "TOR" },
        { team_id: "1610612738", team_name: "Boston Celtics", team_abbreviation: "BOS" },
        { team_id: "1610612752", team_name: "New York Knicks", team_abbreviation: "NYK" },
        { team_id: "1610612762", team_name: "Utah Jazz", team_abbreviation: "UTA" },
        { team_id: "1610612756", team_name: "Phoenix Suns", team_abbreviation: "PHX" },
        { team_id: "1610612753", team_name: "Orlando Magic", team_abbreviation: "ORL" },
        { team_id: "1610612749", team_name: "Milwaukee Bucks", team_abbreviation: "MIL" },
        { team_id: "1610612741", team_name: "Chicago Bulls", team_abbreviation: "CHI" },
        { team_id: "1610612745", team_name: "Houston Rockets", team_abbreviation: "HOU" },
        { team_id: "1610612737", team_name: "Atlanta Hawks", team_abbreviation: "ATL" },
        { team_id: "1610612754", team_name: "Indiana Pacers", team_abbreviation: "IND" },
        { team_id: "1610612759", team_name: "San Antonio Spurs", team_abbreviation: "SAS" },
        { team_id: "1610612765", team_name: "Detroit Pistons", team_abbreviation: "DET" },
        { team_id: "1610612763", team_name: "Memphis Grizzlies", team_abbreviation: "MEM" },
        { team_id: "1610612739", team_name: "Cleveland Cavaliers", team_abbreviation: "CLE" },
        { team_id: "1610612758", team_name: "Sacramento Kings", team_abbreviation: "SAC" },
        { team_id: "1610612743", team_name: "Denver Nuggets", team_abbreviation: "DEN" },
        { team_id: "1610612742", team_name: "Dallas Mavericks", team_abbreviation: "DAL" },
        { team_id: "1610612766", team_name: "Charlotte Hornets", team_abbreviation: "CHA" },
        { team_id: "1610612750", team_name: "Minnesota Timberwolves", team_abbreviation: "MIN" },
        { team_id: "1610612740", team_name: "New Orleans Pelicans", team_abbreviation: "NOP" },
        { team_id: "1610612764", team_name: "Washington Wizards", team_abbreviation: "WAS" },
        { team_id: "1610612755", team_name: "Philadelphia 76ers", team_abbreviation: "PHI" },
        { team_id: "1610612746", team_name: "Los Angeles Clippers", team_abbreviation: "LAC" },
        { team_id: "1610612760", team_name: "Oklahoma City Thunder", team_abbreviation: "OKC" },
    ];


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
            <h1>NBA STAT BLASTER</h1>

            <GameOptionDropDown gameOption={gameOption} setGameOption={setGameOption} />

            
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
                    <DropDown
                        options={homeOrVisitorOptions}
                        perMode={perMode}
                        setPerMode={setPerMode}
                        numPlayers={numPlayers}
                        setNumPlayers={setNumPlayers}
                        selectedTeam={selectedTeam}
                        setSelectedTeam={setSelectedTeam}
                        dropDownType="Home or Visitor"
                        selectedOpponent={selectedOpponent}
                        setSelectedOpponent={setSelectedOpponent}
                        homeOrVisitor={homeOrVisitor}
                        setHomeOrVisitor={setHomeOrVisitor}
                        showOpponent={showOpponent}
                        setShowOpponent={setShowOpponent}
                        selectedOption={selectedOption}
                        setSelectedOption={setSelectedOption}

                    />
                </div>
                <div className="drop-down">
                    <DropDown
                        options={nbaTeams}
                        perMode={perMode}
                        setPerMode={setPerMode}
                        numPlayers={numPlayers}
                        setNumPlayers={setNumPlayers}
                        selectedTeam={selectedTeam}
                        setSelectedTeam={setSelectedTeam}
                        dropDownType="Opponent"
                        selectedOpponent={selectedOpponent}
                        setSelectedOpponent={setSelectedOpponent}
                        homeOrVisitor={homeOrVisitor}
                        setHomeOrVisitor={setHomeOrVisitor}
                        showOpponent={showOpponent}
                        setShowOpponent={setShowOpponent}
                        selectedOption={selectedOption}
                        setSelectedOption={setSelectedOption}

                    />
                </div>
            </div>
            <div className="results-flex">
                <div>
                    <DragNDropRoster roster={roster} setRoster={setRoster} deletePlayer={deletePlayer} />
                </div>
                <div>
                    <PropBetStatsDragNDrop propBetStats={propBetStats} setPropBetStats={setPropBetStats} deletePropBetStat={deletePropBetStat} />
                </div>
                <div>
                    {overUnderLine ?
                        <OverUnderLine overUnderLine={overUnderLine} setOverUnderLine={setOverUnderLine} />
                        :
                        ""
                    }
                </div>
                <div>
                    <PropBetHomeOrVisitor homeOrVisitor={homeOrVisitor} setHomeOrVisitor={setHomeOrVisitor} />
                </div>
                <div>
                    <PropBetOpponent selectedOpponent={selectedOpponent} setSelectedOpponent={setSelectedOpponent} showOpponent={showOpponent} setShowOpponent={setShowOpponent} dropDownType="Opponent" />
                </div>

            </div>
            <PropBetResults careerPlayerBoxScores={careerPlayerBoxScores} setCareerPlayerBoxScores={setCareerPlayerBoxScores} gamesPlayed={gamesPlayed} careerGamesPlayed={careerGamesPlayed} setCareerGamesPlayed={setCareerGamesPlayed} overUnderLine={overUnderLine} propBetStats={propBetStats} selectedOpponent={selectedOpponent} roster={roster} playerBoxScores={playerBoxScores} homeOrVisitor={homeOrVisitor} />

            <PropBetResultsTable selectedSeason={selectedSeason} overUnderLine={overUnderLine} selectedOpponent={selectedOpponent} roster={roster} propBetStats={propBetStats} setPlayerBoxScores={setPlayerBoxScores} playerBoxScores={playerBoxScores} setGamesPlayed={setGamesPlayed} homeOrVisitor={homeOrVisitor} />
            <DataFlex>

                <div>
                    <LineupPlayerSelect options={lineupPlayerOptions} selectedOption={selectedLineupPlayer} setSelectedOption={setSelectedLineupPlayer} setSortField={setSortField} />
                </div>
                <div>
                    <LineupPlayerSelect options={statOptions} selectedOption={selectedBoxType} setSelectedOption={setSelectedBoxType} setSortField={setSortField} />
                </div>

            </DataFlex>
            <div className="d-flex">
                <div>
                <SeasonsDropDown
                    selectedSeason={selectedSeason}
                    setSelectedSeason={setSelectedSeason}
                    predictions={false}
                    />
                </div>
                {selectedBoxType === "Base" || selectedBoxType === "Misc" || selectedBoxType === "Opponent" ?
                    <div>
                    <DropDown
                            options={perModeOptions}
                            perMode={perMode}
                            setPerMode={setPerMode}
                            numPlayers={numPlayers}
                            setNumPlayers={setNumPlayers}
                            selectedTeam={selectedTeam}
                            setSelectedTeam={setSelectedTeam}
                            dropDownType="Per Mode"
                            selectedOpponent={selectedOpponent}
                            setSelectedOpponent={setSelectedOpponent}
                            homeOrVisitor={homeOrVisitor}
                            setHomeOrVisitor={setHomeOrVisitor}
                            showOpponent={showOpponent}
                            setShowOpponent={setShowOpponent}
                            selectedOption={selectedOption}
                            setSelectedOption={setSelectedOption}

                    />
                    </div>
                    :
                    ""}
                {selectedLineupPlayer === "Lineups" ?
                    <div>
                    <DropDown
                            options={numPlayersOptions}
                            perMode={perMode}
                            setPerMode={setPerMode}
                            numPlayers={numPlayers}
                            setNumPlayers={setNumPlayers}
                            selectedTeam={selectedTeam}
                            setSelectedTeam={setSelectedTeam}
                            dropDownType="# of Players"
                            selectedOpponent={selectedOpponent}
                            setSelectedOpponent={setSelectedOpponent}
                            homeOrVisitor={homeOrVisitor}
                            setHomeOrVisitor={setHomeOrVisitor}
                            showOpponent={showOpponent}
                            setShowOpponent={setShowOpponent}
                            selectedOption={selectedOption}
                            setSelectedOption={setSelectedOption}

                    />
                    </div>
                    :
                    ""}
                <div>
                <DropDown
                        options={nbaTeams}
                        perMode={perMode}
                        setPerMode={setPerMode}
                        numPlayers={numPlayers}
                        setNumPlayers={setNumPlayers}
                        selectedTeam={selectedTeam}
                        setSelectedTeam={setSelectedTeam}
                        dropDownType="Team"
                        selectedOpponent={selectedOpponent}
                        setSelectedOpponent={setSelectedOpponent}
                        homeOrVisitor={homeOrVisitor}
                        setHomeOrVisitor={setHomeOrVisitor}
                        showOpponent={showOpponent}
                        setShowOpponent={setShowOpponent}
                        selectedOption={selectedOption}
                        setSelectedOption={setSelectedOption}
                        

                    />
                </div>
                <div>
                    <FindPlayerBottom activePlayers={activePlayers} inputTextBottom={inputTextBottom} setInputTextBottom={setInputTextBottom} selectedPlayerBottom={selectedPlayerBottom} setSelectedPlayerBottom={setSelectedPlayerBottom} roster={roster} setRoster={setRoster} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
            </div>
            
            <StatsTable selectedSeason={selectedSeason} selectedLineupPlayer={selectedLineupPlayer} selectedBoxType={selectedBoxType} numPlayers={numPlayers} perMode={perMode} selectedTeam={selectedTeam} sortField={sortField} setSortField={setSortField} inputText={inputTextBottom} setInputText={setInputTextBottom} />

        </div>
    );
}

export default Home;