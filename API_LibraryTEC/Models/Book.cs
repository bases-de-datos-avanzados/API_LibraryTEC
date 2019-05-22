using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Models
{

    static class CONSTANTS_BOOK
    {
        public const string BOOKS_COLLECTION = "Books";
        public const string ISSN = "_id";
        public const string NAME = "name";
        public const string THEME = "theme";
        public const string DESCRIPTION = "description";
        public const string SUB_LIBRARY_ID = "idLibrary";
        public const string SUB_COPIES_SOLD = "copiesSold";
        public const string SUB_QUANTITY = "quantity";
        public const string LIBRARIES = "libraries";
        public const string PHOTO = "photo";
        public const string PRICE = "price";

        public readonly static Dictionary<int, string> FILTERS = new Dictionary<int, string>()
        {
            { 1, CONSTANTS_BOOK.LIBRARIES+"."+CONSTANTS_BOOK.SUB_LIBRARY_ID},
            { 2, CONSTANTS_BOOK.NAME},
            { 3, CONSTANTS_BOOK.THEME},
            { 4, CONSTANTS_BOOK.PRICE}
        };
    }

    public class Book
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Issn { get; set; }

        [BsonElement(CONSTANTS_BOOK.NAME)]
        public string BookName { get; set; }

        [BsonElement(CONSTANTS_BOOK.THEME)]
        public string Theme { get; set; }

        [BsonElement(CONSTANTS_BOOK.DESCRIPTION)]
        public string Description { get; set; }

        [BsonElement(CONSTANTS_BOOK.LIBRARIES)]
        public List<SubLibrary> Libraries { get; set; }

        [BsonElement(CONSTANTS_BOOK.PHOTO)]
        public string Photo { get; set; }

        [BsonElement(CONSTANTS_BOOK.PRICE)]
        public int Price { get; set; }

    }


    public class SubLibrary
    {
        [BsonElement(CONSTANTS_BOOK.SUB_LIBRARY_ID)]
        public string IdLibrary { get; set; }

        [BsonElement(CONSTANTS_BOOK.SUB_QUANTITY)]
        public int Quantity { get; set; }

        [BsonElement(CONSTANTS_BOOK.SUB_COPIES_SOLD)]
        public int CopiesSold { get; set; } = 0;
    }

}
