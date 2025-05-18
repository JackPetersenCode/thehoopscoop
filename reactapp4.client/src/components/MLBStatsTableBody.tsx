import React from "react";
import { Column, Stats } from "../interfaces/StatsTable";

interface MLBStatsTableBodyProps {
    columns: Column[];
    tableData: Stats[];
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

const MLBStatsTableBody: React.FC<MLBStatsTableBodyProps> = React.memo(({ columns, tableData }) => {
    console.log("Body")
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
                                    if (["average", "obp", "slg", "ops"].includes(accessor)) {
                                        tData = value.toFixed(3).replace(/^0/, ''); // Remove leading 0
                                    } else if (["stolenBasePercentage", "atBatsPerHomeRun", "whip", "era", "strikeoutsPer9", "homeRunsPer9", "runsScoredPer9", "walksPer9" ].includes(accessor)) {
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
                                } else if (accessor === "position" || accessor === "primaryPositionName") {
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
});



export default MLBStatsTableBody;