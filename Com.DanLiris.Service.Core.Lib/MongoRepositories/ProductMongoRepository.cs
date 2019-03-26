using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Com.DanLiris.Service.Core.Lib.MongoModels;
using MongoDB.Driver;

namespace Com.DanLiris.Service.Core.Lib.MongoRepositories
{
    public class ProductMongoRepository : IProductMongoRepository
    {
        private IMongoDbContext _context;

        public ProductMongoRepository(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MongoProductModel>> GetByQuery(FilterDefinition<MongoProductModel> filter)
        {
            return await _context
                           .Products
                           .Find(filter)
                           .ToListAsync();
        }
    }
}
