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
import SportOptionDropDown from '../components/SportOptionDropDown';
import Header from '../components/Header';

interface MLBProps {
    selectedSport: string;
    setSelectedSport: React.Dispatch<React.SetStateAction<string>>
}

const MLB: React.FC<MLBProps> = ({ selectedSport, setSelectedSport}) => {

    const [selectedSeason, setSelectedSeason] = useState('2025');
    const [mlbActivePlayers, setMlbActivePlayers] = useState<MLBActivePlayer[]>([]);
    //const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
    const [inputTextBottom, setInputTextBottom] = useState('');
    const [selectedPlayerBottom, setSelectedPlayerBottom] = useState<MLBActivePlayer | null>(null);
    const [inputTextOpponent, setInputTextOpponent] = useState('');
    const [selectedPlayerOpponent, setSelectedPlayerOpponent] = useState<MLBActivePlayer | null>(null);
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
    //const [columns, setColumns] = useState<Column[]>(mlbBattingColumns);
    //const [splitOptions, setSplitOptions] = useState<string[]>(mlbSplitsBatting);

    useEffect(() => {
        async function getData() {
            const activePlayersResponse = await axios.get(`api/MLBActivePlayer/${selectedSeason}`);
            const activePlayersData = await activePlayersResponse.data;
            setMlbActivePlayers(activePlayersData);
        }

        getData();
    }, [selectedSeason]);
//<StatsTable selectedSeason={selectedSeason} selectedLineupPlayer={selectedLineupPlayer} selectedBoxType={selectedBoxType} numPlayers={numPlayers} perMode={perMode} selectedTeam={selectedTeam} sortField={sortField} setSortField={setSortField} inputText={inputTextBottom} setInputText={setInputTextBottom} selectedOpponent={selectedOpponent} />

// ...
    const setHitting = () => {
        setHittingPitching("hitting");
        //setColumns(mlbBattingColumns);
        //setSplitOptions(mlbSplitsBatting);
    }

    const setPitching = () => {
        setHittingPitching("pitching");
        //setColumns(mlbPitchingColumns);
        //setSplitOptions(mlbSplitsPitching);
    }

    const { statsData, isFetching, columns, originalData } = MLBStatsData({
        selectedSeason, hittingPitching, leagueOption, yearToDateOption, selectedTeam, 
        selectedOpponent, sortField, selectedSplit, selectedPlayerOpponent
    })

    return (
        <>
        <Header selectedSport={selectedSport} setSelectedSport={setSelectedSport} gameOption={gameOption} setGameOption={setGameOption} />

        <div className="home-container">

            {gameOption === "Prop Bet" ?
                <div>
                    <MLBPropBet roster={roster} setRoster={setRoster} 
                        usedPlayers={usedPlayers} setUsedPlayers={setUsedPlayers} gameOption={gameOption}
                     />
                </div>
            :
            ""}
            <div className="margin-top">
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
                        inputTextOpponent={inputTextOpponent}
                        setInputTextOpponent={setInputTextOpponent}
                        selectedPlayerOpponent={selectedPlayerOpponent}
                        setSelectedPlayerOpponent={setSelectedPlayerOpponent}
                        activePlayers={mlbActivePlayers}
                        roster={roster}
                        setRoster={setRoster}
                        setUsedPlayers={setUsedPlayers}
                        isFetching={isFetching}
                        //columns={columns}
                        //splitOptions={splitOptions}
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
                        inputTextOpponent={inputTextOpponent}
                        setInputTextOpponent={setInputTextOpponent}
                        selectedPlayerOpponent={selectedPlayerOpponent}
                        setSelectedPlayerOpponent={setSelectedPlayerOpponent}
                        activePlayers={mlbActivePlayers}
                        roster={roster}
                        setRoster={setRoster}
                        setUsedPlayers={setUsedPlayers}
                        isFetching={isFetching}
                        //columns={columns}
                        //splitOptions={splitOptions}
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
        </div>
        </>
    );
}

export default MLB;