import React, { useEffect, useState } from "react";
//import TableBody from "./TableBody";
//import TableHead from "./TableHead";
//import tableData1 from "../data.json";
import '../App.css';
import axios from 'axios';
import StatsTableHeaders from "./StatsTableHeaders";
import StatsTableBody from "./StatsTableBody";
import { Column, Stats } from "../interfaces/StatsTable";
import { advancedLineupColumns, advancedPlayerColumns, basePlayerColumns, fourFactorsPlayerColumns, miscPlayerColumns,scoringPlayerColumns, baseLineupColumns, fourFactorsLineupColumns, miscLineupColumns, scoringLineupColumns, opponentLineupColumns } from "../interfaces/Columns";
import { MLBTeam } from "../interfaces/Teams";

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
                    
                    const data = await axios.get(`/api/MLBStats/hitting/${selectedSeason}/${leagueOption}/${yearToDateOption}/${selectedTeam.team_id}/${order}/${sortField}`);
                    setTableData(data.data);
                    console.log(data.data);
                } catch (error) {
                    console.log(error);
                }
            }
            else if (selectedLineupPlayer === 'Players') {

                try {
                    console.log(selectedBoxType);
                    console.log(perMode);
                    console.log(selectedOpponent);
                    const data = await axios.get(`/api/BoxScores/${selectedSeason}/${selectedBoxType}/${order}/${sortField}/${perMode}/${selectedTeam.team_id}/${selectedOpponent.team_abbreviation}`);
                    console.log(data.data);
                    console.log(`/api/BoxScores/${selectedSeason}/${selectedBoxType}/${order}/${sortField}/${perMode}/${selectedTeam.team_id}/${selectedOpponent.team_abbreviation}`);
                    setTableData(data.data);
                    console.log("PLAYERSSSSS")
                    if (selectedBoxType === 'Advanced') {
                        setColumns(advancedPlayerColumns);
                    } else if (selectedBoxType === 'Base') {
                        console.log('BASE');
                        setColumns(basePlayerColumns);
                    } else if (selectedBoxType === 'FourFactors') {
                        setColumns(fourFactorsPlayerColumns);
                    } else if (selectedBoxType === 'Misc') {
                        setColumns(miscPlayerColumns);
                    } else if (selectedBoxType === 'Scoring') {
                        setColumns(scoringPlayerColumns);
                    }
                } catch (error) {
                    console.log(error);
                }
            }
        }
        if (selectedSeason) {
            getStats()
        }
    }, [numPlayers, selectedLineupPlayer, selectedSeason, selectedBoxType, perMode, selectedTeam, selectedOpponent]);

    /*
    const columns = [
        { label: "NAME", accessor: "player_name" },
        //{ label: "TEAM", accessor: "team_abbreviation" },
        { label: "MIN", accessor: "avg" },
    ];*/


    const handleSorting = (sortField: string, sortOrder: string) => {
        if (sortField) {
            const sorted = [...tableData].sort((a, b) => {
                if (a[sortField] === null) return 1;
                if (b[sortField] === null) return -1;
                if (a[sortField] === null && b[sortField] === null) return 0;

                const nonNumeric = ['player_id', 'player_name', 'team_id', 'team_abbreviation', 'team_city', 'group_name', 'comment', 'start_position'];

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
        if (!element.player_name) {
            return element.group_name.toString().toLowerCase().includes(inputText.toLowerCase());
        } else {
            return element.player_name.toString().toLowerCase().includes(inputText.toLowerCase());
        }
    })

    return (
        <>
        {filteredData.length > 0 ?
            <div className="player-box-container">
                <table className="w-100">
                    <StatsTableHeaders sortingFunction={handleSorting} columns={columns} smallHeaders={true} sortField={sortField} setSortField={setSortField} order={order} setOrder={setOrder} />
                    <StatsTableBody columns={columns} tableData={filteredData} filteredBoxScores={[]} />
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

export default StatsTable;