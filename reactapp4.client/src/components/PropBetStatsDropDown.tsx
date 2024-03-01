import React, { useState } from 'react';
import { PropBetStats } from '../interfaces/PropBetStats';


interface PropBetStatsDropDownProps {
    selectedStat: PropBetStats | null;
    setSelectedStat: React.Dispatch<React.SetStateAction<PropBetStats | null>>;
}

const PropBetStatsDropDown: React.FC<PropBetStatsDropDownProps> = ({ selectedStat, setSelectedStat }) => {

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
    }

    return (
            <div>
                <select className="drop-flex-select" value={selectedStat ? JSON.stringify(selectedStat) : ''} onChange={handleStatChange}>
                    <option className="drop-flex-option" value="0">Select Stat</option>

                    {stats.map((option, index) => (
                        <option key={index} value={JSON.stringify(option)}>
                            {option.label}
                        </option>
                    ))}

                </select>

            </div>
    );
}

export default PropBetStatsDropDown;