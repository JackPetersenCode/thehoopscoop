import React from 'react';
import { loadLeagueGamesBySeason, loadPlayers } from '../helpers/Loaders';

function ReactComponent() {
    return (
        <div>
            <button onClick={loadLeagueGamesBySeason}>League Games</button>
            <button onClick={loadPlayers}>Players</button>
        </div>
  );
}

export default ReactComponent;