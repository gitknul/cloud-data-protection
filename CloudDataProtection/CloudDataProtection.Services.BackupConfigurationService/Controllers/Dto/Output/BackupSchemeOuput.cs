using CloudDataProtection.Services.Subscription.Entities;

namespace CloudDataProtection.Services.Subscription.Controllers.Dto.Output
{
    public class BackupSchemeOuput
    {
        public long Id { get; set; }
        
        public BackupFrequency Frequency { get; set; }

        public int Hour { get; set; }
        
        public int Minute { get; set; }

        public string Description => $"Backup my data {Frequency.ToString().ToLower()} at {Hour.ToString("00")}:{Minute.ToString("00")}";
    }
}