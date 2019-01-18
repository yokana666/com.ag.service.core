using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Lib.Services.MachineSpinning;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using System.Linq;
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

        public MachineSpinningModel GetNewData(CoreDbContext dbContext)
        {
            SetUnitAndUOM(dbContext);
            var header = Service.GetMachineTypes();
            MachineSpinningModel TestData = new MachineSpinningModel()
            {
                No = "111",
                Code = "Code",
                Name = "Name",
                Year = 2018,
                Condition = "Condition",
                Type = header.First(),
                Delivery = 2,
                CapacityPerHour = 1808.123,
                CounterCondition="test",
                Brand="test",
                UomId="1",
                UomUnit= dbContext.UnitOfMeasurements.FirstOrDefault().Unit,
                Line = "Line",
                UnitCode = "UnitC",
                UnitId = "1",
                UnitName = dbContext.Units.FirstOrDefault().Name
            };

            return TestData;
        }

        public MachineSpinningViewModel GetDataToValidate(CoreDbContext dbContext)
        {
            SetUnitAndUOM(dbContext);
            var header = Service.GetMachineTypes();
            MachineSpinningViewModel TestData = new MachineSpinningViewModel()
            {
                No = "11",                
                Code = "Code",
                Name = "Name",
                Year = 2018,
                Condition = "Condition",
                Type = header.First(),
                Delivery = 2,
                CapacityPerHour = 1808.123,
                CounterCondition = "test",
                Brand = "test",
                UomId = "1",
                UomUnit = dbContext.UnitOfMeasurements.FirstOrDefault().Unit,
                Line = "Line",
                UnitCode = "UnitC",
                UnitId = "1",
                UnitName = dbContext.Units.FirstOrDefault().Name
            };

            return TestData;
        }

        public async Task<MachineSpinningModel> GetTestData(CoreDbContext dbContext)
        {
            SetUnitAndUOM(dbContext);
            MachineSpinningModel model = GetNewData(dbContext);
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }

        private void SetUnitAndUOM(CoreDbContext dbContext)
        {
            Unit unit = new Unit()
            {
                Name = "Test",
                Code = "Code",
                Description = "Description"
            };
            dbContext.Units.Add(unit);
            Uom uom = new Uom()
            {
                Unit = "Unit"
            };
            dbContext.UnitOfMeasurements.Add(uom);
            dbContext.SaveChanges();
        }
    }
}
