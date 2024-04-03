import axios from "axios";
import '../App.css';
import React, { useEffect, useState, useMemo } from "react";
import styled from 'styled-components';
import '../style.css';
import { NBATeam } from "../interfaces/Teams";
interface DropDownProps {
    options: string[] | NBATeam[];
    setPerMode: React.Dispatch<React.SetStateAction<string>>;
    setNumPlayers: React.Dispatch<React.SetStateAction<string>>;
    setSelectedTeam: React.Dispatch<React.SetStateAction<NBATeam>>;
    dropDownType: string;
}

const DropDown: React.FC<DropDownProps> = React.memo(({ options, setPerMode, setNumPlayers, setSelectedTeam, dropDownType }) => {

    const [selectedOption, setSelectedOption] = useState<string | NBATeam>("");
    function handleChange(event: { preventDefault: () => void; target: { value: string | NBATeam; }; }) {
        event.preventDefault();

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
            //if (dropDownType === "Opponent") {
            //    console.log(event.target.value);
            //    console.log('set selected Opponent')
            //    setSelectedOpponent(JSON.parse(event.target.value));
            //    setShowOpponent(true);
            //}
            //if (dropDownType === "Home or Visitor") {
            //    console.log('booger');
            //    setHomeOrVisitor(event.target.value);
            //}
        }


        setSelectedOption(event.target.value);
        console.log(typeof selectedOption)
        console.log(selectedOption);
    }

    //useMemo(() => {
    //    const resetHomeVisitor = async () => {
    //        setSelectedOption("");
    //    }
    //    if (homeOrVisitor === "") {
    //        resetHomeVisitor();
    //    }
    //}, [homeOrVisitor]);

    return (
        <div className="drop-flex">
            <div className="drop-title">
                {dropDownType}
            </div>
            <div>
                <select className="drop-flex-select" value={typeof selectedOption === 'object' ? selectedOption.team_name : selectedOption} onChange={handleChange}>
                    <option className="drop-flex-option" value="0">Select {dropDownType}</option>

                    {options.map((option, index) => (
                        <option key={index} className="option-select" value={typeof option === 'object' ? JSON.stringify(option) : (option as string)}>
                            {typeof option === 'object' ? (option as NBATeam).team_name : option}
                        </option>
                    ))}

                </select>
            </div>
        </div>
    );

});

export default DropDown;