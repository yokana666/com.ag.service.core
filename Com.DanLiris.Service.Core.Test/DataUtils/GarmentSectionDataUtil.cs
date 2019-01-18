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
    public class GarmentSectionDataUtil : BasicDataUtil<CoreDbContext, GarmentSectionService, GarmentSection>, IEmptyData<GarmentSectionViewModel>
    {
        public GarmentSectionDataUtil(CoreDbContext dbContext, GarmentSectionService service) : base(dbContext, service)
        {
        }

        public GarmentSectionViewModel GetEmptyData()
        {
            GarmentSectionViewModel Data = new GarmentSectionViewModel();

            return Data;
        }

        public override GarmentSection GetNewData()
        {
            GarmentSection model = new GarmentSection();

            string guid = Guid.NewGuid().ToString();

            model.Code = guid;
            model.Name = guid;
            model.Remark = guid;

            return model;
        }

        public async override Task<GarmentSection> GetTestDataAsync()
        {
            GarmentSection model = GetNewData();
            await this.Service.CreateModel(model);
            return model;
        }
    }
}
