namespace CloudDataProtection.Services.MailService.Dto
{
    public class PasswordUpdatedModel
    {
        public string Email { get; set; }
        
        public long UserId { get; set; }
    }
}