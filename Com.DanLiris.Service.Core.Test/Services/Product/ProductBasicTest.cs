using Com.Danliris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Models = Com.DanLiris.Service.Core.Lib.Models;

namespace Com.DanLiris.Service.Core.Test.Services.StandardTest
{
    [Collection("ServiceProviderFixture Collection")]
    public class ProductBasicTest : BasicServiceTest<CoreDbContext, ProductService, Models.Product>
    {
        private static readonly string[] createAttrAssertions = { "Name" };
        private static readonly string[] updateAttrAssertions = { "Name" };
        private static readonly string[] existAttrCriteria = { "Name" };

        public ProductBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }
        public override void EmptyCreateModel(Models.Product model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
            model.BuyerAddress = string.Empty;
            model.ColorName = string.Empty;
            model.BuyerName = string.Empty;
            model.Construction = string.Empty;
            model.CurrencyCode = string.Empty;
            model.CurrencySymbol = string.Empty;
            model.Description = string.Empty;
            model.DesignCode = string.Empty;
            model.DesignNumber = string.Empty;
            model.Grade = string.Empty;
            model.OrderTypeCode = string.Empty;
            model.OrderTypeName = string.Empty;
            model.Tags = string.Empty;
            model.UomUnit = string.Empty;
            model.Lot = string.Empty;
            model.Weight = 0;
            model.Length = 0;
            model.Price = 0;
            model.BuyerId = 0;
            model.CurrencyId = 0;
            model.OrderTypeId = 0;
            model.ProductionOrderId = 0;
            model.UomId = 0;
        }

        public override void EmptyUpdateModel(Models.Product model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
            model.BuyerAddress = string.Empty;
            model.ColorName = string.Empty;
            model.BuyerName = string.Empty;
            model.Construction = string.Empty;
            model.CurrencyCode = string.Empty;
            model.CurrencySymbol = string.Empty;
            model.Description = string.Empty;
            model.DesignCode = string.Empty;
            model.DesignNumber = string.Empty;
            model.Grade = string.Empty;
            model.OrderTypeCode = string.Empty;
            model.OrderTypeName = string.Empty;
            model.Tags = string.Empty;
            model.UomUnit = string.Empty;
            model.Lot = string.Empty;
            model.Weight = 0;
            model.Length = 0;
            model.Price = 0;
            model.BuyerId = 0;
            model.CurrencyId = 0;
            model.OrderTypeId = 0;
            model.ProductionOrderId = 0;
            model.UomId = 0;
        }

        public override Models.Product GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new Models.Product()
            {
                Code = guid,
                Name = string.Format("TEST {0}", guid),
                ProductionOrderNo = "ProductionOrderNo",
                CurrencySymbol = "curr",
                Active = true,
                BuyerAddress = "buyerAddress",
                BuyerName = "bname",
                BuyerId = 1,
                ColorName = "color",
                Construction = "cons",
                CurrencyCode = "currcode",
                CurrencyId = 1,
                Description = "desc",
                DesignCode = "dcode",
                DesignNumber = "dnum",
                Grade = "grade",
                Length = 1,
                Lot = "lot",
                OrderTypeCode = "otc",
                OrderTypeId = 1,
                OrderTypeName = "otn",
                Price = 12,
                ProductionOrderId = 1,
                Tags = "tags",
                UomId = 1,
                UomUnit = "uom",
                Weight = 1,
                UId = guid
            };
        }
    }
}
