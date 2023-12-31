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
    selectedPlayer: string;
    setSelectedPlayer: React.Dispatch<React.SetStateAction<string>>;
}

const SearchList: React.FC<SearchListProps> = ({
    refTwo,
    inputText,
    setInputText,
    data,
    selectedPlayer,
    setSelectedPlayer,
}) => {
    const refOne = useRef<HTMLDivElement>(null);

    useEffect(() => {
        function handleClickOutside(event: MouseEvent) {
            if (
                refOne.current &&
                !refOne.current.contains(event.target as Node) &&
                !refTwo.current?.contains(event.target as Node)
            ) {
                setInputText('');
            }
        }

        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, [refTwo, setInputText]);

    const filteredData = data.filter((element: Player) => {
        return element.full_name.toLowerCase().includes(inputText);
    });

    const handleList = (item: Player) => {
        setSelectedPlayer(item.full_name);
        console.log(selectedPlayer);
        setInputText('');
    };

    return (
        <StyledDiv ref={refOne}>
            {filteredData.map((item: Player, index: number) => (
                <StyledOption key={index} onClick={() => handleList(item)}>
                    {item.full_name}
                </StyledOption>
            ))}
        </StyledDiv>
    );
};

export default SearchList;
