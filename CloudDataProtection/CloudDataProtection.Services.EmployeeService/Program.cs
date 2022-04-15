using CloudDataProtection.Core.DependencyInjection.Extensions;
using CloudDataProtection.Core.Papertrail.Extensions;
using CloudDataProtection.Services.EmployeeService.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CloudDataProtection.Services.EmployeeService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Migrate<IEmployeeDbContext, EmployeeDbContext>()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggingBuilder => loggingBuilder.AddPapertrail())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}