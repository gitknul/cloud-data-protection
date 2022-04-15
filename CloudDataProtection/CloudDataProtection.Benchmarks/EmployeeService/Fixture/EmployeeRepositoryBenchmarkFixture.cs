using System;
using System.Linq;
using CloudDataProtection.Benchmarks.EmployeeService.Context;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using CloudDataProtection.Services.EmployeeService.Data.Context;
using CloudDataProtection.Services.EmployeeService.Data.Repository;
using CloudDataProtection.Services.EmployeeService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Benchmarks.EmployeeService.Fixture
{
    public class EmployeeRepositoryBenchmarkFixture
    {
        private const string ConnectionStringTemplate = @"Host=localhost;Port=5433;Database=employeedb_benchmark_{PLACEHOLDER};Username=postgres;Password=postgresCI";

        private static readonly Random Random = new Random();

        private static readonly DateTime CreatedAt = DateTime.Today.AddDays(-1);
        private static readonly DateTime ActivatedAt = DateTime.Today.AddDays(-1).AddHours(1);

        private static readonly string[] FirstNames = { "John", "James", "Peter", "David", "Thomas", "Mark" };
        private static readonly string[] LastNames = { "Johnson", "May", "Smith", "Jones", "Miller", "Davies" };

        private IEmployeeDbContext _context;

        public IEmployeeRepository CreateRepository(string databaseName)
        {
            _context = CreateDbContext(databaseName);
            
            return new EmployeeRepository(_context);
        }

        public void Seed(int rowCount)
        {
            Console.WriteLine($"{LogTime} | Deleting database...");

            _context.Database.EnsureDeleted();

            Console.WriteLine($"{LogTime} | Creating database...");

             _context.Database.EnsureCreated();

            Console.WriteLine($"{LogTime} | Migrating database...");

            _context.Database.Migrate();

            Console.WriteLine($"{LogTime} | Seeding database with {rowCount.ToString()} rows...");

            Employee[] entities = Enumerable.Range(0, rowCount).Select(CreateMockEmployee).ToArray();
            
            _context.Employees.AddRange(entities);

            _context.SaveAsync().Wait();

            Console.WriteLine($"{LogTime} | Database seeded...");
        }

        public void TearDown() => _context?.Database.EnsureDeleted();

        private static Employee CreateMockEmployee(int row)
        {
            return new Employee
            {
                Id = row + 1,
                CreatedAt = CreatedAt,
                ActivatedAt = ActivatedAt,
                ActivationStatus = EmployeeActivationStatus.Activated,
                ActivationStatusMessage = "Activated",
                ContactEmailAddress = $"employee_{row}@cdp.dev",
                UserEmailAddress = $"employee_{row}@cdp.dev",
                CreatedByUserId = 1,
                UserId = 1000 + row,
                FirstName = FirstNames[Random.Next(0, FirstNames.Length)],
                LastName = LastNames[Random.Next(0, LastNames.Length)],
                Gender = Gender.Male,
                PhoneNumber = "+31612345678"
            };
        }
        
        private static TestEmployeeDbContext CreateDbContext(string databaseName)
        {
            IOptions<AesOptions> options = Options.Create(new AesOptions
            {
                EncryptionKey = "KD6Gg7K9jdxYNMvRIs4fh21DwgaDCcNAFGBKBn4G9xE=",
                EncryptionIv = "pdfn2d+30CvymJp2nAp1fg=="
            });

            string connectionString = ConnectionStringTemplate.Replace("{PLACEHOLDER}", databaseName);

            DbContextOptions<EmployeeDbContext> dbContextOptions = new DbContextOptionsBuilder<EmployeeDbContext>().UseNpgsql(connectionString).Options;

            return new TestEmployeeDbContext(dbContextOptions, new AesTransformer(options));
        }

        private static string LogTime => DateTime.Now.ToLongTimeString();

        public void DisposeContext()
        {
            _context?.Dispose();
            _context = null;
        }
    }
}