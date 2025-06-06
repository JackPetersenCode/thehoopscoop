import React, { SetStateAction } from 'react';

interface MLBPropBetHomeOrVisitorProps {
    homeOrVisitor: string;
    setHomeOrVisitor: React.Dispatch<SetStateAction<string>>;
    //showOpponent: boolean;
    //setShowOpponent: React.Dispatch<SetStateAction<boolean>>;
}

const MLBPropBetHomeOrVisitor: React.FC<MLBPropBetHomeOrVisitorProps> = ({ homeOrVisitor, setHomeOrVisitor }) => {


    const deleteHomeOrVisitor = () => {
        setHomeOrVisitor("");
        //setShowOpponent(false);
    }

    return (
        <div>
            {homeOrVisitor !== "" ?
                <div className="over-under-flex">
                    <div className="text-in-box">
                        {homeOrVisitor === "Home" ? "vs." : homeOrVisitor === "Visitor" ? "@" : "All Games"}
                    </div>
                    <div>
                        <button className="x-button" onClick={deleteHomeOrVisitor}>x</button>
                    </div>
                </div>
                :
                ""
            }
        </div>
    );
}

export default MLBPropBetHomeOrVisitor;
