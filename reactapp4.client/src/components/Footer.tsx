import { FaGithub } from 'react-icons/fa';
import { TiSocialLinkedinCircular } from 'react-icons/ti';
import { Link } from "react-router-dom";


const Footer = () => {
    return (
        <div className='footer'>
            <div className="logo-nav-container">
                <div>
                    <Link to="https://www.github.com/JackPetersenCode/thehoopscoop" className="link" >
                        <FaGithub size={60} />
                    </Link>
                </div>
                <div>
                    <Link to="https://www.linkedin.com/in/JackPetersenCode" className="link" >
                        <TiSocialLinkedinCircular size={80} />
                    </Link>
                </div>
            </div>
            <div className='copyright'>
                <span style={{ fontSize: 'medium' }}>&copy;</span> 2023 | JackPetersenCode
            </div>
        </div>
    )
}

export default Footer;