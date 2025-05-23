import React, { useRef, useState } from 'react';
import styled from 'styled-components';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';
import MLBSearchList from './MLBSearchList';

const ContainerDiv = styled.div`
    position: relative;
`;


interface MLBFindPlayerBottomProps {
    activePlayers: MLBActivePlayer[];
    inputTextBottom: string;
    setInputTextBottom: React.Dispatch<React.SetStateAction<string>>;
    selectedPlayerBottom: MLBActivePlayer | null;
    setSelectedPlayerBottom: React.Dispatch<React.SetStateAction<MLBActivePlayer | null>>;
    roster: MLBActivePlayer[];
    setRoster: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
    setUsedPlayers: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
    gameOption: string;
}

const MLBFindPlayerBottom: React.FC<MLBFindPlayerBottomProps> = ({
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
                    <MLBSearchList
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

export default MLBFindPlayerBottom;
