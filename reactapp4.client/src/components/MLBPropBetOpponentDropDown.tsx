import React, { SetStateAction } from 'react';
import { MLBTeam } from '../interfaces/Teams';
import { mlbTeams } from '../interfaces/MLBDropDownOptions';

interface PropBetOpponentDropDownProps {
    selectedOpponent: MLBTeam;
    setSelectedOpponent: React.Dispatch<SetStateAction<MLBTeam>>;
    showOpponent: boolean;
    setShowOpponent: React.Dispatch<SetStateAction<boolean>>;
    disabled: boolean;
}

const MLBPropBetOpponentDropDown: React.FC<PropBetOpponentDropDownProps> = ({ selectedOpponent, setSelectedOpponent, setShowOpponent, disabled }) => {

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

                    {mlbTeams.map((option, index) => (
                        <option key={index} className="option-select" value={JSON.stringify(option)}>
                            {option.team_name}
                        </option>
                    ))}
                </select>
            </div>
        </div>
    );
}

export default MLBPropBetOpponentDropDown;
