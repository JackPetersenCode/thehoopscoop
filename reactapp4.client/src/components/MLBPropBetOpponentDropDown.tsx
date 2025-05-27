import React, { SetStateAction } from 'react';
import { MLBTeam } from '../interfaces/Teams';
import { mlbTeams } from '../interfaces/MLBDropDownOptions';

interface PropBetOpponentDropDownProps {
    selectedOpponent: MLBTeam;
    setSelectedOpponent: React.Dispatch<SetStateAction<MLBTeam>>;
    showOpponent: boolean;
    setShowOpponent: React.Dispatch<SetStateAction<boolean>>;
}

const MLBPropBetOpponentDropDown: React.FC<PropBetOpponentDropDownProps> = ({ selectedOpponent, setSelectedOpponent, setShowOpponent }) => {

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

                {mlbTeams.map((option, index) => (
                    <option key={index} className="option-select" value={JSON.stringify(option)}>
                        {option.team_name}
                    </option>
                ))}

            </select>
        </div>
    );
}

export default MLBPropBetOpponentDropDown;
