using System.Collections.Generic;
using System.Linq;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Extensions;

namespace CloudDataProtection.Functions.BackupDemo.Triggers.Dto.Output
{
    public class FileUploadOutput
    {
        public string Id { get; set; }
        
        public string DisplayName { get; set; }

        public long Bytes { get; set; }
        
        public string ContentType { get; set; }
        
        public List<FileUploadDestinationOutputEntry> UploadedTo { get; set; }

        public bool HasErrors => UploadedTo.Any(u => !u.Success);

        public bool Success => UploadedTo.Any(u => u.Success);
    }
    
    public class FileUploadDestinationOutputEntry
    {
        public int FileDestination { get; set; }
        public string Description { get; set; }
        public bool Success { get; set; }

        public FileUploadDestinationOutputEntry(FileDestinationInfo info)
        {
            FileDestination = (int) info.Destination;
            Description = info.Destination.GetDescription();
            Success = info.UploadSuccess;
        }
    }
}