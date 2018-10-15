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
			
			return Data;
		}

		public override GarmentSupplier GetNewData()
		{
			string guid = Guid.NewGuid().ToString();
			GarmentSupplier TestData = new GarmentSupplier
			{
				Name = "TEST NAME",
				Code = "TEST CODE1",
				UseVat=true,
				Import=false,
				UseTax = true,
				IncomeTaxesName = "TEST INCOME TAX",
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
