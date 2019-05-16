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
    public class LibrariesController : ControllerBase
    {
        private const string LIBRARY_URL = "api/libraries";
        private readonly LibraryService _libraryService;

        /// <summary>
        /// Class constructor 
        /// </summary>
        /// <param name="pLibraryService"></param>
        public LibrariesController(LibraryService pLibraryService)
        {
            _libraryService = pLibraryService;
        }


        /// <summary>
        /// Return a list with the data of all the libraries inside the database
        /// </summary>
        /// <returns></returns>
        [Route(LIBRARY_URL+"/all")]
        [HttpGet]
        public ActionResult<List<Library>> Get()
        {
            return _libraryService.Get();
        }


        /// <summary>
        /// Return the data of a single library by its Id
        /// </summary>
        /// <param name="pId">Id of the library</param>
        /// <returns></returns>
        [Route(LIBRARY_URL+"/get/{pId}")]
        [HttpGet]
        public ActionResult<Library> Get(string pId)
        {
            var library = _libraryService.Get(pId);

            if (library == null)
                return NotFound();
            
            return library;
        }


        /// <summary>
        /// Receives the data of a new library, to insert it in the database
        /// </summary>
        /// <param name="pLibrary">Model class with the data of the new library</param>
        /// <returns>Http status code: 201 if successful, 409 if there is an error</returns>
        [Route(LIBRARY_URL+"/create")]
        [HttpPost]
        public ActionResult<Library> Create(Library pLibrary)
        {
            int result = _libraryService.Create(pLibrary);

            if (result < 0)
                return StatusCode(StatusCodes.Status409Conflict);

            return StatusCode(StatusCodes.Status201Created);
        }


        /// <summary>
        /// Receives the updated data of a library, to update it in the database
        /// </summary>
        /// <param name="pId">Id of the library</param>
        /// <param name="pLibrary">Model class with the updated data</param>
        /// <returns>Http status code: 200 if successfull,
        /// 409 if there is an error during the updating process, 
        /// 404 if the library is not found the database</returns>
        [Route(LIBRARY_URL+"/update/{pId}")]
        [HttpPost]
        public IActionResult Update(string pId, Library pLibrary)
        {
            if (_libraryService.Get(pId) == null)
                return NotFound();

            if (_libraryService.Update(pId, pLibrary) < 0)
                return StatusCode(StatusCodes.Status409Conflict);

            return StatusCode(StatusCodes.Status200OK);
        }


        /// <summary>
        /// Deletes a library from the database, searching it by its Id(_id)
        /// </summary>
        /// <param name="pId">Id of the library</param>
        /// <returns>Http status code: 200 if successfull, 
        /// 404 if the library is not found the database</returns>
        [Route(LIBRARY_URL+"/delete/{pId}")]
        [HttpGet]
        public IActionResult Delete(string pId)
        {
            if (_libraryService.Get(pId) == null)
                return NotFound();

            _libraryService.Remove(pId);
            return StatusCode(StatusCodes.Status200OK);
        }


    }
}