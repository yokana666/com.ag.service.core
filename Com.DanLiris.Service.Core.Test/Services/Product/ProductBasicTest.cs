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
            model.ProductionOrderNo = string.Empty;
        }

        public override void EmptyUpdateModel(Models.Product model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
            model.ProductionOrderNo = string.Empty;
        }

        public override Models.Product GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new Models.Product()
            {
                Code = guid,
                Name = string.Format("TEST {0}", guid),
                ProductionOrderNo = "ProductionOrderNo",
            };
        }
    }
}
