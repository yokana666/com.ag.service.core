using Com.DanLiris.Service.Core.Lib.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Controllers.GarmentUnitControllerTests
{

	[Collection("TestFixture Collection")]
	public class BasicTests
	{
		private const string URI = "v1//master/garment-units";

		protected TestServerFixture TestFixture { get; set; }

		protected HttpClient Client
		{
			get { return this.TestFixture.Client; }
		}

		public BasicTests(TestServerFixture fixture)
		{
			TestFixture = fixture;

		}

		public  UnitViewModel GenerateTestModel()
		{
			string guid = Guid.NewGuid().ToString();

			return new UnitViewModel()
			{
				Name = String.Concat("TEST G-Unit ", guid),
				Code = "TEST CODE",
				Description="DESC"
				 
			};
		}

		[Fact]
		public async Task Post()
		{
			UnitViewModel unitViewModel = GenerateTestModel();
			var response = await this.Client.PostAsync(URI, new StringContent(JsonConvert.SerializeObject(unitViewModel).ToString(), Encoding.UTF8, "application/json"));

			Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		}

	}
}
