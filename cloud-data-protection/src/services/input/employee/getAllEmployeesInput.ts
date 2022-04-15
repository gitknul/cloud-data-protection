import {PaginatedInput} from "services/input/base/paginatedInput";
import {SortedInput} from "services/input/base/sortedInput";

export interface GetAllEmployeesInput extends SortedInput, PaginatedInput {
    searchQuery: string;
}