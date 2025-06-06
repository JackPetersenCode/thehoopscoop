import React, { useRef, useState } from 'react';
import MLBSearchList from './MLBSearchList';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';



interface MLBSearchBarProps {
    activePlayers: MLBActivePlayer[];
    inputText: string;
    setInputText: React.Dispatch<React.SetStateAction<string>>;
    selectedPlayer: MLBActivePlayer | null;
    setSelectedPlayer: React.Dispatch<React.SetStateAction<MLBActivePlayer | null>>;
    roster: MLBActivePlayer[];
    setRoster: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
    setUsedPlayers: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
    gameOption: string;
}

const MLBSearchBar: React.FC<MLBSearchBarProps> = ({
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
        <div className='container-div' ref={refTwo}>
            <div className="drop-title">
                Find Player
            </div>
            <div>
                <input type="text" className="input-box" value={inputText} placeholder="Find Player" onChange={inputHandler} onKeyDown={handleEnter} onClick={handleClick} />
            </div>
            <div className='drop-down-style'>
                {activePlayers.length > 0 && inputText.length > 0 ? (
                    <MLBSearchList
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
                        isBottomFindPlayerInput={false}
                    />
                ) : (
                    ''
                )}
            </div>
        </div>
    );
};

export default MLBSearchBar;
