using Com.DanLiris.Service.Core.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Com.DanLiris.Service.Core.Lib.Helpers;
using Newtonsoft.Json;
using System.Reflection;
using Com.Moonlay.NetCore.Lib;
using Com.DanLiris.Service.Core.Lib.ViewModels;
using CsvHelper.Configuration;
using System.Dynamic;
using Com.DanLiris.Service.Core.Lib.Interfaces;
using Microsoft.Extensions.Primitives;

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class UnitService : BasicService<CoreDbContext, Unit>, IBasicUploadCsvService<UnitViewModel>, IMap<Unit, UnitViewModel>
    {
        private readonly string[] Types = { "Lokal", "Ekspor", "Internal" };
        private readonly string[] Countries = { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cruise Ship", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kuwait", "Kyrgyz Republic", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mauritania", "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Namibia", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Korea", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre and Miquelon", "Samoa", "San Marino", "Satellite", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "South Africa", "South Korea", "Spain", "Sri Lanka", "St Kitts and Nevis", "St Lucia", "St Vincent", "St. Lucia", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States of America", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe" };

        public UnitService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Unit>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null,string Filter="{}")
        {
            IQueryable<Unit> Query = this.DbContext.Units;
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            /* Search With Keyword */
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                {
                    "Code", "DivisionName", "Name"
                };

                Query = Query.Where(General.BuildSearch(SearchAttributes), Keyword);
            }

            /* Const Select */
            List<string> SelectedFields = new List<string>()
            {
                "_id", "code", "division", "name"
            };

            Query = Query
                .Select(u => new Unit
                {
                    Id = u.Id,
                    Code = u.Code,
                    DivisionId = u.DivisionId,
                    DivisionCode = u.DivisionCode,
                    DivisionName = u.DivisionName,
                    Name = u.Name
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
            Pageable<Unit> pageable = new Pageable<Unit>(Query, Page - 1, Size);
            List<Unit> Data = pageable.Data.ToList<Unit>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public UnitViewModel MapToViewModel(Unit unit)
        {
            UnitViewModel unitVM = new UnitViewModel();
            unitVM.division = new UnitDivisionViewModel();

            unitVM._id = unit.Id;
            unitVM._deleted = unit._IsDeleted;
            unitVM._active = unit.Active;
            unitVM._createdDate = unit._CreatedUtc;
            unitVM._createdBy = unit._CreatedBy;
            unitVM._createAgent = unit._CreatedAgent;
            unitVM._updatedDate = unit._LastModifiedUtc;
            unitVM._updatedBy = unit._LastModifiedBy;
            unitVM._updateAgent = unit._LastModifiedAgent;
            unitVM.code = unit.Code;
            unitVM.division._id = unit.DivisionId;
            unitVM.division.code = unit.DivisionCode;
            unitVM.division.name = unit.DivisionName;
            unitVM.name = unit.Name;
            unitVM.description = unit.Description;
            
            return unitVM;
        }

        public Unit MapToModel(UnitViewModel unitVM)
        {
            Unit unit = new Unit();

            unit.Id = unitVM._id;
            unit._IsDeleted = unitVM._deleted;
            unit.Active = unitVM._active;
            unit._CreatedUtc = unitVM._createdDate;
            unit._CreatedBy = unitVM._createdBy;
            unit._CreatedAgent = unitVM._createAgent;
            unit._LastModifiedUtc = unitVM._updatedDate;
            unit._LastModifiedBy = unitVM._updatedBy;
            unit._LastModifiedAgent = unitVM._updateAgent;
            unit.Code = unitVM.code;
            unit.DivisionId = unitVM.division._id;
            unit.DivisionCode = unitVM.division.code;
            unit.DivisionName = unitVM.division.name;
            unit.Name = unitVM.name;
            unit.Description = unitVM.description;

            return unit;
        }

        /* Upload CSV */
        private readonly List<string> Header = new List<string>()
        {
            "Kode Unit", "Divisi", "Nama", "Deskripsi"
        };

        public List<string> CsvHeader => Header;

        public sealed class UnitMap : ClassMap<UnitViewModel>
        {
            public UnitMap()
            {
                Map(b => b.code).Index(0);
                Map(b => b.division.name).Index(1);
                Map(b => b.name).Index(2);
                Map(b => b.description).Index(3);
            }
        }

        public Tuple<bool, List<object>> UploadValidate(List<UnitViewModel> Data, List<KeyValuePair<string, StringValues>> Body)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;
            Division division = null;

            foreach (UnitViewModel unitVM in Data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(unitVM.code))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != unitVM && d.code.Equals(unitVM.code)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                }

                if (string.IsNullOrWhiteSpace(unitVM.name))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != unitVM && d.name.Equals(unitVM.name)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh duplikat, ");
                }

                if(string.IsNullOrWhiteSpace(unitVM.division.name))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Divisi tidak boleh kosong, ");
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    /* Service Validation */
                    division = this.DbContext.Set<Division>().FirstOrDefault(d => d._IsDeleted.Equals(false) && d.Name.Equals(unitVM.division.name));

                    if (this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Code.Equals(unitVM.code)))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                    }

                    if (this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Name.Equals(unitVM.name)))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh duplikat, ");
                    }

                    if (division == null)
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Divisi tidak terdaftar di Master Divisi, ");
                    }
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    unitVM.division._id = division.Id;
                    unitVM.division.code = division.Code;
                }
                else
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;

                    Error.Add("Kode Unit", unitVM.code);
                    Error.Add("Divisi", unitVM.name);
                    Error.Add("Nama", unitVM.name);
                    Error.Add("Deskripsi", unitVM.description);
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