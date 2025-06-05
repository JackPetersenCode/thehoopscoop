import React, { useRef, useEffect } from 'react';
import { Player } from '../interfaces/Player';

interface SearchListProps {
    refTwo: React.RefObject<HTMLDivElement>;
    inputText: string;
    setInputText: React.Dispatch<React.SetStateAction<string>>;
    data: Player[];
    selectedPlayer: Player | null;
    setSelectedPlayer: React.Dispatch<React.SetStateAction<Player | null>>;
    openSearchList: boolean;
    setOpenSearchList: React.Dispatch<React.SetStateAction<boolean>>;
    roster: Player[];
    setRoster: React.Dispatch<React.SetStateAction<Player[]>>;
    setUsedPlayers: React.Dispatch<React.SetStateAction<Player[]>>;
    gameOption: string;
    isBottomFindPlayerInput: boolean;
}

const SearchList: React.FC<SearchListProps> = ({
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
    

    const filteredData = data.filter((element: Player) => {

        return element.full_name.toLowerCase().includes(inputText.toLowerCase());
    });

    const handleList = (item: Player) => {
        setSelectedPlayer(item);
        if (!isBottomFindPlayerInput) {
            if (gameOption == "Head 2 Head") {
                setRoster(roster => [...roster, item]);
            } else if (gameOption == "Prop Bet") {
                setRoster([item]);
            }
        }
        setUsedPlayers(usedPlayers => [...usedPlayers, item]);
        setInputText(item.full_name);
        setOpenSearchList(false);
    };

    return (
        <div>
            {openSearchList && filteredData.length > 0 ?
                <div className="search-list" ref={refOne}>
                    <ul className='search-list-ul'>
                        {filteredData.map((item: Player, index: number) => (
                            <li key={index} onClick={() => handleList(item)} className="search-list-option">
                                {item.full_name}
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

export default SearchList;
