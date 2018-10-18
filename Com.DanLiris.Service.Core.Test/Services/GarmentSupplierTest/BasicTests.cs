using Com.Danliris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Test.DataUtils;
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
            string guid = Guid.NewGuid().ToString();

            model.Code = guid;
			model.Name = "TEST";
			model.UseTax = true;
            model.IncomeTaxesId = 1;
			model.IncomeTaxesName = "TEST";
            model.IncomeTaxesRate = null;
            model.IncomeTaxesRate = -1;
            model.IncomeTaxesRate = 0;
		}

		public override void EmptyUpdateModel(GarmentSupplier model)
		{
            string guid = Guid.NewGuid().ToString();
            model.Code = guid;
			model.Name = "TEST";
			model.UseTax = true;
            model.IncomeTaxesId = 1;
			model.IncomeTaxesName = "TEST";
            model.IncomeTaxesRate = null;
            model.IncomeTaxesRate = -1;
            model.IncomeTaxesRate = 0;
        }
		public override GarmentSupplier GenerateTestModel()
		{
			string guid = Guid.NewGuid().ToString();

			return new GarmentSupplier()
			{
				Code = guid,
				Name = guid,
				UseTax = true,
				UseVat = true,
				Import = true,
				IncomeTaxesId = 1,
				IncomeTaxesName = guid,
				IncomeTaxesRate = 1,
			};
		}

        private GarmentSupplierDataUtil DataUtil
        {
            get { return (GarmentSupplierDataUtil)ServiceProvider.GetService(typeof(GarmentSupplierDataUtil)); }
        }

        private GarmentSupplierService Services
        {
            get { return (GarmentSupplierService)ServiceProvider.GetService(typeof(GarmentSupplierService)); }
        }
    }
}
