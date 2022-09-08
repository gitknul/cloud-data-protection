using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudDataProtection.Core.Extensions;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Linq.Extensions;
using CloudDataProtection.Services.EmployeeService.Data.Context;
using CloudDataProtection.Services.EmployeeService.Entities;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Services.EmployeeService.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IEmployeeDbContext _context;
        private readonly IEFCacheServiceProvider _cacheServiceProvider;

        public EmployeeRepository(IEmployeeDbContext context)
        {
            _context = context;
        }
        
        public EmployeeRepository(IEmployeeDbContext context, IEFCacheServiceProvider cacheServiceProvider)
        {
            _context = context;
            _cacheServiceProvider = cacheServiceProvider;
        }

        public async Task<Employee> Get(long id)
        {
            return _context.Employees
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public async Task Create(Employee entity)
        {
            _context.Employees.Add(entity);

            await _context.SaveAsync();

            _cacheServiceProvider?.ClearAllCachedEntries();
        }

        public async Task<PaginatedQueryResult<Employee>> GetAll(int skip, int take, string orderBy, string searchQuery)
        {
            IEnumerable<Employee> enumerable = string.IsNullOrEmpty(searchQuery)
                ? _context.Employees.Cacheable().AsQueryable()
                : await CreateFilteredQuery(searchQuery);

            int count = enumerable.Count();

            if (!string.IsNullOrEmpty(orderBy))
            {
                enumerable = enumerable.OrderBy(orderBy);
            }

            enumerable = enumerable.Skip(skip).Take(take);

            ICollection<Employee> items = enumerable.ToList();

            return new PaginatedQueryResult<Employee>(items, count);
        }
        
        async Task<IEnumerable<Employee>> CreateFilteredQuery(string searchQuery)
        {
            return _context.Employees
                .AsNoTracking().ToList()
                .Where(e =>
                    e.FirstName == searchQuery || e.FirstName.ContainsIgnoreCase(searchQuery) ||
                    e.LastName == searchQuery || e.LastName.ContainsIgnoreCase(searchQuery) ||
                    e.FullName == searchQuery || e.FullName.ContainsIgnoreCase(searchQuery) ||
                    e.ContactEmailAddress == searchQuery || e.ContactEmailAddress.ContainsIgnoreCase(searchQuery) ||
                    e.UserEmailAddress != null && (e.UserEmailAddress == searchQuery || e.UserEmailAddress.ContainsIgnoreCase(searchQuery)))
                .ToList();
        }

        public async Task<Employee> FindByUserEmailAddress(string email)
        {
            return _context.Employees
                .Where(e => e.UserEmailAddress == email)
                .FirstOrDefault();
        }
    }
}