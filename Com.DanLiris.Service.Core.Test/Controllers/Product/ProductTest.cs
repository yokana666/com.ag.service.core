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
    public class ProductTest
    {
        private const string URI = "v1/master/products";

        protected TestServerFixture TestFixture { get; set; }

        protected HttpClient Client
        {
            get { return this.TestFixture.Client; }
        }

        public ProductTest(TestServerFixture fixture)
        {
            TestFixture = fixture;
        }

        public ProductViewModel GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new ProductViewModel()
            {
                Name = string.Format("TEST {0}", guid),
                Code = "Code",
                Active = true,
                Description = "desc",
                Price = 12,
                Tags = "tags",
                UOM = new ProductUomViewModel { Unit = "unit", Id = 1 },
                Currency = new ProductCurrencyViewModel { Symbol = "rp", Code = "idr", Id = 1 }
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

            ProductViewModel productViewModel = GenerateTestModel();
            var response = await this.Client.PostAsync(URI, new StringContent(JsonConvert.SerializeObject(productViewModel).ToString(), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

    }
}
