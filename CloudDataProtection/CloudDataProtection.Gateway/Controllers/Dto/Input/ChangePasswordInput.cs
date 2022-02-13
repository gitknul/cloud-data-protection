using System.ComponentModel.DataAnnotations;

namespace CloudDataProtection.Controllers.Dto.Input
{
    public class ChangePasswordInput
    {
        [Required]
        public string CurrentPassword { get; set; }
        
        [Required]
        public string NewPassword { get; set; }
    }
}