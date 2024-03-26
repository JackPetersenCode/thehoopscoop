import React from 'react';

interface OverUnderLineInputProps {
    overUnderLine: number | string;
    setOverUnderLine: React.Dispatch<React.SetStateAction<number | string>>;
}

const OverUnderLineInput: React.FC<OverUnderLineInputProps> = ({ overUnderLine, setOverUnderLine }) => {

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {

        if (event.target.value === "") {
            setOverUnderLine("");
        } else {
            setOverUnderLine(parseFloat(event.target.value));
        }
    };

    return (
        <div className="drop-flex">
            <div className="drop-title">
                Over Under Line
            </div>
            
            <input type="number" className="input-box" min="0" max="100" value={typeof overUnderLine == 'number' ? overUnderLine : ""} onChange={handleInputChange} placeholder="Over Under" />
        </div>
    );
}

export default OverUnderLineInput;