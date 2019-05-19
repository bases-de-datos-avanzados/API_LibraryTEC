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
    public class PromosController : ControllerBase
    {
        private const string PROMO_URL = "api/promos";
        private readonly PromoService _promoService;

        /// <summary>
        /// Class constructor 
        /// </summary>
        /// <param name="pPromoService"></param>
        public PromosController(PromoService pPromoService)
        {
            _promoService = pPromoService;
        }


        /// <summary>
        /// Return a list with the data of all the promos inside the database
        /// </summary>
        /// <returns></returns>
        [Route(PROMO_URL + "/all")]
        [HttpGet]
        public ActionResult<List<Promo>> Get()
        {
            return _promoService.Get();
        }


        /// <summary>
        /// Return the data of a single promo by its Id
        /// </summary>
        /// <param name="pId">Id of the promo</param>
        /// <returns></returns>
        [Route(PROMO_URL + "/get/{pId}")]
        [HttpGet]
        public ActionResult<Promo> Get(string pId)
        {
            var promo = _promoService.Get(pId);

            if (promo == null)
                return NotFound();

            return promo;
        }


        /// <summary>
        /// Receives the data of a new promo, to insert it in the database
        /// </summary>
        /// <param name="pPromo">Model class with the data of the new promo</param>
        /// <returns>Http status code: 201 if successful, 409 if there is an error</returns>
        [Route(PROMO_URL + "/create")]
        [HttpPost]
        public IActionResult Create(Promo pPromo)
        {
            int result = _promoService.Create(pPromo);

            if (result < 0)
                return StatusCode(StatusCodes.Status409Conflict);

            return StatusCode(StatusCodes.Status201Created);
        }


        /// <summary>
        /// Receives the updated data of a promo, to update it in the database
        /// </summary>
        /// <param name="pId">Id of the promo</param>
        /// <param name="pPromo">Model class with the updated data</param>
        /// <returns>Http status code: 200 if successfull,
        /// 409 if there is an error during the updating process, 
        /// 404 if the promo is not found the database</returns>
        [Route(PROMO_URL + "/update/{pId}")]
        [HttpPost]
        public IActionResult Update(string pId, Promo pPromo)
        {
            if (_promoService.Get(pId) == null)
                return NotFound();

            if (_promoService.Update(pId, pPromo) < 0)
                return StatusCode(StatusCodes.Status409Conflict);

            return StatusCode(StatusCodes.Status200OK);
        }


        /// <summary>
        /// Deletes a promo from the database, searching it by its Id(_id)
        /// </summary>
        /// <param name="pId">Id of the promo</param>
        /// <returns>Http status code: 200 if successfull, 
        /// 404 if the promo is not found the database</returns>
        [Route(PROMO_URL + "/delete/{pId}")]
        [HttpGet]
        public IActionResult Delete(string pId)
        {
            if (_promoService.Get(pId) == null)
                return NotFound();

            _promoService.Remove(pId);
            return StatusCode(StatusCodes.Status200OK);
        }



    }
}