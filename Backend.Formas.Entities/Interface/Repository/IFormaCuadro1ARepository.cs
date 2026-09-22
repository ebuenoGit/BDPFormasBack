using Backend.Formas.Entities.Models;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IFormaCuadro1ARepository
    {
        Task<DataTable> guardarCabecera(cabeCuadro1 cab);
        Task<DataTable> guardarDetalle(detaCuadro1 det);
        Task<DataTable> guardarTotal(string id_cabecera, string forma_id, totalFormaCuadro1A tot, string GetUserId);
    }
}
