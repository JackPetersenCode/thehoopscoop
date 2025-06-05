import React, { SetStateAction, useEffect, useState } from 'react';
import axios from 'axios';
import { PropBetStats } from '../interfaces/PropBetStats';
import { MLBTeam, NBATeam } from '../interfaces/Teams';
import { Stats } from '../interfaces/StatsTable';
import { homeAwayFilteredBoxScores, MLBhomeAwayFilteredBoxScores, MLBLastTenFilteredBoxScores, overUnderFilteredBoxScores } from '../helpers/BoxScoreFilterFunctions';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';

interface MLBPropBetResultsProps {
    careerPlayerBoxScores: Stats[];
    setCareerPlayerBoxScores: React.Dispatch<SetStateAction<Stats[]>>;
    gamesPlayed: Stats[];
    careerGamesPlayed: Stats[];
    setCareerGamesPlayed: React.Dispatch<SetStateAction<Stats[]>>;
    overUnderLine: number | string;
    propBetStats: PropBetStats[];
    selectedOpponent: MLBTeam;
    roster: MLBActivePlayer[];
    playerBoxScores: Stats[];
    homeOrVisitor: string;
    selectedSeason: string;
    hittingPitching: string;
    lastTenFilteredBoxScores: Stats[];
    setIsFetching: React.Dispatch<SetStateAction<boolean>>;
}

const MLBPropBetResults: React.FC<MLBPropBetResultsProps> = ({ careerPlayerBoxScores, setCareerPlayerBoxScores, gamesPlayed, careerGamesPlayed, setCareerGamesPlayed, overUnderLine, propBetStats, selectedOpponent, 
    roster, playerBoxScores, homeOrVisitor, selectedSeason, hittingPitching, lastTenFilteredBoxScores, setIsFetching }) => {

    
    useEffect(() => {

        const getCareerPlayerResults = async() => {
            setIsFetching(true);
            const jsonPropBetStats = JSON.stringify(propBetStats);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonPropBetStats = encodeURIComponent(jsonPropBetStats);

            const jsonSelectedOpponent = JSON.stringify(selectedOpponent);

            // Encode the JSON string for inclusion in the URL
            const encodedJsonSelectedOpponent = encodeURIComponent(jsonSelectedOpponent);

            if (roster.length == 0) {
                setCareerPlayerBoxScores([]);
                setIsFetching(false);
            } else {
                for (const player of roster) {

                    try {
                        const results = await axios.get(`/api/MLBPlayerResults?hittingPitching=${hittingPitching}&selectedSeason=1&selectedOpponent=${encodedJsonSelectedOpponent}&player_id=${player.playerId}&propBetStats=${encodedJsonPropBetStats}`);
                        const OUFilteredBoxScores = await overUnderFilteredBoxScores(results.data, propBetStats, overUnderLine);
                        const homeVisitorOverUnderFilteredBoxScores = await MLBhomeAwayFilteredBoxScores(OUFilteredBoxScores, homeOrVisitor);
                        const homeVisitorFilteredBoxScores = await MLBhomeAwayFilteredBoxScores(results.data, homeOrVisitor);

                        setCareerGamesPlayed(homeVisitorFilteredBoxScores);
                        setCareerPlayerBoxScores(homeVisitorOverUnderFilteredBoxScores);
                    } catch (error) {
                        console.log(error);
                    } finally {
                        setIsFetching(false);
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
                        <div className="prop-results-text bold">
                            Career Games: 
                        </div>
                        <div className="prop-results">
                            <div className="prop-results-fraction">{careerPlayerBoxScores.length} / {careerGamesPlayed.length}</div> 
                            <div className="neon-orange">{careerGamesPlayed.length === 0 ? 0 : (100 * careerPlayerBoxScores.length / careerGamesPlayed.length).toFixed(2)}%</div>
                        </div>
                    </div>
                    <div className="prop-results-item">
                        <div className="prop-results-text bold">
                            {selectedSeason.replace("_", "-")} Games:
                        </div>
                        <div className="prop-results">
                            <div className="prop-results-fraction">{playerBoxScores.length} / {gamesPlayed.length}</div> 
                            <div className="neon-orange">
                              {(gamesPlayed.length === 0 ? 0 : (100 * playerBoxScores.length / gamesPlayed.length)).toFixed(2)}%
                            </div>
                        </div>
                    </div>
                    <div className="prop-results-item">
                        <div className="prop-results-text bold">
                            Last 10 Games:
                        </div>
                        <div className="prop-results">
                            <div className="prop-results-fraction">{lastTenFilteredBoxScores.length} / {gamesPlayed.length < 10 ? gamesPlayed.length : "10"}</div> 
                            <div className="neon-orange">
                              {
                                (gamesPlayed.length === 0
                                  ? 0
                                  : (100 * lastTenFilteredBoxScores.length / (gamesPlayed.length < 10 ? gamesPlayed.length : 10.0))
                                ).toFixed(2)}%
                            </div>
                        </div>
                    </div>
                </div>
                :
                <div className='text-center'>
                { roster.length > 0 ?
                    "NO STATS EXIST"
                    :
                    ""
                }
                </div>
            }
        </>
    );
}

export default MLBPropBetResults;