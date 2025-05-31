import React, { useState } from 'react';
import SearchBar from './SearchBar';
import PropBetStatsDropDown from './PropBetStatsDropDown';
import OverUnderLineInput from './OverUnderLineInput';
import HomeOrVisitorDropDown from './HomeOrVisitorDropDown';
import PropBetOpponentDropDown from './PropBetOpponentDropDown';
import DragNDropRoster from './DragNDropRoster';
import PropBetStatsDragNDrop from './PropBetStatsDragNDrop';
import OverUnderLine from './OverUnderLine';
import PropBetHomeOrVisitor from './PropBetHomeOrVisitor';
import PropBetOpponent from './PropBetOpponent';
import PropBetResults from './PropBetResults';
import PropBetResultsTable from './PropBetResultsTable';
import SeasonsDropDown from './SeasonsDropDown';
import { Player } from '../interfaces/Player';
import { PropBetStats } from '../interfaces/PropBetStats';
import { ShotChartsGamesData } from '../interfaces/Shot';
import { Stats } from '../interfaces/StatsTable';

interface PropBetProps {
    activePlayers: Player[];
    roster: Player[];
    setRoster: React.Dispatch<React.SetStateAction<Player[]>>;
    usedPlayers: Player[];
    setUsedPlayers: React.Dispatch<React.SetStateAction<Player[]>>;
    gameOption: string;
}

const PropBet: React.FC<PropBetProps> = ({ activePlayers, roster, setRoster, usedPlayers, setUsedPlayers, gameOption }) => {

    const [inputText, setInputText] = useState('');
    const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
    const [propBetStats, setPropBetStats] = useState<PropBetStats[]>([]);
    const [selectedStat, setSelectedStat] = useState<PropBetStats | null>(null);
    const [overUnderLine, setOverUnderLine] = useState<number | string>(0);
    const [homeOrVisitor, setHomeOrVisitor] = useState<string>("");
    const [showHomeOrVisitor, setShowHomeOrVisitor] = useState(false);
    const [showOpponent, setShowOpponent] = useState(false);
    const [selectedOpponent, setSelectedOpponent] = useState({ team_id: '1', team_name: 'All Teams', team_abbreviation: '' });
    const [selectedSeasonPropBet, setSelectedSeasonPropBet] = useState('2024_25');
    const [_selectedGame, setSelectedGame] = useState<ShotChartsGamesData | string>("");
    const [playerBoxScores, setPlayerBoxScores] = useState<Stats[]>([]);
    const [careerPlayerBoxScores, setCareerPlayerBoxScores] = useState<Stats[]>([]);
    const [gamesPlayed, setGamesPlayed] = useState<Stats[]>([]);
    const [careerGamesPlayed, setCareerGamesPlayed] = useState<Stats[]>([]);
    const [lastTenFilteredBoxScores, setLastTenFilteredBoxScores] = useState<Stats[]>([]);

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


    return (
        <div>
        <br></br>
        <div className='statistics-title'>
            NBA Prop Bet Optimizer
        </div>
        <div className='yellow-line'>
        </div>
        <div className="prop-bet-container">
            <div className="flex">
                <div className="drop-down">
                    <SearchBar activePlayers={activePlayers} inputText={inputText} setInputText={setInputText} selectedPlayer={selectedPlayer} setSelectedPlayer={setSelectedPlayer} roster={roster} setRoster={setRoster} setUsedPlayers={setUsedPlayers} gameOption={gameOption} />
                </div>
                <div className="drop-down">
                    <SeasonsDropDown selectedSeason={selectedSeasonPropBet} setSelectedSeason={setSelectedSeasonPropBet} setSelectedPlayerShotCharts={() => { }} setSelectedGame={setSelectedGame} isShotCharts={false} isPredictions={false} />
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
                <div className="results-flex-item">
                    {roster.length > 0 ?
                        <DragNDropRoster roster={roster} setRoster={setRoster} deletePlayer={deletePlayer} />
                        :
                        <div className="prop-bet-placeholders">
                            Select a Player
                        </div>
                    }
                </div>
                <div className="results-flex-item">
                    {propBetStats.length > 0 ?
                        <PropBetStatsDragNDrop propBetStats={propBetStats} setPropBetStats={setPropBetStats} deletePropBetStat={deletePropBetStat} />
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
                        <PropBetOpponent selectedOpponent={selectedOpponent} setSelectedOpponent={setSelectedOpponent} showOpponent={showOpponent} setShowOpponent={setShowOpponent} />
                    }
                </div>

            </div>
            
        </div>
        <div className="prop-bet-results-wrapper">
            <PropBetResults careerPlayerBoxScores={careerPlayerBoxScores} setCareerPlayerBoxScores={setCareerPlayerBoxScores} gamesPlayed={gamesPlayed} careerGamesPlayed={careerGamesPlayed} setCareerGamesPlayed={setCareerGamesPlayed} 
                overUnderLine={overUnderLine} propBetStats={propBetStats} selectedOpponent={selectedOpponent} roster={roster} 
                playerBoxScores={playerBoxScores} homeOrVisitor={homeOrVisitor} selectedSeason={selectedSeasonPropBet} lastTenFilteredBoxScores={lastTenFilteredBoxScores}/>
        </div>

            <PropBetResultsTable selectedSeason={selectedSeasonPropBet} overUnderLine={overUnderLine} selectedOpponent={selectedOpponent} 
                roster={roster} propBetStats={propBetStats} setPlayerBoxScores={setPlayerBoxScores} playerBoxScores={playerBoxScores} 
                gamesPlayed={gamesPlayed} setGamesPlayed={setGamesPlayed} homeOrVisitor={homeOrVisitor} setLastTenFilteredBoxScores={setLastTenFilteredBoxScores}/>
    </div>
    );
}

export default PropBet;