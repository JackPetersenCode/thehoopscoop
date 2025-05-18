import { useEffect, useState } from 'react';
import axios from 'axios';
import { Column, Stats } from '../interfaces/StatsTable';
import { mlbBattingColumns, mlbPitchingColumns } from '../interfaces/Columns';
import { MLBTeam } from '../interfaces/Teams';

interface Props {
    selectedSeason: string;
    hittingPitching: string;
    leagueOption: string;
    yearToDateOption: string;
    selectedTeam: MLBTeam;
    selectedOpponent: MLBTeam;
    sortField: string;
    selectedSplit: string;
    //setLoading: React.Dispatch<React.SetStateAction<boolean>>;
}

export function MLBStatsData({
    selectedSeason,
    hittingPitching,
    leagueOption,
    yearToDateOption,
    selectedTeam,
    selectedOpponent,
    sortField,
    selectedSplit
    //setLoading,
}: Props) {
    const [statsData, setStatsData] = useState<Stats[]>([]);
    const [columns, setColumns] = useState<Column[]>([]);
    const [isFetching, setIsFetching] = useState(false);
    const [originalData, setOriginalData] = useState<Stats[]>([]);

    useEffect(() => {
        const getStats = async () => {
            console.log('api call')
            setIsFetching(true);
            try {
                let url = '';
                if (hittingPitching === 'hitting') {
                    url = ['vs. LHP', 'vs. RHP'].includes(selectedSplit)
                        ? `/api/MLBStats/batting/splits/${selectedSeason}`
                        : `/api/MLBStats/batting/${selectedSeason}`;
                } else {
                    url = ['vs. LHB', 'vs. RHB'].includes(selectedSplit)
                        ? `/api/MLBStats/pitching/splits/${selectedSeason}`
                        : `/api/MLBStats/pitching/${selectedSeason}`;
                }
                const response = await axios.get(url, {
                    params: {
                        leagueOption,
                        yearToDateOption,
                        selectedTeam: selectedTeam.team_id,
                        selectedOpponent: selectedOpponent.team_id,
                        order: 'desc',
                        sortField,
                        selectedSplit,
                    },
                });
                if (response.data) {
                    const newData = response.data;
                    const newColumns = hittingPitching === 'hitting' ? mlbBattingColumns : mlbPitchingColumns;                          
                    setStatsData(newData);
                    setOriginalData(newData);
                    setColumns(newColumns);
                }
            } catch (error) {
                console.error(error);
            } finally {
                setIsFetching(false);
            }
        };

        if (selectedSeason) getStats();
    }, [
        selectedSeason,
        hittingPitching,
        leagueOption,
        yearToDateOption,
        selectedTeam,
        selectedOpponent,
        sortField,
        selectedSplit,
    ]);

    return { statsData, columns, isFetching, originalData };
}   
