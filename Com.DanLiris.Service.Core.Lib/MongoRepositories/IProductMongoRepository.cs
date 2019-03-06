using Com.DanLiris.Service.Core.Lib.MongoModels;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Lib.MongoRepositories
{
    public interface IProductMongoRepository
    {
        Task<IEnumerable<MongoProductModel>> GetByQuery(FilterDefinition<MongoProductModel> filter);
    }
}