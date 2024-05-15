import React, { useEffect, useState } from 'react';
import { NBATeam } from '../interfaces/Teams';
import { SelectedPlayer } from '../interfaces/Player';
import axios from 'axios';
import { ShotChartsGamesData } from '../interfaces/Shot';




interface ShotChartsPlayerDropDownProps {
    selectedPlayer: SelectedPlayer | string;
    setSelectedPlayer: React.Dispatch<React.SetStateAction<SelectedPlayer | string>>;
    selectedTeam: NBATeam | string;
    selectedSeason: string;
    setSelectedGame: React.Dispatch<React.SetStateAction<ShotChartsGamesData | string>>;
}

const ShotChartsPlayerDropDown: React.FC<ShotChartsPlayerDropDownProps> = ({ selectedPlayer, setSelectedPlayer, selectedTeam, selectedSeason, setSelectedGame }) => {

    const [roster, setRoster] = useState<SelectedPlayer[]>([]);
    

    useEffect(() => {

        const getRoster = async () => {

            if (selectedSeason === '0' || typeof selectedTeam === 'string' || selectedPlayer === '0') {
                return;
            }

            const response = await axios.get(`/api/BoxScoreTraditional/roster/${selectedSeason}/${selectedTeam.team_id}`)
            console.log(response.data);
            setRoster(response.data);
        }
        if (selectedTeam && selectedSeason) {
            getRoster()
        }
    }, [selectedSeason, selectedTeam])
    function handleChange(event: React.ChangeEvent<HTMLSelectElement>) {
        if (event.target.value === "0") {
            return;
        }
        const selectedValue = JSON.parse(event.target.value);
        console.log(event.target.value);
        console.log('set selected team')
        setSelectedPlayer(selectedValue);
        setSelectedGame("");
    }

    return (
        <div className="drop-flex">
            <div className="drop-title">
                Select Player
            </div>
            <select className="drop-flex-select" value={JSON.stringify(selectedPlayer)} onChange={handleChange}>
                <option className="drop-flex-option" value="0">Players</option>

                {roster.map((option, index) => (
                    <option key={index} className="option-select" value={JSON.stringify(option)}>
                        {option.player_name}
                    </option>
                ))}

            </select>
        </div>
    );
}

export default ShotChartsPlayerDropDown;