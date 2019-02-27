using Com.DanLiris.Service.Core.Lib.Helpers;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class Storage : StandardEntity, IValidatableObject
    {
        [MaxLength(255)]
        public string UId { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? UnitId { get; set; }

        public string UnitName { get; set; }

        public string DivisionName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Name))
                validationResult.Add(new ValidationResult("Name is required", new List<string> { "name" }));

            if (this.UnitId.Equals(null))
                validationResult.Add(new ValidationResult("Unit is required", new List<string> { "unit" }));

            if (validationResult.Count.Equals(0))
            {
                /* Service Validation */
                StorageService service = (StorageService)validationContext.GetService(typeof(StorageService));

                if (service.DbContext.Set<Storage>().Count(r => r._IsDeleted.Equals(false) && r.Id != this.Id && r.Name.Equals(this.Name) && r.UnitId.Equals(this.UnitId)) > 0)
                {
                    validationResult.Add(new ValidationResult("Name and Unit already exists", new List<string> { "name" }));
                    validationResult.Add(new ValidationResult("Name and Unit already exists", new List<string> { "unit" }));
                }     
            }

            return validationResult;
        }
    }
}
