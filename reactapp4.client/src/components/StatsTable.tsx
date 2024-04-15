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
import styled from 'styled-components';
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

const StyledTable = styled.table`
    max-height: 500px;
`

const TableContainer = styled.div`
    max-width: 800px;
    max-height: 600px;
    overflow: auto;
`



const StatsTable: React.FC<StatsTableProps> = React.memo(({ selectedSeason, selectedLineupPlayer, selectedBoxType, numPlayers, perMode, selectedTeam, sortField, setSortField, inputText, setInputText, selectedOpponent }) => {

    const [order, setOrder] = useState("desc");
    const [tableData, setTableData] = useState<Stats[]>([]);
    const [columns, setColumns] = useState<Column[]>([]);
    const [page, setPage] = useState(1);

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

                try {
                    
                    const data = await axios.get(`/api/LeagueDashLineups/${selectedSeason}/${selectedBoxType}/${numPlayers}/${order}/${sortField}/${perMode}/${selectedTeam.team_id}`);
                    console.log(`/api/LeagueDashLineups/${selectedSeason}/${selectedBoxType}/${numPlayers}/${order}/${sortField}/${perMode}/${selectedTeam.team_id}/`);
                    setTableData(data.data);
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
    }, [numPlayers, selectedLineupPlayer, selectedSeason, selectedBoxType, sortField, order, perMode, selectedTeam, selectedOpponent]);

    /*
    const columns = [
        { label: "NAME", accessor: "player_name" },
        //{ label: "TEAM", accessor: "team_abbreviation" },
        { label: "MIN", accessor: "avg" },
    ];*/


    const handleSorting = (sortField: string, sortOrder: 'asc' | 'desc') => {
        if (sortField) {
            const sorted = [...tableData].sort((a, b) => {
                console.log(sorted)
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

    const handleNextPage = () => {
        setPage(page + 1);
    };

    const handlePrevPage = () => {
        if (page > 1) {
            setPage(page - 1);
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
        <div className="player-box-container">
            {tableData.length > 0 ?
                <table className="w-100">
                    <StatsTableHeaders columns={columns} smallHeaders={true} sortField={sortField} setSortField={setSortField} order={order} setOrder={setOrder} setPage={setPage} />
                    <StatsTableBody columns={columns} tableData={filteredData} filteredBoxScores={[]} />
                </table>
            :
            ""}
        </div>
    );
});

export default StatsTable;