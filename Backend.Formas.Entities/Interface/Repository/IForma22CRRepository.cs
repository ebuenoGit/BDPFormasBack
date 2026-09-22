using Backend.Formas.Entities.Models;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma22CRRepository
    {
        Task<DataTable> guardarCabecera(string form_id, infoForm22CR cab, string usuario);

        Task<DataTable> guardarDetalle(string form_id, string id_detalle, dataForm22CR det, string usuario);
    }
}
