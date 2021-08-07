using System;
using System.ComponentModel;
using CloudDataProtection.Functions.BackupDemo.Extensions;

namespace CloudDataProtection.Functions.BackupDemo.Entities
{
    public class FileDestinationInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public FileDestination Destination { get; set; }
        
        [Description("Id of the file at the upload destination")]
        public string FileId { get; set; }
        
        public bool UploadSuccess { get; set; }
        
        public DateTime UploadedStartedAt { get; set; } = DateTime.Now;
        
        public DateTime? UploadCompletedAt { get; set; }

        public FileDestinationInfo()
        {
            
        }

        public FileDestinationInfo(FileDestination destination)
        {
            Destination = destination;
        }

        public override string ToString()
        {
            if (!UploadSuccess)
            {
                return string.Concat(Destination.GetDescription(), " (error)");
            }
            
            return Destination.GetDescription();
        }
    }
}