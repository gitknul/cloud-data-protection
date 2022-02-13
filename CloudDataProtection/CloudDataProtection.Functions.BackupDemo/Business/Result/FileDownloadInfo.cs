using CloudDataProtection.Functions.BackupDemo.Entities;

namespace CloudDataProtection.Functions.BackupDemo.Business.Result
{
    public class FileDownloadInfo
    {
        public byte[] Bytes { get; set; }
        
        public string FileName { get; set; }
        
        public string ContentType { get; set; }
        
        public FileDestination? DownloadedFrom { get; set; }
    }
}