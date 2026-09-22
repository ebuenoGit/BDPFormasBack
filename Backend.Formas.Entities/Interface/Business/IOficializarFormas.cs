using Backend.Formas.Entities.Models;
using Backend.Formas.Entities.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IOficializarFormas
    {
        Task<ResponseBase<dynamic>> GetOficializarFormas(OficializarModel data,string getUser );
        Task<ResponseBase<dynamic>> ConsultarOficializacion(DateTime fechaOperativa, string idForma);
    }
}
