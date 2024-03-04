import React, { useEffect } from 'react';
import { NBATeam } from '../interfaces/Teams';
import { Player } from '../interfaces/Player';
import { PropBetStats } from '../interfaces/PropBetStats';
import axios from 'axios';


interface PropBetResultsProps {
    selectedSeason: string;
    roster: Player[];
    propBetStats: PropBetStats[];
    overUnderLine: number | null;
    selectedOpponent: NBATeam;
}

const PropBetResults: React.FC<PropBetResultsProps> = ({ selectedSeason, overUnderLine, selectedOpponent, roster, propBetStats }) => {


    useEffect(() => {
        const getPropBetResults = async () => {

            const jsonPropBetStats = JSON.stringify(propBetStats);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonPropBetStats = encodeURIComponent(jsonPropBetStats);

            const jsonRoster = JSON.stringify(roster);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonRoster = encodeURIComponent(jsonRoster);

            // Construct the URL with the encoded JSON as a query parameter
            try {
                const results = await axios.get(`/api/PropBetResults?selectedSeason=${selectedSeason}&overUnderLine=${overUnderLine}&selectedOpponent=${selectedOpponent.team_id}&roster=${encodedJsonRoster}&propBetStats=${encodedJsonPropBetStats}`)
                console.log(results.data);
                console.log('DDDDAAAAAAAAAAAAAAATTTTTTTTTTTTTTTAAAAAAAAAAAAAAAAAAAA')
            } catch (error) {
                console.log(error);
            }
        }
        if (roster.length > 0 && propBetStats.length > 0 && overUnderLine && selectedOpponent) {
            getPropBetResults();
        }
    }, [selectedSeason, roster, propBetStats, overUnderLine, selectedOpponent])

    return (
        <>
        {
            roster.length > 0 && propBetStats.length > 0 && overUnderLine && selectedOpponent ?
            <div>
                WHISTLES
            </div>
            :
            ""
        }
        </>
  );
}

export default PropBetResults;