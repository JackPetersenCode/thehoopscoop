import React, { useEffect } from 'react';
import { useState } from 'react';
import SeasonsDropDown from '../components/SeasonsDropDown';
import { nbaTeams } from '../interfaces/DropDownOptions';
import TeamsDropDown from './TeamsDropDown';
import { NBATeam } from '../interfaces/Teams';
import ShotChartsPlayerDropDown from './ShotChartsPlayerDropDown';
import { SelectedPlayer } from '../interfaces/Player';
import ShotChartsGamesDropDown from './ShotChartsGamesDropDown';
import { Shot, ShotChartsGamesData } from '../interfaces/Shot';
import styled from 'styled-components';
import axios from 'axios';
import ShotChartsSVG from './ShotChartsSVG';

const ContainerDiv = styled.div`
    display: flex;
`
const DropDownWrapper = styled.div`
    width: 100%;
    margin-right: 5px;
`
const SingleShotChartContainer = styled.div`
    display: flex;
    flex-direction: column;
    text-align: center;
    width: 100%;
`
const SVGContainer = styled.div`
  display: flex;
`
const ShotsTitleFlex = styled.div`
  display: flex;
  white-space: nowrap;
  margin: auto;
`
const MadeMissedShotDiv = styled.div`
  margin-left: 20px;
  width: 100%;
`
const ShotColorsFlexDiv = styled.div`
  display: flex;
  text-align: center;
  align-items: center;
  white-space: nowrap;
`
const GreenBlock = styled.div`
  width: 10px;
  height: 10px;
  background-color: rgb(191, 255, 0);
`
const BlueBlock = styled.div`
  width: 10px;
  height: 10px;
  background-color: rgb(64, 154, 244);
`
const ShotCharts = () => {

    const [selectedSeason, setSelectedSeason] = useState('2023_24');
    const [selectedPlayer, setSelectedPlayer] = useState<SelectedPlayer | string>("");
    const [selectedTeam, setSelectedTeam] = useState<NBATeam | string>("");
    const [selectedGame, setSelectedGame] = useState<ShotChartsGamesData | string>("");
    const [gameData, setGameData] = useState<ShotChartsGamesData[]>([]);
    const [seasonShotsData, setSeasonShotsData] = useState<Shot[]>([]);
    const [gameShotsData, setGameShotsData] = useState<Shot[]>([]);


    useEffect(() => {
        console.log('getSeasonShots');
        const getSeasonShots = async () => {

            if (typeof selectedPlayer === 'string') {
                return;
            }
            const results = await axios.get(`/api/Shot/${selectedPlayer.player_id}/${selectedSeason}`);
            setSeasonShotsData(results.data);
        }
        if (typeof selectedPlayer !== 'string') {
            getSeasonShots();
        }
    }, [selectedPlayer]);

    useEffect(() => {
        console.log('useEffect getGameShots')
        const getGameShots = async () => {

            if (typeof selectedGame === 'string' || typeof selectedPlayer === 'string') {
                return;
            }
            const results = await axios.get(`/api/Shot/${selectedPlayer.player_id}/${selectedSeason}/${selectedGame.game_id}`);
            setGameShotsData(results.data);
        }
        if (typeof selectedGame !== 'string') {
            getGameShots();
        }
    }, [selectedGame]);

    return (
        <>
            <ContainerDiv>
                <DropDownWrapper>
                    <SeasonsDropDown selectedSeason={selectedSeason}
                        setSelectedSeason={setSelectedSeason}
                        setSelectedPlayer={setSelectedPlayer}
                        setSelectedGame={setSelectedGame}
                        isShotCharts={true}
                    />
                </DropDownWrapper>  
                <DropDownWrapper>
                    <TeamsDropDown selectedTeam={selectedTeam}
                        setSelectedTeam={setSelectedTeam}
                        setGameData={setGameData} />
                </DropDownWrapper>
                <DropDownWrapper>
                    <ShotChartsPlayerDropDown selectedPlayer={selectedPlayer}
                        setSelectedPlayer={setSelectedPlayer}
                        selectedTeam={selectedTeam}
                        selectedSeason={selectedSeason}
                        setSelectedGame={setSelectedGame}
                    />
                </DropDownWrapper>
                <DropDownWrapper style={{marginRight: "0px"} }>
                    <ShotChartsGamesDropDown selectedPlayer={selectedPlayer}
                        selectedSeason={selectedSeason}
                        selectedTeam={selectedTeam}
                        selectedGame={selectedGame}
                        setSelectedGame={setSelectedGame}
                        gameData={gameData}
                        setGameData={setGameData}
                    />
                </DropDownWrapper>
            </ContainerDiv>

            <SVGContainer>
                <SingleShotChartContainer>
                    <ShotsTitleFlex>
                        <h1 style={{ width: '100%' }}>
                            {seasonShotsData.length > 0 ? `${selectedSeason} Regular Season` : ''}
                        </h1>
                        <MadeMissedShotDiv>
                            <ShotColorsFlexDiv>
                                <GreenBlock>
                                </GreenBlock>

                                <div>
                                    - Made Shot
                                </div>
                            </ShotColorsFlexDiv>
                            <ShotColorsFlexDiv>
                                <BlueBlock>
                                </BlueBlock>
                                <div>
                                    - Missed Shot
                                </div>
                            </ShotColorsFlexDiv>
                        </MadeMissedShotDiv>
                    </ShotsTitleFlex>
                    <div>
                        <ShotChartsSVG shotsData={seasonShotsData} isGameChart={false} />
                    </div>
                </SingleShotChartContainer>
                <SingleShotChartContainer>
                    <ShotsTitleFlex>
                        <h1>
                            {seasonShotsData.length > 0 && typeof selectedGame !== 'string' ? selectedGame.game_date + ' ' + selectedGame.matchup : ''}
                        </h1>
                        <MadeMissedShotDiv>
                            <ShotColorsFlexDiv>
                                <GreenBlock>
                                </GreenBlock>

                                <div>
                                    - Made Shot
                                </div>
                            </ShotColorsFlexDiv>
                            <ShotColorsFlexDiv>
                                <BlueBlock>
                                </BlueBlock>
                                <div>
                                    - Missed Shot
                                </div>
                            </ShotColorsFlexDiv>
                        </MadeMissedShotDiv>
                    </ShotsTitleFlex>
                    <div>
                        <ShotChartsSVG shotsData={gameShotsData} isGameChart={true} />
                    </div>
                </SingleShotChartContainer>
            </SVGContainer>
        </>
    );
}

export default ShotCharts;