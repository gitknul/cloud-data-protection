import {Button, Fab} from "@material-ui/core";
import {Add} from "@material-ui/icons";
import {
    DataGrid,
    GridColDef,
    GridRowModel,
    GridSortModel,
    GridValueGetterParams
} from "@mui/x-data-grid";
import DataTableOptions from "common/dataTable/dataTableOptions";
import EmployeeRow from "common/dataTable/employee/employeeRow";
import {EmployeesProps} from "components/employees/index";
import React, {Fragment, useEffect, useState} from 'react';
import {Link} from "react-router-dom";
import './index.css';

const EmployeesDesktop = (props: EmployeesProps) => {
    const [rows, setRows] = useState<EmployeeRow[]>([]);

    const {employees, itemCount} = props;

    useEffect(() => {
        setRows(props.employees);
    }, [employees, itemCount])

    const columns: GridColDef[] = [
        {
            field: 'firstName',
            headerName: 'Name',
            flex: 1,
            filterable: false,
            headerClassName: 'MuiDataGrid-columnHeader--no-separator'
        },
        {
            field: 'lastName',
            headerName: 'Surname',
            flex: 1,
            filterable: false,
            headerClassName: 'MuiDataGrid-columnHeader--no-separator'
        },
        {
            field: 'contactEmailAddress',
            headerName: 'Email',
            flex: 1,
            filterable: false,
            headerClassName: 'MuiDataGrid-columnHeader--no-separator'
        },
        {
            field: 'phoneNumber',
            headerName: 'Phone',
            flex: 1,
            sortable: false,
            filterable: false,
            headerClassName: 'MuiDataGrid-columnHeader--no-separator'
        },
        {
            field: 'id',
            headerName: 'Actions',
            renderHeader: () => <span/>,
            type: 'actions',
            flex: 0,
            sortable: false,
            filterable: false,
            headerClassName: 'MuiDataGrid-columnHeader--no-separator',
            renderCell: (params: GridValueGetterParams) => renderDetailsButton(params.row)
        },
    ];

    const onSortModelChange = (model: GridSortModel) => {
        props.setPage(1);
        props.setSort(convertSortModel(model));
    }

    const convertSortModel = (model: GridSortModel): string => {
        if (model.length === 0) {
            return 'Id DESC';
        }

        return model.map(m => firstUppercase(m.field) + ' ' + m.sort!.toUpperCase()).join(', ');
    }

    const firstUppercase = (input: string): string => {
        return input[0].toUpperCase() + input.substring(1);
    }   

    const renderDetailsButton = (row: GridRowModel) =>
        <Button component={Link} to={props.getEmployeeDetailUrl(row.id)} color='secondary' variant='contained'>
            Details
        </Button>

    return (
        <Fragment>
            <DataGrid
                className='employees__datagrid'
                columns={columns}
                rows={rows}
                loading={props.loading}
                rowCount={props.itemCount}
                pageSize={props.pageSize}
                onPageChange={(page) => props.setPage(page + 1)}
                onPageSizeChange={(pageSize: number) => props.setPageSize(pageSize)}
                pagination
                paginationMode='server'
                rowsPerPageOptions={DataTableOptions.rowsPerPageOptions}
                autoHeight={true}
                onSortModelChange={onSortModelChange}
                sortingMode='server'
            />
            <Fab component={Link} to='employees/create' color='secondary' className='fab--bottom-right'>
                <Add/>
            </Fab>
        </Fragment>
    )
}

export default EmployeesDesktop;