using System;
using CloudDataProtection.Services.EmployeeService.Entities;
using Xunit;

namespace CloudDataProtection.Services.EmployeeService.Tests.Entities
{
    public class EmployeeTests
    {
        [Fact]
        public void TestSetAuditingInfo()
        {
            long createdByUserId = 9;
            
            Employee employee = new Employee();
            
            employee.SetAuditingInfo(createdByUserId);
            
            Assert.Equal(createdByUserId, employee.CreatedByUserId);
        }
        
        [Fact]
        public void TestCreatedAtIsAutomaticallySet()
        {
            Employee employee = new Employee();

            Assert.NotEqual(DateTime.MinValue, employee.CreatedAt);
            // Theoretically, a test could start at 23:59:59 and complete at 00:00:00
            Assert.True(employee.CreatedAt.Date == DateTime.Today || employee.CreatedAt.Date == DateTime.Today.AddDays(-1));
        }
    }
}