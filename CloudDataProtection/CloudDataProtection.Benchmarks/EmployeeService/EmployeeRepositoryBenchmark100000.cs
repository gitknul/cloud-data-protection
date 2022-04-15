using BenchmarkDotNet.Attributes;
using CloudDataProtection.Benchmarks.EmployeeService.Fixture;

namespace CloudDataProtection.Benchmarks.EmployeeService
{
    public class EmployeeRepositoryBenchmark100000 : EmployeeRepositoryBenchmarkBase
    {
        public EmployeeRepositoryBenchmark100000()
        {
            Fixture = new EmployeeRepositoryBenchmarkFixture();
            Repository = Fixture.CreateRepository(nameof(EmployeeRepositoryBenchmark100000));
        }
        
        [GlobalSetup]
        public void Setup() => Fixture.Seed(100000);

        [GlobalCleanup]
        public void Cleanup() => Fixture.TearDown();

        [IterationSetup]
        public void IterationSetup() => Repository = Fixture.CreateRepository(nameof(EmployeeRepositoryBenchmark100000));

        [IterationCleanup]
        public void IterationCleanup() => Fixture.DisposeContext();
    }
}