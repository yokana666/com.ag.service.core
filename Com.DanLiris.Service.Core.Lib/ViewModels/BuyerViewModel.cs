using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DanLiris.Service.Core.Lib.ViewModels
{
    public class BuyerViewModel
    {
        public int _id { get; set; }

        public bool _deleted { get; set; }

        public bool _active { get; set; }

        public DateTime _createdDate { get; set; }

        public string _createdBy { get; set; }

        public string _createAgent { get; set; }

        public DateTime _updatedDate { get; set; }

        public string _updatedBy { get; set; }

        public string _updateAgent { get; set; }

        public string code { get; set; }
        
        public string name { get; set; }
        
        public string address { get; set; }
        
        public string city { get; set; }
        
        public string country { get; set; }
        
        public string contact { get; set; }

        public int? tempo { get; set; }
        
        public string type { get; set; }
        
        public string NPWP { get; set; }
    }
}
