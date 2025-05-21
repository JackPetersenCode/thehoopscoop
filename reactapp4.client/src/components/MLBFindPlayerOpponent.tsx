import React, { useRef, useState } from 'react';
import styled from 'styled-components';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';
import MLBSearchList from './MLBSearchList';

const ContainerDiv = styled.div`
    position: relative;
`;


interface MLBFindPlayerBottomProps {
    activePlayers: MLBActivePlayer[];
    inputTextOpponent: string;
    setInputTextOpponent: React.Dispatch<React.SetStateAction<string>>;
    selectedPlayerOpponent: MLBActivePlayer | null;
    setSelectedPlayerOpponent: React.Dispatch<React.SetStateAction<MLBActivePlayer | null>>;
    roster: MLBActivePlayer[];
    setRoster: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
    setUsedPlayers: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
    gameOption: string;
}

const MLBFindPlayerBottom: React.FC<MLBFindPlayerBottomProps> = ({
    activePlayers,
    inputTextOpponent,
    setInputTextOpponent,
    selectedPlayerOpponent,
    setSelectedPlayerOpponent,
    roster,
    setRoster,
    setUsedPlayers,
    gameOption
}) => {
    const refTwo = useRef<HTMLDivElement>(null);
    const [openSearchListBottom, setOpenSearchListBottom] = useState(false);
    

    const inputHandler = (e: React.ChangeEvent<HTMLInputElement>) => {
        setInputTextOpponent(e.target.value);
        if (!openSearchListBottom) {
            setOpenSearchListBottom(true);
        }
    };

    const handleEnter = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === 'Enter') {
            console.log(inputTextOpponent);
            // navigate(`/${inputText}`);
        }
    };

    const handleClick = () => {
        setOpenSearchListBottom(true);
    }

    return (
        <ContainerDiv ref={refTwo}>
            <div className="drop-title">
                Find Opponent
            </div>
            {selectedPlayerOpponent ?
                <div className="pitcher-chip">
                  <span>{selectedPlayerOpponent.fullName}</span>
                  <button onClick={() => {
                    setSelectedPlayerOpponent(null);
                    setInputTextOpponent("");
                  }}>
                    ×
                  </button>
                </div>
            :
            <>
                <input type="text" className="input-box" value={inputTextOpponent} placeholder="Find Player" onChange={inputHandler} onKeyDown={handleEnter} onClick={handleClick} />

                <div className='search-list-style'>
                    {activePlayers.length > 0 && inputTextOpponent.length > 0 ? (
                        <MLBSearchList
                            refTwo={refTwo}
                            inputText={inputTextOpponent}
                            setInputText={setInputTextOpponent}
                            data={activePlayers}
                            selectedPlayer={selectedPlayerOpponent}
                            setSelectedPlayer={setSelectedPlayerOpponent}
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
            </>}
        </ContainerDiv>
    );
};

export default MLBFindPlayerBottom;
