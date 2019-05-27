using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_LibraryTEC.Models;
using API_LibraryTEC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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


        /// <summary>
        /// Obtains the number of requests created between the two given dates
        /// </summary>
        /// <param name="pDate1">First date</param>
        /// <param name="pDate2">Second date</param>
        /// <returns>Lists of all the matching requests</returns>
        [Route(REQUEST_URL + "/date/{pDate1}/{pDate2}")]
        [HttpGet]
        public ActionResult<JObject> SearchRequestDate([FromRoute] DateTime pDate1, [FromRoute] DateTime pDate2)
        {
            dynamic respond = new JObject();
            respond.quantity = _requestService.SearchDateRange(pDate1, pDate2).Count;
            return respond;
        }


        /// <summary>
        /// Obtains all the requests created between the two given dates
        /// </summary>
        /// <param name="pDate1">First date</param>
        /// <param name="pDate2">Second date</param>
        /// <returns>Lists of all the matching requests</returns>
        [Route(REQUEST_URL + "/dateReq/{pDate1}/{pDate2}")]
        [HttpGet]
        public ActionResult<List<Request>> RequestsDate([FromRoute] DateTime pDate1, [FromRoute] DateTime pDate2)
        {
            return _requestService.SearchDateRange(pDate1, pDate2);
        }


        /// <summary>
        /// Return all the request of a client filtered by a range of dates
        /// </summary>
        /// <param name="pClient"></param>
        /// <param name="pDate1"></param>
        /// <param name="pDate2"></param>
        /// <returns></returns>
        [Route(REQUEST_URL + "/dateClient/{pClient}/{pDate1}/{pDate2}")]
        [HttpGet]
        public ActionResult<List<Request>> SearchRequestDateClient([FromRoute] string pClient, 
            [FromRoute] DateTime pDate1, [FromRoute] DateTime pDate2)
        {
            return _requestService.SearchDateRangeClient(pClient, pDate1, pDate2);
        }


        /// <summary>
        /// Obtains the number of requests that match the specified state
        /// </summary>
        /// <param name="pState">Integer that represent the state</param>
        /// <returns>List of requests</returns>
        [Route(REQUEST_URL + "/state/{pState}")]
        [HttpGet]
        public ActionResult<JObject> SearchRequestState(int pState)
        {
            dynamic respond = new JObject();
            respond.quantity = _requestService.SearchState(pState).Count;
            return respond;
        }


        /// <summary>
        /// Obtains all the requests that match the specified state
        /// </summary>
        /// <param name="pState">Integer that represent the state</param>
        /// <returns>List of requests</returns>
        [Route(REQUEST_URL + "/stateReq/{pState}")]
        [HttpGet]
        public ActionResult<List<Request>> RequestState(int pState)
        {
            return _requestService.SearchState(pState);
        }


        /// <summary>
        /// Obtains all the requests that match the specified state and client id
        /// </summary>
        /// <param name="pState">Integer that represent the state</param>
        /// <returns>List of requests</returns>
        [Route(REQUEST_URL + "/stateClient/{pClient}/{pState}")]
        [HttpGet]
        public ActionResult<List<Request>> SearchRequestStateClient([FromRoute] string pClient, [FromRoute] int pState)
        {
            return _requestService.SearchStateClient(pClient, pState);
 
        }


        /// <summary>
        /// Obtains the number of requests of books of the specified theme
        /// </summary>
        /// <param name="pTheme">Theme of the book</param>
        /// <returns></returns>
        [Route(REQUEST_URL + "/bookTheme/{pTheme}")]
        [HttpGet]
        public ActionResult<JObject> SearchBookTheme(string pTheme)
        {
            dynamic respond = new JObject();
            respond.quantity = _requestService.SearchBookTheme(pTheme).Count;
            return respond;
        }


        /// <summary>
        /// Obtains all the requests of books of the specified theme
        /// </summary>
        /// <param name="pTheme">Theme of the book</param>
        /// <returns></returns>
        [Route(REQUEST_URL + "/bookThemeReq/{pTheme}")]
        [HttpGet]
        public ActionResult<List<Request>> RequestsBookTheme(string pTheme)
        {
            return _requestService.SearchBookTheme(pTheme);
        }


        /// <summary>
        /// For one client
        /// Obtains all the requests of books of the specified theme
        /// </summary>
        /// <param name="pTheme">Theme of the book</param>
        /// <returns></returns>
        [Route(REQUEST_URL + "/bookThemeClient/{pClient}/{pTheme}")]
        [HttpGet]
        public ActionResult<List<Request>> SearchBookThemeClient([FromRoute] string pClient, [FromRoute] string pTheme)
        {
            return _requestService.SearchBookThemeClient(pClient, pTheme);
        }


        /// <summary>
        /// Obtains the number of requests made by the specified client
        /// </summary>
        /// <param name="pClient">ID of the client</param>
        /// <returns></returns>
        [Route(REQUEST_URL + "/client/{pClient}")]
        [HttpGet]
        public ActionResult<JObject> SearchClient(string pClient)
        {
            dynamic respond = new JObject();
            respond.quantity = _requestService.SearchClient(pClient).Count;
            return respond;
        }


        /// <summary>
        /// Obtains all the requests made by the specified client
        /// </summary>
        /// <param name="pClient">ID of the client</param>
        /// <returns></returns>
        [Route(REQUEST_URL + "/clientReq/{pClient}")]
        [HttpGet]
        public ActionResult<List<Request>> RequestsClient(string pClient)
        {
            return _requestService.SearchClient(pClient);
        }


        /// <summary>
        /// Return all the requests made in the specified library
        /// </summary>
        /// <param name="pLibrary">ID of the library</param>
        /// <returns></returns>
        [Route(REQUEST_URL + "/library/{pLibrary}")]
        [HttpGet]
        public ActionResult<List<Request>> SearchLibrary(string pLibrary)
        {
            return _requestService.SearchLibrary(pLibrary);
        }


        /// <summary>
        /// Return the request with oldest request date
        /// </summary>
        /// <returns></returns>
        [Route(REQUEST_URL + "/takeRequest")]
        [HttpGet]
        public ActionResult<Request> TakeRequest()
        {
            Request req = _requestService.TakeRequest();
            if (req == null)
                return NotFound();

            return req;
        }



    }
}