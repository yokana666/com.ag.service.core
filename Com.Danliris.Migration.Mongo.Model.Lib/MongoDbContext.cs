using Com.Danliris.Migration.Mongo.Model.Lib.MongoModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Com.Danliris.Migration.Mongo.Model.Lib
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _db;

        public MongoDbContext(IOptions<MongoDbSettings> options, IMongoClient client)
        {
            _db = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<UnitOfMeasurement> UnitOfMeasurements => _db.GetCollection<UnitOfMeasurement>("unit-of-measurements");
    }
}
