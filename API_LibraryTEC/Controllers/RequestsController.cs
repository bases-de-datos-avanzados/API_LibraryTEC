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
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private const string REQUEST_URL = "api/requests";
        private readonly RequestService _requestService;

        /// <summary>
        /// Class constructor 
        /// </summary>
        /// <param name="pRequestService"></param>
        public RequestsController(RequestService pRequestService)
        {
            _requestService = pRequestService;
        }


        /// <summary>
        /// Return a list with the data of all the requests inside the database
        /// </summary>
        /// <returns></returns>
        [Route(REQUEST_URL + "/all")]
        [HttpGet]
        public ActionResult<List<Request>> Get()
        {
            return _requestService.Get();
        }


        /// <summary>
        /// Return the data of a single request by its Id
        /// </summary>
        /// <param name="pId">Id of the request</param>
        /// <returns></returns>
        [Route(REQUEST_URL + "/get/{pId}")]
        [HttpGet]
        public ActionResult<Request> Get(string pId)
        {
            var request = _requestService.Get(pId);

            if (request == null)
                return NotFound();

            return request;
        }


        /// <summary>
        /// Receives the data of a new request, to insert it in the database
        /// </summary>
        /// <param name="pRequest">Model class with the data of the new request</param>
        /// <returns>Http status code: 201 if successful, 409 if there is an error</returns>
        [Route(REQUEST_URL + "/create")]
        [HttpPost]
        public IActionResult Create(Request pRequest)
        {
            int result = _requestService.Create(pRequest);

            if (result < 0)
                return StatusCode(StatusCodes.Status409Conflict);

            return StatusCode(StatusCodes.Status201Created);
        }


        /// <summary>
        /// Receives the updated data of a request, to update it in the database
        /// </summary>
        /// <param name="pId">Id of the request</param>
        /// <param name="pRequest">Model class with the updated data</param>
        /// <returns>Http status code: 200 if successfull,
        /// 409 if there is an error during the updating process, 
        /// 404 if the request is not found the database</returns>
        [Route(REQUEST_URL + "/update/{pId}")]
        [HttpPost]
        public IActionResult Update(string pId, Request pRequest)
        {
            if (_requestService.Get(pId) == null)
                return NotFound();

            if (_requestService.Update(pId, pRequest) < 0)
                return StatusCode(StatusCodes.Status409Conflict);

            return StatusCode(StatusCodes.Status200OK);
        }


        /// <summary>
        /// Deletes a request from the database, searching it by its Id(_id)
        /// </summary>
        /// <param name="pId">Id of the request</param>
        /// <returns>Http status code: 200 if successfull, 
        /// 404 if the request is not found the database</returns>
        [Route(REQUEST_URL + "/delete/{pId}")]
        [HttpGet]
        public IActionResult Delete(string pId)
        {
            if (_requestService.Get(pId) == null)
                return NotFound();

            _requestService.Remove(pId);
            return StatusCode(StatusCodes.Status200OK);
        }



    }
}