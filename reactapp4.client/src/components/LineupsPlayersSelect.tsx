import '../App.css';
import React, { useEffect, useRef, useState } from "react";
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

interface LineupsPlayersSelectProps {
    options: string[];
    selectedLineupPlayer: string;
    setSelectedLineupPlayer: React.Dispatch<React.SetStateAction<string>>;
    setSelectedBoxType: React.Dispatch<React.SetStateAction<string>>;
    setSortField: React.Dispatch<React.SetStateAction<string>>;
}

const LineupsPlayersSelect: React.FC<LineupsPlayersSelectProps> = ({ options, selectedLineupPlayer, setSelectedLineupPlayer, setSelectedBoxType, setSortField }) => {
    const [isOpen, setIsOpen] = useState(false);
    const refOne = useRef<HTMLDivElement>(null);

    const toggleSelect = () => {
        setIsOpen(!isOpen);
        console.log(isOpen);
    };
    function handleOptionChange(option: string) {

        if (option === "0") {
            return;
        }
        if (option === 'Players') {
            setSelectedBoxType('Base');
        }
        setSelectedLineupPlayer(option);
        setSortField("min");
        setIsOpen(!isOpen);
    }

    useEffect(() => {
        function handleClickOutside(event: MouseEvent | TouchEvent) {
            if (
                refOne.current &&
                !refOne.current.contains(event.target as Node) //&&
                //!refTwo.current?.contains(event.target as Node)
            ) {
                setIsOpen(false);
            }
        }

        document.addEventListener('mousedown', handleClickOutside);
        document.addEventListener('touchstart', handleClickOutside); // Add touchstart event listener

        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
            document.removeEventListener('touchstart', handleClickOutside); // Add touchstart event listener

        };
    }, [isOpen, setIsOpen]);

    return (
        <div className="container-div" ref={refOne}>
            <button className="lineup-player-dropdown" type="button" value={selectedLineupPlayer} onClick={toggleSelect}>
                <div className="button-container">
                    <div className="button-text-div">
                        {selectedLineupPlayer}
                    </div>
                    {isOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                </div>
            </button>

            {isOpen && (
                <ul className="styled-ul">
                    {options.map((option: string, index: React.Key | null | undefined) => (
                        <li className='search-list-option' key={index} value={option} onClick={() => handleOptionChange(option)}>{option}</li>
                    ))}
                </ul>
            )}

        </div>
    );
};

export default LineupsPlayersSelect;