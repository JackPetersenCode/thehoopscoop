import '../App.css';
import React from "react";
import '../style.css';
import { SelectedPlayer } from "../interfaces/Player";
import { ShotChartsGamesData } from "../interfaces/Shot";
interface SeasonsDropDownProps {
    selectedSeason: string;
    setSelectedSeason: React.Dispatch<React.SetStateAction<string>>;
    setSelectedPlayerShotCharts: React.Dispatch<React.SetStateAction<SelectedPlayer | string>>;
    setSelectedGame: React.Dispatch<React.SetStateAction<ShotChartsGamesData | string>>;
    isShotCharts: boolean;
    isPredictions: boolean;
    disabled: boolean;
}

const SeasonsDropDown: React.FC<SeasonsDropDownProps> = ({ selectedSeason, setSelectedSeason, setSelectedPlayerShotCharts, setSelectedGame, isShotCharts, isPredictions, disabled }) => {

   
    const predictionSeasonsData = [
        { season: '2016_17', display: '2016-17' },
        { season: '2017_18', display: '2017-18' },
        { season: '2018_19', display: '2018-19' },
        { season: '2019_20', display: '2019-20' },
        { season: '2020_21', display: '2020-21' },
        { season: '2021_22', display: '2021-22' },
        { season: '2022_23', display: '2022-23' },
        { season: '2023_24', display: '2023-24' },
        { season: '2024_25', display: '2024-25' }
    ];
    
    const seasonsData = [
        { season: '2015_16', display: '2015-16' },
        { season: '2016_17', display: '2016-17' },
        { season: '2017_18', display: '2017-18' },
        { season: '2018_19', display: '2018-19' },
        { season: '2019_20', display: '2019-20' },
        { season: '2020_21', display: '2020-21' },
        { season: '2021_22', display: '2021-22' },
        { season: '2022_23', display: '2022-23' },
        { season: '2023_24', display: '2023-24' },
        { season: '2024_25', display: '2024-25' }
    ];
    function handleSeasonChange(event: { preventDefault: () => void; target: { value: string; }; }) {
        event.preventDefault();
        if (event.target.value === "0") {
            return;
        }
        setSelectedSeason(event.target.value);
        if (isShotCharts) {

            setSelectedPlayerShotCharts("");
            setSelectedGame("");
        }
    }

    return (
        <div className="drop-flex">
            <div className="drop-title">
                Season
            </div>
            <div className="select-wrapper">
                <select className="drop-flex-select" value={selectedSeason} onChange={handleSeasonChange} disabled={disabled} >
                    <option className="drop-flex-option" value="0">Select Season</option>

                    {!isPredictions ? seasonsData.map((option, index) => (
                        <option key={index} value={option.season}>{option.display}</option>
                    ))
                    :
                    predictionSeasonsData.map((option, index) => (
                        <option key={index} value={option.season}>{option.display}</option>
                    ))}

                </select>
            </div>
        </div>
    );

};

export default SeasonsDropDown;