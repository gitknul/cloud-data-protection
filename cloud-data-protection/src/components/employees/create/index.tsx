import {Button, FormControlLabel, FormLabel, IconButton, Input, Radio, RadioGroup, Typography} from "@material-ui/core";
import {ArrowBack} from "@material-ui/icons";
import {CancelTokenSource} from "axios";
import {formatEnum} from "common/formatting/enumFormat";
import {http} from "common/http";
import {startLoading, stopLoading} from "common/progress/helper";
import SnackbarOptions from "common/snackbar/options";
import {selectLoading} from "features/progressSlice";
import {useSnackbar} from "notistack";
import React, {useEffect, useState} from "react";
import {useSelector} from "react-redux";
import {Link, useHistory} from "react-router-dom";
import EmployeeService from "services/employeeService";
import CreateEmployeeInput from "services/input/employee/createEmployeeInput";
import {EmployeeResult, Gender} from "services/result/employee/employeeResult";
import './index.css';

const CreateEmployee = () => {
    let cancelTokenSource: CancelTokenSource;

    useEffect(() => {
        return () => {
            cancelTokenSource?.cancel();
        }
    }, [])

    const [firstName, setFirstName] = useState<string>('');
    const [lastName, setLastName] = useState<string>('');
    const [contactEmailAddress, setContactEmailAddress] = useState<string>('');
    const [phoneNumber, setPhoneNumber] = useState<string>('');
    const [gender, setGender] = useState<Gender>(Gender.NotSpecified);

    const service = new EmployeeService();

    const {enqueueSnackbar} = useSnackbar();
    const history = useHistory();

    const loading = useSelector(selectLoading);

    const onGenderChange = (e: string) => {
        const gender: Gender = parseInt(e) as Gender;

        setGender(gender);
    }

    const handleSubmit = async (e: any) => {
        e.preventDefault();

        const errors = validateFields();

        if (errors.length > 0) {
            const fieldString = errors.joinNice(', ', ' and ');

            const message = errors.length === 1 ? `The field ${fieldString} is required` : `The fields ${fieldString} are required`;

            onError(message);

            stopLoading();

            return;
        }

        startLoading();

        const input: CreateEmployeeInput = {firstName, lastName, contactEmailAddress, phoneNumber, gender};

        cancelTokenSource = http.CancelToken.source();

        await service.create(input, cancelTokenSource.token)
            .then(r => onCreateSuccess(r))
            .catch(e => onError(e))
            .finally(() => stopLoading());
    }

    const validateFields = () => {
        const errors: string[] = [];

        if (!firstName) {
            errors.push('first name');
        }

        if (!lastName) {
            errors.push('last name');
        }

        if (!phoneNumber) {
            errors.push('phone number');
        }

        if (!contactEmailAddress) {
            errors.push('email address');
        }

        if (gender === undefined) {
            errors.push('gender');
        }

        return errors;
    }

    const onCreateSuccess = (result: EmployeeResult) => {
        enqueueSnackbar(`Employee ${result.fullName} has been created`, SnackbarOptions.success)

        history.push('/employees');
    }

    const onError = (e: string) => {
        enqueueSnackbar(e, SnackbarOptions.error);
    }

    const canSubmit = () => {
        return !loading && validateFields().length === 0;
    }

    return (
        <div className='employee__create'>
            <div className='content__navigation'>
                <IconButton className='content__navigation__back' component={Link} to='/employees' color='secondary' size='small'>
                    <ArrowBack/>
                </IconButton>
            </div>
            <Typography variant='h2' className='employee__create__header'>New employee</Typography>
            <form className='employee__create__form' onSubmit={(e) => handleSubmit(e)}>
                <div className='employee__create__form__grid'>
                    <Input className='employee__form__input' autoFocus={true} type="text" placeholder="First name"
                           required value={firstName} onChange={(e) => setFirstName(e.target.value)}/>
                    <Input className='employee__form__input' type="text" placeholder="Surname" required
                           value={lastName} onChange={(e) => setLastName(e.target.value)}/>
                    <Input className='employee__form__input' type="email" placeholder="Email address" required
                           value={contactEmailAddress} onChange={(e) => setContactEmailAddress(e.target.value)}/>
                    <Input className='employee__form__input' type="phone" placeholder="Phone number" required
                           value={phoneNumber} onChange={(e) => setPhoneNumber(e.target.value)}/>
                </div>
                <FormLabel component='legend' className='employee__gender__label'>Gender</FormLabel>
                <RadioGroup className='employee__gender__group' onChange={(e) => onGenderChange(e.target.value)}
                            value={gender}>
                    <FormControlLabel className='employee__gender__select' key={Gender.Male.toString()}
                                      value={Gender.Male} control={<Radio/>} label={formatEnum(Gender.Male, Gender)}/>
                    <FormControlLabel className='employee__gender__select' key={Gender.Female.toString()}
                                      value={Gender.Female} control={<Radio/>}
                                      label={formatEnum(Gender.Female, Gender)}/>
                    <FormControlLabel className='employee__gender__select' key={Gender.NotSpecified.toString()}
                                      value={Gender.NotSpecified} control={<Radio/>}
                                      label={formatEnum(Gender.NotSpecified, Gender)}/>
                </RadioGroup>
                <Button className='employee__form__submit' type="submit" variant='contained' color='primary' disabled={!canSubmit()}>Create</Button>
            </form>
        </div>
    )
}

export default CreateEmployee;