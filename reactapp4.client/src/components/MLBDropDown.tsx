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

        console.log(dropDownType);

        if (typeof event.target.value === 'string') {
            console.log(typeof event.target.value);
            console.log(event.target.value);

            if (event.target.value === "0") {
                return;
            }
            if (dropDownType === "League Options") {
                console.log(event.target.value);
                setLeagueOption(event.target.value);
                console.log(dropDownType);
            }
            if (dropDownType === "Team") {
                console.log(event.target.value);
                setSelectedTeam(JSON.parse(event.target.value));
                console.log(dropDownType);
            }
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
                <select title='drop-flex-select' className="drop-flex-select" value={typeof selectedOption === 'object' ? selectedOption.team_name : selectedOption} onChange={handleChange}>
                    <option className="drop-flex-option" value="0">Select {dropDownType}</option>

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