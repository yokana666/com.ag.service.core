using Com.DanLiris.Service.Core.Lib.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Controllers.ProcessType
{
    [Collection("TestFixture Collection")]
    public class ProcessTypeBasicTest
    {
        private const string URI = "v1/master/process-types";
        protected TestServerFixture TestFixture { get; set; }
        protected HttpClient Client
        {
            get { return this.TestFixture.Client; }
        }
        public ProcessTypeBasicTest(TestServerFixture fixture)
        {
            TestFixture = fixture;

        }

        public ProcessTypeViewModel GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new ProcessTypeViewModel()
            {
                Code = guid,
                Name = string.Format("TEST {0}", guid),
                Remark = "remark",
                OrderType = new OrderTypeViewModel
                {
                    Code = guid + "Order",
                    Name = string.Format("TEST Order {0}", guid),
                    Remark = "remark",
                }
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

            ProcessTypeViewModel VM = GenerateTestModel();
            var response = await this.Client.PostAsync(URI, new StringContent(JsonConvert.SerializeObject(VM).ToString(), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

    }
}
