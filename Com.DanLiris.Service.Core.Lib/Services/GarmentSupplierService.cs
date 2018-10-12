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
	public class GarmentSupplierService : BasicService<CoreDbContext, GarmentSupplier>, IBasicUploadCsvService<GarmentSupplierViewModel>, IMap<GarmentSupplier, GarmentSupplierViewModel>
	{

		private readonly string[] ImportAllowed = { "True", "False" };
		private readonly string[] UseVatAllowed = { "True", "False" };
		private readonly string[] UseTaxAllowed = { "True", "False" };
		public GarmentSupplierService(IServiceProvider serviceProvider) : base(serviceProvider)
		{
		}
		public override Tuple<List<GarmentSupplier>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
		{
			IQueryable<GarmentSupplier> Query = this.DbContext.GarmentSuppliers;
			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
			Query = ConfigureFilter(Query, FilterDictionary);
			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
			/* Search With Keyword */
			if (Keyword != null)
			{
				List<string> SearchAttributes = new List<string>()
				{
					"Code", "Name","Address"
				};

				Query = Query.Where(General.BuildSearch(SearchAttributes), Keyword);
			}

			/* Const Select */
			List<string> SelectedFields = new List<string>()
			{
				"_id", "code", "name", "address", "import", "NPWP", "usevat", "usetax", "IncomeTaxes"
			};

			Query = Query
				.Select(s => new GarmentSupplier
				{
					Id = s.Id,
					Code = s.Code,
					Name = s.Name,
					Address = s.Address,
					Import = s.Import,
					NPWP = s.NPWP,
					UseVat = s.UseVat,
					UseTax = s.UseTax,
					IncomeTaxesId = s.IncomeTaxesId,
					IncomeTaxesName = s.IncomeTaxesName,
					IncomeTaxesRate = s.IncomeTaxesRate,
					_LastModifiedUtc =s._LastModifiedUtc
				}).OrderByDescending(b => b._LastModifiedUtc);

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

				Query = OrderType.Equals(General.ASCENDING ) ?
					Query.OrderBy(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b)) :
					Query.OrderByDescending(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b));
			}

			/* Pagination */
			Pageable<GarmentSupplier> pageable = new Pageable<GarmentSupplier>(Query , Page - 1, Size);
			List<GarmentSupplier> Data = pageable.Data.ToList<GarmentSupplier>();

			int TotalData = pageable.TotalCount;

			return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
		}
		public GarmentSupplierViewModel MapToViewModel(GarmentSupplier GarmentSupplier)
		{
			GarmentSupplierViewModel GarmentSupplierVM = new GarmentSupplierViewModel();

			GarmentSupplierVM._id = GarmentSupplier.Id;
			GarmentSupplierVM._deleted = GarmentSupplier._IsDeleted;
			GarmentSupplierVM._active = GarmentSupplier.Active;
			GarmentSupplierVM._createdDate = GarmentSupplier._CreatedUtc;
			GarmentSupplierVM._createdBy = GarmentSupplier._CreatedBy;
			GarmentSupplierVM._createAgent = GarmentSupplier._CreatedAgent;
			GarmentSupplierVM._updatedDate = GarmentSupplier._LastModifiedUtc;
			GarmentSupplierVM._updatedBy = GarmentSupplier._LastModifiedBy;
			GarmentSupplierVM._updateAgent = GarmentSupplier._LastModifiedAgent;
			GarmentSupplierVM.code = GarmentSupplier.Code;
			GarmentSupplierVM.name = GarmentSupplier.Name;
			GarmentSupplierVM.address = GarmentSupplier.Address;
			GarmentSupplierVM.contact = GarmentSupplier.Contact;
			GarmentSupplierVM.PIC = GarmentSupplier.PIC;
			GarmentSupplierVM.import = GarmentSupplier.Import;
			GarmentSupplierVM.usevat = GarmentSupplier.UseVat;
			GarmentSupplierVM.NPWP = GarmentSupplier.NPWP;
			GarmentSupplierVM.serialNumber = GarmentSupplier.SerialNumber;
			GarmentSupplierVM.usetax = GarmentSupplier.UseTax;

			GarmentSupplierVM.IncomeTaxes = new IncomeTaxViewModel
			{
				_id = (int)GarmentSupplier.IncomeTaxesId,
				name = GarmentSupplier.IncomeTaxesName,
				rate = GarmentSupplier.IncomeTaxesRate
			};

			return GarmentSupplierVM;
		}
		public GarmentSupplier MapToModel(GarmentSupplierViewModel GarmentSupplierVM)
		{
			GarmentSupplier GarmentSupplier = new GarmentSupplier();

			GarmentSupplier.Id = GarmentSupplierVM._id;
			GarmentSupplier._IsDeleted = GarmentSupplierVM._deleted;
			GarmentSupplier.Active = GarmentSupplierVM._active;
			GarmentSupplier._CreatedUtc = GarmentSupplierVM._createdDate;
			GarmentSupplier._CreatedBy = GarmentSupplierVM._createdBy;
			GarmentSupplier._CreatedAgent = GarmentSupplierVM._createAgent;
			GarmentSupplier._LastModifiedUtc = GarmentSupplierVM._updatedDate;
			GarmentSupplier._LastModifiedBy = GarmentSupplierVM._updatedBy;
			GarmentSupplier._LastModifiedAgent = GarmentSupplierVM._updateAgent;
			GarmentSupplier.Code = GarmentSupplierVM.code;
			GarmentSupplier.Name = GarmentSupplierVM.name;
			GarmentSupplier.Address = GarmentSupplierVM.address;
			GarmentSupplier.Contact = GarmentSupplierVM.contact;
			GarmentSupplier.PIC = GarmentSupplierVM.PIC;
			GarmentSupplier.Import = !Equals(GarmentSupplierVM.import, null) ? Convert.ToBoolean(GarmentSupplierVM.import) : null;
			GarmentSupplier.UseVat = !Equals(GarmentSupplierVM.usevat, null) ? Convert.ToBoolean(GarmentSupplierVM.usevat) : null;
			GarmentSupplier.UseTax = !Equals(GarmentSupplierVM.usetax, null) ? Convert.ToBoolean(GarmentSupplierVM.usetax) : null; /* Check Null */
			GarmentSupplier.NPWP = GarmentSupplierVM.NPWP;
			GarmentSupplier.SerialNumber = GarmentSupplierVM.serialNumber;
			if (GarmentSupplierVM.IncomeTaxes != null)
			{
				GarmentSupplier.IncomeTaxesId = GarmentSupplierVM.IncomeTaxes._id;
				GarmentSupplier.IncomeTaxesName = GarmentSupplierVM.IncomeTaxes.name;
				GarmentSupplier.IncomeTaxesRate = GarmentSupplierVM.IncomeTaxes.rate;
			}
			else
			{
				GarmentSupplier.IncomeTaxesId = 0;
				GarmentSupplier.IncomeTaxesName = "";
				GarmentSupplier.IncomeTaxesRate = 0;
			}

			return GarmentSupplier;
		}

		/* Upload CSV */
		private readonly List<string> Header = new List<string>()
		{
			"Kode", "Nama Supplier", "Alamat", "Kontak", "PIC", "Import","Kena PPN", "NPWP", "Serial Number", 
		};
		public List<string> CsvHeader => Header;

		public sealed class GarmentSupplierMap : ClassMap<GarmentSupplierViewModel>
		{
			public GarmentSupplierMap()
			{

				Map(s => s.code).Index(0);
				Map(s => s.name).Index(1);
				Map(s => s.address).Index(2);
				Map(s => s.contact).Index(3);
				Map(s => s.PIC).Index(4);
				Map(s => s.import).Index(5).TypeConverter<StringConverter>();
				Map(s => s.usevat ).Index(6).TypeConverter<StringConverter>();
				Map(s => s.NPWP).Index(7);
				Map(s => s.serialNumber).Index(8);
				Map(s => s.IncomeTaxes.rate).Index(9);
			}
		}

		public Tuple<bool, List<object>> UploadValidate(List<GarmentSupplierViewModel> Data, List<KeyValuePair<string, StringValues>> Body)
		{
			List<object> ErrorList = new List<object>();
			string ErrorMessage;
			bool Valid = true;
			IncomeTax incomeTax = null;

			foreach (GarmentSupplierViewModel GarmentSupplierVM in Data)
			{
				ErrorMessage = "";

				if (string.IsNullOrWhiteSpace(GarmentSupplierVM.code))
				{
					ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh kosong, ");
				}
				else if (Data.Any(d => d != GarmentSupplierVM && d.code.Equals(GarmentSupplierVM.code)))
				{
					ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
				}

				if (string.IsNullOrWhiteSpace(GarmentSupplierVM.name))
				{
					ErrorMessage = string.Concat(ErrorMessage, "Nama tidak boleh kosong, ");
				}

				if (string.IsNullOrWhiteSpace(GarmentSupplierVM.import))
				{
					ErrorMessage = string.Concat(ErrorMessage, "Import tidak boleh kosong, ");
				}
				else if (!ImportAllowed.Any(i => i.Equals(GarmentSupplierVM.import, StringComparison.CurrentCultureIgnoreCase)))
				{
					ErrorMessage = string.Concat(ErrorMessage, "Import harus diisi dengan True atau False, ");
				}
				if (string.IsNullOrWhiteSpace(GarmentSupplierVM.usevat))
				{
					ErrorMessage = string.Concat(ErrorMessage, "Kena PPN tidak boleh kosong, ");
				}
				else if (!UseVatAllowed.Any(i => i.Equals(GarmentSupplierVM.import, StringComparison.CurrentCultureIgnoreCase)))
				{
					ErrorMessage = string.Concat(ErrorMessage, "Kena PPN harus diisi dengan True atau False, ");
				}
				if (string.IsNullOrWhiteSpace(GarmentSupplierVM.IncomeTaxes.name))
				{
					ErrorMessage = string.Concat(ErrorMessage, "Kode PPH tidak boleh kosong, ");
				}
				else
				{
					IncomeTax supplier = DbContext.IncomeTaxes.FirstOrDefault(s => s.Name == GarmentSupplierVM.IncomeTaxes.name);
					if (supplier == null)
					{
						ErrorMessage = string.Concat(ErrorMessage, "Kode PPH tidak ada di master, ");
					}
					else
					{
						GarmentSupplierVM.IncomeTaxes._id = supplier.Id;
						GarmentSupplierVM.IncomeTaxes.name = supplier.Name;
					}
				}
			
				if (string.IsNullOrEmpty(ErrorMessage))
				{
					/* Service Validation */
					incomeTax = this.DbContext.Set<IncomeTax>().FirstOrDefault(d => d._IsDeleted.Equals(false) && d.Id.Equals(GarmentSupplierVM.IncomeTaxes._id));
					if (this.DbSet.Any(d => d._IsDeleted.Equals(false) && d.Code.Equals(GarmentSupplierVM.code)))
					{
						ErrorMessage = string.Concat(ErrorMessage, "Kode tidak boleh duplikat, ");
					}
					if (incomeTax==null)
					{
						ErrorMessage = string.Concat(ErrorMessage, "PPH tidak terdaftar dalam master Income Tax");
					}
				}

				if (string.IsNullOrEmpty(ErrorMessage))
				{
					GarmentSupplierVM.import = Convert.ToBoolean(GarmentSupplierVM.import);
				}
				else
				{
					ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
					var Error = new ExpandoObject() as IDictionary<string, object>;

					Error.Add("Kode", GarmentSupplierVM.code);
					Error.Add("Nama Supplier", GarmentSupplierVM.name);
					Error.Add("Alamat", GarmentSupplierVM.address);
					Error.Add("Kontak", GarmentSupplierVM.code);
					Error.Add("PIC", GarmentSupplierVM.PIC);
					Error.Add("Import", GarmentSupplierVM.import);
					Error.Add("Kena PPN", GarmentSupplierVM.usevat);
					Error.Add("Kena PPH", GarmentSupplierVM.usetax);
					Error.Add("PPH", GarmentSupplierVM.IncomeTaxes.name);
					Error.Add("NPWP", GarmentSupplierVM.NPWP);
					Error.Add("Serial Number", GarmentSupplierVM.serialNumber);
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
