using Com.DanLiris.Service.Core.Lib.Helpers;
using System;

namespace Com.DanLiris.Service.Core.Lib.ViewModels
{
    public class CategoryViewModel : BasicViewModelOld
    {
        public string code { get; set; }

        public string name { get; set; }

        public string codeRequirement { get; set; }
    }
}
