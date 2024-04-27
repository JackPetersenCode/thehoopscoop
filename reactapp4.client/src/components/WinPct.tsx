import '../App.css';
import React, { useEffect, useState, useMemo } from "react";
import { getJsonResponseStartup } from '../helpers/GetJsonResponse';
import { NBATeam } from '../interfaces/Teams';

interface WinPctProps {
    selectedSeason: string;
    selectedTeam: NBATeam | string;
}

const WinPct: React.FC<WinPctProps> = ({ selectedSeason, selectedTeam }) => {

    const [winPct, setWinPct] = useState(0);
    const [greenOverall, setGreenOverall] = useState(0);
    const [redOverall, setRedOverall] = useState(0);
    const [redSeason, setRedSeason] = useState(0);
    const [greenSeason, setGreenSeason] = useState(0);

    useEffect(() => {

        const getWinPct = async() => {
            let results;
            if(!selectedTeam || selectedTeam === '0') {
                results = await getJsonResponseStartup(`/api/Gambling/winPct/${selectedSeason}`);
                console.log(results);
            } else {
                console.log(selectedTeam)
                results = await getJsonResponseStartup(`/api/Gambling/winPctByTeam/${selectedSeason}/${typeof selectedTeam === 'string' ? selectedTeam : selectedTeam.team_name}`);
                console.log(results);
            }
            let pct = 0;
            if (results.length > 0) {
                pct = (results[1].count / (results[1].count + results[0].count)) * 100;
                setRedSeason(results[0].count);
                setGreenSeason(results[1].count);
            } else {
                setRedSeason(0);
                setGreenSeason(0);
            }

            setWinPct(pct);
        }
        if(selectedSeason) {
            getWinPct();
        }
    }, [selectedSeason, selectedTeam])

    useMemo(() => {
        const getOverall = async() => {
            let results = await getJsonResponseStartup(`/api/Gambling/winPctOverall`);
            
            if (results.length > 0) {
                for(let i = 0; i < results.length; i++) {
                    if(results[i].green_red === 'green') {
                        setGreenOverall((greenOverall) => greenOverall += results[i].count);

                    } else {
                        setRedOverall((redOverall) => redOverall += results[i].count);
                    }
                }
            } else {
                setGreenOverall(0);
                setRedOverall(0);
            }
            

        }
        getOverall();
    }, [])



    return (
        <div className='win-pct-div'>
            {greenOverall + redOverall > 0 ?
            <div className='overall-flex' >
                <div className='overall-w-l'>
                    PREDICTIONS W-L RECORD (7 SEASONS):
                </div>
                <div className='overall-pct'>
                    {' ' + greenOverall + ' - ' + redOverall}  {'|'}  {' %' + (greenOverall / (redOverall + greenOverall) * 100).toFixed(2)}
                </div>
            </div>
            :
            ''}
            {greenOverall + redOverall > 0 ?
            <div>
                {selectedTeam !== '0' && selectedTeam !== '' ? 
                <div className='overall-flex' >
                    <div className='overall-w-l'>
                        {selectedSeason + ' ' + (selectedTeam as string).toUpperCase() + ':'}
                    </div>
                    <div className='overall-pct'>
                        {greenSeason + ' - ' + redSeason + ' | %' + winPct}
                    </div>
                </div>
                :
                <div className='overall-flex' >
                    <div className='overall-w-l'> 
                        {selectedSeason + ' ALL TEAMS:'}
                    </div>
                    <div className='overall-pct'>
                        {greenSeason + ' - ' + redSeason + ' | %' + winPct}
                    </div>
                </div>
                }
            </div>
            :
            ''}
        </div>
    )
}

export default WinPct;