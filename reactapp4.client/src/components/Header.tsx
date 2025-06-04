import GameOptionDropDown from "./GameOptionDropDown"
import SportOptionDropDown from "./SportOptionDropDown"


interface HeaderProps {
    selectedSport: string;
    setSelectedSport: React.Dispatch<React.SetStateAction<string>>;
    gameOption: string;
    setGameOption: React.Dispatch<React.SetStateAction<string>>;
}

const Header: React.FC<HeaderProps> = ({ selectedSport, setSelectedSport, gameOption, setGameOption }) => {
    let headerImage = selectedSport === "NBA" ? "/images/khlogo_nba_2.png" : "/images/khlogo_mlb_2.png"; 
    return (
            <div className='logo-flex'>
                <div className="image-div">
                    <img src={headerImage} alt="Home"/>
                </div>
                <div className="hide-title">
                    <div className="image-div">
                        <img src={"/images/knucklehead.png"} alt="KNUCKLEHEAD" />
                    </div>
                    <div className="image-div">
                        <img src={"/images/stats.png"} alt="STATS" />
                    </div>
                </div>
                <div className="sport-game-flex">
                    <SportOptionDropDown selectedSport={selectedSport} setSelectedSport={setSelectedSport} />
                    <GameOptionDropDown gameOption={gameOption} setGameOption={setGameOption} isMLB={selectedSport === 'MLB' ? true : false} />
                </div>
            </div>
    )
}

export default Header;