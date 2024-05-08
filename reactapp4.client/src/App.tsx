import { useEffect, useState } from 'react';
import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Home from './pages/Home'
import Layout from './pages/Layout'
import Admin from './pages/Admin';
import LoadingIndicator from './components/LoadingIndicator';
import axios from 'axios';


function App() {

    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const checkBackend = async () => {
            try {
                const response = await axios.get('/api/boxscoretraditional/2023_24'); // Health check endpoint
                if (response.data) {
                    setLoading(false); // Backend is ready, stop loading
                } else {
                    setTimeout(checkBackend, 1000); // Retry after 1 second
                }
            } catch (error) {
                console.error('Error checking backend:', error);
                setTimeout(checkBackend, 1000); // Retry after 1 second
            }
        };

        // Start checking the backend
        checkBackend();
    }, []);

    return (
      <div>
        {loading ? 
            <LoadingIndicator />
            :
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Layout />}>
                        <Route path="/" element={<Home />} />
                        <Route path="/Admin" element={<Admin />} />
                    </Route>
                </Routes>
            </BrowserRouter>
        }
      </div>
    );
}

export default App;