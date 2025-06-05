import React, { useEffect, useState } from 'react';
import OverUnderLineInput from './OverUnderLineInput';
import HomeOrVisitorDropDown from './HomeOrVisitorDropDown';
import OverUnderLine from './OverUnderLine';
import PropBetHomeOrVisitor from './PropBetHomeOrVisitor';
import { PropBetStats } from '../interfaces/PropBetStats';
import { ShotChartsGamesData } from '../interfaces/Shot';
import { Stats } from '../interfaces/StatsTable';
import MLBSeasonsDropDown from './MLBSeasonsDropDown';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';
import MLBSearchBar from './MLBSearchBar';
import MLBDragNDropRoster from './MLBDragNDropRoster';
import MLBPropBetResults from './MLBPropBetResults';
import MLBPropBetResultsTable from './MLBPropBetResultsTable';
import MLBPropBetStatsDropDown from './MLBPropBetStatsDropDown';
import MLBPropBetOpponentDropDown from './MLBPropBetOpponentDropDown';
import MLBPropBetStatsDragNDrop from './MLBPropBetStatsDragNDrop';
import MLBPropBetOpponent from './MLBPropBetOpponent';
import axios from 'axios';

interface MLBPropBetProps {
    //activePlayers: MLBActivePlayer[];
    roster: MLBActivePlayer[];
    setRoster: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
    usedPlayers: MLBActivePlayer[];
    setUsedPlayers: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
    gameOption: string;
}

