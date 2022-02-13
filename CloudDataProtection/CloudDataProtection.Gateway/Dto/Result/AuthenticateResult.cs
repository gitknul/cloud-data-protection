namespace CloudDataProtection.Dto.Result
{
    public class AuthenticateResult
    {
        public string Token { get; set; }

        public LoginUserResult User { get; set; }
    }
}