import React from 'react';
import { loadBoxScoresTraditional, loadLeagueGamesBySeason, loadPlayers, loadLeagueDashLineupsFunction } from '../helpers/Loaders';

function Admin() {

    const loadLineupsLoop = () => {
        const seasons = ['2015_16', '2016_17', '2017_18', '2018_19', '2019_20', '2020_21', '2021_22', '2022_23', '2023_24'];
        //const seasons = ['2015_16'];
        const boxTypes = ['Opponent'];
        const numPlayers = ['2', '3', '4', '5'];

        for (let i = 0; i < boxTypes.length; i++) {
            for (let j = 0; j < seasons.length; j++) {
                for (let k = 0; k < numPlayers.length; k++) {
                    loadLeagueDashLineupsFunction(seasons[j], boxTypes[i], numPlayers[k]);
                }
            }
        }
    }
    
    return (
        <div>
            <button onClick={loadLeagueGamesBySeason}>League Games</button>
            <button onClick={loadPlayers}>Players</button>
            <button onClick={loadBoxScoresTraditional}>Box Score Traditional</button>
            <button onClick={loadLineupsLoop}>League Dash Lineups</button>
        </div>
  );
}

export default Admin;