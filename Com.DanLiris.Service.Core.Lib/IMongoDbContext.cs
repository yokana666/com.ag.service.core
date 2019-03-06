using Com.DanLiris.Service.Core.Lib.MongoModels;
using MongoDB.Driver;

namespace Com.DanLiris.Service.Core.Lib
{
    public interface IMongoDbContext
    {
        IMongoCollection<MongoProductModel> Products { get; }
    }
}