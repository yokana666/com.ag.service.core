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
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class ProductService : BasicService<CoreDbContext, Product>, IBasicUploadCsvService<ProductViewModel>, IMap<Product, ProductViewModel>
    {
        private const string UserAgent = "core-product-service";

        public ProductService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Tuple<List<Product>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null,string Filter="{}")
        {
            IQueryable<Product> Query = this.DbContext.Products;
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
                "Id", "Code", "Name", "UOM", "Currency", "Price", "Tags", "_LastModifiedUtc"
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
                    CurrencySymbol = p.CurrencySymbol,
                    Price = p.Price,
                    Tags = p.Tags,
                    _LastModifiedUtc = p._LastModifiedUtc
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
            Pageable<Product> pageable = new Pageable<Product>(Query, Page - 1, Size);
            List<Product> Data = pageable.Data.ToList<Product>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public ProductViewModel MapToViewModel(Product product)
        {
            ProductViewModel productVM = new ProductViewModel
            {
                Id = product.Id,
                _IsDeleted = product._IsDeleted,
                Active = product.Active,
                _CreatedUtc = product._CreatedUtc,
                _CreatedBy = product._CreatedBy,
                _CreatedAgent = product._CreatedAgent,
                _LastModifiedUtc = product._LastModifiedUtc,
                _LastModifiedBy = product._LastModifiedBy,
                _LastModifiedAgent = product._LastModifiedAgent,
                Code = product.Code,
                Name = product.Name,
                Price = product.Price,
                Currency = new ProductCurrencyViewModel
                {
                    Id = product.CurrencyId,
                    Code = product.CurrencyCode,
                    Symbol = product.CurrencySymbol
                },
                Description = product.Description,
                UOM = new ProductUomViewModel
                {
                    Id = product.UomId,
                    Unit = product.UomUnit
                },
                Tags = product.Tags
            };

            return productVM;
        }

        public Product MapToModel(ProductViewModel productVM)
        {
            Product product = new Product
            {
                Id = productVM.Id,
                _IsDeleted = productVM._IsDeleted,
                Active = productVM.Active,
                _CreatedUtc = productVM._CreatedUtc,
                _CreatedBy = productVM._CreatedBy,
                _CreatedAgent = productVM._CreatedAgent,
                _LastModifiedUtc = productVM._LastModifiedUtc,
                _LastModifiedBy = productVM._LastModifiedBy,
                _LastModifiedAgent = productVM._LastModifiedAgent,
                Code = productVM.Code,
                Name = productVM.Name,
                Price = !Equals(productVM.Price, null) ? Convert.ToDecimal(productVM.Price) : 0 /* Check Null */
            };

            if (!Equals(productVM.Currency, null))
            {
                product.CurrencyId = productVM.Currency.Id;
                product.CurrencyCode = productVM.Currency.Code;
                product.CurrencySymbol = productVM.Currency.Symbol;
            }
            else
            {
                product.CurrencyId = null;
                product.CurrencyCode = null;
            }

            product.Description = productVM.Description;

            if (!Equals(productVM.UOM, null))
            {
                product.UomId = productVM.UOM.Id;
                product.UomUnit = productVM.UOM.Unit;
            }
            else
            {
                product.UomId = null;
                product.UomUnit = null;
            }

            product.Tags = productVM.Tags;

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
                Map(p => p.Code).Index(0);
                Map(p => p.Name).Index(1);
                Map(p => p.UOM.Unit).Index(2);
                Map(p => p.Currency.Code).Index(3);
                Map(p => p.Price).Index(4).TypeConverter<StringConverter>();
                Map(p => p.Tags).Index(5);
                Map(p => p.Description).Index(6);
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

                if (string.IsNullOrWhiteSpace(productVM.Code))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != productVM && d.Code.Equals(productVM.Code)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                }

                if (string.IsNullOrWhiteSpace(productVM.Name))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != productVM && d.Name.Equals(productVM.Name)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh duplikat, ");
                }

                if (string.IsNullOrWhiteSpace(productVM.UOM.Unit))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Satuan tidak boleh kosong, ");
                }

                if (string.IsNullOrWhiteSpace(productVM.Currency.Code))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Mata Uang tidak boleh kosong, ");
                }

                decimal Price = 0;
                if (string.IsNullOrWhiteSpace(productVM.Price))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga tidak boleh kosong, ");
                }
                else if (!decimal.TryParse(productVM.Price, out Price))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga harus numerik, ");
                }
                else if (Price < 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga harus lebih besar dari 0, ");
                }
                else
                {
                    string[] PriceSplit = productVM.Price.Split('.');
                    if (PriceSplit.Count().Equals(2) && PriceSplit[1].Length > 2)
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Harga maksimal memiliki 2 digit dibelakang koma, ");
                    }
                }

                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    /* Service Validation */
                    currency = this.DbContext.Set<Currency>().FirstOrDefault(d => d._IsDeleted.Equals(false) && d.Code.Equals(productVM.Currency.Code));
                    uom = this.DbContext.Set<Uom>().FirstOrDefault(d => d._IsDeleted.Equals(false) && d.Unit.Equals(productVM.UOM.Unit));

                    if (this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Code.Equals(productVM.Code)))
                    {
                        ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                    }

                    if (this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Name.Equals(productVM.Name)))
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
                    productVM.Price = Price;
                    productVM.Currency.Id = currency.Id;
                    productVM.UOM.Id = uom.Id;
                }
                else
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;

                    Error.Add("Kode Barang", productVM.Code);
                    Error.Add("Nama Barang", productVM.Name);
                    Error.Add("Satuan", productVM.UOM.Unit);
                    Error.Add("Mata Uang", productVM.Currency.Code);
                    Error.Add("Harga", productVM.Price);
                    Error.Add("Tags", productVM.Tags);
                    Error.Add("Keterangan", productVM.Description);
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

        public List<Product> GetByIds(List<string> ids)
        {
            return this.DbSet.Where(p => ids.Contains(p.Id.ToString()) && p._IsDeleted==false)
                .ToList();
        }

        public async Task<bool> CreateProduct(PackingModel packings)
        {
            var productNames = (from packingDetail in packings.PackingDetails
                                select string.Format("{0}/{1}/{2}/{3}/{4}/{5}", packings.ProductionOrderNo, packings.ColorName, packings.Construction,
                                packingDetail.Lot, packingDetail.Grade, packingDetail.Length) +
                                (string.IsNullOrWhiteSpace(packingDetail.Remark) ? "" : string.Format("/{0}", packingDetail.Remark)))
                                .Except((from product in DbSet where product._IsDeleted == false select product.Name));
            if (productNames.Count() > 0)
            {
                var uomId = (from uom in DbContext.UnitOfMeasurements
                             where uom.Unit == packings.PackingUom && uom._IsDeleted == false
                             select uom.Id).FirstOrDefault();
                if (uomId == 0)
                {
                    Uom uom = new Uom
                    {
                        Active = true,
                        Unit = packings.PackingUom,
                        _IsDeleted = false,
                        _CreatedBy = this.Username,
                        _CreatedUtc = DateTimeOffset.Now.DateTime,
                        _CreatedAgent = UserAgent
                    };
                    await DbContext.UnitOfMeasurements.AddAsync(uom);
                    await DbContext.SaveChangesAsync();

                    uomId = uom.Id;
                }
                var tags = string.Format("sales contract #{0}", packings.SalesContractNo);
                CodeGenerator codeGenerator = new CodeGenerator();
                IEnumerable<Product> products = from packingDetail in packings.PackingDetails
                                                where productNames.Contains(string.Format("{0}/{1}/{2}/{3}/{4}/{5}", packings.ProductionOrderNo, packings.ColorName, packings.Construction,
                                                    packingDetail.Lot, packingDetail.Grade, packingDetail.Length) +
                                                    (string.IsNullOrWhiteSpace(packingDetail.Remark) ? "" : string.Format("/{0}", packingDetail.Remark)))
                                                select new Product
                                                {
                                                    Active = true,
                                                    Code = codeGenerator.GenerateCode(),
                                                    Name = string.Format("{0}/{1}/{2}/{3}/{4}/{5}", packings.ProductionOrderNo, packings.ColorName, packings.Construction,
                                                        packingDetail.Lot, packingDetail.Grade, packingDetail.Length) +
                                                        (string.IsNullOrWhiteSpace(packingDetail.Remark) ? "" : string.Format("/{0}", packingDetail.Remark)),
                                                    UomId = uomId,
                                                    UomUnit = packings.PackingUom,
                                                    Tags = tags,
                                                    ProductionOrderId = packings.ProductionOrderId,
                                                    ProductionOrderNo = packings.ProductionOrderNo,
                                                    DesignCode = packings.DesignCode,
                                                    DesignNumber = packings.DesignNumber,
                                                    OrderTypeId = packings.OrderTypeId,
                                                    OrderTypeCode = packings.OrderTypeCode,
                                                    OrderTypeName = packings.OrderTypeName,
                                                    BuyerId = packings.BuyerId,
                                                    BuyerName = packings.BuyerName,
                                                    BuyerAddress = packings.BuyerAddress,
                                                    ColorName = packings.ColorName,
                                                    Construction = packings.Construction,
                                                    Lot = packingDetail.Lot,
                                                    Grade = packingDetail.Grade,
                                                    Weight = packingDetail.Weight,
                                                    Length = packingDetail.Length,
                                                    _IsDeleted = false,
                                                    _CreatedBy = this.Username,
                                                    _CreatedUtc = DateTimeOffset.Now.DateTime,
                                                    _CreatedAgent = UserAgent
                                                };
                await DbContext.AddRangeAsync(products);
                var rowAffected = await DbContext.SaveChangesAsync();
                if(rowAffected > 0)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }
    }
}