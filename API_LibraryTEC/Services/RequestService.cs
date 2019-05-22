using API_LibraryTEC.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Services
{
    public class RequestService
    {
        // Holds the collection "Requests" of the database
        private readonly IMongoCollection<Request> _requests;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="config"></param>
        public RequestService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("LibraryTECDB"));
            var database = client.GetDatabase("LibraryTECDB");
            _requests = database.GetCollection<Request>(CONSTANTS_REQUEST.REQUESTS_COLLECTION);
        }


        /// <summary>
        /// Obtains all the documents in the collection "Request"
        /// </summary>
        /// <returns></returns>
        public List<Request> Get()
        {
            return _requests.Find(request => true).ToList();
        }


        /// <summary>
        /// Obtains a single document of the collection "Requests" that matches the given Id (_id)
        /// </summary>
        /// <param name="pId">Id of the document</param>
        /// <returns></returns>
        public Request Get(string pId)
        {
            var filter = Builders<Request>.Filter.Eq(CONSTANTS_REQUEST.ID, pId);
            return _requests.Find<Request>(filter).FirstOrDefault();
        }


        /// <summary>
        /// Create a new document inside the collection "Requests"
        /// </summary>
        /// <param name="pRequest">New request to be created</param>
        /// <returns>0 if successful, -1 if there is an error</returns>
        public int Create(Request pRequest)
        {
            try
            {
                _requests.InsertOne(pRequest);
            }
            catch (Exception e)
            {
                return -1;
            }

            return 0;
        }


        /// <summary>
        /// Update a document of the collection "Requests", searching it by its Id (_id) 
        /// </summary>
        /// <param name="pId">Id of the request</param>
        /// <param name="pRequest">New request with updated data</param>
        /// <returns>0 if successful, -1 if there is an error</returns>
        public int Update(string pId, Request pRequest)
        {
            try
            {
                _requests.ReplaceOne(request => request.Id == pId, pRequest);
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }


        /// <summary>
        /// Deletes a document in the collection "Requests" that has the specified Id (_id)
        /// </summary>
        /// <param name="pId">Id of the request to be deleted</param>
        public void Remove(string pId)
        {
            _requests.DeleteOne(request => request.Id == pId);
        }


        /// <summary>
        /// Retrieves all documents with the value of the field "requestDate" between the two
        /// dates specified
        /// </summary>
        /// <param name="pDate1">First date</param>
        /// <param name="pDate2">Second date</param>
        /// <returns>List with all the matching documents</returns>
        public List<Request> SearchDateRange(DateTime pDate1, DateTime pDate2)
        {
            var gte = Builders<Request>.Filter.Gte(CONSTANTS_REQUEST.REQUEST_DATE, pDate1);
            var lte = Builders<Request>.Filter.Lte(CONSTANTS_REQUEST.REQUEST_DATE, pDate2);
            var filter = Builders<Request>.Filter.And(gte, lte);

            return _requests.Find<Request>(filter).ToList();
        }


        /// <summary>
        /// Search all the documents in the collection "Requests" that has the specified state
        /// The state is specified with an integer that represent it
        /// </summary>
        /// <param name="pState">Integer that represent the state</param>
        /// <returns></returns>
        public List<Request> SearchState(int pState)
        {
            var filter = Builders<Request>.Filter.Eq(CONSTANTS_REQUEST.STATE, CONSTANTS_REQUEST.STATES[pState]);
            return _requests.Find(filter).ToList();
        }


        /// <summary>
        /// Return all the documents of the collection "Requests" that have in the field "requestBooks" the 
        /// id of a book of the specified theme.
        /// First obtains all the IDs of the documents in "Requests", and then query the the data of those 
        /// documents
        /// </summary>
        /// <param name="pTheme">Theme of the book</param>
        /// <returns></returns>
        public List<Request> SearchBookTheme(string pTheme)
        {
            var unwind = new BsonDocument("$unwind", "$requestBooks");
            var lookup = new BsonDocument { { "$lookup", new BsonDocument { { "from", CONSTANTS_BOOK.BOOKS_COLLECTION },
                { "localField", CONSTANTS_REQUEST.REQUEST_BOOKS },
                { "foreignField", CONSTANTS_BOOK.ISSN },
                { "as", "book_docs" } } } };
            var match = new BsonDocument("$match", new BsonDocument("book_docs.theme", pTheme));
            var project = new BsonDocument { { "$project", new BsonDocument { { "_id", 1 } } } };
            var group = new BsonDocument("$group", new BsonDocument(CONSTANTS_REQUEST.ID, "$_id"));
            var pipeline = new[] { unwind, lookup, match, project, group };

            List<BsonDocument> ids = _requests.Aggregate<BsonDocument>(pipeline).ToList();
            List<Request> requests = new List<Request>();
            if(ids.Count != 0)
            {
                for(int i=0; i<ids.Count; ++i)
                {
                    Request req = this.Get(ids[i].GetValue(CONSTANTS_REQUEST.ID).AsString);
                    requests.Add(req);
                }
            }

            return requests;
        }


        /// <summary>
        /// Return all the documents in the collection "Requests" with the value "idClient" equal to
        /// the specified client id
        /// </summary>
        /// <param name="pClient">ID of the client</param>
        /// <returns></returns>
        public List<Request> SearchClient(string pClient)
        {
            var query = Builders<Request>.Filter.Eq(CONSTANTS_REQUEST.ID_CLIENT, pClient);
            return _requests.Find(query).ToList();
        }



    }
}
