using Backend.Formas.API.Controllers.Base;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Interface.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// forma9Masiva
    /// MCG  --- > Mejoras BDP ++ 
    /// 2022 - Feb - 15
    /// Carga de archivo Excel Formato predefindo por ecopetrol de la forma 9 que incluye el pden_id por pozo formacion 
    /// 
    /// </summary>
    /// <seealso cref="BaseController" />

    [Route("api/[controller]")]
    [ApiController]
    public class Forma9MasivaController : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly IFormas9MasivaBusiness _business;

        /// <summary>
        /// Initializes a new instance of the <see cref="Forma9MasivaController"/> class.
        /// </summary>
        /// <param name="business">The business.</param>
        public Forma9MasivaController(IFormas9MasivaBusiness business)
        {
            _business = business;
        }

        /// <summary>
        /// forma9Masiva. Uploads the file.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="files">The files.</param>
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

        /// <summary>
        /// forma9Masiva. Creates the form.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [HttpPost("registerForm")]
        public async Task<ActionResult> CreateForm(DateTime date, RequestCreateForma9Masiva data)
        {
            int nCodes = 0;
            string cRespuestas = "";
            try
            {
                //registerForm 
                //Detail9Multiple
                var result = await _business.GuardarFormaPPDM(data.DetaSaveBDP);

                int StartIndexSql = result.IndexOf("Sql_Respuesta:", 0) ;
                int StartIndexBsp = result.IndexOf("BDP_Respuesta:", 0) ;

                // crSql 0 => OK                        2 => ERROR SQL
                // crBdp 0 => OK 1 => OK CON MENSAJES   2 => ERROR ORACLE 
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
                if (crSql == "0" && crBdp == "0")
                {
                    nCodes = 200;
                    cRespuestas = result;
                }

                if (crSql == "2" && crBdp == "0")
                {
                    nCodes = 200;
                    cRespuestas = _business.StringBetween(result, "Sql_Respuesta:2;", "BDP_Respuesta:0;");
                }

                if (crSql == "0" && crBdp == "1")
                {
                    nCodes = 201;
                    cRespuestas = _business.StringBetween(result, "BDP_Respuesta:1;", ".;");
                }
                if (crSql == "0" && crBdp == "2")
                {
                    nCodes = 201;
                    cRespuestas = _business.StringBetween(result, "BDP_Respuesta:2;", ".;");
                }
                if (crSql == "2" && crBdp == "1")
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
        /// consultaForm9Masiva 
        /// </summary>
        /// <param name="nAnio"></param>
        /// <param name="nMes"></param>
        /// <param name="nTodo"></param>
        /// <returns></returns>
        [HttpGet("consultaForm9Masiva")]
        public async Task<ActionResult> ConsultarF9(int nAnio, int nMes, int nTodo)
        {
            try
            {
                var result = await _business.consultaforma9(nAnio, nMes, nTodo);
                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                return StatusCode(506, e);
            }
        }

        /// <summary>
        /// consultaValidaAcumulaForm9Masiva
        /// </summary>
        /// <param name="nAnio"></param>
        /// <param name="nMes"></param>
        /// <returns></returns>
        [HttpGet("consultaValidaAcumulaForm9Masiva")] 
        public async Task<ActionResult> ConsultaValidarAcumuladoF9(int nAnio, int nMes)
        {
            try
            {
                var result = await _business.consultaValidaAcumlaforma9(nAnio, nMes, GetUserId);
                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                return StatusCode(506, e);
            }
        }
    }
}