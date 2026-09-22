using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public interface IForma23Aprobacion
    {
        Task<Aprobacion<Entities.DTOI.Forma23Aprobacion>> JsonFormat(Aprobacioncarga data, string getUserid);
    }
}
