using Com.DanLiris.Service.Core.Lib.Helpers;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class SizeModel : StandardEntity, IValidatableObject
    {
        [MaxLength(255)]
        public string UId { get; set; }
        public string Size { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            SizeService service = validationContext.GetService<SizeService>();

            if (string.IsNullOrWhiteSpace(this.Size))
            {
                yield return new ValidationResult("Size harus diisi", new List<string> { "Size" });
            }
            else
            {
                if (service.DbSet.Count(r => r.Id != this.Id && r.Size.Equals(this.Size) && r._IsDeleted.Equals(false)) > 0)
                    yield return new ValidationResult("Size sudah ada", new List<string> { "Size" });
            }

        }
    }
}