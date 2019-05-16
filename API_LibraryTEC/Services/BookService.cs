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
    public class BookService
    {
        // Holds the collection "Books" of the database
        private readonly IMongoCollection<Book> _books;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="config"></param>
        public BookService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("LibraryTECDB"));
            var database = client.GetDatabase("LibraryTECDB");
            _books = database.GetCollection<Book>("Books");
        }


        /// <summary>
        /// Obtains all the documents in the collection "Books"
        /// </summary>
        /// <returns></returns>
        public List<Book> Get()
        {
            return _books.Find(book => true).ToList();
        }


        /// <summary>
        /// Obtains a single document of the collection "Books" that matches the given Issn (_id)
        /// </summary>
        /// <param name="pIssn">Issn (_id) of the book</param>
        /// <returns>Book model</returns>
        public Book Get(string pIssn)
        {
            var filter = Builders<Book>.Filter.Eq("_id", pIssn);
            return _books.Find<Book>(filter).FirstOrDefault();
            //return _books.Find<Book>(book => book.Issn == pIssn).FirstOrDefault();
        }


        /// <summary>
        /// Create a new document inside the collection "Books"
        /// </summary>
        /// <param name="book">New book to be created (inserted)</param>
        /// <returns>0 if successful, -1 if there is an error</returns>
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

        /// <summary>
        /// Update a document of the collection "Books", searching it by its Issn (_id)
        /// </summary>
        /// <param name="pIssn">Issn (_id) of the book</param>
        /// <param name="pBook">New book with updated data</param>
        /// <returns></returns>
        public int Update(string pIssn, Book pBook)
        {
            try
            {
                _books.ReplaceOne(book => book.Issn == pIssn, pBook);
            }catch(Exception e)
            {
                return -1;
            }
            return 0;
        }


        /// <summary>
        /// Deletes a document in the collection "Books" that has the specified Issn (_id)
        /// </summary>
        /// <param name="pIssn">Issn (_id) of the book to be deleted</param>
        public void Remove(string pIssn)
        {
            _books.DeleteOne(book => book.Issn == pIssn);
        }


        /// <summary>
        /// Obtains all the documents in the collection "Books" that meets all the filters specified
        /// </summary>
        /// <param name="pFilters">List of integers representing the filter fields</param>
        /// <param name="pValues">List with the values of the searched fields</param>
        /// <returns></returns>
        public List<Book> SearchFilters(List<int> pFilters, List<string> pValues)
        {
            if (pFilters.Count == 1)
                return this.Filter(pFilters[0], pValues[0]);

            List<string> fields = new List<string>();
            for (int i=0; i<pFilters.Count; ++i)
            {
                switch (pFilters[i])
                {
                    case 1:
                        fields.Add("idLibraries");
                        break;
                    case 2:
                        fields.Add("name");
                        break;
                    case 3:
                        fields.Add("theme");
                        break;
                }
            }

            var query = Builders<Book>.Filter.Eq(fields[0], pValues[0]);
            for(int i=1; i<fields.Count; ++i)
            {
                query = query & (Builders<Book>.Filter.Eq(fields[i], pValues[i]));
            }

            return _books.Find(query).ToList();
        }



        /// <summary>
        /// Obtains all the documents in the collection "Books" that meets the specified filter
        /// </summary>
        /// <param name="pFilter">Field under which the search is going to be performed</param>
        /// <param name="pValue">Value of the searched field</param>
        /// <returns></returns>
        public List<Book> Filter(int pFilter, string pValue)
        {
            string field = "";
            switch (pFilter)
            {
                case 1:
                    field = "idLibraries";
                    break;
                case 2:
                    field = "name";
                    break;
                case 3:
                    field = "theme";
                    break;
            }
            var filter = Builders<Book>.Filter.Eq(field, pValue);

            return _books.Find<Book>(filter).ToList<Book>();
        }


        /// <summary>
        /// Obtains all the documents inside the collection "Books" that has the 
        /// value of the field "price" between the two specified values
        /// </summary>
        /// <param name="pPrice1">Range start</param>
        /// <param name="pPrice2">Range end</param>
        /// <returns></returns>
        public List<Book> PriceRange(int pPrice1, int pPrice2)
        {
            var gte = Builders<Book>.Filter.Gte("price", pPrice1);
            var lte = Builders<Book>.Filter.Lte("price", pPrice2);
            var filter = Builders<Book>.Filter.And(gte, lte);

            return _books.Find(filter).ToList();
        }

    }
}
