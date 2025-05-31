// helpers/MLBSortingFunction.ts

import { Stats, Column } from '../interfaces/StatsTable';

const nonNumeric = [
  "teamSide", "teamName", "personId", "fullName", "leagueName",
  "summary", "note", "full_name", "boxscore_name",
  "jersey_number", "position", "position_abbr",
  "status_code", "status_description"
];

export type SortOrder = "asc" | "desc" | "original";

export function getSortedData(
  data: Stats[],
  column: string,
  order: SortOrder,
  originalData: Stats[]
): Stats[] {
  if (order === "original") {
    return [...originalData];
  }

  const isNonNumeric = nonNumeric.includes(column);

  return [...data].sort((a, b) => {
    const valA = a[column];
    const valB = b[column];

    if (isNonNumeric) {
      return String(valA).localeCompare(String(valB)) * (order === "asc" ? 1 : -1);
    }

    return (Number(valA) - Number(valB)) * (order === "asc" ? 1 : -1);
  });
}
