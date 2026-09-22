using Backend.Formas.Entities.Models;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma30Repository
    {
        Task<bool> CreaForma30(ParametrosCreaForma30 id);
        Task<bool> PvsCrea(ParametrosPvsCrea id, string p_code);
        Task<bool> PvsoCrea(ParametrosPvsoCrea id, string p_code);
    }
}
