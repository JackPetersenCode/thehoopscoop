import { React, useEffect, useState, useRef } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import CareerStats from './CareerStats';

const DropdownStyle = styled.div`
    background-color: white;
    display: flex;
    flex-direction: column;
    padding: 10px;
    text-align: left;
    min-width: 230px;
    border-radius: 5px;
    position: absolute;
    z-index: 20;
`

const DropdownRowStyle = styled.div`
    cursor: pointer;
    margin: 2px 0;
    padding-left: 3px;
    padding-right: 3px;
`
const StyledDiv = styled.div`
    background-color: white;
    padding: 10px;
    text-align: left;
    min-width: 230px;
    z-index: 20;
    box-shadow: 0 0 2px rgb(0, 0, 0), 0 0 2px rgb(0, 0, 0),
    0 0 2px rgb(0, 0, 0);
    max-height: 500px;
    overflow-y: auto;

    
`
const StyledOption = styled.div`
    cursor: pointer;
    &:hover {
      background-color: lightblue;
    }
`

function SearchList({ refTwo, inputText, setInputText, data, selectedPlayer, setSelectedPlayer }) {
    //create a new array by filtering the original array
    function useOutsideAlerter(ref, ref2) {
        useEffect(() => {
            /**
             * Alert if clicked on outside of element
             */
            function handleClickOutside(event) {
                if (ref.current && !ref.current.contains(event.target) && !refTwo.current.contains(event.target)) {
                    console.log(refTwo.current)
                    console.log(event.target.getAttribute('data-testid'))
                    console.log(event.target)
                    console.log(event)

                    setInputText('');
                }
            }
            // Bind the event listener
            document.addEventListener("mousedown", handleClickOutside);
            return () => {
                // Unbind the event listener on clean up
                document.removeEventListener("mousedown", handleClickOutside);
            };
        }, [ref]);
    }

    const refOne = useRef(null);
    useOutsideAlerter(refOne, refTwo)

    const filteredData = data.filter((element: { full_name: string; }) => {
        //if no input the return the original
        //return the item which contains the user input
        return element.full_name.toLowerCase().includes(inputText);

    })

    function handleList(item) {
        console.log(item)
        setSelectedPlayer(() => item);
        setInputText('');
    }


    return (
        <div ref={refOne} >
            {
                <StyledDiv>
                    {filteredData.map((item, index) => (
                        <StyledOption key={index} onClick={() => handleList(item)} >{item.full_name}</StyledOption>
                    ))}
                </StyledDiv>
            }
        </div>
    )
}

export default SearchList;