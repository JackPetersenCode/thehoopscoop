import { useEffect, useState } from 'react';
import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Home from './pages/Home'
import Layout from './pages/Layout'
import Admin from './pages/Admin';
import LoadingIndicator from './components/LoadingIndicator';
import axios from 'axios';
import MLB from './pages/MLB';
import SignIn from './pages/SignIn';
import { AuthProvider } from './auth/AuthContext';
import AdminLogin from './pages/AdminLogin';
import RequireAuth from './auth/RequireAuth';


function App() {

    //const [loading, setLoading] = useState(true);
    const [selectedSport, setSelectedSport] = useState<string>("");
    const [gameOption, selectedGameOption] = useState<string>("Prop Bet")
    //console.log("App")

    //useEffect(() => {
    //    const checkBackend = async () => {
    //        try {
    //            const response = await axios.get('/api/boxscoretraditional/2023_24'); // Health check endpoint
    //            if (response.data) {
    //                setLoading(false); // Backend is ready, stop loading
    //            } else {
    //                setTimeout(checkBackend, 1000); // Retry after 1 second
    //            }
    //        } catch (error) {
    //            console.error('Error checking backend:', error);
    //            setTimeout(checkBackend, 1000); // Retry after 1 second
    //        }
    //    };
//
    //    // Start checking the backend
    //    checkBackend();
    //}, []);

    return (
      <div>
          <AuthProvider>
            <BrowserRouter>
                <Routes>
                  <Route path="/" element={<Layout />}>
                    <Route path="/" element={<MLB selectedSport={selectedSport} setSelectedSport={setSelectedSport} />} />
                    <Route path="/MLB" element={<MLB selectedSport={selectedSport} setSelectedSport={setSelectedSport} />} />
                    <Route path="/NBA" element={<Home selectedSport={selectedSport} setSelectedSport={setSelectedSport} />} />
                    <Route path="/Admin" element={
                      <RequireAuth>
                        <Admin />
                      </RequireAuth>
                    } />                    
                    <Route path="/admin-login" element={<AdminLogin />} />
                  </Route>
                </Routes>
            </BrowserRouter>
          </AuthProvider>
      </div>
    );
}

export default App;