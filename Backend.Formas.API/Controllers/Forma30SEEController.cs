using Backend.Formas.Entities.Interface.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// Forma20
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]

    public class Forma30SEEController : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly IForma30SEEBusiness _business;

        /// <summary>
        /// Initializes a new instance of the <see cref="Forma30SEEController"/> class.
        /// </summary>
        /// <param name="business">The business.</param>
        public Forma30SEEController(IForma30SEEBusiness business)
        {
            _business = business;
        }
        /// <summary>
        /// Inserts the forma30.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="date">The date.</param>
        /// <param name="forma">The forma.</param>
        /// <param name="fileJSON">The file json.</param>
        /// <returns></returns>
        [HttpPost("ValidarForma30Json")]
        public async Task<ActionResult> InsertForma30( IFormFile file,  DateTime date,  string forma,  string fileJSON)
        {
            var result = await _business.InsertForma30(date, fileJSON, forma, GetUserId);

            return StatusCode(result.Code, result);
        }
        /// <summary>
        /// Validars the json BDP.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="date">The date.</param>
        /// <param name="forma">The forma.</param>
        /// <param name="fileJSON">The file json.</param>
        /// <returns></returns>
        [HttpPost("GuardarArchivoBDP")]
        public async Task<ActionResult> ValidarJsonBDP( IFormFile file,  DateTime date,  string forma,  string fileJSON)
        {
            if (file != null)
            {
                var fileStream = file.OpenReadStream();
                var result = await _business.ValidarJsonBDP(date, fileStream, file.FileName, forma, fileJSON, GetUserId, GetNames, GetEmail);
                return StatusCode(result.Code, result);
            }
            else
            {
                return StatusCode(506, "el archivo esta vacio.");
            }
        }
    }
}
