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
        function handleClickOutside(event: MouseEvent) {
            if (
                refOne.current &&
                !refOne.current.contains(event.target as Node) &&
                !refTwo.current?.contains(event.target as Node)
            ) {
                setOpenSearchList(false);
            }
        }

        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
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
        console.log(selectedPlayer);
        setInputText(item.full_name);
        setOpenSearchList(false);
    };

    return (
        <>
            {openSearchList ?
                <div className="search-list" ref={refOne}>
                    {filteredData.map((item: Player, index: number) => (
                        <option key={index} onClick={() => handleList(item)} className="search-list-option">
                            {item.full_name}
                        </option>
                    ))}
                </div >
            : 
            ""}
        </>
    );
};

export default SearchList;
