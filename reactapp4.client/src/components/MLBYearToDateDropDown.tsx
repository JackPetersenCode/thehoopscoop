import React from "react";

interface MLBYearToDateDropDownProps {
    options: string[];
    yearToDateOption: string;
    setYearToDateOption: React.Dispatch<React.SetStateAction<string>>;
}

const MLBYearToDateDropDown: React.FC<MLBYearToDateDropDownProps> = ({ options, yearToDateOption, setYearToDateOption }) => {
    const handleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setYearToDateOption(event.target.value);
    };

    return (
        <div className="drop-flex">
            <div className="drop-title">Year To Date</div>
            <select
                className="drop-flex-select"
                value={yearToDateOption}
                onChange={handleChange}
            >
                {options.map((option, index) => (
                    <option key={index} value={option}>
                        {option}
                    </option>
                ))}
            </select>
        </div>
    );
};

export default MLBYearToDateDropDown;
