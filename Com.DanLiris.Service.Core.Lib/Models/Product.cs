using Com.DanLiris.Service.Core.Lib.MongoModels;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.Moonlay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class Product : StandardEntity, IValidatableObject
    {
        public Product()
        {
        }

        public Product(MongoProductModel mongoProduct, Uom uom, Currency currency)
        {
            Active = mongoProduct._active;
            Code = mongoProduct.code;
            CurrencyCode = mongoProduct.currency.code;
            Description = mongoProduct.description;
            Name = mongoProduct.name;
            Price = mongoProduct.price;
            Tags = mongoProduct.tags;
            UId = mongoProduct._id.ToString();
            _CreatedAgent = mongoProduct._createAgent;
            _CreatedBy = mongoProduct._createdBy;
            _CreatedUtc = mongoProduct._createdDate;
            UomId = uom != null ? uom.Id : 0;
            UomUnit = uom != null ? uom.Unit : "";
            CurrencyId = currency != null ? currency.Id : 0;
            CurrencyCode = currency != null ? currency.Code : "";
            CurrencySymbol = currency != null ? currency.Symbol : "";
            _DeletedAgent = mongoProduct._updateAgent;
            _DeletedBy = mongoProduct._updatedBy;
            _DeletedUtc = mongoProduct._updatedDate;
            _IsDeleted = mongoProduct._deleted;
            _LastModifiedAgent = mongoProduct._updateAgent;
            _LastModifiedBy = mongoProduct._updatedBy;
            _LastModifiedUtc = mongoProduct._updatedDate;
        }

        [MaxLength(255)]
        public string UId { get; set; }

        [StringLength(100)]
        public string Code { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int? CurrencyId { get; set; }

        [StringLength(255)]
        public string CurrencyCode { get; set; }
        [StringLength(255)]
        public string CurrencySymbol { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int? UomId { get; set; }

        [StringLength(500)]
        public string UomUnit { get; set; }

        [StringLength(500)]
        public string Tags { get; set; }

        public ProductSPPProperty SPPProperties { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResult = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Code))
                validationResult.Add(new ValidationResult("Code is required", new List<string> { "code" }));

            if (string.IsNullOrWhiteSpace(this.Name))
                validationResult.Add(new ValidationResult("Name is required", new List<string> { "name" }));

            if (this.CurrencyId.Equals(null))
                validationResult.Add(new ValidationResult("Currency is required", new List<string> { "currency" }));

            if (this.UomId.Equals(null))
                validationResult.Add(new ValidationResult("Uom is required", new List<string> { "uom" }));

            if (validationResult.Count.Equals(0))
            {
                /* Service Validation */
                ProductService service = (ProductService)validationContext.GetService(typeof(ProductService));

                if (service.DbContext.Set<Product>().Count(r => r._IsDeleted.Equals(false) && r.Id != this.Id && r.Code.Equals(this.Code)) > 0) /* Code Unique */
                    validationResult.Add(new ValidationResult("Code already exists", new List<string> { "code" }));
            }

            return validationResult;
        }
    }
}
