import GameOptionDropDown from "./GameOptionDropDown"
import SportOptionDropDown from "./SportOptionDropDown"


interface HeaderProps {
    selectedSport: string;
    setSelectedSport: React.Dispatch<React.SetStateAction<string>>;
    gameOption: string;
    setGameOption: React.Dispatch<React.SetStateAction<string>>;
}

const Header: React.FC<HeaderProps> = ({ selectedSport, setSelectedSport, gameOption, setGameOption }) => {
    return (
            <div className='logo-flex'>
                <div>
                    <img src="/images/ball7.png" className="ball" alt="Home"/>
                </div>
                <div className="sport-game-flex">
                    <SportOptionDropDown selectedSport={selectedSport} setSelectedSport={setSelectedSport} />
                    <GameOptionDropDown gameOption={gameOption} setGameOption={setGameOption} isMLB={selectedSport === 'MLB' ? true : false} />
                </div>
            </div>
    )
}

export default Header;