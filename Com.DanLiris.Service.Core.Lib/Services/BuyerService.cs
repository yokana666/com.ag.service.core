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
using CsvHelper.TypeConversion;
using Microsoft.Extensions.Primitives;

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class BuyerService : BasicService<CoreDbContext, Buyer>, IBasicUploadCsvService<BuyerViewModel>, IMap<Buyer, BuyerViewModel>
    {
        private readonly string[] Types = { "Lokal", "Ekspor", "Internal" };
        private readonly string[] Countries = { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Anguilla", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Chad", "Chile", "China", "Colombia", "Congo", "Cook Islands", "Costa Rica", "Cote D Ivoire", "Croatia", "Cruise Ship", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Estonia", "Ethiopia", "Falkland Islands", "Faroe Islands", "Fiji", "Finland", "France", "French Polynesia", "French West Indies", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kuwait", "Kyrgyz Republic", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Mauritania", "Mauritius", "Mexico", "Moldova", "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Namibia", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Korea", "Norway", "Oman", "Pakistan", "Palestine", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Saint Pierre and Miquelon", "Samoa", "San Marino", "Satellite", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "South Africa", "South Korea", "Spain", "Sri Lanka", "St Kitts and Nevis", "St Lucia", "St Vincent", "St. Lucia", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Timor L'Este", "Togo", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States of America", "Uruguay", "Uzbekistan", "Venezuela", "Vietnam", "Virgin Islands (US)", "Yemen", "Zambia", "Zimbabwe" };

        public BuyerService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Buyer>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Buyer> Query = this.DbContext.Buyers;
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
                "_id", "code", "name", "address", "city", "country", "contact", "tempo"
            };

            Query = Query
                .Select(b => new Buyer
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name,
                    Address = b.Address,
                    City = b.City,
                    Country = b.Country,
                    Contact = b.Contact,
                    Tempo = b.Tempo
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
            Pageable<Buyer> pageable = new Pageable<Buyer>(Query, Page - 1, Size);
            List<Buyer> Data = pageable.Data.ToList<Buyer>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public BuyerViewModel MapToViewModel(Buyer buyer)
        {
            BuyerViewModel buyerVM = new BuyerViewModel();

            buyerVM._id = buyer.Id;
            buyerVM._deleted = buyer._IsDeleted;
            buyerVM._active = buyer.Active;
            buyerVM._createdDate = buyer._CreatedUtc;
            buyerVM._createdBy = buyer._CreatedBy;
            buyerVM._createAgent = buyer._CreatedAgent;
            buyerVM._updatedDate = buyer._LastModifiedUtc;
            buyerVM._updatedBy = buyer._LastModifiedBy;
            buyerVM._updateAgent = buyer._LastModifiedAgent;
            buyerVM.code = buyer.Code;
            buyerVM.name = buyer.Name;
            buyerVM.address = buyer.Address;
            buyerVM.city = buyer.City;
            buyerVM.country = buyer.Country;
            buyerVM.contact = buyer.Contact;
            buyerVM.tempo = buyer.Tempo;
            buyerVM.type = buyer.Type;
            buyerVM.NPWP = buyer.NPWP;

            return buyerVM;
        }

        public Buyer MapToModel(BuyerViewModel buyerVM)
        {
            Buyer buyer = new Buyer();

            buyer.Id = buyerVM._id;
            buyer._IsDeleted = buyerVM._deleted;
            buyer.Active = buyerVM._active;
            buyer._CreatedUtc = buyerVM._createdDate;
            buyer._CreatedBy = buyerVM._createdBy;
            buyer._CreatedAgent = buyerVM._createAgent;
            buyer._LastModifiedUtc = buyerVM._updatedDate;
            buyer._LastModifiedBy = buyerVM._updatedBy;
            buyer._LastModifiedAgent = buyerVM._updateAgent;
            buyer.Code = buyerVM.code;
            buyer.Name = buyerVM.name;
            buyer.Address = buyerVM.address;
            buyer.City = buyerVM.city;
            buyer.Country = buyerVM.country;
            buyer.Contact = buyerVM.contact;
            buyer.Tempo = !Equals(buyerVM.tempo, null) ? Convert.ToInt32(buyerVM.tempo) : null; /* Check Null */
            buyer.Type = buyerVM.type;
            buyer.NPWP = buyerVM.NPWP;

            return buyer;
        }

        /* Upload CSV */
        private readonly List<string> Header = new List<string>()
        {
            "Kode Buyer", "Nama", "Alamat", "Kota", "Negara", "NPWP", "Jenis Buyer", "Kontak", "Tempo"
        };

        public List<string> CsvHeader => Header;

        public sealed class BuyerMap : ClassMap<BuyerViewModel>
        {
            public BuyerMap()
            {
                Map(b => b.code).Index(0);
                Map(b => b.name).Index(1);
                Map(b => b.address).Index(2);
                Map(b => b.city).Index(3);
                Map(b => b.country).Index(4);
                Map(b => b.NPWP).Index(5);
                Map(b => b.type).Index(6);
                Map(b => b.contact).Index(7);
                Map(b => b.tempo).Index(8).TypeConverter<StringConverter>();
            }
        }

        public Tuple<bool, List<object>> UploadValidate(List<BuyerViewModel> Data, List<KeyValuePair<string, StringValues>> Body)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;

            foreach (BuyerViewModel buyerVM in Data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(buyerVM.code))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh kosong, ");
                }
                else if(Data.Any(d => d != buyerVM && d.code.Equals(buyerVM.code)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                }

                if (string.IsNullOrWhiteSpace(buyerVM.name))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh kosong, ");
                }

                if (string.IsNullOrWhiteSpace(buyerVM.type))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Jenis Buyer tidak boleh kosong, ");
                }
                else if (!Types.Any(t => t.Equals(buyerVM.type)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Jenis Buyer harus salah satu dari Lokal, Ekspor, Internal; ");
                }

                if (string.IsNullOrWhiteSpace(buyerVM.country))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Negara tidak boleh kosong, ");
                }
                else if (!Countries.Any(c => c.Equals(buyerVM.country, StringComparison.CurrentCultureIgnoreCase)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Negara tidak terdaftar di list Negara, ");
                }

                int Tempo = 0;
                if (string.IsNullOrWhiteSpace(buyerVM.tempo))
                {
                    buyerVM.tempo = 0;
                }
                else if (!int.TryParse(buyerVM.tempo, out Tempo))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Tempo harus angka, ");
                }

                if(string.IsNullOrEmpty(ErrorMessage))
                {
                    /* Service Validation */
                    if (this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Code.Equals(buyerVM.code)))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                    }
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    buyerVM.tempo = Tempo;
                    buyerVM.country = Countries.First(c => c.Equals(buyerVM.country, StringComparison.CurrentCultureIgnoreCase));
                }
                else
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;

                    Error.Add("Kode Buyer", buyerVM.code);
                    Error.Add("Nama", buyerVM.name);
                    Error.Add("Alamat", buyerVM.address);
                    Error.Add("Kota", buyerVM.city);
                    Error.Add("Negara", buyerVM.country);
                    Error.Add("NPWP", buyerVM.NPWP);
                    Error.Add("Jenis Buyer", buyerVM.type);
                    Error.Add("Kontak", buyerVM.contact);
                    Error.Add("Tempo", buyerVM.tempo);
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