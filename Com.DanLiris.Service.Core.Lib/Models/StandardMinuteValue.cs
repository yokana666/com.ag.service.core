using Com.Moonlay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class StandardMinuteValue : StandardEntity, IValidatableObject
    {
        public int BuyerId { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
        public int ComodityId { get; set; }
        public string ComodityCode { get; set; }
        public string ComodityName { get; set; }
        public DateTimeOffset SMVDate { get; set; }
        public decimal MinuteCutting { get; set; }
        public decimal MinuteSewing { get; set; }
        public decimal MinuteFinishing { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(this.BuyerName))
            {
                yield return new ValidationResult("Buyer Harus Diisi", new List<string> { "BuyerName" });
            }
            if (string.IsNullOrWhiteSpace(this.ComodityName))
            {
                yield return new ValidationResult("Komoditi harus diisi", new List<string> { "ComodityName" });
            }
            if (this.MinuteCutting <= 0)
            {
                yield return new ValidationResult("Cutting Harus Lebih dari 0", new List<string> { "MinuteCutting" });
            }
            if (this.MinuteSewing <= 0)
            {
                yield return new ValidationResult("Sewing Harus Lebih dari 0", new List<string> { "MinuteSewing" });
            }
            if (this.MinuteFinishing <= 0)
            {
                yield return new ValidationResult("Finishing Harus Lebih dari 0", new List<string> { "MinuteFinishing" });
            }
        }
    }
}
