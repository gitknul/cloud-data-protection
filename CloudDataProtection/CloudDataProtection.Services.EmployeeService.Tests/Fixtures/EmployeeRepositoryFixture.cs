using System;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using CloudDataProtection.Services.EmployeeService.Data.Context;
using CloudDataProtection.Services.EmployeeService.Data.Repository;
using CloudDataProtection.Services.EmployeeService.Entities;
using CloudDataProtection.Services.EmployeeService.Tests.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace CloudDataProtection.Services.EmployeeService.Tests.Fixtures
{
    public class EmployeeRepositoryFixture : IDisposable
    {
        private const string ConnectionStringTemplate = @"Host=localhost;Port=5433;Database=employeedb_ci_{PLACEHOLDER};Username=postgres;Password=postgresCI";

        private static readonly Random Random = new Random();

        private static readonly DateTime CreatedAt = DateTime.Today.AddDays(-1);
        private static readonly DateTime ActivatedAt = DateTime.Today.AddDays(-1).AddHours(1);

        private static readonly string[] FirstNames = { "John", "James", "Peter", "David", "Thomas", "Mark", "Scott" };
        private static readonly string[] LastNames = { "Johnson", "May", "Smith", "Jones", "Miller", "Davies", "Williams" };

        private EmployeeDbContext _context;

        public ITestOutputHelper Output { get; set; }

        public IEmployeeRepository CreateRepository(string databaseName)
        {
            _context = CreateDbContext(databaseName);

            return new EmployeeRepository(_context);
        }

        public async Task Initialize() => await Initialize(0);

        public async Task Initialize(int rowCount)
        {
            Output.WriteLine("Deleting database...");

            await _context.Database.EnsureDeletedAsync();

            Output.WriteLine("Creating database...");

            await _context.Database.EnsureCreatedAsync();

            Output.WriteLine("Migrating database...");

            await _context.Database.MigrateAsync();

            Output.WriteLine($"Seeding database with {rowCount} rows...");

            _context.Employees.AddRange(Enumerable.Range(0, rowCount).Select(CreateMockEmployee));

            await _context.SaveAsync();

            Output.WriteLine("Database seeded...");
        }

        private static EmployeeDbContext CreateDbContext(string databaseName)
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

        private static Employee CreateMockEmployee(int row)
        {
            int id = row + 1;
            
            return new Employee
            {
                Id = id,
                CreatedAt = CreatedAt,
                ActivatedAt = ActivatedAt,
                ActivationStatus = EmployeeActivationStatus.Activated,
                ActivationStatusMessage = "Activated",
                ContactEmailAddress = $"employee_{id}@cdp.dev",
                UserEmailAddress = $"employee_{id}+user@cdp.dev",
                CreatedByUserId = 1,
                UserId = 1000 + id,
                FirstName = FirstNames[Random.Next(0, FirstNames.Length)],
                LastName = LastNames[Random.Next(0, LastNames.Length)],
                Gender = Gender.NotSpecified,
                PhoneNumber = "+31612345678"
            };
        }

        public void Dispose()
        {
            try
            {
                _context.Database.EnsureDeleted();
                _context.Dispose();
            }
            catch
            {
                // ignored
            }
        }
    }
}