using Com.Danliris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Models = Com.DanLiris.Service.Core.Lib.Models;

namespace Com.DanLiris.Service.Core.Test.Services.SizeTests
{
    [Collection("ServiceProviderFixture Collection")]
    public class SizeBasicTest : BasicServiceTest<CoreDbContext, SizeService, Models.SizeModel>
    {
        private static readonly string[] createAttrAssertions = { "Name" };
        private static readonly string[] updateAttrAssertions = { "Name" };
        private static readonly string[] existAttrCriteria = { "Name" };

        public SizeBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }
        public override void EmptyCreateModel(Models.SizeModel model)
        {
            model.Size = string.Empty;
        }

        public override void EmptyUpdateModel(Models.SizeModel model)
        {
            model.Size = string.Empty;
        }

        public override Models.SizeModel GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new Models.SizeModel()
            {
                Size = "S",
            };
        }
    }
}