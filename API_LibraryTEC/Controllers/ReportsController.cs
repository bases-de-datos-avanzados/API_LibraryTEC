using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using API_LibraryTEC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_LibraryTEC.Controllers
{
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private const string REPORT_URL = "api/reports";
        private readonly ReportService _reportService;

        /// <summary>
        /// Class constructor 
        /// </summary>
        /// <param name="pReportService"></param>
        public ReportsController(ReportService pReportService)
        {
            _reportService = pReportService;
        }


        /// <summary>
        /// Returns the average price and amount of requested books of all themes
        /// </summary>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportAdmin1")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportAdmin1()
        {
            return _reportService.ReportAdmin1();
        }


        /// <summary>
        /// Return the range of requests of the clients
        /// </summary>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportAdmin2")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportAdmin2()
        {
            return _reportService.ReportAdmin2();
        }


        /// <summary>
        /// Return the 5 most sell books
        /// </summary>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportAdmin3")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportAdmin3()
        {
            return _reportService.ReportAdmin3();
        }


        /// <summary>
        /// Returns the top 3 clients with most requests
        /// </summary>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportAdmin4")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportAdmin4()
        {
            return _reportService.ReportAdmin4();
        }


        /// <summary>
        /// For an specific library
        /// Returns the average price and amount of requested books of all themes
        /// </summary>
        /// <param name="pLibrary"></param>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportManager1/{pLibrary}")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportManager1(string pLibrary)
        {
            return _reportService.ReportManager1(pLibrary);
        }


        /// <summary>
        /// For an specific library
        /// Range of requests made by clients
        /// </summary>
        /// <param name="pLibrary"></param>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportManager2/{pLibrary}")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportManager2(string pLibrary)
        {
            return _reportService.ReportManager2(pLibrary);
        }


        /// <summary>
        /// Top most buyed books
        /// </summary>
        /// <param name="pLibrary">Library id</param>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportManager3/{pLibrary}")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportManager3(string pLibrary)
        {
            return _reportService.ReportManager3(pLibrary);
        }


        /// <summary>
        /// Amount of requests made by a client in a library
        /// </summary>
        /// <param name="pClient">Client id</param>
        /// <param name="pLibrary">Library id</param>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportManager4_1/{pClient}/{pLibrary}")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportManager4_1([FromRoute] string pClient, [FromRoute] string pLibrary)
        {
            return _reportService.ReportManager4_1(pClient, pLibrary);
        }


        /// <summary>
        /// Return the amout of books registers in one library, between the two specified libraries
        /// </summary>
        /// <param name="pLibrary">Library id</param>
        /// <param name="pDate1">First date</param>
        /// <param name="pDate2">Second date</param>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportManager4_2/{pLibrary}/{pDate1}/{pDate2}")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportManager4_2([FromRoute] string pLibrary, [FromRoute] DateTime pDate1, 
            [FromRoute] DateTime pDate2)
        {
            return _reportService.ReportManager4_2(pLibrary, pDate1, pDate2);
        }


        /// <summary>
        /// Return the amount of books of a specific theme, requested in one library
        /// </summary>
        /// <param name="pLibrary">Library id</param>
        /// <param name="pTheme">Theme of the book</param>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportManager4_3/{pLibrary}/{pTheme}")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportManager4_3([FromRoute] string pLibrary, [FromRoute] string pTheme)
        {
            return _reportService.ReportManager4_3(pLibrary, pTheme);
        }


        /// <summary>
        /// Return the amount of requested books in one library that has the specified state
        /// </summary>
        /// <param name="pLibrary">Library id</param>
        /// <param name="pState">State of the request</param>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportManager4_4/{pLibrary}/{pState}")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportManager4_4([FromRoute] string pLibrary, [FromRoute] int pState)
        {
            return _reportService.ReportManager4_4(pLibrary, pState);
        }


        /// <summary>
        /// Top 3 users of users with most books buyed
        /// </summary>
        /// <param name="pLibrary">Library id</param>
        /// <returns></returns>
        [Route(REPORT_URL + "/reportManager4_5/{pLibrary}")]
        [HttpGet]
        public ActionResult<List<ExpandoObject>> ReportManager4_5(string pLibrary)
        {
            return _reportService.ReportManager4_5(pLibrary);
        }




    }
}