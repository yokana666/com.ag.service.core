using System;
using Com.DanLiris.Service.Core.Lib.MongoModels;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Com.DanLiris.Service.Core.Lib
{
    public class MongoDbContext : IMongoDbContext
    {
        private const string _productsCollection = "products";
        private readonly IMongoDatabase _db;

        public MongoDbContext(IOptions<MongoDbSettings> options, IMongoClient client)
        {
            _db = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<MongoProductModel> Products => _db.GetCollection<MongoProductModel>(_productsCollection);
    }
}
