import React, { SetStateAction, useEffect } from 'react';
import { NBATeam } from '../interfaces/Teams';
import { Player } from '../interfaces/Player';
import { PropBetStats } from '../interfaces/PropBetStats';
import axios from 'axios';
import { BoxScoreTraditional } from '../interfaces/BoxScoreTraditional';
import { Stats } from '../interfaces/StatsTable';


interface PropBetResultsProps {
    selectedSeason: string;
    roster: Player[];
    propBetStats: PropBetStats[];
    overUnderLine: number | null;
    selectedOpponent: NBATeam;
    setPlayerBoxScores: React.Dispatch<SetStateAction<Stats[]>>
}

const PropBetResults: React.FC<PropBetResultsProps> = ({ selectedSeason, overUnderLine, selectedOpponent, roster, propBetStats, setPlayerBoxScores }) => {


    useEffect(() => {
        const getPropBetResults = async () => {

            const jsonPropBetStats = JSON.stringify(propBetStats);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonPropBetStats = encodeURIComponent(jsonPropBetStats);

            const jsonRoster = JSON.stringify(roster);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonRoster = encodeURIComponent(jsonRoster);

            // Construct the URL with the encoded JSON as a query parameter
            for (const player of roster) {

                try {
                    const results = await axios.get(`/api/PlayerResults?selectedSeason=${selectedSeason}&overUnderLine=${overUnderLine}&selectedOpponent=${selectedOpponent.team_id}&player_id=${player.player_id}&propBetStats=${encodedJsonPropBetStats}`);
                    console.log(results.data);
                    setPlayerBoxScores(results.data);
                    console.log('DDDDAAAAAAAAAAAAAAATTTTTTTTTTTTTTTAAAAAAAAAAAAAAAAAAAA')
                } catch (error) {
                    console.log(error);
                }
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