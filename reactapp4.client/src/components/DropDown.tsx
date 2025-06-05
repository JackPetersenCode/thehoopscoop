import '../App.css';
import React, { useState } from "react";
import '../style.css';
import { NBATeam } from "../interfaces/Teams";
interface DropDownProps {
    options: string[] | NBATeam[];
    setPerMode: React.Dispatch<React.SetStateAction<string>>;
    setNumPlayers: React.Dispatch<React.SetStateAction<string>>;
    setSelectedTeam: React.Dispatch<React.SetStateAction<NBATeam>>;
    setSelectedOpponent: React.Dispatch<React.SetStateAction<NBATeam>>;
    dropDownType: string;
    disabled: boolean;
}

const DropDown: React.FC<DropDownProps> = React.memo(({ options, setPerMode, setNumPlayers, setSelectedTeam, 
    setSelectedOpponent, dropDownType, disabled }) => {

    const [selectedOption, setSelectedOption] = useState<string | NBATeam>("");
    function handleChange(event: { preventDefault: () => void; target: { value: string | NBATeam; }; }) {
        event.preventDefault();

        if (typeof event.target.value === 'string') {

            if (event.target.value === "0") {
                return;
            }
            if (dropDownType === "Per Mode") {
                setPerMode(event.target.value);
            }
            if (dropDownType === "# of Players") {
                setNumPlayers(event.target.value);
            }

            if (dropDownType === "Team") {
                setSelectedTeam(JSON.parse(event.target.value));
            }
            if (dropDownType === "Opponent") {
                setSelectedOpponent(JSON.parse(event.target.value));
            }
            //if (dropDownType === "Home or Visitor") {
            //    console.log('booger');
            //    setHomeOrVisitor(event.target.value);
            //}
        }


        setSelectedOption(event.target.value);
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
                <select title='drop-flex-select' className="drop-flex-select" value={typeof selectedOption === 'object' ? selectedOption.team_name : selectedOption} onChange={handleChange} disabled={disabled} >
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