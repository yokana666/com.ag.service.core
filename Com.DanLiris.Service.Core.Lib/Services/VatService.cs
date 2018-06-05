using Com.DanLiris.Service.Core.Lib.Models;
using Com.Moonlay.NetCore.Lib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Com.DanLiris.Service.Core.Lib.Helpers;
using Newtonsoft.Json;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using Com.DanLiris.Service.Core.Lib.Interfaces;
using CsvHelper.Configuration;
using System.Dynamic;
using CsvHelper.TypeConversion;
using Microsoft.Extensions.Primitives;

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class VatService : BasicService<CoreDbContext, Vat>, IBasicUploadCsvService<VatViewModel>, IMap<Vat, VatViewModel>
    {
        public VatService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Vat>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Vat> Query = this.DbContext.Vats;
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            /* Search With Keyword */
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                {
                    "Name"
                };

                Query = Query.Where(General.BuildSearch(SearchAttributes), Keyword);
            }

            /* Const Select */
            List<string> SelectedFields = new List<string>()
            {
                "_id", "name", "rate"
            };

            Query = Query
                .Select(v => new Vat
                {
                    Id = v.Id,
                    Name = v.Name,
                    Rate = v.Rate
                });

            /* Order */
            if (OrderDictionary.Count.Equals(0))
            {
                OrderDictionary.Add("_updatedDate", General.DESCENDING);

                Query = Query.OrderByDescending(b => b._LastModifiedUtc); /* Default Order */
            }
            else
            {
                string Key = OrderDictionary.Keys.First();
                string OrderType = OrderDictionary[Key];
                string TransformKey = General.TransformOrderBy(Key);

                BindingFlags IgnoreCase = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

                Query = OrderType.Equals(General.ASCENDING) ?
                    Query.OrderBy(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b)) :
                    Query.OrderByDescending(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b));
            }

            /* Pagination */
            Pageable<Vat> pageable = new Pageable<Vat>(Query, Page - 1, Size);
            List<Vat> Data = pageable.Data.ToList<Vat>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public VatViewModel MapToViewModel(Vat vat)
        {
            VatViewModel vatVM = new VatViewModel();

            vatVM._id = vat.Id;
            vatVM._deleted = vat._IsDeleted;
            vatVM._active = vat.Active;
            vatVM._createdDate = vat._CreatedUtc;
            vatVM._createdBy = vat._CreatedBy;
            vatVM._createAgent = vat._CreatedAgent;
            vatVM._updatedDate = vat._LastModifiedUtc;
            vatVM._updatedBy = vat._LastModifiedBy;
            vatVM._updateAgent = vat._LastModifiedAgent;
            vatVM.name = vat.Name;
            vatVM.rate = vat.Rate;
            vatVM.description = vat.Description;

            return vatVM;
        }

        public Vat MapToModel(VatViewModel vatVM)
        {
            Vat vat = new Vat();

            vat.Id = vatVM._id;
            vat._IsDeleted = vatVM._deleted;
            vat.Active = vatVM._active;
            vat._CreatedUtc = vatVM._createdDate;
            vat._CreatedBy = vatVM._createdBy;
            vat._CreatedAgent = vatVM._createAgent;
            vat._LastModifiedUtc = vatVM._updatedDate;
            vat._LastModifiedBy = vatVM._updatedBy;
            vat._LastModifiedAgent = vatVM._updateAgent;
            vat.Name = vatVM.name;
            vat.Rate = !Equals(vatVM.rate, null) ? Convert.ToDouble(vatVM.rate) : null; /* Check Null */
            vat.Description = vatVM.description;

            return vat;
        }

        /* Upload CSV */
        private readonly List<string> Header = new List<string>()
        {
            "Nama", "Rate", "Deskripsi"
        };

        public List<string> CsvHeader => Header;

        public sealed class VatMap : ClassMap<VatViewModel>
        {
            public VatMap()
            {
                Map(v => v.name).Index(0);
                Map(v => v.rate).Index(1).TypeConverter<StringConverter>();
                Map(v => v.description).Index(2);
            }
        }

        public Tuple<bool, List<object>> UploadValidate(List<VatViewModel> Data, List<KeyValuePair<string, StringValues>> Body)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;

            foreach (VatViewModel vatVM in Data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(vatVM.name))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh kosong, ");
                }

                double Rate = 0;
                if (string.IsNullOrWhiteSpace(vatVM.rate))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Rate tidak boleh kosong, ");
                }
                else if(!double.TryParse(vatVM.rate, out Rate))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Rate harus numerik, ");
                }
                else if(Rate < 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Rate harus lebih besar dari 0, ");
                }
                else
                {
                    string[] RateSplit = vatVM.rate.Split('.');
                    if(RateSplit.Count().Equals(2) && RateSplit[1].Length > 2)
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Rate maksimal memiliki 2 digit dibelakang koma, ");
                    }
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    /* Service Validation */
                    if (Data.Any(d => d != vatVM && d.name.Equals(vatVM.name) && d.rate.Equals(vatVM.rate)) || this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Name.Equals(vatVM.name) && d.Rate.Equals(Rate)))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Kombinasi Nama dan Rate tidak boleh sama, ");
                    }
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    vatVM.rate = Rate;
                }
                else
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;

                    Error.Add("Nama", vatVM.name);
                    Error.Add("Rate", vatVM.rate);
                    Error.Add("Deskripsi", vatVM.description);
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
    }
}