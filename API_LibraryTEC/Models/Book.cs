using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Models
{
    public class Book
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Issn { get; set; }

        [BsonElement("name")]
        public string BookName { get; set; }

        [BsonElement("theme")]
        public string Theme { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("copiesSold")]
        public int CopiesSold { get; set; }

        [BsonElement("idLibraries")]
        public List<string> IDLibraries { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("photo")]
        public string Photo { get; set; }

        [BsonElement("price")]
        public int Price { get; set; }

    }


}
