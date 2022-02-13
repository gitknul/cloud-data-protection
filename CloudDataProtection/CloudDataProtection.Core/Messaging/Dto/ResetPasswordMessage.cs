using System;

namespace CloudDataProtection.Core.Messaging.Dto
{
    public class ResetPasswordMessage
    {
        public long Id { get; set; }
        
        public string Email { get; set; }
        
        public string Url { get; set; }
        
        public DateTime Expiration { get; set; }
    }
}