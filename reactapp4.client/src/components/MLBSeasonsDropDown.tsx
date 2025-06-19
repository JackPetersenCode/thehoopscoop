import '../App.css';
import React from "react";
import '../style.css';

interface SeasonsDropDownProps {
    selectedSeason: string;
    setSelectedSeason: React.Dispatch<React.SetStateAction<string>>;
    isPredictions: boolean;
    disabled: boolean;
}

const MLBSeasonsDropDown: React.FC<SeasonsDropDownProps> = ({ selectedSeason, setSelectedSeason, isPredictions, disabled }) => {

   
    const predictionSeasonsData = [
        { season: '2016', display: '2016' },
        { season: '2017', display: '2017' },
        { season: '2018', display: '2018' },
        { season: '2019', display: '2019' },
        { season: '2020', display: '2020' },
        { season: '2021', display: '2021' },
        { season: '2022', display: '2022' },
        { season: '2023', display: '2023' },
        { season: '2024', display: '2024' },
        { season: '2025', display: '2025' }
    ];
    
    const seasonsData = [
        { season: '2015', display: '2015' },
        { season: '2016', display: '2016' },
        { season: '2017', display: '2017' },
        { season: '2018', display: '2018' },
        { season: '2019', display: '2019' },
        { season: '2020', display: '2020' },
        { season: '2021', display: '2021' },
        { season: '2022', display: '2022' },
        { season: '2023', display: '2023' },
        { season: '2024', display: '2024' },
        { season: '2025', display: '2025' }
    ];
    function handleSeasonChange(event: { preventDefault: () => void; target: { value: string; }; }) {
        event.preventDefault();
        if (event.target.value === "0") {
            return;
        }
        setSelectedSeason(event.target.value);
    }

    return (
        <div className="drop-flex">
            <div className="drop-title">
                Season
            </div>
            <div className="select-wrapper">
                <select className="drop-flex-select" value={selectedSeason} onChange={handleSeasonChange} disabled={disabled}>
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

export default MLBSeasonsDropDown;