using System;
using System.Collections.Generic;
using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Functions.BackupDemo.Business;
using CloudDataProtection.Functions.BackupDemo.Context;
using CloudDataProtection.Functions.BackupDemo.Extensions;
using CloudDataProtection.Functions.BackupDemo.Repository;
using CloudDataProtection.Functions.BackupDemo.Service;
using CloudDataProtection.Functions.BackupDemo.Service.Amazon;
using CloudDataProtection.Functions.BackupDemo.Service.Azure;
using CloudDataProtection.Functions.BackupDemo.Service.Google;
using CloudDataProtection.Functions.BackupDemo.Settings.App;
using Environment = System.Environment;

namespace CloudDataProtection.Functions.BackupDemo.Factory
{
    public class FileManagerLogicFactory
    {
        private static FileManagerLogicFactory _instance;

        public static FileManagerLogicFactory Instance => _instance ??= new FileManagerLogicFactory();

        private readonly string _environment;

        private AppSettings _appSettings;

        private FileManagerLogicFactory()
        {
            _environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

            if (_environment == null)
            {
                throw new Exception("Could not determine hosting environment because AZURE_FUNCTIONS_ENVIRONMENT is not set");
            }
        }

        public FileManagerLogic GetLogic()
        {
            InitializeAppSettingsProvider();

            AesOptions options = new()
            {
                EncryptionKey = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_BLOB_AES_KEY"),
                EncryptionIv = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_BLOB_AES_IV")
            };

            MongoDbOptions mongoDbOptions = new()
            {
                ConnectionString = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_MONGODB_CONNECTION"),
                Database = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_MONGODB_DATABASE")
            };

            IFileContext context = new MongoDbFileContext(mongoDbOptions);
            IFileRepository repository = new FileRepository(context);

            ICollection<IFileService> fileServices = GetFileServices(_appSettings);

            IDataTransformer transformer = new AesStreamTransformer(options);

            return new FileManagerLogic(fileServices, transformer, repository);
        }

        private void InitializeAppSettingsProvider()
        {
            AppSettingsSource appSettingsSource = GetAppSettingsSource();

            string appSettingsPath;
            
            switch (appSettingsSource)
            {
                case AppSettingsSource.File:
                    appSettingsPath = GetAppSettingsFilepath();
                    break;
                case AppSettingsSource.EmbeddedResource:
                    appSettingsPath = GetEmbeddedResourceFilepath();
                    break;
                default:
                    throw new Exception($"Unknown app settings source {appSettingsSource}");
            }
            
            AppSettingsProvider.Instance.Init(appSettingsSource, appSettingsPath);

            _appSettings = AppSettingsProvider.Instance.Settings;
        }

        private ICollection<IFileService> GetFileServices(AppSettings settings)
        {
            ICollection<IFileService> fileServices = new List<IFileService>();

            fileServices.AddIf(() => new BlobStorageFileService(), settings.BlobStorage.IsEnabled);
            fileServices.AddIf(() => new S3FileService(), settings.S3.IsEnabled);
            fileServices.AddIf(() => new GoogleCloudStorageFileService(), settings.Gcs.IsEnabled);

            return fileServices;
        }
        
        private AppSettingsSource GetAppSettingsSource()
        {
            return string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CDP_BACKUP_DEMO_SETTINGS_FILE"))
                ? AppSettingsSource.EmbeddedResource
                : AppSettingsSource.File;
        }

        private string GetEmbeddedResourceFilepath()
        {
            return $"appsettings.{_environment}.json";
        }

        private string GetAppSettingsFilepath()
        {
            return EnvironmentVariableHelper.GetEnvironmentVariable("CDP_BACKUP_DEMO_SETTINGS_FILE");
        }
    }
}