using Com.Danliris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.DanLiris.Service.Core.Test.Services.GarmentSupplierTest
{
	[Collection("ServiceProviderFixture Collection")]
	public class BasicTests : BasicServiceTest<CoreDbContext, GarmentSupplierService, GarmentSupplier>
	{
		private static readonly string[] createAttrAssertions = { "Name" };
		private static readonly string[] updateAttrAssertions = { "Name" };
		private static readonly string[] existAttrCriteria = { "Code" };
		public BasicTests(ServiceProviderFixture fixture) : base(fixture, createAttrAssertions, updateAttrAssertions, existAttrCriteria)
		{
		}
		public override void EmptyCreateModel(GarmentSupplier model)
		{
			model.Code = string.Empty;
			model.Name = string.Empty;
			model.IncomeTaxesName = string.Empty;
		}

		public override void EmptyUpdateModel(GarmentSupplier model)
		{
			model.Code = string.Empty;
			model.Name = string.Empty;
			model.IncomeTaxesName = string.Empty;

		}
		public override GarmentSupplier GenerateTestModel()
		{
			string guid = Guid.NewGuid().ToString();

			return new GarmentSupplier()
			{
				Code = guid,
				Name = string.Format("TEST {0}", guid),
				IncomeTaxesName = string.Format("TEST {0}", guid),
			};
		}
	}
}
