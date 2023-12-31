import axios from "axios";
import '../App.css';
import React, { useEffect, useState } from "react";

const SeasonsDropDown = ({ selectedSeason, setSelectedSeason, predictions }) => {

    let seasonsData;
    if (predictions) {
        seasonsData = [
            { season: '2016-2017' },
            { season: '2017-2018' },
            { season: '2018-2019' },
            { season: '2019-2020' },
            { season: '2020-2021' },
            { season: '2021-2022' },
            { season: '2022-2023' },
            { season: '2023-2024' }
        ]
    } else {
        seasonsData = [
            { season: '2015-2016' },
            { season: '2016-2017' },
            { season: '2017-2018' },
            { season: '2018-2019' },
            { season: '2019-2020' },
            { season: '2020-2021' },
            { season: '2021-2022' },
            { season: '2022-2023' },
            { season: '2023-2024' }
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