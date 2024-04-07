import React, { useState } from "react";
import { SortingFunction, Column } from "../interfaces/StatsTable";


interface StatsTableHeadersProps {
    columns: Column[] | string[];
    smallHeaders?: boolean;
    sortField?: string;
    setSortField?: React.Dispatch<React.SetStateAction<string>>;
    order?: string;
    setOrder?: React.Dispatch<React.SetStateAction<string>>;
    setPage?: React.Dispatch<React.SetStateAction<number>>;
}



const isColumnArray = (input: Column[] | string[]): input is Column[] => {
    return typeof input[0] !== 'string';
};


const StatsTableHeaders: React.FC<StatsTableHeadersProps> = ({ columns, smallHeaders, sortField, setSortField, order, setOrder, setPage }) => {

  
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
        const handleSortingChange = (accessor: string) => {
            if (setSortField && setOrder) {
                const sortOrder =
                    accessor === sortField && order === "asc" ? "desc" : "asc";
                setSortField(accessor);
                setOrder(sortOrder);
                //setPage(1);
            }
            //handleSorting(accessor, sortOrder);
        };
        if (smallHeaders) {
            return (
                <thead className="small-headers">
                    <tr>
                        {columns.map(({ label, accessor }) => {
                            return (

                                <th key={accessor} className="header-item" onClick={() => handleSortingChange(accessor)}>
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
                                <th key={accessor} className="header-item" onClick={() => handleSortingChange(accessor)}>
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