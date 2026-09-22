using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IFormas4Business
    {
        Task<ResponseBase<RequestFormas<ResponseForma4>>> LoadFile(List<RequestForma4> date, Stream _file, string fileName, string getUserId);
        Task<ResponseBase<dynamic>> Create(List<ResponseForma4> data, string GetUserId, string userName);
    }
}