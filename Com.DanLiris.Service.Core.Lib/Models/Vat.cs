using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class Vat : StandardEntity, IValidatableObject
    {
        public string MongoId { get; set; }

        public string Name { get; set; }

        public float Rate { get; set; }

        public string Description { get; set; }        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Name is required", new List<string> { "Name" });

            if (this.Rate < 0)
                yield return new ValidationResult("Rate must be greater than zero", new List<string> { "Rate" });
        }
    }
}
