using Backend.Formas.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IReporteBusiness
    {
        Task<ResponseBase<List<DetalleReporte>>> GenerarReporteFormas(string forma, DateTime date);
        Task<ResponseBase<List<DetalleReporteForma>>> GenerarReporteFormasDetalle(string forma, DateTime date, string idOperador, string idCampo, string idContrato);
    }
}
