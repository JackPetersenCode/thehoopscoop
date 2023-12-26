import React from 'react';
import { useEffect, useState } from 'react';
import { Game } from '../interfaces/Game';
import { Player } from '../interfaces/Player';
import { BoxScoreTraditional } from '../interfaces/BoxScoreTraditional';
import styled from "styled-components";




const DataFlex = styled.div`
    display: flex;
`

function Home() {

    const [games, setGames] = useState<Game[]>([]);
    const [players, setPlayers] = useState<Player[]>([]);
    const [boxScoreTraditional, setBoxScoreTraditional] = useState<BoxScoreTraditional[]>([]);

    useEffect(() => {
        async function getData() {
            const gamesResponse = await fetch('api/LeagueGames/2015_2016');
            const gamesData = await gamesResponse.json();
            console.log(gamesData)
            setGames(gamesData);

            const playersResponse = await fetch('api/players');
            const playersData = await playersResponse.json();
            console.log(playersData)
            setPlayers(playersData);

            const boxScoreTraditionalResponse = await fetch('api/BoxScoreTraditional/2015_2016');
            const boxScoreTraditionalData = await boxScoreTraditionalResponse.json();
            console.log(boxScoreTraditionalData)
            setBoxScoreTraditional(boxScoreTraditionalData);
        }

        //populateWeatherData();
        //getEmployeesData();
        getData();
    }, []);


    return (
        <div>
            <h1>League Games</h1>

            <DataFlex>
                <div>
                    {players.map((player, index) => (
                        <div key={index}>
                            {player.full_name}
                        </div>
                    ))}
                </div>
                <div>
                    
                    {games.length > 0 ? (
                        games.map((game, index) => (
                            <div key={index}>
                                <p>{game.matchup}</p>
                                <p>{game.wl}</p>
                                
                            </div>
                        ))
                    ) : (
                        <p>No games available</p>
                    )}
                </div>
                <div>
                    {boxScoreTraditional.map((boxScore, index) => (
                        <div key={index}>
                            {boxScore.player_name}
                            <br></br>
                            {boxScore.team_abbreviation}
                        </div>
                    ))}
                </div>
            </DataFlex>

        </div>
    );

    async function populateWeatherData() {
        const response = await fetch('api/weatherforecast');
        const data = await response.json();
        console.log(data)
        setForecasts(data);
    }

    async function getEmployeesData() {
        try {
            const response = await fetch('api/ballers');
            const data = await response.json();
            console.log(data)
        } catch (error) {
            console.log(error)
        }
    }



}

export default Home;