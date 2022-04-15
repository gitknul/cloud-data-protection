import React, { Fragment } from "react";
import {useSelector} from "react-redux";
import {selectUser} from "features/userSlice";
import AdminRouter from "router/admin";
import ClientRouter from "./client";
import EmployeeRouter from "./employee";
import AnonymousRouter from "./anonymous";
import UserRole from "entities/userRole";
import {Switch} from "react-router-dom";
import {Route} from "react-router";
import ConfirmEmailChange from "components/auth/changeEmail/confirmChangeEmail";
import ResetPassword from "components/auth/resetPassword/resetPassword";

const Router = () => {
    const user = useSelector(selectUser);

    const router = () => {
        if (!user) {
            return <AnonymousRouter />
        }

        switch (user.role) {
            case UserRole.Client:
                return <ClientRouter />
            case UserRole.Employee:
                return <EmployeeRouter />
            case UserRole.Admin:
                return <AdminRouter />
        }
    }

    return (
        <Fragment>
                <Switch>
                    <Route exact path='/ConfirmEmailChange' component={ConfirmEmailChange}/>
                    <Route exact path='/ResetPassword' component={ResetPassword}/>
                    {
                        router()
                    }
                </Switch>
        </Fragment>
    )
}

export default Router;