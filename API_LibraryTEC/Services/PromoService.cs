using API_LibraryTEC.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Services
{
    public class PromoService
    {
        // Holds the collection "Promos" of the database
        private readonly IMongoCollection<Promo> _promos;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="config"></param>
        public PromoService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("LibraryTECDB"));
            var database = client.GetDatabase("LibraryTECDB");
            _promos = database.GetCollection<Promo>(CONSTANTS_PROMO.PROMOS_COLLECTION);
        }


        /// <summary>
        /// Obtains all the documents in the collection "Promos"
        /// </summary>
        /// <returns></returns>
        public List<Promo> Get()
        {
            return _promos.Find(promo => true).ToList();
        }


        /// <summary>
        /// Obtains a single document of the collection "Promos" that matches the given Id (_id)
        /// </summary>
        /// <param name="pId">Id of the document</param>
        /// <returns></returns>
        public Promo Get(string pId)
        {
            var filter = Builders<Promo>.Filter.Eq(CONSTANTS_PROMO.ID, pId);
            return _promos.Find<Promo>(filter).FirstOrDefault();
        }


        /// <summary>
        /// Create a new document inside the collection "Promos"
        /// </summary>
        /// <param name="pPromo">New Promo to be created</param>
        /// <returns>0 if successful, -1 if there is an error</returns>
        public int Create(Promo pPromo)
        {
            try
            {
                _promos.InsertOne(pPromo);
            }
            catch (Exception e)
            {
                return -1;
            }

            return 0;
        }


        /// <summary>
        /// Update a document of the collection "Promos", searching it by its Id (_id) 
        /// </summary>
        /// <param name="pId">Id of the promo</param>
        /// <param name="pPromo">New promo with updated data</param>
        /// <returns>0 if successful, -1 if there is an error</returns>
        public int Update(string pId, Promo pPromo)
        {
            try
            {
                _promos.ReplaceOne(promo => promo.Id == pId, pPromo);
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }


        /// <summary>
        /// Deletes a document in the collection "Promos" that has the specified Id (_id)
        /// </summary>
        /// <param name="pId">Id of the promo to be deleted</param>
        public void Remove(string pId)
        {
            _promos.DeleteOne(promo => promo.Id == pId);
        }



    }
}
