import React, { useState } from "react";
import { SortingFunction, Column } from "../interfaces/StatsTable";


interface StatsTableHeadersProps {
    columns: Column[] | string[];
    handleSorting: SortingFunction;
    smallHeaders: boolean;
    sortField: string;
    setSortField: React.Dispatch<React.SetStateAction<string>>;
    order: string;
    setOrder: React.Dispatch<React.SetStateAction<string>>;
}



const isColumnArray = (input: Column[] | string[]): input is Column[] => {
    return typeof input[0] !== 'string';
};


const StatsTableHeaders: React.FC<StatsTableHeadersProps> = ({ columns, handleSorting, smallHeaders, sortField, setSortField, order, setOrder }) => {

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
            const sortOrder =
                accessor === sortField && order === "asc" ? "desc" : "asc";
            setSortField(accessor);
            setOrder(sortOrder);
            //handleSorting(accessor, sortOrder);
        };
        if (smallHeaders) {
            return (
                <thead className="smallHeaders">
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
                <thead className="regularHeaders">
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