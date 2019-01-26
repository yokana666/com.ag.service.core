using Com.DanLiris.Service.Core.Lib.Helpers;
using Com.DanLiris.Service.Core.Lib.Helpers.IdentityService;
using Com.DanLiris.Service.Core.Lib.Models;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
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

        private readonly List<string> Header = new List<string>()
        {
            "No Mesin", "Unit", "Line", "Merk Mesin", "Type Mesin", "Jenis Proses", "Tahun", "Kondisi Mesin", "Kondisi Counter",
            "Jumlah Delivery", "Kapasitas/Hari", "Satuan"
        };

        private readonly Dictionary<string, string> MachineTypes = new Dictionary<string, string>()
        {
            { "Blowing","BK" },
            { "Carding","CK" },
            { "Pre-Drawing","PD" },
            { "Combing","CM" },
            { "Finish Drawing","FD" },
            { "Flying","FL" },
            { "Ring Spinning","RF" },
            { "Winding","WD" }

        };
        public List<string> CsvHeader => Header;

        public MachineSpinningService(IServiceProvider serviceProvider, CoreDbContext dbContext)
        {
            _DbContext = dbContext;
            _DbSet = dbContext.Set<MachineSpinningModel>();
            _IdentityService = serviceProvider.GetService<IIdentityService>();
        }

        public MachineSpinningService(IServiceProvider provider)
        {
        }

        public sealed class MachineSpinningMap : ClassMap<MachineSpinningViewModel>
        {
            public MachineSpinningMap()
            {
                Map(b => b.No).Index(0);
                Map(b => b.UnitName).Index(1);
                Map(b => b.Line).Index(2);
                Map(b => b.Name).Index(3);
                Map(b => b.Brand).Index(4);
                Map(b => b.Type).Index(5);
                Map(b => b.Year).Index(6);
                Map(b => b.Condition).Index(7);
                Map(b => b.CounterCondition).Index(8);
                Map(b => b.Delivery).Index(9);
                Map(b => b.CapacityPerHour).Index(10);
                Map(b => b.UomUnit).Index(11);
            }
        }

        public async Task<int> CreateAsync(MachineSpinningModel model)
        {
            model.Code = GenerateCode(model);
            model.FlagForCreate(_IdentityService.Username, _UserAgent);
            model._LastModifiedAgent = _UserAgent;
            model._LastModifiedBy = _IdentityService.Username;
            model._LastModifiedUtc = DateTime.Now;
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
                    CounterCondition = s.CounterCondition,
                    Brand = s.Brand,
                    No = s.No,
                    Name = s.Name,
                    Type = s.Type,
                    Year = s.Year,
                    Line = s.Line,
                    UomId = s.UomId,
                    UnitCode = s.UnitCode,
                    UnitId = s.UnitId,
                    UnitName = s.UnitName,
                    UomUnit = s.UomUnit
                });

            List<string> searchAttributes = new List<string>()
            {
                "Brand", "Name", "No"
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
                   CounterCondition = s.CounterCondition,
                   Brand = s.Brand,
                   Name = s.Name,
                   No = s.No,
                   Type = s.Type,
                   Year = s.Year,
                   Line = s.Line,
                   UnitCode = s.UnitCode,
                   UnitId = s.UnitId,
                   UnitName = s.UnitName,
                   UomId = s.UomId,
                   UomUnit = s.UomUnit
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

        public Tuple<bool, List<object>> UploadValidate(List<MachineSpinningViewModel> Data, List<KeyValuePair<string, StringValues>> Body)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;
            var dbData = _DbSet.ToList();
            foreach (var machineSpinningVM in Data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(machineSpinningVM.Name))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Merk Mesin tidak boleh kosong, ");
                }

                if (string.IsNullOrWhiteSpace(machineSpinningVM.No))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "No tidak boleh kosong, ");
                }

                if (string.IsNullOrWhiteSpace(machineSpinningVM.Brand))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Type Mesin tidak boleh kosong, ");
                }

                if (machineSpinningVM.Year == null || machineSpinningVM.Year <= 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Tahun tidak boleh kosong, ");
                }

                if (string.IsNullOrWhiteSpace(machineSpinningVM.Condition))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kondisi tidak boleh kosong, ");
                }

                if (string.IsNullOrWhiteSpace(machineSpinningVM.CounterCondition))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kondisi Counter tidak boleh kosong, ");
                }

                if (string.IsNullOrWhiteSpace(machineSpinningVM.Type))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Jenis Proses tidak boleh kosong, ");
                }
                else
                {
                    if (!GetMachineTypes().Contains(machineSpinningVM.Type))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Jenis Proses tidak termasuk kategori yang ditentukan, ");
                    }
                }

                if (machineSpinningVM.Delivery == null || machineSpinningVM.Delivery <= 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Delivery tidak boleh kosong, ");
                }

                if (string.IsNullOrWhiteSpace(machineSpinningVM.UomUnit))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Satuan tidak boleh kosong, ");
                }
                else
                {
                    if (!_DbContext.UnitOfMeasurements.Any(x => x.Unit == machineSpinningVM.UomUnit))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Nama Satuan tidak terdaftar, ");
                    }
                }

                if (machineSpinningVM.CapacityPerHour == null || machineSpinningVM.CapacityPerHour <= 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Capacity per Hour tidak boleh kosong, ");
                }

                if (string.IsNullOrEmpty(machineSpinningVM.Line))
                    ErrorMessage = string.Concat(ErrorMessage, "Line tidak boleh kosong, ");

                if (string.IsNullOrEmpty(machineSpinningVM.UnitName))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Unit tidak boleh kosong, ");
                }
                else
                {
                    if (!_DbContext.Units.Any(x => x.Name == machineSpinningVM.UnitName))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Nama Unit tidak terdaftar, ");
                    }
                }

                if (!string.IsNullOrEmpty(machineSpinningVM.Name) && !string.IsNullOrEmpty(machineSpinningVM.UnitName) && !string.IsNullOrEmpty(machineSpinningVM.No))
                {

                    if (dbData.Any(r => r._IsDeleted.Equals(false) && r.Id != machineSpinningVM.Id && r.Name.Equals(machineSpinningVM.Name) && r.No == machineSpinningVM.No
                                && r.UnitName == machineSpinningVM.UnitName && r.Line == machineSpinningVM.Line && r.Brand == machineSpinningVM.Brand && r.Type == machineSpinningVM.Type))/* Name Unique */
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Nomor, Unit, Line, Merk, Type dan Jenis Proses Mesin sudah ada di database, ");
                    }

                    if (Data.Any(d => d != machineSpinningVM && d.Name.Equals(machineSpinningVM.Name) && d.UnitName.Equals(machineSpinningVM.UnitName) && d.No.Equals(machineSpinningVM.No)
                            && d.Line == machineSpinningVM.Line && d.Brand == machineSpinningVM.Brand && d.Type == machineSpinningVM.Type))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Nomor, Unit, Line, Merk, Type dan Jenis Proses Mesin tidak boleh duplikat, ");
                    }
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;

                    Error.Add("No Mesin", machineSpinningVM.No);
                    Error.Add("Unit", machineSpinningVM.UnitName);
                    Error.Add("Line", machineSpinningVM.Line);
                    Error.Add("Merk Mesin", machineSpinningVM.Name);
                    Error.Add("Type Mesin", machineSpinningVM.Brand);
                    Error.Add("Jenis Proses", machineSpinningVM.Type);
                    Error.Add("Tahun Mesin", machineSpinningVM.Year);
                    Error.Add("Kondisi Mesin", machineSpinningVM.Condition);
                    Error.Add("Kondisi Counter", machineSpinningVM.CounterCondition);
                    Error.Add("Jumlah Delivery", machineSpinningVM.Delivery);
                    Error.Add("Kapasitas/Hari", machineSpinningVM.CapacityPerHour);
                    Error.Add("Satuan", machineSpinningVM.UomUnit);
                    Error.Add("Error", ErrorMessage);

                    ErrorList.Add(Error);
                }
            }

            if (ErrorList.Count > 0)
            {
                Valid = false;
            }

            return Tuple.Create(Valid, ErrorList);
        }

        public Task<int> UploadData(List<MachineSpinningModel> data)
        {
            return Task.Factory.StartNew(async () =>
            {
                const int pageSize = 1000;
                int offset = 0;
                int processed = 0;

                var batch = data.Where((item, index) => offset <= index && index < offset + pageSize);
                using (var transaction = _DbContext.Database.BeginTransaction())
                {
                    while (batch.Count() > 0)
                    {
                        foreach (var item in batch)
                        {
                            item.Code = GenerateCode(item);
                            var unit = _DbContext.Units.FirstOrDefault(x => x.Name == item.UnitName);
                            item.UnitId = unit?.Id.ToString();
                            item.UnitCode = unit?.Code.ToString();

                            var uom = _DbContext.UnitOfMeasurements.FirstOrDefault(x => x.Unit == item.UomUnit);
                            item.UomId = uom?.Id.ToString();

                            item.FlagForCreate(_IdentityService.Username, _UserAgent);
                            item.FlagForUpdate(_IdentityService.Username, _UserAgent);
                        }
                        _DbContext.MachineSpinnings.AddRange(batch);
                        var result = await _DbContext.SaveChangesAsync();
                        processed += batch.Count();
                        offset = pageSize;
                    };
                    transaction.Commit();
                }

                return processed;
            }).Unwrap();
        }

        public MemoryStream DownloadTemplate()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream))
                {

                    using (var csvWriter = new CsvWriter(streamWriter))
                    {
                        foreach (var item in CsvHeader)
                        {
                            csvWriter.WriteField(item);
                        }
                        csvWriter.NextRecord();
                    }
                }
                return stream;
            }
        }

        public List<string> GetMachineTypes()
        {
            return MachineTypes.Keys.ToList();
        }

        private string GenerateCode(MachineSpinningModel model)
        {
            string value;
            if (MachineTypes.TryGetValue(model.Type, out value))
            {
                int dataCount = _DbContext.MachineSpinnings.Count(x => x.Type == model.Type && x.Line == model.Line);
                string dataCountString = (dataCount + 1).ToString("000");

                return value + dataCountString + model.Line;
            }
            return "";
        }

        public List<MachineSpinningModel> GetSimple()
        {
            return this._DbSet.Select(x => new MachineSpinningModel()
            {
                Id = x.Id,
                No = x.No,
                Code = x.Code,
                Brand = x.Brand,
                Name = x.Name,
                Type = x.Type,
                Year = x.Year,
                Condition = x.Condition,
                CounterCondition = x.CounterCondition,
                Delivery = x.Delivery,
                CapacityPerHour = x.CapacityPerHour,
                UomId = x.UomId,
                UomUnit = x.UomUnit,
                Line = x.Line,
                UnitCode = x.UnitCode,
                UnitId = x.UnitId,
                UnitName = x.UnitName
            }).ToList();
        }
    }

}
