using Com.DanLiris.Service.Core.Lib.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Controllers.Product
{
    [Collection("TestFixture Collection")]
    public class ProductBasicTest
    {
        private const string URI = "v1/master/products";

        protected TestServerFixture TestFixture { get; set; }

        protected HttpClient Client
        {
            get { return this.TestFixture.Client; }
        }

        public ProductBasicTest(TestServerFixture fixture)
        {
            TestFixture = fixture;

        }

        public ProductViewModel GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new ProductViewModel()
            {
                Code = guid,
                Name = string.Format("TEST {0}", guid),
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

            ProductViewModel VM = GenerateTestModel();
            var response = await this.Client.PostAsync(URI, new StringContent(JsonConvert.SerializeObject(VM).ToString(), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
