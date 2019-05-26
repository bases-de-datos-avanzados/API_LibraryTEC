using API_LibraryTEC.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;

namespace API_LibraryTEC.Services
{
    public class ReportService
    {
        // Holds the collection "Books" of the database
        private readonly IMongoCollection<Book> _books;
        private readonly IMongoCollection<Library> _libraries;
        private readonly IMongoCollection<Promo> _promos;
        private readonly IMongoCollection<Request> _requests;
        private readonly IMongoCollection<User> _users;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="config"></param>
        public ReportService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("LibraryTECDB"));
            var database = client.GetDatabase("LibraryTECDB");
            _books = database.GetCollection<Book>(CONSTANTS_BOOK.BOOKS_COLLECTION);
            _libraries = database.GetCollection<Library>(CONSTANTS_LIBRARY.LIBRARIES_COLLECTION);
            _promos = database.GetCollection<Promo>(CONSTANTS_PROMO.PROMOS_COLLECTION);
            _requests = database.GetCollection<Request>(CONSTANTS_REQUEST.REQUESTS_COLLECTION);
            _users = database.GetCollection<User>(CONSTANTS_USER.USERS_COLLECTION);
        }


        /// <summary>
        /// Obtains the average price and the total amount of books of all themes requested
        /// </summary>
        /// <returns></returns>
        public List<ExpandoObject> ReportAdmin1()
        {
            var unwind = new BsonDocument {{"$unwind","$requestBooks"}};

            var lookup = new BsonDocument {{"$lookup", new BsonDocument{{"from","Books"}, {"localField","requestBooks.idBook"},
                                {"foreignField","_id"},
                                {"as","tema"}}}};

            var unwind2 = new BsonDocument {{"$unwind","$tema"}};
            var group = new BsonDocument{{"$group", new BsonDocument{{"_id", "$tema.theme" },
                { "quantity",new BsonDocument{{"$sum",1}}},
                { "average", new BsonDocument{{"$avg", "$tema.price"}}}}}};


            var pipeline = new[] { unwind, lookup, unwind2, group };

            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }


        /// <summary>
        /// Obtains the range of requests
        /// </summary>
        /// <returns></returns>
        public List<ExpandoObject> ReportAdmin2()
        {
            var group = new BsonDocument{{ "$group", new BsonDocument{{"_id", "$idClient" },
                { "menor",new BsonDocument{{"$min", new BsonDocument{{"$size","$requestBooks" } } } } },
                { "mayor",new BsonDocument{{"$max", new BsonDocument{{"$size","$requestBooks" } } } } },
                { "cantidad",new BsonDocument{{"$sum", new BsonDocument { { "$size", "$requestBooks" } } } }},}}};

            var sort = new BsonDocument {{"$sort", new BsonDocument{{"cantidad",-1}}}};

            var pipeline = new[] { group, sort };
            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }


        /// <summary>
        /// Return the 5 most selled books
        /// </summary>
        /// <returns></returns>
        public List<ExpandoObject> ReportAdmin3()
        {
            var unwind = new BsonDocument {{"$unwind","$requestBooks"}};

            var lookup = new BsonDocument {{"$lookup", new BsonDocument{{"from","Books"},
                { "localField","requestBooks.idBook"}, {"foreignField","_id"}, {"as","tema"}}}};

            var unwind2 = new BsonDocument {{"$unwind","$tema"}};

            var group = new BsonDocument{{"$group", new BsonDocument{{"_id", "$requestBooks.idBook" },
                { "nombre", new BsonDocument{ {"$first","$tema.name"} } },
                { "cantidad",new BsonDocument{{"$sum",1}}},}}};

            var sort = new BsonDocument {{"$sort", new BsonDocument{{"cantidad",-1}}}};

            var limit = new BsonDocument {{"$limit",5}};

            var pipeline = new[] { unwind, lookup, unwind2, group, sort, limit };
            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }


        /// <summary>
        /// Return the top 3 clients with more requests
        /// </summary>
        /// <returns></returns>
        public List<ExpandoObject> ReportAdmin4()
        {
            var group = new BsonDocument{{ "$group", new BsonDocument{{ "_id", "$idClient" },
                { "cantidad",new BsonDocument{{"$sum", new BsonDocument { { "$size", "$requestBooks" } } } }}, }}};

            var sort = new BsonDocument {{ "$sort", new BsonDocument{{"cantidad",-1}}}};
            var limit = new BsonDocument {{"$limit",3}};

            var pipeline = new[] { group, sort, limit };

            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }



        /// <summary>
        /// For an specific library
        /// Obtains the average price and the total amount of books of all themes requested
        /// </summary>
        /// <param name="pLibrary"></param>
        /// <returns></returns>
        public List<ExpandoObject> ReportManager1(string pLibrary)
        {
            var unwind = new BsonDocument {{ "$unwind","$requestBooks"}};
            var match = new BsonDocument {{ "$match", new BsonDocument{{"requestBooks.idLibrary",pLibrary}}}};
            var lookup = new BsonDocument {{ "$lookup", new BsonDocument{{"from","Books"},
                { "localField","requestBooks.idBook"}, {"foreignField","_id"}, {"as","tema"}}}};

            var unwind2 = new BsonDocument {{"$unwind","$tema"}};
            var group = new BsonDocument{{"$group", new BsonDocument{{"_id", "$tema.theme" },
                { "cantidad",new BsonDocument{{"$sum",1}}},
                { "Promedio", new BsonDocument{{"$avg", "$tema.price"}}}}}};


            var pipeline = new[] { unwind, match, lookup, unwind2, group };
            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }


        /// <summary>
        /// For an specific library
        /// Range of requests made by clients
        /// </summary>
        /// <param name="pLibrary"></param>
        /// <returns></returns>
        public List<ExpandoObject> ReportManager2(string pLibrary)
        {
            var match = new BsonDocument {{ "$match", new BsonDocument{{"requestBooks.idLibrary", pLibrary}}}};
            var group = new BsonDocument{{ "$group", new BsonDocument{{"_id", "$idClient" },
                { "menor",new BsonDocument{{"$min", new BsonDocument{{"$size","$requestBooks" } } } } },
                { "mayor",new BsonDocument{{"$max", new BsonDocument{{"$size","$requestBooks" } } } } },
                { "cantidad",new BsonDocument{{"$sum", new BsonDocument { { "$size", "$requestBooks" } } } }},}}};

            var sort = new BsonDocument {{ "$sort", new BsonDocument{{"cantidad",-1}}}};

            var pipeline = new[] { match, group, sort };
            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }


        /// <summary>
        /// Top most buyed books
        /// </summary>
        /// <param name="pLibrary">Library id</param>
        /// <returns></returns>
        public List<ExpandoObject> ReportManager3(string pLibrary)
        {
            var unwind = new BsonDocument {{"$unwind","$requestBooks"}};
            var match = new BsonDocument {{ "$match", new BsonDocument{{"requestBooks.idLibrary", pLibrary}}}};

            var lookup = new BsonDocument {{ "$lookup", new BsonDocument{{"from","Books"},
                { "localField","requestBooks.idBook"}, {"foreignField","_id"}, {"as","tema"}}}};

            var unwind2 = new BsonDocument {{"$unwind","$tema"}};
            var group = new BsonDocument{{"$group", new BsonDocument{{"_id", "$requestBooks.idBook" },
                { "nombre", new BsonDocument{ {"$first","$tema.name"} } },
                { "cantidad",new BsonDocument{{"$sum",1}}},}}};

            var sort = new BsonDocument {{ "$sort", new BsonDocument{{"cantidad",-1}}}};
            var limit = new BsonDocument {{"$limit",5}};

            var pipeline = new[] { unwind, match, lookup, unwind2, group, sort, limit };
            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }


        /// <summary>
        /// Amount of requests made by a client in a library
        /// </summary>
        /// <param name="pClient">Client id</param>
        /// <param name="pLibrary">Library id</param>
        /// <returns></returns>
        public List<ExpandoObject> ReportManager4_1(string pClient, string pLibrary)
        {
            var match = new BsonDocument {{"$match", new BsonDocument{{"idClient",pClient},
                { "requestBooks.idLibrary",pLibrary}}}};
            var group = new BsonDocument{{"$group", new BsonDocument{{"_id", 0 },
                { "cantidad",new BsonDocument{{"$sum", 1}}},}}};


            var pipeline = new[] { match, group };
            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }


        /// <summary>
        /// Return the amout of books registers in one library, between the two specified libraries
        /// </summary>
        /// <param name="pLibrary">Library id</param>
        /// <param name="pDate1">First date</param>
        /// <param name="pDate2">Second date</param>
        /// <returns></returns>
        public List<ExpandoObject> ReportManager4_2(string pLibrary, DateTime pDate1, DateTime pDate2)
        {
            var match = new BsonDocument {{"$match", new BsonDocument{{"requestDate",
                    new BsonDocument {{ "$gte", pDate1 },{"$lte", pDate2}} },
                { "requestBooks.idLibrary", pLibrary}}}};

            var group = new BsonDocument{{"$group", new BsonDocument{{"_id", 0 },
                { "cantidad",new BsonDocument{{"$sum", 1}}},}}};


            var pipeline = new[] { match, group };
            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }


        /// <summary>
        /// Return the amount of books of a specific theme, requested in one library
        /// </summary>
        /// <param name="pLibrary">Library id</param>
        /// <param name="pTheme">Theme of the book</param>
        /// <returns></returns>
        public List<ExpandoObject> ReportManager4_3(string pLibrary, string pTheme)
        {
            var unwind = new BsonDocument {{"$unwind","$requestBooks"}};
            var match1 = new BsonDocument {{"$match", new BsonDocument{{ "requestBooks.idLibrary", pLibrary}}}};
            var lookup = new BsonDocument {{"$lookup", new BsonDocument{{"from","Books"},
                { "localField","requestBooks.idBook"}, {"foreignField","_id"}, {"as","tema"}}}};
            var unwind2 = new BsonDocument {{"$unwind","$tema"}};
            var match = new BsonDocument {{"$match", new BsonDocument{{"tema.theme", pTheme}}}};
            var group = new BsonDocument{{"$group", new BsonDocument{{"_id", 0 },
                { "cantidad",new BsonDocument{{"$sum",1}}},}}};


            var pipeline = new[] { unwind, match1, lookup, unwind2, match, group };
            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }


        /// <summary>
        /// Return the amount of requested books in one library that has the specified state
        /// </summary>
        /// <param name="pLibrary">Library id</param>
        /// <param name="pState">State of the request</param>
        /// <returns></returns>
        public List<ExpandoObject> ReportManager4_4(string pLibrary, int pState)
        {
            var match = new BsonDocument {{"$match", new BsonDocument{{"state",CONSTANTS_REQUEST.STATES[pState]},
                { "requestBooks.idLibrary", pLibrary}}}};
            var group = new BsonDocument{{"$group", new BsonDocument{{"_id", 0 },
                { "cantidad",new BsonDocument{{"$sum", 1}}},}}};


            var pipeline = new[] { match, group };
            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }


        /// <summary>
        /// Top 3 users of users with most books buyed
        /// </summary>
        /// <param name="pLibrary">Library id</param>
        /// <returns></returns>
        public List<ExpandoObject> ReportManager4_5(string pLibrary)
        {
            var match = new BsonDocument {{"$match", new BsonDocument{{"requestBooks.idLibrary", pLibrary}}}};
            var group = new BsonDocument{{"$group", new BsonDocument{{"_id", "$idClient" },
                { "cantidad",new BsonDocument{{"$sum", new BsonDocument { { "$size", "$requestBooks" } } } }},}}};
            var sort = new BsonDocument {{"$sort", new BsonDocument{{"cantidad",-1}}}};

            var limit = new BsonDocument {{"$limit",3}};

            var pipeline = new[] { match, group, sort, limit };
            return _requests.Aggregate<ExpandoObject>(pipeline).ToList();
        }
       




    }
}
