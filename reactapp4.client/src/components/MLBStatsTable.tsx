import React, { useEffect, useState } from "react";
//import TableBody from "./TableBody";
//import TableHead from "./TableHead";
//import tableData1 from "../data.json";
import '../App.css';
import axios from 'axios';
import StatsTableHeaders from "./StatsTableHeaders";
import { Column, Stats } from "../interfaces/StatsTable";
import { mlbBattingColumns, mlbPitchingColumns } from "../interfaces/Columns";
import { MLBTeam } from "../interfaces/Teams";
import MLBStatsTableBody from "./MLBStatsTableBody";
import MLBStatsTableHeaders from "./MLBStatsTableHeaders";

interface MLBStatsTableProps {
    selectedSeason: string;
    hittingPitching: string;
    leagueOption: string;
    yearToDateOption: string;
    selectedTeam: MLBTeam;
    setSelectedTeam: React.Dispatch<React.SetStateAction<MLBTeam>>;
    selectedOpponent: MLBTeam;
    sortField: string;
    setSortField: React.Dispatch<React.SetStateAction<string>>;
    selectedSplit: string;
    inputText: string;
    setInputText: React.Dispatch<React.SetStateAction<string>>;
    loading: boolean;
    setLoading: React.Dispatch<React.SetStateAction<boolean>>;
}



const MLBStatsTable: React.FC<MLBStatsTableProps> = React.memo(({ selectedSeason, hittingPitching, leagueOption, yearToDateOption, selectedTeam, 
    setSelectedTeam, selectedOpponent, selectedSplit, inputText, sortField, setSortField, loading, setLoading }) => {

    const [order, setOrder] = useState<string>("desc");
    const [tableData, setTableData] = useState<Stats[]>([]);
    const [columns, setColumns] = useState<Column[]>([]);
    //const [sortField, setSortField] = useState<string>("");


    useEffect(() => {
        const getStats = async () => {          
            //setLoading(true);
            //setTableData([]);
            if (hittingPitching === 'hitting') {

                try {
                    let url = ``;
                    if (["vs. LHP", "vs. RHP"].includes(selectedSplit)) {
                        url = `/api/MLBStats/batting/splits/${selectedSeason}`;
                    } else {
                        url = `/api/MLBStats/batting/${selectedSeason}`;
                    }
                    const response = await axios.get(url, {
                        params: {
                            leagueOption: leagueOption,
                            yearToDateOption: yearToDateOption,
                            selectedTeam: selectedTeam.team_id,
                            selectedOpponent: selectedOpponent.team_id,
                            order: order,
                            sortField: sortField,
                            selectedSplit: selectedSplit 
                        }
                    });


                    setTableData(response.data);
                    
                    setColumns(mlbBattingColumns);
                } catch (error) {
                    console.log(error);
                }
                
            } else if (hittingPitching === 'pitching') {
                try {

                    let url = ``;
                    if (["vs. LHB", "vs. RHB"].includes(selectedSplit)) {
                        url = `/api/MLBStats/pitching/splits/${selectedSeason}`;
                    } else {
                        url = `/api/MLBStats/pitching/${selectedSeason}`;
                    }
                    const response = await axios.get(url, {
                        params: {
                            leagueOption: leagueOption,
                            yearToDateOption: yearToDateOption,
                            selectedTeam: selectedTeam.team_id,
                            selectedOpponent: selectedOpponent.team_id,
                            order: order,
                            sortField: sortField,
                            selectedSplit: selectedSplit 
                        }
                    });


                    setTableData(response.data);
                    
                    setColumns(mlbPitchingColumns);
                } catch (error) {
                    console.log(error);
                }
            }
        }
        if (selectedSeason) {
            getStats()
        }
    }, [ selectedSeason, yearToDateOption, selectedTeam, selectedOpponent, hittingPitching, selectedSplit ]);
    
    
    const handleSorting = (sortField: string, sortOrder: string) => {
        if (sortField) {
            const sorted = [...tableData].sort((a, b) => {
                if (a[sortField] === null) return 1;
                if (b[sortField] === null) return -1;
                if (a[sortField] === null && b[sortField] === null) return 0;

                const nonNumeric = [
                    "teamSide", "teamName", "personId", "fullName", "leagueName",
                    "summary", "note", "full_name", "boxscore_name",
                    "jersey_number", "position", "position_abbr",
                    "status_code", "status_description"
                ];
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
                    <MLBStatsTableHeaders sortingFunction={handleSorting} columns={columns} order={order} />
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