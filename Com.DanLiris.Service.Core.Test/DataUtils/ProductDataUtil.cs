using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Test.DataUtils
{
    public class ProductDataUtil
    {
        public CoreDbContext DbContext { get; set; }
        public ProductService ProductService { get; set; }

        public ProductDataUtil(CoreDbContext dbContext, ProductService productService)
        {
            this.DbContext = dbContext;
            this.ProductService = productService;
        }
        public Task<Product> GetTestBuget()
        {
            Product test = ProductService.DbSet.FirstOrDefault(product => product.Code.Equals("Test"));

            if (test != null)
                return Task.FromResult(test);
            else
            {
                string guid = Guid.NewGuid().ToString();
                test = new Product()
                {
                    Code = guid,
                    Name = string.Format("Test {0}", guid),
                };

                int id = ProductService.Create(test);
                return ProductService.GetAsync(id);
            }
        }
    }
}
