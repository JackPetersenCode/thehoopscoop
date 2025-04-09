import React from "react";
import { MLBTeam } from "../interfaces/Teams";

interface MLBOpponentDropDownProps {
    options: MLBTeam[];
    selectedOpponent: MLBTeam;
    setSelectedOpponent: React.Dispatch<React.SetStateAction<MLBTeam>>;
}

const MLBOpponentDropDown: React.FC<MLBOpponentDropDownProps> = ({ options, selectedOpponent, setSelectedOpponent }) => {
    const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const parsed = JSON.parse(event.target.value) as MLBTeam;
        setSelectedOpponent(parsed);
    };

    return (
        <div className="drop-flex">
            <div className="drop-title">Opponent</div>
            <select
                className="drop-flex-select"
                value={JSON.stringify(selectedOpponent)}
                onChange={handleChange}
            >
                {options.map((option, index) => (
                    <option key={index} value={JSON.stringify(option)}>
                        {option.team_name}
                    </option>
                ))}
            </select>
        </div>
    );
};

export default MLBOpponentDropDown;
