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
    /// Forma21Controller
    /// </summary>
    /// <seealso cref="Backend.Formas.API.Controllers.Base.BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class Forma21Controller : BaseController
    {
        private readonly IForma21Business _forma21Business;
        /// <summary>
        /// Initializes a new instance of the <see cref="Forma21Controller"/> class.
        /// </summary>
        /// <param name="forma21Business">The forma21 business.</param>
        public Forma21Controller(IForma21Business forma21Business)
        {
            _forma21Business = forma21Business;
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
                var json = fileJSON.Deserialize<List<RequestForma21>>();
                if (file != null)
                {
                    var fileStream = file.OpenReadStream();
                    string fileName = file.FileName;
                    var result = await _forma21Business.LoadFile(json, fileStream, fileName, getUserId: GetUserId);
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
        public async Task<ActionResult> CreateForm(List<ResponseForma21> data)
        {
            var result = await _forma21Business.Create(data, GetUserId, GetNames);
            return StatusCode(result.Code, result);
        }
    }
}
