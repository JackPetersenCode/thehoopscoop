import React, { SetStateAction } from 'react';
import { NBATeam } from '../interfaces/Teams';
import { nbaTeams } from '../interfaces/DropDownOptions';

interface PropBetOpponentDropDownProps {
    selectedOpponent: NBATeam;
    setSelectedOpponent: React.Dispatch<SetStateAction<NBATeam>>;
    showOpponent: boolean;
    setShowOpponent: React.Dispatch<SetStateAction<boolean>>;
    disabled: boolean;
}

const PropBetOpponentDropDown: React.FC<PropBetOpponentDropDownProps> = ({ selectedOpponent, setSelectedOpponent, setShowOpponent, disabled }) => {

    function handleChange(event: React.ChangeEvent<HTMLSelectElement>) {
        const selectedValue = JSON.parse(event.target.value);
        setSelectedOpponent(selectedValue);
        setShowOpponent(true);
    }

    return (
        <div className="drop-flex">
            
            <div className="drop-title">
                Opponent
            </div>
            <div className="select-wrapper">
                <select className="drop-flex-select" value={JSON.stringify(selectedOpponent)} onChange={handleChange} disabled={disabled} >
                    <option className="drop-flex-option" value="0">Opponent</option>
        
                    {nbaTeams.map((option, index) => (
                        <option key={index} className="option-select" value={JSON.stringify(option)}>
                            {option.team_name}
                        </option>
                    ))}
    
                </select>
            </div>
        </div>
    );
}

export default PropBetOpponentDropDown;
