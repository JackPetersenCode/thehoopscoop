import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

interface SignInProps {
    selectedSport: string;
    setSelectedSport: React.Dispatch<React.SetStateAction<string>>;
}

const SignIn: React.FC<SignInProps> = ({ selectedSport, setSelectedSport }) => {
    const navigate = useNavigate();

    const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const sport = e.target.value;
        setSelectedSport(sport);
        if (sport === 'NBA' || sport === 'MLB') {
            navigate(`/${sport}`);
        }
    };

    return (
        <div style={{ padding: '2rem', textAlign: 'center' }}>
            <h2>Select a Sport</h2>
            <select value={selectedSport} onChange={handleChange}>
                <option value="">-- Choose a sport --</option>
                <option value="NBA">NBA</option>
                <option value="MLB">MLB</option>
            </select>
        </div>
    );
};

export default SignIn;
