import React, { useState, useEffect } from 'react';
import { Column, Stats } from '../interfaces/StatsTable';
import MLBStatsTableHeaders from './MLBStatsTableHeaders';
import MLBStatsTableBody from './MLBStatsTableBody';

interface MLBStatsTableProps {
  inputText: string;
  statsData: Stats[];
  columns: Column[];
}

const MLBStatsTable: React.FC<MLBStatsTableProps> = React.memo(
  ({ inputText, statsData, columns }) => {
    const [order, setOrder] = useState<'asc' | 'desc'>('desc');
    const [displayedStats, setDisplayedStats] = useState<Stats[]>([]);
    const [isFetching, setIsFetching] = useState<boolean>(true);

    // Detect new data fetches
    useEffect(() => {
      setIsFetching(true);
    }, [statsData]);

    // Apply delay after receiving new stats
    useEffect(() => {
      const timeout = setTimeout(() => {
        setDisplayedStats(statsData);
        setIsFetching(false);
      }, 100); // slight delay prevents flicker

      return () => clearTimeout(timeout);
    }, [statsData]);

    return (
      <div className="player-box-container" style={{ position: 'relative' }}>
        {displayedStats.length > 0 || isFetching ? (
          <table className="w-100" style={{ opacity: isFetching ? 0.5 : 1, transition: 'opacity 0.3s ease' }}>
            <MLBStatsTableHeaders columns={columns} order={order} />
            <MLBStatsTableBody columns={columns} tableData={displayedStats} filteredBoxScores={[]} />
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
  }
);

export default MLBStatsTable;
