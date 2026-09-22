using Backend.Formas.Entities.DAO;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IForma20Repository
    {
        Task<DataTable> InsertarCabeceraForma20(Forma20Archivo forArc);
        Task<DataTable> InsertarFileDataForma20(Forma20Datafile fileData);
        Task<bool> InsertarDataDataForma20(Forma20Data datos);

    }
}
