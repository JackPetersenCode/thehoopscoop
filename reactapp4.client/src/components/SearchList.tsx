import React, { useRef, useEffect } from 'react';
import styled from 'styled-components';
import { Player } from '../interfaces/Player';

const StyledDiv = styled.div`
    /* Styles for the dropdown */
`;

const StyledOption = styled.div`
    /* Styles for the dropdown option */
`;

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
    roster,
    setRoster,
    setUsedPlayers,
    gameOption
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
        if (gameOption == "Head 2 Head") {
            setRoster(roster => [...roster, item]);
        } else if (gameOption == "Prop Bet") {
            setRoster([item]);
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
                        <option key={index} onClick={() => handleList(item)}>
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
