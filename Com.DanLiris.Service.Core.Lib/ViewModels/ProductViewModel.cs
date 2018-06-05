using System;

namespace Com.DanLiris.Service.Core.Lib.ViewModels
{
    public class ProductViewModel
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

        public dynamic price { get; set; }

        public ProductCurrencyViewModel currency { get; set; }

        public string description { get; set; }

        public ProductUomViewModel uom { get; set; }

        public string tags { get; set; }
    }

    public class ProductCurrencyViewModel
    {
        public int? _id { get; set; }

        public string code { get; set; }
    }

    public class ProductUomViewModel
    {
        public int? _id { get; set; }

        public string unit { get; set; }
    }
}
