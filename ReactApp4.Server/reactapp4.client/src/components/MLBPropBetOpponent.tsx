import React, { SetStateAction } from 'react';
import { MLBTeam } from '../interfaces/Teams';

interface MLBPropBetOpponentProps {
    selectedOpponent: MLBTeam;
    setSelectedOpponent: React.Dispatch<SetStateAction<MLBTeam>>;
    showOpponent: boolean;
    setShowOpponent: React.Dispatch<SetStateAction<boolean>>;
}

const MLBPropBetOpponent: React.FC<MLBPropBetOpponentProps> = ({ selectedOpponent, setSelectedOpponent, showOpponent, setShowOpponent }) => {


    const deleteSelectedOpponent = () => {
        setSelectedOpponent({ team_id: '1', team_name: 'All Teams' });
        setShowOpponent(false);
    }

    return (
        <div>
            {showOpponent &&
                <div className="over-under-flex">
                    <div className="text-in-box">
                        {selectedOpponent.team_name}
                    </div>
                    <div>
                        <button className="x-button" onClick={deleteSelectedOpponent}>x</button>
                    </div>
                </div>
            }
        </div>
    );
}

export default MLBPropBetOpponent;
