using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public interface IC4
    {
        Task<Aprobacion<DetalleForma4>> JsonFormat(Aprobacioncarga data, string getUserid);
    }
}
