import React from "react";
import { mlbSplitsBatting, mlbSplitsPitching } from "../interfaces/MLBDropDownOptions";

interface MLBSplitsDropDownProps {
    hittingPitching: string;
    selectedSplit: string;
    setSelectedSplit: React.Dispatch<React.SetStateAction<string>>;
    disabled: boolean;
    //splitOptions: string[];
}

const MLBSplitsDropDown: React.FC<MLBSplitsDropDownProps> = React.memo(({ hittingPitching, selectedSplit, setSelectedSplit, disabled }) => {
    const handleSplitChange = (event: { preventDefault: () => void; target: { value: string; }; }) => {
        event.preventDefault();
        if (event.target.value === "0") {
            return;
        }
        setSelectedSplit(event.target.value);
    }
    
    const splitOptions = hittingPitching === 'pitching' ? mlbSplitsPitching : mlbSplitsBatting;

    return (
        <div className="drop-flex">
            <div className="drop-title">
                Splits
            </div>
            <div className="select-wrapper">
                <select className="drop-flex-select" value={selectedSplit} onChange={handleSplitChange} disabled={disabled}>
                    <option className="drop-flex-option" value="0">Select Split</option>

                    {splitOptions.map((option, index) => (
                        <option key={option} value={option}>{option}</option>
                    ))}

                </select>
            </div>
        </div>
    );
})
export default MLBSplitsDropDown;
