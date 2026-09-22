using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Interface.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// Forma15
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class Forma15Controller : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly IForma15Business _business;

        /// <summary>
        /// Initializes a new instance of the <see cref="Forma15Controller"/> class.
        /// </summary>
        /// <param name="business">The business.</param>
        public Forma15Controller(IForma15Business business)
        {
            _business = business;
        }

        /// <summary>
        /// Forma15. Uploads the file.
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
                    var result = await _business.LoadFile(_date, fileStream);
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
        /// Creates the form.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost("registerForm")]
        public async Task<ActionResult> CreateForm(DateTime date, ResponseForma15 data)
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
    }
}