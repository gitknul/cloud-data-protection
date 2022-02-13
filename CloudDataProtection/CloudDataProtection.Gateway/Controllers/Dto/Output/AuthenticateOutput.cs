namespace CloudDataProtection.Controllers.Dto.Output
{
    public class AuthenticateOutput
    {
        public string Token { get; set; }

        public LoginUserOutput User { get; set; }
    }
}