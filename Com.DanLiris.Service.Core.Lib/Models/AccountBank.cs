using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class AccountBank : StandardEntity, IValidatableObject
    {
        public string MongoId { get; set; }

        public string Code { get; set; }

        public string BankName { get; set; }

        public string BankAddress { get; set; }        

        public string AccountName { get; set; }
        
        public string AccountNumber { get; set; }
        
        public string SwiftCode { get; set; }
        
        public int CurrencyId { get; set; }

        public Currency Currency { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.BankName))
                yield return new ValidationResult("Code is required", new List<string> { "Code" });

            if (string.IsNullOrWhiteSpace(this.AccountName))
                yield return new ValidationResult("Name is required", new List<string> { "Name" });
        }
    }
}
