using Backend.Formas.Entities.Interface.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// Controllers 17 CR
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Forma17CRController : Base.BaseController
    {
        private readonly IFormas17Business formas17;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_formas17"></param>
        public Forma17CRController(IFormas17Business _formas17)
        {
            formas17 = _formas17;
        }

        /// <summary>
        /// Validador forma17
        /// </summary>
        /// <param name="date"></param>
        /// <param name="forma"></param>
        /// <param name="fileJSON"></param>
        /// <returns></returns>
        [HttpPost("ValidateForm17CR")]
        public async Task<ActionResult> ValidateForm17CR( DateTime date,  string forma,  string fileJSON)
        {
            var result = await formas17.ValidateForm17CR(date, forma, fileJSON, GetUserId);
            return StatusCode(result.Code, result);
        }

        /// <summary>
        /// Insertar Forma 17
        /// </summary>
        /// <param name="file"></param>
        /// <param name="date"></param>
        /// <param name="forma"></param>
        /// <param name="fileJSON"></param>
        /// <returns></returns>
        [HttpPost("InsertForma17")]
        public async Task<ActionResult> InsertForma17( IFormFile file,  DateTime date,  string forma,  string fileJSON)
        {
            if (file != null)
            {
                var fileStream = file.OpenReadStream();
                var result = await formas17.InsertForma17(fileStream, date, forma, fileJSON, GetUserId, GetNames, GetEmail, file.FileName);
                return StatusCode(result.Code, result);
            }
            else
            {
                return StatusCode(506, "el archivo esta vacio.");
            }
        }
    }
}
