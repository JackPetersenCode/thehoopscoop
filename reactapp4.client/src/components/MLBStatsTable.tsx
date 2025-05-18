import React, { useState, useEffect } from 'react';
import { Column, Stats } from '../interfaces/StatsTable';
import MLBStatsTableHeaders from './MLBStatsTableHeaders';
import MLBStatsTableBody from './MLBStatsTableBody';


interface MLBStatsTableProps {
  inputText: string;
  statsData: Stats[];
  columns: Column[];
  isFetching: boolean; // 👈 receive this from parent
}

const MLBStatsTable: React.FC<MLBStatsTableProps> = React.memo(({ inputText, statsData, columns, isFetching }) => {
  const [order, setOrder] = useState<'asc' | 'desc'>('desc');

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
          <MLBStatsTableHeaders columns={columns} order={order} />
          <MLBStatsTableBody columns={columns} tableData={statsData} filteredBoxScores={[]} />
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
