import { useEffect, useState } from 'react';
import { Player } from '../interfaces/Player';
import styled from "styled-components";
import SeasonsDropDown from '../components/SeasonsDropDown';
import SearchBar from '../components/SearchBar';
import LineupPlayerSelect from '../components/LineupPlayerSelect';
import StatsTable from '../components/StatsTable';
import DropDown from '../components/DropDown';
import { NBATeam } from '../interfaces/Teams';

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
    const [selectedTeam, setSelectedTeam] = useState({team_id: '1', team_name: 'All Teams'});
    const [sortField, setSortField] = useState("min");

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

    const perModeOptions = [
        'Per Game',
        'Per 100 Poss',
        'Per Minute',
        'Totals'
    ];

    const numPlayersOptions = [
        '2',
        '3',
        '4',
        '5'
    ]

    const nbaTeams: NBATeam[] = [
        { team_id: "1", team_name: "All Teams" },
        { team_id: "1610612744", team_name: "Golden State Warriors" },
        { team_id: "1610612757", team_name: "Portland Trail Blazers" },
        { team_id: "1610612751", team_name: "Brooklyn Nets" },
        { team_id: "1610612748", team_name: "Miami Heat" },
        { team_id: "1610612747", team_name: "Los Angeles Lakers" },
        { team_id: "1610612761", team_name: "Toronto Raptors" },
        { team_id: "1610612738", team_name: "Boston Celtics" },
        { team_id: "1610612752", team_name: "New York Knicks" },
        { team_id: "1610612762", team_name: "Utah Jazz" },
        { team_id: "1610612756", team_name: "Phoenix Suns" },
        { team_id: "1610612753", team_name: "Orlando Magic" },
        { team_id: "1610612749", team_name: "Milwaukee Bucks" },
        { team_id: "1610612741", team_name: "Chicago Bulls" },
        { team_id: "1610612745", team_name: "Houston Rockets" },
        { team_id: "1610612737", team_name: "Atlanta Hawks" },
        { team_id: "1610612754", team_name: "Indiana Pacers" },
        { team_id: "1610612759", team_name: "San Antonio Spurs" },
        { team_id: "1610612765", team_name: "Detroit Pistons" },
        { team_id: "1610612763", team_name: "Memphis Grizzlies" },
        { team_id: "1610612739", team_name: "Cleveland Cavaliers" },
        { team_id: "1610612758", team_name: "Sacramento Kings" },
        { team_id: "1610612743", team_name: "Denver Nuggets" },
        { team_id: "1610612742", team_name: "Dallas Mavericks" },
        { team_id: "1610612766", team_name: "Charlotte Hornets" },
        { team_id: "1610612750", team_name: "Minnesota Timberwolves" },
        { team_id: "1610612740", team_name: "New Orleans Pelicans" },
        { team_id: "1610612764", team_name: "Washington Wizards" },
        { team_id: "1610612755", team_name: "Philadelphia 76ers" },
        { team_id: "1610612746", team_name: "Los Angeles Clippers" },
        { team_id: "1610612760", team_name: "Oklahoma City Thunder" },
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
                    <LineupPlayerSelect options={lineupPlayerOptions} selectedOption={selectedLineupPlayer} setSelectedOption={setSelectedLineupPlayer} setSortField={setSortField} />
                </div>
                <div>
                    <LineupPlayerSelect options={statOptions} selectedOption={selectedBoxType} setSelectedOption={setSelectedBoxType} setSortField={setSortField} />
                </div>
                <div className="search-container">
                    <SearchBar activePlayers={activePlayers} inputText={inputText} setInputText={setInputText} selectedPlayer={selectedPlayer} setSelectedPlayer={setSelectedPlayer} />
                </div>

            </DataFlex>
            <DataFlex>
                <SeasonsDropDown
                    selectedSeason={selectedSeason}
                    setSelectedSeason={setSelectedSeason}
                    predictions={false}
                />
                {selectedBoxType === "Base" || selectedBoxType === "Misc" || selectedBoxType === "Opponent" ?
                    <DropDown
                        options={perModeOptions}
                        perMode={perMode}
                        setPerMode={setPerMode}
                        numPlayers={numPlayers}
                        setNumPlayers={setNumPlayers}
                        selectedTeam={selectedTeam}
                        setSelectedTeam={setSelectedTeam}
                        dropDownType="Per Mode"
                    />
                    :
                ""}
                {selectedLineupPlayer === "Lineups" ?
                    <DropDown
                        options={numPlayersOptions}
                        perMode={perMode}
                        setPerMode={setPerMode}
                        numPlayers={numPlayers}
                        setNumPlayers={setNumPlayers}
                        selectedTeam={selectedTeam}
                        setSelectedTeam={setSelectedTeam}
                        dropDownType="# of Players"
                    />
                    :
                ""}
                <DropDown
                    options={nbaTeams}
                    perMode={perMode}
                    setPerMode={setPerMode}
                    numPlayers={numPlayers}
                    setNumPlayers={setNumPlayers}
                    selectedTeam={selectedTeam}
                    setSelectedTeam={setSelectedTeam}
                    dropDownType="Team"
                />
            </DataFlex>
            <StatsTable selectedSeason={selectedSeason} selectedLineupPlayer={selectedLineupPlayer} selectedBoxType={selectedBoxType} numPlayers={numPlayers} perMode={perMode} selectedTeam={selectedTeam} sortField={sortField} setSortField={setSortField} inputText={inputText} setInputText={setInputText} />

        </div>
    );
}

export default Home;