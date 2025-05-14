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
}



const MLBStatsTable: React.FC<MLBStatsTableProps> = React.memo(({ selectedSeason, hittingPitching, leagueOption, yearToDateOption, selectedTeam, setSelectedTeam, selectedOpponent, selectedSplit, inputText, sortField, setSortField }) => {

    console.log("MLB Stats Table")
    const [order, setOrder] = useState<string>("desc");
    const [tableData, setTableData] = useState<Stats[]>([]);
    const [columns, setColumns] = useState<Column[]>([]);
    //const [sortField, setSortField] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(false);

    console.log(tableData)

    useEffect(() => {
        console.log("get stats hook")
        const getStats = async () => {          
            //setLoading(true);
            setTableData([]);
            if (hittingPitching === 'hitting') {

                try {
                    const response = await axios.get(`/api/MLBStats/batting/${selectedSeason}`, {
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
                    console.log(response.data);
                } catch (error) {
                    console.log(error);
                }
                
            } else if (hittingPitching === 'pitching') {
                try {
                    const response = await axios.get(`/api/MLBStats/pitching/${selectedSeason}`, {
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
                    console.log(response.data);
                } catch (error) {
                    console.log(error);
                }
            }
        }
        if (selectedSeason) {
            getStats()
        }
    }, [ selectedSeason, yearToDateOption, selectedTeam, selectedOpponent, hittingPitching, selectedSplit ]);

    //useEffect(() => {
    //    if (leagueOption === "American League") {
    //        setSelectedTeam({ team_id: "1", team_name: "All American League" });    
    //    } else if (leagueOption === "National League") {
    //        setSelectedTeam({ team_id: "1", team_name: "All National League" });
    //    } else if (leagueOption === "MLB") {
    //        setSelectedTeam({ team_id: "1", team_name: "All MLB Teams" });
    //    } else {
    //        console.log('bad league selection')
    //        return;
    //    }
    //}, [leagueOption]);
    

    //const handleSorting = (clickedField: string) => {
    //    console.log("handle Sorting")
    //    if (!clickedField) return;
    //
    //    let nextOrder = "desc";
    //
    //    // If user clicks the same column again, flip the order
    //    if (sortField === clickedField) {
    //        nextOrder = order === "desc" ? "asc" : "desc";
    //    }
    //
    //    const sorted = [...tableData].sort((a, b) => {
    //        if (a[clickedField] === null) return 1;
    //        if (b[clickedField] === null) return -1;
    //        if (a[clickedField] === null && b[clickedField] === null) return 0;
    //
    //        const nonNumeric = [
    //            "teamSide", "teamName", "personId", "fullName", "leagueName",
    //            "summary", "note", "full_name", "boxscore_name",
    //            "jersey_number", "position", "position_abbr",
    //            "status_code", "status_description"
    //        ];
    //
    //        if (!nonNumeric.includes(clickedField)) {
    //            return (a[clickedField] as number - (b[clickedField] as number)) * (nextOrder === 'desc' ? -1 : 1);
    //        }
    //
    //        return (
    //            a[clickedField].toString().localeCompare(b[clickedField].toString(), 'en', { numeric: true })
    //            * (nextOrder === 'desc' ? -1 : 1)
    //        );
    //    });
    //
    //    setSortField(clickedField); // Update which field we're sorting on
    //    setOrder(nextOrder);        // Update order
    //    setTableData(sorted);
    //};
    
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