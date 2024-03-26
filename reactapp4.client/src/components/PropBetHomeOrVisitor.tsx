import React, { SetStateAction, useState, useEffect } from 'react';
import { NBATeam } from '../interfaces/Teams';

interface PropBetHomeOrVisitorProps {
    homeOrVisitor: string;
    setHomeOrVisitor: React.Dispatch<SetStateAction<string>>;
    //showOpponent: boolean;
    //setShowOpponent: React.Dispatch<SetStateAction<boolean>>;
}

const PropBetHomeOrVisitor: React.FC<PropBetHomeOrVisitorProps> = ({ homeOrVisitor, setHomeOrVisitor }) => {


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

export default PropBetHomeOrVisitor;
