import React from "react";
import { Column } from "../interfaces/StatsTable";


interface MLBStatsTableHeadersProps {
    columns: Column[];
    onSort: (column: string) => void; 
}


const MLBStatsTableHeaders: React.FC<MLBStatsTableHeadersProps> = React.memo(({ columns, onSort }) => {

    //const handleSortingChange = (accessor: string) => {
    //    if (setSortField && setOrder) {
    //        const sortOrder =
    //            accessor === sortField && order === "asc" ? "desc" : "asc";
    //        setSortField(accessor);
    //        setOrder(sortOrder);
    //        //setPage(1);
    //    }
    //    //handleSorting(accessor, sortOrder);
    //};

    return (
        <thead className="small-headers">
            <tr>
                {columns.map(({ label, accessor }) => {
                    return (
                        <th key={accessor} className={label === 'NAME' ? 'header-item' : 'header-item-center'} 
                            onClick={() => onSort(accessor)}>
                            {label}
                        </th>
                    );
                })}
            </tr>
        </thead>
    );
});

export default MLBStatsTableHeaders;