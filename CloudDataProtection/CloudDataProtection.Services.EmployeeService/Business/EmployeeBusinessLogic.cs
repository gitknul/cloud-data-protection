using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.EmployeeService.Data.Repository;
using CloudDataProtection.Services.EmployeeService.Entities;

namespace CloudDataProtection.Services.EmployeeService.Business
{
    public class EmployeeBusinessLogic
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeBusinessLogic(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<BusinessResult<Employee>> Create(Employee employee)
        {
            if (await _repository.FindByUserEmailAddress(employee.UserEmailAddress) != null)
            {
                return BusinessResult<Employee>.Conflict($"An employee with the e-mail address {employee.UserEmailAddress} already exists.");
            }
            
            await _repository.Create(employee);
            
            return BusinessResult<Employee>.Ok(employee);
        }

        public async Task<PaginatedBusinessResult<Employee>> Search(int skip, int take, string orderBy, string searchQuery)
        {
            PaginatedQueryResult<Employee> result = await _repository.GetAll(skip, take, orderBy, searchQuery);
            
            return PaginatedBusinessResult<Employee>.Ok(result.Items, result.ItemCount);
        }

        public async Task<BusinessResult<Employee>> Get(long id)
        {
            Employee employee = await _repository.Get(id);

            if (employee == null)
            {
                return BusinessResult<Employee>.NotFound($"Could not find employee with id = {id}");
            }
            
            return BusinessResult<Employee>.Ok(employee);
        }
    }
}