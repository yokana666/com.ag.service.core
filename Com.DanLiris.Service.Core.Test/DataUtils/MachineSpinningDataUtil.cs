using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services.MachineSpinning;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Test.DataUtils
{
    public class MachineSpinningDataUtil
    {
        private readonly MachineSpinningService Service;

        public MachineSpinningDataUtil(MachineSpinningService service)
        {
            Service = service;
        }

        public MachineSpinningModel GetNewData()
        {
            MachineSpinningModel TestData = new MachineSpinningModel()
            {
                Code = "Code",
                Name = "Name",
                Year = 2018,
                Condition = "Condition",
                Type = "Type",
                Delivery = 2,
                CapacityPerHour = 1808.123,
                CounterCondition="test",
                Brand="test",
                UomId="1",
                UomUnit="uomtest",
                Line = "Line",
                UnitCode = "UnitC",
                UnitId = "1",
                UnitName = "UnitName"
            };

            return TestData;
        }

        public MachineSpinningViewModel GetDataToValidate()
        {
            MachineSpinningViewModel TestData = new MachineSpinningViewModel()
            {
                Code = "Code",
                Name = "Name",
                Year = 2018,
                Condition = "Condition",
                Type = "Type",
                Delivery = 2,
                CapacityPerHour = 1808.123,
                CounterCondition = "test",
                Brand = "test",
                UomId = "1",
                UomUnit = "uomtest",
                Line = "Line",
                UnitCode = "UnitC",
                UnitId = "1",
                UnitName = "UnitName"
            };

            return TestData;
        }

        public async Task<MachineSpinningModel> GetTestData()
        {
            MachineSpinningModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }
    }
}
