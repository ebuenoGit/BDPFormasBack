using Backend.Formas.Entities.DTO.Dominios;
using Backend.Formas.Entities.Responses;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Formas.Entities.Interface.Business
{
    public interface IForma23Business
    {
        Task<ResponseBase<RequestFormas<RequestForma23>>> LoadFile(List<RequestForma23> data, Stream _file, string fileName, string getUserId);
        Task<ResponseBase<dynamic>> Create(List<ResponseForma23> data, string GetUserId, string userName);
    }
}
