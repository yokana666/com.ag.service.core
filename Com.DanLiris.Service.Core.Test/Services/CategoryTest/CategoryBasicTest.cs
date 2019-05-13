using Com.Danliris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Services.CategoryTest
{
    [Collection("ServiceProviderFixture Collection")]
    public class CategoryBasicTest : BasicServiceTest<CoreDbContext, CategoryService, Category>
    {
        private static readonly string[] createAttrAssertions = { "Name", "Code" };
        private static readonly string[] updateAttrAssertions = { "Name", "Code" };
        private static readonly string[] existAttrCriteria = { "Code" };

        public CategoryBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Category model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
        }

        public override void EmptyUpdateModel(Category model)
        {
            model.Code = string.Empty;
            model.Name = string.Empty;
        }

        public override Category GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new Category
            {
                Code = string.Format("CategoryCode {0}", guid),
                Name = string.Format("CategoryName {0}", guid),
            };
        }
    }
}
