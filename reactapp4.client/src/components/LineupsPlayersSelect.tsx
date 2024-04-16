import axios from "axios";
import '../App.css';
import React, { useEffect, useRef, useState } from "react";
import styled from "styled-components";
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

const StyledButton = styled.button`
    position: relative;
    padding: 2px;
    border: solid transparent;
    border - right: 12px solid transparent;
    background - color: rgb(238, 238, 238);
    border - radius: 5px;
    width: 100%;
`

const StyledUl = styled.ul`
    position: absolute;
    top: 40px;
    left: 0px;
    list-style-type: none;
    padding: 10px;
    padding-left: 10px;
    margin: 0px;
    text-align: left;
    background-color: white;
    overflow-y: auto;
    border-radius: 5px;
    box-shadow: 2px 2px 3px rgba(0,0,0,0.1), -2px 2px 3px rgba(0,0,0,0.1);
`

const ContainerDiv = styled.div`
    position: relative;
`

const ButtonContainer = styled.div`
    display: flex;
    white-space: nowrap;
    align-items: center;
    justify-content: space-between;
`

const ButtonTextDiv = styled.div`
    padding: 5px;
`
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
        function handleClickOutside(event: MouseEvent) {
            if (
                refOne.current &&
                !refOne.current.contains(event.target as Node) //&&
                //!refTwo.current?.contains(event.target as Node)
            ) {
                setIsOpen(false);
            }
        }

        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, [isOpen, setIsOpen]);

    return (
        <ContainerDiv ref={refOne}>
            <button className="lineup-player-dropdown" type="button" value={selectedLineupPlayer} onClick={toggleSelect}>
                <ButtonContainer>
                    <ButtonTextDiv>
                        {selectedLineupPlayer}
                    </ButtonTextDiv>
                    {isOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                </ButtonContainer>
            </button>

            {isOpen && (
                <StyledUl>
                    {options.map((option: string, index: React.Key | null | undefined) => (
                        <li key={index} value={option} onClick={() => handleOptionChange(option)}>{option}</li>
                    ))}
                </StyledUl>
            )}

        </ContainerDiv>
    );
};

export default LineupsPlayersSelect;