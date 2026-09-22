using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Entities.Models;
using Backend.Formas.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// FormaCuadro1AController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FormaCuadro1AController : Base.BaseController
    {
        private readonly IFormaCuadro1ABusiness cuadro1ABusiness;

        /// <summary>
        /// Constructor Cuadro 1
        /// </summary>
        /// <param name="_cuadro1ABusiness"></param>
        public FormaCuadro1AController(IFormaCuadro1ABusiness _cuadro1ABusiness)
        {
            cuadro1ABusiness = _cuadro1ABusiness;
        }
        /// <summary>
        /// Validar Estructura Forma Cuadro 1A.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="forma"></param>
        /// <param name="fileJSON"></param>
        /// <returns></returns>
        [HttpPost("ValidateFormaCuadro1A")]
        public async Task<ActionResult> ValidateFormaCuadro1A( DateTime date,  string forma,  string fileJSON)
        {
            var json = fileJSON.Deserialize<List<FormaCuadro1A>>();

            var result = await cuadro1ABusiness.ValidateFormaCuadro1A(date, forma, fileJSON, GetUserId);
            return StatusCode(result.Code, result);
        }
        /// <summary>
        /// Inserts the forma cuadro1 a.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="date">The date.</param>
        /// <param name="forma">The forma.</param>
        /// <param name="fileJSON">The file json.</param>
        /// <returns></returns>
        [HttpPost("InsertFormaCuadro1A")]
        public async Task<ActionResult> InsertFormaCuadro1A( IFormFile file,  DateTime date,  string forma,  string fileJSON)
        {
            if (file != null)
            {
                var fileStream = file.OpenReadStream();
                var result = await cuadro1ABusiness.InsertFormaCuadro1A(fileStream, date, forma, fileJSON, GetUserId, GetNames, GetEmail, file.FileName);
                return StatusCode(result.Code, result);
            }
            else
            {
                return StatusCode(506, "el archivo esta vacio.");
            }
        }
    }
}
