using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Models
{
    public class Library
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("schedule")]
        public string Schedule { get; set; }

        

    }


    static class CONSTANTS_LIBRARY {
        public const string LIBRARIES_COLLECTION = "Libraries";
        public const string ID = "_id";
        public const string NAME = "name";
        public const string COUNTRY = "country";
        public const string LOCATION = "location";
        public const string PHONE = "phone";
        public const string SCHEDULE = "schedule";
    }

}
