import '../App.css';
import React, { useEffect, useState } from "react";
import HistoricalGrid from './HistoricalGrid';
import { getJsonResponseStartup } from '../helpers/GetJsonResponse';
import { NBATeam } from '../interfaces/Teams';


interface HistoricalResultsProps {
    selectedSeason: string;
    selectedTeam: NBATeam | string; 
}

const HistoricalResults: React.FC<HistoricalResultsProps> = ({selectedSeason, selectedTeam}) => {

    const [historicalResults, setHistoricalResults] = useState([]);
 
    useEffect(() => {

        const getHistoricalResults = async() => {
            let results;
            if (selectedTeam === '0' || selectedTeam === '') {
                results = await getJsonResponseStartup(`/api/Gambling/historicalResults/${selectedSeason}`);
                setHistoricalResults(results);
            } else {
                results = await getJsonResponseStartup(`/api/Gambling/historicalResults/${selectedSeason}/${typeof selectedTeam === 'string' ? selectedTeam : selectedTeam.team_name}`);
                setHistoricalResults(results);
            }
        }
        if (selectedSeason) {
            getHistoricalResults();
        }
    }, [selectedSeason, selectedTeam])
/*
    useEffect(() => {

        const getHistoricalResultsByTeam = async() => {

            let results = await hoop.get(`/api/gambling/historicalResults/ByTeam/${selectedTeam}/${selectedSeason}`);
            console.log(results.data);
            setHistoricalResultsByTeam(results.data);
        }
        if (selectedSeason && selectedTeam) {
            getHistoricalResultsByTeam();
        }
    }, [selectedSeason, selectedTeam])
*/

    return (
        <div className="historical-container">
            {historicalResults.map((game, index) => (
                <div key={index}>
                    <HistoricalGrid game={game} />
                </div>
            ))}
        </div>
    )
}

export default HistoricalResults;

