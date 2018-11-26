using Com.DanLiris.Service.Core.Lib.Helpers;
using Com.DanLiris.Service.Core.Lib.Helpers.IdentityService;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Lib.Services.MachineSpinning
{
    public class MachineSpinningService : IMachineSpinningService
    {
        private const string _UserAgent = "core-service";
        protected DbSet<MachineSpinningModel> _DbSet;
        protected IIdentityService _IdentityService;
        public CoreDbContext _DbContext;

        public MachineSpinningService(IServiceProvider serviceProvider, CoreDbContext dbContext)
        {
            _DbContext = dbContext;
            _DbSet = dbContext.Set<MachineSpinningModel>();
            _IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public async Task<int> CreateAsync(MachineSpinningModel model)
        {
            var codeGenerator = new CodeGenerator();
            do
            {
                model.Code = codeGenerator.GenerateCode();
            }
            while (_DbSet.Any(d => d.Code.Equals(model.Code)));

            model.FlagForCreate(_IdentityService.Username, _UserAgent);
            _DbSet.Add(model);
            return await _DbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var model = _DbSet.Where(w => w.Id.Equals(id)).FirstOrDefault();
            model.FlagForDelete(_IdentityService.Username, _UserAgent);
            _DbSet.Update(model);
            return await _DbContext.SaveChangesAsync();
        }

        public ReadResponse<MachineSpinningModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<MachineSpinningModel> Query = _DbSet;

            Query = Query
                .Select(s => new MachineSpinningModel
                {
                    Id = s.Id,
                    _CreatedBy = s._CreatedBy,
                    _CreatedUtc = s._CreatedUtc,
                    Code = s.Code,
                    _LastModifiedUtc = s._LastModifiedUtc,
                    CapacityPerHour = s.CapacityPerHour,
                    Condition = s.Condition,
                    Delivery = s.Delivery,
                    Name = s.Name,
                    Type = s.Type,
                    Year = s.Year
                });

            List<string> searchAttributes = new List<string>()
            {
                "Code", "Name", "Type","Condition"
            };

            Query = QueryHelper<MachineSpinningModel>.Search(Query, searchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<MachineSpinningModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<MachineSpinningModel>.Order(Query, OrderDictionary);

            Pageable<MachineSpinningModel> pageable = new Pageable<MachineSpinningModel>(Query, page - 1, size);
            List<MachineSpinningModel> Data = pageable.Data.ToList();

            List<MachineSpinningModel> list = new List<MachineSpinningModel>();
            list.AddRange(
               Data.Select(s => new MachineSpinningModel
               {
                   Id = s.Id,
                   _CreatedBy = s._CreatedBy,
                   _CreatedUtc = s._CreatedUtc,
                   Code = s.Code,
                   _LastModifiedUtc = s._LastModifiedUtc,
                   CapacityPerHour = s.CapacityPerHour,
                   Condition = s.Condition,
                   Delivery = s.Delivery,
                   Name = s.Name,
                   Type = s.Type,
                   Year = s.Year
               }).ToList()
            );

            int TotalData = pageable.TotalCount;

            return new ReadResponse<MachineSpinningModel>(list, TotalData, OrderDictionary, new List<string>());
        }

        public async Task<MachineSpinningModel> ReadByIdAsync(int id)
        {
            return await _DbSet.Where(w => w.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(int id, MachineSpinningModel model)
        {
            model.FlagForUpdate(_IdentityService.Username, _UserAgent);
            _DbSet.Update(model);
            return await _DbContext.SaveChangesAsync();
        }
    }
}
