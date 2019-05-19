using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Models
{
    static class CONSTANTS_PROMO
    {
        public const string PROMOS_COLLECTION = "Promos";
        public const string ID = "_id";
        public const string DESCRIPTION = "description";
        public const string START_DATE = "startDate";
        public const string END_DATE = "endDate";
        public const string DISCOUNT = "discount";
    }

    public class Promo
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        [BsonElement(CONSTANTS_PROMO.DESCRIPTION)]
        public string Description { get; set; }

        [BsonElement(CONSTANTS_PROMO.START_DATE)]
        public DateTime StartDate { get; set; }

        [BsonElement(CONSTANTS_PROMO.END_DATE)]
        public DateTime EndDate { get; set; }

        [BsonElement(CONSTANTS_PROMO.DISCOUNT)]
        public int Discount { get; set; }

    }
}
