import React, {useEffect} from "react";
import {AuthService} from "services/authService";
import {useSnackbar} from 'notistack';
import SnackbarOptions from "common/snackbar/options";
import { useHistory } from "react-router-dom";

const Logout = () => {
    const authService = new AuthService();

    const { enqueueSnackbar } = useSnackbar();

    const history = useHistory();

    const handleLogout = () => {
        authService.logout();

        enqueueSnackbar('You are now logged out', SnackbarOptions.info);

        history.push('/');
    }

    useEffect(() => {
        handleLogout();
    })

    return (
        <div className='logout'>
        </div>
    )
}

export default Logout;