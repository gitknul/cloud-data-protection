namespace CloudDataProtection.Controllers.Dto.Input
{
    public class ResetPasswordInput
    {
        public string Password { get; set; }
        
        public string Token { get; set; }
    }
}