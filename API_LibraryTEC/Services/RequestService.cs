using API_LibraryTEC.Models;
using Microsoft.Extensions.Configuration;
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



    }
}
