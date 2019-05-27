using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using API_LibraryTEC.Models;
using API_LibraryTEC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API_LibraryTEC.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        /// <summary>
        /// Class constructor 
        /// </summary>
        /// <param name="pBookService"></param>
        public BooksController(BookService pBookService)
        {
            _bookService = pBookService;
        }

        /// <summary>
        /// Return a list with the data of all the books inside the database
        /// </summary>
        /// <returns></returns>
        [Route("api/books/all")]
        [HttpGet]
        public ActionResult<List<Book>> Get()
        {
            return _bookService.Get();
        }


        /// <summary>
        /// Return the data of a single book by its Issn
        /// </summary>
        /// <param name="pIssn">Issn of the book</param>
        /// <returns></returns>
        [Route("api/books/get/{pIssn}")]
        [HttpGet]
        public ActionResult<Book> Get(string pIssn)
        {
            var book = _bookService.Get(pIssn);

            if (book == null)
            {
                return NotFound();
            }
            return book;
        }


        /// <summary>
        /// Receives the data of a new book, to insert it in the database
        /// </summary>
        /// <param name="book">Model class with the data of the new book</param>
        /// <returns>Http status code: 201 if successful, 409 if there is an error</returns>
        [Route("api/books/create")]
        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            int result = _bookService.Create(book);

            if (result < 0)
                return StatusCode(StatusCodes.Status409Conflict);

            return StatusCode(StatusCodes.Status201Created);
        }


        /// <summary>
        /// Receives the updated data of a book, to update it in the database
        /// </summary>
        /// <param name="pIssn">Issn of the book to be updated</param>
        /// <param name="pBook">Model class with the updated data</param>
        /// <returns>Http status code: 200 if successfull,
        /// 409 if there is an error during the updating process, 
        /// 404 if the book is not found the database</returns>
        [Route("api/books/update/{pIssn}")]
        [HttpPost]
        public IActionResult Update(string pIssn, Book pBook)
        {
            var book = _bookService.Get(pIssn);

            if (book == null)
                return NotFound();

            if (_bookService.Update(pIssn, pBook) < 0)
                return StatusCode(StatusCodes.Status409Conflict);

            return StatusCode(StatusCodes.Status200OK);
        }


        /// <summary>
        /// Deletes a book from the database, searching it by its Issn(_id)
        /// </summary>
        /// <param name="pIssn">Issn(_id) of the book</param>
        /// <returns></returns>
        [Route("api/books/delete/{pIssn}")]
        [HttpGet]
        public IActionResult Delete(string pIssn)
        {
            if (_bookService.Get(pIssn) == null)
                return NotFound();

            _bookService.Remove(pIssn);
            return StatusCode(StatusCodes.Status200OK);
        }


        /// <summary>
        /// Filter the books inside the database using the specified filter conditions
        /// </summary>
        /// <param name="data">Object that holds the filters data</param>
        /// <returns></returns>
        [Route("api/books/filter")]
        [HttpPost]
        public ActionResult<List<Book>> Filter([FromBody] JObject data)
        {
            //List<int> filters = new List<int>() { 1 };
            List<int> filters = data["filters"].ToObject<List<int>>();
            //List<string> values = new List<string>() { "LIB-1"};
            List<string> values = data["values"].ToObject<List<string>>();

            return _bookService.SearchFilters(filters, values);
        }


        /// <summary>
        /// Filter the books according to the specified range of prices
        /// </summary>
        /// <param name="pLow">Range start</param>
        /// <param name="pHigh">Range end</param>
        /// <returns></returns>
        [Route("api/books/price/{pLow}/{pHigh}")]
        [HttpGet]
        public ActionResult<List<Book>> PriceRange([FromRoute] int pLow, [FromRoute] int pHigh)
        {
            return _bookService.PriceRange(pLow, pHigh);
        }


        /// <summary>
        /// Translate text to the specified language using the Google Translate API
        /// </summary>
        /// <param name="pData">Data object with text and language</param>
        /// <returns></returns>
        [Route("api/books/translate")]
        [HttpPost]
        public ActionResult<ExpandoObject> Translate([FromBody] JObject pData)
        {
            string text = pData["text"].ToString();
            string language = pData["language"].ToString();
            dynamic response = new ExpandoObject();
            response.translation = _bookService.Translate(text, language);

            return response;
        }


    }
}