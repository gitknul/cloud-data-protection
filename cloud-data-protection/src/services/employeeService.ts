import {AxiosResponse, CancelToken} from "axios";
import {http} from "common/http";
import {ServiceBase} from "services/base/serviceBase";
import CreateEmployeeInput from "services/input/employee/createEmployeeInput";
import {GetAllEmployeesInput} from "services/input/employee/getAllEmployeesInput";
import {EmployeeResult} from "services/result/employee/employeeResult";
import {GetAllEmployeesResult} from "services/result/employee/getAllEmployeesResult";

class EmployeeService extends ServiceBase {
    public async getAll(input: GetAllEmployeesInput, cancelToken?: CancelToken): Promise<GetAllEmployeesResult> {
        return await http.get('/Employee', { cancelToken, params: input })
            .then((response: AxiosResponse<GetAllEmployeesResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }

    public async get(id: number, cancelToken?: CancelToken): Promise<EmployeeResult> {
        return await http.get(`/Employee/${id}`, { cancelToken })
            .then((response: AxiosResponse<EmployeeResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }

    public async create(input: CreateEmployeeInput, cancelToken?: CancelToken): Promise<EmployeeResult> {
        return await http.post(`/Employee`, input, { cancelToken })
            .then((response: AxiosResponse<EmployeeResult>) => response.data)
            .catch((e: any) => this.onError(e));
    }
}

export default EmployeeService;