const MLBPropBet: React.FC<MLBPropBetProps> = ({ roster, setRoster, usedPlayers, 
    setUsedPlayers, gameOption }) => {

    const [inputText, setInputText] = useState('');
    const [selectedPlayer, setSelectedPlayer] = useState<MLBActivePlayer | null>(null);
    const [propBetStats, setPropBetStats] = useState<PropBetStats[]>([]);
    const [selectedStat, setSelectedStat] = useState<PropBetStats | null>(null);
    const [overUnderLine, setOverUnderLine] = useState<number | string>(0);
    const [homeOrVisitor, setHomeOrVisitor] = useState<string>("");
    const [showHomeOrVisitor, setShowHomeOrVisitor] = useState(false);
    const [showOpponent, setShowOpponent] = useState(false);
    const [selectedOpponent, setSelectedOpponent] = useState({ team_id: '1', team_name: 'All Teams' });
    const [selectedSeasonPropBet, setSelectedSeasonPropBet] = useState('2025');
    const [_selectedGame, setSelectedGame] = useState<ShotChartsGamesData | string>("");
    const [playerBoxScores, setPlayerBoxScores] = useState<Stats[]>([]);
    const [careerPlayerBoxScores, setCareerPlayerBoxScores] = useState<Stats[]>([]);
    const [gamesPlayed, setGamesPlayed] = useState<Stats[]>([]);
    const [careerGamesPlayed, setCareerGamesPlayed] = useState<Stats[]>([]);
    const [hittingPitchingPropBet, setHittingPitchingPropBet] = useState<string>("hitting");
    const [lastTenFilteredBoxScores, setLastTenFilteredBoxScores] = useState<Stats[]>([]);
    const [isFetching, setIsFetching] = useState<boolean>(false);
    const [mlbActivePlayers, setMlbActivePlayers] = useState<MLBActivePlayer[]>([]);

    useEffect(() => {
        async function getData() {
            const activePlayersResponse = await axios.get(`api/MLBActivePlayer/${selectedSeasonPropBet}`);
            const activePlayersData = await activePlayersResponse.data;
            setMlbActivePlayers(activePlayersData);
        }

        getData();
    }, [selectedSeasonPropBet]);

    const deletePlayer = (player: MLBActivePlayer) => {
        const rows = [...roster];
        const newRows = rows.filter(eachPlayer =>
            eachPlayer.playerId !== player.playerId
        )
        setRoster(newRows);
        setUsedPlayers(usedPlayers.filter(eachPlayer => eachPlayer.playerId !== player.playerId))
        setSelectedPlayer(null);
    }

    const deletePropBetStat = (stat: PropBetStats) => {
        const rows = [...propBetStats];
        const newRows = rows.filter(eachStat =>
            eachStat.label !== stat.label
        )
        setPropBetStats(newRows);
    }

    const setHitting = () => {
        setHittingPitchingPropBet("hitting");
        setRoster([]);
        setPropBetStats([]);
        setOverUnderLine(0);
        setHomeOrVisitor("");
        setSelectedOpponent({ team_id: '1', team_name: 'All Teams' });
        //setColumns(mlbBattingColumns);
        //setSplitOptions(mlbSplitsBatting);
    }

    const setPitching = () => {
        setHittingPitchingPropBet("pitching");
        setRoster([]);
        setPropBetStats([]);
        setOverUnderLine(0);
        setHomeOrVisitor("");
        setSelectedOpponent({ team_id: '1', team_name: 'All Teams' });
        //setColumns(mlbPitchingColumns);
        //setSplitOptions(mlbSplitsPitching);
    }

    return (
        <div>
        <br></br>
        <div className='statistics-title'>
            MLB Prop Bet Optimizer
        </div>
        <div className='yellow-line'>
        </div>
        <div className="hitting-pitching-container">
            <div className={`hitting-pitching ${hittingPitchingPropBet === "hitting" ? "active" : ""}`} onClick={setHitting}>
                Hitting
            </div>
            <div className={`hitting-pitching ${hittingPitchingPropBet === "pitching" ? "active" : ""}`} onClick={setPitching}>
                Pitching
            </div>
        </div>
        <div className="prop-bet-container">
            <div className="flex">
                <div className="drop-down">
                    <MLBSearchBar activePlayers={mlbActivePlayers} inputText={inputText} setInputText={setInputText} 
                        selectedPlayer={selectedPlayer} setSelectedPlayer={setSelectedPlayer} roster={roster} 
                        setRoster={setRoster} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
                <div className="drop-down">
                    <MLBSeasonsDropDown selectedSeason={selectedSeasonPropBet} setSelectedSeason={setSelectedSeasonPropBet} 
                        isPredictions={false} disabled={isFetching} />
                </div>
                <div className="drop-down">
                    <MLBPropBetStatsDropDown selectedStat={selectedStat} setSelectedStat={setSelectedStat} propBetStats={propBetStats} 
                        setPropBetStats={setPropBetStats} hittingPitching={hittingPitchingPropBet} disabled={isFetching} />
                </div>
                <div className="drop-down">
                    <OverUnderLineInput overUnderLine={overUnderLine} setOverUnderLine={setOverUnderLine} />
                </div>
                <div className="drop-down">
                    <HomeOrVisitorDropDown homeOrVisitor={homeOrVisitor} setHomeOrVisitor={setHomeOrVisitor} showHomeOrVisitor={showHomeOrVisitor} setShowHomeOrVisitor={setShowHomeOrVisitor} disabled={isFetching} />
                </div>
                <div className="drop-down" style={{ marginRight: "0px" }}>
                    <MLBPropBetOpponentDropDown selectedOpponent={selectedOpponent} setSelectedOpponent={setSelectedOpponent} showOpponent={showOpponent} setShowOpponent={setShowOpponent} disabled={isFetching} />
                </div>
            </div>

            <div className="results-flex">
                <div className="results-flex-item">
                    {roster.length > 0 ?
                        <MLBDragNDropRoster roster={roster} setRoster={setRoster} deletePlayer={deletePlayer} />
                        :
                        <div className="prop-bet-placeholders">
                            Select a Player
                        </div>
                    }
                </div>
                <div className="results-flex-item">
                    {propBetStats.length > 0 ?
                        <MLBPropBetStatsDragNDrop propBetStats={propBetStats} setPropBetStats={setPropBetStats} deletePropBetStat={deletePropBetStat} />
                        :
                        <div className="prop-bet-placeholders">
                            Select Prop
                        </div>
                    }
                </div>
                <div className="results-flex-item">
                    {overUnderLine ?
                        <OverUnderLine overUnderLine={overUnderLine} setOverUnderLine={setOverUnderLine} />
                        :
                        <div className="prop-bet-placeholders">
                            Over/Under
                        </div>
                    }
                </div>
                <div className="results-flex-item">
                    {homeOrVisitor !== "" ?
                        <PropBetHomeOrVisitor homeOrVisitor={homeOrVisitor} setHomeOrVisitor={setHomeOrVisitor} />
                        :
                        <div className="prop-bet-placeholders">
                            All Games
                        </div>
                    }
                </div>
                <div className="results-flex-item" style={{ marginRight: '0px' } }>
                    {selectedOpponent.team_id === '1' ?
                        <div className="prop-bet-placeholders">
                            All Opponents
                        </div>
                        :
                        <MLBPropBetOpponent selectedOpponent={selectedOpponent} setSelectedOpponent={setSelectedOpponent} 
                        showOpponent={showOpponent} setShowOpponent={setShowOpponent} />
                    }
                </div>

            </div>
            
        </div>
        <div className="prop-bet-results-wrapper">
            <MLBPropBetResults careerPlayerBoxScores={careerPlayerBoxScores} setCareerPlayerBoxScores={setCareerPlayerBoxScores} 
                gamesPlayed={gamesPlayed} careerGamesPlayed={careerGamesPlayed} setCareerGamesPlayed={setCareerGamesPlayed} 
                overUnderLine={overUnderLine} propBetStats={propBetStats} selectedOpponent={selectedOpponent} roster={roster} 
                playerBoxScores={playerBoxScores} homeOrVisitor={homeOrVisitor} selectedSeason={selectedSeasonPropBet} 
                hittingPitching={hittingPitchingPropBet} lastTenFilteredBoxScores={lastTenFilteredBoxScores} setIsFetching={setIsFetching}/>
        </div>
            <MLBPropBetResultsTable selectedSeason={selectedSeasonPropBet} overUnderLine={overUnderLine} 
                selectedOpponent={selectedOpponent} roster={roster} propBetStats={propBetStats} 
                setPlayerBoxScores={setPlayerBoxScores} playerBoxScores={playerBoxScores} gamesPlayed={gamesPlayed} 
                setGamesPlayed={setGamesPlayed} homeOrVisitor={homeOrVisitor} hittingPitching={hittingPitchingPropBet} 
                setLastTenFilteredBoxScores={setLastTenFilteredBoxScores} setIsFetching={setIsFetching} />
    </div>
    );
}

export default MLBPropBet;