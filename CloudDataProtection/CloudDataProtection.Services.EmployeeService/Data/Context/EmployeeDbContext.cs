using System.IO;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Data.Context;
using CloudDataProtection.Services.EmployeeService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CloudDataProtection.Services.EmployeeService.Data.Context
{
    public class EmployeeDbContext : EncryptedDbContextBase, IEmployeeDbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeDbContext()
        {
        }

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options, ITransformer transformer) : base(options, transformer)
        {
        }

        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0;
        }
        
        protected override void ConfigureForEfCoreTools(DbContextOptionsBuilder builder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();
            
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseNpgsql(connectionString);
        }
    }
}