using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.EmployeeService.Business;
using CloudDataProtection.Services.EmployeeService.Entities;
using CloudDataProtection.Services.EmployeeService.Tests.Mocks;
using Xunit;

namespace CloudDataProtection.Services.EmployeeService.Tests.Business
{
    public class EmployeeBusinessLogicTests
    {
        private EmployeeBusinessLogic _logic;
        
        public EmployeeBusinessLogicTests()
        {
            Initialize();
        }

        [Fact]
        public async Task TaskGet()
        {
            long id = 1;
            
            BusinessResult<Employee> result = await _logic.Get(id);
            
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(id, result.Data.Id);
        }

        [Fact]
        public async Task TaskGetNonExisting()
        {
            long id = -1;
            
            BusinessResult<Employee> result = await _logic.Get(id);
            
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal(ResultError.NotFound, result.ErrorType);
        }

        [Fact]
        public async Task TestCreate()
        {
            Employee employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                ContactEmailAddress = "contact@cdp.dev",
                UserEmailAddress = "new@cdp.dev",
                Gender = Gender.NotSpecified
            };

            BusinessResult<Employee> result = await _logic.Create(employee);
            
            Assert.True(result.Success);
            Assert.NotEqual(0, result.Data.Id);
        }

        [Fact]
        public async Task TestCreateWithExistingUserEmail()
        {
            Employee employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                ContactEmailAddress = "contact@cdp.dev",
                UserEmailAddress = "user@cdp.dev",
                Gender = Gender.NotSpecified
            };

            BusinessResult<Employee> result = await _logic.Create(employee);
            
            Assert.False(result.Success);
            Assert.NotEmpty(result.Message);
        }

        [Fact]
        public async Task TestSearch()
        {
            PaginatedBusinessResult<Employee> result = await _logic.Search(0, 10, "Id ASC", "John Doe");
            
            Assert.True(result.Success);
            Assert.NotEmpty(result.Data);
            Assert.Equal("John", result.Data.First().FirstName);
            Assert.Equal("Doe", result.Data.First().LastName);
        }
        
        [Fact]
        public async Task TestSearchNoMatches()
        {
            PaginatedBusinessResult<Employee> result = await _logic.Search(0, 0, "Id ASC", "does not exist");
            
            Assert.True(result.Success);
            Assert.Empty(result.Data);
            Assert.Equal(0, result.ItemCount);
        }

        private void Initialize()
        {
            Employee employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                ContactEmailAddress = "contact@cdp.dev",
                UserEmailAddress = "user@cdp.dev",
                Gender = Gender.NotSpecified
            };
            
            MockEmployeeRepository repository = new  MockEmployeeRepository();
            
            repository.Create(employee);

            _logic = new EmployeeBusinessLogic(repository);
        }
    }
}