import React, { useEffect, useState } from 'react';
import { BoxScoreTraditional } from '../interfaces/BoxScoreTraditional';
import StatsTableHeaders from './StatsTableHeaders';
import StatsTableBody from './StatsTableBody';
import { Column, Stats } from '../interfaces/StatsTable';
import { basePlayerColumns } from '../interfaces/Columns';

interface PropBetResultsTableProps {
    playerBoxScores: Stats[];
}

const PropBetResultsTable: React.FC<PropBetResultsTableProps> = ({ playerBoxScores }) => {


    const [columns, setColumns] = useState<Column[]>([]);


    useEffect(() => {
        setColumns(basePlayerColumns);

    }, [playerBoxScores])


    return (
        <div className="player-box-container">
            <table>
                <caption>
                    Click on a stat header to sort all players by stat
                </caption>
                <StatsTableHeaders columns={columns} smallHeaders={true} />
                <StatsTableBody columns={columns} tableData={playerBoxScores} />
            </table>
        </div>
    );
}

export default PropBetResultsTable;