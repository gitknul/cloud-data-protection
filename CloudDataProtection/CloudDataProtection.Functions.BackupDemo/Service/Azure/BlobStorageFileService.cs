using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Extensions;
using CloudDataProtection.Functions.BackupDemo.Service.Result;

namespace CloudDataProtection.Functions.BackupDemo.Service.Azure
{
    public class BlobStorageFileService : IBlobStorageFileService
    {
        private static string ConnectionString => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_STORAGE");

        private static readonly string ContainerName = "cdp-demo-blobstorage";
        
        public static bool IsEnabled
        {
            get
            {
                string environmentValue = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_DISABLE")?.ToLower();

                bool disabled = environmentValue == "1" || environmentValue == "true";

                return !disabled;
            }
        }

        public FileDestination Destination => FileDestination.AzureBlobStorage;

        public async Task<UploadFileResult> Upload(Stream stream, string uploadFileName)
        {
            try
            {
                BlobClient blobClient = await GetBlobClient(uploadFileName);

                Response<BlobContentInfo> response = await blobClient.UploadAsync(stream);

                if (!response.IsSuccessStatusCode())
                {
                    return UploadFileResult.Error();
                }

                return UploadFileResult.Ok(uploadFileName);
            }
            catch (Exception e)
            {
                return UploadFileResult.Error();
            }
        }

        public async Task<Stream> GetDownloadStream(string id)
        {
            try
            {
                BlobClient blobClient = await GetBlobClient(id);

                BlobDownloadInfo response = await blobClient.DownloadAsync();
                
                return response.Content;
            }
            catch (Exception e)
            {
                return Stream.Null;
            }
        }

        private async Task<BlobClient> GetBlobClient(string id)
        {
            BlobServiceClient client = new BlobServiceClient(ConnectionString);

            BlobContainerClient containerClient = client.GetBlobContainerClient(ContainerName);

            await containerClient.CreateIfNotExistsAsync();

            return containerClient.GetBlobClient(id);
        }
    }
}