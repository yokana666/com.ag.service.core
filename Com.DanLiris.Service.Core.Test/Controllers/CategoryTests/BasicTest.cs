using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.DanLiris.Service.Core.Test.DataUtils;
using Com.DanLiris.Service.Core.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Controllers.CategoryTests
{
    [Collection("TestFixture Collection")]
    public class BasicTest : BasicControllerTestOldVM<CoreDbContext, CategoryService, Category, CategoryViewModel, CategoryDataUtil>
    {
        private const string URI = "v1/master/categories";

        private static List<string> CreateValidationAttributes = new List<string> { "Code", "Name" };
        private static List<string> UpdateValidationAttributes = new List<string> { "Code", "Name" };

        public BasicTest(TestServerFixture fixture) : base(fixture, URI, CreateValidationAttributes, UpdateValidationAttributes)
        {
        }
    }
}
