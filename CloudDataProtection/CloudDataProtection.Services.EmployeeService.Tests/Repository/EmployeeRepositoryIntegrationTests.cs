using System;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.EmployeeService.Data.Repository;
using CloudDataProtection.Services.EmployeeService.Entities;
using CloudDataProtection.Services.EmployeeService.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace CloudDataProtection.Services.EmployeeService.Tests.Repository
{
    public class EmployeeRepositoryIntegrationTests : IClassFixture<EmployeeRepositoryFixture>, IDisposable
    {
        private readonly EmployeeRepositoryFixture _fixture;
        private readonly IEmployeeRepository _repository;

        public EmployeeRepositoryIntegrationTests(EmployeeRepositoryFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _fixture.Output = output;

            _repository = _fixture.CreateRepository(nameof(EmployeeRepositoryIntegrationTests));
        }

        [Fact]
        public async Task TestGetAllReturnsAll()
        {
            int seededRows = 5000, expectedRows = 5000;

            await _fixture.Initialize(seededRows);
            
            PaginatedQueryResult<Employee> result = await _repository.GetAll(0, int.MaxValue, null, null);

            Assert.Equal(expectedRows, result.Items.Count);
            Assert.Equal(expectedRows, result.ItemCount);
        }
        
        [Fact]
        public async Task TestGetAllSkip()
        {
            int skip = 20;
            int seededRows = 100;
            int take = seededRows;

            int expectedRows = take - skip;

            await _fixture.Initialize(seededRows);

            PaginatedQueryResult<Employee> result = await _repository.GetAll(skip, expectedRows, null, null);

            Assert.Equal(expectedRows, result.Items.Count);
            Assert.Equal(seededRows, result.ItemCount);
        }

        [Fact]
        public async Task TestGetAllTake()
        {
            int take = 20;
            int seededRows = 100;

            await _fixture.Initialize(seededRows);

            PaginatedQueryResult<Employee> result = await _repository.GetAll(0, take, null, null);

            Assert.Equal(take, result.Items.Count);
            Assert.Equal(seededRows, result.ItemCount);
        }

        [Fact]
        public async Task TestGetAllSkipAndTake()
        {
            int skip = 20;
            int take = 20;
            int seededRows = 100;
            
            await _fixture.Initialize(seededRows);

            PaginatedQueryResult<Employee> result = await _repository.GetAll(skip, take, null, null);

            Assert.Equal(take, result.Items.Count);
            Assert.Equal(seededRows, result.ItemCount);
        }

        [Fact]
        public async Task TestGetAllWithOrderBy()
        {
            int firstBatchSkip = 0;
            int secondBatchSkip = 20;
            int take = 20;
            int seededRows = 1000;

            string orderBy = $"{nameof(Employee.FirstName)} DESC";
            
            await _fixture.Initialize(seededRows);

            PaginatedQueryResult<Employee> firstBatch = await _repository.GetAll(firstBatchSkip, take, orderBy, null);
            PaginatedQueryResult<Employee> secondBatch = await _repository.GetAll(secondBatchSkip, take, orderBy, null);

            Assert.Equal(take, firstBatch.Items.Count);
            Assert.Equal(seededRows, firstBatch.ItemCount);
            Assert.Equal(take, secondBatch.Items.Count);
            Assert.Equal(seededRows, secondBatch.ItemCount);

            for (int i = 0; i < take; i++)
            {
                bool hasNextElement = i < take - 1;

                Employee current = firstBatch.Items.ElementAt(i);

                Employee next = hasNextElement ? firstBatch.Items.ElementAt(i + 1) : secondBatch.Items.First();

                int compare = string.Compare(current.FirstName, next.FirstName, StringComparison.Ordinal);

                Assert.True(compare >= 0);
            }
        }

        [Fact]
        public async Task TestGetAllWithOrderByMultipleProperties()
        {
            int firstBatchSkip = 0;
            int secondBatchSkip = 20;
            int take = 20;
            int seededRows = 1000;

            string orderBy = $"{nameof(Employee.FirstName)} DESC, {nameof(Employee.LastName)} ASC";
            
            await _fixture.Initialize(seededRows);

            PaginatedQueryResult<Employee> firstBatch = await _repository.GetAll(firstBatchSkip, take, orderBy, null);
            PaginatedQueryResult<Employee> secondBatch = await _repository.GetAll(secondBatchSkip, take, orderBy, null);

            Assert.Equal(take, firstBatch.Items.Count);
            Assert.Equal(seededRows, firstBatch.ItemCount);
            Assert.Equal(take, secondBatch.Items.Count);
            Assert.Equal(seededRows, secondBatch.ItemCount);

            // First batch
            for (int i = 0; i < take; i++)
            {
                bool hasNextElement = i < take - 1;

                Employee current = firstBatch.Items.ElementAt(i);
                
                Employee next = hasNextElement ? firstBatch.Items.ElementAt(i + 1) : secondBatch.Items.First();

                int firstNameCompare = string.Compare(current.FirstName, next.FirstName, StringComparison.Ordinal);
                int lastNameCompare = string.Compare(current.LastName, next.LastName, StringComparison.Ordinal);
                
                Assert.True(firstNameCompare >= 0);
                
                // If the first name is equal, the last name should be sorted
                Assert.True(firstNameCompare == 0 || lastNameCompare <= 0);
            }
        }

        [Fact]
        public async Task TestGetExisting()
        {
            long id = 1;

            await _fixture.Initialize(5);

            Employee employee =await _repository.Get(id);
            
            Assert.NotNull(employee);
            Assert.Equal(id, employee.Id);
        }

        [Fact]
        public async Task TestGetNonExisting()
        {
            long id = 999;

            await _fixture.Initialize(5);

            Employee employee = await _repository.Get(id);
            
            Assert.Null(employee);
        }

        [Fact]
        public async Task TestGetNonExistingNoData()
        {
            long id = 999;

            await _fixture.Initialize();

            Employee employee = await _repository.Get(id);
            
            Assert.Null(employee);
        }

        [Fact]
        public async Task TestCreate()
        {
            await _fixture.Initialize();

            Employee employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.NotSpecified,
                ContactEmailAddress = "info@exmaple.com",
                PhoneNumber = "+31612345678",
                CreatedByUserId = 1
            };

            await _repository.Create(employee);
            
            Assert.Equal(1, employee.Id);
            Assert.NotEqual(DateTime.MinValue, employee.CreatedAt);
        }

        [Fact]
        public async Task TestCreateAndGet()
        {
            await _fixture.Initialize();
            
            Employee employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.NotSpecified,
                ContactEmailAddress = "info@exmaple.com",
                PhoneNumber = "+31612345678",
                CreatedByUserId = 1
            };

            await _repository.Create(employee);

            Employee retrieved = await _repository.Get(employee.Id);
            
            Assert.Equal(employee.FirstName, retrieved.FirstName);
            Assert.Equal(employee.LastName, retrieved.LastName);
            Assert.Equal(employee.Gender, retrieved.Gender);
            Assert.Equal(employee.ContactEmailAddress, retrieved.ContactEmailAddress);
            Assert.Equal(employee.PhoneNumber, retrieved.PhoneNumber);
            Assert.Equal(employee.CreatedByUserId, retrieved.CreatedByUserId);
            Assert.Equal(employee.CreatedAt, retrieved.CreatedAt);
            
            Assert.Equal(1, employee.Id);
        }

        [Fact]
        public async Task TestFindByUserEmailAddress()
        {
            await _fixture.Initialize(50);

            string email = "employee_10+user@cdp.dev";
            long id = 10;

            Employee employee = await _repository.FindByUserEmailAddress(email);
            
            Assert.NotNull(employee);
            Assert.Equal(id, employee.Id);
        }

        [Fact]
        public async Task TestFindByNonExistingUserEmailAddress()
        {
            await _fixture.Initialize(50);

            string email = "not_exist@example.com";

            Employee employee = await _repository.FindByUserEmailAddress(email);
            
            Assert.Null(employee);
        }

        [InlineData("Rare Name")]
        [Theory]
        public async Task TestSearchByFullName(string searchQuery)
        {
            await _fixture.Initialize(100);
                        
            Employee newEmployee = new Employee
            {
                Id = 1002,
                FirstName = "Rare",
                LastName = "Name",
                Gender = Gender.NotSpecified,
                ContactEmailAddress = "info@exmaple.com",
                PhoneNumber = "+31612345678",
                CreatedByUserId = 1
            };

            await _repository.Create(newEmployee);

            PaginatedQueryResult<Employee> result = await _repository.GetAll(0, int.MaxValue, null, searchQuery);
            
            Assert.Equal(searchQuery, result.Items.First().FullName);
            Assert.Equal(1, result.Items.Count);
            Assert.Equal(1, result.ItemCount);
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }
    }
}