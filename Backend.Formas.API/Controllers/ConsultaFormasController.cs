using Backend.Formas.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Formas.API.Controllers
{
    /// <summary>
    /// ConsultaFormasController
    /// </summary>
    /// <seealso cref="Backend.Formas.API.Controllers.Base.BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaFormasController : Base.BaseController//Base.BaseController
    {
        /// <summary>
        /// The business
        /// </summary>
        private readonly Entities.Interface.Business.IConsultaFormasBusiness Business;
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsultaFormasController"/> class.
        /// </summary>
        /// <param name="business">The business.</param>
        public ConsultaFormasController(Entities.Interface.Business.IConsultaFormasBusiness business)
        {
            Business = business;
        }
        /// <summary>
        /// Gets the consulta formas versión 23032022.
        /// </summary>
        /// <param name="anio">selected year o current year.</param>
        /// <param name="forma">number forma o identification.</param>
        /// <param name="mes">selected month.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFormas")]
        public async Task<ActionResult> GetConsultaFormas(int anio, int forma, int mes)
        {
            DetalleForma data = new DetalleForma()
            {
                I_ANNO = anio,
                I_MES = mes,
                I_FORMA = forma,
                I_ID_OPERADOR = null,
                I_ID_USUARIO = GetUserId,
                I_ID_CONTRATO = null
            };

            var result = await Business.GetConsultaFormas(data);
            return StatusCode(result.Code, result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="forma"></param>
        /// <param name="mes"></param>
        /// <param name="operador"></param>
        /// <param name="campo"></param>
        /// <param name="contrato"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFormasOperador")]
        public async Task<ActionResult> GetFormasOperador(int anio, int forma, int mes, string operador, string campo = null, string contrato = null)
        {
            DetalleForma data = new DetalleForma()
            {
                I_ANNO = anio,
                I_MES = mes,
                I_FORMA = forma,
                I_ID_OPERADOR = operador,
                I_ID_CAMPO = campo,                    // MCG 1-04-2022  Mejora para identificar el Nombre del campo asociado a la Forma Oficial
                I_ID_CONTRATO = contrato               // MCG 1-19-2022  Mejora para identificar el Contrato de la forma 

            };
            var result = await Business.GetConsultaFormasOperador(data);
            return StatusCode(result.Code, result);
        }
    }
}
