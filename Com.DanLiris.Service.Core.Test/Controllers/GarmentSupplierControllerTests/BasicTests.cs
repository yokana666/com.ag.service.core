using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.DanLiris.Service.Core.Test.Helpers;
using Models = Com.DanLiris.Service.Core.Lib.Models;
using Xunit;
using Com.DanLiris.Service.Core.Test.DataUtils;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Text;

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
				code = guid,
				address = string.Empty,
				import = true,
				NPWP = "NPWP-TEST",
				usevat = true,
				usetax = true,
				IncomeTaxes = new IncomeTaxViewModel
				{
					_id = 1,
					name = "TEST NAME",
					rate = 1.5,
				},
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

