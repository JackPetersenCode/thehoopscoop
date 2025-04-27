import { Column, Stats } from "../interfaces/StatsTable";

interface MLBStatsTableBodyProps {
    columns: Column[] | string[];
    tableData: Stats[] | string[];
    filteredBoxScores: Stats[];
}

const positionShorthandMap: Record<string, string> = {
    "Catcher": "C",
    "Designated Hitter": "DH",
    "First Base": "1B",
    "Second Base": "2B",
    "Third Base": "3B",
    "Shortstop": "SS",
    "Outfielder": "OF",  // covers both Outfield & Outfielder
    "Outfield": "OF",
    "Pitcher": "P",
    "Two-Way Player": "2W"
  };
  

const isTableDataArray = (input: Stats[] | string[]): input is Stats[] => {
    return typeof input[0] !== 'string';
};

const isColumnArray = (input: Column[] | string[]): input is Column[] => {
    return typeof input[0] !== 'string';
};
function isNumber(value: string | number): value is number {
    return typeof value === 'number';
}
//<td>
//    <button onClick={() => (deletePlayer(index))}>x</button>
//</td>

const MLBStatsTableBody: React.FC<MLBStatsTableBodyProps> = ({ columns, tableData, filteredBoxScores }) => {


    if (!isTableDataArray(tableData)) {
        console.log(tableData);
        return (
            <tbody>
                {tableData.length > 0 ? tableData.map((data, index) => (
                    <tr key={index}>
                        <td>{data}</td>
                    </tr>
                )) : 'Loading'}
            </tbody>
        );
    } else {
        if (isColumnArray(columns)) {
            
            return (
                <tbody>
                    {tableData.map((data, index) => {
                        return (
                            <tr key={index} >
                                {columns.map(({ accessor }) => {
                                let tData;

                                const value = data[accessor];

                                if (value !== null && value !== undefined) {
                                    if (typeof value === 'number') {
                                        if (accessor.includes("pct") && !accessor.includes("rank")) {
                                            tData = (value * 100).toFixed(1);
                                        } else if (["average", "obp", "slg", "ops"].includes(accessor)) {
                                            tData = value.toFixed(3).replace(/^0/, ''); // Remove leading 0
                                        } else if (["stolenBasePercentage", "atBatsPerHomeRun"].includes(accessor)) {
                                            tData = value.toFixed(2);
                                        } else {
                                            tData = value;
                                        }
                                    } else if (accessor === "leagueName") {
                                        if (value === "American League") {
                                            tData = "AL";
                                        } else if (value === "National League") {
                                            tData = "NL";
                                        } else {
                                            tData = "MLB";
                                        }
                                    } else if (accessor === "position") {
                                        tData = positionShorthandMap[value] || value;
                                    } else {
                                        tData = value.toString();
                                    }
                                } else {
                                    tData = "--";
                                }
                                    return <td className={accessor === "group_name" ? "group-name p-1" : "not-group-name"} key={accessor}>{tData}</td>;
                                })}
                            </tr>
                        );
                    })}
                </tbody>
            );
        }
    }
};



export default MLBStatsTableBody;