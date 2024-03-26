import React, { SetStateAction, useState, useEffect } from 'react';
import { homeOrVisitorOptions, nbaTeams } from '../interfaces/DropDownOptions';

interface HomeOrVisitorDropDownProps {
    homeOrVisitor: string;
    setHomeOrVisitor: React.Dispatch<SetStateAction<string>>;
    showHomeOrVisitor: boolean;
    setShowHomeOrVisitor: React.Dispatch<SetStateAction<boolean>>;
}

const HomeOrVisitorDropDown: React.FC<HomeOrVisitorDropDownProps> = ({ homeOrVisitor, setHomeOrVisitor, showHomeOrVisitor, setShowHomeOrVisitor }) => {

    function handleChange(event: React.ChangeEvent<HTMLSelectElement>) {
        console.log('booger');
        setHomeOrVisitor(event.target.value);
        setShowHomeOrVisitor(true);

    }

    return (
        <div className="drop-flex">

            <div className="drop-title">
                Home or Visitor
            </div>

            <select className="drop-flex-select" value={homeOrVisitor} onChange={handleChange}>
                <option className="drop-flex-option" value="0">Home or Visitor</option>

                {homeOrVisitorOptions.map((option, index) => (
                    <option key={index} className="option-select" value={option}>
                        {option}
                    </option>
                ))}

            </select>
        </div>
    );
}

export default HomeOrVisitorDropDown;
