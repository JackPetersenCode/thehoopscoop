import React, { useEffect, useState } from "react";
//import TableBody from "./TableBody";
//import TableHead from "./TableHead";
//import tableData1 from "../data.json";
import '../App.css';
import axios from 'axios';
import StatsTableHeaders from "./StatsTableHeaders";
import StatsTableBody from "./StatsTableBody";
import { Column, LeagueDashLineupAdvanced, Stats } from "../interfaces/StatsTable";

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

            if (selectedBoxType === 'Advanced') {
                if (selectedLineupPlayer === 'Player') {
                    setColumns([
                        { label: "NAME", accessor: "name" },
                        { label: "TEAM", accessor: "team_abbreviation" },
                        { label: "TM TOV PCT", accessor: "season" },
                        { label: "MIN", accessor: "min" },
                        { label: "AST PCT", accessor: "ast_pct" },
                        { label: "AST RATIO", accessor: "ast_ratio" },
                        { label: "AST TO", accessor: "ast_to" },
                        { label: "DEF RATING", accessor: "def_rating" },
                        { label: "DEF REB PCT", accessor: "dreb_pct" },
                        { label: "EFG PCT", accessor: "efg_pct" },
                        { label: "GP", accessor: "gp" },
                        { label: "NET RATING", accessor: "net_rating" },
                        { label: "OFF RATING", accessor: "off_rating" },
                        { label: "OREB PCT", accessor: "oreb_pct" },
                        { label: "PACE", accessor: "pace" },
                        { label: "REB PCT", accessor: "reb_pct" },
                        { label: "TM TOV PCT", accessor: "tm_tov_pct" },
                        { label: "TS PCT", accessor: "ts_pct" },
                        { label: "USG PCT", accessor: "usg_pct" },
                        { label: "POSS", accessor: "poss" },
                        { label: "PIE", accessor: "pie" }
                    ]);
                } else if (selectedLineupPlayer === 'Lineups') {
                    setColumns([
                        { label: "GROUP SET", accessor: "group_set" },
                        { label: "GROUP ID", accessor: "group_id" },
                        { label: "GROUP NAME", accessor: "group_name" },
                        { label: "TEAM ID", accessor: "team_id" },
                        { label: "TEAM ABBREVIATION", accessor: "team_abbreviation" },
                        { label: "GP", accessor: "gp" },
                        { label: "W", accessor: "w" },
                        { label: "L", accessor: "l" },
                        { label: "W PCT", accessor: "w_pct" },
                        { label: "MIN", accessor: "min" },
                        { label: "E OFF RATING", accessor: "e_off_rating" },
                        { label: "OFF RATING", accessor: "off_rating" },
                        { label: "E DEF RATING", accessor: "e_def_rating" },
                        { label: "DEF RATING", accessor: "def_rating" },
                        { label: "E NET RATING", accessor: "e_net_rating" },
                        { label: "NET RATING", accessor: "net_rating" },
                        { label: "AST PCT", accessor: "ast_pct" },
                        { label: "AST TO", accessor: "ast_to" },
                        { label: "AST RATIO", accessor: "ast_ratio" },
                        { label: "OREB PCT", accessor: "oreb_pct" },
                        { label: "DREB PCT", accessor: "dreb_pct" },
                        { label: "REB PCT", accessor: "reb_pct" },
                        { label: "TM TOV PCT", accessor: "tm_tov_pct" },
                        { label: "EFG PCT", accessor: "efg_pct" },
                        { label: "TS PCT", accessor: "ts_pct" },
                        { label: "E PACE", accessor: "e_pace" },
                        { label: "PACE", accessor: "pace" },
                        { label: "PACE PER40", accessor: "pace_per40" },
                        { label: "POSS", accessor: "poss" },
                        { label: "PIE", accessor: "pie" },
                        { label: "GP RANK", accessor: "gp_rank" },
                        { label: "W RANK", accessor: "w_rank" },
                        { label: "L RANK", accessor: "l_rank" },
                        { label: "W PCT RANK", accessor: "w_pct_rank" },
                        { label: "MIN RANK", accessor: "min_rank" },
                        { label: "OFF RATING RANK", accessor: "off_rating_rank" },
                        { label: "DEF RATING RANK", accessor: "def_rating_rank" },
                        { label: "NET RATING RANK", accessor: "net_rating_rank" },
                        { label: "AST PCT RANK", accessor: "ast_pct_rank" },
                        { label: "AST TO RANK", accessor: "ast_to_rank" },
                        { label: "AST RATIO RANK", accessor: "ast_ratio_rank" },
                        { label: "OREB PCT RANK", accessor: "oreb_pct_rank" },
                        { label: "DREB PCT RANK", accessor: "dreb_pct_rank" },
                        { label: "REB PCT RANK", accessor: "reb_pct_rank" },
                        { label: "TM TOV PCT RANK", accessor: "tm_tov_pct_rank" },
                        { label: "EFG PCT RANK", accessor: "efg_pct_rank" },
                        { label: "TS PCT RANK", accessor: "ts_pct_rank" },
                        { label: "PACE RANK", accessor: "pace_rank" },
                        { label: "PIE RANK", accessor: "pie_rank" }
                    ]);

                } else {
                    setColumns([

                        { label: "GROUP SET", accessor: "group_set" },
                        { label: "GROUP ID", accessor: "group_id" },
                        { label: "GROUP NAME", accessor: "group_name" },
                        { label: "TEAM ID", accessor: "team_id" },
                        { label: "TEAM ABBREVIATION", accessor: "team_abbreviation" },
                        { label: "GP", accessor: "gp" },
                        { label: "W", accessor: "w" },
                        { label: "L", accessor: "l" },
                        { label: "W PCT", accessor: "w_pct" },
                        { label: "MIN", accessor: "min" },
                        { label: "E OFF RATING", accessor: "e_off_rating" },
                        { label: "OFF RATING", accessor: "off_rating" },
                        { label: "E DEF RATING", accessor: "e_def_rating" },
                        { label: "DEF RATING", accessor: "def_rating" },
                        { label: "E NET RATING", accessor: "e_net_rating" },
                        { label: "NET RATING", accessor: "net_rating" },
                        { label: "AST PCT", accessor: "ast_pct" },
                        { label: "AST TO", accessor: "ast_to" },
                        { label: "AST RATIO", accessor: "ast_ratio" },
                        { label: "OREB PCT", accessor: "oreb_pct" },
                        { label: "DREB PCT", accessor: "dreb_pct" },
                        { label: "REB PCT", accessor: "reb_pct" },
                        { label: "TM TOV PCT", accessor: "tm_tov_pct" },
                        { label: "EFG PCT", accessor: "efg_pct" },
                        { label: "TS PCT", accessor: "ts_pct" },
                        { label: "E PACE", accessor: "e_pace" },
                        { label: "PACE", accessor: "pace" },
                        { label: "PACE PER40", accessor: "pace_per40" },
                        { label: "POSS", accessor: "poss" },
                        { label: "PIE", accessor: "pie" },
                        { label: "GP RANK", accessor: "gp_rank" },
                        { label: "W RANK", accessor: "w_rank" },
                        { label: "L RANK", accessor: "l_rank" },
                        { label: "W PCT RANK", accessor: "w_pct_rank" },
                        { label: "MIN RANK", accessor: "min_rank" },
                        { label: "OFF RATING RANK", accessor: "off_rating_rank" },
                        { label: "DEF RATING RANK", accessor: "def_rating_rank" },
                        { label: "NET RATING RANK", accessor: "net_rating_rank" },
                        { label: "AST PCT RANK", accessor: "ast_pct_rank" },
                        { label: "AST TO RANK", accessor: "ast_to_rank" },
                        { label: "AST RATIO RANK", accessor: "ast_ratio_rank" },
                        { label: "OREB PCT RANK", accessor: "oreb_pct_rank" },
                        { label: "DREB PCT RANK", accessor: "dreb_pct_rank" },
                        { label: "REB PCT RANK", accessor: "reb_pct_rank" },
                        { label: "TM TOV PCT RANK", accessor: "tm_tov_pct_rank" },
                        { label: "EFG PCT RANK", accessor: "efg_pct_rank" },
                        { label: "TS PCT RANK", accessor: "ts_pct_rank" },
                        { label: "PACE RANK", accessor: "pace_rank" },
                        { label: "PIE RANK", accessor: "pie_rank" }
                    ]);
                }

            }

            if (selectedLineupPlayer === 'Lineup') {
                try {
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