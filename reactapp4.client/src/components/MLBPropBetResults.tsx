import React, { SetStateAction, useEffect } from 'react';
import axios from 'axios';
import { PropBetStats } from '../interfaces/PropBetStats';
import { NBATeam } from '../interfaces/Teams';
import { Stats } from '../interfaces/StatsTable';
import { homeAwayFilteredBoxScores, overUnderFilteredBoxScores } from '../helpers/BoxScoreFilterFunctions';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';

interface MLBPropBetResultsProps {
    careerPlayerBoxScores: Stats[];
    setCareerPlayerBoxScores: React.Dispatch<SetStateAction<Stats[]>>;
    gamesPlayed: Stats[];
    careerGamesPlayed: Stats[];
    setCareerGamesPlayed: React.Dispatch<SetStateAction<Stats[]>>;
    overUnderLine: number | string;
    propBetStats: PropBetStats[];
    selectedOpponent: NBATeam;
    roster: MLBActivePlayer[];
    playerBoxScores: Stats[];
    homeOrVisitor: string;
    selectedSeason: string;
}

const MLBPropBetResults: React.FC<MLBPropBetResultsProps> = ({ careerPlayerBoxScores, setCareerPlayerBoxScores, gamesPlayed, careerGamesPlayed, setCareerGamesPlayed, overUnderLine, propBetStats, selectedOpponent, roster, playerBoxScores, homeOrVisitor, selectedSeason }) => {


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
                        const results = await axios.get(`/api/PlayerResults?selectedSeason=1&selectedOpponent=${encodedJsonSelectedOpponent}&player_id=${player.playerId}&propBetStats=${encodedJsonPropBetStats}`);
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
                    <div className="prop-results-item">
                        <div className="prop-results bold">
                            Career Games: 
                        </div>
                        <div className="prop-results">
                                <span className="prop-results-fraction">{careerPlayerBoxScores.length} / {careerGamesPlayed.length}</span> <span className="neon-orange">{careerGamesPlayed.length === 0 ? 0 : (100 * careerPlayerBoxScores.length / careerGamesPlayed.length).toFixed(2)}%</span>

                        </div>
                    </div>
                  
                    <div className="prop-results-item">
                        <div className="prop-results bold">
                            {selectedSeason.replace("_", "-")} Games:
                        </div>
                            <div className="prop-results">
                                <span className="prop-results-fraction">{playerBoxScores.length} / {gamesPlayed.length}</span> <span className="neon-orange">{gamesPlayed.length === 0 ? 0 : (100 * playerBoxScores.length / gamesPlayed.length).toFixed(2)}%</span>
                        </div>
                    </div>
                </div>
                :

                ""
            }
        </>
    );
}

export default MLBPropBetResults;