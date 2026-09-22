using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Interface.Business;
using Backend.Formas.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// Forma20
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]

    public class Forma20Controller : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly IForma20Business _business;

        /// <summary>
        /// Initializes a new instance of the <see cref="Forma20Controller"/> class.
        /// </summary>
        /// <param name="business">The business.</param>
        public Forma20Controller(IForma20Business business)
        {
            _business = business;
        }
        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="fileJSON">The file json.</param>
        /// <returns></returns>
        [HttpPost("uploadFile")]
        public async Task<ActionResult> UploadFile( IFormFile file,  string fileJSON)
        {
            try
            {
                var json = fileJSON.Deserialize<List<Forma20Request>>();
                if (file != null)
                {
                    var fileStream = file.OpenReadStream();
                    string fileName = file.FileName;
                    var result = await _business.LoadFile(json, fileStream, fileName, getUserId: GetUserId);
                    return StatusCode(result.Code, result);
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
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost("registerForm")]
        public async Task<ActionResult> CreateForm(List<Forma20Request> data)
        {
            var result = await _business.Create(data, GetUserId, GetNames);
            return StatusCode(result.Code, result);
        }


    }
}
