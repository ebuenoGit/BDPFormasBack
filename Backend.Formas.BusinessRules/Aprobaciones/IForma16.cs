using Backend.Formas.Entities.DAO;
using Backend.Formas.Entities.DTOI;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public interface IForma16
    {
        Task<Aprobacion<Backend.Formas.Entities.DTOI.Forma16>> JsonFormat(Aprobacioncarga data, string getUserid);
    }
}
