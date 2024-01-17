import axios from "axios";
import '../App.css';
import React, { useEffect, useState } from "react";


interface SeasonsDropDownProps {
    selectedSeason: string;
    setSelectedSeason: React.Dispatch<React.SetStateAction<string>>;
    predictions: boolean;
}

const SeasonsDropDown: React.FC<SeasonsDropDownProps> = ({ selectedSeason, setSelectedSeason, predictions }) => {

    let seasonsData;
    if (predictions) {
        seasonsData = [
            { season: '2016_17' },
            { season: '2017_18' },
            { season: '2018_19' },
            { season: '2019_20' },
            { season: '2020_21' },
            { season: '2021_22' },
            { season: '2022_23' },
            { season: '2023_24' }
        ]
    } else {
        seasonsData = [
            { season: '2015_16' },
            { season: '2016_17' },
            { season: '2017_18' },
            { season: '2018_19' },
            { season: '2019_20' },
            { season: '2020_21' },
            { season: '2021_22' },
            { season: '2022_23' },
            { season: '2023_24' }
        ]
    }
    function handleSeasonChange(event: { preventDefault: () => void; target: { value: string; }; }) {
        event.preventDefault();
        if (event.target.value === "0") {
            return;
        }
        setSelectedSeason(event.target.value);
        console.log(selectedSeason)
    }

    return (
        <div>
            <div>
                SEASON
            </div>
            <div>
                <select value={selectedSeason} onChange={handleSeasonChange}>
                    <option value="0">Select Season</option>

                    {seasonsData.map((option, index) => (
                        <option key={index} value={Object.values(option)}>{Object.values(option)}</option>
                    ))}

                </select>
            </div>
        </div>
    );

};

export default SeasonsDropDown;