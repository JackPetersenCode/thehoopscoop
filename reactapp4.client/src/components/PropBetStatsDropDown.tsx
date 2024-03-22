import React, { useState } from 'react';
import { PropBetStats } from '../interfaces/PropBetStats';


interface PropBetStatsDropDownProps {
    selectedStat: PropBetStats | null;
    setSelectedStat: React.Dispatch<React.SetStateAction<PropBetStats | null>>;
    propBetStats: PropBetStats[];
    setPropBetStats: React.Dispatch<React.SetStateAction<PropBetStats[]>>;
}

const PropBetStatsDropDown: React.FC<PropBetStatsDropDownProps> = ({ selectedStat, setSelectedStat, propBetStats, setPropBetStats }) => {

    const stats: PropBetStats[] = [
        { label: 'Points', accessor: 'pts' },
        { label: 'Rebounds', accessor: 'reb' },
        { label: 'Assists', accessor: 'ast' },
        { label: 'Steals', accessor: 'stl' },
        { label: 'Blocks', accessor: 'blk' },
        { label: 'Turnovers', accessor: 'tov' }
    ];
    function handleStatChange(event: React.ChangeEvent<HTMLSelectElement>) {
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


    return (
            <div>
            <select className="drop-flex-select" value={"0"} onChange={handleStatChange}>
                    <option className="drop-flex-option" value="0">Select Stat</option>

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