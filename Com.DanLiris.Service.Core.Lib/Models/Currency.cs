using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class Currency : StandardEntity, IValidatableObject
    {
        public string Code { get; set; }

        public string Symbol { get; set; }

        public float Rate { get; set; }

        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Code))
                yield return new ValidationResult("Code is required", new List<string> { "Code" });
                
            if (string.IsNullOrWhiteSpace(this.Symbol))
                yield return new ValidationResult("Symbol is required", new List<string> { "Symbol" });

            if (this.Rate <= 0)
                yield return new ValidationResult("Rate must be greater than zero", new List<string> { "Symbol" });
        }
    }
}
