using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudDataProtection.Functions.BackupDemo.Entities
{
    public class File
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public string DisplayName { get; set; }
        
        public long Bytes { get; set; }
        
        public string ContentType { get; set; }

        public List<FileDestinationInfo> UploadedTo { get; set; } = new List<FileDestinationInfo>(1);

        public bool IsUploaded => UploadedTo.Any(u => u.UploadSuccess);

        public void AddDestination(FileDestinationInfo info)
        {
            UploadedTo.Add(info);
        }
    }
}