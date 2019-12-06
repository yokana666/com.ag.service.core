using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Core.Lib.ViewModels
{
    public class StorageByNameViewModel
    {
        public string Code { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public Unit Unit { get; set; }
    }

    public class Unit
    {
        public string Name { get; set; }
        public Division Division { get; set; }
    }

    public class Division
    {
        public string Name { get; set; }
    }
}
