using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CloudDataProtection.Functions.BackupDemo.Settings.App
{
    public class AppSettingsProvider
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings = new()
        {
            MissingMemberHandling = MissingMemberHandling.Error
        };

        private static AppSettingsProvider _instance;
        public static AppSettingsProvider Instance => _instance ??= new AppSettingsProvider();

        private AppSettings _settings;
        public AppSettings Settings => _settings ??= ReadAppSettings();

        private AppSettingsSource _source;
        private string _path;

        private AppSettingsProvider()
        {
            _source = AppSettingsSource.EmbeddedResource;
        }

        public void Init(AppSettingsSource source, string path)
        {
            _source = source;
            _path = path;
            _settings = null;
        }

        private AppSettings ReadAppSettings()
        {
            if (_path == null)
            {
                throw new Exception("AppSettingsProvider is not initialized, please call AppSettingsProvider.Init() method");
            }

            switch (_source)
            {
                case AppSettingsSource.File:
                    return GetSettingsFromFile(_path);
                case AppSettingsSource.EmbeddedResource:
                    return GetSettingsFromEmbeddedResource(_path);
                default:
                    throw new Exception($"Unknown app settings source {_source}");
            }
        }

        private AppSettings GetSettingsFromFile(string path)
        {
            string content = File.ReadAllText(path);

            return Deserialize(content) ?? throw new Exception($"Could not deserialize app settings from {path}");
        }

        private AppSettings GetSettingsFromEmbeddedResource(string path)
        {
            string content = GetEmbeddedResourceContent(path);

            return Deserialize(content) ?? throw new Exception($"Could not deserialize app settings from embedded resource {path}");
        }

        private string GetEmbeddedResourceContent(string path)
        {
            string[] resourceNames = GetType().Assembly.GetManifestResourceNames();

            string identifier = resourceNames.FirstOrDefault(resourceName => resourceName.EndsWith(path));

            if (identifier == null)
            {
                throw new Exception($"Could not find embedded resource {path}.");
            }

            using (Stream stream = GetType().Assembly.GetManifestResourceStream(identifier))
            {
                using (StreamReader reader = new(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private AppSettings Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<AppSettings>(content, _jsonSerializerSettings);
        }
    }
}