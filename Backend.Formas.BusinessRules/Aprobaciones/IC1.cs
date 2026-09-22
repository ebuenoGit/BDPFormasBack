using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public interface IC1
    {
        Task<Aprobacion<Cuadro1>> JsonFormat(Aprobacioncarga data, string getUserid);
    }
}
