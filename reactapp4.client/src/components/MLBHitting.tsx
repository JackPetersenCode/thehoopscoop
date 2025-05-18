
import MLBFindPlayerBottom from '../components/MLBFindPlayerBottom';
import MLBSeasonsDropDown from '../components/MLBSeasonsDropDown';
import MLBLeagueOptionDropDown from '../components/MLBLeagueOptionDropDown';
import MLBSelectedTeamDropDown from '../components/MLBSelectedTeamDropDown';
import MLBYearToDateDropDown from '../components/MLBYearToDateDropDown';
import MLBOpponentDropDown from '../components/MLBOpponentDropDown';
import MLBSplitsDropDown from '../components/MLBSplitsDropDown';
import LoadingSelectDropDown from '../components/LoadingSelectDropDown';
import { MLBTeam } from '../interfaces/Teams';
import { MLBActivePlayer } from '../interfaces/MLBActivePlayer';
import { mlbLeagueOptions, mlbTeams, yearToDateOptions } from '../interfaces/MLBDropDownOptions';
import React from 'react';

interface Props {
  selectedSeason: string;
  setSelectedSeason: React.Dispatch<React.SetStateAction<string>>;
  leagueOption: string;
  setLeagueOption: React.Dispatch<React.SetStateAction<string>>;
  selectedTeam: MLBTeam;
  setSelectedTeam: React.Dispatch<React.SetStateAction<MLBTeam>>;
  yearToDateOption: string;
  setYearToDateOption: React.Dispatch<React.SetStateAction<string>>;
  selectedOpponent: MLBTeam;
  setSelectedOpponent: React.Dispatch<React.SetStateAction<MLBTeam>>;
  selectedSplit: string;
  setSelectedSplit: React.Dispatch<React.SetStateAction<string>>;
  inputTextBottom: string;
  setInputTextBottom: React.Dispatch<React.SetStateAction<string>>;
  selectedPlayerBottom: MLBActivePlayer | null;
  setSelectedPlayerBottom: React.Dispatch<React.SetStateAction<MLBActivePlayer | null>>;
  activePlayers: MLBActivePlayer[];
  isFetching: boolean;
  roster: MLBActivePlayer[];
  setRoster: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
  setUsedPlayers: React.Dispatch<React.SetStateAction<MLBActivePlayer[]>>;
}

const MLBHitting: React.FC<Props> = React.memo(({
  selectedSeason,
  setSelectedSeason,
  leagueOption,
  setLeagueOption,
  selectedTeam,
  setSelectedTeam,
  yearToDateOption,
  setYearToDateOption,
  selectedOpponent,
  setSelectedOpponent,
  selectedSplit,
  setSelectedSplit,
  inputTextBottom,
  setInputTextBottom,
  selectedPlayerBottom,
  setSelectedPlayerBottom,
  activePlayers,
  isFetching,
  roster,
  setRoster,
  setUsedPlayers
}) => {
    console.log('MLBHitting')
    return (
      <div className="display-flex">
      {isFetching ? (
        <>
          {Array.from({ length: 7 }).map((_, i) => (
            <div className="drop-down" key={i}>
              <LoadingSelectDropDown />
            </div>
          ))}
        </>
      ) : (
        <>
            <div className="drop-down">
              <MLBSeasonsDropDown
                selectedSeason={selectedSeason}
                setSelectedSeason={setSelectedSeason}
                isPredictions={false}
              />
            </div>
            <div className="drop-down">
              <MLBLeagueOptionDropDown
                leagueOption={leagueOption}
                setLeagueOption={setLeagueOption}
                setSelectedTeam={setSelectedTeam}
                options={mlbLeagueOptions} // update this dynamically if needed
              />
            </div>
            <div className="drop-down">
              <MLBSelectedTeamDropDown
                selectedTeam={selectedTeam}
                setSelectedTeam={setSelectedTeam}
                options={mlbTeams} // update dynamically if needed
              />
            </div>
            <div className="drop-down">
              <MLBYearToDateDropDown
                yearToDateOption={yearToDateOption}
                setYearToDateOption={setYearToDateOption}
                options={yearToDateOptions} // update dynamically if needed
              />
            </div>
            <div className="drop-down">
              <MLBOpponentDropDown
                selectedOpponent={selectedOpponent}
                setSelectedOpponent={setSelectedOpponent}
                options={mlbTeams} // update dynamically if needed
              />
            </div>
            <div className="drop-down">
              <MLBSplitsDropDown
                hittingPitching="hitting"
                selectedSplit={selectedSplit}
                setSelectedSplit={setSelectedSplit}
              />
            </div>
            <div className="drop-down">
              <MLBFindPlayerBottom
                activePlayers={activePlayers}
                inputTextBottom={inputTextBottom}
                setInputTextBottom={setInputTextBottom}
                selectedPlayerBottom={selectedPlayerBottom}
                setSelectedPlayerBottom={setSelectedPlayerBottom}
                roster={roster}
                setRoster={setRoster}
                setUsedPlayers={setUsedPlayers}
                gameOption="Prop Bet"
              />
            </div>
        </>)}
      </div>
    );
});

export default MLBHitting;
