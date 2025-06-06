import React from "react";
import "../style.css";
import { MLBTeam } from "../interfaces/Teams";

interface MLBSelectedTeamDropDownProps {
  selectedTeam: MLBTeam;
  setSelectedTeam: React.Dispatch<React.SetStateAction<MLBTeam>>;
  options: MLBTeam[];
  disabled: boolean;
}

const MLBSelectedTeamDropDown: React.FC<MLBSelectedTeamDropDownProps> = ({
  selectedTeam,
  setSelectedTeam,
  options,
  disabled
}) => {
  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const parsed = JSON.parse(e.target.value) as MLBTeam;
    setSelectedTeam(parsed);
  };

  return (
    <div className="drop-flex">
      <div className="drop-title">Team</div>
      <div>
        <select
          className="drop-flex-select"
          value={JSON.stringify(selectedTeam)}
          onChange={handleChange}
          disabled={disabled}
        >
          {options.map((team, index) => (
            <option
              key={index}
              value={JSON.stringify(team)}
              className="option-select"
            >
              {team.team_name}
            </option>
          ))}
        </select>
      </div>
    </div>
  );
};

export default MLBSelectedTeamDropDown;
