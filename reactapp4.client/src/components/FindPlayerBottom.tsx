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


interface FindPlayerBottomProps {
    activePlayers: Player[];
    inputTextBottom: string;
    setInputTextBottom: React.Dispatch<React.SetStateAction<string>>;
    selectedPlayerBottom: Player | null;
    setSelectedPlayerBottom: React.Dispatch<React.SetStateAction<Player | null>>;
    roster: Player[];
    setRoster: React.Dispatch<React.SetStateAction<Player[]>>;
    setUsedPlayers: React.Dispatch<React.SetStateAction<Player[]>>;
    gameOption: string;
}

const FindPlayerBottom: React.FC<FindPlayerBottomProps> = ({
    activePlayers,
    inputTextBottom,
    setInputTextBottom,
    selectedPlayerBottom,
    setSelectedPlayerBottom,
    roster,
    setRoster,
    setUsedPlayers,
    gameOption
}) => {
    const refTwo = useRef<HTMLDivElement>(null);
    const [openSearchListBottom, setOpenSearchListBottom] = useState(false);
    

    const inputHandler = (e: React.ChangeEvent<HTMLInputElement>) => {
        setInputTextBottom(e.target.value);
        if (!openSearchListBottom) {
            setOpenSearchListBottom(true);
        }
    };

    const handleEnter = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === 'Enter') {
            console.log(inputTextBottom);
            // navigate(`/${inputText}`);
        }
    };

    const handleClick = () => {
        setOpenSearchListBottom(true);
    }

    return (
        <ContainerDiv ref={refTwo}>
            <div className="drop-title">
                Find Player
            </div>
            <input type="text" className="input-box" value={inputTextBottom} placeholder="Find Player" onChange={inputHandler} onKeyDown={handleEnter} onClick={handleClick} />

            <div className='search-list-style'>
                {activePlayers.length > 0 && inputTextBottom.length > 0 ? (
                    <SearchList
                        refTwo={refTwo}
                        inputText={inputTextBottom}
                        setInputText={setInputTextBottom}
                        data={activePlayers}
                        selectedPlayer={selectedPlayerBottom}
                        setSelectedPlayer={setSelectedPlayerBottom}
                        openSearchList={openSearchListBottom}
                        setOpenSearchList={setOpenSearchListBottom}
                        roster={roster}
                        setRoster={setRoster}
                        setUsedPlayers={setUsedPlayers}
                        gameOption={gameOption}
                        isBottomFindPlayerInput={true}
                    />
                ) : (
                    ''
                )}
            </div>
        </ContainerDiv>
    );
};

export default FindPlayerBottom;
