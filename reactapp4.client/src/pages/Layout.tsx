import { Outlet, Link } from 'react-router-dom'
export default function Layout() {
    return (
        <>

            <Link to="/">Home</Link>
            <br></br>

            OUTLET BELOW
            <Outlet />
        </>
    )
}