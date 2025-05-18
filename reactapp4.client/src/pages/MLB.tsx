import { useEffect, useState } from 'react';
import GameOptionDropDown from '../components/GameOptionDropDown';
import axios from 'axios';
import MLBPropBet from '../components/MLBPropBet';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';
import MLBStatsTable from '../components/MLBStatsTable';
import { MLBTeam } from '../interfaces/Teams';
import { MLBStatsData } from '../hooks/MLBStatsData';
import MLBPitching from '../components/MLBPitching';
import MLBHitting from '../components/MLBHitting';
import { mlbBattingColumns } from '../interfaces/Columns';

// interface MLBProps {
    // selectedSport: string;
    // setSelectedSport: React.Dispatch<React.SetStateAction<string>>
// }

const MLB = ({}) => {

    console.log("MLB Page")

    const [selectedSeason, setSelectedSeason] = useState('2023');
    const [mlbActivePlayers, setMlbActivePlayers] = useState<MLBActivePlayer[]>([]);
    //const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
    const [inputTextBottom, setInputTextBottom] = useState('');
    const [selectedPlayerBottom, setSelectedPlayerBottom] = useState<MLBActivePlayer | null>(null);
    const [selectedTeam, setSelectedTeam] = useState<MLBTeam>({team_id: '1', team_name: 'All MLB Teams'});
    const [sortField, setSortField] = useState<string>("");
    const [gameOption, setGameOption] = useState<string>('Prop Bet');
    const [roster, setRoster] = useState<MLBActivePlayer[]>([]);
    const [usedPlayers, setUsedPlayers] = useState<MLBActivePlayer[]>([]);

    const [hittingPitching, setHittingPitching] = useState<string>("hitting");
    const [leagueOption, setLeagueOption] = useState<string>("MLB");
    const [yearToDateOption, setYearToDateOption] = useState<string>("Year To Date");
    const [selectedOpponent, setSelectedOpponent] = useState<MLBTeam>({team_id: '1', team_name: 'All MLB Teams'})
    const [selectedSplit, setSelectedSplit] = useState<string>("None");
    
    ///const [loading, setLoading] = useState<boolean>(false);

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

// ...
    const setHitting = () => setHittingPitching("hitting");
    const setPitching = () => setHittingPitching("pitching");

    const { statsData, columns, isFetching, originalData } = MLBStatsData({
        selectedSeason, hittingPitching, leagueOption, yearToDateOption, selectedTeam, selectedOpponent, sortField, selectedSplit
    })

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
                <div className={`hitting-pitching ${hittingPitching === "hitting" ? "active" : ""}`} onClick={setHitting}>
                    Hitting
                </div>
                <div className={`hitting-pitching ${hittingPitching === "pitching" ? "active" : ""}`} onClick={setPitching}>
                    Pitching
                </div>
            </div>
            <div style={{ display: hittingPitching === 'hitting' ? 'block' : 'none' }}>
                <MLBHitting
                    selectedSeason={selectedSeason}
                    setSelectedSeason={setSelectedSeason}
                    leagueOption={leagueOption}
                    setLeagueOption={setLeagueOption}
                    selectedTeam={selectedTeam}
                    setSelectedTeam={setSelectedTeam}
                    yearToDateOption={yearToDateOption}
                    setYearToDateOption={setYearToDateOption}
                    selectedOpponent={selectedOpponent}
                    setSelectedOpponent={setSelectedOpponent}
                    selectedSplit={selectedSplit}
                    setSelectedSplit={setSelectedSplit}
                    inputTextBottom={inputTextBottom}
                    setInputTextBottom={setInputTextBottom}
                    selectedPlayerBottom={selectedPlayerBottom}
                    setSelectedPlayerBottom={setSelectedPlayerBottom}
                    activePlayers={mlbActivePlayers}
                    roster={roster}
                    setRoster={setRoster}
                    setUsedPlayers={setUsedPlayers}
                    isFetching={isFetching}
                />
            </div>
            <div style={{ display: hittingPitching === 'pitching' ? 'block' : 'none' }}>
                <MLBPitching
                    selectedSeason={selectedSeason}
                    setSelectedSeason={setSelectedSeason}
                    leagueOption={leagueOption}
                    setLeagueOption={setLeagueOption}
                    selectedTeam={selectedTeam}
                    setSelectedTeam={setSelectedTeam}
                    yearToDateOption={yearToDateOption}
                    setYearToDateOption={setYearToDateOption}
                    selectedOpponent={selectedOpponent}
                    setSelectedOpponent={setSelectedOpponent}
                    selectedSplit={selectedSplit}
                    setSelectedSplit={setSelectedSplit}
                    inputTextBottom={inputTextBottom}
                    setInputTextBottom={setInputTextBottom}
                    selectedPlayerBottom={selectedPlayerBottom}
                    setSelectedPlayerBottom={setSelectedPlayerBottom}
                    activePlayers={mlbActivePlayers}
                    roster={roster}
                    setRoster={setRoster}
                    setUsedPlayers={setUsedPlayers}
                    isFetching={isFetching}
                />
            </div>
            <div>
                <MLBStatsTable
                    inputText={inputTextBottom}
                    statsData={statsData}
                    columns={columns}
                    isFetching={isFetching}
                    originalData={originalData}
                />
            </div>
        </div>
        </>
    );
}

export default MLB;