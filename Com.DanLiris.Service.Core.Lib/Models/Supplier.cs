using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class Supplier : StandardEntity, IValidatableObject
    {
        public string MongoId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Contact { get; set; }
        
        public string PIC { get; set; }
        
        public bool Import { get; set; }
        
        public string NPWP { get; set; }
        
        public string SerialNumber { get; set; }        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.Code))
                yield return new ValidationResult("Code is required", new List<string> { "Code" });
                
            if (string.IsNullOrWhiteSpace(this.Name))
                yield return new ValidationResult("Name is required", new List<string> { "Name" });
        }
    }
}
