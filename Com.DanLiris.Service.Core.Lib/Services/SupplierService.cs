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

namespace Com.DanLiris.Service.Core.Lib.Services
{
    public class SupplierService : StandardEntityService<CoreDbContext, Supplier>, IGeneralService<Supplier, SupplierViewModel>, IGeneralUploadService<Supplier>
    {
        public SupplierService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public Tuple<List<Supplier>, int, Dictionary<string, string>, List<string>> Read(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null)
        {
            IQueryable<Supplier> Query = this.DbContext.Suppliers;
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
                    "_id", "code", "name", "address", "import", "NPWP"
                };

            Query = Query
                .Select(s => new Supplier
                {
                    Id = s.Id,
                    Code = s.Code,
                    Name = s.Name,
                    Address = s.Address,
                    Import = s.Import,
                    NPWP = s.NPWP
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
            Pageable<Supplier> pageable = new Pageable<Supplier>(Query, Page - 1, Size);
            List<Supplier> Data = pageable.Data.ToList<Supplier>();

            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public SupplierViewModel MapToViewModel(Supplier supplier)
        {
            SupplierViewModel supplierVM = new SupplierViewModel();

            supplierVM._id = supplier.Id;
            supplierVM._deleted = supplier._IsDeleted;
            supplierVM._active = supplier.Active;
            supplierVM._createdDate = supplier._CreatedUtc;
            supplierVM._createdBy = supplier._CreatedBy;
            supplierVM._createAgent = supplier._CreatedAgent;
            supplierVM._updatedDate = supplier._LastModifiedUtc;
            supplierVM._updatedBy = supplier._LastModifiedBy;
            supplierVM._updateAgent = supplier._LastModifiedAgent;
            supplierVM.code = supplier.Code;
            supplierVM.name = supplier.Name;
            supplierVM.address = supplier.Address;
            supplierVM.contact = supplier.Contact;
            supplierVM.PIC = supplier.PIC;
            supplierVM.import = supplier.Import;
            supplierVM.NPWP = supplier.NPWP;
            supplierVM.serialNumber = supplier.SerialNumber;

            return supplierVM;
        }

        public Supplier MapToModel(SupplierViewModel supplierVM)
        {
            Supplier supplier = new Supplier();

            supplier.Id = supplierVM._id;
            supplier._IsDeleted = supplierVM._deleted;
            supplier.Active = supplierVM._active;
            supplier._CreatedUtc = supplierVM._createdDate;
            supplier._CreatedBy = supplierVM._createdBy;
            supplier._CreatedAgent = supplierVM._createAgent;
            supplier._LastModifiedUtc = supplierVM._updatedDate;
            supplier._LastModifiedBy = supplierVM._updatedBy;
            supplier._LastModifiedAgent = supplierVM._updateAgent;
            supplier.Code = supplierVM.code;
            supplier.Name = supplierVM.name;
            supplier.Address = supplierVM.address;
            supplier.Contact = supplierVM.contact;
            supplier.PIC = supplierVM.PIC;
            supplier.Import = supplierVM.import;
            supplier.NPWP = supplierVM.NPWP;
            supplier.SerialNumber = supplierVM.serialNumber;

            return supplier;
        }

        /* Upload CSV */
        private readonly List<string> Header = new List<string>()
        {
            "Kode", "Nama Supplier", "Alamat", "Kontak", "PIC", "Import", "NPWP", "Serial Number"
        };

        public List<string> CsvHeader => Header;

        public sealed class SupplierMap : ClassMap<Supplier>
        {
            public SupplierMap()
            {
                Map(s => s.Code).Name("Kode");
                Map(s => s.Name).Name("Nama Supplier");
                Map(s => s.Address).Name("Alamat");
                Map(s => s.Contact).Name("Kontak");
                Map(s => s.PIC).Name("PIC");
                Map(s => s.Import).Name("Import");
                Map(s => s.NPWP).Name("NPWP");
                Map(s => s.SerialNumber).Name("Serial Number");
            }
        }

        public Tuple<bool, List<object>> UploadValidate(List<Supplier> Data)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;

            foreach (Supplier supplier in Data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(supplier.Code))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh kosong, ");
                }
                else if (Data.Any(d => d != supplier && d.Code.Equals(supplier.Code)) || this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Code.Equals(supplier.Code)))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
                }

                if (string.IsNullOrWhiteSpace(supplier.Name))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh kosong, ");
                }

                if (!string.IsNullOrEmpty(ErrorMessage)) /* Not Empty */
                {
                    var Error = new ExpandoObject() as IDictionary<string, object>;
                    
                    Error.Add("Kode", supplier.Code);
                    Error.Add("Nama Supplier", supplier.Name);
                    Error.Add("Alamat", supplier.Address);
                    Error.Add("Kontak", supplier.Contact);
                    Error.Add("PIC", supplier.PIC);
                    Error.Add("Import", supplier.Import);
                    Error.Add("NPWP", supplier.NPWP);
                    Error.Add("Serial Number", supplier.SerialNumber);
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