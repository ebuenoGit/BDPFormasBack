using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IForma21Business
    {
        Task<ResponseBase<RequestFormas<RequestForma21>>> LoadFile(List<RequestForma21> data, Stream _file, string fileName, string getUserId);
        Task<ResponseBase<dynamic>> Create(List<ResponseForma21> data, string GetUserId, string userName);
    }
}
