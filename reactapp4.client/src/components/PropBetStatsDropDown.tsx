import React from 'react';
import { PropBetStats } from '../interfaces/PropBetStats';


interface PropBetStatsDropDownProps {
    selectedStat: PropBetStats | null;
    setSelectedStat: React.Dispatch<React.SetStateAction<PropBetStats | null>>;
    propBetStats: PropBetStats[];
    setPropBetStats: React.Dispatch<React.SetStateAction<PropBetStats[]>>;
    disabled: boolean;
}

const PropBetStatsDropDown: React.FC<PropBetStatsDropDownProps> = ({ setSelectedStat, propBetStats, setPropBetStats, disabled }) => {

    const stats: PropBetStats[] = [
        { label: 'Points', accessor: 'pts' },
        { label: 'Rebounds', accessor: 'reb' },
        { label: 'Assists', accessor: 'ast' },
        { label: 'Steals', accessor: 'stl' },
        { label: 'Blocks', accessor: 'blk' },
        { label: 'Turnovers', accessor: 'tov' },
        { label: '3 Pointers Made', accessor: 'fg3m' }
    ];
    
    const handleStatChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        event.preventDefault();

        const selectedValue = JSON.parse(event.target.value);
        setSelectedStat(selectedValue);
        if (propBetStats.some(stat => stat.label === selectedValue.label)) {
            return;
        } else {
            setPropBetStats(propBetStats => [...propBetStats, selectedValue]);
        }
    }


    return (
        <div className="drop-flex">
            <div className="drop-title">
                Prop Bet Stats
            </div>
            <div className="select-wrapper">
                <select className="drop-flex-select" value={"0"} onChange={handleStatChange} disabled={disabled} >
                        <option className="drop-flex-option" value="0">Prop Bet Stats</option>

                    {stats.map((option: PropBetStats, index: number) => (
                            <option key={index} value={JSON.stringify(option)} >
                                {option.label}
                            </option>
                        ))}

                </select>
            </div>
        </div>
    );
}

export default PropBetStatsDropDown;