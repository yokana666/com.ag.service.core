using Com.DanLiris.Service.Core.Lib.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.ViewModels
{
    public class MachineSpinningProcessTypeViewModel : BasicViewModel, IValidatableObject
    {
        public string Type { get; set; }

        public int MachineSpinningId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
