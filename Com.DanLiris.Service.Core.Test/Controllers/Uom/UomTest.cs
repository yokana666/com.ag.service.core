using Com.DanLiris.Service.Core.Lib.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Controllers.Uom
{
    [Collection("TestFixture Collection")]
    public class UomTest
    {
        private const string URI = "v1/master/uoms";
        protected TestServerFixture TestFixture { get; set; }

        protected HttpClient Client
        {
            get { return this.TestFixture.Client; }
        }

        public UomTest(TestServerFixture fixture)
        {
            TestFixture = fixture;
        }

        public UomViewModel GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new UomViewModel()
            {
                Unit = "uom",
            };
        }

        [Fact]
        public async Task GetSimple()
        {
            var response = await this.Client.GetAsync(string.Concat(URI, "/simple"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
