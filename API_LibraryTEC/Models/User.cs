using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Models
{
    static class CONSTANTS_USER
    {
        public const string USERS_COLLECTION = "Users";
        public const string ID = "_id";
        public const string NAME = "name";
        public const string SUB_FIRST_NAME = "firstName";
        public const string SUB_SUR_NAME = "surName";
        public const string SUB_LAST_NAME = "lastName";
        public const string BIRTHDAY = "birthday";
        public const string STATE = "state";
        public const string LOCATION = "location";
        public const string EMAIL = "email";
        public const string PHONES = "phones";
        public const string USER_NAME = "userName";
        public const string PASS = "pass";
        public const string USER_TYPE = "userType";
    }

    public class User
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }

        [BsonElement(CONSTANTS_USER.NAME)]
        public UserName Name { get; set; }

        [BsonElement(CONSTANTS_USER.BIRTHDAY)]
        public DateTime Birthday { get; set; }

        [BsonElement(CONSTANTS_USER.STATE)]
        public string State { get; set; }

        [BsonElement(CONSTANTS_USER.LOCATION)]
        public string Location { get; set; }

        [BsonElement(CONSTANTS_USER.EMAIL)]
        public string Email { get; set; }

        [BsonElement(CONSTANTS_USER.PHONES)]
        public List<string> Phones { get; set; }

        [BsonElement(CONSTANTS_USER.USER_NAME)]
        public string UserName { get; set; }

        [BsonElement(CONSTANTS_USER.PASS)]
        public string Pass { get; set; }

        [BsonElement(CONSTANTS_USER.USER_TYPE)]
        public string UserType { get; set; }
    }


    public class UserName
    {
        [BsonElement(CONSTANTS_USER.SUB_FIRST_NAME)]
        public string FirstName { get; set; }

        [BsonElement(CONSTANTS_USER.SUB_SUR_NAME)]
        public string SurName { get; set; }

        [BsonElement(CONSTANTS_USER.SUB_LAST_NAME)]
        public string LastName { get; set; }
    }
}
