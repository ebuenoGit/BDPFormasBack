using Backend.Formas.API.Controllers.Base;
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
    /// Forma23Controller
    /// </summary>
    /// <seealso cref="Backend.Formas.API.Controllers.Base.BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class Forma23Controller : Base.BaseController
    {
        private readonly IForma23Business _forma23Business;
        /// <summary>
        /// Initializes a new instance of the <see cref="Forma23Controller"/> class.
        /// </summary>
        /// <param name="forma23Business">The forma23 business.</param>
        public Forma23Controller(IForma23Business forma23Business)
        {
            _forma23Business = forma23Business;
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
                var json = fileJSON.Deserialize<List<RequestForma23>>();
                if (file != null)
                {
                    var fileStream = file.OpenReadStream();
                    string fileName = file.FileName;
                    var result = await _forma23Business.LoadFile(json, fileStream, fileName, getUserId: GetUserId);
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
        public async Task<ActionResult> CreateForm(List<ResponseForma23> data)
        {
            var result = await _forma23Business.Create(data, GetUserId, GetNames);
            return StatusCode(result.Code, result);
        }
    }
}
