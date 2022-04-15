import {Avatar, CircularProgress, Divider, Fab, List, ListItem, ListItemAvatar, ListItemText} from "@material-ui/core";
import {Add} from "@material-ui/icons";
import {getAvatarColor} from "common/avatar/avatarColor";
import EmployeeRow from "common/dataTable/employee/employeeRow";
import {EmployeesProps} from "components/employees/index";
import React, {Fragment, useEffect, useMemo, useState} from 'react';
import {Link} from "react-router-dom";
import InfiniteScroll from 'react-infinite-scroller';
import './index.css';

const EmployeesMobile = (props: EmployeesProps) => {
    const [rows, setRows] = useState<EmployeeRow[]>([]);

    const {employees, itemCount} = props;

    useEffect(() => {
        props.setAppendData(true);
        props.setSort('FirstName ASC, LastName ASC');
        props.setPageSize(20);
    }, []);

    useEffect(() => {
        setRows(props.employees);
    }, [employees])

    const onNext = (page: number) => {
        if (props.loading) {
            return;
        }

        props.setAppendData(true);
        props.setPage(props.page + 1);
    }

    // TODO Extract to component | https://github.com/OlivierBouchoms/cloud-data-protection/issues/48
    const endMessage = useMemo(() =>
            <div className='infinite-scroll__end' key={0}>
                <Divider/>
                <span className='infinite-scroll__end__text'>{itemCount} employees</span>
            </div>
        , [itemCount]);

    // TODO Extract to component | https://github.com/OlivierBouchoms/cloud-data-protection/issues/48
    const loader = useMemo(() =>
        <div className='infinite-scroll__progress' key={0}>
            <CircularProgress size='1.5rem'/>
        </div>, []);

    const hasMore: boolean = itemCount > rows.length;
    const renderedRows: number[] = [];

    return (
        <Fragment>
            <List className='employees__search__list' disablePadding={true}>
                <InfiniteScroll initialLoad={false} hasMore={hasMore} loadMore={onNext}
                                loader={loader} useWindow={true}>
                    {rows.map((employee) =>
                        /* Prevent double renders */
                        renderedRows.indexOf(employee.id) === -1 && renderedRows.push(employee.id) &&
                        <ListItem key={employee.id} button={true} component={Link}
                                  to={props.getEmployeeDetailUrl(employee.id)}
                                  className='employees__list__item'>
                            <ListItemAvatar>
                                <Avatar style={{background: getAvatarColor(employee)}}>
                                    {employee.fullName[0].toUpperCase()}
                                </Avatar>
                            </ListItemAvatar>
                            <ListItemText>
                                {employee.fullName}
                            </ListItemText>
                        </ListItem>
                    )}
                    {!hasMore && endMessage}
                </InfiniteScroll>
            </List>
            <Fab component={Link} to='employees/create' color='secondary' className='fab--bottom-right'>
                <Add/>
            </Fab>
        </Fragment>
    )
}

export default EmployeesMobile;