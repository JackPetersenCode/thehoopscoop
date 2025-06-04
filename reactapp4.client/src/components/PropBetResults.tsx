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
    lastTenFilteredBoxScores: Stats[];
    setIsFetching: React.Dispatch<SetStateAction<boolean>>;
}

const PropBetResults: React.FC<PropBetResultsProps> = ({ careerPlayerBoxScores, setCareerPlayerBoxScores, gamesPlayed, careerGamesPlayed, setCareerGamesPlayed, overUnderLine, 
    propBetStats, selectedOpponent, roster, playerBoxScores, homeOrVisitor, selectedSeason, lastTenFilteredBoxScores, setIsFetching }) => {
    //const [isFetching, setIsFetching] = useState<boolean>(false);

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
                        const results = await axios.get(`/api/PlayerResults?selectedSeason=1&selectedOpponent=${encodedJsonSelectedOpponent}&player_id=${player.player_id}&propBetStats=${encodedJsonPropBetStats}`);
                        console.log(results.data);

                        const OUFilteredBoxScores = await overUnderFilteredBoxScores(results.data, propBetStats, overUnderLine);
                        const homeVisitorOverUnderFilteredBoxScores = await homeAwayFilteredBoxScores(OUFilteredBoxScores, homeOrVisitor);
                        const homeVisitorFilteredBoxScores = await homeAwayFilteredBoxScores(results.data, homeOrVisitor);

              
                        setCareerGamesPlayed(homeVisitorFilteredBoxScores)
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
                    <div className="prop-results-item">
                        <div className="prop-results bold">
                            Last 10 Games:
                        </div>
                        <div className="prop-results">
                            <span className="prop-results-fraction">{lastTenFilteredBoxScores.length} / {gamesPlayed.length < 10 ? gamesPlayed.length : "10"}</span> <span className="neon-orange">{(100 * lastTenFilteredBoxScores.length / (gamesPlayed.length < 10 ? gamesPlayed.length : 10.0)).toFixed(2)}%</span>
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

export default PropBetResults;