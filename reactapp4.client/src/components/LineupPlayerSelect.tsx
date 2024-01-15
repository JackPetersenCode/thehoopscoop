import axios from "axios";
import '../App.css';
import React, { useEffect, useState } from "react";
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
    width: 100 %;
` 

const StyledUl = styled.ul`
    position: absolute;
    top: 40px;
    left: 0px;
    list-style-type: none;
    padding: 5px;
    padding-left: 10px;
    margin: 0px;
    text-align: left;
`

const ContainerDiv = styled.div`
    position: relative;
`

const ButtonContainer = styled.div`
    display: flex;
    white-space: nowrap;
    align-items: center;
`

const ButtonTextDiv = styled.div`
    padding: 5px;
`
interface LineupPlayerSelectProps {
    options: string[];
    selectedOption: string;
    setSelectedOption: React.Dispatch<React.SetStateAction<string>>;
}

const LineupPlayerSelect: React.FC<LineupPlayerSelectProps> = ({ options, selectedOption, setSelectedOption }) => {
    const [isOpen, setIsOpen] = useState(false);

    const toggleSelect = () => {
        setIsOpen(!isOpen);
        console.log(isOpen);
    };
    function handleOptionChange(option: string) {
        
        if (option === "0") {
            return;
        }
        setSelectedOption(option);
        setIsOpen(!isOpen);
        console.log(selectedOption)
    }

    return (
        <ContainerDiv>
            <StyledButton type="button" value={selectedOption} onClick={toggleSelect}>
                <ButtonContainer>
                    <ButtonTextDiv>
                        {selectedOption}
                    </ButtonTextDiv>
                    {isOpen ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                </ButtonContainer>
            </StyledButton>

            {isOpen && (
                <StyledUl>
                    {options.map((option: string, index: React.Key | null | undefined) => (
                        <li key={index} value={option} onClick={() => handleOptionChange(option)}>{option === 'Base' ? 'Traditional' : option}</li>
                    ))}
                </StyledUl>
            )}

        </ContainerDiv>
    );
};

export default LineupPlayerSelect;