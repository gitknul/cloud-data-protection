using System.ComponentModel.DataAnnotations;
using CloudDataProtection.Services.EmployeeService.Entities;

namespace CloudDataProtection.Services.EmployeeService.Controllers.Dto.Input
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class CreateEmployeeInput
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string ContactEmailAddress { get; set; }
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        
        [Required]
        public Gender Gender { get; set; }
    }
}