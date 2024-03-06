import React, { useEffect, useState } from 'react';
import { BoxScoreTraditional } from '../interfaces/BoxScoreTraditional';
import StatsTableHeaders from './StatsTableHeaders';
import StatsTableBody from './StatsTableBody';
import { Column, Stats } from '../interfaces/StatsTable';
import { basePlayerColumns, basePlayerColumnsNoName } from '../interfaces/Columns';

interface PropBetResultsTableProps {
    playerBoxScores: Stats[];
}

const PropBetResultsTable: React.FC<PropBetResultsTableProps> = ({ playerBoxScores }) => {


    const [columns, setColumns] = useState<Column[]>([]);


    useEffect(() => {
        setColumns(basePlayerColumnsNoName);

    }, [playerBoxScores])


    return (
        <div className="player-box-container">
            {playerBoxScores.length > 0 ?
                <div>
                    <div className="text-left">
                        {playerBoxScores[0].player_name + " " + playerBoxScores[0].team_abbreviation}
                    </div>
                    <table>
                        <StatsTableHeaders columns={columns} smallHeaders={true} />
                        <StatsTableBody columns={columns} tableData={playerBoxScores} />
                    </table>
                </div>
            :
            ""}
        </div>
    );
}

export default PropBetResultsTable;