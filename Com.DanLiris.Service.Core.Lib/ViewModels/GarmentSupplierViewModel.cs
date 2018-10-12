using Com.DanLiris.Service.Core.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.ViewModels
{
    public class GarmentSupplierViewModel : BasicViewModel
	{
		public int _id { get; set; }

		public bool _deleted { get; set; }

		public bool _active { get; set; }

		public DateTime _createdDate { get; set; }

		public string _createdBy { get; set; }

		public string _createAgent { get; set; }

		public DateTime _updatedDate { get; set; }

		public string _updatedBy { get; set; }

		public string _updateAgent { get; set; }

		public string code { get; set; }

		public string name { get; set; }

		public string address { get; set; }

		public string contact { get; set; }

		public string PIC { get; set; }

		/* Bool */
		public dynamic import { get; set; }
		public dynamic usevat { get; set; }
		public string NPWP { get; set; }

		public string serialNumber { get; set; }

		public dynamic usetax { get; set; }

		public IncomeTaxViewModel IncomeTaxes { get; set; }
	}
}
