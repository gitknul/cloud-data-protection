using System;
using CloudDataProtection.Services.EmployeeService.Entities;

namespace CloudDataProtection.Services.EmployeeService.Controllers.Dto.Output
{
    public class EmployeeOutput
    {
        public long Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public Gender Gender { get; set; }
        
        public string ContactEmailAddress { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? ActivatedAt { get; set; }
        
        public EmployeeActivationStatus ActivationStatus { get; set; }
        
        public string ActivationStatusMessage { get; set; }
        
        public long? UserId { get; set; }

        public string UserEmailAddress { get; set; }
        
        public string FullName { get; set; }
    }
}