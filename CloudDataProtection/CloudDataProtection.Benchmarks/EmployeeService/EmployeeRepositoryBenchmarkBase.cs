using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using CloudDataProtection.Benchmarks.EmployeeService.Fixture;
using CloudDataProtection.Services.EmployeeService.Data.Repository;

namespace CloudDataProtection.Benchmarks.EmployeeService
{
    public abstract class EmployeeRepositoryBenchmarkBase
    {
        protected EmployeeRepositoryBenchmarkFixture Fixture;
        protected IEmployeeRepository Repository;

        /// <summary>
        /// Employees to fetch: 10
        /// Properties to order by: 1
        /// </summary>
        [Benchmark]
        public virtual async Task PerformanceEmployees_10()
        {
            await Repository.GetAll(0, 10, "LastName ASC", "John");
        }

        /// <summary>
        /// Employees to fetch: 100
        /// Properties to order by: 1
        /// </summary>
        [Benchmark]
        public virtual async Task PerformanceEmployees_100()
        {
            await Repository.GetAll(0, 100, "LastName ASC", "John");
        }
    }
}