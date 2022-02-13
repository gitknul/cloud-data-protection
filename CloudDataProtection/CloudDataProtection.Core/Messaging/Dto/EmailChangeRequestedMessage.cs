using System;

namespace CloudDataProtection.Core.Messaging.Dto
{
    public class EmailChangeRequestedMessage
    {
        public string NewEmail { get; set; }
        
        public string Url { get; set; }
        
        public DateTime ExpiresAt { get; set; }
    }
}