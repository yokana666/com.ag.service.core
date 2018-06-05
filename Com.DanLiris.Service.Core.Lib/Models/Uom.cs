using Com.DanLiris.Service.Core.Lib.Services;
using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class Uom : StandardEntity, IValidatableObject
    {
        [StringLength(500)]
        public string Unit { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Unit))
                validationResult.Add(new ValidationResult("Unit is required", new List<string> { "unit" }));

            if (validationResult.Count.Equals(0))
            {
                /* Service Validation */
                UomService service = (UomService)validationContext.GetService(typeof(UomService));

                if (service.DbContext.Set<Uom>().Count(r => r._IsDeleted.Equals(false) && r.Id != this.Id && r.Unit.Equals(this.Unit)) > 0) /* Unit Unique */
                    validationResult.Add(new ValidationResult("Unit already exists", new List<string> { "unit" }));
            }

            return validationResult;
        }
    }
}
