using CloudDataProtection.Core.Cryptography.Aes;
using CloudDataProtection.Core.Cryptography.Aes.Options;
using CloudDataProtection.Core.Environment;
using CloudDataProtection.Functions.BackupDemo.Business;
using CloudDataProtection.Functions.BackupDemo.Context;
using CloudDataProtection.Functions.BackupDemo.Repository;
using CloudDataProtection.Functions.BackupDemo.Service;
using CloudDataProtection.Functions.BackupDemo.Service.Amazon;
using CloudDataProtection.Functions.BackupDemo.Service.Azure;
using CloudDataProtection.Functions.BackupDemo.Service.Google;

namespace CloudDataProtection.Functions.BackupDemo.Factory
{
    public class FileManagerLogicFactory
    {
        private static FileManagerLogicFactory _instance;

        public static FileManagerLogicFactory Instance => _instance ??= new FileManagerLogicFactory();

        public FileManagerLogic GetLogic()
        {
            AesOptions options = new AesOptions
            {
                KeySize = 256,
                BlockSize = 128,
                EncryptionKey = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_AES_KEY"),
                EncryptionIv = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_BLOB_AES_IV")
            };

            IDataTransformer transformer = new AesStreamTransformer(options);

            MongoDbOptions mongoDbOptions = new MongoDbOptions
            {
                ConnectionString = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_MONGO"),
                Database = EnvironmentVariableHelper.GetEnvironmentVariable("CDP_DEMO_MONGO_DB"),
            };

            IFileContext context = new MongoDbFileContext(mongoDbOptions);
            
            IBlobStorageFileService blobStorageFileService = new BlobStorageFileService();
            IS3FileService s3FileService = new S3FileService();
            IGoogleCloudStorageFileService googleCloudStorageFileService = new GoogleCloudStorageFileService();
            IFileRepository repository = new FileRepository(context);

            return new FileManagerLogic
                (blobStorageFileService, s3FileService, googleCloudStorageFileService, transformer, repository);
        }
    }
}