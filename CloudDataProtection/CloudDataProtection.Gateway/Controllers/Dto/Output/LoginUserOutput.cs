using CloudDataProtection.Entities;

namespace CloudDataProtection.Controllers.Dto.Output
{
    public class LoginUserOutput
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}