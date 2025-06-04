import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

interface SportOptionDropDownProps {
    selectedSport: string;
    setSelectedSport: React.Dispatch<React.SetStateAction<string>>;
}

const SportOptionDropDown: React.FC<SportOptionDropDownProps> = ({ selectedSport, setSelectedSport }) => {
    const navigate = useNavigate();

    const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const sport = e.target.value;
        setSelectedSport(sport);
        if (sport === 'NBA' || sport === 'MLB') {
            navigate(`/${sport}`);
        }
    };

    return (
        <div className="game-option-flex">
            <div className="drop-title">
                Select a Sport
            </div>
            <div>
                <select className="drop-flex-select-blue" value={selectedSport} onChange={handleChange}>
                    <option className="drop-flex-option" value="NBA">NBA</option>
                    <option className="drop-flex-option" value="MLB">MLB</option>
                </select>
            </div>
        </div>
    );
};

export default SportOptionDropDown;
