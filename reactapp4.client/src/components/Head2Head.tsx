import { useEffect, useState } from 'react';
import '../App.css';
import axios from 'axios';

interface Player {
    full_name: string,
    first_name: string,
    last_name: string,
    playerid: string,
    is_active: string
}

const Head2Head = () => {

    const [players, setPlayers] = useState([]);

    useEffect(() => {
      // Make a GET request to the Flask API endpoint
        const getPlayers = async() => {
            try {
                console.log('hello guy')
                let response = await axios.get('http://localhost:5000/api/players')
                console.log(response.data);
                setPlayers(response.data);
            } catch (error) {
              // Handle errors here
              console.error('Error fetching data:', error);
            }
        }
        getPlayers();
    }, []); // The empty dependency array ensures this effect runs once on component mount

    // <div>
    // <h1>Players:</h1>
    //   {players.map((player: Player, index: React.Key) => (
    //     <div key={index}>
    //       {player.full_name}
    //     </div>
    //   ))}
    // </div>
    return (
        <div>
            <br></br>
            <div className='statistics-title'>
                NBA Head 2 Head
            </div>
            <div className='yellow-line'>
            </div>
            <div className="under-construction">
                Under Construction
            </div>
            <div className='center'>
                <img className='steve-balmer' src="/images/UnderConstruction.jpg" />
            </div>
        </div>
    )
}

export default Head2Head;

