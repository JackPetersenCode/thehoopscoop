import React, { SetStateAction } from 'react';



interface OverUnderLineProps {
    overUnderLine: number | string | null;
    setOverUnderLine: React.Dispatch<SetStateAction<number | string | null>>;
}



const OverUnderLine: React.FC<OverUnderLineProps> = ({ overUnderLine, setOverUnderLine }) => {

    const deleteOverUnderLine = () => {
        setOverUnderLine(0); // Set to 0 instead of ""
    }

    return (
            <div className="over-under-flex">
                 
                <div className="text-in-box">
                    <span>{">= "}</span>
                    <span className="neon-orange">{overUnderLine}</span>
                </div>
                <div>
                    <button className="x-button" onClick={() => deleteOverUnderLine()}>x</button>
                </div>
            </div>
        
    );
}

export default OverUnderLine;