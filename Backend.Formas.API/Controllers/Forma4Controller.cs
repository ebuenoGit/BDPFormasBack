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
    /// Forma4
    /// </summary>
    /// <seealso cref="BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class Forma4Controller : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly IFormas4Business _business;

        /// <summary>
        /// Initializes a new instance of the <see cref="Forma4Controller"/> class.
        /// </summary>
        /// <param name="business">The business.</param>
        public Forma4Controller(IFormas4Business business)
        {
            _business = business;
        }

        /// <summary>
        /// uploadFile
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileJSON"></param>
        /// <returns></returns>
        [HttpPost("uploadFile")]
        public async Task<ActionResult> UploadFile( IFormFile file,  string fileJSON)
        {
            try
            {
                var json = fileJSON.Deserialize<List<RequestForma4>>();
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
        /// registerForm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("registerForm")]
        public async Task<ActionResult> CreateForm([FromBody] List<ResponseForma4> data)
        {
            var result = await _business.Create(data, GetUserId, GetNames);
            return StatusCode(result.Code, result);
        }
    }
}