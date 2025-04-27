import { useEffect, useState } from 'react';
import SeasonsDropDown from '../components/SeasonsDropDown';
import StatsTable from '../components/StatsTable';
import GameOptionDropDown from '../components/GameOptionDropDown';
import { americanLeagueTeams, mlbLeagueOptions, mlbTeams, nationalLeagueTeams, yearToDateOptions } from '../interfaces/MLBDropDownOptions';

import DropDown from '../components/DropDown';
import axios from 'axios';
import MLBPropBet from '../components/MLBPropBet';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';
import MLBFindPlayerBottom from '../components/MLBFindPlayerBottom';
import MLBSeasonsDropDown from '../components/MLBSeasonsDropDown';
import MLBDropDown from '../components/MLBDropDown';
import MLBStatsTable from '../components/MLBStatsTable';
import MLBLeagueOptionDropDown from '../components/MLBLeagueOptionDropDown';
import MLBSelectedTeamDropDown from '../components/MLBSelectedTeamDropDown';
import MLBYearToDateDropDown from '../components/MLBYearToDateDropDown';
import { MLBTeam } from '../interfaces/Teams';
import MLBOpponentDropDown from '../components/MLBOpponentDropDown';
import MLBSplitsDropDown from '../components/MLBSplitsDropDown';

interface MLBProps {
    selectedSport: string;
    setSelectedSport: React.Dispatch<React.SetStateAction<string>>
}

