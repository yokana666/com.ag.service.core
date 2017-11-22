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
using CsvHelper.Configuration;
using System.Dynamic;
using Com.DanLiris.Service.Core.Lib.Interfaces;
using CsvHelper.TypeConversion;
using Microsoft.Extensions.Primitives;

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class ProductService : BasicService<CoreDbContext, Product>, IGeneralUploadService<ProductViewModel>, IMap<Product, ProductViewModel>
    {
        public ProductService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Product>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Product> Query = this.DbContext.Products;
            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);

            /* Search With Keyword */
            if (Keyword != null)
            {
                List<string> SearchAttributes = new List<string>()
                {
                    "Code", "Name"
                };

                Query = Query.Where(General.BuildSearch(SearchAttributes, Keyword), Keyword);
            }

            /* Const Select */
            List<string> SelectedFields = new List<string>()
            {
                "_id", "code", "name", "uom", "currency", "price", "tags"
            };

            Query = Query
                .Select(p => new Product
                {
                    Id = p.Id,
                    Code = p.Code,
                    Name = p.Name,
                    UomId = p.UomId,
                    UomUnit = p.UomUnit,
                    CurrencyId = p.CurrencyId,
                    CurrencyCode = p.CurrencyCode,
                    Price = p.Price,
                    Tags = p.Tags
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
            Pageable<Product> pageable = new Pageable<Product>(this.DbContext.Products, Page - 1, Size);
            List<Product> Data = pageable.Data.ToList<Product>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public ProductViewModel MapToViewModel(Product product)
        {
            ProductViewModel productVM = new ProductViewModel();
            productVM.currency = new ProductCurrencyViewModel();
            productVM.uom = new ProductUomViewModel();

            productVM._id = product.Id;
            productVM._deleted = product._IsDeleted;
            productVM._active = product.Active;
            productVM._createdDate = product._CreatedUtc;
            productVM._createdBy = product._CreatedBy;
            productVM._createAgent = product._CreatedAgent;
            productVM._updatedDate = product._LastModifiedUtc;
            productVM._updatedBy = product._LastModifiedBy;
            productVM._updateAgent = product._LastModifiedAgent;
            productVM.code = product.Code;
            productVM.name = product.Name;
            productVM.price = product.Price;
            productVM.currency._id = product.CurrencyId;
            productVM.currency.code = product.CurrencyCode;
            productVM.description = product.Description;
            productVM.uom._id = product.UomId;
            productVM.uom.unit = product.UomUnit;
            productVM.tags = product.Tags;

            return productVM;
        }

        public Product MapToModel(ProductViewModel productVM)
        {
            Product product = new Product();

            product.Id = productVM._id;
            product._IsDeleted = productVM._deleted;
            product.Active = productVM._active;
            product._CreatedUtc = productVM._createdDate;
            product._CreatedBy = productVM._createdBy;
            product._CreatedAgent = productVM._createAgent;
            product._LastModifiedUtc = productVM._updatedDate;
            product._LastModifiedBy = productVM._updatedBy;
            product._LastModifiedAgent = productVM._updateAgent;
            product.Code = productVM.code;
            product.Name = productVM.name;
            product.Price = !Equals(productVM.price, null) ? Convert.ToDecimal(productVM.price) : 0; /* Check Null */

            if (!Equals(productVM.currency, null))
            {
                product.CurrencyId = productVM.currency._id;
                product.CurrencyCode = productVM.currency.code;
            }
            else
            {
                product.CurrencyId = null;
                product.CurrencyCode = null;
            }

            product.Description = productVM.description;
            
            if(!Equals(productVM.uom, null))
            {
                product.UomId = productVM.uom._id;
                product.UomUnit = productVM.uom.unit;
            }
            else
            {
                product.UomId = null;
                product.UomUnit = null;
            }

            product.Tags = productVM.tags;

            return product;
        }

        /* Upload CSV */
        private readonly List<string> Header = new List<string>()
        {
            "Kode Barang", "Nama Barang", "Satuan", "Mata Uang", "Harga", "Tags", "Keterangan"
        };

        public List<string> CsvHeader => Header;

        public sealed class ProductMap : ClassMap<ProductViewModel>
        {
            public ProductMap()
            {
                Map(p => p.code).Index(0);
                Map(p => p.name).Index(1);
                Map(p => p.uom.unit).Index(2);
                Map(p => p.currency.code).Index(3);
                Map(p => p.price).Index(4).TypeConverter<StringConverter>();
                Map(p => p.tags).Index(5);
                Map(p => p.description).Index(6);
            }
        }

        public Tuple<bool, List<object>> UploadValidate(List<ProductViewModel> Data, List<KeyValuePair<string, StringValues>> Body)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;
            Currency currency = null;
            Uom uom = null;

            foreach (ProductViewModel productVM in Data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(productVM.code))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != productVM && d.code.Equals(productVM.code)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                }

                if (string.IsNullOrWhiteSpace(productVM.name))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != productVM && d.name.Equals(productVM.name)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh duplikat, ");
                }

                if(string.IsNullOrWhiteSpace(productVM.uom.unit))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Satuan tidak boleh kosong, ");
                }

                if (string.IsNullOrWhiteSpace(productVM.currency.code))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Mata Uang tidak boleh kosong, ");
                }

                decimal Price = 0;
                if (string.IsNullOrWhiteSpace(productVM.price))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga tidak boleh kosong, ");
                }
                else if (!decimal.TryParse(productVM.price, out Price))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga harus numerik, ");
                }
                else if (Price < 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga harus lebih besar dari 0, ");
                }
                else
                {
                    string[] PriceSplit = productVM.price.Split('.');
                    if (PriceSplit.Count().Equals(2) && PriceSplit[1].Length > 2)
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Harga maksimal memiliki 2 digit dibelakang koma, ");
                    }
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    /* Service Validation */
                    currency = this.DbContext.Set<Currency>().FirstOrDefault(d => d._IsDeleted.Equals(false) && d.Code.Equals(productVM.currency.code));
                    uom = this.DbContext.Set<Uom>().FirstOrDefault(d => d._IsDeleted.Equals(false) && d.Unit.Equals(productVM.uom.unit));

                    if (this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Code.Equals(productVM.code)))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                    }

                    if (this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Name.Equals(productVM.name)))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh duplikat, ");
                    }

                    if (currency == null)
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Mata Uang tidak terdaftar dalam master Mata Uang, ");
                    }

                    if (uom == null)
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Satuan tidak terdaftar dalam master Satuan, ");
                    }
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    productVM.price = Price;
                    productVM.currency._id = currency.Id;
                    productVM.uom._id = uom.Id;
                }
                else
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;

                    Error.Add("Kode Barang", productVM.code);
                    Error.Add("Nama Barang", productVM.name);
                    Error.Add("Satuan", productVM.uom.unit);
                    Error.Add("Mata Uang", productVM.currency.code);
                    Error.Add("Harga", productVM.price);
                    Error.Add("Tags", productVM.tags);
                    Error.Add("Keterangan", productVM.description);
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