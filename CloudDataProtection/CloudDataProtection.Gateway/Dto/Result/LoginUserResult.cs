using CloudDataProtection.Entities;

namespace CloudDataProtection.Dto.Result
{
    public class LoginUserResult
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
    }
}