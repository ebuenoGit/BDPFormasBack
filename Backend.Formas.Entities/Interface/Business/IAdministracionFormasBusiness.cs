using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IAdministracionFormasBusiness
    {
        Task<ResponseBase<IList<AprobacionDTO>>> GetAprobacionFormas(string UserId);
        Task<ResponseBase<List<Aprobacionprecarga>>> GetPrecargasList();
        Task<ResponseBase<dynamic>> generarAprobacion(List<Aprobacioncarga> data, string GetUserId);
        Task<ResponseBase<List<Aprobacionprecarga>>> RegistrarPrecargas(List<Aprobacionprecarga> data, string GetUserId, string GetNames);
        Task<ResponseBase<Aprobacionprecarga>> ActualizarPrecarga(Aprobacionprecarga data, string usuarioId);
    }
}