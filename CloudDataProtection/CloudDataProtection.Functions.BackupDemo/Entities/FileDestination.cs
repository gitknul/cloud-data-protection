using System.ComponentModel;

namespace CloudDataProtection.Functions.BackupDemo.Entities
{
    public enum FileDestination
    {
        [Description("Azure Blob Storage")]
        AzureBlobStorage = 0,
        
        [Description("Amazon S3")]
        AmazonS3 = 1,
        
        [Description("Google Cloud Storage")]
        GoogleCloudStorage = 2
    }
}