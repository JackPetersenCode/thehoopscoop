import { Outlet } from 'react-router-dom';
import Footer from '../components/Footer';

export default function Layout() {
	return (
		<div className="layout-container">
			<div className="layout-content">
				<Outlet />
			</div>
			<Footer />
		</div>
	);
}
