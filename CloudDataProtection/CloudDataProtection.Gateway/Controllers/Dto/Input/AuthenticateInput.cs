using System.ComponentModel.DataAnnotations;

namespace CloudDataProtection.Controllers.Dto.Input
{
    public class AuthenticateInput
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}