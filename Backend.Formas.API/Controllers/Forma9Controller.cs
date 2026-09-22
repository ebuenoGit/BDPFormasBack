using Backend.Formas.API.Controllers.Base;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Interface.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// Forma9
    /// </summary>
    /// <seealso cref="BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class Forma9Controller : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly IFormas9Business _business;

        /// <summary>
        /// Initializes a new instance of the <see cref="Forma9Controller"/> class.
        /// </summary>
        /// <param name="business">The business.</param>
        public Forma9Controller(IFormas9Business business)
        {
            _business = business;
        }

        /// <summary>
        /// Forma9. Uploads the file.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="files">The files.</param>
        /// <returns></returns>
        [HttpPost("uploadFile")]
        public async Task<ActionResult> UploadFile(DateTime date, IFormFile files)
        {
            try
            {
                if (files != null)
                {
                    var fileStream = files.OpenReadStream();

                    var result = await _business.UploadFile(date, fileStream, files.FileName, GetUserId);
                    return StatusCode(200, result);
                }

                return StatusCode(506, "error");
            }
            catch (Exception e)
            {
                return StatusCode(506, e);
            }
        }

        /// <summary>
        /// Forma9. Gets all.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getAll")]
        public ActionResult GetAll()
        {
            try
            {
                var all = new List<string>
                {
                    "testo"
                };
                //var result = await _business.UploadFile();
                return StatusCode(200, all);
            }
            catch (Exception e)
            {
                return StatusCode(506, e);
            }
        }

        /// <summary>
        /// Forma9. Creates the form.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost("registerForm")]
        public async Task<ActionResult> CreateForm(DateTime date, RequestCreateForma9 data)
        {
            try
            {
                var result = await _business.Create(data, date, GetUserId, GetNames);
                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                return StatusCode(506, e);
            }
        }
    }
}