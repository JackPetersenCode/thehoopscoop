import React from 'react';

interface OverUnderLineInputProps {
    overUnderLine: number;
    setOverUnderLine: React.Dispatch<React.SetStateAction<number>>;
}

const OverUnderLineInput: React.FC<OverUnderLineInputProps> = ({ overUnderLine, setOverUnderLine }) => {

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = parseFloat(event.target.value);
        setOverUnderLine(newValue);
    };

    return (
        <div className="form-group">
            <input type="number" className="form-control border-radius-5 placeholder-gray" value={overUnderLine} onChange={handleInputChange} placeholder="Over Under" />
        </div>
    );
}

export default OverUnderLineInput;