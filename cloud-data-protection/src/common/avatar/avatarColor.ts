import EmployeeRow from "common/dataTable/employee/employeeRow";
import {EmployeeResult} from "services/result/employee/employeeResult";
import stc from "string-to-color";

export const getAvatarColor = (employee: EmployeeResult | EmployeeRow) => {
    return stc(employee.id);
}