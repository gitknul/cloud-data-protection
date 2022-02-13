namespace CloudDataProtection.Core.Messaging.Dto
{
    public class GoogleAccountConnectedMessage
    {
        public long UserId { get; set; }
        
        public string Email { get; set; }

        public GoogleAccountConnectedMessage()
        {
            
        }
    }
}