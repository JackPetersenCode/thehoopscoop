import React, { useEffect, useState } from "react";
//import TableBody from "./TableBody";
//import TableHead from "./TableHead";
//import tableData1 from "../data.json";
import '../App.css';
import axios from 'axios';
import StatsTableHeaders from "./StatsTableHeaders";
import StatsTableBody from "./StatsTableBody";
import { Column, Stats } from "../interfaces/StatsTable";
import { mlbBattingColumns } from "../interfaces/Columns";
import { MLBTeam } from "../interfaces/Teams";
import MLBStatsTableBody from "./MLBStatsTableBody";

interface MLBStatsTableProps {
    selectedSeason: string;
    hittingPitching: string;
    leagueOption: string;
    yearToDateOption: string;
    selectedTeam: MLBTeam;
    sortField: string;
    setSortField: React.Dispatch<React.SetStateAction<string>>;
    inputText: string;
    setInputText: React.Dispatch<React.SetStateAction<string>>;
}



const MLBStatsTable: React.FC<MLBStatsTableProps> = React.memo(({ selectedSeason, hittingPitching, leagueOption, yearToDateOption, selectedTeam, sortField, setSortField, inputText }) => {

    const [order, setOrder] = useState<string>("desc");
    const [tableData, setTableData] = useState<Stats[]>([]);
    const [columns, setColumns] = useState<Column[]>([]);

    useEffect(() => {

        const getStats = async () => {

            if (hittingPitching === 'hitting') {

                try {
                    const response = await axios.get(`/api/MLBStats/batting/${selectedSeason}`, {
                        params: {
                            leagueOption: leagueOption,
                            yearToDateOption: yearToDateOption,
                            selectedTeam: selectedTeam.team_id,
                            order: order,
                            sortField: sortField
                        }
                    });
                
                    setTableData(response.data);
                    setColumns(mlbBattingColumns);
                    console.log(response.data);
                } catch (error) {
                    console.log(error);
                }
                
            }
        }
        if (selectedSeason) {
            getStats()
        }
    }, [ selectedSeason, leagueOption, yearToDateOption, selectedTeam, order, sortField ]);


    const handleSorting = (sortField: string, sortOrder: string) => {
        if (sortField) {
            const sorted = [...tableData].sort((a, b) => {
                if (a[sortField] === null) return 1;
                if (b[sortField] === null) return -1;
                if (a[sortField] === null && b[sortField] === null) return 0;

                const nonNumeric = ["team_side", "team_name", "player_id", "summary", "note", "full_name", "boxscore_name", "jersey_number", "position", "position_abbr", "status_code", "status_description"];

                if (!nonNumeric.includes(sortField)) {
                    
                    return (a[sortField] as number - (b[sortField] as number)) * (sortOrder === 'desc' ? 1 : -1);
                }

                return (
                    a[sortField].toString().localeCompare(b[sortField].toString(), 'en', {
                        numeric: true,
                    }) * (sortOrder === 'desc' ? 1 : -1)
                );
            });

            setOrder(order === "asc" ? "desc" : "asc");
            setTableData(sorted);
        }
    };

    const filteredData = tableData.filter((element) => {
        console.log(element);
        //if no input the return the original
        //return the item which contains the user input
        if (element.fullName) {
            return element.fullName.toString().toLowerCase().includes(inputText.toLowerCase());
        }
    })

    return (
        <>
        {filteredData.length > 0 ?
            <div className="player-box-container">
                <table className="w-100">
                    <StatsTableHeaders sortingFunction={handleSorting} columns={columns} smallHeaders={true} sortField={sortField} setSortField={setSortField} order={order} setOrder={setOrder} />
                    <MLBStatsTableBody columns={columns} tableData={filteredData} filteredBoxScores={[]} />
                </table>
            </div>
            :
            <div className="no-stats-exist">
                NO STATS EXIST
            </div>
        }
        </>
    );
});

export default MLBStatsTable;