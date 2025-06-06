import React from "react";
import { SortingFunction, Column } from "../interfaces/StatsTable";


interface StatsTableHeadersProps {
    columns: Column[] | string[];
    onSort: (column: string) => void; 
}



const isColumnArray = (input: Column[] | string[]): input is Column[] => {
    return typeof input[0] !== 'string';
};


const StatsTableHeaders: React.FC<StatsTableHeadersProps> = ({ columns, onSort }) => {

    if (!isColumnArray(columns)) {
        return (
            <thead>
                <tr>
                    {columns.map((col, index) => {
                        return (
                            <th key={index} >
                                {col}
                            </th>
                        );
                    })}
                </tr>
            </thead>
        );
    } else {
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
                            <th key={accessor} className={label === 'NAME' ? 'header-item' : 'header-item-center'} onClick={() => onSort(accessor)}>
                                {label}
                            </th>
                        );
                    })}
                </tr>
            </thead>
        );
    }
};

export default StatsTableHeaders;