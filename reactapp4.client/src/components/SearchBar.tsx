import React, { useRef } from 'react';
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
`;

interface SearchBarProps {
    activePlayers: Player[];
    inputText: string;
    setInputText: React.Dispatch<React.SetStateAction<string>>;
    selectedPlayer: string;
    setSelectedPlayer: React.Dispatch<React.SetStateAction<string>>;
}

const SearchBar: React.FC<SearchBarProps> = ({
    activePlayers,
    inputText,
    setInputText,
    selectedPlayer,
    setSelectedPlayer,
}) => {
    const refTwo = useRef<HTMLDivElement>(null);

    const inputHandler = (e: React.ChangeEvent<HTMLInputElement>) => {
        const lowerCase = e.target.value.toLowerCase();
        setInputText(lowerCase);
    };

    const handleEnter = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === 'Enter') {
            console.log(inputText);
            // navigate(`/${inputText}`);
        }
    };

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
                                <Link to={`/${inputText}`}>
                                    <div className="search-icon">
                                        <SearchIcon />
                                    </div>
                                </Link>
                            </InputAdornment>
                        ),
                    }}
                    onKeyDown={handleEnter}
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
                    />
                ) : (
                    ''
                )}
            </DropdownStyle>
        </ContainerDiv>
    );
};

export default SearchBar;
