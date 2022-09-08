using CloudDataProtection.Services.EmployeeService.Data.Repository;

namespace CloudDataProtection.Benchmarks.EmployeeService.Fixture
{
    public interface IEmployeeRepositoryBenchmarkFixture
    {
        IEmployeeRepository GetRepository(string databaseName);

        void Seed(string databaseName, int rowCount);

        void TearDown();

        void DisposeContext();
    }
}