using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public interface IForma22Aprobacion
    {
        Task<Aprobacion<Entities.DTOI.Detalle22Aprobacion>> JsonFormat(Aprobacioncarga data, string getUserid);
    }
}
