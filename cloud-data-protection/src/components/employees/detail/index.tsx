import {Avatar, Card, CardContent, IconButton, Tooltip, Typography} from "@material-ui/core";
import {AddCircle, ArrowBack, Check, Email, InfoOutlined, Person, Phone, Sync} from "@material-ui/icons";
import {getAvatarColor} from "common/avatar/avatarColor";
import {formatEnum} from "common/formatting/enumFormat";
import {formatDate} from "common/formatting/timeFormat";
import {http} from "common/http";
import {startLoading, stopLoading} from "common/progress/helper";
import SnackbarOptions from "common/snackbar/options";
import {useSnackbar} from "notistack";
import React, {useEffect, useState} from 'react';
import {RouteComponentProps} from "react-router";
import {Link, useHistory} from "react-router-dom";
import EmployeeService from "services/employeeService";
import {EmployeeActivationStatus, EmployeeResult} from "services/result/employee/employeeResult";
import './index.css';

const EmployeeDetail = (props: RouteComponentProps) => {
    const [employee, setEmployee] = useState<EmployeeResult>();

    const {enqueueSnackbar} = useSnackbar();

    const history = useHistory();

    const service = new EmployeeService();

    useEffect(() => {
        let id;

        if (props.location.search) {
            const params = new URLSearchParams(props.location.search);

            try {
                id = parseInt(params.get('id') || '');
            } catch (e) {
                console.warn('Could not parse Employee ID')
            }
        }

        startLoading();

        if (!id) {
            return;
        }

        const cancelTokenSource = http.CancelToken.source();

        service.get(id, cancelTokenSource.token)
            .then(e => setEmployee(e))
            .catch(e => onError(e))
            .finally(() => stopLoading());

        return () => {
            cancelTokenSource?.cancel();
        }
    }, [])


    const onError = (e: string) => {
        enqueueSnackbar(e, SnackbarOptions.error);
        history.push('/employees')
    }

    return (
        <div className='employee__detail'>
            <div className='content__navigation'>
                <IconButton className='content__navigation__back' component={Link} to='/employees' color='secondary' size='small'>
                    <ArrowBack/>
                </IconButton>
            </div>
            <div className='employee__detail__title'>
                {employee?.fullName
                    ? <Avatar style={{background: getAvatarColor(employee)}}>
                        {employee.fullName[0].toUpperCase()}
                    </Avatar>
                    : <Avatar className='skeleton skeleton--avatar'/>
                }
                {employee?.fullName
                    ? <Typography variant='h2'>{employee.fullName}</Typography>
                    : <Typography variant='h2' className='skeleton skeleton--24 skeleton--flex skeleton--corners--sm'/>
                }
            </div>
            <div className='employee__detail__grid'>
                <Card>
                    <CardContent>
                        <div className='card__title'>
                            <Typography variant='h6' className='card__title__text--employee'>
                                Contact details
                            </Typography>
                        </div>
                        <div className='card__content'>
                            <div className='card__content__row'>
                                <Person/>
                                {employee?.contactEmailAddress
                                    ? <span>{employee.firstName + ' ' + employee.lastName}</span>
                                    : <div className='skeleton skeleton--16 skeleton--flex skeleton--corners--sm'/>
                                }
                            </div>
                            <div className='card__content__row'>
                                <Email/>
                                {employee?.contactEmailAddress
                                    ? <span>{employee.contactEmailAddress}</span>
                                    : <div className='skeleton skeleton--16 skeleton--flex skeleton--corners--sm'/>
                                }
                            </div>
                            <div className='card__content__row'>
                                <Phone/>
                                {employee?.phoneNumber
                                    ? <span>{employee.phoneNumber}</span>
                                    : <div className='skeleton skeleton--16 skeleton--flex skeleton--corners--sm'/>
                                }
                            </div>
                        </div>
                    </CardContent>
                </Card>
                <Card>
                    <CardContent>
                        <div className='card__title'>
                            <Typography variant='h6' className='card__title__text--employee'>
                                Account details
                            </Typography>
                        </div>
                        <div className='card__content'>
                            <div className='card__content__row'>
                                <Email/>
                                {employee?.userEmailAddress
                                    ? <span>{employee.userEmailAddress}</span>
                                    : <div className='skeleton skeleton--16 skeleton--flex'/>
                                }
                                {employee?.userEmailAddress &&
                                <Tooltip title="E-mail address used to log in" placement='right-end'>
                                    <InfoOutlined className='card__content__row__inline-icon'/>
                                </Tooltip>
                                }
                            </div>
                            <div className='card__content__row'>
                                <AddCircle/>
                                {employee?.createdAt
                                    ? <span>{formatDate(employee.createdAt)}</span>
                                    : <div className='skeleton skeleton--16 skeleton--flex'/>
                                }
                            </div>
                            <div className='card__content__row'>
                                <Check/>
                                {employee?.activationStatus !== undefined
                                    ? <span>{formatEnum(employee.activationStatus, EmployeeActivationStatus)}</span>
                                    : <div className='skeleton skeleton--16 skeleton--flex'/>
                                }
                            </div>
                            {(employee?.activationStatus === EmployeeActivationStatus.Activated || employee?.activationStatus === EmployeeActivationStatus.UserDeleted) &&
                            employee.activatedAt &&
                            <div className='card__content__row'>
                                <Sync/>
                                <span>{formatDate(employee.activatedAt)}</span>
                            </div>
                            }
                            {(employee?.activationStatusMessage) &&
                            <div className='card__content__row'>
                                <Sync/>
                                <span>{employee.activationStatusMessage}</span>
                            </div>
                            }
                        </div>
                    </CardContent>
                </Card>
            </div>
        </div>
    )
}

export default EmployeeDetail;