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
    selectedSeason: string;
}

const PropBetResults: React.FC<PropBetResultsProps> = ({ careerPlayerBoxScores, setCareerPlayerBoxScores, gamesPlayed, careerGamesPlayed, setCareerGamesPlayed, overUnderLine, propBetStats, selectedOpponent, roster, playerBoxScores, homeOrVisitor, selectedSeason }) => {


    useEffect(() => {

        const getCareerPlayerResults = async() => {

            const jsonPropBetStats = JSON.stringify(propBetStats);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonPropBetStats = encodeURIComponent(jsonPropBetStats);

            const jsonSelectedOpponent = JSON.stringify(selectedOpponent);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonSelectedOpponent = encodeURIComponent(jsonSelectedOpponent);

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

              
                        setCareerGamesPlayed(homeVisitorFilteredBoxScores)
                        setCareerPlayerBoxScores(homeVisitorOverUnderFilteredBoxScores);
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
                <div className="prop-results-container">
                    <div>
                        <div className="prop-results bold">
                            Career Games: 
                        </div>
                        <div className="prop-results">
                                {careerPlayerBoxScores.length} / {careerGamesPlayed.length} {careerGamesPlayed.length === 0 ? 0 : (100 * careerPlayerBoxScores.length / careerGamesPlayed.length).toFixed(2)}%

                        </div>
                    </div>
                  
                    <div>
                        <div className="prop-results bold">
                            {selectedSeason.replace("_", "-")} Games:
                        </div>
                        <div className="prop-results">
                                {playerBoxScores.length} / {gamesPlayed.length} {gamesPlayed.length === 0 ? 0 : (100 * playerBoxScores.length / gamesPlayed.length).toFixed(2)}%
                        </div>
                    </div>
                </div>
                :

                ""
            }
        </>
    );
}

export default PropBetResults;