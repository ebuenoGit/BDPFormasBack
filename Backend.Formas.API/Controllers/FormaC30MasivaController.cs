using Backend.Formas.API.Controllers.Base;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Interface.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// FarmaC30Masiva
    /// MCG  --- > Mejoras BDP ++ 
    /// 2022 - Abril - 29
    /// Carga de archivo Excel Formato predefindo por ecopetrol de Forma 30 que incluye el pden_id PDEN_LAND_RIGHT 
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FormaC30MasivaController : Base.BaseController
    {
        /// <summary>
        /// The Business 
        /// </summary>
        private readonly IFormasC30MasivaBusiness _business;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormaC30MasivaController"/> class.
        /// </summary>
        /// <param name="business"></param>
        public FormaC30MasivaController(IFormasC30MasivaBusiness business)
        {
            _business = business;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("uploadFile")]
        public async Task<ActionResult> UploadFile(DateTime date, IFormFile files)
        {
            var nAnio = date.Year;
            var nMes = date.Month;
            try
            {
                if (files != null)
                {
                    var dDate = new DateTime(nAnio, nMes, DateTime.DaysInMonth(nAnio, nMes));
                    var fileStream = files.OpenReadStream();

                    var result = await _business.UploadFile(dDate, fileStream, files.FileName, GetUserId);
                    return StatusCode(200, result);
                }

                return StatusCode(506, "error");
            }
            catch (Exception e)
            {
                return StatusCode(506, e);
            }
        }

        //actualizaC30
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("actualizaC30")]
        public async Task<ActionResult> CreateForm(DateTime date, RequestCreateFormaC30Masiva data)
        {
            int nCodes = 0;
            string cRespuestas = "";
            try
            {
                //registerForm 
                //Detail9Multiple
                var result = await _business.GuardarFormaC30PPDM(data.DetaSaveBDP);

                int StartIndexSql = result.IndexOf("Sql_Respuesta:", 0);
                int StartIndexBsp = result.IndexOf("BDP_Respuesta:", 0);
                string crSql = "0";
                if (StartIndexSql >= 0)
                {
                    StartIndexSql += 14;
                    crSql = result.Substring(StartIndexSql, 1);
                }
                string crBdp = "0";  // 0 es Sin errores  // 1 con errores de datos  // 2 con Error de Oracle
                if (StartIndexBsp >= 0)
                {
                    StartIndexBsp += 14;
                    crBdp = result.Substring(StartIndexBsp, 1);
                }
                if (crSql == "2" || crBdp == "2")
                {
                    nCodes = 500;
                    cRespuestas = _business.StringBetween(result, "BDP_Respuesta:2", ".;");
                }
                if (crSql == "1" && crBdp == "0")
                {
                    nCodes = 200;
                    cRespuestas = _business.StringBetween(result, "Sql_Respuesta:1;", "BDP_Respuesta:0;");
                }
                if (crSql == "0" && crBdp == "1")
                {
                    nCodes = 201;
                    cRespuestas = _business.StringBetween(result, "BDP_Respuesta:0;", ".;");
                }
                if (crSql == "0" && crBdp == "0")
                {
                    nCodes = 200;
                    cRespuestas = result;
                }
                if (crSql == "1" && crBdp == "1")
                {
                    nCodes = 200;
                    cRespuestas = _business.StringBetween(result, "BDP_Respuesta:1;", ".;");
                }
                return StatusCode(nCodes, cRespuestas);
            }
            catch (Exception e)
            {
                return StatusCode(506, e);
            }
        }

        /// <summary>
        /// consultaFormC30Masiva 
        /// </summary>
        /// <param name="nAnio"></param>
        /// <param name="nMes"></param>
        /// <param name="nTodo"></param>
        /// <returns></returns>
        [HttpGet("consultaFormC30Masiva")]
        public async Task<ActionResult> ConsultarC30(int nAnio, int nMes, int nTodo)
        {
            try
            {
                var result = await _business.consultaformaC30(nAnio, nMes, nTodo);
                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                return StatusCode(506, e);
            }
        }
      
    }


}
