import CreateEmployee from "components/employees/create";
import EmployeeDetail from "components/employees/detail";
import React, {Fragment} from "react";
import {Route} from "react-router";
import Employees from "components/employees";
import Settings from "components/settings/settings";
import Home from "components/home/home";
import Logout from "components/logout/logout";

const AdminRouter = () => {
    return (
        <Fragment>
            <Route exact path='/logout' component={Logout}/>
            <Route exact path='/settings' component={Settings} />
            <Route exact path='/employees' component={Employees} />
            <Route exact path='/employees/create' component={CreateEmployee} />
            <Route path='/employees/detail' component={EmployeeDetail} />
            <Route exact path='/' component={Home}/>
        </Fragment>
    )
}

export default AdminRouter;