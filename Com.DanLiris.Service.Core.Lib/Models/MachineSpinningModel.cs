using Com.Moonlay.Models;

namespace Com.DanLiris.Service.Core.Lib.Models
{
    public class MachineSpinningModel : StandardEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Condition { get; set; }
        public string Type { get; set; }
        public int Delivery { get; set; }
        public double CapacityPerHour { get; set; }
    }
}
