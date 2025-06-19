import React, { useEffect } from 'react';
import { SelectedPlayer } from '../interfaces/Player';
import { ShotChartsGamesData } from '../interfaces/Shot';
import axios from 'axios';
import { NBATeam } from '../interfaces/Teams';


interface ShotChartsGamesDropDownProps {
    selectedPlayer: SelectedPlayer | string;
    selectedSeason: string;
    selectedTeam: NBATeam | string;
    selectedGame: ShotChartsGamesData | string;
    setSelectedGame: React.Dispatch<React.SetStateAction<ShotChartsGamesData | string>>;
    gameData: ShotChartsGamesData[];
    setGameData: React.Dispatch<React.SetStateAction<ShotChartsGamesData[]>>;
}

const ShotChartsGamesDropDown: React.FC<ShotChartsGamesDropDownProps> = ({ selectedPlayer, selectedSeason, selectedTeam, selectedGame, setSelectedGame, gameData, setGameData }) => {

    useEffect(() => {

        const getGames = async () => {
            if (selectedSeason === '0' || typeof selectedTeam === 'string' || typeof selectedPlayer === 'string') {
                return;
            }
            const games = await axios.get(`/api/LeagueGames/shotChartsGames/${selectedPlayer.player_id}/${selectedSeason}`)
            setGameData(games.data);
        }
        if (selectedPlayer) {
            getGames();
        } else {
            setGameData([])
        }
    }, [selectedPlayer, selectedSeason])

    function handleChange(event: React.ChangeEvent<HTMLSelectElement>) {
        if (event.target.value === "0") {
            return;
        }
        const selectedValue = JSON.parse(event.target.value);
        setSelectedGame(selectedValue);
    }

    return (
        <div className="drop-flex">
            <div className="drop-title">
                Select Game
            </div>
            <div className="select-wrapper">
                <select className="drop-flex-select" value={JSON.stringify(selectedGame)} onChange={handleChange}>
                    <option className="drop-flex-option" value="0">Select Game</option>

                    {gameData.map((option, index) => (
                        <option key={index} className="option-select" value={JSON.stringify(option)}>
                            {option.game_date + ' ' + option.matchup}
                        </option>
                    ))}

                </select>
            </div>
        </div>
    );
}

export default ShotChartsGamesDropDown;