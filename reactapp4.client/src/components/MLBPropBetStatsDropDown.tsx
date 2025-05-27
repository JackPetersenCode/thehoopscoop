import React from 'react';
import { PropBetStats } from '../interfaces/PropBetStats';


interface PropBetStatsDropDownProps {
    selectedStat: PropBetStats | null;
    setSelectedStat: React.Dispatch<React.SetStateAction<PropBetStats | null>>;
    propBetStats: PropBetStats[];
    setPropBetStats: React.Dispatch<React.SetStateAction<PropBetStats[]>>;
    hittingPitching: string;
}

const PropBetStatsDropDown: React.FC<PropBetStatsDropDownProps> = ({ setSelectedStat, propBetStats, setPropBetStats, hittingPitching }) => {


    const hittingStats: PropBetStats[] = [
        { label: 'Hits', accessor: 'hits' },
        { label: 'Total Bases', accessor: 'total_bases' },
        { label: 'Singles', accessor: 'singles' },
        { label: 'Doubles', accessor: 'doubles' },
        { label: 'Triples', accessor: 'triples' },
        { label: 'Home Runs', accessor: 'home_runs' },
        { label: 'Strikeouts', accessor: 'strikeouts' },
        { label: 'Walks', accessor: 'base_on_balls' },
        { label: 'RBI', accessor: 'rbi' },
        { label: 'Runs', accessor: 'runs' },
        { label: 'Stolen Bases', accessor: 'stolen_bases' }
    ];

    const pitchingStats: PropBetStats[] = [
        { label: 'Strikeouts', accessor: 'strikeouts' },
        { label: 'Outs Recorded', accessor: 'outs' },
        { label: 'Wins', accessor: 'wins' },
        { label: 'Hits', accessor: 'hits' },
        { label: 'Earned Runs', accessor: 'earned_runs' },
        { label: 'Walks', accessor: 'walks' },
        { label: '3 Pointers Made', accessor: 'fg3m' }
    ];
    
    const handleStatChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        event.preventDefault();

        const selectedValue = JSON.parse(event.target.value);
        setSelectedStat(selectedValue);
        console.log(selectedValue);
        if (propBetStats.some(stat => stat.label === selectedValue.label)) {
            return;
        } else {
            setPropBetStats(propBetStats => [...propBetStats, selectedValue]);
            console.log(selectedValue);
        }
    }

    const stats = hittingPitching === "pitching" ? pitchingStats : hittingStats;

    return (
        <div className="drop-flex">
            <div className="drop-title">
                Prop Bet Stats
            </div>
            <select className="drop-flex-select" value={"0"} onChange={handleStatChange}>
                    <option className="drop-flex-option" value="0">Prop Bet Stats</option>

                {stats.map((option: PropBetStats, index: number) => (
                        <option key={index} value={JSON.stringify(option)} >
                            {option.label}
                        </option>
                    ))}

                </select>

            </div>
    );
}

export default PropBetStatsDropDown;