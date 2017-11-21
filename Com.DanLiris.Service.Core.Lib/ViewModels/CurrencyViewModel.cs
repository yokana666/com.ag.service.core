using System;

namespace Com.DanLiris.Service.Core.Lib.ViewModels
{
    public class CurrencyViewModel
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

        public string symbol { get; set; }

        /* Double */
        public dynamic rate { get; set; }

        public string description { get; set; }
    }
}
