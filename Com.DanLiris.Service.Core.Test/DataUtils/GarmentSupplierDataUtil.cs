using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.DanLiris.Service.Core.Test.Helpers;
using Com.DanLiris.Service.Core.Test.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Test.DataUtils
{
	public class GarmentSupplierDataUtil : BasicDataUtil<CoreDbContext, GarmentSupplierService, GarmentSupplier>, IEmptyData<GarmentSupplierViewModel>
	{
		public GarmentSupplierDataUtil(CoreDbContext dbContext, GarmentSupplierService service) : base(dbContext, service)
		{

		}
		public GarmentSupplierViewModel GetEmptyData()
		{
			GarmentSupplierViewModel Data = new GarmentSupplierViewModel();
            Data.name = string.Empty;
            Data.code = string.Empty;
            Data.usevat = true;
            Data.import = true;
            Data.usetax = true;
            Data.IncomeTaxes = null;
			
			return Data;
		}

		public override GarmentSupplier GetNewData()
		{
			string guid = Guid.NewGuid().ToString();
			GarmentSupplier TestData = new GarmentSupplier
			{
				Name = guid,
				Code = guid,
				UseVat = true,
				Import = true,
				UseTax = true,
                NPWP = guid,
                SerialNumber = guid,
                PIC = guid,
                Address = guid,
                Contact = guid,
                IncomeTaxesId = 1,
                IncomeTaxesName = guid,
                IncomeTaxesRate = 2
			};

			return TestData;
		}

		public override async Task<GarmentSupplier> GetTestDataAsync()
		{
			GarmentSupplier Data = GetNewData();
			await this.Service.CreateModel(Data);
			return Data;
		}
	}
}
