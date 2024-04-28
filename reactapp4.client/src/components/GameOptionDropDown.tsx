import '../App.css';
import React from "react";
import '../style.css';
interface GameOptionDropDownProps {
    gameOption: string;
    setGameOption: React.Dispatch<React.SetStateAction<string>>;
}

const SeasonsDropDown: React.FC<GameOptionDropDownProps> = ({ gameOption, setGameOption }) => {


    const options = [
        "Prop Bet",
        "Head 2 Head",
        "Shot Charts",
        "Legacy Predictions"
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
                <select className="drop-flex-select-blue" value={gameOption} onChange={handleGameOptionChange}>
                    <option className="drop-flex-option" value="0">Select Tool Option</option>

                    {options.map((option, index) => (
                        <option className="drop-flex-option" key={index} value={option}>{option}</option>
                    ))}

                </select>
            </div>
        </div>
    );

};

export default SeasonsDropDown;