using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    /// <summary>
    /// IConsultaFormasBusiness
    /// </summary>
    public interface IConsultaFormasBusiness
    {
        /// <summary>
        /// Gets the consulta formas.
        /// </summary>
        /// <param name="parametros">The parametros.</param>
        /// <returns></returns>
        Task<Responses.ResponseBase<List<ConsultaFormasModel>>> GetConsultaFormas(DetalleForma parametros);
        Task<Entities.Responses.ResponseBase<List<dynamic>>> GetConsultaFormasOperador(DetalleForma parametros);
    }
}
