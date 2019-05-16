using API_LibraryTEC.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Services
{
    public class LibraryService
    {

        // Holds the collection "Books" of the database
        private readonly IMongoCollection<Library> _libraries;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="config"></param>
        public LibraryService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("LibraryTECDB"));
            var database = client.GetDatabase("LibraryTECDB");
            _libraries = database.GetCollection<Library>(CONSTANTS_LIBRARY.LIBRARIES_COLLECTION);
        }


        /// <summary>
        /// Obtains all the documents in the collection "Libraries"
        /// </summary>
        /// <returns></returns>
        public List<Library> Get()
        {
            return _libraries.Find(library => true).ToList();
        }


        /// <summary>
        /// Obtains a single document of the collection "Libraries" that matches the given Id (_id)
        /// </summary>
        /// <param name="pId">Id of the document</param>
        /// <returns></returns>
        public Library Get(string pId)
        {
            var filter = Builders<Library>.Filter.Eq(CONSTANTS_LIBRARY.ID, pId);
            return _libraries.Find<Library>(filter).FirstOrDefault();
        }


        /// <summary>
        /// Create a new document inside the collection "Libraries"
        /// </summary>
        /// <param name="pLibrary">New library to be created</param>
        /// <returns>0 if successful, -1 if there is an error</returns>
        public int Create(Library pLibrary)
        {
            try
            {
                _libraries.InsertOne(pLibrary);
            }
            catch (Exception e)
            {
                return -1;
            }

            return 0;
        }


        /// <summary>
        /// Update a document of the collection "Libraries", searching it by its Id (_id) 
        /// </summary>
        /// <param name="pId">Id of the library</param>
        /// <param name="pLibrary">New library with updated data</param>
        /// <returns>0 if successful, -1 if there is an error</returns>
        public int Update(string pId, Library pLibrary)
        {
            try
            {
                _libraries.ReplaceOne(library => library.Id == pId, pLibrary);
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }


        /// <summary>
        /// Deletes a document in the collection "Libraries" that has the specified Id (_id)
        /// </summary>
        /// <param name="pId">Id of the library to be deleted</param>
        public void Remove(string pId)
        {
            _libraries.DeleteOne(library => library.Id == pId);
        }


    }
}
