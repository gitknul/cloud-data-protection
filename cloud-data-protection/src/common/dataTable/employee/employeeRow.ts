import {GridRowData} from "@mui/x-data-grid";

interface EmployeeRow extends GridRowData {
    id: number;
    firstName: string;
    lastName: string;
    contactEmailAddress: string;
    phoneNumber: string;
    fullName: string;
}

export default EmployeeRow;