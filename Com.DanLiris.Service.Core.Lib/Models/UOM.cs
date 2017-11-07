using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class UOM : StandardEntity, IValidatableObject
    {
        public string MongoId { get; set; }

        public string Unit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Unit))
                yield return new ValidationResult("Unit is required", new List<string> { "Unit" });
        }
    }
}
