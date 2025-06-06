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

