import React from "react";
import "../style.css";
import { MLBTeam } from "../interfaces/Teams";

interface LeagueDropDownProps {
    options: string[];
    leagueOption: string;
    setLeagueOption: React.Dispatch<React.SetStateAction<string>>;
    setSelectedTeam: React.Dispatch<React.SetStateAction<MLBTeam>>;
}

const MLBLeagueOptionDropDown: React.FC<LeagueDropDownProps> = ({ options, leagueOption, setLeagueOption, setSelectedTeam }) => {

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setLeagueOption(e.target.value);
        if (leagueOption === "American League") {
            setSelectedTeam({ team_id: "1", team_name: "All American League" });    
        } else if (leagueOption === "National League") {
            setSelectedTeam({ team_id: "1", team_name: "All National League" });
        } else if (leagueOption === "MLB") {
            setSelectedTeam({ team_id: "1", team_name: "All MLB Teams" });
        } else {
            console.log('bad league selection')
            return;
        }
  };

  return (
    <div className="drop-flex">
      <div className="drop-title">League</div>
      <div>
        <select
          className="drop-flex-select"
          value={leagueOption}
          onChange={handleChange}
        >
          {options.map((option, idx) => (
            <option key={idx} value={option}>
              {option}
            </option>
          ))}
        </select>
      </div>
    </div>
  );
};

export default MLBLeagueOptionDropDown;
