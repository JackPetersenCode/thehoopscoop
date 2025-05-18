import React, { useState, useEffect, useMemo } from 'react';
import { Column, Stats } from '../interfaces/StatsTable';
import MLBStatsTableHeaders from './MLBStatsTableHeaders';
import MLBStatsTableBody from './MLBStatsTableBody';
import { getSortedData, SortOrder } from '../helpers/MLBSortingFunction';


interface MLBStatsTableProps {
    inputText: string;
    statsData: Stats[];
    columns: Column[];
    isFetching: boolean; // 👈 receive this from parent
    originalData: Stats[];
}

const MLBStatsTable: React.FC<MLBStatsTableProps> = React.memo(({ inputText, statsData, columns, isFetching, originalData }) => {
    const [order, setOrder] = useState<'asc' | 'desc'>('desc');
    const [sortColumn, setSortColumn] = useState<string | null>(null);
    const [sortOrder, setSortOrder] = useState<SortOrder>('original');
    //const [originalData, setOriginalData] = useState<Stats[]>([]);

    //useEffect(() => {
    //  console.log('shitfuck')
    //  setOriginalData(statsData);
    //}, [statsData]);

    const sortedData = useMemo(() => {
      if (!sortColumn || sortOrder === 'original') return [...originalData];
      return getSortedData(statsData, sortColumn, sortOrder, originalData);
    }, [statsData, sortColumn, sortOrder, originalData]);

    const handleSort = (column: string) => {
    
      if (column !== sortColumn) {
        setSortColumn(column);
        setSortOrder('desc');
      } else {
        setSortOrder(prev =>
          prev === 'desc' ? 'asc' : prev === 'asc' ? 'original' : 'desc'
        );
      }
    };

    const filteredData = sortedData.filter((element) => {
        //if no input the return the original
        //return the item which contains the user input
        if (element.fullName) {
            return element.fullName.toString().toLowerCase().includes(inputText.toLowerCase());
        }
    })

    return (
        <div className="player-box-container" style={{ position: 'relative' }}>
            {statsData.length > 0 || isFetching ? (
                <table
                    className="w-100"
                    style={{
                      opacity: isFetching ? 0.5 : 1,
                      transition: 'opacity 0.3s ease',
                    }}
                >
                    <MLBStatsTableHeaders columns={columns} sortColumn={sortColumn} sortOrder={sortOrder} onSort={handleSort} />
                    <MLBStatsTableBody columns={columns} tableData={filteredData} filteredBoxScores={[]}  />
                </table>
            ) : (
                <div className="no-stats-exist">NO STATS EXIST</div>
            )}

            {isFetching && (
                <div className="table-loading-overlay">
                    <div className="spinner">Loading...</div>
                </div>
            )}
        </div>
    );
});

export default MLBStatsTable;
