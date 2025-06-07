import { PropBetStats } from "../interfaces/PropBetStats";
import { Stats } from "../interfaces/StatsTable";

const overUnderFilteredBoxScores = async (boxScores: Stats[], propBetStats: PropBetStats[], overUnderLine: number | string | null) => {

    const filteredBoxScores = boxScores.filter((element: Stats) => {
        let total = 0;
        if (isNaN(overUnderLine as number)) {
            return true;
        } else {
            if (propBetStats != null && propBetStats.length > 0) {
                for (const stat of propBetStats) {
                    total += element[stat.accessor] as number;
                }
                return total >= (overUnderLine as number);
            } else {
                return true;
            }
        }
    });

    if (filteredBoxScores) {
        return filteredBoxScores;
    } else {
        return boxScores;
    }

}

const homeAwayFilteredBoxScores = async (boxScores: Stats[], homeOrVisitor: string) => {

    let filteredBoxScores = [];
    if (homeOrVisitor == "Home") {
        filteredBoxScores = boxScores.filter((element: Stats) => {
            if (element.matchup.toString().includes('vs.')) {
                return true;
            } else {
                return false;
            }
        })

        if (filteredBoxScores) {
            return filteredBoxScores;
        } else {
            return boxScores;
        }
    }
    else if (homeOrVisitor == "Visitor") {
        filteredBoxScores = boxScores.filter((element: Stats) => {
            if (element.matchup.toString().includes('@')) {
                return true;
            } else {
                return false;
            }
        })

        if (filteredBoxScores) {
            return filteredBoxScores;
        } else {
            return boxScores;
        }
    }
    else if (homeOrVisitor == "All Games") {
        return boxScores;

    } else {
        return boxScores;
    }
}

const MLBLastTenFilteredBoxScores = async(boxScores: Stats[], homeOrVisitor: string) => {
    //let filteredBoxScores = [];
    // Filter by team side (e.g., "home" or "away")
    // Return the last 10 games (most recent)
    const lastTen = boxScores.slice(10); // gets last 10 elements
    return lastTen;
}

const MLBhomeAwayFilteredBoxScores = async (boxScores: Stats[], homeOrVisitor: string) => {

    let filteredBoxScores = [];
    if (homeOrVisitor == "Home") {
        filteredBoxScores = boxScores.filter((element: Stats) => {
            if (element.teamSide === "home") {
                return true;
            } else {
                return false;
            }
        })

        if (filteredBoxScores) {
            return filteredBoxScores;
        } else {
            return boxScores;
        }
    }
    else if (homeOrVisitor == "Visitor") {
        filteredBoxScores = boxScores.filter((element: Stats) => {
            if (element.teamSide === "away") {
                return true;
            } else {
                return false;
            }
        })

        if (filteredBoxScores) {
            return filteredBoxScores;
        } else {
            return boxScores;
        }
    }
    else if (homeOrVisitor == "All Games") {
        return boxScores;

    } else {
        return boxScores;
    }
}



export { overUnderFilteredBoxScores, homeAwayFilteredBoxScores, MLBhomeAwayFilteredBoxScores, MLBLastTenFilteredBoxScores }