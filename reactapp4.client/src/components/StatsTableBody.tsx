import { SortingFunction, Column, Stats } from "../interfaces/StatsTable";

interface StatsTableBodyProps {
    columns: Column[] | string[];
    tableData: Stats[] | string[];
    filteredBoxScores: Stats[];
}

const isTableDataArray = (input: Stats[] | string[]): input is Stats[] => {
    return typeof input[0] !== 'string';
};

const isColumnArray = (input: Column[] | string[]): input is Column[] => {
    return typeof input[0] !== 'string';
};
function isNumber(value: string | number): value is number {
    return typeof value === 'number';
}
//<td>
//    <button onClick={() => (deletePlayer(index))}>x</button>
//</td>

const StatsTableBody: React.FC<StatsTableBodyProps> = ({ columns, tableData, filteredBoxScores }) => {


    if (!isTableDataArray(tableData)) {
        return (
            <tbody>
                {tableData.length > 0 ? tableData.map((data, index) => (
                    <tr key={index}>
                        <td>{data}</td>
                    </tr>
                )) : 'Loading'}
            </tbody>
        );
    } else {
        if (isColumnArray(columns)) {

            return (
                <tbody>
                    {tableData.map((data, index) => {
                        return (
                            <tr key={index} className={filteredBoxScores.includes(data) ? "green" : ""} >
                                {columns.map(({ accessor }) => {
                                    let tData;
                                   
                                    if (typeof data === 'object' && accessor in data) {
                                        tData = data[accessor] !== null && data[accessor] !== undefined
                                            ? data[accessor].toString()
                                            : "--";
                                    } else {
                                        tData = "No data";
                                    }
                                    if (isNumber(data[accessor])) {

                                        if (accessor.includes("pct") && !accessor.includes("rank")) {
                                            
                                            const temp = data[accessor] as number * 100;
                                            tData = temp.toFixed(1);
                                        } else {
                                            tData = (data[accessor] as number).toFixed(1);
                                        }
                                    }

                                    return <td className={accessor === "group_name" ? "group-name p-1" : "not-group-name"} key={accessor}>{tData}</td>;
                                })}
                            </tr>
                        );
                    })}
                </tbody>
            );
        }
    }
};



export default StatsTableBody;