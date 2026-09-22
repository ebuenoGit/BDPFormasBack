using Backend.Formas.Entities.DAO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IAdministracionFormas
    {
        Task<Guid> GetState(string name);
        Task<List<Formstate>> GetListState();
        Task<IList<Aprobacioncarga>> GetAprobacion(string userId);
        Task<List<Acumulados>> GetAcumulador(Guid id);
        Task<Aprobacioncarga> CreteAprobacion(Aprobacioncarga _data);
        Task<Aprobacioncarga> GetAprobacion(decimal id);
        Task<Aprobacioncarga> FormaAprobada(decimal id);
        Task<List<Aprobacionprecarga>> GetPrecarga(string forma, DateTime fechaforma, string operador, string campo, string contrato);
        Task<List<Aprobacionprecarga>> GetPrecargaF22(string forma, DateTime fechaforma, string operador, string campo, string contrato);
        Task<List<Aprobacionprecarga>> GetPrecarga(string forma, DateTime fechaforma, string operador, string campo);
        Task<List<Aprobacionprecarga>> GetPrecarga(string forma, DateTime fechaforma, string operador, string campo, string contrato, string user);
        Task<Aprobacioncarga> GetaFormAprobacion(string forma, DateTime fechaforma, string operador, string campo, string contrato);

        Task<Aprobacioncarga> GetaFormAprobacion(string forma, DateTime fechaforma, string operador, string campo, string contrato, string user);

        Task<Aprobacioncarga> GetFormAprobacionF22(string forma, DateTime fechaforma, string operador, string campo, string contrato);
        Task<Aprobacioncarga> GetaFormAprobacion(string forma, DateTime fechaforma, string operador, string campo);
        Task<bool> Delete(Guid id);
        Task<bool> CreatePrecarga(List<Aprobacionprecarga> data);
        Task<bool> UpdatePrecarga(Aprobacionprecarga data);
        Task<List<Aprobacionprecarga>> PrecargaList();
    }
}