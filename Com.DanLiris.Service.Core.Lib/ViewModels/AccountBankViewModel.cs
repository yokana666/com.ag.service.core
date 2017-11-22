using System;

namespace Com.DanLiris.Service.Core.Lib.ViewModels
{
    public class AccountBankViewModel
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

        public string bankName { get; set; }

        public string bankAddress { get; set; }

        public string accountName { get; set; }

        public string accountNumber { get; set; }

        public string swiftCode { get; set; }

        public AccountBankCurrencyViewModel currency { get; set; }
    }

    public class AccountBankCurrencyViewModel
    {
        public int? _id { get; set; }

        public string code { get; set; }
    }
}
