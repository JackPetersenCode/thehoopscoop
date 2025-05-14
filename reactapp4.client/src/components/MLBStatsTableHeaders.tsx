import React from "react";
import { MLBSortingFunction, Column } from "../interfaces/StatsTable";


interface MLBStatsTableHeadersProps {
    sortingFunction: MLBSortingFunction;
    columns: Column[];
    //sortField?: string;
    //setSortField?: React.Dispatch<React.SetStateAction<string>>;
    order: string;
    //setOrder?: React.Dispatch<React.SetStateAction<string>>;
}


const MLBStatsTableHeaders: React.FC<MLBStatsTableHeadersProps> = ({ sortingFunction, columns, order }) => {


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
                            onClick={() => sortingFunction(accessor, order)}>
                            {label}
                        </th>
                    );
                })}
            </tr>
        </thead>
    );
};

export default MLBStatsTableHeaders;