import React from 'react';
import { useEffect, useState } from 'react';
import { Game } from '../interfaces/Game';
import { Player } from '../interfaces/Player';







function Home() {

    const [games, setGames] = useState<Game[]>([]);
    const [players, setPlayers] = useState<Player[]>([]);

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
        }

        //populateWeatherData();
        //getEmployeesData();
        getData();
    }, []);


    return (
        <div>
            <h1>League Games</h1>

            <div>
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
            </div>

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