namespace CloudDataProtection.App.Shared.Rest
{
    public class AuthenticateOutput
    {
        public string Token { get; set; }

        public LoginUserOutput User { get; set; }

        public AuthenticateOutput()
        {
        }
    }

    public class LoginUserOutput
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        /// <summary>
        /// Client who uses CDP services
        /// </summary>
        Client = 0,

        /// <summary>
        /// Employee of CDP
        /// </summary>
        Employee = 1,

        /// <summary>
        /// Admin of CDP
        /// </summary>
        Admin = 2
    }
}


