using Backend.Formas.Entities.DAO;
using System.Threading.Tasks;

namespace Backend.Formas.BusinessRules.Aprobaciones
{
    public interface IFormaAprobacion<T>
    {
        Task<T> JsonFormat(Aprobacioncarga data, string getUserid);
    }
}
