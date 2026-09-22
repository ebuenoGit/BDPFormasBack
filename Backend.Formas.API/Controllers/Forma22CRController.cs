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

    public class Forma22CRController : Base.BaseController
    {
        /// <summary>
        /// The forma22 cr
        /// </summary>
        private readonly IForma22CRBusiness forma22CR;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_forma22CR"></param>
        public Forma22CRController(IForma22CRBusiness _forma22CR)
        {
            forma22CR = _forma22CR;
        }
        /// <summary>
        /// Validar Forma22CR
        /// </summary>
        /// <param name="date"></param>
        /// <param name="forma"></param>
        /// <param name="fileJSON"></param>
        /// <returns></returns>
        [HttpPost("ValidateForma22CR")]
        public async Task<ActionResult> ValidateForma22CR( DateTime date,  string forma,  string fileJSON)
        {
            var result = await forma22CR.ValidateForma22CR(date, forma, fileJSON, GetUserId);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Insert Forma 22CR
        /// </summary>
        /// <param name="file"></param>
        /// <param name="date"></param>
        /// <param name="forma"></param>
        /// <param name="fileJSON"></param>
        /// <returns></returns>
        [HttpPost("InsertForma22CR")]
        public async Task<ActionResult> InsertForma22CR( IFormFile file,  DateTime date,  string forma,  string fileJSON)
        {
            if (file != null)
            {
                var fileStream = file.OpenReadStream();
                var result = await forma22CR.InsertForma22CR(fileStream, date, forma, fileJSON, GetUserId, GetNames, GetEmail, file.FileName);
                return StatusCode(result.Code, result);
            }
            else
            {
                return StatusCode(506, "el archivo esta vacio.");
            }
        }
    }
}
