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
    const [sortColumn, setSortColumn] = useState<string | null>(null);
    const [sortOrder, setSortOrder] = useState<SortOrder>('original');
    //const [originalData, setOriginalData] = useState<Stats[]>([]);
    const [hasLoadedOnce, setHasLoadedOnce] = useState(false);

    useEffect(() => {
      if (!isFetching) {
        setHasLoadedOnce(true);
      }
    }, [isFetching]);

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

    const isInitialLoad = isFetching && statsData.length === 0;
    const showNoStats = !isFetching && statsData.length > 0 && filteredData.length === 0;


    return (
    	<div className="player-box-container" style={{ position: 'relative' }}>
    		{isInitialLoad && (
    			<div className="initial-load">
    				<div className="spinner">Loading...</div>
    			</div>
    		)}

    		{showNoStats ? (
    			<div className="no-stats-available">NO STATS AVAILABLE</div>
    		) : (
    			<>
    				<table
    					className="w-100"
    					style={{
    						opacity: isFetching ? 0.5 : 1,
    						transition: 'opacity 0.3s ease',
    					}}
    				>
    					<MLBStatsTableHeaders columns={columns} onSort={handleSort} />
    					<MLBStatsTableBody columns={columns} tableData={filteredData} filteredBoxScores={[]} />
    				</table>
            
    				{isFetching && statsData.length > 0 && (
    					<div className="table-loading-overlay">
    						<div className="spinner">Loading...</div>
    					</div>
    				)}
    			</>
    		)}
    	</div>
    );

});

export default MLBStatsTable;
