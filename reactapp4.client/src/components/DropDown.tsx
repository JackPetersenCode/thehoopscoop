import axios from "axios";
import '../App.css';
import React, { useEffect, useState } from "react";
import styled from 'styled-components';
import '../style.css';
import { NBATeam } from "../interfaces/Teams";
interface DropDownProps {
    options: string[] | NBATeam[];
    perMode: string;
    setPerMode: React.Dispatch<React.SetStateAction<string>>;
    numPlayers: string;
    setNumPlayers: React.Dispatch<React.SetStateAction<string>>;
    selectedTeam: NBATeam;
    setSelectedTeam: React.Dispatch<React.SetStateAction<NBATeam>>;
    dropDownType: string;
    setSelectedOpponent: React.Dispatch<React.SetStateAction<NBATeam>>;
    setHomeOrVisitor: React.Dispatch<React.SetStateAction<string>>;
}

const DropDown: React.FC<DropDownProps> = ({ options, perMode, setPerMode, numPlayers, setNumPlayers, selectedTeam, setSelectedTeam, dropDownType, setSelectedOpponent, setHomeOrVisitor }) => {

    const [selectedOption, setSelectedOption] = useState<string | NBATeam>("");
    function handleChange(event: { preventDefault: () => void; target: { value: string | NBATeam; }; }) {
        event.preventDefault();

        console.log(event.target.value);
        console.log(dropDownType);

        if (typeof event.target.value === 'string') {
            console.log(typeof event.target.value);
            console.log(event.target.value);

            if (event.target.value === "0") {
                return;
            }
            if (dropDownType === "Per Mode") {
                console.log(event.target.value);
                setPerMode(event.target.value);
                console.log(dropDownType);
            }
            if (dropDownType === "# of Players") {
                console.log(event.target.value);
                setNumPlayers(event.target.value);
            }

            if (dropDownType === "Team") {
                console.log(event.target.value);
                setSelectedTeam(JSON.parse(event.target.value));
            }
            if (dropDownType === "Opponent") {
                console.log(event.target.value);
                setSelectedOpponent(JSON.parse(event.target.value));
            }
            if (dropDownType === "Home or Visitor") {
                console.log('booger');
                setHomeOrVisitor(event.target.value);
            }
        }
        setSelectedOption(event.target.value);
    }

    return (
        <div className="drop-flex">
            {dropDownType === "Opponent" ?
                ""
                :
                <div className="drop-title">
                    {dropDownType}
                </div>
            }
            <div>
                <select className="drop-flex-select" value={typeof selectedOption === 'object' ? selectedOption.team_name : selectedOption} onChange={handleChange}>
                    <option className="drop-flex-option" value="0">Select {dropDownType}</option>

                    {options.map((option, index) => (
                        <option key={index} value={typeof option === 'object' ? JSON.stringify(option) : (option as string)}>
                            {typeof option === 'object' ? (option as NBATeam).team_name : option}
                        </option>
                    ))}

                </select>
            </div>
        </div>
    );

};

export default DropDown;