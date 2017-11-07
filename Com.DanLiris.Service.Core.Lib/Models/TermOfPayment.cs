using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class TermOfPayment : StandardEntity, IValidatableObject
    {
        public string MongoId { get; set; }

        public string Code { get; set; }

        public string TOP { get; set; }

        public bool IsExport { get; set; }     

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.TOP))
                yield return new ValidationResult("Term of Payment is required", new List<string> { "TermOfPayment" });
        }
    }
}
