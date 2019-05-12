using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_LibraryTEC.Models;
using API_LibraryTEC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_LibraryTEC.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService pBookService)
        {
            _bookService = pBookService;
        }

        [Route("api/books/all")]
        [HttpGet]
        public ActionResult<List<Book>> Get()
        {
            return _bookService.Get();
        }

        [Route("api/books/get/{pIssn:length(14)}")]
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

        [Route("api/books/create")]
        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            int result = _bookService.Create(book);

            if (result < 0)
                return StatusCode(StatusCodes.Status409Conflict);

            return StatusCode(StatusCodes.Status201Created);
        }


        [Route("api/books/update/{pIssn:length(14)}")]
        [HttpPost]
        public IActionResult Update(string pIssn, Book pBook)
        {
            var book = _bookService.Get(pIssn);

            if (book == null)
                return NotFound();

            if (_bookService.Update(pIssn, pBook) < 0)
                return StatusCode(StatusCodes.Status409Conflict);

            return NoContent();
        }



    }
}