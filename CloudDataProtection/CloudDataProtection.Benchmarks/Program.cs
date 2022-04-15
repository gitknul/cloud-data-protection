using BenchmarkDotNet.Running;
using CloudDataProtection.Benchmarks.EmployeeService;

namespace CloudDataProtection.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<EmployeeRepositoryBenchmark1000>();
            BenchmarkRunner.Run<EmployeeRepositoryBenchmark10000>();
            BenchmarkRunner.Run<EmployeeRepositoryBenchmark100000>();
        }
    }
}