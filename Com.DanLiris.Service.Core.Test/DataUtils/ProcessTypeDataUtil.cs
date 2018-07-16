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
    public class ProcessTypeDataUtil : BasicDataUtil<CoreDbContext, ProcessTypeService, ProcessType>, IEmptyData<ProcessTypeViewModel>
    {
        public ProcessTypeDataUtil(CoreDbContext dbContext, ProcessTypeService service) : base(dbContext, service)
        {
        }
        public ProcessTypeViewModel GetEmptyData()
        {
            ProcessTypeViewModel Data = new ProcessTypeViewModel();

            Data.Name = "";
            Data.Code = "";
            Data.OrderType = null;
            return Data;
        }

        public override ProcessType GetNewData()
        {
            string guid = Guid.NewGuid().ToString();
            ProcessType TestData = new ProcessType
            {
                Name = "TEST",
                Code = guid,
                Remark = "remark",
                OrderTypeId = 1,
                OrderTypeCode= "testOrderId",
                OrderTypeName="testOrderName",
                OrderTypeRemark="OrderTypeRemark",
            };

            return TestData;
        }

        public override async Task<ProcessType> GetTestDataAsync()
        {
            ProcessType Data = GetNewData();
            await this.Service.CreateModel(Data);
            return Data;
        }
    }
}
