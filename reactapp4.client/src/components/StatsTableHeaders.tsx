import React from "react";
import { SortingFunction, Column } from "../interfaces/StatsTable";


interface StatsTableHeadersProps {
    sortingFunction: SortingFunction;
    columns: Column[] | string[];
    smallHeaders?: boolean;
    sortField?: string;
    setSortField?: React.Dispatch<React.SetStateAction<string>>;
    order: string;
    setOrder?: React.Dispatch<React.SetStateAction<string>>;
}



const isColumnArray = (input: Column[] | string[]): input is Column[] => {
    return typeof input[0] !== 'string';
};


const StatsTableHeaders: React.FC<StatsTableHeadersProps> = ({ sortingFunction, columns, smallHeaders, order }) => {

    console.log("headers")
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
        if (smallHeaders) {
            return (
                <thead className="small-headers">
                    <tr>
                        {columns.map(({ label, accessor }) => {
                            return (

                                <th key={accessor} className={label === 'NAME' ? 'header-item' : 'header-item-center'} onClick={() => sortingFunction(accessor, order)}>
                                    {label}
                                </th>
                            );
                        })}
                    </tr>
                </thead>
            );
        } else {
            return (
                <thead>
                    <tr>
                        {columns.map(({ label, accessor }) => {
                            return (
                                <th key={accessor} className={label === 'NAME' ? 'header-item' : 'header-item-center'} onClick={() => sortingFunction(accessor, order)}>
                                    {label}
                                </th>
                            );
                        })}
                    </tr>
                </thead>
            );
        }
    }
};

export default StatsTableHeaders;