using System.Collections.Generic;
using CloudDataProtection.Functions.BackupDemo.Entities;

namespace CloudDataProtection.Functions.BackupDemo.Triggers.Dto
{
    public class FileInfoResult
    {
        public string Name { get; set; }

        public long Bytes { get; set; }
        
        public string ContentType { get; set; }
        
        public List<FileInfoDestinationResultEntry> UploadedTo { get; set; }
    }

    public class FileInfoDestinationResultEntry
    {
        public int FileDestination { get; set; }
        public string Description { get; set; }

        public FileInfoDestinationResultEntry(FileDestinationInfo info)
        {
            FileDestination = (int) info.Destination;
            Description = info.ToString();
        }
    }
}