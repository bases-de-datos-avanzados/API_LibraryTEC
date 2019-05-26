using API_LibraryTEC.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Services
{
    public class UserService
    {
        // Holds the collection "Users" of the database
        private readonly IMongoCollection<User> _users;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="config"></param>
        public UserService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("LibraryTECDB"));
            var database = client.GetDatabase("LibraryTECDB");
            _users = database.GetCollection<User>(CONSTANTS_USER.USERS_COLLECTION);
        }


        /// <summary>
        /// Obtains all the documents in the collection "Users"
        /// </summary>
        /// <returns></returns>
        public List<User> Get()
        {
            return _users.Find(user => true).ToList();
        }


        /// <summary>
        /// Obtains a single document of the collection "Users" that matches the given Id (_id)
        /// </summary>
        /// <param name="pId">Id of the document</param>
        /// <returns></returns>
        public User Get(string pId)
        {
            var filter = Builders<User>.Filter.Eq(CONSTANTS_USER.ID, pId);
            return _users.Find<User>(filter).FirstOrDefault();
        }


        /// <summary>
        /// Create a new document inside the collection "User"
        /// </summary>
        /// <param name="pUser">New user to be created</param>
        /// <returns>0 if successful, -1 if there is an error</returns>
        public int Create(User pUser)
        {
            try
            {
                _users.InsertOne(pUser);
            }
            catch (Exception e)
            {
                return -1;
            }

            return 0;
        }


        /// <summary>
        /// Update a document of the collection "Users", searching it by its Id (_id) 
        /// </summary>
        /// <param name="pId">Id of the user</param>
        /// <param name="pUser">New user with updated data</param>
        /// <returns>0 if successful, -1 if there is an error</returns>
        public int Update(string pId, User pUser)
        {
            try
            {
                _users.ReplaceOne(user => user.Id == pId, pUser);
            }
            catch (Exception e)
            {
                return -1;
            }
            return 0;
        }


        /// <summary>
        /// Deletes a document in the collection "Users" that has the specified Id (_id)
        /// </summary>
        /// <param name="pId">Id of the user to be deleted</param>
        public void Remove(string pId)
        {
            _users.DeleteOne(user => user.Id == pId);
        }



        /// <summary>
        /// Return the data of a document with matching username
        /// </summary>
        /// <param name="pUserName"></param>
        /// <returns></returns>
        public User Login(string pUserName)
        {
            var match = Builders<User>.Filter.Eq(CONSTANTS_USER.USER_NAME, pUserName);
            return _users.Find<User>(match).FirstOrDefault();
        }




    }
}
