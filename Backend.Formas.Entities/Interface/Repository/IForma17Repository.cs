using Backend.Formas.Entities.Models;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma17Repository
    {
        Task<DataTable> guardarCabecera(string form_id, InfoForma17CR cab, string usuario);

        Task<DataTable> guardarDetalle(string form_id, string id_detalle, dataForma17CR det, string usuario);
    }
}
