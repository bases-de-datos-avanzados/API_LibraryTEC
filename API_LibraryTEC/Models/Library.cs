using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Models
{

    static class CONSTANTS_LIBRARY
    {
        public const string LIBRARIES_COLLECTION = "Libraries";
        public const string ID = "_id";
        public const string NAME = "name";
        public const string COUNTRY = "country";
        public const string LOCATION = "location";
        public const string PHONE = "phone";
        public const string SCHEDULE = "schedule";
        public const string ID_MANAGER = "idManager";
    }

    public class Library
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        [BsonElement(CONSTANTS_LIBRARY.NAME)]
        public string Name { get; set; }

        [BsonElement(CONSTANTS_LIBRARY.COUNTRY)]
        public string Country { get; set; }

        [BsonElement(CONSTANTS_LIBRARY.LOCATION)]
        public string Location { get; set; }

        [BsonElement(CONSTANTS_LIBRARY.PHONE)]
        public string Phone { get; set; }

        [BsonElement(CONSTANTS_LIBRARY.SCHEDULE)]
        public string Schedule { get; set; }

        [BsonElement(CONSTANTS_LIBRARY.ID_MANAGER)]
        public string IdManager { get; set; } = "";

    }

}
