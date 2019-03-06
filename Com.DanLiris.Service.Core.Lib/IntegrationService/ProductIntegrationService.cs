using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.MongoModels;
using Com.DanLiris.Service.Core.Lib.MongoRepositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Lib.IntegrationService
{
    public class ProductIntegrationService : IProductIntegrationService
    {
        private readonly CoreDbContext _dbContext;
        private readonly DbSet<Product> _productDbSet;
        private readonly DbSet<Uom> _uomDbSet;
        private readonly DbSet<Currency> _currencyDbSet;
        private readonly IProductMongoRepository _mongoRepository;

        public ProductIntegrationService(CoreDbContext dbContext, IProductMongoRepository mongoRepository)
        {
            _dbContext = dbContext;
            _productDbSet = dbContext.Set<Product>();
            _uomDbSet = dbContext.Set<Uom>();
            _currencyDbSet = dbContext.Set<Currency>();
            _mongoRepository = mongoRepository;
        }

        public async Task<int> IntegrateData()
        {
            var existingUids = _productDbSet.IgnoreQueryFilters().Select(s => new ObjectId(s.UId)).ToList();

            var filter = Builders<MongoProductModel>.Filter.Nin(_ => _._id, existingUids);

            var newProducts = await _mongoRepository.GetByQuery(filter);

            var productsToInsert = new List<Product>();
            foreach (var newProduct in newProducts)
            {
                var uom = _uomDbSet.FirstOrDefault(f => f.Unit.Equals(newProduct.uom.unit));
                var currency = _currencyDbSet.FirstOrDefault(f => f.Symbol.Equals(newProduct.currency.symbol));

                var transformedProduct = new Product(newProduct, uom, currency);

                productsToInsert.Add(transformedProduct);
            }

            _productDbSet.AddRange(productsToInsert);

            return await _dbContext.SaveChangesAsync();
        }
    }
}
