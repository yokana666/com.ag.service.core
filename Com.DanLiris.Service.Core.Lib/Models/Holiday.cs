using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class Holiday : StandardEntity, IValidatableObject
    {
        public string MongoId { get; set; }

        public string Code { get; set; }
        
        public DateTime Date { get; set; }

        public string Name { get; set; }

        public int DivisionId { get; set; }

        public Division Division { get; set; }

        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Name is required", new List<string> { "Name" });
        }
    }
}
