import React, { useEffect, useState } from "react";
//import TableBody from "./TableBody";
//import TableHead from "./TableHead";
//import tableData1 from "../data.json";
import '../App.css';
import axios from 'axios';
import StatsTableHeaders from "./StatsTableHeaders";
import StatsTableBody from "./StatsTableBody";
import { Column, LeagueDashLineupAdvanced, Stats } from "../interfaces/StatsTable";
import { advancedLineupColumns, advancedPlayerColumns, basePlayerColumns, baseLineupColumns } from "../interfaces/Columns";

interface StatsTableProps {
    selectedSeason: string;
    selectedLineupPlayer: string;
    selectedBoxType: string;
    numPlayers: string;
}





const StatsTable: React.FC<StatsTableProps> = ({ selectedSeason, selectedLineupPlayer, selectedBoxType, numPlayers }) => {

    const [tableData, setTableData] = useState<Stats[]>([]);
    const [columns, setColumns] = useState<Column[]>([]);


    useEffect(() => {

        const getStats = async () => {

            //if (selectedBoxType === 'Advanced') {
            //    if (selectedLineupPlayer === 'Player') {
            //        setColumns(advancedPlayerColumns);
            //    } else {
            //        setColumns(advancedLineupColumns);
            //    }  
            //}

            if (selectedLineupPlayer === 'Lineups') {
                if (selectedBoxType === 'Advanced') {
                    setColumns(advancedLineupColumns);
                } else if (selectedBoxType === 'Base') {
                    setColumns(baseLineupColumns);
                } else {
                    setColumns(advancedLineupColumns);
                }
                try {
                    console.log(selectedBoxType);
                    const data = await axios.get(`/api/LeagueDashLineups/${selectedSeason}/${selectedBoxType}/${numPlayers}`);
                    console.log(data.data);
                    setTableData(data.data);
                } catch (error) {
                    console.log(error);
                }
            }
            else if (selectedLineupPlayer === 'Player') {
                try {
                    const data = await axios.get(`/api/Player/${selectedSeason}/${selectedBoxType}`);
                    console.log(data.data)

                    setTableData(data.data)
                } catch (error) {
                    console.log(error);
                }
            } else {
                try {
                    console.log(selectedSeason);
                    console.log(selectedBoxType);
                    console.log(numPlayers);
                    const data = await axios.get(`/api/LeagueDashLineups/${selectedSeason}/${selectedBoxType}/${numPlayers}`);
                    console.log(data.data);
                    setTableData(data.data);
                } catch (error) {
                    console.log(error);
                }
            }
        }
        if (selectedSeason) {
            getStats()
        }
    }, [numPlayers, selectedLineupPlayer, selectedSeason, selectedBoxType]);

    /*
    const columns = [
        { label: "NAME", accessor: "player_name" },
        //{ label: "TEAM", accessor: "team_abbreviation" },
        { label: "MIN", accessor: "avg" },
    ];*/


    const handleSorting = (sortField: string, sortOrder: 'asc' | 'desc') => {
        if (sortField) {
            const sorted = [...tableData].sort((a, b) => {
                if (a[sortField] === null) return 1;
                if (b[sortField] === null) return -1;
                if (a[sortField] === null && b[sortField] === null) return 0;

                const nonNumeric = ['player_id', 'player_name', 'team_id', 'team_abbreviation'];

                //if (!nonNumeric.includes(sortField)) {
                //    
                //    return (a[sortField] - b[sortField]) * (sortOrder === 'desc' ? 1 : -1);
                //}

                return (
                    a[sortField].toString().localeCompare(b[sortField].toString(), 'en', {
                        numeric: true,
                    }) * (sortOrder === 'desc' ? 1 : -1)
                );
            });

            setTableData(sorted);
        }
    };



    return (
        <div>
            <table className='ultimateTable'>
                <caption>
                    Click on a stat header to sort all players by stat
                </caption>
                <StatsTableHeaders columns={columns} handleSorting={handleSorting} smallHeaders={false} />
                <StatsTableBody columns={columns} tableData={tableData} />
            </table>
        </div>
    );
};

export default StatsTable;