import {IconButton, Input, Typography} from "@material-ui/core";
import {Search} from "@material-ui/icons";
import {http} from "common/http";
import {startLoading, stopLoading} from "common/progress/helper";
import SnackbarOptions from "common/snackbar/options";
import EmployeesDesktop from "components/employees/desktop";
import EmployeesMobile from "components/employees/mobile";
import {selectLoading} from "features/progressSlice";
import {useSnackbar} from "notistack";
import React, {ChangeEvent, useEffect, useState} from 'react';
import {BrowserView, MobileView} from 'react-device-detect';
import {useSelector} from "react-redux";
import EmployeeService from "services/employeeService";
import {EmployeeResult} from "services/result/employee/employeeResult";
import {GetAllEmployeesResult} from "services/result/employee/getAllEmployeesResult";
import debounce from 'lodash.debounce';
import './index.css';

export interface EmployeesProps {
    getEmployeeDetailUrl: (id: number) => string;
    appendData: boolean;
    employees: EmployeeResult[];
    itemCount: number;
    page: number;
    pageSize: number;
    sort: string;
    loading: boolean;
    setAppendData: (append: boolean) => void;
    setPage: (page: number) => void;
    setPageSize: (page: number) => void;
    setSort: (sort: string) => void;
}

const Employees = () => {
    let cancelTokenSource = http.CancelToken.source();
    let debouncedSearch = debounce(() => search(), 500);

    const [appendData, setAppendData] = useState<boolean>(false);
    const [employees, setEmployees] = useState<EmployeeResult[]>([]);
    const [itemCount, setItemCount] = useState<number>(0);
    const [pageSize, setPageSize] = useState<number>(10);
    const [page, setPage] = useState<number>(1);
    const [searchQuery, setSearchQuery] = useState<string>('');
    const [sort, setSort] = useState<string>('Id DESC');

    const loading = useSelector(selectLoading);

    const service = new EmployeeService();

    const {enqueueSnackbar} = useSnackbar();

    useEffect(() => {
        search();

        return () => {
            debouncedSearch?.cancel();
            cancelTokenSource?.cancel();
        }
    }, [page, pageSize, sort]);

    useEffect(() => {
        debouncedSearch();
    }, [searchQuery]);

    const search = () => {
        startLoading();

        service.getAll({pageSize, page, orderBy: sort, searchQuery}, cancelTokenSource.token)
            .then((result) => onGetAllEmployees(result))
            .catch((e) => onError(e))
            .finally(() => stopLoading());
    }

    const onGetAllEmployees = (result: GetAllEmployeesResult) => {
        if (!result) {
            return;
        }

        if (appendData) {
            setEmployees(employees.concat(result.data));
        } else {
            setEmployees(result.data);
        }

        setItemCount(result.itemCount);
    }

    const getEmployeeDetailUrl = (id: number) => {
        return `/employees/detail?id=${id}`;
    }

    const props: EmployeesProps = {
        getEmployeeDetailUrl, employees, itemCount,
        loading, page, pageSize, sort,
        setPage, setPageSize, setSort,
        appendData, setAppendData
    }

    const onError = (e: string) => {
        enqueueSnackbar(e, SnackbarOptions.error);
    }

    const onSearchChange = (e: ChangeEvent<HTMLInputElement>) => {
        setAppendData(false);
        setPage(1);
        setSearchQuery(e.target.value);
    }
    return (
        <div className='employees'>
            <Typography variant='h1'>Employees</Typography>
            {/* TODO Extract to component | https://github.com/OlivierBouchoms/cloud-data-protection/issues/48 */}
            <div className='search-container employees__search'>
                <Input className='employees__search__input' placeholder='Search' value={searchQuery}
                       onChange={onSearchChange} />
                <IconButton className='employees__search__button' size='small'>
                    <Search/>
                </IconButton>
            </div>
            <BrowserView renderWithFragment>
                <EmployeesDesktop {...props} />
            </BrowserView>
            <MobileView renderWithFragment>
                <EmployeesMobile {...props} />
            </MobileView>
        </div>
    )
}

export default Employees;