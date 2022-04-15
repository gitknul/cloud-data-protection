using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using CloudDataProtection.Benchmarks.EmployeeService.Fixture;

namespace CloudDataProtection.Benchmarks.EmployeeService
{
    public class EmployeeRepositoryBenchmark1000 : EmployeeRepositoryBenchmarkBase
    {
        public EmployeeRepositoryBenchmark1000()
        {
            Fixture = new EmployeeRepositoryBenchmarkFixture();
            Repository = Fixture.CreateRepository(nameof(EmployeeRepositoryBenchmark1000));
        }
                
        [GlobalSetup]
        public void Setup() => Fixture.Seed(1000);

        [GlobalCleanup]
        public void Cleanup() => Fixture.TearDown();

        [IterationSetup]
        public void IterationSetup() => Repository = Fixture.CreateRepository(nameof(EmployeeRepositoryBenchmark1000));

        [IterationCleanup]
        public void IterationCleanup() => Fixture.DisposeContext();

        [Benchmark(OperationsPerInvoke = 10)]
        public override async Task PerformanceEmployees_10()
        {
            await base.PerformanceEmployees_10();
            await base.PerformanceEmployees_10();
            await base.PerformanceEmployees_10();
            await base.PerformanceEmployees_10();
            await base.PerformanceEmployees_10();
            await base.PerformanceEmployees_10();
            await base.PerformanceEmployees_10();
            await base.PerformanceEmployees_10();
            await base.PerformanceEmployees_10();
            await base.PerformanceEmployees_10();
        }

        [Benchmark(OperationsPerInvoke = 10)]
        public override async Task PerformanceEmployees_100()
        {
            await base.PerformanceEmployees_100();
            await base.PerformanceEmployees_100();
            await base.PerformanceEmployees_100();
            await base.PerformanceEmployees_100();
            await base.PerformanceEmployees_100();
            await base.PerformanceEmployees_100();
            await base.PerformanceEmployees_100();
            await base.PerformanceEmployees_100();
            await base.PerformanceEmployees_100();
            await base.PerformanceEmployees_100();
        }
    }
}