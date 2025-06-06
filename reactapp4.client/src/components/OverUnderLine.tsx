import React, { SetStateAction } from 'react';



interface OverUnderLineProps {
    overUnderLine: number | string;
    setOverUnderLine: React.Dispatch<SetStateAction<number | string>>;
}



const OverUnderLine: React.FC<OverUnderLineProps> = ({ overUnderLine, setOverUnderLine }) => {

    const deleteOverUnderLine = () => {
        setOverUnderLine(() => "");
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