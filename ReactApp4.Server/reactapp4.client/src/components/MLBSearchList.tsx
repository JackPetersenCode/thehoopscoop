import React, { useRef, useEffect } from 'react';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';

interface MLBSearchListProps {
    refTwo: React.RefObject<HTMLDivElement>;
    inputText: string;
    setInputText: React.Dispatch<React.SetStateAction<string>>;
    data: MLBActivePlayer[];
    selectedPlayer: MLBActivePlayer | null;
    setSelectedPlayer: React.Dispatch<React.SetStateAction<MLBActivePlayer | null>>;
    openSearchList: boolean;
    setOpenSearchList: React.Dispatch<React.SetStateAction<boolean>>;
    roster: MLBActivePlayer[];
    setRoster: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
    setUsedPlayers: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
    gameOption: string;
    isBottomFindPlayerInput: boolean;
}

const MLBSearchList: React.FC<MLBSearchListProps> = ({
    refTwo,
    inputText,
    setInputText,
    data,
    selectedPlayer,
    setSelectedPlayer,
    openSearchList,
    setOpenSearchList,
    setRoster,
    setUsedPlayers,
    gameOption,
    isBottomFindPlayerInput
}) => {
    const refOne = useRef<HTMLDivElement>(null);

    useEffect(() => {
        function handleClickOutside(event: MouseEvent | TouchEvent) {
            if (
                refOne.current &&
                !refOne.current.contains(event.target as Node) &&
                !refTwo.current?.contains(event.target as Node)
            ) {
                setOpenSearchList(false);
            }
        }
    
        document.addEventListener('mousedown', handleClickOutside);
        document.addEventListener('touchstart', handleClickOutside); // Add touchstart event listener
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
            document.removeEventListener('touchstart', handleClickOutside); // Remove touchstart event listener on cleanup
        };
    }, [refTwo, setOpenSearchList]);
    

    const filteredData = data.filter((element: MLBActivePlayer) => {

        return element.fullName.toLowerCase().includes(inputText.toLowerCase());
    });

    const handleList = (item: MLBActivePlayer) => {
        setSelectedPlayer(item);
        if (!isBottomFindPlayerInput) {
            if (gameOption == "Head 2 Head") {
                setRoster(roster => [...roster, item]);
            } else if (gameOption == "Prop Bet") {
                setRoster([item]);
            }
        }
        setUsedPlayers(usedPlayers => [...usedPlayers, item]);
        setInputText(item.fullName);
        setOpenSearchList(false);
    };

    return (
        <div>
            {openSearchList && filteredData.length > 0 ?
                <div className="search-list" ref={refOne}>
                    <ul className='search-list-ul'>
                        {filteredData.map((item: MLBActivePlayer, index: number) => (
                            <li key={index} onClick={() => handleList(item)} className="search-list-option">
                                {item.fullName}
                            </li>
                        ))}
                    </ul>
                </div>
                :
                ""
            }
        </div>
    );
    
};

export default MLBSearchList;
