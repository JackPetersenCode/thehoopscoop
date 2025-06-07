import React from 'react';

interface OverUnderLineInputProps {
    overUnderLine: number | string | null;
    setOverUnderLine: React.Dispatch<React.SetStateAction<number | string | null>>;
}

const OverUnderLineInput: React.FC<OverUnderLineInputProps> = ({ overUnderLine, setOverUnderLine }) => {

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const value = event.target.value;

        if (value === "" || isNaN(Number(value))) {
            setOverUnderLine(null); // or null, depending on your preferred default
        } else {
            setOverUnderLine(parseFloat(value));
        }
    };

    return (
        <div className="drop-flex">
            <div className="drop-title">
                Over Under Line
            </div>
            <input
              type="number"
              className="input-box"
              min="0"
              max="100"
              value={typeof overUnderLine === 'number' ? overUnderLine : undefined}
              onChange={handleInputChange}
              placeholder="Over Under"
            />
        </div>
    );
}

export default OverUnderLineInput;