using Com.DanLiris.Service.Core.Lib.ViewModels;
using System;
using Newtonsoft.Json;
using System.Net;
using Models = Com.DanLiris.Service.Core.Lib.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Com.DanLiris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Test.DataUtils;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Core.Test.Controllers.GarmentCurrency
{
	[Collection("TestFixture Collection")]
	public class BasicTests 
	{
		private const string URI = "v1/master/garment-currencies";
		protected TestServerFixture TestFixture { get; set; }

		protected HttpClient Client
		{
			get { return this.TestFixture.Client; }
		}

		public BasicTests(TestServerFixture fixture)
		{
			TestFixture = fixture;
		}

		public GarmentCurrencyViewModel GenerateTestModel()
		{
			string guid = Guid.NewGuid().ToString();

			return new GarmentCurrencyViewModel()
			{
				code = guid,
				date = DateTime.Now,
				rate = guid,
			};
		}

		[Fact]
		public async Task Should_Success_GetById()
		{
			var response = await this.Client.GetAsync(string.Concat(URI, "/"));
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}



	}
}
