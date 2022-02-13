namespace CloudDataProtection.Core.Messaging.Dto
{
    public class PasswordUpdatedMessage
    {
        public string Email { get; set; }
        public long UserId { get; set; }
    }
}