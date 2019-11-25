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

        [StringLength(32)]
        public string Type { get; set; }
        [StringLength(256)]
        public string YarnType1 { get; set; }
        [StringLength(256)]
        public string YarnType2 { get; set; }
        [StringLength(128)]
        public string WovenType { get; set; }
        [StringLength(128)]
        public string Construction { get; set; }
        [StringLength(32)]
        public string Width { get; set; }
        [StringLength(32)]
        public string Grade { get; set; }
        [StringLength(128)]
        public string Lot { get; set; }
        [StringLength(128)]
        public string Composition { get; set; }
        [StringLength(128)]
        public string Design { get; set; }


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

            if (!string.IsNullOrWhiteSpace(Type))
            {
                switch (Type.ToUpper())
                {
                    case "YARN":
                        if (string.IsNullOrWhiteSpace(YarnType1))
                            validationResult.Add(new ValidationResult("Jenis Benang harus diisi", new List<string> { "YarnType1" }));

                        if (string.IsNullOrWhiteSpace(Lot))
                            validationResult.Add(new ValidationResult("Nomor Lot harus diisi", new List<string> { "Lot" }));

                        break;
                    case "GREIGE":
                        if (string.IsNullOrWhiteSpace(WovenType))
                            validationResult.Add(new ValidationResult("Jenis Anyaman harus diisi", new List<string> { "WovenType" }));

                        if (string.IsNullOrWhiteSpace(Construction))
                            validationResult.Add(new ValidationResult("Konstruksi harus diisi", new List<string> { "Construction" }));

                        if (string.IsNullOrWhiteSpace(Width))
                            validationResult.Add(new ValidationResult("Lebar harus diisi", new List<string> { "Width" }));

                        if (string.IsNullOrWhiteSpace(YarnType1))
                            validationResult.Add(new ValidationResult("Jenis Benang 1 harus diisi", new List<string> { "YarnType1" }));

                        if (string.IsNullOrWhiteSpace(YarnType2))
                            validationResult.Add(new ValidationResult("Jenis Benang 2 harus diisi", new List<string> { "YarnType2" }));

                        if (string.IsNullOrWhiteSpace(Grade))
                            validationResult.Add(new ValidationResult("Grade harus diisi", new List<string> { "Grade" }));

                        break;
                    case "FABRIC":
                        if (string.IsNullOrWhiteSpace(Composition))
                            validationResult.Add(new ValidationResult("Komposisi harus diisi", new List<string> { "Composition" }));

                        if (string.IsNullOrWhiteSpace(Construction))
                            validationResult.Add(new ValidationResult("Konstruksi harus diisi", new List<string> { "Construction" }));

                        if (string.IsNullOrWhiteSpace(Width))
                            validationResult.Add(new ValidationResult("Lebar harus diisi", new List<string> { "Width" }));

                        if (string.IsNullOrWhiteSpace(Design))
                            validationResult.Add(new ValidationResult("Design harus diisi", new List<string> { "Design" }));

                        if (string.IsNullOrWhiteSpace(Grade))
                            validationResult.Add(new ValidationResult("Grade harus diisi", new List<string> { "Grade" }));

                        break;
                    default:
                        break;
                }
            }

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
