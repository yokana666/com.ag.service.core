using Com.DanLiris.Service.Core.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.DanLiris.Service.Core.Lib.ViewModels
{
    public class MachineSpinningViewModel : BasicViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public string Condition { get; set; }
        public string Type { get; set; }
        public int? Delivery { get; set; }
        public double? CapacityPerHour { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Nama harus diisi", new List<string> { "Name" });
            }

            if (Year == null || Year <= 0)
            {
                yield return new ValidationResult("Tahun harus diisi", new List<string> { "Year" });
            }

            if (string.IsNullOrWhiteSpace(Condition))
            {
                yield return new ValidationResult("Kondisi harus diisi", new List<string> { "Condition" });
            }

            if (string.IsNullOrWhiteSpace(Type))
            {
                yield return new ValidationResult("Tipe harus diisi", new List<string> { "Type" });
            }

            if (Delivery == null || Delivery <= 0)
            {
                yield return new ValidationResult("Delivery harus diisi", new List<string> { "Delivery" });
            }

            if (CapacityPerHour == null || CapacityPerHour <= 0)
            {
                yield return new ValidationResult("Kapasitas per Jam harus diisi", new List<string> { "CapacityPerHour" });
            }
        }
    }
}
