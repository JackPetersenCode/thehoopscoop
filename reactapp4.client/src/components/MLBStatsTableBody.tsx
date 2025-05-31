import React from "react";
import { Column, Stats } from "../interfaces/StatsTable";
import { MLBTeamIds } from "../interfaces/Teams";

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

const MLBStatsTableBody: React.FC<MLBStatsTableBodyProps> = React.memo(({ columns, tableData, filteredBoxScores }) => {

    return (
        <tbody>
            {tableData.map((data, index) => {
                return (
                    <tr key={index} className={filteredBoxScores.includes(data) ? "green" : ""}>
                        {columns.map(({ accessor }) => {
                            let tData;
                            const value = data[accessor];

                            if (accessor === "matchup") {
                                if (data["teamId"] === data["homeTeamId"]) {
                                    tData = (
                                        <span>{MLBTeamIds[Number(data["homeTeamId"])] + " vs. " + 
                                            MLBTeamIds[Number(data["awayTeamId"])]}</span>
                                    )
                                } else if (data["teamId"] === data["awayTeamId"]) {
                                    tData = (
                                        <span>{MLBTeamIds[Number(data["awayTeamId"])] + " @ " + 
                                            MLBTeamIds[Number(data["homeTeamId"])]}</span>
                                    )
                                } else {
                                    tData = "--";
                                }
                            }
                            else if (accessor != "matchup" && value !== null && value !== undefined) {
                                if (typeof value === 'number') {
                                    if (["average", "obp", "slg", "ops"].includes(accessor)) {
                                        tData = value.toFixed(3).replace(/^0/, ''); // Remove leading 0
                                    } else if (["stolenBasePercentage", "atBatsPerHomeRun", "whip", "era", "strikeoutsPer9", "homeRunsPer9", "runsScoredPer9", "walksPer9" ].includes(accessor)) {
                                        tData = value.toFixed(2);
                                    } else {
                                        tData = value;
                                    }
                                } else if (accessor === "gameDate") {
                                    tData = data[accessor].toString().split("T")[0];
                                } else if (accessor === "leagueName") {
                                    if (value === "American League") {
                                        tData = "AL";
                                    } else if (value === "National League") {
                                        tData = "NL";
                                    } else {
                                        tData = "MLB";
                                    }
                                } else if (accessor === "fullName") {
                                    tData = (
                                      <span>
                                        <span className="player-name">{value}</span>
                                        {data["position"] && (
                                          <span className="player-position"> {positionShorthandMap[data["position"]]}</span>
                                        )}
                                      </span>
                                    );
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