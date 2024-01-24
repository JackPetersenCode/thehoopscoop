import { useEffect, useState } from 'react';
import { Player } from '../interfaces/Player';
import styled from "styled-components";
import SeasonsDropDown from '../components/SeasonsDropDown';
import SearchBar from '../components/SearchBar';
import LineupPlayerSelect from '../components/LineupPlayerSelect';
import StatsTable from '../components/StatsTable';

const DataFlex = styled.div`
    display: flex;
`

function Home() {

    const [selectedSeason, setSelectedSeason] = useState('2023_24');
    const [activePlayers, setActivePlayers] = useState<Player[]>([]);
    const [inputText, setInputText] = useState('');
    const [selectedPlayer, setSelectedPlayer] = useState('');
    const [selectedLineupPlayer, setSelectedLineupPlayer] = useState('Lineups');
    const [selectedBoxType, setSelectedBoxType] = useState('Advanced');
    const [numPlayers, setNumPlayers] = useState('5');
    const [perMode, setPerMode] = useState('Totals');

    const lineupPlayerOptions = [
        'Lineups',
        'Players'
    ];

    const statOptions = [
        'Traditional',
        'Advanced',
        'FourFactors',
        'Misc',
        'Scoring',
        'Opponent'
    ];


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
                    <LineupPlayerSelect options={lineupPlayerOptions} selectedOption={selectedLineupPlayer} setSelectedOption={setSelectedLineupPlayer} />
                </div>
                <div>
                    <LineupPlayerSelect options={statOptions} selectedOption={selectedBoxType} setSelectedOption={setSelectedBoxType} />
                </div>
                <div className="search-container">
                    <SearchBar activePlayers={activePlayers} inputText={inputText} setInputText={setInputText} selectedPlayer={selectedPlayer} setSelectedPlayer={setSelectedPlayer} />
                </div>

            </DataFlex>
            <SeasonsDropDown
                selectedSeason={selectedSeason}
                setSelectedSeason={setSelectedSeason}
                predictions={false}
            />
            <PerModeDropDown perMode={perMode} setPerMode={setPerMode} />
            <StatsTable selectedSeason={selectedSeason} selectedLineupPlayer={selectedLineupPlayer} selectedBoxType={selectedBoxType} numPlayers={numPlayers} perMode={perMode} />

        </div>
    );
}

export default Home;