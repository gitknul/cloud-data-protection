using BenchmarkDotNet.Attributes;
using CloudDataProtection.Benchmarks.EmployeeService.Fixture;

namespace CloudDataProtection.Benchmarks.EmployeeService
{
    public class EmployeeRepositoryBenchmark10000 : EmployeeRepositoryBenchmarkBase
    {
        public EmployeeRepositoryBenchmark10000()
        {
            Fixture = new EmployeeRepositoryBenchmarkFixture();
            Repository = Fixture.CreateRepository(nameof(EmployeeRepositoryBenchmark10000));
        }
        
        [GlobalSetup]
        public void Setup() => Fixture.Seed(10000);

        [GlobalCleanup]
        public void Cleanup() => Fixture.TearDown();

        [IterationSetup]
        public void IterationSetup() => Repository = Fixture.CreateRepository(nameof(EmployeeRepositoryBenchmark10000));

        [IterationCleanup]
        public void IterationCleanup() => Fixture.DisposeContext();
    }
}