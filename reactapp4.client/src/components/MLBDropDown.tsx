import '../App.css';
import React, { useState } from "react";
import '../style.css';
import { MLBTeam } from "../interfaces/Teams";
interface MLBDropDownProps {
    options: string[] | MLBTeam[];
    setLeagueOption: React.Dispatch<React.SetStateAction<string>>;
    setSelectedTeam: React.Dispatch<React.SetStateAction<MLBTeam>>;
    setYearToDateOption: React.Dispatch<React.SetStateAction<string>>;
    dropDownType: string;
}

const MLBDropDown: React.FC<MLBDropDownProps> = React.memo(({ options, setLeagueOption, setSelectedTeam, setYearToDateOption, dropDownType }) => {

    const [selectedOption, setSelectedOption] = useState<string | MLBTeam>("");

    function handleChange(event: { preventDefault: () => void; target: { value: string | MLBTeam; }; }) {
        event.preventDefault();

        if (typeof event.target.value === 'string') {

            if (event.target.value === "0") {
                return;
            }
            if (dropDownType === "League Options") {
                setLeagueOption(event.target.value);
            }
            if (dropDownType === "Team") {
                setSelectedTeam(JSON.parse(event.target.value));
            }
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
            <div className="select-wrapper">
                <select title='drop-flex-select' className="drop-flex-select" value={typeof selectedOption === 'string' && selectedOption.includes("team_id") ? 
                    JSON.parse(selectedOption).team_name : selectedOption} onChange={handleChange}>
                    
                    {options.map((option, index) => (
                        <option key={index} className="option-select" value={typeof option === 'object' ? JSON.stringify(option) : (option as string)}>
                            {typeof option === 'object' ? (option as MLBTeam).team_name : option}
                        </option>
                    ))}

                </select>
            </div>
        </div>
    );

});

export default MLBDropDown;