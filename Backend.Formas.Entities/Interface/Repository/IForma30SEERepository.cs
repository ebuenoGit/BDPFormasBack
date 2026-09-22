using Backend.Formas.Entities.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma30SEERepository
    {
        Task<DataTable> consultarCargueForma9(DateTime mes, string forma);

        DataTable GuardarCabecera(BDConcreteForm Cabecera);

        DataTable GuardarForma30BD(BDForma30 dat);

        DataTable GuardarForma30BD(BDForma30Detail dat);

        DataTable guardarAprobacion(AprobacionCarga pr);

        Task<DataTable> ConsultarCabeceraForma30(Guid? id);
        Task<DataTable> ConsultarCabeceraForma30Reg(Guid? id);

        Task<DataTable> ConsultarCabeceraForma30Det(Guid? id);

        Task<DataTable> ConsultarPermisoCargua(string forma, DateTime fechaForma, string operador, string contrato, string usuario);
        DataTable ConsultarNotificacion(decimal id);

        Task<DataTable> ConsultarEncabezado(decimal id);
        Task<DataTable> ConsultarDetalle(decimal id);

        Task<DataTable> ConsultarpreCargua(string forma, DateTime fecha);
    }
}
