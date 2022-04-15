import {Gender} from "services/result/employee/employeeResult";

interface CreateEmployeeInput {
    firstName: string;
    lastName: string;
    contactEmailAddress: string;
    phoneNumber: string;
    gender: Gender;
}

export default CreateEmployeeInput;