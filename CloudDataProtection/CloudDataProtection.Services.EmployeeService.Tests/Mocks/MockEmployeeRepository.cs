using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Extensions;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Linq.Extensions;
using CloudDataProtection.Services.EmployeeService.Data.Repository;
using CloudDataProtection.Services.EmployeeService.Entities;

namespace CloudDataProtection.Services.EmployeeService.Tests.Mocks
{
    public class MockEmployeeRepository : MockCrudRepositoryBase<Employee>, IEmployeeRepository
    {
        public Task<PaginatedQueryResult<Employee>> GetAll(int skip, int take, string orderBy, string searchQuery)
        {
            IEnumerable<Employee> enumerable = _data
                .Where(e =>
                    e.FirstName == searchQuery || e.FirstName.ContainsIgnoreCase(searchQuery) ||
                    e.LastName == searchQuery || e.LastName.ContainsIgnoreCase(searchQuery) ||
                    e.FullName == searchQuery || e.FullName.ContainsIgnoreCase(searchQuery) ||
                    e.ContactEmailAddress == searchQuery || e.ContactEmailAddress.ContainsIgnoreCase(searchQuery) ||
                    e.UserEmailAddress != null && (e.UserEmailAddress == searchQuery || e.UserEmailAddress.ContainsIgnoreCase(searchQuery)))
                .ToList();

            int count = enumerable.Count();

            List<Employee> items = enumerable
                .OrderBy(orderBy)
                .Skip(skip)
                .Take(take)
                .ToList();

            return Task.FromResult(new PaginatedQueryResult<Employee>(items, count));
        }

        public Task<Employee> FindByUserEmailAddress(string email)
        {
            return Task.FromResult(_data.FirstOrDefault(e => e.UserEmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase)));
        }
    }
}