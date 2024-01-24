import axios from "axios";
import '../App.css';
import React, { useEffect, useState } from "react";
import styled from 'styled-components';
import '../style.css';
interface SeasonsDropDownProps {
    selectedSeason: string;
    setSelectedSeason: React.Dispatch<React.SetStateAction<string>>;
    predictions: boolean;
}

const SeasonFlex = styled.div`
    display: flex;
    flex-direction: column;
`

const DropTitle = styled.div`
    color: rgb(153, 153, 153);
    font-size: x-small;
    font-weight: 600;
    text-align: left;
`

const SeasonSelect = styled.select`
    padding: 5px;
    border: solid transparent;
    border-right: 12px solid transparent;
    background-color: rgb(238, 238, 238);
    border-radius: 5px;
    width: 100%;
`

const SeasonOption = styled.option`
    padding: 10px;
`

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
        <div className="drop-flex">
            <div className="drop-title">
                SEASON
            </div>
            <div>
                <select className="drop-flex-select" value={selectedSeason} onChange={handleSeasonChange}>
                    <option className="drop-flex-option" value="0">Select Season</option>

                    {seasonsData.map((option, index) => (
                        <option key={index} value={Object.values(option)}>{Object.values(option)}</option>
                    ))}

                </select>
            </div>
        </div>
    );

};

export default SeasonsDropDown;