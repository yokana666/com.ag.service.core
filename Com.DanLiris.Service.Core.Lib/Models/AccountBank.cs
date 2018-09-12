using Com.DanLiris.Service.Core.Lib.Helpers;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class AccountBank : StandardEntity, IValidatableObject
    {
        [MaxLength(255)]
        public string UId { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        [StringLength(500)]
        public string BankName { get; set; }

        public string BankAddress { get; set; }

        [StringLength(500)]
        public string AccountName { get; set; }

        [StringLength(100)]
        public string AccountNumber { get; set; }

        [StringLength(100)]
        public string SwiftCode { get; set; }
        
        public int? CurrencyId { get; set; }

        public string CurrencyCode { get; set; }

        public string CurrencySymbol { get; set; }

        /* Double */
        public double CurrencyRate { get; set; }

        public string CurrencyDescription { get; set; }

        //Division
        public string DivisionName { get; set; }
        public string DivisionCode { get; set; }
        public int? DivisionId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.BankName))
                validationResult.Add(new ValidationResult("Bank Name is required", new List<string> { "BankName" }));

            if (string.IsNullOrWhiteSpace(this.AccountName))
                validationResult.Add(new ValidationResult("Account Name is required", new List<string> { "AccountName" }));

            if (string.IsNullOrWhiteSpace(this.AccountNumber))
                validationResult.Add(new ValidationResult("Account Number is required", new List<string> { "AccountNumber" }));

            if(CurrencyId.Equals(null))
                validationResult.Add(new ValidationResult("Currency is required", new List<string> { "CurrencyID" }));

            if (validationResult.Count.Equals(0))
            {
                /* Service Validation */
                AccountBankService service = (AccountBankService)validationContext.GetService(typeof(AccountBankService));

                if (service.DbContext.Set<AccountBank>().Count(r => r._IsDeleted.Equals(false) && r.Id != this.Id && r.BankName.Equals(this.BankName) && r.AccountNumber.Equals(this.AccountNumber)) > 0) /* Unique */
                {
                    validationResult.Add(new ValidationResult("Bank Name and Account Number already exists", new List<string> { "BankName" }));
                    validationResult.Add(new ValidationResult("Bank Name and Account Number already exists", new List<string> { "AccountNumber" }));
                }
            }

            return validationResult;
        }
    }
}
