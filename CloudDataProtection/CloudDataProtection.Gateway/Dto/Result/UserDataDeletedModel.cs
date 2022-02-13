using System;

namespace CloudDataProtection.Dto.Result
{
    public class UserDataDeletedModel
    {
        public long UserId { get; set; }
        
        public DateTime StartedAt { get; set; }
        
        public DateTime CompletedAt { get; set; }

        public string Service { get; set; }
    }
}