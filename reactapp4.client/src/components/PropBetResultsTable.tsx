import React, { SetStateAction, useEffect, useState } from 'react';
import { NBATeam } from '../interfaces/Teams';
import { Player } from '../interfaces/Player';
import { PropBetStats } from '../interfaces/PropBetStats';
import axios from 'axios';
import { Column, Stats } from '../interfaces/StatsTable';
import StatsTableHeaders from './StatsTableHeaders';
import StatsTableBody from './StatsTableBody';
import { basePlayerColumnsNoName } from '../interfaces/Columns';
import { homeAwayFilteredBoxScores, overUnderFilteredBoxScores } from '../helpers/BoxScoreFilterFunctions';


interface PropBetResultsTableProps {
    selectedSeason: string;
    roster: Player[];
    propBetStats: PropBetStats[];
    overUnderLine: number | string | null;
    selectedOpponent: NBATeam;
    setPlayerBoxScores: React.Dispatch<SetStateAction<Stats[]>>
    playerBoxScores: Stats[];
    gamesPlayed: Stats[];
    setGamesPlayed: React.Dispatch<SetStateAction<Stats[]>>;
    homeOrVisitor: string;
    setLastTenFilteredBoxScores: React.Dispatch<SetStateAction<Stats[]>>;
    setIsFetching: React.Dispatch<SetStateAction<boolean>>;
}

const PropBetResultsTable: React.FC<PropBetResultsTableProps> = ({ selectedSeason, overUnderLine, selectedOpponent, roster, propBetStats, setPlayerBoxScores, playerBoxScores, gamesPlayed, 
    setGamesPlayed, homeOrVisitor, setLastTenFilteredBoxScores, setIsFetching }) => {

    const [columns] = useState<Column[]>(basePlayerColumnsNoName);

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
                        const resultsWithOpponent = await axios.get(`/api/PlayerResults?selectedSeason=${selectedSeason}&selectedOpponent=${encodedJsonSelectedOpponent}&player_id=${player.player_id}&propBetStats=${encodedJsonPropBetStats}`);
                        const OUFilteredBoxScores = await overUnderFilteredBoxScores(resultsWithOpponent.data, propBetStats, overUnderLine);
                        const homeVisitorOverUnderFilteredBoxScores = await homeAwayFilteredBoxScores(OUFilteredBoxScores, homeOrVisitor);
                        const homeVisitorFilteredBoxScores = await homeAwayFilteredBoxScores(resultsWithOpponent.data, homeOrVisitor);

                        const lastTenFilteredBoxScores = await overUnderFilteredBoxScores(homeVisitorFilteredBoxScores.slice(0, 10), propBetStats, overUnderLine);
                        setLastTenFilteredBoxScores(lastTenFilteredBoxScores);                    
                        setGamesPlayed(homeVisitorFilteredBoxScores);
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
        <div className="player-box-container">
            {gamesPlayed.length > 0 ?
                <div>
                    <table className="w-100">
                        <StatsTableHeaders columns={columns} onSort={() => {}} />
                        <StatsTableBody columns={columns} tableData={gamesPlayed} filteredBoxScores={playerBoxScores} />
                    </table>
                </div>
                :
                ""}
        </div>
    );
}

export default PropBetResultsTable;