import { useEffect, useState } from 'react';
import SeasonsDropDown from '../components/SeasonsDropDown';
import StatsTable from '../components/StatsTable';
import GameOptionDropDown from '../components/GameOptionDropDown';
import { mlbLeagueOptions, mlbTeams, yearToDateOptions } from '../interfaces/MLBDropDownOptions';

import DropDown from '../components/DropDown';
import axios from 'axios';
import MLBPropBet from '../components/MLBPropBet';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';
import MLBFindPlayerBottom from '../components/MLBFindPlayerBottom';
import MLBSeasonsDropDown from '../components/MLBSeasonsDropDown';
import MLBDropDown from '../components/MLBDropDown';

interface MLBProps {
    selectedSport: string;
    setSelectedSport: React.Dispatch<React.SetStateAction<string>>
}

const MLB: React.FC<MLBProps> = ({ selectedSport, setSelectedSport }) => {

    const [selectedSeason, setSelectedSeason] = useState('2023');
    const [mlbActivePlayers, setMlbActivePlayers] = useState<MLBActivePlayer[]>([]);
    //const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
    const [inputTextBottom, setInputTextBottom] = useState('');
    const [selectedPlayerBottom, setSelectedPlayerBottom] = useState<MLBActivePlayer | null>(null);
    const [selectedLineupPlayer, setSelectedLineupPlayer] = useState('Players');
    const [selectedBoxType, setSelectedBoxType] = useState('Base');
    const [numPlayers, setNumPlayers] = useState('5');
    const [perMode, setPerMode] = useState('Totals');
    const [selectedTeam, setSelectedTeam] = useState({team_id: '0', team_name: 'All Teams'});
    const [sortField, setSortField] = useState("AtBats");
    //const [selectedStat, setSelectedStat] = useState<PropBetStats | null>(null);
    const [gameOption, setGameOption] = useState<string>('Prop Bet');
    const [roster, setRoster] = useState<MLBActivePlayer[]>([]);
    const [usedPlayers, setUsedPlayers] = useState<MLBActivePlayer[]>([]);
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
    const [hittingPitching, setHittingPitching] = useState<string>("hitting");
    const [leagueOption, setLeagueOption] = useState<string>("MLB");
    const [yearToDateOption, setYearToDateOption] = useState<string>("Year To Date");
    console.log(selectedSport);




    useEffect(() => {
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

            <div className="display-flex">
                <div className="drop-down">
                    <MLBSeasonsDropDown
                        selectedSeason={selectedSeason}
                        setSelectedSeason={setSelectedSeason}
                        isPredictions={false}
                    />
                </div>
                <div className="drop-down">
                    <MLBDropDown
                        options={mlbLeagueOptions}
                        setLeagueOption={setLeagueOption}
                        setSelectedTeam={setSelectedTeam}
                        setYearToDateOption={setYearToDateOption}
                        dropDownType="League Options"
                    />
                </div>
                <div className="drop-down">
                    <MLBDropDown
                        options={mlbTeams}
                        setLeagueOption={setLeagueOption}
                        setSelectedTeam={setSelectedTeam}
                        setYearToDateOption={setYearToDateOption}
                        dropDownType="Team"
                    />
                </div>
                <div className="drop-down">
                    <MLBDropDown
                        options={yearToDateOptions}
                        setLeagueOption={setLeagueOption}
                        setSelectedTeam={setSelectedTeam}
                        setYearToDateOption={setYearToDateOption}
                        dropDownType="Year To Date"
                    />
                </div>
                <div className="drop-down">
                    <MLBFindPlayerBottom activePlayers={mlbActivePlayers} inputTextBottom={inputTextBottom} setInputTextBottom={setInputTextBottom} selectedPlayerBottom={selectedPlayerBottom} setSelectedPlayerBottom={setSelectedPlayerBottom} roster={roster} setRoster={setRoster} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
            </div>
            <div>
                <MLBStatsTable selectedSeason={selectedSeason} selectedLineupPlayer={selectedLineupPlayer} selectedBoxType={selectedBoxType} numPlayers={numPlayers} perMode={perMode} selectedTeam={selectedTeam} sortField={sortField} setSortField={setSortField} inputText={inputTextBottom} setInputText={setInputTextBottom} selectedOpponent={selectedOpponent} />
            </div>
        </div>
        </>
    );
}

export default MLB;