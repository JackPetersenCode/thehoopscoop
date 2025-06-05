import React, { SetStateAction, useEffect, useState } from 'react';
import { MLBTeam } from '../interfaces/Teams';
import { PropBetStats } from '../interfaces/PropBetStats';
import axios from 'axios';
import { Column, Stats } from '../interfaces/StatsTable';
import { mlbBattingColumnsPropBet, mlbPitchingColumnsPropBet } from '../interfaces/Columns';
import { MLBhomeAwayFilteredBoxScores, overUnderFilteredBoxScores } from '../helpers/BoxScoreFilterFunctions';
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
    setLastTenFilteredBoxScores: React.Dispatch<SetStateAction<Stats[]>>;
    setIsFetching: React.Dispatch<SetStateAction<boolean>>;
}

const MLBPropBetResultsTable: React.FC<MLBPropBetResultsTableProps> = ({ selectedSeason, overUnderLine, selectedOpponent, roster, propBetStats, 
    setPlayerBoxScores, playerBoxScores, gamesPlayed, setGamesPlayed, homeOrVisitor, hittingPitching, setLastTenFilteredBoxScores, setIsFetching }) => {

    const columns: Column[] =
      hittingPitching === "hitting"
        ? mlbBattingColumnsPropBet
        : mlbPitchingColumnsPropBet;

    useEffect(() => {
        const getPropBetResults = async () => {
            setIsFetching(true);
            const jsonPropBetStats = JSON.stringify(propBetStats);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonPropBetStats = encodeURIComponent(jsonPropBetStats);

            const jsonSelectedOpponent = JSON.stringify(selectedOpponent);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonSelectedOpponent = encodeURIComponent(jsonSelectedOpponent);

            // Construct the URL with the encoded JSON as a query parameter
            if (roster.length == 0) {
                setPlayerBoxScores([]);
                setGamesPlayed([]);
                setIsFetching(false);
            } else {
                for (const player of roster) {

                    try {
                        const resultsWithOpponent = await axios.get(`/api/MLBPlayerResults?hittingPitching=${hittingPitching}&selectedSeason=${selectedSeason}&selectedOpponent=${encodedJsonSelectedOpponent}&player_id=${player.playerId}&propBetStats=${encodedJsonPropBetStats}`);
                        const OUFilteredBoxScores = await overUnderFilteredBoxScores(resultsWithOpponent.data, propBetStats, overUnderLine);
                        const homeVisitorOverUnderFilteredBoxScores = await MLBhomeAwayFilteredBoxScores(OUFilteredBoxScores, homeOrVisitor);
                        const homeVisitorFilteredBoxScores = await MLBhomeAwayFilteredBoxScores(resultsWithOpponent.data, homeOrVisitor);
                        const lastTenFilteredBoxScores = await overUnderFilteredBoxScores(homeVisitorFilteredBoxScores.slice(0, 10), propBetStats, overUnderLine);
                        setLastTenFilteredBoxScores(lastTenFilteredBoxScores);
                        setGamesPlayed(homeVisitorFilteredBoxScores)
                        setPlayerBoxScores(homeVisitorOverUnderFilteredBoxScores);
                    } catch (error) {
                        console.log(error);
                    } finally {
                        setIsFetching(false);
                    }
                }
            }
        }
        getPropBetResults();

    }, [selectedSeason, roster, propBetStats, overUnderLine, selectedOpponent, homeOrVisitor])


    return (
        <>
            {gamesPlayed.length > 0 ?
                <div className="player-box-container" >
                    <div>
                        <table className="w-100">
                            <MLBStatsTableHeaders columns={columns} onSort={() => {}} />
                            <MLBStatsTableBody columns={columns} tableData={gamesPlayed} filteredBoxScores={playerBoxScores} />
                        </table>
                    </div>
                </div>
                :
                ""}
        </>
    );
}

export default MLBPropBetResultsTable;