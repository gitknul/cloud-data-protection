import {EmployeeResult} from "services/result/employee/employeeResult";

export interface GetAllEmployeesResult {
    data: EmployeeResult[];
    itemCount: number;
}