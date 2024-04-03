import axios from "axios";
import '../App.css';
import React, { useEffect, useState } from "react";
import styled from 'styled-components';
import '../style.css';
interface GameOptionDropDownProps {
    gameOption: string;
    setGameOption: React.Dispatch<React.SetStateAction<string>>;
}

const SeasonsDropDown: React.FC<GameOptionDropDownProps> = ({ gameOption, setGameOption }) => {


    const options = [
        "Prop Bet",
        "Head 2 Head",
        "Shot Charts"
    ];
    function handleGameOptionChange(event: { preventDefault: () => void; target: { value: string; }; }) {
        event.preventDefault();
        if (event.target.value === "0") {
            return;
        }
        setGameOption(event.target.value);
        console.log(gameOption);
    }

    return (
        <div className="game-option-flex">
            <div className="drop-title">
                Tool Option
            </div>
            <div>
                <select className="drop-flex-select" value={gameOption} onChange={handleGameOptionChange}>
                    <option className="drop-flex-option" value="0">Select Tool Option</option>

                    {options.map((option, index) => (
                        <option key={index} value={option}>{option}</option>
                    ))}

                </select>
            </div>
        </div>
    );

};

export default SeasonsDropDown;