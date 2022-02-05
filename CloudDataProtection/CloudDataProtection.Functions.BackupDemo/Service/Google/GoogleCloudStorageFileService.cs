using System;
using System.IO;
using System.Threading.Tasks;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Service.Result;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Object = Google.Apis.Storage.v1.Data.Object;

namespace CloudDataProtection.Functions.BackupDemo.Service.Google
{
    public class GoogleCloudStorageFileService : IFileService
    {
        private string ProjectId => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_GCS_PROJECT_ID");
        private string JsonPath => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_GCS_JSON_FILE");

        private static readonly string BucketName = "cdp-demo-storage";

        public FileDestination Destination => FileDestination.GoogleCloudStorage;

        public async Task<UploadFileResult> Upload(Stream stream, string uploadFileName)
        {
            try
            {
                StorageClient client = await GetClient();

                Object result = await client.UploadObjectAsync(BucketName, uploadFileName, null, stream);

                return UploadFileResult.Ok(result.Name);
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
                StorageClient client = await GetClient();

                Stream destination = new MemoryStream();

                DownloadObjectOptions options = new()
                {
                    DownloadValidationMode = DownloadValidationMode.Always
                };

                await client.DownloadObjectAsync(BucketName, id, destination, options);

                destination.Position = 0;

                return destination;
            }
            catch (Exception e)
            {
                return Stream.Null;
            }
        }

        private async Task<StorageClient> GetClient()
        {
            GoogleCredential credential = GoogleCredential.FromFile(JsonPath);
            
            StorageClient client = await StorageClient.CreateAsync(credential);

            try
            {
                Bucket bucket = await client.GetBucketAsync(BucketName);

                if (bucket == null)
                {
                    throw new GoogleApiException("storage", "The specified bucket does not exist");
                }
            }
            catch (GoogleApiException e)
            {
                Bucket newBucket = new()
                {
                    Name = BucketName,
                    StorageClass = StorageClasses.Standard
                };

                await client.CreateBucketAsync(ProjectId, newBucket);
            }

            return client;
        }
    }
}