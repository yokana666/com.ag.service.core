using Com.DanLiris.Service.Core.Lib.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Controllers.AccountBankControllerTests
{
    [Collection("TestFixture Collection")]
    public class Basic
    {
        private const string URI = "v1/master/account-banks";

        protected TestServerFixture TestFixture { get; set; }

        protected HttpClient Client
        {
            get { return this.TestFixture.Client; }
        }

        public Basic(TestServerFixture fixture)
        {
            TestFixture = fixture;

        }

        public AccountBankViewModel GenerateTestModel()
        {
            string guid = Guid.NewGuid().ToString();

            return new AccountBankViewModel()
            {
                Code = guid,
                BankName = string.Format("TEST Bank {0}", guid),
                BankAddress = string.Format("TEST Bank {0}", guid),
                AccountName = string.Format("TEST Bank {0}", guid),
                AccountNumber = string.Format("TEST Bank {0}", guid),
                Fax = string.Format("TEST Bank {0}", guid),
                Phone = string.Format("TEST Bank {0}", guid),
                SwiftCode = string.Format("TEST Bank {0}", guid),
                Division= new DivisionViewModel
                {
                    Name = string.Format("TEST Bank {0}", guid),
                    Code= string.Format("TEST Bank {0}", guid),
                    Id=1
                },
                Currency= new CurrencyViewModel
                {
                    Code = string.Format("TEST Bank {0}", guid),
                    Id=1,
                    Description = string.Format("TEST Bank {0}", guid),
                    Rate=1,
                    Symbol = string.Format("TEST Bank {0}", guid),
                },
                
            };
        }

        //[Fact]
        //public async Task Get()
        //{
        //    var response = await this.Client.GetAsync(URI);
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        //[Fact]
        //public async Task GetById()
        //{
        //    var response = await this.Client.GetAsync(string.Concat(URI, "/"));
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        [Fact]
        public async Task Post()
        {

            AccountBankViewModel budgetVM = GenerateTestModel();
            var response = await this.Client.PostAsync(URI, new StringContent(JsonConvert.SerializeObject(budgetVM).ToString(), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
