import { SortingFunction, Column, Stats } from "../interfaces/StatsTable";

interface StatsTableBodyProps {
    columns: Column[] | string[];
    tableData: Stats[] | string[];
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

const StatsTableBody: React.FC<StatsTableBodyProps> = ({ columns, tableData }) => {
    console.log(tableData);

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
        console.log('is table data array')
        if (isColumnArray(columns)) {
            console.log(columns)

            return (
                <tbody>
                    {tableData.map((data, index) => {
                        return (
                            <tr key={index}>
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
                                        
                                        tData = (data[accessor] as number).toFixed(2);
                                    }

                                    return <td key={accessor}>{tData}</td>;
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