import React, { useState } from 'react';
import { Column, Stats } from '../interfaces/StatsTable';
import MLBStatsTableHeaders from './MLBStatsTableHeaders';
import MLBStatsTableBody from './MLBStatsTableBody';

interface MLBStatsTableProps {
  statsData: Stats[];
  columns: Column[];
  sortField: string;
  setSortField: React.Dispatch<React.SetStateAction<string>>;
  inputText: string;
  setInputText: React.Dispatch<React.SetStateAction<string>>;
}

const MLBStatsTable: React.FC<MLBStatsTableProps> = ({
  statsData,
  columns,
  sortField,
  setSortField,
  inputText,
  setInputText,
}) => {
  const [order, setOrder] = useState<'asc' | 'desc'>('desc');

  const handleSorting = (field: string, sortOrder: string) => {
    if (field) {
      const sorted = [...statsData].sort((a, b) => {
        if (a[field] == null) return 1;
        if (b[field] == null) return -1;

        const isNumber = typeof a[field] === 'number' && typeof b[field] === 'number';
        return isNumber
          ? ((a[field] as number) - (b[field] as number)) * (sortOrder === 'desc' ? -1 : 1)
          : a[field].toString().localeCompare(b[field].toString(), 'en', {
              numeric: true,
            }) * (sortOrder === 'desc' ? 1 : -1);
      });

      setOrder(order === 'asc' ? 'desc' : 'asc');
      setSortField(field);
    }
  };

  const filteredData = statsData.filter((element) => {
      //if no input the return the original
      //return the item which contains the user input
      if (element.fullName) {
          return element.fullName.toString().toLowerCase().includes(inputText.toLowerCase());
      }
  })

  return (
    <>
      {filteredData.length > 0 ? (
        <div className="player-box-container">
          <table className="w-100">
            <MLBStatsTableHeaders sortingFunction={handleSorting} columns={columns} order={order} />
            <MLBStatsTableBody columns={columns} tableData={filteredData} filteredBoxScores={[]} />
          </table>
        </div>
      ) : (
        <div className="no-stats-exist">NO STATS EXIST</div>
      )}
    </>
  );
};

export default MLBStatsTable;
