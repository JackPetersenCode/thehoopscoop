import { Outlet, Link } from 'react-router-dom'
import Footer from '../components/Footer'
export default function Layout() {
    return (
        <>
            <Outlet />
            <Footer />
        </>
    )
}