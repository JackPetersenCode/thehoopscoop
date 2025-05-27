import React, { SetStateAction, useEffect, useState } from 'react';
import { MLBTeam } from '../interfaces/Teams';
import { PropBetStats } from '../interfaces/PropBetStats';
import axios from 'axios';
import { Column, Stats } from '../interfaces/StatsTable';
import StatsTableHeaders from './StatsTableHeaders';
import StatsTableBody from './StatsTableBody';
import { basePlayerColumnsNoName, mlbBattingColumnsPropBet, mlbPitchingColumns, mlbPitchingColumnsPropBet } from '../interfaces/Columns';
import { homeAwayFilteredBoxScores, MLBhomeAwayFilteredBoxScores, overUnderFilteredBoxScores } from '../helpers/BoxScoreFilterFunctions';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';
import MLBStatsTableHeaders from './MLBStatsTableHeaders';
import MLBStatsTableBody from './MLBStatsTableBody';


interface MLBPropBetResultsTableProps {
    selectedSeason: string;
    roster: MLBActivePlayer[];
    propBetStats: PropBetStats[];
    overUnderLine: number | string;
    selectedOpponent: MLBTeam;
    setPlayerBoxScores: React.Dispatch<SetStateAction<Stats[]>>
    playerBoxScores: Stats[];
    gamesPlayed: Stats[];
    setGamesPlayed: React.Dispatch<SetStateAction<Stats[]>>;
    homeOrVisitor: string;
    hittingPitching: string;
}

const MLBPropBetResultsTable: React.FC<MLBPropBetResultsTableProps> = ({ selectedSeason, overUnderLine, selectedOpponent, roster, propBetStats, setPlayerBoxScores, playerBoxScores, gamesPlayed, setGamesPlayed, homeOrVisitor, hittingPitching }) => {

    const columns: Column[] =
      hittingPitching === "hitting"
        ? mlbBattingColumnsPropBet
        : mlbPitchingColumnsPropBet;

    useEffect(() => {
        const getPropBetResults = async () => {

            const jsonPropBetStats = JSON.stringify(propBetStats);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonPropBetStats = encodeURIComponent(jsonPropBetStats);

            const jsonSelectedOpponent = JSON.stringify(selectedOpponent);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonSelectedOpponent = encodeURIComponent(jsonSelectedOpponent);

            // Construct the URL with the encoded JSON as a query parameter
            console.log(roster);

            if (roster.length == 0) {
                setPlayerBoxScores([]);
                setGamesPlayed([]);
            } else {
                for (const player of roster) {

                    try {
                        const resultsWithOpponent = await axios.get(`/api/MLBPlayerResults?hittingPitching=${hittingPitching}&selectedSeason=${selectedSeason}&selectedOpponent=${encodedJsonSelectedOpponent}&player_id=${player.playerId}&propBetStats=${encodedJsonPropBetStats}`);
                        console.log(resultsWithOpponent.data);

                        const OUFilteredBoxScores = await overUnderFilteredBoxScores(resultsWithOpponent.data, propBetStats, overUnderLine);
                        const homeVisitorOverUnderFilteredBoxScores = await MLBhomeAwayFilteredBoxScores(OUFilteredBoxScores, homeOrVisitor);
                        const homeVisitorFilteredBoxScores = await MLBhomeAwayFilteredBoxScores(resultsWithOpponent.data, homeOrVisitor);

                    
                        setGamesPlayed(homeVisitorFilteredBoxScores)
                        setPlayerBoxScores(homeVisitorOverUnderFilteredBoxScores);
                    } catch (error) {
                        console.log(error);
                    }
                }
            }
        }
        getPropBetResults();

    }, [selectedSeason, roster, propBetStats, overUnderLine, selectedOpponent, homeOrVisitor])


    return (
        <div className="player-box-container">
            {gamesPlayed.length > 0 ?
                <div>
                    <table className="w-100">
                        <MLBStatsTableHeaders columns={columns} onSort={() => {}} />
                        <MLBStatsTableBody columns={columns} tableData={gamesPlayed} filteredBoxScores={playerBoxScores} />
                    </table>
                </div>
                :
                ""}
        </div>
    );
}

export default MLBPropBetResultsTable;