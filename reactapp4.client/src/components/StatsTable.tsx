import React, { useEffect, useState } from "react";
//import TableBody from "./TableBody";
//import TableHead from "./TableHead";
//import tableData1 from "../data.json";
import '../App.css';
import axios from 'axios';
import StatsTableHeaders from "./StatsTableHeaders";
import StatsTableBody from "./StatsTableBody";
import { Column, LeagueDashLineupAdvanced, Stats } from "../interfaces/StatsTable";
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
}

const StyledTable = styled.table`
    max-height: 500px;
`

const TableContainer = styled.div`
    max-width: 800px;
    max-height: 600px;
    overflow: auto;
`



const StatsTable: React.FC<StatsTableProps> = ({ selectedSeason, selectedLineupPlayer, selectedBoxType, numPlayers, perMode, selectedTeam }) => {

    const [sortField, setSortField] = useState("min");
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
                try {
                    console.log(selectedBoxType);
                    console.log(perMode);
                    const data = await axios.get(`/api/LeagueDashLineups/${selectedSeason}/${selectedBoxType}/${numPlayers}/${order}/${sortField}/${page}/${perMode}/${selectedTeam.team_id}`);
                    console.log(data.data);
                    console.log(`/api/LeagueDashLineups/${selectedSeason}/${selectedBoxType}/${numPlayers}/${order}/${sortField}/${page}/${perMode}/${selectedTeam.team_id}`);
                    setTableData(data.data);
                } catch (error) {
                    console.log(error);
                }
            }
            else if (selectedLineupPlayer === 'Players') {
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
                try {
                    console.log(selectedBoxType);
                    console.log(perMode);
                    const data = await axios.get(`/api/BoxScores/${selectedSeason}/${selectedBoxType}/${order}/${sortField}/${page}/${perMode}/${selectedTeam.team_id}`);
                    console.log(data.data);
                    console.log(`/api/BoxScores/${selectedSeason}/${selectedBoxType}/${order}/${sortField}/${page}/${perMode}/${selectedTeam.team_id}`);
                    setTableData(data.data);
                } catch (error) {
                    console.log(error);
                }
            } else {
                try {
                    console.log(selectedLineupPlayer);
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
    }, [numPlayers, selectedLineupPlayer, selectedSeason, selectedBoxType, sortField, order, page, perMode, selectedTeam]);

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

    const handleNextPage = () => {
        setPage(page + 1);
    };

    const handlePrevPage = () => {
        if (page > 1) {
            setPage(page - 1);
        }
    };

    return (
        <TableContainer>
            <StyledTable>
                <caption>
                    Click on a stat header to sort all players by stat
                </caption>
                <StatsTableHeaders columns={columns} handleSorting={handleSorting} smallHeaders={false} sortField={sortField} setSortField={setSortField} order={order} setOrder={setOrder} />
                <StatsTableBody columns={columns} tableData={tableData} />
            </StyledTable>
            <div>
                <button onClick={handlePrevPage} disabled={page === 1}>Previous</button>
                <button onClick={handleNextPage}>Next</button>
            </div>
        </TableContainer>
    );
};

export default StatsTable;