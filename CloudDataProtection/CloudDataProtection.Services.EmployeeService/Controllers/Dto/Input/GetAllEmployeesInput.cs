using System.ComponentModel.DataAnnotations;
using CloudDataProtection.Core.Controllers.Dto.Input;

namespace CloudDataProtection.Services.EmployeeService.Controllers.Dto.Input
{
    public class GetAllEmployeesInput : ISortedInput, IPaginatedInput
    {
        [Required]
        public string OrderBy { get; set; }

        [Range(5, 100)]
        public int PageSize { get; set; } = 20;

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        public string SearchQuery { get; set; } = "";

        public int Skip => (Page - 1) * PageSize;
    }
}