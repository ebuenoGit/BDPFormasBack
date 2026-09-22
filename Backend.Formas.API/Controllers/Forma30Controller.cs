using Backend.Formas.API.Controllers.Base;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// Forma30
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Forma30Controller : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly IForma30Business _business;

        /// <summary>
        /// Initializes a new instance of the <see cref="Forma30Controller"/> class.
        /// </summary>
        /// <param name="business">The business.</param>
        public Forma30Controller(IForma30Business business)
        {
            _business = business;
        }

        /// <summary>
        /// Forma30. Uploads the file.
        /// </summary>
        /// <param name="_date">The date.</param>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        [HttpPost("uploadFile")]
        public async Task<ActionResult> UploadFile(DateTime _date, IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var fileStream = file.OpenReadStream();
                    var fileName = file.FileName;
                    var result = await _business.LoadFile(_date, fileStream, fileName);
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
        /// Forma30. Gets all.
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
        /// Forma30. Register Form.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost("registerForm")]
        public async Task<ActionResult> CreateForm(DateTime date, dynamic data)
        {
            try
            {
                var result = await _business.Create(data, date);
                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                return StatusCode(506, e);
            }
        }

        /// <summary>
        /// Forma30. Crear Forma 30.
        /// </summary>
        /// <returns></returns>
        [HttpPost("creaForma30")]
        public async Task<ActionResult> CreaForma30([FromBody] ParametrosCreaForma30 id)
        {
            var result = await _business.CreaForma30(id);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Forma30. crea PVS.
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreaPvs")]
        public async Task<ActionResult> PvsCrea([FromBody] ParametrosPvsCrea id)
        {
            string p_code = GetUserId;
            var result = await _business.PvsCrea(id, p_code);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Forma30. crear PVSO.
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreaPvso")]
        public async Task<ActionResult> PvsoCrea([FromBody] ParametrosPvsoCrea id)
        {
            string p_code = GetUserId;
            var result = await _business.PvsoCrea(id, p_code);
            return StatusCode(result.Code, result);
        }

    }
}