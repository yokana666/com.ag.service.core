using Com.DanLiris.Service.Core.Lib.Helpers;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class Product : StandardEntity, IValidatableObject
    {
        [MaxLength(255)]
        public string UId { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int? CurrencyId { get; set; }

        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }

        public string Description { get; set; }

        public int? UomId { get; set; }

        [StringLength(500)]
        public string UomUnit { get; set; }

        [StringLength(500)]
        public string Tags { get; set; }

        #region Property
        public int ProductionOrderId { get; set; }
        [MaxLength(25)]
        public string ProductionOrderNo { get; set; }
        [MaxLength(250)]
        public string DesignCode { get; set; }
        [MaxLength(250)]
        public string DesignNumber { get; set; }

        public int OrderTypeId { get; set; }
        [MaxLength(25)]
        public string OrderTypeCode { get; set; }
        [MaxLength(25)]
        public string OrderTypeName { get; set; }

        public int BuyerId { get; set; }
        [MaxLength(250)]
        public string BuyerName { get; set; }
        [MaxLength(250)]
        public string BuyerAddress { get; set; }

        [MaxLength(250)]
        public string ColorName { get; set; }

        [MaxLength(300)]
        public string Construction { get; set; }

        [MaxLength(250)]
        public string Lot { get; set; }
        [MaxLength(100)]
        public string Grade { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        #endregion

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Code))
                validationResult.Add(new ValidationResult("Code is required", new List<string> { "Code" }));

            if (string.IsNullOrWhiteSpace(this.Name))
                validationResult.Add(new ValidationResult("Name is required", new List<string> { "Name" }));

            if(this.CurrencyId.Equals(null))
                validationResult.Add(new ValidationResult("Currency is required", new List<string> { "Currency" }));

            if (this.UomId.Equals(null))
                validationResult.Add(new ValidationResult("Uom is required", new List<string> { "Uom" }));

            if (validationResult.Count.Equals(0))
            {
                /* Service Validation */
                BudgetService service = (BudgetService)validationContext.GetService(typeof(BudgetService));

                if (service.DbContext.Set<Budget>().Count(r => r._IsDeleted.Equals(false) && r.Id != this.Id && r.Code.Equals(this.Code)) > 0) /* Code Unique */
                    validationResult.Add(new ValidationResult("Code already exists", new List<string> { "Code" }));
            }

            return validationResult;
        }
    }
}
