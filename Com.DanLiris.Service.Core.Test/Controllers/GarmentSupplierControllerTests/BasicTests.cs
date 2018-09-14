using Com.DanLiris.Service.Core.Lib.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Controllers.GarmentSupplierControllerTests
{
    [Collection("TestFixture Collection")]
    public class BasicTests
	{
		private const string URI = "v1/master/garment-suppliers";

		protected TestServerFixture TestFixture { get; set; }

		protected HttpClient Client
		{
			get { return this.TestFixture.Client; }
		}

		public BasicTests(TestServerFixture fixture)
		{
			TestFixture = fixture;

		}

		public GarmentSupplierViewModel GenerateTestModel()
		{
			string guid = Guid.NewGuid().ToString();

			return new GarmentSupplierViewModel()
			{
				name = String.Concat("TEST G-Supplier ", guid),
				code = "TEST CODE",
				address = string.Empty,
				import = true,
				NPWP = "NPWP-TEST",
				usevat = true
			};
		}

		[Fact]
		public async Task Post()
		{
			GarmentSupplierViewModel garmentSupplierViewModel = GenerateTestModel();
			var response = await this.Client.PostAsync(URI, new StringContent(JsonConvert.SerializeObject(garmentSupplierViewModel).ToString(), Encoding.UTF8, "application/json"));

			Assert.Equal(HttpStatusCode.Created, response.StatusCode);
		}

	}
}
