using CloudDataProtection.Functions.BackupDemo.Entities;

namespace CloudDataProtection.Functions.BackupDemo.Triggers.Dto.Input
{
    public class FileUploadInput
    {
        public FileDestination[] Destinations { get; set; }
    }
}