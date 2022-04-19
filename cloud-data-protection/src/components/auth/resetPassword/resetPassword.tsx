import {Button, Input, Typography} from "@material-ui/core";
import {RouteComponentProps} from "react-router";
import {useHistory} from "react-router-dom";
import React, {useEffect, useState} from "react";
import {startLoading, stopLoading} from "common/progress/helper";
import SnackbarOptions from "common/snackbar/options";
import {useSnackbar} from "notistack";
import {CancelTokenSource} from "axios";
import {http} from "common/http";
import {AccountService} from "services/accountService";
import ResetPasswordInput from "services/input/account/resetPasswordInput";
import './resetPassword.css';

const ResetPassword = (props: RouteComponentProps) => {
    const [password, setPassword] = useState('');
    const [repeatPassword, setRepeatPassword] = useState('');
    const [token, setToken] = useState('');

    let cancelTokenSource: CancelTokenSource;

    const {enqueueSnackbar} = useSnackbar();

    const history = useHistory();

    const accountService = new AccountService();

    useEffect(() => {
        if (props.location.search) {
            const params = new URLSearchParams(props.location.search);

            const token = params.get('token');

            if (token) {
                setToken(token);
            } else {
                history.push('/');
            }
        } else {
            history.push('/');
        }

        return () => {
            cancelTokenSource?.cancel();
        }
    }, [props]);


    const handleSubmit = async (e: any) => {
        e.preventDefault();

        cancelTokenSource = http.CancelToken.source();

        const input: ResetPasswordInput = {token: token, password: password};

        if (password !== repeatPassword) {
            onError('Entered passwords do not match');
            return;
        }

        startLoading();

        await accountService.resetPassword(input, cancelTokenSource.token)
            .then(() => onSuccess())
            .catch((e: any) => onError(e))
            .finally(() => onFinish());
    }

    const onSuccess = () => {
        enqueueSnackbar('Your password has been set. You can now log in using the specified credentials', SnackbarOptions.success);

        history.push("/login");
    }

    const onError = (e: string) => {
        enqueueSnackbar(e, SnackbarOptions.error);
    }

    const onFinish = () => {
        setPassword("");
        setRepeatPassword('');

        stopLoading();
    }

    return (
        <div className='reset-password'>
            <Typography variant='h1' className='register__header'>Password reset</Typography>
            <form className='register__form' onSubmit={(e) => handleSubmit(e)}>
                <Input className='register__form__input' type="password" placeholder="New password" value={password}
                       onChange={(e) => setPassword(e.target.value)} autoComplete="new-password"/>
                <Input className='register__form__input' type="password" placeholder="Repeat password" value={repeatPassword}
                       onChange={(e) => setRepeatPassword(e.target.value)}/>
                <Button className='register__form__submit' type="submit" variant='contained' color='primary'>Reset password</Button>
            </form>
        </div>
    )
}

export default ResetPassword;