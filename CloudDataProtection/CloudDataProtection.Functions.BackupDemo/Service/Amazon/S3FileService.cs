using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Functions.BackupDemo.Entities;
using CloudDataProtection.Functions.BackupDemo.Extensions;
using CloudDataProtection.Functions.BackupDemo.Service.Result;

namespace CloudDataProtection.Functions.BackupDemo.Service.Amazon
{
    public class S3FileService : IFileService
    {
        private static string AccessKeyId => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_AWS_KEY");
        private static string AccessSecret => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_AWS_SECRET");

        private static readonly string BucketName = "cdp-demo-s3";

        public FileDestination Destination => FileDestination.AmazonS3;

        public async Task<UploadFileResult> Upload(Stream stream, string uploadFileName)
        {
            try
            {
                AmazonS3Client client = await GetS3Client();

                PutObjectRequest request = new()
                {
                    BucketName = BucketName,
                    Key = uploadFileName,
                    StorageClass = S3StorageClass.Standard,
                    InputStream = stream
                };

                PutObjectResponse response = await client.PutObjectAsync(request);

                return !response.IsSuccessStatusCode() 
                    ? UploadFileResult.Error() 
                    : UploadFileResult.Ok(uploadFileName);
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
                AmazonS3Client client = await GetS3Client();

                GetObjectRequest request = new()
                {
                    BucketName = BucketName,
                    Key = id
                };

                GetObjectResponse response = await client.GetObjectAsync(request);

                return response.ResponseStream;
            }
            catch (Exception e)
            {
                return Stream.Null;
            }
        }

        private async Task<AmazonS3Client> GetS3Client()
        {
            AmazonS3Client client = new(AccessKeyId, AccessSecret, RegionEndpoint.EUCentral1);
            
            ListBucketsResponse response = await client.ListBucketsAsync();

            if (!response.Buckets.Any(b => b.BucketName.Equals(BucketName)))
            {
                PutBucketRequest request = new()
                {
                    BucketName = BucketName,
                    UseClientRegion = true
                };

                await client.PutBucketAsync(request);
            }

            return client;
        }
    }
}