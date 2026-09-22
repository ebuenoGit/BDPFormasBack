using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public interface IC7
    {
        Task<Aprobacion<DetalleC7Aprobacion>> JsonFormat(Aprobacioncarga data, string getUserid);
    }
}
