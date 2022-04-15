using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Services.EmployeeService.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Services.EmployeeService.Tests.Context
{
    public class TestEmployeeDbContext : EmployeeDbContext
    {
        public TestEmployeeDbContext(DbContextOptions<EmployeeDbContext> options, ITransformer transformer) : base(options, transformer)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            // Disables logging
        }
    }
}