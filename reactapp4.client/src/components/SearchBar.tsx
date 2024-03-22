import React, { useRef, useState } from 'react';
import TextField from '@mui/material/TextField';
import InputAdornment from '@mui/material/InputAdornment';
import SearchIcon from '@mui/icons-material/Search';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { Player } from '../interfaces/Player';
import SearchList from './SearchList';

const ContainerDiv = styled.div`
    position: relative;
`;

const TextDiv = styled.div``;

const DropdownStyle = styled.div`
    position: absolute;
    z-index: 5;
`;

interface SearchBarProps {
    activePlayers: Player[];
    inputText: string;
    setInputText: React.Dispatch<React.SetStateAction<string>>;
    selectedPlayer: Player | null;
    setSelectedPlayer: React.Dispatch<React.SetStateAction<Player | null>>;
    roster: Player[];
    setRoster: React.Dispatch<React.SetStateAction<Player[]>>;
    setUsedPlayers: React.Dispatch<React.SetStateAction<Player[]>>;
    gameOption: string;
}

const SearchBar: React.FC<SearchBarProps> = ({
    activePlayers,
    inputText,
    setInputText,
    selectedPlayer,
    setSelectedPlayer,
    roster,
    setRoster,
    setUsedPlayers,
    gameOption
}) => {
    const refTwo = useRef<HTMLDivElement>(null);
    const [openSearchList, setOpenSearchList] = useState(false);

    const inputHandler = (e: React.ChangeEvent<HTMLInputElement>) => {
        setInputText(e.target.value);
        if (!openSearchList) {
            setOpenSearchList(true);
        }
    };

    const handleEnter = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === 'Enter') {
            console.log(inputText);
            // navigate(`/${inputText}`);
        }
    };

    const handleClick = () => {
        setOpenSearchList(true);
    }

    return (
        <ContainerDiv ref={refTwo}>
            <TextDiv>
                <TextField
                    fullWidth
                    id="outlined-basic"
                    onChange={inputHandler}
                    variant="outlined"
                    label="Find Player"
                    size="small"
                    style={{ backgroundColor: 'white', borderRadius: '5px' }}
                    value={inputText}
                    InputProps={{
                        endAdornment: (
                            <InputAdornment position="start">

                                <div className="search-icon">
                                    <SearchIcon />
                                </div>

                            </InputAdornment>
                        ),
                    }}
                    onKeyDown={handleEnter}
                    onClick={handleClick}
                />
            </TextDiv>
            <DropdownStyle>
                {activePlayers.length > 0 && inputText.length > 0 ? (
                    <SearchList
                        refTwo={refTwo}
                        inputText={inputText}
                        setInputText={setInputText}
                        data={activePlayers}
                        selectedPlayer={selectedPlayer}
                        setSelectedPlayer={setSelectedPlayer}
                        openSearchList={openSearchList}
                        setOpenSearchList={setOpenSearchList}
                        roster={roster}
                        setRoster={setRoster}
                        setUsedPlayers={setUsedPlayers}
                        gameOption={gameOption}
                    />
                ) : (
                    ''
                )}
            </DropdownStyle>
        </ContainerDiv>
    );
};

export default SearchBar;
