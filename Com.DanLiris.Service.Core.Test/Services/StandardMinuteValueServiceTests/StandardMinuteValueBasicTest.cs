using Com.Danliris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Services;
using System;
using Xunit;
using Models = Com.DanLiris.Service.Core.Lib.Models;

namespace Com.DanLiris.Service.Core.Test.Services.StandardMinuteValueServiceTests
{
    [Collection("ServiceProviderFixture Collection")]
    public class StandardMinuteValueBasicTest : BasicServiceTest<CoreDbContext, StandardMinuteValueService, Models.StandardMinuteValue>
    {
        private static readonly string[] createAttrAssertions = { "BuyerName", "ComodityName", "MinuteCutting", "MinuteSewing", "MinuteFinishing" };
        private static readonly string[] updateAttrAssertions = { "BuyerName", "ComodityName", "MinuteCutting", "MinuteSewing", "MinuteFinishing" };
        private static readonly string[] existAttrCriteria = { };

        public StandardMinuteValueBasicTest(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
        {
        }

        public override void EmptyCreateModel(Models.StandardMinuteValue model)
        {
            model.BuyerName = string.Empty;
            model.ComodityName = string.Empty;
            model.MinuteCutting = 0;
            model.MinuteFinishing = 0;
            model.MinuteSewing = 0;
        }

        public override void EmptyUpdateModel(Models.StandardMinuteValue model)
        {
            model.BuyerName = string.Empty;
            model.ComodityName = string.Empty;
            model.MinuteCutting = 0;
            model.MinuteFinishing = 0;
            model.MinuteSewing = 0;
        }

        public override Models.StandardMinuteValue GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new Models.StandardMinuteValue()
            {
                BuyerId = 1,
                BuyerCode = string.Format("TEST {0}", guid),
                BuyerName = string.Format("TEST {0}", guid),
                ComodityId = 1,
                ComodityCode = string.Format("TEST {0}", guid),
                ComodityName = string.Format("TEST {0}", guid),
                MinuteCutting = 1,
                MinuteFinishing = 1,
                MinuteSewing = 2,
            };
        }
    }
}
