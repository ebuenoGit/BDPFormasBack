using Backend.Formas.Entities.Interface.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// Controllers 15 CR
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class Forma15CRController : Base.BaseController
    {
        private readonly IFormas15CRBusiness formas15CR;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_formas15CR"></param>
        public Forma15CRController(IFormas15CRBusiness _formas15CR)
        {
            formas15CR = _formas15CR;
        }

        /// <summary>
        /// Validador forma 15CR
        /// </summary>
        /// <param name="date"></param>
        /// <param name="forma"></param>
        /// <param name="fileJSON"></param>
        /// <returns></returns>
        [HttpPost("ValidateForm15CR")]
        public async Task<ActionResult> ValidateForm15CR( DateTime date,  string forma,  string fileJSON)
        {
            try
            {
                var result = await formas15CR.ValidateForm15CR(date, forma, fileJSON, GetUserId);
                return StatusCode(result.Code, result);
            }
            catch (Exception)
            {
                return StatusCode(200, "No es posible cargar el archivo, revise la estructura y vuelva a intentarlo.");
            }
            
        }

        /// <summary>
        /// Insert Forma 15CR
        /// </summary>
        /// <param name="file"></param>
        /// <param name="date"></param>
        /// <param name="forma"></param>
        /// <param name="fileJSON"></param>
        /// <returns></returns>
        [HttpPost("InsertForma15")]
        public async Task<ActionResult> InsertForma15( IFormFile file,  DateTime date,  string forma,  string fileJSON)
        {
            if (file != null)
            {
                var fileStream = file.OpenReadStream();
                var result = await formas15CR.InsertForma15(fileStream, date, forma, fileJSON, GetUserId, GetNames, GetEmail, file.FileName);
                return StatusCode(result.Code, result);
            }
            else
            {
                return StatusCode(506, "el archivo esta vacio.");
            }
        }
    }
}
