using Newtonsoft.Json;

namespace CloudDataProtection.Functions.BackupDemo.Settings.Storage
{
    public class GoogleCloudStorageSettings
    {
        [JsonProperty(PropertyName = "Enabled", Required = Required.Always)]
        public bool IsEnabled { get; set; }
    }
}