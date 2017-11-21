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

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class CurrencyService : StandardEntityService<CoreDbContext, Currency>, IGeneralService<Currency>, IGeneralUploadService<CurrencyViewModel>, IMap<Currency, CurrencyViewModel>
    {
        public CurrencyService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Tuple<List<Currency>, int, Dictionary<string, string>, List<string>> Read(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Currency> Query = this.DbContext.Currencies;
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            /* Search With Keyword */
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                {
                    "Code", "Symbol"
                };

                Query = Query.Where(General.BuildSearch(SearchAttributes, Keyword), Keyword);
            }

            /* Const Select */
            List<string> SelectedFields = new List<string>()
            {
                "_id", "code", "symbol", "description"
            };

            Query = Query
                .Select(c => new Currency
                {
                    Id = c.Id,
                    Code = c.Code,
                    Symbol = c.Symbol,
                    Description = c.Description
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
            Pageable<Currency> pageable = new Pageable<Currency>(Query, Page - 1, Size);
            List<Currency> Data = pageable.Data.ToList<Currency>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public CurrencyViewModel MapToViewModel(Currency currency)
        {
            CurrencyViewModel currencyVM = new CurrencyViewModel();

            currencyVM._id = currency.Id;
            currencyVM._deleted = currency._IsDeleted;
            currencyVM._active = currency.Active;
            currencyVM._createdDate = currency._CreatedUtc;
            currencyVM._createdBy = currency._CreatedBy;
            currencyVM._createAgent = currency._CreatedAgent;
            currencyVM._updatedDate = currency._LastModifiedUtc;
            currencyVM._updatedBy = currency._LastModifiedBy;
            currencyVM._updateAgent = currency._LastModifiedAgent;
            currencyVM.code = currency.Code;
            currencyVM.symbol = currency.Symbol;
            currencyVM.rate = currency.Rate;
            currencyVM.description = currency.Description;

            return currencyVM;
        }

        public Currency MapToModel(CurrencyViewModel currencyVM)
        {
            Currency currency = new Currency();

            currency.Id = currencyVM._id;
            currency._IsDeleted = currencyVM._deleted;
            currency.Active = currencyVM._active;
            currency._CreatedUtc = currencyVM._createdDate;
            currency._CreatedBy = currencyVM._createdBy;
            currency._CreatedAgent = currencyVM._createAgent;
            currency._LastModifiedUtc = currencyVM._updatedDate;
            currency._LastModifiedBy = currencyVM._updatedBy;
            currency._LastModifiedAgent = currencyVM._updateAgent;
            currency.Code = currencyVM.code;
            currency.Symbol = currencyVM.symbol;
            currency.Rate = currencyVM.rate;
            currency.Description = currencyVM.description;

            return currency;
        }

        /* Upload CSV */
        private readonly List<string> Header = new List<string>()
        {
            "Kode", "Simbol", "Rate", "Keterangan"
        };

        public List<string> CsvHeader => Header;

        public sealed class CurrencyMap : ClassMap<CurrencyViewModel>
        {
            public CurrencyMap()
            {
                Map(c => c.code).Index(0);
                Map(c => c.symbol).Index(1);
                Map(c => c.rate).Index(2).TypeConverter<StringConverter>();
                Map(c => c.description).Index(3);
            }
        }

        public Tuple<bool, List<object>> UploadValidate(List<CurrencyViewModel> Data)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;

            foreach (CurrencyViewModel currencyVM in Data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(currencyVM.code))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != currencyVM && d.code.Equals(currencyVM.code)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                }

                if(string.IsNullOrWhiteSpace(currencyVM.symbol))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Simbol tidak boleh kosong, ");
                }

                double Rate = 0;
                if (string.IsNullOrWhiteSpace(currencyVM.rate))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Rate tidak boleh kosong, ");
                }
                else if (!double.TryParse(currencyVM.rate, out Rate))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Rate harus numerik, ");
                }
                else if (Rate < 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Rate harus lebih besar dari 0, ");
                }
                else
                {
                    string[] RateSplit = currencyVM.rate.Split('.');
                    if (RateSplit.Count().Equals(2) && RateSplit[1].Length > 2)
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Rate maksimal memiliki 2 digit dibelakang koma, ");
                    }
                }

                if (string.IsNullOrWhiteSpace(currencyVM.description))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Keterangan tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != currencyVM && d.description.Equals(currencyVM.description)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Keterangan tidak boleh duplikat, ");
                }

                if(string.IsNullOrEmpty(ErrorMessage))
                {
                    /* Service Validation */
                    if(this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Code.Equals(currencyVM.code)))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                    }

                    if(this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Description.Equals(currencyVM.description)))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Keterangan tidak boleh duplikat, ");
                    }
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    currencyVM.rate = Rate;
                }
                else
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;

                    Error.Add("Kode", currencyVM.code);
                    Error.Add("Simbol", currencyVM.symbol);
                    Error.Add("Rate", currencyVM.rate);
                    Error.Add("Keterangan", currencyVM.description);
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