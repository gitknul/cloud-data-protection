using System;
using System.Threading.Tasks;
using CloudDataProtection.Functions.BackupDemo.Entities;
using MongoDB.Driver;

namespace CloudDataProtection.Functions.BackupDemo.Context
{
    public class MongoDbFileContext : IFileContext
    {
        private readonly MongoDbOptions _options;

        private MongoClient _client;
        private MongoClient Client
        {
            get
            {
                if (_client == null)
                {
                    MongoClientSettings settings = MongoClientSettings.FromConnectionString(_options.ConnectionString);

                    _client = new MongoClient(settings);
                }
                
                return _client;
            }
        }

        private IMongoDatabase _database;
        private IMongoDatabase Database => _database ??= Client.GetDatabase(_options.Database);

        private IMongoCollection<File> _collection;
        private IMongoCollection<File> Collection => _collection ??= Database.GetCollection<File>("File", Settings);

        private MongoCollectionSettings Settings => new MongoCollectionSettings
        {
            AssignIdOnInsert = false
        };

        public MongoDbFileContext(MongoDbOptions options)
        {
            _options = options;
        }
        
        public async Task<File> Get(Guid id)
        {
            FilterDefinition<File> filter = Builders<File>.Filter.Eq("Id", id.ToString());

            IAsyncCursor<File> cursor = await Collection.FindAsync(filter);

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<File> Create(File file)
        {
            await Collection.InsertOneAsync(file);

            return file;
        }
    }
}