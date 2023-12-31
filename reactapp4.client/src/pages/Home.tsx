import React from 'react';
import { useEffect, useState } from 'react';
import { Game } from '../interfaces/Game';
import { Player } from '../interfaces/Player';
import { BoxScoreTraditional } from '../interfaces/BoxScoreTraditional';
import styled from "styled-components";
import SeasonsDropDown from '../components/SeasonsDropDown';
import SearchBar from '../components/SearchBar';




const DataFlex = styled.div`
    display: flex;
`

function Home() {

    const [selectedSeason, setSelectedSeason] = useState('2023_2024');
    const [activePlayers, setActivePlayers] = useState<Player[]>([]);
    const [inputText, setInputText] = useState('');
    const [selectedPlayer, setSelectedPlayer] = useState('');

    useEffect(() => {
        async function getData() {

            const activePlayersResponse = await fetch('api/players/active');
            const activePlayersData = await activePlayersResponse.json();
            console.log(activePlayersData)
            setActivePlayers(activePlayersData);


        }

        //populateWeatherData();
        //getEmployeesData();
        getData();
    }, []);


    return (
        <div>
            <h1>NBA</h1>

            <DataFlex>

                <div>
                    <SeasonsDropDown
                        selectedSeason={selectedSeason}
                        setSelectedSeason={setSelectedSeason}
                        predictions={false}
                    />
                </div>
                
                <div>
                    <select>
                        {activePlayers.map((player, index) => (
                            <option key={index}>
                                {player.full_name}
                            </option>
                        ))}
                    </select>
                </div>
                <div className="search-container">
                    <SearchBar activePlayers={activePlayers} inputText={inputText} setInputText={setInputText} selectedPlayer={selectedPlayer} setSelectedPlayer={setSelectedPlayer} />
                </div>

            </DataFlex>

        </div>
    );
}

export default Home;