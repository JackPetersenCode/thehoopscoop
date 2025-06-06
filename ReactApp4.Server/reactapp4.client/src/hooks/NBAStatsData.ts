import React, { useEffect, useState } from "react";
//import TableBody from "./TableBody";
//import TableHead from "./TableHead";
//import tableData1 from "../data.json";
import '../App.css';
import axios from 'axios';
import { Column, Stats } from "../interfaces/StatsTable";
import { advancedLineupColumns, advancedPlayerColumns, basePlayerColumns, fourFactorsPlayerColumns, miscPlayerColumns,scoringPlayerColumns, baseLineupColumns, fourFactorsLineupColumns, miscLineupColumns, scoringLineupColumns, opponentLineupColumns } from "../interfaces/Columns";
import { NBATeam } from "../interfaces/Teams";

interface Props {
    selectedSeason: string;
    selectedLineupPlayer: string;
    selectedBoxType: string;
    numPlayers: string;
    perMode: string;
    selectedTeam: NBATeam;
    sortField: string;
    selectedOpponent: NBATeam;
}



export function NBAStatsData({ selectedSeason, selectedLineupPlayer, selectedBoxType, numPlayers, perMode, selectedTeam, sortField, selectedOpponent }: Props) {

    const [statsData, setStatsData] = useState<Stats[]>([]);
    const [columns, setColumns] = useState<Column[]>([]);
    const [isFetching, setIsFetching] = useState(false);
    const [originalData, setOriginalData] = useState<Stats[]>([]);

    useEffect(() => {

        const getStats = async () => {
            setIsFetching(true);
            //if (selectedBoxType === 'Advanced') {
            //    if (selectedLineupPlayer === 'Player') {
            //        setColumns(advancedPlayerColumns);
            //    } else {
            //        setColumns(advancedLineupColumns);
            //    }  
            //}

            if (selectedLineupPlayer === 'Lineups') {

                try {
                    
                    const data = await axios.get(`/api/LeagueDashLineups/${selectedSeason}/${selectedBoxType}/${numPlayers}/desc/${sortField}/${perMode}/${selectedTeam.team_id}`);
                    const newData = data.data;

                    setStatsData(newData);
                    setOriginalData(newData);
                    if (selectedBoxType === 'Advanced') {
                        setColumns(advancedLineupColumns);
                    } else if (selectedBoxType === 'Base') {
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
                } finally {
                    setIsFetching(false);
                }
            }
            else if (selectedLineupPlayer === 'Players') {

                try {
                    const data = await axios.get(`/api/BoxScores/${selectedSeason}/${selectedBoxType}/desc/${sortField}/${perMode}/${selectedTeam.team_id}/${selectedOpponent.team_abbreviation}`);
                    const newData = data.data;
                    
                    setStatsData(newData);
                    setOriginalData(newData);

                    if (selectedBoxType === 'Advanced') {
                        setColumns(advancedPlayerColumns);
                    } else if (selectedBoxType === 'Base') {
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
                } finally {
                    setIsFetching(false);
                }
            }
        }
        if (selectedSeason) {
            getStats()
        }
    }, [numPlayers, selectedLineupPlayer, selectedSeason, selectedBoxType, perMode, selectedTeam, selectedOpponent]);


    return { statsData, isFetching, columns, originalData };
}