const MLB: React.FC<MLBProps> = ({ selectedSport, setSelectedSport }) => {

    console.log("MLB Page")

    const [selectedSeason, setSelectedSeason] = useState('2023');
    const [mlbActivePlayers, setMlbActivePlayers] = useState<MLBActivePlayer[]>([]);
    //const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
    const [inputTextBottom, setInputTextBottom] = useState('');
    const [selectedPlayerBottom, setSelectedPlayerBottom] = useState<MLBActivePlayer | null>(null);
    const [selectedTeam, setSelectedTeam] = useState<MLBTeam>({team_id: '1', team_name: 'All MLB Teams'});
    const [sortField, setSortField] = useState("AtBats");
    //const [selectedStat, setSelectedStat] = useState<PropBetStats | null>(null);
    const [gameOption, setGameOption] = useState<string>('Prop Bet');
    const [roster, setRoster] = useState<MLBActivePlayer[]>([]);
    const [usedPlayers, setUsedPlayers] = useState<MLBActivePlayer[]>([]);
    //const [overUnderLine, setOverUnderLine] = useState<number | string>(0);
    //const [propBetStats, setPropBetStats] = useState<PropBetStats[]>([]);
    //const [playerBoxScores, setPlayerBoxScores] = useState<Stats[]>([]);
    //const [careerPlayerBoxScores, setCareerPlayerBoxScores] = useState<Stats[]>([]);

    //const [gamesPlayed, setGamesPlayed] = useState<Stats[]>([]);
    //const [careerGamesPlayed, setCareerGamesPlayed] = useState<Stats[]>([]);
    //const [homeOrVisitor, setHomeOrVisitor] = useState<string>("");
    //const [showOpponent, setShowOpponent] = useState(false);
    //const [showHomeOrVisitor, setShowHomeOrVisitor] = useState(false);
    const [hittingPitching, setHittingPitching] = useState<string>("hitting");
    const [leagueOption, setLeagueOption] = useState<string>("MLB");
    const [yearToDateOption, setYearToDateOption] = useState<string>("Year To Date");
    const [selectedOpponent, setSelectedOpponent] = useState<MLBTeam>({team_id: '1', team_name: 'All MLB Teams'})
    const [selectedSplit, setSelectedSplit] = useState<string>("None");
    console.log(selectedSport);

    //useEffect(() => {
    //    console.log("active players hook")
    //    async function getPlayersBySeason() {
    //        const season = "2023";
    //        const response = await axios.get(`api/MLBPlayersBySeason/${season}`);
    //        const data = await response.data;
    //        console.log(data)
    //        setMlbActivePlayers(data);
    //    }
//
    //    getPlayersBySeason();
    //}, []);

    useEffect(() => {
        console.log("active players hook")
        async function getData() {
            const season = "2023";
            const activePlayersResponse = await axios.get(`api/MLBActivePlayer/${season}`);
            const activePlayersData = await activePlayersResponse.data;
            console.log(activePlayersData)
            setMlbActivePlayers(activePlayersData);
        }

        getData();
    }, []);
//<StatsTable selectedSeason={selectedSeason} selectedLineupPlayer={selectedLineupPlayer} selectedBoxType={selectedBoxType} numPlayers={numPlayers} perMode={perMode} selectedTeam={selectedTeam} sortField={sortField} setSortField={setSortField} inputText={inputTextBottom} setInputText={setInputTextBottom} selectedOpponent={selectedOpponent} />

    return (
        <>
        <div className='logo-flex'>
            <div>
                <img src="/images/ball7.png" className="ball" alt="Home"/>
            </div>
            <div>
                <GameOptionDropDown gameOption={gameOption} setGameOption={setGameOption} />
            </div>
        </div>

        <div className="home-container">

            {gameOption === "Prop Bet" ?
                <div>
                    <MLBPropBet activePlayers={mlbActivePlayers} roster={roster} setRoster={setRoster} usedPlayers={usedPlayers} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
            :
            ""}
            <div className="hitting-pitching-container">
                <div onClick={()=> setHittingPitching("hitting")}>
                    Hitting
                </div>
                <div onClick={() => setHittingPitching("pitching")}>
                    Pitching
                </div>
            </div>
            
        {hittingPitching === 'hitting' ?
            
            <div className="display-flex">
                <div className="drop-down">
                    <MLBSeasonsDropDown
                        selectedSeason={selectedSeason}
                        setSelectedSeason={setSelectedSeason}
                        isPredictions={false}
                    />
                </div>
                <div className="drop-down">
                    <MLBLeagueOptionDropDown
                        options={mlbLeagueOptions}
                        leagueOption={leagueOption}
                        setLeagueOption={setLeagueOption}
                        setSelectedTeam={setSelectedTeam}
                    />
                </div>
                <div className="drop-down">
                    <MLBSelectedTeamDropDown
                        options={leagueOption === "National League" ? nationalLeagueTeams :
                            leagueOption === "American League" ? americanLeagueTeams :
                            leagueOption === "MLB" ? mlbTeams :
                            mlbTeams
                        }
                        selectedTeam={selectedTeam}
                        setSelectedTeam={setSelectedTeam}

                    />
                </div>
                <div className="drop-down">
                    <MLBYearToDateDropDown
                        options={yearToDateOptions}
                        yearToDateOption={yearToDateOption}
                        setYearToDateOption={setYearToDateOption}
                    />
                </div>
                <div className="drop-down">
                    <MLBOpponentDropDown
                        options={mlbTeams}
                        selectedOpponent={selectedOpponent}
                        setSelectedOpponent={setSelectedOpponent}
                    />
                </div>
                <div className="drop-down">
                    <MLBSplitsDropDown
                        selectedSplit={selectedSplit}
                        setSelectedSplit={setSelectedSplit}
                    />
                </div>
                <div className="drop-down">
                    <MLBFindPlayerBottom activePlayers={mlbActivePlayers} inputTextBottom={inputTextBottom} setInputTextBottom={setInputTextBottom} selectedPlayerBottom={selectedPlayerBottom} setSelectedPlayerBottom={setSelectedPlayerBottom} roster={roster} setRoster={setRoster} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
            </div>
        :
        hittingPitching === 'pitching' ? 

            <div className="display-flex">
                <div className="drop-down">
                    <MLBSeasonsDropDown
                        selectedSeason={selectedSeason}
                        setSelectedSeason={setSelectedSeason}
                        isPredictions={false}
                    />
                </div>
                <div className="drop-down">
                    <MLBLeagueOptionDropDown
                        options={mlbLeagueOptions}
                        leagueOption={leagueOption}
                        setLeagueOption={setLeagueOption}
                        setSelectedTeam={setSelectedTeam}
                    />
                </div>
                <div className="drop-down">
                    <MLBSelectedTeamDropDown
                        options={leagueOption === "National League" ? nationalLeagueTeams :
                            leagueOption === "American League" ? americanLeagueTeams :
                            leagueOption === "MLB" ? mlbTeams :
                            mlbTeams
                        }
                        selectedTeam={selectedTeam}
                        setSelectedTeam={setSelectedTeam}

                    />
                </div>
                <div className="drop-down">
                    <MLBYearToDateDropDown
                        options={yearToDateOptions}
                        yearToDateOption={yearToDateOption}
                        setYearToDateOption={setYearToDateOption}
                    />
                </div>
                <div className="drop-down">
                    <MLBFindPlayerBottom activePlayers={mlbActivePlayers} inputTextBottom={inputTextBottom} setInputTextBottom={setInputTextBottom} selectedPlayerBottom={selectedPlayerBottom} setSelectedPlayerBottom={setSelectedPlayerBottom} roster={roster} setRoster={setRoster} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
            </div>
        :
        
        ""}
            <div>
                <MLBStatsTable selectedSeason={selectedSeason} hittingPitching={hittingPitching} leagueOption={leagueOption}
                    yearToDateOption={yearToDateOption} selectedTeam={selectedTeam} setSelectedTeam={setSelectedTeam} selectedOpponent={selectedOpponent} sortField={sortField} setSortField={setSortField} 
                    selectedSplit={selectedSplit} inputText={inputTextBottom} setInputText={setInputTextBottom} />
            </div>
        </div>
        </>
    );
}

export default MLB;