using Com.Danliris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Services;
using System;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Services.Product
{
    [Collection("ServiceProviderFixture Collection")]
    public class ProductBasicTest : BasicServiceTest<CoreDbContext, ProductService, Lib.Models.Product>
    {
        private static readonly string[] createAttrAssertions = { "Name" };
        private static readonly string[] updateAttrAssertions = { "Name" };
        private static readonly string[] existAttrCriteria = { "Code" };
        public ProductBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Lib.Models.Product model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
        }

        public override void EmptyUpdateModel(Lib.Models.Product model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
        }

        public override Lib.Models.Product GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new Lib.Models.Product()
            {
                Code = guid,
                Name = string.Format("TEST {0}", guid),
            };
        }
    }
}
