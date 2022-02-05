using CloudDataProtection.Functions.BackupDemo.Settings.Storage;
using Newtonsoft.Json;

namespace CloudDataProtection.Functions.BackupDemo.Settings.App
{
    public class AppSettings
    {
        [JsonProperty("S3", Required = Required.Always)]
        public S3Settings S3 { get; set; }
        
        [JsonProperty("BlobStorage", Required = Required.Always)]
        public BlobStorageSettings BlobStorage { get; set; }
        
        [JsonProperty("GCS", Required = Required.Always)]
        public GoogleCloudStorageSettings Gcs { get; set; }
    }
}