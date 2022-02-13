using System.ComponentModel.DataAnnotations;

namespace CloudDataProtection.Controllers.Dto.Input
{
    public class ChangeEmailInput
    {
        [Required]
        public string Email { get; set; }
    }
}