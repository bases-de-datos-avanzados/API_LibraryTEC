using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Models
{
    static class CONSTANTS_REQUEST
    {
        public const string REQUESTS_COLLECTION = "Requests";
        public const string ID = "_id";
        public const string ID_CLIENT = "idClient";
        public const string REQUEST_DATE = "requestDate";
        public const string REQUEST_BOOKS = "requestBooks";
        public const string STATE = "state";
        public const string TOTAL = "total";
        public const string DELIVERY_DATE = "deliveryDate";
        public const string DELIVERY_PLACE = "deliveryPlace";
    }


    public class Request
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        [BsonElement(CONSTANTS_REQUEST.ID_CLIENT)]
        public string IdClient { get; set; }

        [BsonElement(CONSTANTS_REQUEST.REQUEST_DATE)]
        public DateTime RequestDate { get; set; }

        [BsonElement(CONSTANTS_REQUEST.REQUEST_BOOKS)]
        public List<string> RequestBooks { get; set; }

        [BsonElement(CONSTANTS_REQUEST.STATE)]
        public string State { get; set; }

        [BsonElement(CONSTANTS_REQUEST.TOTAL)]
        public int Total { get; set; }

        [BsonElement(CONSTANTS_REQUEST.DELIVERY_DATE)]
        public DateTime DeliveryDate { get; set; }

        [BsonElement(CONSTANTS_REQUEST.DELIVERY_PLACE)]
        public string DeliveryPlace { get; set; } = "";
    }
}
