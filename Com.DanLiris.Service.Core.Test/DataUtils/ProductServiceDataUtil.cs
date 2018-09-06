using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.DanLiris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Test.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Test.DataUtils
{
    public class ProductServiceDataUtil : BasicDataUtil<CoreDbContext, ProductService, Product>, IEmptyData<ProductViewModel>
    {

        public ProductServiceDataUtil(CoreDbContext dbContext, ProductService service) : base(dbContext, service)
        {
        }        

        public ProductViewModel GetEmptyData()
        {
            ProductViewModel Data = new ProductViewModel();

            Data.Name = "";
            Data.Code = "";
            return Data;
        }

        public override Product GetNewData()
        {
            string guid = Guid.NewGuid().ToString();
            Product TestData = new Product
            {
                Name = "TEST",
                Code = guid
            };

            return TestData;
        }

        public override async Task<Product> GetTestDataAsync()
        {
            Product Data = GetNewData();
            await this.Service.CreateModel(Data);
            return Data;
        }
    }
}