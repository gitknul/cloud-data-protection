using System;
namespace CloudDataProtection.App.Shared.Rest
{
    public class AuthenticateInput
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public AuthenticateInput()
        {
        }

        public AuthenticateInput(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
