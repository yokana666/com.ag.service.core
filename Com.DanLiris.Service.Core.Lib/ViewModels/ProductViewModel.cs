using Com.DanLiris.Service.Core.Lib.Helpers;
using System;

namespace Com.DanLiris.Service.Core.Lib.ViewModels
{
    public class ProductViewModel : BasicViewModel
    {

        public string Code { get; set; }

        public string Name { get; set; }

        public dynamic Price { get; set; }

        public ProductCurrencyViewModel Currency { get; set; }

        public string Description { get; set; }

        public ProductUomViewModel UOM { get; set; }

        public string Tags { get; set; }
    }

    public class ProductCurrencyViewModel
    {
        public int? Id { get; set; }

        public string Code { get; set; }
    }

    public class ProductUomViewModel
    {
        public int? Id { get; set; }

        public string Unit { get; set; }
    }
}
