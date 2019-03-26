namespace Com.DanLiris.Service.Core.Lib.MongoModels
{
    public class MongoCurrencyModel : MongoBaseModel
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public double rate { get; set; }
        public string description { get; set; }
    }
}