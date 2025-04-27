import React from "react";
import { mlbSplits } from "../interfaces/MLBDropDownOptions";

interface MLBSplitsDropDownProps {
    selectedSplit: string;
    setSelectedSplit: React.Dispatch<React.SetStateAction<string>>;
}

const MLBSplitsDropDown: React.FC<MLBSplitsDropDownProps> = ({ selectedSplit, setSelectedSplit }) => {
    const handleSplitChange = (event: { preventDefault: () => void; target: { value: string; }; }) => {
        event.preventDefault();
        if (event.target.value === "0") {
            return;
        }
        console.log(event.target.value);
        setSelectedSplit(event.target.value);
    }
    
    return (
        <div className="drop-flex">
            <div className="drop-title">
                Season
            </div>
            <div>
                <select className="drop-flex-select" value={selectedSplit} onChange={handleSplitChange}>
                    <option className="drop-flex-option" value="0">Select Season</option>

                    {mlbSplits.map((option, index) => (
                        <option key={index} value={option}>{option}</option>
                    ))}

                </select>
            </div>
        </div>
    );
}
export default MLBSplitsDropDown;
