using Com.DanLiris.Service.Core.Lib.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Controllers.IncomeTax
{
    [Collection("TestFixture Collection")]
    class IncomeTaxBasicTest
    {
        private const string URI = "v1/master/income-taxes";

        protected TestServerFixture TestFixture { get; set; }

        protected HttpClient Client
        {
            get { return this.TestFixture.Client; }
        }

        public IncomeTaxBasicTest(TestServerFixture fixture)
        {
            TestFixture = fixture;

        }

        public IncomeTaxViewModel GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new IncomeTaxViewModel()
            {
                rate = 5,
                name = string.Format("TEST IncomeTax {0}", guid),
                description="test"
            };
        }

        [Fact]
        public async Task Get()
        {
            var response = await this.Client.GetAsync(URI);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetById()
        {
            var response = await this.Client.GetAsync(string.Concat(URI, "/"));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post()
        {

            IncomeTaxViewModel incomeTaxViewModel = GenerateTestModel();
            var response = await this.Client.PostAsync(URI, new StringContent(JsonConvert.SerializeObject(incomeTaxViewModel).ToString(), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
