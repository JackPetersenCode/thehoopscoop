import React, { SetStateAction, useEffect, useState } from 'react';
import axios from 'axios';
import { PropBetStats } from '../interfaces/PropBetStats';
import { NBATeam } from '../interfaces/Teams';
import { Player } from '../interfaces/Player';
import { Stats } from '../interfaces/StatsTable';
import { homeAwayFilteredBoxScores, overUnderFilteredBoxScores } from '../helpers/BoxScoreFilterFunctions';

interface PropBetResultsProps {
    careerPlayerBoxScores: Stats[];
    setCareerPlayerBoxScores: React.Dispatch<SetStateAction<Stats[]>>;
    gamesPlayed: Stats[];
    careerGamesPlayed: Stats[];
    setCareerGamesPlayed: React.Dispatch<SetStateAction<Stats[]>>;
    overUnderLine: number | string;
    propBetStats: PropBetStats[];
    selectedOpponent: NBATeam;
    roster: Player[];
    playerBoxScores: Stats[];
    homeOrVisitor: string;
}

const PropBetResults: React.FC<PropBetResultsProps> = ({ careerPlayerBoxScores, setCareerPlayerBoxScores, gamesPlayed, careerGamesPlayed, setCareerGamesPlayed, overUnderLine, propBetStats, selectedOpponent, roster, playerBoxScores, homeOrVisitor }) => {


    //useEffect(() => {
    //
    //    const getPropBetResults = async () => {
    //        const results = await axios.get('/api/PlayerResults')
    //    }
    //}, []);

    //display avg stat

    useEffect(() => {

        const getCareerPlayerResults = async() => {

            const jsonPropBetStats = JSON.stringify(propBetStats);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonPropBetStats = encodeURIComponent(jsonPropBetStats);

            const jsonSelectedOpponent = JSON.stringify(selectedOpponent);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonSelectedOpponent = encodeURIComponent(jsonSelectedOpponent);
            console.log(roster);

            if (roster.length == 0) {
                setCareerPlayerBoxScores([]);
            } else {
                for (const player of roster) {

                    try {
                        const results = await axios.get(`/api/PlayerResults?selectedSeason=1&selectedOpponent=${encodedJsonSelectedOpponent}&player_id=${player.player_id}&propBetStats=${encodedJsonPropBetStats}`);
                        console.log(results.data);

                        const OUFilteredBoxScores = await overUnderFilteredBoxScores(results.data, propBetStats, overUnderLine);
                        const homeVisitorOverUnderFilteredBoxScores = await homeAwayFilteredBoxScores(OUFilteredBoxScores, homeOrVisitor);
                        const homeVisitorFilteredBoxScores = await homeAwayFilteredBoxScores(results.data, homeOrVisitor);

                        console.log(homeVisitorOverUnderFilteredBoxScores);
                        console.log(homeVisitorFilteredBoxScores);
                        setCareerGamesPlayed(homeVisitorFilteredBoxScores)
                        setCareerPlayerBoxScores(homeVisitorOverUnderFilteredBoxScores);
                        console.log('DDDDAAAAAAAAAAAAAAATTTTTTTTTTTTTTTAAAAAAAAAAAAAAAAAAAA')
                    } catch (error) {
                        console.log(error);
                    }
                }
            }
        }
        getCareerPlayerResults();

    }, [roster, propBetStats, overUnderLine, selectedOpponent, homeOrVisitor])

    return (
        <>
        {
                careerPlayerBoxScores.length > 0 ?
                <div>
                    <div className="player-box-container">
                        {careerPlayerBoxScores.length} / {careerGamesPlayed.length}
                    </div>
                    <div>
                        {playerBoxScores.length} / {gamesPlayed.length}
                    </div>
                </div>
                :

                ""
            }
        </>
    );
}

export default PropBetResults;