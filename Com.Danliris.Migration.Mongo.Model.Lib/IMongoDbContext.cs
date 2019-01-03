using Com.Danliris.Migration.Mongo.Model.Lib.MongoModels;
using MongoDB.Driver;

namespace Com.Danliris.Migration.Mongo.Model.Lib
{
    public interface IMongoDbContext
    {
        IMongoCollection<UnitOfMeasurement> UnitOfMeasurements { get; }
    }
}
