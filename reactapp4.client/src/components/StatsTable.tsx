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
import { NBATeam } from "../interfaces/Teams";

interface StatsTableProps {
    selectedSeason: string;
    selectedLineupPlayer: string;
    selectedBoxType: string;
    numPlayers: string;
    perMode: string;
    selectedTeam: NBATeam;
    sortField: string;
    setSortField: React.Dispatch<React.SetStateAction<string>>;
    inputText: string;
    setInputText: React.Dispatch<React.SetStateAction<string>>;
    selectedOpponent: NBATeam;
}



const StatsTable: React.FC<StatsTableProps> = React.memo(({ selectedSeason, selectedLineupPlayer, selectedBoxType, numPlayers, perMode, selectedTeam, sortField, setSortField, inputText, selectedOpponent }) => {

    const [order, setOrder] = useState<string>("desc");
    const [tableData, setTableData] = useState<Stats[]>([]);
    const [columns, setColumns] = useState<Column[]>([]);
    console.log('StatsTable')

    useEffect(() => {

        const getStats = async () => {
            console.log('getSTats hook')
            //if (selectedBoxType === 'Advanced') {
            //    if (selectedLineupPlayer === 'Player') {
            //        setColumns(advancedPlayerColumns);
            //    } else {
            //        setColumns(advancedLineupColumns);
            //    }  
            //}

            if (selectedLineupPlayer === 'Lineups') {

                try {
                    
                    const data = await axios.get(`/api/LeagueDashLineups/${selectedSeason}/${selectedBoxType}/${numPlayers}/${order}/${sortField}/${perMode}/${selectedTeam.team_id}`);
                    console.log(`/api/LeagueDashLineups/${selectedSeason}/${selectedBoxType}/${numPlayers}/${order}/${sortField}/${perMode}/${selectedTeam.team_id}/`);
                    setTableData(data.data);
                    console.log(data.data);
                    if (selectedBoxType === 'Advanced') {
                        setColumns(advancedLineupColumns);
                    } else if (selectedBoxType === 'Base') {
                        console.log('BASE');
                        setColumns(baseLineupColumns);
                    } else if (selectedBoxType === 'FourFactors') {
                        setColumns(fourFactorsLineupColumns);
                    } else if (selectedBoxType === 'Misc') {
                        setColumns(miscLineupColumns);
                    } else if (selectedBoxType === 'Scoring') {
                        setColumns(scoringLineupColumns);
                    } else if (selectedBoxType === 'Opponent') {
                        setColumns(opponentLineupColumns);
                    }
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