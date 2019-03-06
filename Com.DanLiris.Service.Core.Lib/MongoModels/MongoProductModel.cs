using MongoDB.Bson;

namespace Com.DanLiris.Service.Core.Lib.MongoModels
{
    public class MongoProductModel : MongoBaseModel
    {
        public string code { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public MongoCurrencyModel currency { get; set; }
        public string description { get; set; }
        public ObjectId uomId { get; set; }
        public MongoUomModel uom { get; set; }
        public string tags { get; set; }
        public object properties { get; set; }
        public ObjectId currencyId { get; set; }
    }
}
