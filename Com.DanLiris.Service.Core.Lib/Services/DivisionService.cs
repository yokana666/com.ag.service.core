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
    public class DivisionService : BasicService<CoreDbContext, Division>, IBasicUploadCsvService<DivisionViewModel>, IMap<Division, DivisionViewModel>
    {
        private readonly string[] Types = { "Lokal", "Ekspor", "Internal" };
        private readonly string[] Countries = { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cruise Ship", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kuwait", "Kyrgyz Republic", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mauritania", "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Namibia", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Korea", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre and Miquelon", "Samoa", "San Marino", "Satellite", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "South Africa", "South Korea", "Spain", "Sri Lanka", "St Kitts and Nevis", "St Lucia", "St Vincent", "St. Lucia", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States of America", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe" };

        public DivisionService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Division>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null,string Filter="{}")
        {
            IQueryable<Division> Query = this.DbContext.Divisions;
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            /* Search With Keyword */
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                {
                    "Code", "Name"
                };

                Query = Query.Where(General.BuildSearch(SearchAttributes), Keyword);
            }

            /* Const Select */
            List<string> SelectedFields = new List<string>()
                {
                    "_id", "name"
                };

            Query = Query
                .Select(b => new Division
                {
                    Id = b.Id,
                    Name = b.Name
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
            Pageable<Division> pageable = new Pageable<Division>(Query, Page - 1, Size);
            List<Division> Data = pageable.Data.ToList<Division>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public DivisionViewModel MapToViewModel(Division division)
        {
            DivisionViewModel divisionVM = new DivisionViewModel();

            divisionVM._id = division.Id;
            divisionVM._deleted = division._IsDeleted;
            divisionVM._active = division.Active;
            divisionVM._createdDate = division._CreatedUtc;
            divisionVM._createdBy = division._CreatedBy;
            divisionVM._createAgent = division._CreatedAgent;
            divisionVM._updatedDate = division._LastModifiedUtc;
            divisionVM._updatedBy = division._LastModifiedBy;
            divisionVM._updateAgent = division._LastModifiedAgent;
            divisionVM.code = division.Code;
            divisionVM.name = division.Name;
            divisionVM.description = division.Description;

            return divisionVM;
        }

        public Division MapToModel(DivisionViewModel divisionVM)
        {
            Division division = new Division();

            division.Id = divisionVM._id;
            division._IsDeleted = divisionVM._deleted;
            division.Active = divisionVM._active;
            division._CreatedUtc = divisionVM._createdDate;
            division._CreatedBy = divisionVM._createdBy;
            division._CreatedAgent = divisionVM._createAgent;
            division._LastModifiedUtc = divisionVM._updatedDate;
            division._LastModifiedBy = divisionVM._updatedBy;
            division._LastModifiedAgent = divisionVM._updateAgent;
            division.Code = divisionVM.code;
            division.Name = divisionVM.name;
            division.Description = divisionVM.description;

            return division;
        }

        /* Upload CSV */
        private readonly List<string> Header = new List<string>()
        {
            "Nama", "Deskripsi"
        };

        public List<string> CsvHeader => Header;

        public sealed class DivisionMap : ClassMap<DivisionViewModel>
        {
            public DivisionMap()
            {
                Map(b => b.name).Index(0);
                Map(b => b.description).Index(1);
            }
        }

        public Tuple<bool, List<object>> UploadValidate(List<DivisionViewModel> Data, List<KeyValuePair<string, StringValues>> Body)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;

            foreach (DivisionViewModel divisionVM in Data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(divisionVM.name))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh kosong, ");
                }
                else if(Data.Any(d => d != divisionVM && d.name.Equals(divisionVM.name)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh duplikat, ");
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    /* Service Validation */
                    if (this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Name.Equals(divisionVM.name)))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh duplikat, ");
                    }
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;

                    Error.Add("Nama", divisionVM.name);
                    Error.Add("Deskripsi", divisionVM.description);
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

        public override void OnCreating(Division model)
        {
            CodeGenerator codeGenerator = new CodeGenerator();

            do
            {
                model.Code = codeGenerator.GenerateCode();
            }
            while (this.DbSet.Any(d => d.Code.Equals(model.Code)));

            base.OnCreating(model);
        }
    }
}