namespace CloudDataProtection.Core.Messaging.Dto
{
    public class BackupConfigurationEnteredMessage
    {
        public long UserId { get; set; }
        
        public string Email { get; set; }
    }
}