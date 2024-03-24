import React, { SetStateAction, useState, useEffect } from 'react';
import { NBATeam } from '../interfaces/Teams';

interface PropBetOpponentProps {
    selectedOpponent: NBATeam;
    setSelectedOpponent: React.Dispatch<SetStateAction<NBATeam>>;
    showOpponent: boolean;
    setShowOpponent: React.Dispatch<SetStateAction<boolean>>;
}

const PropBetOpponent: React.FC<PropBetOpponentProps> = ({ selectedOpponent, setSelectedOpponent, showOpponent, setShowOpponent }) => {


    const deleteSelectedOpponent = () => {
        setSelectedOpponent({ team_id: '1', team_name: 'All Teams', team_abbreviation: '' });
        setShowOpponent(false);
    }

    return (
        <div>
            {showOpponent &&
                <div className="over-under-flex">
                    <div>
                        {selectedOpponent.team_abbreviation}
                    </div>
                    <div>
                        <button className="x-button" onClick={deleteSelectedOpponent}>x</button>
                    </div>
                </div>
            }
        </div>
    );
}

export default PropBetOpponent;
