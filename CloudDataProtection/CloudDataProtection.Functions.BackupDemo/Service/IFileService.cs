using System.IO;
using System.Threading.Tasks;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Service.Result;

namespace CloudDataProtection.Functions.BackupDemo.Service
{
    public interface IFileService
    {
        Task<UploadFileResult> Upload(Stream stream, string uploadFileName);

        Task<Stream> GetDownloadStream(string id);
        
        FileDestination Destination { get; }
    }
}