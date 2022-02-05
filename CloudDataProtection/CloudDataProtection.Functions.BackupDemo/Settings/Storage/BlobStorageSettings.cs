using Newtonsoft.Json;

namespace CloudDataProtection.Functions.BackupDemo.Settings.Storage
{
    public class BlobStorageSettings
    {
        [JsonProperty(PropertyName = "Enabled", Required = Required.Always)]
        public bool IsEnabled { get; set; }
    }
}