using System;
using System.Threading.Tasks;
using Backend.Formas.Entities.Interface.Business;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// ReporteController
    /// </summary>
    /// <seealso cref="Backend.Formas.API.Controllers.Base.BaseController" />
    [Route("api/[controller]")]
    public class ReporteController : Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly IReporteBusiness business;
        /// <summary>
        /// Initializes a new instance of the <see cref="ReporteController"/> class.
        /// </summary>
        /// <param name="_bussines">The bussines.</param>
        public ReporteController(IReporteBusiness _bussines)
        {
            business = _bussines;
        }
        /// <summary>
        /// Gets the specified forma.
        /// </summary>
        /// <param name="forma">The forma.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ListarFormarReportadas(string forma, DateTime date)
        {
            var result = await business.GenerarReporteFormas(forma, date);
            return StatusCode(result.Code, result);
        }


         // GET: api/<controller>
         /// <summary>
         /// </summary>
         /// <param name="forma"></param>
         /// <param name="date"></param>
         /// <param name="idOperador"></param>
         /// <param name="idCampo"></param>
         /// <param name="idContrato"></param>
         /// <returns></returns>
        [HttpGet("detalle")]
        public async Task<ActionResult> ObtenerDetalle(string forma, DateTime date,string idOperador, string idCampo, string idContrato)
        {
            var result = await business.GenerarReporteFormasDetalle(forma, date, idOperador, idCampo, idContrato);   // MCG 1-04-2022  Mejora para identificar el Nombre del campo asociado a la Forma Oficial
                                                                                                                     // MCG 1-19-2022  Mejora para identificar el Contrato de la forma 
            return StatusCode(result.Code, result);
        }

    }
}
