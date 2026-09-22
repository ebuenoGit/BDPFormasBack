using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Repository
{
    public interface IFormaValidate
    {
        Task<List<dynamic>> ValidaOperador(string nombre);
        Task<List<dynamic>> ValidaContrato(string nombre);
        Task<List<dynamic>> ValidaCampo(string campo);
        Task<List<dynamic>> ValidaBloque(string nombre);
        Task<List<dynamic>> ValidaPozo(string nombre);
        Task<List<dynamic>> ValidaYacimiento(string nombre);
        Task<List<dynamic>> ValidaFormacion(string nombre);
        Task<List<dynamic>> ValidaModalidad(string nombre);
        Task<List<dynamic>> ValidaPozoYacimiento(string nombre);
        Task<List<dynamic>> ValidaFacilidad(string nombre);
        Task<List<dynamic>> ValidaCodigoDane(string nombre);
        Task<List<dynamic>> ValidaProduccion(string nombre);
        Task<List<dynamic>> ValidaForma9Producion(List<dynamic> data);
        Task<List<dynamic>> ValidaCampoContratoOperador(string operador, string campo, string contrato);
        Task<List<dynamic>> ValidaCompaniaCampo(string compania, string campo);
        Task<List<dynamic>> ValidaCompaniaCampoEstado(string compania, string campo,string fecha);
        Task<dynamic> ConsultarOficilizacion(string fechaPeriodo, string idForma);
        Task<dynamic> Oficialializar(string json);

        Task<List<dynamic>> ValidarEstadoPozo(string estadopozo);
        Task<List<dynamic>> ValidarMethodoProducion(string method);
    }
}