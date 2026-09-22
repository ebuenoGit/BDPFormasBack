using Backend.Formas.Entities.Models;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma15CRRepository
    {
        Task<DataTable> guardarCabecera(string form_id, infoForma15CR cab, string usuario);
        Task<DataTable> guardarDetalle(string form_id, string id_detalle, dataForma15CR det, string usuario);
    }
}
