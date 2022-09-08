using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using CloudDataProtection.Benchmarks.EmployeeService.Fixture;

namespace CloudDataProtection.Benchmarks.EmployeeService
{
    public class EmployeeRepositoryBenchmark1000<TFixture> : EmployeeRepositoryBenchmarkBase where TFixture : IEmployeeRepositoryBenchmarkFixture, new()
    {
        public EmployeeRepositoryBenchmark1000()
        {
            Fixture = new TFixture();
        }
                
        [GlobalSetup]
        public void Setup() => Fixture.Seed(nameof(EmployeeRepositoryBenchmark1000<TFixture>), 1000);

        [GlobalCleanup]
        public void Cleanup() => Fixture.TearDown();

        [IterationSetup]
        public void IterationSetup() => Repository = Fixture.GetRepository(nameof(EmployeeRepositoryBenchmark1000<TFixture>));

        [IterationCleanup]
        public void IterationCleanup() => Fixture.DisposeContext();

        [Benchmark(OperationsPerInvoke = 100)]
        public async Task PerformanceEmployees_By_Id()
        {
            for (int i = 0; i < 100; i++)
            {
                await GetEmployee_By_Id(i * 5);
            }
        }
        
        [Benchmark(OperationsPerInvoke = 10)]
        public async Task PerformanceEmployees_10()
        {
            for (int i = 0; i < 10; i++)
            {
                await GetEmployees_10();
            }
        }
        
        [Benchmark(OperationsPerInvoke = 10)]
        public async Task PerformanceEmployees_100()
        {
            for (int i = 0; i < 10; i++)
            {
                await GetEmployees_100();
            }
        }
    }
}