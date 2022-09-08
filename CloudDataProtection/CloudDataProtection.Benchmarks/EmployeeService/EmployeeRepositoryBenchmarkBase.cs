using System.Threading.Tasks;
using CloudDataProtection.Benchmarks.EmployeeService.Fixture;
using CloudDataProtection.Services.EmployeeService.Data.Repository;

namespace CloudDataProtection.Benchmarks.EmployeeService
{
    public abstract class EmployeeRepositoryBenchmarkBase
    {
        protected IEmployeeRepositoryBenchmarkFixture Fixture;
        protected IEmployeeRepository Repository;

        /// <summary>
        /// Employees to fetch: 1
        /// Properties to order by: 1
        /// </summary>
        public virtual async Task GetEmployee_By_Id(long id)
        {
            await Repository.Get(id);
        }

        /// <summary>
        /// Employees to fetch: 10
        /// Properties to order by: 1
        /// </summary>
        public virtual async Task GetEmployees_10()
        {
            await Repository.GetAll(0, 10, "LastName ASC", "John");
        }

        /// <summary>
        /// Employees to fetch: 100
        /// Properties to order by: 1
        /// </summary>
        public virtual async Task GetEmployees_100()
        {
            await Repository.GetAll(0, 100, "LastName ASC", "John");
        }
    }
}