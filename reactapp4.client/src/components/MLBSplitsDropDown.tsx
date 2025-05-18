import React, { MutableRefObject } from "react";
import { mlbSplitsBatting, mlbSplitsPitching } from "../interfaces/MLBDropDownOptions";

interface MLBSplitsDropDownProps {
    hittingPitching: string;
    selectedSplit: string;
    setSelectedSplit: React.Dispatch<React.SetStateAction<string>>;
    //loading: MutableRefObject<boolean>;
}

const MLBSplitsDropDown: React.FC<MLBSplitsDropDownProps> = React.memo(({ hittingPitching, selectedSplit, setSelectedSplit }) => {
    console.log("drop down")
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
                Splits
            </div>
            <div>
                <select className="drop-flex-select" value={selectedSplit} onChange={handleSplitChange}>
                    <option className="drop-flex-option" value="0">Select Split</option>

                    {hittingPitching === 'pitching' ? mlbSplitsPitching.map((option, index) => (
                        <option key={index} value={option}>{option}</option>
                    ))
                    :
                    mlbSplitsBatting.map((option, index) => (
                        <option key={index} value={option}>{option}</option>
                    ))}

                </select>
            </div>
        </div>
    );
})
export default MLBSplitsDropDown;
