import React from 'react';
import { NBATeam } from '../interfaces/Teams';
import { nbaTeams } from '../interfaces/DropDownOptions'; 
import { ShotChartsGamesData } from '../interfaces/Shot';




interface TeamsDropDownProps {
    selectedTeam: NBATeam | string;
    setSelectedTeam: React.Dispatch<React.SetStateAction<NBATeam | string>>;
    setGameData: React.Dispatch<React.SetStateAction<ShotChartsGamesData[]>>;
}

const TeamsDropDown: React.FC<TeamsDropDownProps> = ({ selectedTeam, setSelectedTeam, setGameData }) => {

    function handleChange(event: React.ChangeEvent<HTMLSelectElement>) {
        if (event.target.value === "0") {
            return;
        }
        const selectedValue = JSON.parse(event.target.value);
        console.log(event.target.value);
        console.log(selectedValue)
        setSelectedTeam(selectedValue);
        setGameData([]);
    }

    return (
        <div className="drop-flex">
            <div className="drop-title">
                Select Team
            </div>
            <select className="drop-flex-select" value={JSON.stringify(selectedTeam)} onChange={handleChange}>
                <option className="drop-flex-option" value="0">Team</option>

                {nbaTeams.map((option, index) => (
                    <option key={index} className="option-select" value={JSON.stringify(option)}>
                        {option.team_name}
                    </option>
                ))}

            </select>
        </div>
    );
}

export default TeamsDropDown;