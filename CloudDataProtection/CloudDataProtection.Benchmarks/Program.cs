using BenchmarkDotNet.Running;
using CloudDataProtection.Benchmarks.EmployeeService;
using CloudDataProtection.Benchmarks.EmployeeService.Fixture;

namespace CloudDataProtection.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<EmployeeRepositoryBenchmark10000<EmployeeRepositoryBenchmarkFixture>>();
            BenchmarkRunner.Run<EmployeeRepositoryBenchmark10000<CacheEmployeeRepositoryBenchmarkFixture>>();
        }
    }
}