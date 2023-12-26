import React from 'react';
import { loadBoxScoresTraditional, loadLeagueGamesBySeason, loadPlayers } from '../helpers/Loaders';

function ReactComponent() {
    return (
        <div>
            <button onClick={loadLeagueGamesBySeason}>League Games</button>
            <button onClick={loadPlayers}>Players</button>
            <button onClick={loadBoxScoresTraditional}>Box Score Traditional</button>
        </div>
  );
}

export default ReactComponent;