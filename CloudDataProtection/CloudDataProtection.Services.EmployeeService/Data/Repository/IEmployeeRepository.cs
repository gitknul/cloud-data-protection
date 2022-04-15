using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.EmployeeService.Entities;

namespace CloudDataProtection.Services.EmployeeService.Data.Repository
{
    public interface IEmployeeRepository
    {
        Task<Employee> Get(long id);

        Task Create(Employee entity);
        
        Task<PaginatedQueryResult<Employee>> GetAll(int skip, int take, string orderBy, string searchQuery);
        
        Task<Employee> FindByUserEmailAddress(string email);
    }
}