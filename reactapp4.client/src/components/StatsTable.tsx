import React, { useEffect, useMemo, useState } from "react";
//import TableBody from "./TableBody";
//import TableHead from "./TableHead";
//import tableData1 from "../data.json";
import '../App.css';
import axios from 'axios';
import StatsTableHeaders from "./StatsTableHeaders";
import StatsTableBody from "./StatsTableBody";
import { Column, Stats } from "../interfaces/StatsTable";
import { advancedLineupColumns, advancedPlayerColumns, basePlayerColumns, fourFactorsPlayerColumns, miscPlayerColumns,scoringPlayerColumns, baseLineupColumns, fourFactorsLineupColumns, miscLineupColumns, scoringLineupColumns, opponentLineupColumns } from "../interfaces/Columns";
import { NBATeam } from "../interfaces/Teams";
import { getSortedData, SortOrder } from "../helpers/MLBSortingFunction";

interface StatsTableProps {
    inputText: string;
    statsData: Stats[];
    columns: Column[];
    isFetching: boolean; // 👈 receive this from parent
    originalData: Stats[];
}



const StatsTable: React.FC<StatsTableProps> = React.memo(({ inputText, statsData, columns, isFetching, originalData }) => {
    const [sortColumn, setSortColumn] = useState<string | null>(null);
    const [sortOrder, setSortOrder] = useState<SortOrder>('original');

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
        if (element.player_name) {
            return element.player_name.toString().toLowerCase().includes(inputText.toLowerCase());
        }
    })

    const isInitialLoad = isFetching && statsData.length === 0;
    const showNoStats = !isFetching && statsData.length > 0 && filteredData.length === 0;
    const noStatsData = !isFetching && statsData.length === 0 && !isInitialLoad;    

    return (
    	<div className="player-box-container" style={{ position: 'relative' }}>
    		{isInitialLoad && (
    			<div className="initial-load">
    				<div className="spinner">Loading...</div>
    			</div>
    		)}

		    {showNoStats ? (
		    	<div className="no-stats-available">NO STATS EXIST</div>
		    ) : noStatsData ? (
		    	<div className="no-stats-available">NO STATS EXIST</div>
		    ) : (
    			<>
    				<table
    					className="w-100"
    					style={{
    						opacity: isFetching ? 0.5 : 1,
    						transition: 'opacity 0.3s ease',
    					}}
    				>
    					<StatsTableHeaders columns={columns} onSort={handleSort} />
    					<StatsTableBody columns={columns} tableData={filteredData} filteredBoxScores={[]} />
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

export default StatsTable;