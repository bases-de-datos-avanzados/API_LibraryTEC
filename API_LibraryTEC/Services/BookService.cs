using API_LibraryTEC.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_LibraryTEC.Services
{
    public class BookService
    {

        private readonly IMongoCollection<Book> _books;

        public BookService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("LibraryTECDB"));
            var database = client.GetDatabase("LibraryTECDB");
            _books = database.GetCollection<Book>("Books");
        }


        public List<Book> Get()
        {
            return _books.Find(book => true).ToList();
        }


        public Book Get(string pIssn)
        {
            var filter = Builders<Book>.Filter.Eq("_id", pIssn);
            return _books.Find<Book>(filter).FirstOrDefault();
            //return _books.Find<Book>(book => book.Issn == pIssn).FirstOrDefault();
        }


        public int Create(Book book)
        {
            try
            {
                _books.InsertOne(book);
            } catch(Exception e)
            {
                return -1;
            }
            
            return 0;
        }

    }
}
