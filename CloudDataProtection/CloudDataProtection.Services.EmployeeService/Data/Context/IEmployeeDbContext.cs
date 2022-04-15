using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CloudDataProtection.Services.EmployeeService.Data.Context
{
    public interface IEmployeeDbContext : IDisposable
    {
        DbSet<Entities.Employee> Employees { get; set; }
        
        DatabaseFacade Database { get; }

        Task<bool> SaveAsync();
    }
}