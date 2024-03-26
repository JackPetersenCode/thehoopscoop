import React, { SetStateAction, useState, useEffect } from 'react';
import { NBATeam } from '../interfaces/Teams';
import { nbaTeams } from '../interfaces/DropDownOptions';

interface PropBetOpponentDropDownProps {
    selectedOpponent: NBATeam;
    setSelectedOpponent: React.Dispatch<SetStateAction<NBATeam>>;
    showOpponent: boolean;
    setShowOpponent: React.Dispatch<SetStateAction<boolean>>;
}

const PropBetOpponentDropDown: React.FC<PropBetOpponentDropDownProps> = ({ selectedOpponent, setSelectedOpponent, showOpponent, setShowOpponent }) => {

    function handleChange(event: React.ChangeEvent<HTMLSelectElement>) {
        const selectedValue = JSON.parse(event.target.value);
        console.log(event.target.value);
        console.log('set selected Opponent')
        setSelectedOpponent(selectedValue);
        setShowOpponent(true);

    }

    return (
        <div className="drop-flex">
            
            <div className="drop-title">
                Opponent
            </div>
            
            <select className="drop-flex-select" value={JSON.stringify(selectedOpponent)} onChange={handleChange}>
                <option className="drop-flex-option" value="0">Opponent</option>

                {nbaTeams.map((option, index) => (
                    <option key={index} className="option-select" value={JSON.stringify(option)}>
                        {option.team_name}
                    </option>
                ))}

            </select>
        </div>
    );
}

export default PropBetOpponentDropDown;
