import React from 'react';

interface OverUnderLineInputProps {
    overUnderLine: number | null;
    setOverUnderLine: React.Dispatch<React.SetStateAction<number | null>>;
}

const OverUnderLineInput: React.FC<OverUnderLineInputProps> = ({ overUnderLine, setOverUnderLine }) => {

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = parseFloat(event.target.value);
        setOverUnderLine(isNaN(newValue) ? null : newValue);
    };

    return (
        <div>
            <input type="number" value={overUnderLine || ""} onChange={handleInputChange} />
        </div>
    );
}

export default OverUnderLineInput;